using NailApp.Models;

namespace NailApp.Services
{
    public interface ICommandService
    {
        public bool CreateAppointment(Agenda request, CancellationToken cancellationToken);
    }
}
