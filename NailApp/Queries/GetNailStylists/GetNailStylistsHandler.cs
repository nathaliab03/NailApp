using MediatR;
using NailApp.Services;
using System.Net;

namespace NailApp.Queries.GetNailStylists
{
    public class GetNailStylistsHandler : IRequestHandler<GetNailStylistsRequest,GetNailStylistsResponse>
    {
        private readonly IQueriesServices Services;
        private readonly ILogger<GetNailStylistsHandler> Logger;

        public GetNailStylistsHandler(IQueriesServices services, ILogger<GetNailStylistsHandler> logger)
        {
            Services = services;
            Logger = logger;
        }

        public async Task<GetNailStylistsResponse> Handle(GetNailStylistsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Starting handle request: GetNailStylistsHandler");
                var nailStylists = await Services.GetAllNailStylistAsync(cancellationToken);

                if (nailStylists == null || !nailStylists.Any())
                {
                    return new GetNailStylistsResponse
                    { 
                        IsSuccess = false,
                        Errors = new List<string> { "No NailStylists found." },
                        StatusCode = HttpStatusCode.NotFound,
                    };
                }

                Logger.LogInformation("Finishing handle request: GetNailStylistsHandler" + nailStylists);
                return new GetNailStylistsResponse {
                    IsSuccess = true,
                    Content = nailStylists,
                    StatusCode = HttpStatusCode.OK
                };

            } 
            catch (Exception ex)
            {
                Logger.LogInformation("Unexpected error: GetNailStylistsHandler" + ex.Message);
                return new GetNailStylistsResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Unexpected error: {ex.Message}" },
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
