using Application.Services;
using Bowling.DisplayModel;
using Domain;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bowling.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<DisplayGame> displayGames = new List<DisplayGame>();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            AddTestGame_AllStrikes();
            AddTestGame_AllOpen();
            AddTestGame_AllSpare();
        }

        private DisplayGame SetDisplayGame(string gameName, int frameDefaultSize, string message, int score, List<Frame> frames)
        {
            var displayGame = new DisplayGame { GameName = gameName, Message = message, Score = score };
            foreach(var frame in frames)
            {
                var displayFrame = new DisplayFrame();
                if(frame.FrameType == FrameType.Spare)
                {
                    displayFrame.Shots.AddRange(frame.FrameShots.Take(frame.FrameShots.Count() - 1).Select(x => x.ToString()));
                    displayFrame.Shots.Add("/");
                }
                else if (frame.FrameType == FrameType.Strike)
                {
                    displayFrame.Shots.AddRange(frame.FrameShots.Select(x => "X"));
                    for (int i = 0; i < frameDefaultSize - 1; i++)
                    {
                        displayFrame.Shots.Add("");
                    }
                }
                else
                {
                    displayFrame.Shots.AddRange(frame.FrameShots.Select(x => x.ToString()));
                }

                displayGame.Frames.Add(displayFrame);
            }

            return displayGame;
        }

        private void AddTestGame_AllStrikes()
        {
            var bowlingService = new BowlingGameService();
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            bowlingService.AddShot(10);
            displayGames.Add(SetDisplayGame("All Strikes", bowlingService.GetDefaultFrameSize(), bowlingService.GetMessage(), bowlingService.GetScore(), bowlingService.GetAllFrames()));
        }
        private void AddTestGame_AllOpen()
        {
            var bowlingService = new BowlingGameService();
            bowlingService.AddShot(3);
            bowlingService.AddShot(4);
            bowlingService.AddShot(3);
            bowlingService.AddShot(5);
            bowlingService.AddShot(6);
            bowlingService.AddShot(3);
            bowlingService.AddShot(2);
            bowlingService.AddShot(3);
            bowlingService.AddShot(1);
            bowlingService.AddShot(3);
            bowlingService.AddShot(3);
            bowlingService.AddShot(1);
            bowlingService.AddShot(3);
            bowlingService.AddShot(4);
            bowlingService.AddShot(3);
            bowlingService.AddShot(5);
            bowlingService.AddShot(3);
            bowlingService.AddShot(6);
            bowlingService.AddShot(3);
            bowlingService.AddShot(2);
            displayGames.Add(SetDisplayGame("All Open", bowlingService.GetDefaultFrameSize(), bowlingService.GetMessage(), bowlingService.GetScore(), bowlingService.GetAllFrames()));

        }
        private void AddTestGame_AllSpare()
        {
            var bowlingService = new BowlingGameService();
            bowlingService.AddShot(3);
            bowlingService.AddShot(7);
            bowlingService.AddShot(3);
            bowlingService.AddShot(7);
            bowlingService.AddShot(6);
            bowlingService.AddShot(4);
            bowlingService.AddShot(5);
            bowlingService.AddShot(5);
            bowlingService.AddShot(7);
            bowlingService.AddShot(3);
            bowlingService.AddShot(8);
            bowlingService.AddShot(2);
            bowlingService.AddShot(6);
            bowlingService.AddShot(4);
            bowlingService.AddShot(5);
            bowlingService.AddShot(5);
            bowlingService.AddShot(4);
            bowlingService.AddShot(6);
            bowlingService.AddShot(8);
            bowlingService.AddShot(2);
            bowlingService.AddShot(9);
            displayGames.Add(SetDisplayGame("All Spares", bowlingService.GetDefaultFrameSize(), bowlingService.GetMessage(), bowlingService.GetScore(), bowlingService.GetAllFrames()));

        }
    }
}