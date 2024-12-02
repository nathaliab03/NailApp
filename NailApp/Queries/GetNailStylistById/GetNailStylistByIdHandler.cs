using MediatR;
using NailApp.Models;
using NailApp.Services;
using System.Net;

namespace NailApp.Queries.GetNailStylistById
{
    public class GetNailStylistByIdHandler : IRequestHandler<GetNailStylistByIdRequest, GetNailStylistByIdResponse>
    {
        private readonly ILogger<GetNailStylistByIdHandler> Logger;
        private readonly IQueriesServices Services;

        public GetNailStylistByIdHandler(ILogger<GetNailStylistByIdHandler> logger, IQueriesServices services)
        {
            Logger = logger;
            Services = services;
        }

        public async Task<GetNailStylistByIdResponse> Handle(GetNailStylistByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Starting handle request: GetNailStylistByIdHandler");
                var nailStylist = await Services.GetNailStylist(request.Id, cancellationToken);

                if (nailStylist == null)
                {
                    return new GetNailStylistByIdResponse
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "NailStylist not found." },
                        StatusCode = HttpStatusCode.NotFound,
                    };
                }

                Logger.LogInformation("Finishing handle request: GetNailStylistByIdHandler" + nailStylist);
                return new GetNailStylistByIdResponse
                {
                    IsSuccess = true,
                    Content = nailStylist,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Unexpected error: GetNailStylistByIdHandler" + ex.Message);
                return new GetNailStylistByIdResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Unexpected error: {ex.Message}" },
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
