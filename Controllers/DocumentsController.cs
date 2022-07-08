using DocumentsExternal.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace DocumentsExternal.Controllers
{
    [ApiController]
    [Route("")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> logger;
        private readonly IAvaxApiClient client;

        public DocumentsController(ILogger<DocumentsController> logger, IAvaxApiClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        [HttpGet("file/{id}")]
        public async Task<ActionResult> GetFile(string id)
        {
            var documentView = await client.GetNotificationFile(Guid.Parse(id));

            if (documentView != null)
            {
                logger.LogInformation("Запрошен файл \"{FileName}\" по идентификатору: {Guid}", documentView.Name, id);
                Response.Headers.Add(HeaderNames.ContentDisposition, "inline; filename=restriction.pdf");
                return File(documentView.Data, "application/pdf");
            }
            else
            {
                logger.LogWarning("Запрошенный файл с id: \'{Id}\' не найден.", id);
                return NotFound(new { Reason = "Не удалось найти файл." });
            }
        }
    }
}