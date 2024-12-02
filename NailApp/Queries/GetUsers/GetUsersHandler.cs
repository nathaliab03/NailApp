using MediatR;
using NailApp.Models;
using NailApp.Services;
using System.Net;

namespace NailApp.Queries.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        #region Properties
        private ILogger<GetUsersHandler> Logger;
        private IQueriesServices Services;
        #endregion

        #region Constructor
        public GetUsersHandler(ILogger<GetUsersHandler> logger, IQueriesServices services)
        {
            Logger = logger;
            Services = services;
        }
        #endregion

        #region Implementation
        public async Task<GetUsersResponse> Handle (GetUsersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Starting handle request: GetNailStylistsHandler");
                var users = await Services.GetAllUsersAsync(cancellationToken);
                users.Select(n => new ApplicationUser
                {
                    Id = n.Id,
                    Name = n.Name,
                }).ToList();

                if (users == null || !users.Any())
                {
                    return new GetUsersResponse
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "No NailStylists found." },
                        StatusCode = HttpStatusCode.NotFound,
                    };
                }

                Logger.LogInformation("Finishing handle request: GetNailStylistsHandler" + users);
                return new GetUsersResponse
                {
                    IsSuccess = true,
                    Content = users,
                    StatusCode = HttpStatusCode.OK
                };

            } 
            catch (Exception ex)
            {
                Logger.LogInformation("Unexpected error: GetNailStylistsHandler" + ex.Message);
                return new GetUsersResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Unexpected error: {ex.Message}" },
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        #endregion
    }
}
