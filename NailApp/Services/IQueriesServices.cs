using NailApp.Models;

namespace NailApp.Services
{
    public interface IQueriesServices
    {
        public Task<IEnumerable<NailStylist>> GetAllNailStylistAsync(CancellationToken cancellationToken);
        public Task<NailStylist> GetNailStylist(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<Agenda>> GetAllAppointments(CancellationToken cancellationToken);
        public Task<IEnumerable<Agenda>> GetAllAppointmentsByNailStylist(int nailStylistId, CancellationToken cancellationToken);
    }
}
