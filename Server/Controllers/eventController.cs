using MyServer.DTO;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class eventController : ControllerBase
    {
        private readonly Service _service;
        public eventController(Service _service)
        {
            this._service = _service;
        }

        [HttpPost]
        public ActionResult CreateNew([FromBody] EventDTO ev)
        {
            if (ev is null)
            {
                throw new ArgumentNullException(nameof(ev));
            }

            try
            {
                _service.CreateEvent(ev.Name, ev.StartDate, ev.EndDate, ev.MaxRegistrations, ev.Location);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/registration")]

        public ActionResult<List<User>> GetUsersOfEvent(int id)
        {
            try
            {
                return Ok(_service.RetrieveUsersOfEvent(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{id}/registration")]
        public ActionResult RegisterUserToEvent(int id,[FromBody] EventUserDTO ev)
        {
            try
            {
                _service.RegisterUserToEvent(ev.UserRef, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{id}")]
        public ActionResult getEvent(int id)
        {
            try
            {
                return Ok(_service.RetrieveEvent(id));
            }
            catch
            {
                return NotFound("Event Not Found");
            }
        }
        [HttpPut("{id}")]
        public ActionResult updateEventDetails(int id, [FromBody] EventParametersDTO e)
        {
            try
            {
                _service.updateEventDetails(id, e.StartDate, e.EndDate, e.MaxRegistrations, e.Location);
                return Ok();
            }
            catch
            {
                return NotFound("Event Not Found");
            }
        }
        [HttpDelete("{id}")]
        public ActionResult deleteEvent(int id)
        {
            try
            {
                _service.deleteEvent(id);
                return Ok();
            }
            catch
            {
                return NotFound("Event Not Found");
            }
        }

        [HttpGet("{id}/weather")]
        public ActionResult getWeather(int id)
        {
            try
            {
                return Ok(_service.RetrieveWeather(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
    

