namespace AvalonStudio
{
    using ReactiveUI;
    using System;

    public abstract class PaneViewModel : ReactiveObject
    {
        public PaneViewModel (string contentId)
        {
            this.ContentId = contentId;
        }

        #region Title
        private string title;
        public string ToolTitle
        {
            get { return title; }
            set { title = value; this.RaisePropertyChanged (); }
        }

        #endregion

        public virtual Uri IconSource
        {
            get;

            protected set;
        }

        #region ContentId

        private string contentId;
        public string ContentId
        {
            get { return contentId; }

            set { contentId = value; this.RaisePropertyChanged (); }
        }
        #endregion

        #region IsSelected
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; this.RaisePropertyChanged (); }
        }
        #endregion

        #region IsActive

        private bool isActive;
        public bool IsActive
        {
            get{return isActive;}

            set { isActive = value; this.RaisePropertyChanged (); }
        }
        #endregion

        //private Visibility visibility;
        //public Visibility Visibility
        //{
        //    get { return visibility; }
        //    set { visibility = value; this.RaisePropertyChanged(); }
        //}
       

        private bool isCollapsed;
        public bool IsCollapsed
        {
            get { return isCollapsed; }
            set { isCollapsed = value; this.RaisePropertyChanged(); }
        }

    }

}
