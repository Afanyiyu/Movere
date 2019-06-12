﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;
using Movere.Services;

namespace Movere.ViewModels
{
    internal sealed class SaveFileDialogViewModel : ReactiveObject
    {
        private readonly MessageDialogService _messageDialogService;
        private readonly Action<SaveFileDialogResult> _closeAction;

        private string _fileName;

        public SaveFileDialogViewModel(MessageDialogService messageDialogService, Action<SaveFileDialogResult> closeAction)
        {
            _messageDialogService = messageDialogService;
            _closeAction = closeAction;

            _fileName = String.Empty;

            FileExplorer = new FileExplorerViewModel(false);

            SaveCommand = ReactiveCommand.Create(SaveAsync);
            CancelCommand = ReactiveCommand.Create(Cancel);

            FileExplorer.FileOpened.Subscribe(async file => await SaveAsync());

            FileExplorer.FileExplorerFolder.WhenAnyValue(vm => vm.SelectedItem).Subscribe(SelectedItemChanged);
        }

        public FileExplorerViewModel FileExplorer { get; }

        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        private async Task SaveAsync()
        {
            if (FileExplorer.FileExplorerFolder.SelectedItem is DirectoryInfo directory)
            {
                FileExplorer.NavigateTo(new Folder(directory));
                return;
            }

            var path = FileName;

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(FileExplorer.CurrentFolder.FullPath, path);
            }

            path = Path.GetFullPath(path);

            if (System.IO.File.Exists(path))
            {
                var result = await _messageDialogService.ShowMessageDialogAsync(
                    new MessageDialogOptions(
                        DialogIcon.Warning,
                        "Save",
                        "The file already exists, overwrite it?",
                        DialogResultSet.YesNoCancel));

                if (result != DialogResult.Yes)
                {
                    if (result == DialogResult.No)
                    {
                        Cancel();
                    }

                    return;
                }
            }

            Close(new SaveFileDialogResult(FileExplorer.FileExplorerFolder.SelectedItem?.FullName));
        }

        private void Cancel() => Close(new SaveFileDialogResult(null));

        private void Close(SaveFileDialogResult result) => _closeAction(result);

        private void SelectedItemChanged(FileSystemInfo? entry)
        {
            if (entry is FileInfo file)
            {
                FileName = file.Name;
            }
        }
    }
}