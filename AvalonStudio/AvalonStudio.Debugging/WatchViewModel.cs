using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using Mono.Debugging.Client;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
    public class ObjectValueViewModel : ViewModel<ObjectValue>
    {
        private static readonly ObjectValueViewModel DummyChild = new ObjectValueViewModel();
        private ObservableCollection<ObjectValueViewModel> children;

        private IBrush background;

        private bool hasChanged;

        private bool isExpanded;

        private readonly WatchListViewModel watchList;

        private ObjectValueViewModel() : base(ObjectValue.CreateUnknown("Dummy"))
        {
        }

        public ObjectValueViewModel(WatchListViewModel watchList, ObjectValue model)
            : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this.watchList = watchList;

            model.ValueChanged += (sender, e) =>
            {
                this.RaisePropertyChanged(nameof(Value));
            };

            DeleteCommand = ReactiveCommand.Create(()=> IoC.Get<IWatchList>().Remove(Model));

            if (model.HasChildren)
            {
                children = new ObservableCollection<ObjectValueViewModel>();
                children.Add(DummyChild);
            }

            AddWatchPointCommand = ReactiveCommand.Create(() =>
            {
                IoC.Get<IDebugManager2>().Breakpoints.Add(new WatchPoint(Model.Name));
            });

            DisplayFormatCommand = ReactiveCommand.Create<string>(s =>
            {
                /*var format = s as string;

				switch (format)
				{
					case "hex":
						await Model.SetFormat(WatchFormat.Hexadecimal);
						break;

					case "dec":
						await Model.SetFormat(WatchFormat.Decimal);
						break;

					case "bin":
						await Model.SetFormat(WatchFormat.Binary);
						break;

					case "nat":
						await Model.SetFormat(WatchFormat.Natural);
						break;

					case "oct":
						await Model.SetFormat(WatchFormat.Octal);
						break;
                }
				
				await Invalidate(debugger);*/
            });
        }

        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (value)
                {
                    Expand();
                }
                else
                {
                    Children?.Clear();

                    if (Model.HasChildren)
                    {
                        Children?.Add(DummyChild);
                    }
                }

                this.RaiseAndSetIfChanged(ref isExpanded, value);
            }
        }

        public ObservableCollection<ObjectValueViewModel> Children
        {
            get { return children; }
            set { this.RaiseAndSetIfChanged(ref children, value); }
        }

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }

        public ReactiveCommand<String, Unit> DisplayFormatCommand { get; }

        public ReactiveCommand<Unit, Unit> AddWatchPointCommand { get; }

        public string Value
        {
            get
            {
                if (Model != null)
                {
                    if (Model.IsEvaluating)
                    {
                        return "Evaluating...";
                    }

                    return Model.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public IBrush Background
        {
            get { return background; }
            set { this.RaiseAndSetIfChanged(ref background, value); }
        }

        public bool HasChanged
        {
            get
            {
                return hasChanged;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref hasChanged, value);

                if (value)
                {
                    Background = Brush.Parse("#33008299");
                }
                else
                {
                    Background = null;
                }
            }
        }

        public string DisplayName
        {
            get { return Model?.Name; }
        }

        public string TypeName
        {
            get { return Model?.TypeName; }
        }

        private void Expand()
        {
            Children.Remove(DummyChild);

            Task.Run(() =>
            {
                var children = Model.GetAllChildren();

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var child in children)
                    {
                        Children.Add(new ObjectValueViewModel(watchList, child));
                    }
                });
            });
        }

        public bool ApplyChange(ObjectValue newValue)
        {
            var result = false;

            bool hasChanged = Model.Value != newValue?.Value;
            bool didHaveChildren = Model.HasChildren;

            if ((object)Model != (object)newValue)
            {
                Model = newValue;

                if (Model != null)
                {
                    if (Model.HasChildren && !didHaveChildren)
                    {
                        hasChanged = true;

                        Children = new ObservableCollection<ObjectValueViewModel>();

                        Children.Add(DummyChild);
                    }
                }
            }

            if (IsExpanded && Model.HasChildren)
            {
                if (newValue.Value != null)
                {
                    var newChildren = newValue.GetAllChildren();
                    
                    for (int i = 0; i < Children.Count && i < newChildren.Length; i++)
                    {
                        if (Children[i].Model != null)
                        {
                            Children[i].ApplyChange(newChildren[i]);
                        }
                    }
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Children.Clear();
                    });
                }
            }
            else if (IsExpanded && !Model.HasChildren)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IsExpanded = false;
                });
            }

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                HasChanged = hasChanged;

                if (hasChanged)
                {
                    Invalidate();
                }
            });

            return result;
        }
    }
}