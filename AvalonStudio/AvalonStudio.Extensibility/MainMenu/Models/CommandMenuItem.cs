﻿namespace AvalonStudio.Extensibility.MainMenu.Models
{
    using Commands;
    using Perspex.Input;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Input;

    public class CommandMenuItem : StandardMenuItem, ICommandUiItem
    {
        private readonly Command _command;
        private readonly KeyGesture _keyGesture;
        private readonly StandardMenuItem _parent;
        private readonly List<StandardMenuItem> _listItems;

        public override string Text
        {
            get { return _command.Text; }
        }

        public override Uri IconSource
        {
            get { return _command.IconSource; }
        }

        public override string InputGestureText
        {
            get
            {
                return _keyGesture == null
                    ? string.Empty
                    : "gesture";//_keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture);
            }
        }

        private ICommand __command;
        public override ICommand Command
        {
            get { return __command; }
        }

        public override bool IsChecked
        {
            get { return _command.Checked; }
        }

        public override bool IsVisible
        {
            get { return _command.Visible; }
        }

        private bool IsListItem { get; set; }

        public CommandMenuItem(Command command, ICommand cmd, StandardMenuItem parent)
        {
            __command = cmd;
            _command = command;
            
            _keyGesture = IoC.Get<ICommandKeyGestureService>().GetPrimaryKeyGesture(_command.CommandDefinition);
            _parent = parent;

            _listItems = new List<StandardMenuItem>();
        }

        CommandDefinitionBase ICommandUiItem.CommandDefinition
        {
            get { return _command.CommandDefinition; }
        }

        void ICommandUiItem.Update(CommandHandlerWrapper commandHandler)
        {
            //if (_command != null && _command.CommandDefinition.IsList && !IsListItem)
            //{
            //    foreach (var listItem in _listItems)
            //        _parent.Children.Remove(listItem);

            //    _listItems.Clear();

            //    var listCommands = new List<Command>();
            //    commandHandler.Populate(_command, listCommands);

            //    _command.Visible = false;

            //    int startIndex = _parent.Children.IndexOf(this) + 1;

            //    foreach (var command in listCommands)
            //    {
            //        var newMenuItem = new CommandMenuItem(command, _parent)
            //        {
            //            IsListItem = true
            //        };
            //        _parent.Children.Insert(startIndex++, newMenuItem);
            //        _listItems.Add(newMenuItem);
            //    }
            //}
        }
    }
}