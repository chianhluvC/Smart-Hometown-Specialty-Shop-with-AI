using DALTWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace DALTWeb.ViewComponents
{
    public class ChatbotViewComponent: ViewComponent
    {
        private readonly DeepSeekService _deepSeekService;

        public ChatbotViewComponent(DeepSeekService deepSeekService)
        {
            _deepSeekService = deepSeekService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

    }
}
