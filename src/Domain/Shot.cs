
namespace Domain
{
    public class Shot
    {
        public int Id { get; set; }
        public int PinsKnocked { get; set; } = 0;
        public bool IsProcessed { get; set; } = false;
    }
}
