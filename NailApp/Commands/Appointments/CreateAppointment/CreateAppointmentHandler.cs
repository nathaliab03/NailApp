using MediatR;
using NailApp.Models;
using NailApp.Services;
using System.Net;

namespace NailApp.Commands.Appointments.CreateAppointment
{
    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentRequest, CreateAppointmentResponse>
    {
        private ILogger<CreateAppointmentHandler> Logger;
        private ICommandService Services;

        public CreateAppointmentHandler(ILogger<CreateAppointmentHandler> logger, ICommandService services)
        {
            Logger = logger;
            Services = services;
        }

        public async Task<CreateAppointmentResponse> Handle (CreateAppointmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Starting handle request: GetNailStylistByIdHandler");
                var response = Services.CreateAppointment(new Agenda
                {
                    Date = request.Date,
                    Time = request.Time,
                    ClientId = request.ClientId,
                    NailStylistId = request.NailStylistId
                }, cancellationToken);

                if (response == false)
                {
                    return new CreateAppointmentResponse
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "NailStylist not found." },
                        StatusCode = HttpStatusCode.NotFound,
                    };
                }

                Logger.LogInformation("Finishing handle request: GetNailStylistByIdHandler" + response);
                return new CreateAppointmentResponse
                {
                    IsSuccess = true,
                    Content = response,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Unexpected error: GetNailStylistByIdHandler" + ex.Message);
                return new CreateAppointmentResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Unexpected error: {ex.Message}" },
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
