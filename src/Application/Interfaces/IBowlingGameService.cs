
using Domain;

namespace Application.Interfaces
{
    public interface IBowlingGameService
    {
        public void AddShot(int pinsKnocked);
        public List<Frame> GetAllFrames();
        public int GetScore();
        public string GetMessage();
        public int GetDefaultFrameSize();
        public void StartNewGame();
    }
}
