using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NailApp.Commands.Appointments.CreateAppointment;
using NailApp.Queries.Appointments.GetAvailableAppointment;

namespace NailApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IMediator Mediator;

        public AppointmentController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        [Route("api/appointment/availableDates")]
        [AllowAnonymous]
        public async Task<IActionResult> AvailableDates()
        {
            var response = await Mediator.Send(new GetAvailableAppointmentRequest());

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Route("api/appointment/create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var response = await Mediator.Send(request);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
