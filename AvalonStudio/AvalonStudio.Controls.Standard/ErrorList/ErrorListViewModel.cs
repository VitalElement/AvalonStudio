﻿namespace AvalonStudio.Controls.Standard.ErrorList
{
    using AvalonStudio.MVVM;
    using Languages;
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using System;
    using Extensibility;
    using Extensibility.Plugin;
    using Extensibility.Utils;

    public class ErrorListViewModel : ToolViewModel, IPlugin, IErrorList
    {
        private IShell shell;

        public ErrorListViewModel()
        {
            Title = "Error List";
            errors = new ObservableCollection<ErrorViewModel>();
        }

        private ObservableCollection<ErrorViewModel> errors;
        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set { this.RaiseAndSetIfChanged(ref errors, value); }
        }

        private ErrorViewModel selectedError;
        public ErrorViewModel SelectedError
        {
            get { return selectedError; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedError, value);

                if (value != null)
                {
                    var document = shell.OpenDocument(shell.CurrentSolution.FindFile(PathSourceFile.FromPath(null, null, value.Model.File)), value.Line);

                    document.Wait();

                    if (document != null)
                    {
                        document.Result.GotoOffset(value.Model.Offset);
                    }
                }
            }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.Bottom;
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Version Version
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Activate()
        {
            
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant(this, typeof(IErrorList));        
        }

        public void Activation()
        {
            shell = IoC.Get<IShell>();
        }
    }
}
