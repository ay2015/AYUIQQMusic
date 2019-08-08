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

namespace AYQQMusic
{
    public class AyHorizonDropHandler : AyDefaultDropHandler
    {
        public override void Drop(AyDropInfo dropInfo)
        {
            IList ChuShiList = GetList(dropInfo.TargetCollection);
            StringBuilder chuSb = new StringBuilder();
            foreach (var item in ChuShiList)
            {
                PupilViewModel dd = item as PupilViewModel;
                if (dd != null) {
                    chuSb.Append(dd.FullName+",");
                }
            }
            base.Drop(dropInfo);
            IList destinationList = GetList(dropInfo.TargetCollection);
            StringBuilder endSb = new StringBuilder();
            foreach (var item in destinationList)
            {
                PupilViewModel dd = item as PupilViewModel;
                if (dd != null)
                {
                    endSb.Append(dd.FullName + ",");
                }
            }
            if (chuSb.ToString() != endSb.ToString())
            {
                MessageBox.Show("拖放前顺序:" + chuSb.ToString() + "\n拖放后顺序：" + endSb.ToString());
            }
           
        }
    }
}
