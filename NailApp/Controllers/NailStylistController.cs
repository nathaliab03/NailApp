using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NailApp.Queries.GetNailStylistById;
using NailApp.Queries.GetNailStylists;

namespace NailApp.Controllers
{
    public class NailStylistController : Controller
    {
        private readonly IMediator Mediator;

        public NailStylistController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        [Route("api/nailstylist")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNailStylists()
        {
            var response = await Mediator.Send(new GetNailStylistsRequest());

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNailStylistById(int id)
        {
            var response = await Mediator.Send(new GetNailStylistByIdRequest { Id = id });

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
