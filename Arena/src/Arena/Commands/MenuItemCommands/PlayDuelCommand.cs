﻿using Arena.ViewModels;
using Common.Utilities;

namespace Arena.Commands.MenuItemCommands
{
    public class PlayDuelCommand : CommandBase
    {
        private readonly MainWindowViewModel _viewModel;

        public PlayDuelCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter = null)
        {
            _viewModel.IsGameInProgress = true;
            if (_viewModel.IsGamePaused)
            {
                _viewModel.IsGamePaused = false;
                await _viewModel.ResumeGameAsync();
            }
            else
            {
                await _viewModel.PlayNextGameAsync();
            }
            _viewModel.IsGameInProgress = false;

            if (_viewModel.ShouldRestartGame)
            {
                _viewModel.RestartGame();
            }
        }
    }
}