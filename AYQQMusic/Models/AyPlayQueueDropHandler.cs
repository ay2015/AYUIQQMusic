using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using Ay.Framework.WPF.Controls;
using Ay.Framework.WPF;
using System.Text;
using System.Windows.Input;
using AYQQMusic.Models;

namespace AYQQMusic
{
    public class AyPlayQueueDropHandler : AyDefaultDropHandler
    {
        public override void DragOver(AyDropInfo dropInfo)
        {
            base.DragOver(dropInfo); 
        }
        public override void Drop(AyDropInfo dropInfo)
        {
            //IList ChuShiList = GetList(dropInfo.TargetCollection);
            //StringBuilder chuSb = new StringBuilder();
            //foreach (var item in ChuShiList)
            //{
            //    PupilViewModel dd = item as PupilViewModel;
            //    if (dd != null) {
            //        chuSb.Append(dd.FullName+",");
            //    }
            //}
            base.Drop(dropInfo);
            //IList destinationList = GetList(dropInfo.TargetCollection);
            //StringBuilder endSb = new StringBuilder();

            dropInfo.DragInfo.VisualSource.GiveFeedback -= MusicMainWindowModel.handler22;
            MainWindow.SaveLocalPlayData();
            //foreach (var item in destinationList)
            //{
            //    PupilViewModel dd = item as PupilViewModel;
            //    if (dd != null)
            //    {
            //        endSb.Append(dd.FullName + ",");
            //    }
            //}
            //if (chuSb.ToString() != endSb.ToString())
            //{
            //    MessageBox.Show("拖放前顺序:" + chuSb.ToString() + "\n拖放后顺序：" + endSb.ToString());
            //}

        }
    }
}
