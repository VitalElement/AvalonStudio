﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System;
using System.Reactive.Disposables;

namespace AvalonStudio.Behaviors
{
    class HideOnLostFocusBehavior : Behavior<Control>
    {
        private CompositeDisposable _disposables = new CompositeDisposable();
        private Control _attachedControl;

        static readonly AvaloniaProperty<string> AttachedControlNameProperty = AvaloniaProperty.Register<HideOnLostFocusBehavior, string>(nameof(AttachedControlName));

        public string AttachedControlName
        {
            get { return GetValue(AttachedControlNameProperty); }
            set { SetValue(AttachedControlNameProperty, value); }
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AttachedToLogicalTree += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(AttachedControlName))
                {
                    _attachedControl = AssociatedObject.FindControl<Control>(AttachedControlName);

                    if (_attachedControl == null)
                    {
                        throw new Exception($"Control: {AttachedControlName} was not found on the control.");
                    }

                    _disposables.Add(_attachedControl.GetObservable(Control.IsFocusedProperty).Subscribe(focused =>
                    {
                        if (!focused)
                        {
                            AssociatedObject.IsVisible = false;
                        }
                    }));
                }
                else
                {
                    _disposables.Add(AssociatedObject.GetObservable(Control.IsFocusedProperty).Subscribe(focused =>
                    {
                        if (!focused)
                        {
                            AssociatedObject.IsVisible = false;
                        }
                    }));
                }
            };
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            _disposables.Dispose();
        }
    }
}
