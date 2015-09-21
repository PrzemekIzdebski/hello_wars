﻿using System.Linq;
using Common;
using Game.CubeClash.Models;
using Game.CubeClash.ViewModels;

namespace Game.CubeClash.Commands
{
    internal class GoDownCommand : CommandBase
    {
        private CubeClashViewModel _viewModel;

        public GoDownCommand(CubeClashViewModel viewModel)
            : base()
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter = null)
        {
            var ooo = _viewModel.PlayersCollection.First() as CubeModel;
            ooo.Down();
        }
    }
}