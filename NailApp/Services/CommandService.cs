using Microsoft.Extensions.Logging;
using NailApp.Models;
using NailApp.Repository;

namespace NailApp.Services
{
    public class CommandService : ICommandService
    {
        private CommandRepository Repository { get; set; }

        public CommandService(CommandRepository repository)
        {
            Repository = repository;
        }

        public bool CreateAppointment(Agenda request, CancellationToken cancellationToken)
        {
            var result = Repository.CreateAppointment(request, cancellationToken);

            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
