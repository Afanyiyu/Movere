﻿using System.Threading.Tasks;

using Avalonia.Controls;

using Movere.Models;
using Movere.ViewModels;
using OpenFileDialog = Movere.Views.OpenFileDialog;

namespace Movere.Services
{
    public sealed class OpenFileDialogService
    {
        private readonly Window _owner;

        public OpenFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task<OpenFileDialogResult> ShowDialogAsync(bool allowMultipleSelection = false)
        {
            var dialog = new OpenFileDialog();

            var viewModel = new OpenFileDialogViewModel(
                allowMultipleSelection,
                new DefaultFileIconProvider(),
                new ClipboardService(),
                dialog.Close);

            dialog.DataContext = viewModel;

            return dialog.ShowDialog<OpenFileDialogResult>(_owner);
        }
    }
}
