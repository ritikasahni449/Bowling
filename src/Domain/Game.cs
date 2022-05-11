
namespace Domain
{
    public class Game
    {
        public int MaxPinsAllowed { get; set; } = 10;

        public int MaxFramesAllowed { get; set; } = 10;

        public int DefaultFrameSize { get; set; } = 2;

        public int SpareFrameExtraShots { get; set; } = 1;
        public int StrikeFrameExtraShots { get; set; } = 2;

        public string Message { get; set; } = string.Empty;

        public List<Frame> Frames { get; set; } = new List<Frame>();

        public List<Shot> Shots { get; set; } = new List<Shot>();
    }
}
