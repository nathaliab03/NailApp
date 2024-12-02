using Microsoft.AspNetCore.Identity;

namespace NailApp.Models
{
    public class Agenda
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        // Relacionamento com NailStylist
        public int NailStylistId { get; set; }
        public NailStylist NailStylist { get; set; } = null!;

        // Cliente (pode ser simplificado ou detalhado como outra entidade)
        public string ClientId { get; set; } = null!;
        public ApplicationUser Client { get; set; } = null!;
    }
}
