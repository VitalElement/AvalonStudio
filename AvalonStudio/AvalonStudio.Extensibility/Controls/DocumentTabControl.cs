using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.LogicalTree;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.VisualTree;

namespace AvalonStudio.Controls
{
    public class DocumentTabControl : SelectingItemsControl
    {
        private List<object> _cacheItems;

        public DocumentTabControl()
        {
            SelectionMode = SelectionMode.AlwaysSelected;
            _cacheItems = new List<object>();
        }

        /// <summary>
        /// Defines an <see cref="IMemberSelector"/> that selects the content of a <see cref="TabItem"/>.
        /// </summary>
        public static readonly IMemberSelector ContentSelector =
            new FuncMemberSelector<object, object>(SelectContent);

        public static readonly StyledProperty<object> HeaderSeperatorContentProperty = AvaloniaProperty.Register<DocumentTabControl, object>(nameof(HeaderSeperatorContent));

        public object HeaderSeperatorContent
        {
            get { return GetValue(HeaderSeperatorContentProperty); }
            set { SetValue(HeaderSeperatorContentProperty, value); }
        }

        public static readonly StyledProperty<IDataTemplate> HeaderTemplateProperty = AvaloniaProperty.Register<DocumentTabControl, IDataTemplate>(nameof(HeaderTemplate));

        public IDataTemplate HeaderTemplate
        {
            get { return GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly AvaloniaProperty<ObservableCollection<Visual>> VisibleItemsProperty = AvaloniaProperty.Register<DocumentTabControl, ObservableCollection<Visual>>(nameof(VisibleItems));

        public ObservableCollection<Visual> VisibleItems
        {
            get { return GetValue(VisibleItemsProperty); }
            set { SetValue(VisibleItemsProperty, value); }
        }

        public static readonly AvaloniaProperty<bool> CacheTabsProperty = AvaloniaProperty.Register<DocumentTabControl, bool>(nameof(CacheTabs), true);

        public bool CacheTabs
        {
            get { return GetValue(CacheTabsProperty); }
            set { SetValue(CacheTabsProperty, value); }
        }

        public static readonly AvaloniaProperty<uint> CacheSizeProperty = AvaloniaProperty.Register<DocumentTabControl, uint>(nameof(CacheSize), 5);

        public uint CacheSize
        {
            get { return GetValue(CacheSizeProperty); }
            set { SetValue(CacheSizeProperty, value); }
        }

        protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {

            }
        }

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);

            if (Items.Count() > 0)
            {
                SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Selects the content of a tab item.
        /// </summary>
        /// <param name="o">The tab item.</param>
        /// <returns>The content.</returns>
        private static object SelectContent(object o)
        {
            var content = o as IContentControl;

            if (content != null)
            {
                return content.Content;
            }
            else
            {
                return o;
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            var carousel = e.NameScope.Find<Carousel>("PART_Carousel");

            if (carousel != null)
            {
                carousel.MemberSelector = ContentSelector;
            }
        }
    }
}