using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NailApp.Queries.GetUsers;

namespace NailApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMediator Mediator;

        public UsersController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        [Route("api/users")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await Mediator.Send(new GetUsersRequest());

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
