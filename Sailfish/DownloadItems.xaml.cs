using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sailfish
{
    /// <summary>
    /// DownloadItems.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadItems : UserControl
    {
        public DownloadItems()
        {
            InitializeComponent();
        }
        public void SetIcon(String filename) {
            icon.Source = ToImageSource(GetFileIcon(filename));
        }
        public string str = "";
        /// <summary>
        /// 设置暂停/继续状态
        /// </summary>
        /// <param name="i">0为暂停 1为继续</param>
        public void SetDrow(int i) {
            if (i == 0)
                p.Data = Geometry.Parse("M910.8 303.6c-5.4-10.5-16.3-17.8-28.9-17.8-17.8 0-32.2 14.4-32.2 32.1 0 6 1.7 11.7 4.6 16.5l-0.1 0.1c26.9 52.4 42.1 111.8 42.1 174.7 0 211.6-171.6 383.2-383.2 383.2S129.8 720.8 129.8 509.2 301.4 126.1 513 126.1c62.5 0 121.5 15 173.6 41.5l0.2-0.4c4.6 2.6 10 4.1 15.7 4.1 17.8 0 32.2-14.4 32.2-32.1 0-13.1-7.9-24.4-19.3-29.4C654.6 78.9 585.9 61.5 513 61.5 265.7 61.5 65.3 262 65.3 509.2S265.7 956.9 513 956.9s447.7-200.4 447.7-447.7c0-74.1-18-144-49.9-205.6z M385.4 352.2V672c0 17.5 14.3 31.9 31.9 31.9 17.6 0 32-14.4 31.9-31.9V352.2c0-17.5-14.3-31.9-31.9-31.9-17.5 0-31.9 14.3-31.9 31.9zM578.9 352.2V672c0 17.5 14.3 31.9 31.9 31.9 17.5 0 31.9-14.4 31.9-31.9V352.2c0-17.5-14.3-31.9-31.9-31.9-17.5 0-31.9 14.3-31.9 31.9z M772.7 217.7a32.2 32.1 0 1 0 64.4 0 32.2 32.1 0 1 0-64.4 0Z");
            else p.Data = Geometry.Parse("M512 0C229.228 0 0 229.228 0 512s229.228 512 512 512c282.773 0 512-229.228 512-512S794.772 0 512 0z m0 954.183C267.818 954.183 69.818 756.205 69.818 512c0-244.205 198-442.182 442.182-442.182 244.228 0 442.182 197.977 442.182 442.182S756.228 954.183 512 954.183z M750.545 481.772l-349.09-201.545a34.843 34.843 0 0 0-34.91 0 34.897 34.897 0 0 0-17.454 30.228v403.09c0 12.478 6.659 24 17.454 30.228A34.85 34.85 0 0 0 384 748.456a34.85 34.85 0 0 0 17.455-4.683l349.09-201.545A34.896 34.896 0 0 0 768 512c0-12.477-6.659-24-17.455-30.228zM418.909 653.079V370.92L663.272 512 418.909 653.079z");
        }
        public static Icon GetFileIcon(string fileName, bool largeIcon=true) { SHFILEINFO info = new SHFILEINFO(true); int cbFileInfo = Marshal.SizeOf(info); SHGFI flags; if (largeIcon) flags = SHGFI.Icon | SHGFI.LargeIcon | SHGFI.UseFileAttributes; else flags = SHGFI.Icon | SHGFI.SmallIcon | SHGFI.UseFileAttributes; SHGetFileInfo(fileName, 256, out info, (uint)cbFileInfo, flags); return Icon.FromHandle(info.hIcon); }
        [DllImport("Shell32.dll")] private static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);[StructLayout(LayoutKind.Sequential)] private struct SHFILEINFO { public SHFILEINFO(bool b) { hIcon = IntPtr.Zero; iIcon = 0; dwAttributes = 0; szDisplayName = ""; szTypeName = ""; } public IntPtr hIcon; public int iIcon; public uint dwAttributes;[MarshalAs(UnmanagedType.LPStr, SizeConst = 260)] public string szDisplayName;[MarshalAs(UnmanagedType.LPStr, SizeConst = 80)] public string szTypeName; }; private enum SHGFI { SmallIcon = 0x00000001, LargeIcon = 0x00000000, Icon = 0x00000100, DisplayName = 0x00000200, Typename = 0x00000400, SysIconIndex = 0x00004000, UseFileAttributes = 0x00000010 }
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return imageSource;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Size.Text == "已完成")
                Process.Start(str);
        }
    }
}