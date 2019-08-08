using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Collections;
using Ay.Framework.WPF.Controls;
using Ay.Framework.WPF;

namespace AYQQMusic
{
    public class MainViewModel : IAyDropTarget,IAyDragSource
    {
        public MainViewModel()
        {
            ObservableCollection<SchoolViewModel> schools = new ObservableCollection<SchoolViewModel>();

            schools.Add(new SchoolViewModel
            {
                Name = "AY1",
                Pupils = new ObservableCollection<PupilViewModel>
                {
                    new PupilViewModel { FullName = "Ay1111" },
                    new PupilViewModel { FullName = "Ay1112" },
                    new PupilViewModel { FullName = "Ay1113" },
                    new PupilViewModel { FullName = "Ay1114" }
                }
            });
            ILove = new SchoolViewModel
            {
                Name = "我喜欢的歌曲",
                Pupils = new ObservableCollection<PupilViewModel>
                {
                     new PupilViewModel { FullName = "好孩子就是ay" },
                     new PupilViewModel { FullName = "看着都好笑" },
                     new PupilViewModel { FullName = "来来就不要走" },
                     new PupilViewModel { FullName = "测试.mp4" }
                }
            };
            schools.Add(ILove);

            schools.Add(new SchoolViewModel
            {
                Name = "AY3",
                Pupils = new ObservableCollection<PupilViewModel>
                {
                    new PupilViewModel { FullName = "AY2016-05-20" },
                    new PupilViewModel { FullName = "A" },
                    new PupilViewModel { FullName = "B" },
                    new PupilViewModel { FullName = "C" },
                    new PupilViewModel { FullName = "D" },
                    new PupilViewModel { FullName = "E" } ,  new PupilViewModel { FullName = "F" },
                    new PupilViewModel { FullName = "G" },
                    new PupilViewModel { FullName = "H" },
                    new PupilViewModel { FullName = "I" },
                    new PupilViewModel { FullName = "J" },  new PupilViewModel { FullName = "K" },
                    new PupilViewModel { FullName = "L" },
                    new PupilViewModel { FullName = "M" },
                    new PupilViewModel { FullName = "N" },
                    new PupilViewModel { FullName = "O" } ,  new PupilViewModel { FullName = "P" },
                    new PupilViewModel { FullName = "Q" },
                    new PupilViewModel { FullName = "R" },
                    new PupilViewModel { FullName = "S" },
                    new PupilViewModel { FullName = "T" }
                }
            });


            Schools = CollectionViewSource.GetDefaultView(schools);
        }

        void IAyDropTarget.DragOver(AyDropInfo dropInfo)
        {
            if (dropInfo.Data is PupilViewModel && dropInfo.TargetItem is SchoolViewModel)
            {
                dropInfo.DropTargetAdorner = AyDropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }
  


        void IAyDropTarget.Drop(AyDropInfo dropInfo)
        {
            SchoolViewModel school = (SchoolViewModel)dropInfo.TargetItem;
            PupilViewModel pupil = (PupilViewModel)dropInfo.Data;
            school.Pupils.Add(pupil);
    
            ((IList)dropInfo.DragInfo.SourceCollection).Remove(pupil);
        }

        public void StartDrag(AyDragInfo dragInfo)
        {
            int itemCount = dragInfo.SourceItems.Cast<object>().Count();

            if (itemCount == 1)
            {
                dragInfo.Data = dragInfo.SourceItems.Cast<object>().First();
            }
            else if (itemCount > 1)
            {
                dragInfo.Data = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems);
            }

            dragInfo.Effects = (dragInfo.Data != null) ?
                DragDropEffects.Copy | DragDropEffects.Move :
                DragDropEffects.None;
        }

        public void DoubleClickItem(AyDragInfo dragInfo)
        {
            PupilViewModel pupil = dragInfo.SourceItem as PupilViewModel;
            MessageBox.Show("播放歌曲:"+ pupil.FullName);
        }
        public SchoolViewModel ILove { get; set; }
        public ICollectionView Schools { get; private set; }
    }

  
}
