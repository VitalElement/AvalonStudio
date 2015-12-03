namespace AvalonStudio.Controls.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using AvalonStudio.Models.Solutions;
    using AvalonStudio.MVVM;
    using ReactiveUI;

    class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(Project model) : base(model)
        {
            //MenuConfigCommand = new RoutingCommand((o) =>
            //{
            //    if (model.SelectedConfiguration.ToolChain is BitThunderToolChain)
            //    {
            //        (model.SelectedConfiguration.ToolChain as BitThunderToolChain).MenuConfig(Workspace.This.VEConsole, this.Model, Workspace.This.ProcessCancellationToken);
            //    }
            //});

            ConfigureCommand = ReactiveCommand.Create();
            ConfigureCommand.Subscribe((o) =>
            {
                //Workspace.This.ModalDialog = new ProjectConfigurationDialogViewModel(Model, () => this.RaisePropertyChanged(() => Icon));
                //Workspace.This.ModalDialog.ShowDialog();
            });

            SetDefaultProjectCommand = ReactiveCommand.Create();
            SetDefaultProjectCommand.Subscribe((o) =>
            {
                Model.Solution.DefaultProject = Model;
                Model.Solution.SaveChanges();

                Workspace.This.SolutionExplorer.Solution.First().VisitAllChildren((p) =>
                {
                    if (p is StandardProjectViewModel)
                    {
                        (p as StandardProjectViewModel).InvalidateFontWeight();
                    }

                    return false;
                });
            });
        }

        //public object Icon
        //{
        //    get
        //    {
        //        if (Model.SelectedConfiguration.IsLibrary)
        //        {
        //            return Application.Current.FindResource("appbar_book_perspective");
        //        }
        //        else
        //        {
        //            return Application.Current.FindResource("appbar_box_layered");
        //        }
        //    }
        //}


        public ReactiveCommand<object> SetDefaultProjectCommand { get; private set; }
        public ReactiveCommand<object> ConfigureCommand { get; private set; }
    }
}
