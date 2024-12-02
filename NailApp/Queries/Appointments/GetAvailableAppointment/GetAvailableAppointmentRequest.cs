using MediatR;

namespace NailApp.Queries.Appointments.GetAvailableAppointment
{
    public class GetAvailableAppointmentRequest : IRequest<GetAvailableAppointmentResponse>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public List<DateTime>? AvailableDates { get; set; }
    }
}
