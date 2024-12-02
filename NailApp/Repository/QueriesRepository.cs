using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NailApp.Data;
using NailApp.Models;
using NailApp.Queries.GetNailStylists;

namespace NailApp.Repository
{
    public class QueriesRepository
    {
        #region Properties
        private readonly ApplicationDbContext Repository;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly ILogger<GetNailStylistsHandler> Logger;
        #endregion

        #region Constructor
        public QueriesRepository(ApplicationDbContext repository, ILogger<GetNailStylistsHandler> logger)
        {
            Repository = repository;
            Logger = logger;
        }
        #endregion

        #region Implementation
        public async Task<IEnumerable<NailStylist>> GetAllNailStylist(CancellationToken cancellationToken)
        {
            return await Repository.NailStylists
                .Where(n => n.IsAvailable)
                .ToListAsync(cancellationToken);
        }

        public async Task<NailStylist> GetNailStylistById(int id, CancellationToken cancellationToken)
        {
            return await Repository.NailStylists
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await Repository.Users.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Agenda>> GetAllAppointments(CancellationToken cancellationToken)
        {
            return await Repository.Agendas
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Agenda>> GetAllAppointmentsByNailStylist(int nailStylistId, CancellationToken cancellationToken)
        {
            return await Repository.Agendas
                .Where(n => n.NailStylistId == nailStylistId)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
