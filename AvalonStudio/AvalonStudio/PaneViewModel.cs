using Perspex.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AvalonStudio.MVVM;

namespace AvalonStudio
{
    public abstract class PaneViewModel : ViewModelBase
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
            set { title = value; OnPropertyChanged (); }
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

            set { contentId = value; OnPropertyChanged (); }
        }
        #endregion

        #region IsSelected
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged (); }
        }
        #endregion

        #region IsActive

        private bool isActive;
        public bool IsActive
        {
            get{return isActive;}

            set { isActive = value; OnPropertyChanged (); }
        }
        #endregion

        //private Visibility visibility;
        //public Visibility Visibility
        //{
        //    get { return visibility; }
        //    set { visibility = value; OnPropertyChanged(); }
        //}
       

        private bool isCollapsed;
        public bool IsCollapsed
        {
            get { return isCollapsed; }
            set { isCollapsed = value; OnPropertyChanged(); }
        }

    }

}
