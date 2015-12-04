﻿using Arena.ViewModels;
using Common.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Common.Models;
using System;

namespace Arena.Commands.MenuItemCommands
{
    class GameSpeedChangeCommand : CommandBase
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly GameSpeedMode _speedMode;

        private readonly Dictionary<GameSpeedMode, int> _gameModesDictionary = new Dictionary<GameSpeedMode, int>()
        {
            { GameSpeedMode.Normal, 150 },
            { GameSpeedMode.Fast, 50 },
            { GameSpeedMode.VeryFast, 0 }
        };

        public GameSpeedChangeCommand(MainWindowViewModel viewModel, GameSpeedMode speedMode)
        {
            _viewModel = viewModel;
            _speedMode = speedMode;
        }

        public override void Execute(object parameter = null)
        {
            _viewModel.CurrentSpeedMode = _speedMode;
            _viewModel.Game.ChangeDelayTime(_gameModesDictionary[_speedMode]);
        }

       
        public override bool CanExecute(object parameter = null)
        {
            return _speedMode != _viewModel.CurrentSpeedMode;
        }
    }
}
