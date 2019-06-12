﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

using ReactiveUI;

using Movere.Models;

namespace Movere.ViewModels
{
    internal sealed class OpenFileDialogViewModel : ReactiveObject
    {
        private readonly Action<OpenFileDialogResult> _closeAction;

        private string _fileName;

        public OpenFileDialogViewModel(Action<OpenFileDialogResult> closeAction)
        {
            _closeAction = closeAction;

            _fileName = String.Empty;

            FileExplorer = new FileExplorerViewModel(true);

            OpenCommand = ReactiveCommand.Create(Open);
            CancelCommand = ReactiveCommand.Create(Cancel);

            FileExplorer.FileOpened.Subscribe(_ => Open());

            FileExplorer.FileExplorerFolder.WhenAnyValue(vm => vm.SelectedItem).Subscribe(SelectedItemChanged);
        }

        public FileExplorerViewModel FileExplorer { get; }

        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        public ICommand OpenCommand { get; }

        public ICommand CancelCommand { get; }

        private void Open()
        {
            if (FileExplorer.FileExplorerFolder.SelectedItem is DirectoryInfo directory)
            {
                FileExplorer.NavigateTo(new Folder(directory));
                return;
            }

            Close(new OpenFileDialogResult(FileExplorer.FileExplorerFolder.SelectedItems.OfType<FileInfo>().Select(info => info.FullName)));
        }

        private void Cancel() => Close(new OpenFileDialogResult(Enumerable.Empty<string>()));

        private void Close(OpenFileDialogResult result) => _closeAction(result);

        private void SelectedItemChanged(FileSystemInfo? entry)
        {
            if (entry is FileInfo file)
            {
                FileName = file.Name;
            }
        }
    }
}