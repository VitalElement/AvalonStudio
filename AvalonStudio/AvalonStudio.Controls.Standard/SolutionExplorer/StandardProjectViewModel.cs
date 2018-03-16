using Avalonia.Media;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;
using System.IO;
using System.Reactive.Linq;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    internal class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(ISolutionParentViewModel parent, IProject model) : base(parent, model)
        {
            if (model.Solution.StartupProject == model)
            {
                this.VisitParents(parentVm =>
                {
                    parentVm.IsExpanded = true;
                });

                IsExpanded = true;
            }

            NewFileCommand = ReactiveCommand.Create(async () =>
            {
                var observable = Items.ObserveNewItems().OfType<SourceFileViewModel>().FirstOrDefaultAsync();

                using (var subscription = observable.Subscribe(item =>
                {
                    item.InEditMode = true;
                }))
                {
                    File.CreateText(Path.Combine(model.CurrentDirectory, "NewFile1"));

                    await observable;
                }
            });
        }

        public ReactiveCommand NewFileCommand { get; }

        public ReactiveCommand SetDefaultProjectCommand { get; private set; }

        public ReactiveCommand Remove { get; private set; }

        public override DrawingGroup Icon => Model.GetIcon();
    }
}