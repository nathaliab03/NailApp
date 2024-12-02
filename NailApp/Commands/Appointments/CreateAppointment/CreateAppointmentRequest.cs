using MediatR;
using NailApp.Models;

namespace NailApp.Commands.Appointments.CreateAppointment
{
    public class CreateAppointmentRequest : IRequest<CreateAppointmentResponse>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int NailStylistId { get; set; }
        public string ClientId { get; set; } = null!;
    }
}
