using Domain.Enumerations;

namespace Domain
{
    public class Frame
    {
        public int Id { get; set; }
        public FrameType FrameType { get; set; } = FrameType.None;
        public List<int> FrameShots { get; set; } = new List<int>();
    }
}