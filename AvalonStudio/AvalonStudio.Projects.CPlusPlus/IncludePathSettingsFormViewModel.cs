namespace AvalonStudio.Projects.CPlusPlus
{
    using MVVM;
    using Perspex.Controls;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Utils;

    public class IncludePathSettingsFormViewModel : ViewModel<CPlusPlusProject>
    {
        public IncludePathSettingsFormViewModel(CPlusPlusProject model) : base(model)
        {
            defines = new ObservableCollection<string>(model.Defines);
            includePaths = new ObservableCollection<string>(model.Includes);

            AddDefineCommand = ReactiveCommand.Create();// new RoutingCommand(AddDefine, (o) => DefineText != string.Empty && DefineText != null && !Defines.Contains(DefineText));
            AddDefineCommand.Subscribe(AddDefine);

            RemoveDefineCommand = ReactiveCommand.Create();// new RoutingCommand(RemoveDefine, (o) => SelectedDefine != string.Empty && SelectedDefine != null);
            RemoveDefineCommand.Subscribe(RemoveDefine);

            AddIncludePathCommand = ReactiveCommand.Create();
            AddIncludePathCommand.Subscribe(AddIncludePath);

            RemoveIncludePathCommand = ReactiveCommand.Create();
            RemoveIncludePathCommand.Subscribe(RemoveIncludePath);
        }

        private void Save()
        {
            Model.Defines.Clear();
            
            foreach(var define in Defines)
            {
                Model.Defines.Add(define);
            }

            Model.Includes.Clear();

            foreach(var include in IncludePaths)
            {
                Model.Includes.Add(include);
            }

            Model.Save();
        }

        private void AddDefine(object param)
        {
            Defines.Add(DefineText);
            DefineText = string.Empty;
            Save();
        }

        private void RemoveDefine(object param)
        {
            Defines.Remove(SelectedDefine);
            Save();
        }

        private async void AddIncludePath(object param)
        {
            OpenFolderDialog fbd = new OpenFolderDialog();

            fbd.InitialDirectory = Model.CurrentDirectory;

            var result = await fbd.ShowAsync();

            if (result != string.Empty)
            {
                string newInclude = Model.CurrentDirectory.MakeRelativePath(result);

                if (newInclude == string.Empty)
                {
                    newInclude = "\\";
                }

                IncludePaths.Add(newInclude);

                Save();
            }
        }

        public ReactiveCommand<object> AddIncludePathCommand { get; private set; }
        public ReactiveCommand<object> RemoveIncludePathCommand { get; private set; }
        public ReactiveCommand<object> AddDefineCommand { get; private set; }
        public ReactiveCommand<object> RemoveDefineCommand { get; private set; }

        private bool AddIncludePathCanExecute(object param)
        {
            return true;
        }

        private void RemoveIncludePath(object param)
        {
            includePaths.Remove(SelectedInclude);
            Save();
        }

        private string defineText;
        public string DefineText
        {
            get { return defineText; }
            set { this.RaiseAndSetIfChanged(ref defineText, value); }
        }

        private string selectedDefine;
        public string SelectedDefine
        {
            get { return selectedDefine; }
            set { this.RaiseAndSetIfChanged(ref selectedDefine, value); DefineText = value; }
        }

        private ObservableCollection<string> defines;
        public ObservableCollection<string> Defines
        {
            get { return defines; }
            set { this.RaiseAndSetIfChanged(ref defines, value); }
        }

        private string selectedInclude;
        public string SelectedInclude
        {
            get { return selectedInclude; }
            set { this.RaiseAndSetIfChanged(ref selectedInclude, value); }
        }

        private ObservableCollection<string> includePaths;
        public ObservableCollection<string> IncludePaths
        {
            get { return includePaths; }
            set { this.RaiseAndSetIfChanged(ref includePaths, value); }
        }
    }
}
