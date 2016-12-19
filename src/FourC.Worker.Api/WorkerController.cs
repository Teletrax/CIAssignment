using System;
using FourC.Worker.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FourC.Worker.Api
{
    [Route("v1/[controller]")]
    public class WorkerController : Controller
    {
        private readonly IQueueService _queueService;
        private readonly ILogger<WorkerController> _logger;

        public WorkerController(IQueueService queueService, ILoggerFactory factory)
        {
            _queueService = queueService;
            _logger = factory.CreateLogger<WorkerController>();
        }

        [HttpPost("")]
        public IActionResult DoWork([FromBody] WorkModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Received request: {0}", input);

            try
            {
                _queueService.Send(input);
                _logger.LogInformation("Message sent: {1}", input);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured processing request:\n {0}", ex);
                return StatusCode(500, new {ex.Message});
            }
            return Ok();
        }
    }
}