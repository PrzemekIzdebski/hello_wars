﻿using Arena.ViewModels;

namespace Arena.Commands.MenuItemCommands
{
    class AboutCommand : CommandBase
    {
        protected readonly MainWindowViewModel _viewModel;

        public AboutCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter = null)
        {

        }
    }
}
