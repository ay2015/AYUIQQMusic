using System.IO;
using System.Windows;

namespace AYQQMusic.Models
{
    public class ExtendUtils
    {
        #region << 主窗体句柄
        public static Window WHD = Application.Current.MainWindow;
        #endregion 

        #region << 主屏幕宽
        public static double SCREEN_WIDTH = SystemParameters.PrimaryScreenWidth;
        #endregion

        #region << 主屏幕高
        public static double SCREEN_HEIGHT = SystemParameters.PrimaryScreenHeight;
        #endregion

        #region << 执行程序根目录
        public static string EXECUTE_PATH = Directory.GetCurrentDirectory() + @"\";
        #endregion

        #region<<资源文件夹
        public static string DATA_PATH = EXECUTE_PATH + @"Contents\data\";
        public static string DATA_PATH_TEMP = EXECUTE_PATH + @"Contents\data\TEMP";
        public static string DATA_PATH_ALBUM_TEMP = EXECUTE_PATH + @"Contents\data\TEMP\ALBUM";
        #endregion
        #region<< 播放列表文件名称
        public static string musiclst = ExtendUtils.DATA_PATH + "musiclst.ayl";

        #endregion

        public const string CurrentPlayDbName = "aylist.db";

        public static string GetDATA_PATH()
        {
            if (!System.IO.Directory.Exists(DATA_PATH))
            {
                System.IO.Directory.CreateDirectory(DATA_PATH);
            }
            return DATA_PATH;
        }
        public static string GetDATA_TEMP_PATH()
        {
            if (!System.IO.Directory.Exists(DATA_PATH_TEMP))
            {
                System.IO.Directory.CreateDirectory(DATA_PATH_TEMP);
            }
            return DATA_PATH_TEMP;
        }
        public static string GetDATA_TEMP_ALBUM_PATH()
        {
            if (!System.IO.Directory.Exists(DATA_PATH_ALBUM_TEMP))
            {
                System.IO.Directory.CreateDirectory(DATA_PATH_ALBUM_TEMP);
            }
            return DATA_PATH_ALBUM_TEMP;
        }

        //public static Cursor curDragInsert = new Cursor(ExtendUtils.EXECUTE_PATH + "Contents/Cur/1064.cur");

        //public static Cursor curDrag = new Cursor(ExtendUtils.EXECUTE_PATH + "Contents/Cur/1066.cur");
        //public static Cursor curDragNone = new Cursor(ExtendUtils.EXECUTE_PATH + "Contents/Cur/1069.cur");
    }
}
