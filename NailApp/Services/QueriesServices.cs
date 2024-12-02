using NailApp.Models;
using NailApp.Repository;

namespace NailApp.Services
{
    public class QueriesServices : IQueriesServices
    {
        public QueriesRepository QueriesRepository { get; set; }

        public QueriesServices(QueriesRepository queriesRepository)
        {
            QueriesRepository = queriesRepository;
        }

        public async Task<IEnumerable<NailStylist>> GetAllNailStylistAsync(CancellationToken cancellationToken)
        {
            var nailStylists = await QueriesRepository.GetAllNailStylist(cancellationToken);
            return nailStylists.Select(n => new NailStylist
            {
                Id = n.Id,
                Name = n.Name,
                IsAvailable = n.IsAvailable
            }).ToList();
        }

        public async Task<NailStylist> GetNailStylist(int id, CancellationToken cancellationToken)
        {
            return await QueriesRepository.GetNailStylistById(id, cancellationToken);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await QueriesRepository.GetAllUsers(cancellationToken);
        }

        public async Task<IEnumerable<Agenda>> GetAllAppointments(CancellationToken cancellationToken)
        {
            var appointment = await QueriesRepository.GetAllAppointments(cancellationToken);
            return appointment.Select(n => new Agenda
            {
                Id = n.Id,
                Date = n.Date,
                Time = n.Time,
                NailStylistId = n.NailStylistId,
                NailStylist = n.NailStylist,
                Client = n.Client,
                ClientId = n.ClientId,
            }).ToList();
        }

        public async Task<IEnumerable<Agenda>> GetAllAppointmentsByNailStylist(int id, CancellationToken cancellationToken)
        {
            return await QueriesRepository.GetAllAppointmentsByNailStylist(id, cancellationToken);
        }

        public async Task<IEnumerable<DateTime>> GetAvailableSlotsByNailStylist(int nailStylistId, DateTime date, CancellationToken cancellationToken)
        {
            // Verifique se a data é válida (segunda a sábado)
            if (date.DayOfWeek == DayOfWeek.Sunday)
                throw new ArgumentException("A nailstylist não trabalha aos domingos.");

            // Gera a lista de horários disponíveis (das 9h às 18h)
            var availableSlots = Enumerable.Range(9, 10) // Horas de 9h até 18h
                .Select(hour => new DateTime(date.Year, date.Month, date.Day, hour, 0, 0))
                .ToList();

            // Obtém todos os agendamentos para a nailstylist
            var appointments = await GetAllAppointmentsByNailStylist(nailStylistId, cancellationToken);

            // Filtra horários já ocupados
            var occupiedSlots = appointments
                .Where(a => a.Date.Date == date.Date) // Apenas para o dia especificado
                .Select(a => a.Date); // Extraí apenas os horários

            // Remove horários ocupados
            availableSlots = availableSlots
                .Where(slot => !occupiedSlots.Contains(slot))
                .ToList();

            return availableSlots;
        }
    }
}
