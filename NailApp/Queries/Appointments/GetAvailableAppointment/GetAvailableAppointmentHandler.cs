using MediatR;
using NailApp.Models;
using NailApp.Services;
using System.Net;

namespace NailApp.Queries.Appointments.GetAvailableAppointment
{
    public class GetAvailableAppointmentHandler : IRequestHandler<GetAvailableAppointmentRequest, GetAvailableAppointmentResponse>
    {

        private readonly ILogger<GetAvailableAppointmentHandler> Logger;
        private readonly IQueriesServices Services;

        public GetAvailableAppointmentHandler(ILogger<GetAvailableAppointmentHandler> logger, IQueriesServices services)
        {
            Logger = logger;
            Services = services;
        }

        public async Task<GetAvailableAppointmentResponse> Handle(GetAvailableAppointmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Obtém a data atual
                var currentDate = DateTime.Now;

                // Define a data final (3 meses à frente)
                var endDate = currentDate.AddMonths(3);

                // Verifique se a data fornecida está dentro do intervalo (hoje até 3 meses à frente)
                var dateToUse = request.Date >= currentDate && request.Date <= endDate
                    ? request.Date
                    : currentDate;

                var availableSlots = new Dictionary<DateTime, List<string>>();

                // Itera sobre os próximos três meses (a partir da data fornecida ou data atual)
                for (var date = dateToUse.Date; date <= endDate; date = date.AddDays(1))
                {
                    // Verifica se o dia é um dia útil (segunda a sábado)
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    // Gera a lista de horários disponíveis (das 9h às 18h)
                    var slotsForDay = Enumerable.Range(9, 10) // Horas de 9h até 18h
                        .Select(hour => new DateTime(date.Year, date.Month, date.Day, hour, 0, 0))
                        .ToList();

                    // Obtém todos os agendamentos para a nailstylist
                    var appointments = await Services.GetAllAppointmentsByNailStylist(request.Id, cancellationToken);

                    // Filtra os agendamentos que ocorreram no mesmo dia
                    var occupiedSlots = appointments
                        .Where(a => a.Date.Date == date.Date) // Apenas para o dia especificado
                        .Select(a => a.Date) // Extraí apenas os horários ocupados
                        .ToList();

                    // Remove os horários ocupados
                    var availableForDay = slotsForDay
                        .Where(slot => !occupiedSlots.Contains(slot))
                        .Select(slot => slot.ToString("HH:mm")) // Converte os horários para o formato "HH:mm"
                        .ToList();

                    // Se houver horários disponíveis, adicione ao dicionário
                    if (availableForDay.Any())
                    {
                        availableSlots[date] = availableForDay;
                    }
                }

                Logger.LogInformation("Finishing handle request: GetNailStylistByIdHandler" + availableSlots);
                return new GetAvailableAppointmentResponse
                {
                    IsSuccess = true,
                    Content = availableSlots,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Unexpected error: GetNailStylistByIdHandler" + ex.Message);
                return new GetAvailableAppointmentResponse
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Unexpected error: {ex.Message}" },
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
