using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using ReactiveUI;
using Mono.Debugging.Client;

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
            this.watchList = watchList;

            DeleteCommand = ReactiveCommand.Create();

            if (model.HasChildren)
            {
                children = new ObservableCollection<ObjectValueViewModel>();
                children.Add(DummyChild);
            }

            DeleteCommand.Subscribe(_ => { IoC.Get<IWatchList>().Remove(Model); });


            DisplayFormatCommand = ReactiveCommand.Create();
            DisplayFormatCommand.Subscribe(s =>
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
            get { return isExpanded; }
            set
            {
                if (value)
                {
                    Expand();
                }
                else
                {
                    Children?.Clear();
                    Children?.Add(DummyChild);
                }

                this.RaiseAndSetIfChanged(ref isExpanded, value);
            }
        }


        public ObservableCollection<ObjectValueViewModel> Children
        {
            get { return children; }
            set { this.RaiseAndSetIfChanged(ref children, value); }
        }

        public ReactiveCommand<object> DeleteCommand { get; }

        public ReactiveCommand<object> DisplayFormatCommand { get; }

        public string Value
        {
            get { return Model.Value; }
        }

        public IBrush Background
        {
            get { return background; }
            set { this.RaiseAndSetIfChanged(ref background, value); }
        }

        public bool HasChanged
        {
            get { return hasChanged; }
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
            get { return Model.Name; }
        }

        public string TypeName
        {
            get { return Model.TypeName; }
        }

        private void Expand()
        {
            Children.Remove(DummyChild);

            var children = Model.GetAllChildren();

            foreach (var child in children)
            {
                Children.Add(new ObjectValueViewModel(watchList, child));
            }
        }

        public bool ApplyChange(ObjectValue newValue)
        {
            var result = false;

            bool hasChanged = Model.Value != newValue?.Value;
            bool didHaveChildren = Model.HasChildren;

            Model = newValue;

            if (Model.HasChildren && !didHaveChildren)
            {
                hasChanged = true;

                Children = new ObservableCollection<ObjectValueViewModel>();

                Children.Add(DummyChild);
            }

            if (IsExpanded)
            {
                if (newValue.Value != null)
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        Children[i].ApplyChange(newValue.GetChild(Children[i].Model.Name));
                    }
                }
                else
                {
                    Children.Clear();
                }
            }

            Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    HasChanged = hasChanged;

                    if (hasChanged)
                    {
                        Invalidate();
                    }
                }).Wait();

            return result;
        }
    }
}