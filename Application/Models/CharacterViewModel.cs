namespace Application.Models
{
    public class CharacterViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Image { get; set; }

        public string? Description { get; set; }

        public virtual List<int> Connections{ get; set; } = new List<int>();
    }
}
