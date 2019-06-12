﻿using System.Threading.Tasks;

using Avalonia.Controls;

using Movere.Models;
using Movere.ViewModels;
using Movere.Views;

namespace Movere.Services
{
    public sealed class MessageDialogService
    {
        private readonly Window _owner;

        public MessageDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task<IDialogResult> ShowMessageDialogAsync(MessageDialogOptions options)
        {
            var dialog = new MessageDialog();
            var viewModel = new MessageDialogViewModel(options, dialog.Close);

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<IDialogResult>(_owner);
        }
    }
}