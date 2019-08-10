﻿using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Movere.Controls
{
    internal class DoubleClickContentControl : ContentControl
    {
        public static readonly DirectProperty<DoubleClickContentControl, ICommand?> DoubleClickCommandProperty =
            AvaloniaProperty.RegisterDirect<DoubleClickContentControl, ICommand?>(
                nameof(DoubleClickCommand),
                c => c.DoubleClickCommand,
                (c, v) => c.DoubleClickCommand = v);

        public static readonly StyledProperty<object?> DoubleClickCommandParameterProperty =
            AvaloniaProperty.Register<DoubleClickContentControl, object?>(nameof(DoubleClickCommandParameter));

        private ICommand? _doubleClickCommand;

        public DoubleClickContentControl()
        {
            DoubleTapped += OnDoubleTapped;
        }

        protected virtual void OnDoubleTapped(object sender, RoutedEventArgs e)
        {
            if (DoubleClickCommand != null)
            {
                var parameter = DoubleClickCommandParameter;

                if (DoubleClickCommand.CanExecute(parameter))
                {
                    DoubleClickCommand.Execute(parameter);
                }
            }
        }

        public ICommand? DoubleClickCommand
        {
            get => _doubleClickCommand;
            set => SetAndRaise(DoubleClickCommandProperty, ref _doubleClickCommand, value);
        }

        public object? DoubleClickCommandParameter
        {
            get => GetValue(DoubleClickCommandParameterProperty);
            set => SetValue(DoubleClickCommandParameterProperty, value);
        }
    }
}
