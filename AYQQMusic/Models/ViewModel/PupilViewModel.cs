using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AYQQMusic
{
    public class PupilViewModel : AyPropertyChanged
    {
        public string FullName { get; set; }

        private bool selected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    OnPropertyChanged("Selected");
                }

            }
        }
    }
}
