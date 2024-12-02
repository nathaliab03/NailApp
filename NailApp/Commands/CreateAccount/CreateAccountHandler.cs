using MediatR;
using Microsoft.AspNetCore.Identity;
using NailApp.Data;
using NailApp.Models;
using System.Net;

namespace NailApp.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, CreateAccountResponse>
    {
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly ILogger<CreateAccountHandler> Logger;

        #region Constructor
        public CreateAccountHandler (RoleManager<IdentityRole> signInManager, UserManager<ApplicationUser> userManager, ILogger<CreateAccountHandler> logger)
        {
            RoleManager = signInManager;
            UserManager = userManager;
            Logger = logger;
        }
        #endregion

        public async Task<CreateAccountResponse> Handle(CreateAccountRequest request,  CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Starting handle request: CreateAccountHandler");
                var existingUser = await UserManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new CreateAccountResponse { IsSuccess = false, Errors = new List<string> { "Email already exists" }, StatusCode = HttpStatusCode.BadRequest };
                }

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.Email,
                    Name = request.Name,
                };

                var result = await UserManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var roleUser = await UserManager.FindByEmailAsync(request.Email);
                    await UserManager.AddToRoleAsync(user, "User");

                    Logger.LogInformation("Finishing handle request: CreateAccountHandler created successfully user: " + request.Email);
                    return new CreateAccountResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK };
                }

                return new CreateAccountResponse { IsSuccess = false, Errors = result.Errors.Select(e => e.Description).ToList(), StatusCode = HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new CreateAccountResponse { IsSuccess = false, Errors = new List<string> { "Erro inesperado: " + ex.Message }, StatusCode = HttpStatusCode.BadRequest };
            }
        }
    }
}
