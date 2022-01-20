﻿using System;
using System.Windows.Input;

namespace DocumentTemplateManager.DesktopClient.Commands
{
    internal class CommonCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool> _canExecute;

        public CommonCommand(Action<object?> execute, Func<object?, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute?.Invoke(parameter);
    }
}
