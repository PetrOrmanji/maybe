using EnsureThat;
using Maybe.App.Serivces;
using Maybe.Domain.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maybe.Api.Controllers
{
    [Route("maybe/[controller]")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private readonly TelegramService _telegramService;

        public PublicationController(TelegramService telegramService)
        { 
            EnsureArg.IsNotNull(telegramService, nameof(telegramService));

            _telegramService = telegramService;
        }

        [HttpPost("publicate")]
        public async Task<IActionResult> Publicate(Idea idea)
        {
            try
            {
                EnsureArg.IsNotNull(idea, nameof(idea));

                await _telegramService.MakePublication(idea);

                return Ok("Идея опубликована");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при публикации идеи: {ex.Message}");
            }            
        }

        [HttpPost("notify")]
        public async Task<IActionResult> Notify(string message)
        {
            try
            {
                EnsureArg.IsNotNullOrWhiteSpace(message, nameof(message));

                await _telegramService.MakeNotification(message);

                return Ok("Уведомление отправлено");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при отправке уведомления: {ex.Message}");
            }
        }
    }
}
