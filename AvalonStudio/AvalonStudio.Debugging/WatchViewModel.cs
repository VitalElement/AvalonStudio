namespace AvalonStudio.Debugging
{
    using Avalonia.Threading;
    using Debugging;
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    public class WatchViewModel : ViewModel<VariableObject>
    {
        public WatchViewModel(IDebugger debugger, VariableObject model)
            : base(model)
        {
            this.debugger = debugger;

            DeleteCommand = ReactiveCommand.Create();
            DeleteCommand.Subscribe(_ =>
            {
                //ShellViewModel.Instance.DebugManager.WatchList.RemoveWatch(this);
            });


            DisplayFormatCommand = ReactiveCommand.Create();
            DisplayFormatCommand.Subscribe((s) =>
            {
                var format = s as string;

                switch (format)
                {
                    case "hex":
                        Model.SetFormat(WatchFormat.Hexadecimal);
                        break;

                    case "dec":
                        Model.SetFormat(WatchFormat.Decimal);
                        break;

                    case "bin":
                        Model.SetFormat(WatchFormat.Binary);
                        break;

                    case "nat":
                        Model.SetFormat(WatchFormat.Natural);
                        break;

                    case "oct":
                        Model.SetFormat(WatchFormat.Octal);
                        break;
                }

                this.Invalidate(debugger);

            });
        }

        private IDebugger debugger;

        private async void Expand()
        {
            foreach(var child in Children)
            {
                if(child.Children == null)
                {
                    child.Children = new ObservableCollection<WatchViewModel>();
                    
                    if (!child.Model.AreChildrenEvaluated)
                    {
                        await child.Model.EvaluateChildrenAsync();

                        for (int i = 0; i < child.Model.NumChildren; i++)
                        {
                            var newchild = new WatchViewModel(debugger, child.Model.Children[i]);
                            await newchild.Evaluate(debugger);
                            child.Children.Add(newchild);
                        }
                    }                    
                }                    
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value)
                {
                    Expand();
                }

                this.RaiseAndSetIfChanged(ref isExpanded, value);
            }
        }

        private ObservableCollection<WatchViewModel> children;
        public ObservableCollection<WatchViewModel> Children
        {
            get { return children; }
            set { this.RaiseAndSetIfChanged(ref children, value); }
        }

        public ReactiveCommand<object> DeleteCommand { get; private set; }

        public ReactiveCommand<object> DisplayFormatCommand { get; private set; }

        private string val;
        public string Value
        {
            get { return val; }
            set { this.RaiseAndSetIfChanged(ref val, value); }
        }

        public string Name { get { return Model.Expression; } }

        public string Type { get { return Model.Type; } }

        public bool ApplyChange(VariableObjectChange change)
        {
            bool result = false;

            if (change.Expression.Contains(Model.Id))
            {
                if (change.Expression == Model.Id)
                {
                    result = true;

                    if (change.InScope)
                    {
                        Value = change.Value;
                    }
                    else
                    {
                        Value = "{ Out of Scope. }";
                        Model.Children.Clear();
                        Model.ClearEvaluated();
                        Children.Clear();
                    }

                    if (change.TypeChanged)
                    {
                        //throw new NotImplementedException ("This needs implementing cope with type change.");
                    }
                }
                else
                {
                    foreach (var child in Children)
                    {
                        if (child != null)
                        {
                            result = child.ApplyChange(change);

                            if (result)
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Investigate this case.");
                        }
                    }
                }
            }

            return result;
        }

        public async Task Invalidate(IDebugger debugger)
        {
            await Model.EvaluateAsync(debugger, false);

            foreach (var child in Children)
            {
                if (child.IsExpanded)
                {
                    child.Invalidate();
                }
            }

            if (Model.Value != null)
            {
                Value = Model.Value;
            }
            else
            {
                Value = "{ " + Model.Type + " }";
            }
        }

        public async Task Evaluate(IDebugger debugger)
        {
            await Model.EvaluateAsync(debugger);

            Children = new ObservableCollection<WatchViewModel>();

            await Model.EvaluateChildrenAsync();

            for (int i = 0; i < Model.NumChildren; i++)
            {                
                Children.Add(new WatchViewModel(debugger, Model.Children[i]));
            }
            
            if (Model.Value != null)
            {
                Value = Model.Value;
            }
            else
            {
                Value = "{ " + Model.Type + " }";
            }
        }
    }
}
