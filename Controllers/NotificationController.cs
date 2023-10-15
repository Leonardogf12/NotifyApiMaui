using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using NotifyApiMaui.Models;

namespace NotifyApiMaui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] NotificationDto model)
        {
            SetGoogleCredentials();

            var content = new Message
            {
                Token = model.Token,
                Notification = new Notification
                {
                    Title = model.Title,
                    Body = model.Description,
                    ImageUrl = model.UrlImage,
                }
            };

            string response = string.Empty;

            try
            {
                var date = model.DateTimeOfNotification;

                var delay = date - DateTime.Now;

                if (delay.TotalSeconds <= 0) return BadRequest("Envio da mensagem cancelada devido ao atraso da requisição.");
               
                content.Notification.Body += $" * Detalhes: {date.Hour}:{date.Minute}:{date.Second}";

                var timer = new Timer(async (_) =>
                {
                    response = await FirebaseMessaging.DefaultInstance.SendAsync(content);

                }, null, (int)delay.TotalMilliseconds, Timeout.Infinite);

            }
            catch (Exception e)
            {
                return BadRequest("Erro Generico. Ocorreu uma falha ao tentar salvar a notificação do lembrete. Detalhes: " + e.Message);
            }

            return Ok(response);
        }

        private void SetGoogleCredentials()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("private_key.json")
                });
            }
        }
    }
}
