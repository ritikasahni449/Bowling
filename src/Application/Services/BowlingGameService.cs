using Application.Interfaces;
using Domain;
using Domain.Enumerations;

namespace Application.Services
{
    public class BowlingGameService : IBowlingGameService
    {
        private Game game;

        public BowlingGameService()
        {
            game = new Game();
        }


        public int GetScore()
        {
            int total = 0;
            if (game != null && game.Frames != null)
            {
                foreach (var frame in game.Frames)
                {
                    if (frame.FrameType == FrameType.Spare)
                    {
                        total += frame.FrameShots.Sum() + GetNextShots(frame.Id, game.SpareFrameExtraShots);
                    }
                    else if (frame.FrameType == FrameType.Strike)
                    {
                        total += frame.FrameShots.Sum() + GetNextShots(frame.Id, game.StrikeFrameExtraShots);
                    }
                    else
                    {
                        total += frame.FrameShots.Sum();
                    }
                }
            }
            return total;
        }

        private int GetNextShots(int frameId, int count)
        {
            var processedCount = 0;
            var sum = 0;

            int currentFrameId = frameId;

            while (processedCount < count)
            {
                var nextFrame = game.Frames.OrderBy(x => x.Id).SkipWhile(x => x.Id != currentFrameId).Skip(1).FirstOrDefault();
                if (nextFrame != null)
                {
                    sum += nextFrame.FrameShots.Take(count - processedCount).Sum();
                    currentFrameId = nextFrame.Id;
                    processedCount = processedCount + nextFrame.FrameShots.Count();
                }
                else
                {
                    var nextShots = game.Shots.OrderBy(x => x.Id).Where(x => !x.IsProcessed).Take(count - processedCount);
                    if (nextShots.Any())
                    {
                        var pins = nextShots.Select(x => x.PinsKnocked);
                        sum += pins.Sum();
                        processedCount = processedCount + pins.Count();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return sum;
        }
        public string GetMessage()
        {
            return game.Message;
        }
        public int GetDefaultFrameSize()
        {
            return game.DefaultFrameSize;
        }

        public List<Frame> GetAllFrames()
        {
            return game.Frames;
        }

        public void AddShot(int pinsKnocked)
        {
            if (!HasMaxFrames())
            {
                game.Shots.Add(new Shot { Id = game.Shots.Count() + 1, PinsKnocked = pinsKnocked, IsProcessed = false });
                SetShotDisplayMessage();
                ProcessShots();
            }
            else
            {
                game.Message = "Max number of frames allowed in this game are: " + game.MaxFramesAllowed;
            }
        }

        private bool HasMaxFrames()
        {
            return game.Frames.Count() == game.MaxFramesAllowed;
        }

        private void SetShotDisplayMessage()
        {
            var unprocessedShots = game.Shots.Where(x => !x.IsProcessed).OrderBy(x => x.Id).Select(x => x.PinsKnocked).ToList();
            if (unprocessedShots.Count == 1 && IsStrike(unprocessedShots.First()))
            {
                game.Message = "STRIKE";
            }
            else if (IsSpare(unprocessedShots))
            {
                game.Message = "SPARE";
            }
            else
            {
                game.Message = string.Empty;
            }
        }

        private void ProcessShots()
        {
            var unprocessedShots = game.Shots.Where(x => !x.IsProcessed).OrderBy(x => x.Id).ToList();
            if (unprocessedShots.Count() > game.DefaultFrameSize + 1)
            {
                game.Message = "There was an error in processing the shots, and scoring is incorrect. Please start a new game";
            }
            else if (unprocessedShots.Count() > 0)
            {
                var knockedPins = unprocessedShots.Select(x => x.PinsKnocked).ToList();
                if (IsLastFrame())
                {
                    if (IsStrike(unprocessedShots.First().PinsKnocked))
                    {
                        if (unprocessedShots.Count() == game.DefaultFrameSize + 1)
                        {
                            AddFrame(FrameType.Last, knockedPins);
                            MarkShotAsProcessed(unprocessedShots);
                        }
                    }
                    else if (IsSpare(unprocessedShots.Take(game.DefaultFrameSize).Select(x => x.PinsKnocked).ToList()))
                    {
                        if (unprocessedShots.Count() == game.DefaultFrameSize + 1)
                        {
                            AddFrame(FrameType.Last, knockedPins);
                            MarkShotAsProcessed(unprocessedShots);
                        }
                    }
                    else if (unprocessedShots.Count() == game.DefaultFrameSize)
                    {
                        AddFrame(FrameType.Last, knockedPins);
                        MarkShotAsProcessed(unprocessedShots);
                    }
                }
                else
                {
                    if (unprocessedShots.Count() == 1 && IsStrike(unprocessedShots.First().PinsKnocked))
                    {
                        AddFrame(FrameType.Strike, knockedPins);
                        MarkShotAsProcessed(unprocessedShots);
                    }
                    else
                    {
                        if (unprocessedShots.Count() == game.DefaultFrameSize && IsSpare(knockedPins))
                        {
                            AddFrame(FrameType.Spare, knockedPins);
                            MarkShotAsProcessed(unprocessedShots);
                        }
                        else if (unprocessedShots.Count() == game.DefaultFrameSize)
                        {
                            AddFrame(FrameType.Open, knockedPins);
                            MarkShotAsProcessed(unprocessedShots);
                        }
                    }
                }
            }
        }

        private void MarkShotAsProcessed(List<Shot> unprocessedShots)
        {
            foreach (var unprocessedShot in unprocessedShots)
            {
                var shot = game.Shots.FirstOrDefault(x => x.Id == unprocessedShot.Id);
                if (shot != null)
                {
                    shot.IsProcessed = true;
                }
            }
        }

        private bool IsSpare(List<int> pinsKnocked)
        {
            return pinsKnocked.Sum() == game.MaxPinsAllowed;
        }

        private bool IsStrike(int pinsKnocked)
        {
            return pinsKnocked == game.MaxPinsAllowed;
        }

        private bool IsLastFrame()
        {
            return game.Frames.Count() == game.MaxFramesAllowed - 1;
        }

        private void AddFrame(FrameType frameType, List<int> shots)
        {
            if (!HasMaxFrames())
            {
                game.Frames.Add(new Frame { Id = game.Frames.Count() + 1, FrameShots = shots, FrameType = frameType });
            }
            else
            {
                game.Message = "Max number of frames allowed in this game are: " + game.MaxFramesAllowed;
            }
        }
    }
}