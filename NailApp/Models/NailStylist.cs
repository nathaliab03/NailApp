namespace NailApp.Models
{
    public class NailStylist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

        public ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();
    }
}
