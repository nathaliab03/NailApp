using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NailApp.Data;
using NailApp.Models;
using NailApp.Queries.GetNailStylists;

namespace NailApp.Repository
{
    public class CommandRepository
    {
        #region Properties
        private readonly ApplicationDbContext Repository;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly ILogger<GetNailStylistsHandler> Logger;
        #endregion

        #region Constructor
        public CommandRepository(ApplicationDbContext repository, ILogger<GetNailStylistsHandler> logger, UserManager<ApplicationUser> userManager)
        {
            Repository = repository;
            Logger = logger;
            UserManager = userManager;
        }
        #endregion

        #region Implementation
        public async Task<Agenda> CreateAppointment(Agenda request, CancellationToken cancellationToken)
        {

            var createAppointment = await Repository.AddAsync(request, cancellationToken);
            await Repository.SaveChangesAsync(cancellationToken);

            return createAppointment.Entity;
        }
        #endregion
    }
}
