using DALTWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace DALTWeb.Areas.Customer.Controllers
{
    [ApiController]
    [Area("Customer")]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly DeepSeekService _deepSeekService;

        public ChatController(DeepSeekService deepSeekService)
        {
            _deepSeekService = deepSeekService;
        }

        [HttpPost("Ask")]
        public async Task<IActionResult> Ask([FromBody] UserMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest(new { reply = "Nội dung trống.", productSuggestions = "" });

            var (reply, suggestionsHtml) = await _deepSeekService.GetReplyFromDeepSeekAsync(dto.Message);
            return Ok(new { reply, productSuggestions = suggestionsHtml });
        }

        public class UserMessageDto
        {
            public string Message { get; set; }
        }
    }

}

