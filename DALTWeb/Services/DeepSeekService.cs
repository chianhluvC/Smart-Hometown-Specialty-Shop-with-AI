using DALTWeb.Data;
using DALTWeb.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace DALTWeb.Services
{
    public class DeepSeekService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly ApplicationDbContext _dbContext;

        public DeepSeekService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = configuration["DeepSeek:ApiKey"];
            _apiUrl = configuration["DeepSeek:ApiUrl"];
            _dbContext = dbContext;
        }

        public async Task<(string reply, string productSuggestions)> GetReplyFromDeepSeekAsync(string userMessage)
        {
            string stringModelDeepSeek = "deepseek/deepseek-chat-v3-0324:free";
            string searchKeyword = ExtractSearchKeyword(userMessage);

            var products = await _dbContext.SanPham
                .Where(p =>
                    p.Name.Contains(searchKeyword) ||
                    (p.Description != null && p.Description.Contains(searchKeyword)) ||
                    (p.TheLoai.Name != null && p.TheLoai.Name.Contains(searchKeyword)))
                .ToListAsync();

            string productSuggestions = BuildProductSuggestionsHtml(products);

            
            string bestChoiceExplanation = "";
            if (products.Any())
            {
                var productDescriptions = string.Join("\n", products.Select(p =>
                    $"- Tên: {p.Name}, Mô tả: {p.Description?.Substring(0, 100)}"));

                var analysisPrompt = $@"
                Dưới đây là danh sách đặc sản đã tìm thấy:

                {productDescriptions}

                Hãy chọn ra một đặc sản phù hợp nhất với người dùng, hãy trả lời thân thiện vui vẻ ngắn gọn và giải thích vì sao lựa chọn đó là hợp lý.
                ";

                var analysisRequest = new
                {
                    prompt = analysisPrompt,
                    model = stringModelDeepSeek,
                    max_tokens = 200
                };

                var analysisContent = new StringContent(JsonConvert.SerializeObject(analysisRequest), Encoding.UTF8, "application/json");
                var analysisMessage = new HttpRequestMessage(HttpMethod.Post, _apiUrl) { Content = analysisContent };
                analysisMessage.Headers.Add("Authorization", $"Bearer {_apiKey}");

                var analysisResponse = await _httpClient.SendAsync(analysisMessage);

                if (analysisResponse.IsSuccessStatusCode)
                {
                    var analysisResult = await analysisResponse.Content.ReadAsStringAsync();
                    var analysisJson = JsonConvert.DeserializeObject<dynamic>(analysisResult);
                    bestChoiceExplanation = analysisJson?.choices[0]?.text?.ToString()?.Trim() ?? "";
                }

                
                var bestChoiceReply = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(bestChoiceExplanation))
                {
                    bestChoiceReply.AppendLine();
                    bestChoiceReply.AppendLine("🤔 Gợi ý đặc biệt dành cho bạn:");
                    bestChoiceReply.AppendLine(bestChoiceExplanation);
                }

                return (bestChoiceReply.ToString(), productSuggestions);
            }

            var systemPrompt = @"
            Bạn là một chatbot thông minh, thân thiện và vui vẻ, chuyên hỗ trợ người dùng tìm kiếm đặc sản vùng miền Việt Nam.
            Hãy trả lời bằng tiếng Việt, giọng điệu gần gũi, dễ hiểu như một người bạn hướng dẫn ẩm thực. 
            Luôn cố gắng đưa ra gợi ý đặc sản phù hợp với câu hỏi người dùng một cách sinh động, hấp dẫn.";

            var fullPrompt = $"{systemPrompt}\nNgười dùng: {userMessage}";

            var requestBody = new
            {
                prompt = fullPrompt,
                model = stringModelDeepSeek,
                max_tokens = 400
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _apiUrl) { Content = content };
            requestMessage.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
                return ("Không thể kết nối với DeepSeek, vui lòng thử lại.", "");

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string botReply = result?.choices[0]?.text?.ToString()?.Trim() ?? "Xin lỗi, có lỗi xảy ra. Vui lòng thử lại.";

            var fullReply = new StringBuilder();
            fullReply.AppendLine(botReply);
            fullReply.AppendLine();
            fullReply.AppendLine("🙂 Nếu cần gợi ý đặc sản hãy cho mình biết nhé!");

            return (fullReply.ToString(), "");
        }

        private string ExtractSearchKeyword(string userMessage)
        {
            
            string lowerMessage = userMessage.ToLower();

            
            var patterns = new[]
            {
                @"tìm (đặc sản|món ăn)? (?<keyword>.+)",
                @"gợi ý (món ăn|đặc sản)? (?<keyword>.+)",
                @"đặc sản (ở|tại)? (?<keyword>.+)",
                @"muốn ăn (gì)? (ở|tại)? (?<keyword>.+)",
                @"món ngon (ở|tại)? (?<keyword>.+)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(lowerMessage, pattern, RegexOptions.IgnoreCase);
                if (match.Success && match.Groups["keyword"].Success)
                {
                    var keyword = match.Groups["keyword"].Value.Trim();

                    
                    if (keyword.Length > 50)
                        keyword = keyword.Substring(0, 50);

                    return keyword;
                }
            }

            
            var fallback = lowerMessage.Split(" ").Where(w => w.Length > 3).Take(3);
            return string.Join(" ", fallback);
        }


        private string BuildProductSuggestionsHtml(List<SanPham> products)
        {
            var html = new StringBuilder();

            if (products.Any())
            {
                html.Append("<p>🥳 Đây là những đặc sản mà mình đã tìm được dành cho bạn nè:</p>");

                foreach (var product in products)
                {
                    html.Append($@"
                    <div class='product-suggestion' style='border: 1px solid #e0e0e0; border-radius: 8px; padding: 15px; margin: 10px 0; display: flex; flex-direction: column; max-width: 300px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); transition: transform 0.2s, box-shadow 0.2s;'>
                        <img src='{product.ImageUrl}' alt='{product.Name}' style='width: 100%; height: auto; object-fit: cover; border-radius: 6px;'>
                        <h5 style='margin: 12px 0 8px; font-size: 18px; font-weight: 600; color: #333;'>{product.Name}</h5>
                        <p style='margin: 0 0 15px; font-size: 14px; color: #666; line-height: 1.4;'>{product.Description?.Substring(0, 100)}...</p>
                        <a href='/Customer/Home/Details?sanphamId={product.Id}' class='btn btn-info' style='background-color: #17a2b8; color: white; text-decoration: none; padding: 8px 15px; border-radius: 4px; text-align: center; font-weight: 500; border: none; cursor: pointer; transition: background-color 0.2s; margin-top: auto;'>Xem chi tiết</a>
                    </div>
                ");
                }
            }
            else
            {
                html.Append("<p>😥 Oops, mình chưa tìm thấy đặc sản nào phù hợp với yêu cầu của bạn. Thử nói cách khác xem sao nha!</p>");
            }

            return html.ToString();
        }


    }
}
