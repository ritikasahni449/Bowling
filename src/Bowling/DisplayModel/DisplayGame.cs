namespace Bowling.DisplayModel
{
    public class DisplayGame
    {
        public List<DisplayFrame> Frames { get; set; } = new List<DisplayFrame>();
        public int Score { get; set; }
        public string Message { get; set; } = string.Empty;
        public string GameName { get; set; } = string.Empty;
    }
}
