using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace MyServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class scheduleController : ControllerBase
    {
        private readonly Service _service;
        public scheduleController(Service _service)
        {
            this._service = _service;
        }

        [HttpGet]
        public ActionResult GetAllEventSchedule()
        {
            try
            {
                return Ok(_service.getEventsSchedule());
            }
            catch (Exception ex)
            {
                return NotFound("The list is empty");
            }
        }
    }
}
