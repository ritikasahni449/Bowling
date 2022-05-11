using Application.Interfaces;
using Application.Services;
using Bowling.DisplayModel;
using Domain;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bowling.Pages
{
    public class PlayBowlingGameModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public DisplayGame DisplayGame { get; set; } = new DisplayGame();

        [BindProperty]
        public string PinsRolled { get; set; } = string.Empty;

        private readonly IBowlingGameService _bowlingGameService;
        public PlayBowlingGameModel(ILogger<IndexModel> logger, IBowlingGameService bowlingGameService)
        {
            _logger = logger;
            _bowlingGameService = bowlingGameService;
        }
        public void OnGet()
        {
            StartNewGame();
        }

        public IActionResult OnPost()
        {
            if (int.TryParse(PinsRolled, out int pins))
            {
                RollAShot(pins);
            }
            return Page();
        }

        private void StartNewGame()
        {
            _bowlingGameService.StartNewGame();
            DisplayGame = SetDisplayGame("New Game", _bowlingGameService.GetDefaultFrameSize(), _bowlingGameService.GetMessage(), _bowlingGameService.GetScore(), _bowlingGameService.GetAllFrames());
        }

        private void RollAShot(int roll)
        {
             _bowlingGameService.AddShot(roll);
            DisplayGame = SetDisplayGame("My Game", _bowlingGameService.GetDefaultFrameSize(), _bowlingGameService.GetMessage(), _bowlingGameService.GetScore(), _bowlingGameService.GetAllFrames());
            PinsRolled = string.Empty;
        }


        private DisplayGame SetDisplayGame(string gameName, int frameDefaultSize, string message, int score, List<Frame> frames)
        {
            var displayGame = new DisplayGame { GameName = gameName, Message = message, Score = score };

            foreach (var frame in frames)
            {
                var displayFrame = new DisplayFrame();
                if (frame.FrameType == FrameType.Spare)
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
    }
}
