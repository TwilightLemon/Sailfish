using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Sailfish
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<XL.DownTaskParam> DownloadDatas = new List<XL.DownTaskParam>();
        List<DownloadItems> DownloadControls = new List<DownloadItems>();
        public MainWindow()
        {
            InitializeComponent();
            T.Interval = 500;
            T.Tick += delegate {
                XL.XL_QueryTaskInfoEx(a, info);
                if (info.stat!=XL.DOWN_TASK_STATUS.TSC_COMPLETE)
                {
                    DownloadControls[0].pro.Value = (int)(info.fPercent * 100);
                    title.Text = "Sailfish   --" + HumanReadableFilesize(info.nSpeed)+"/s";
                }
                else
                {
                    T.Stop();
                    DownloadControls[0].pro.Value = DownloadControls[0].pro.Minimum;
                    a = new IntPtr(0);
                    DownloadControls[0].Size.Text = "已完成";
                    title.Text = "Sailfish";
                    DownloadDatas.RemoveAt(0);
                    DownloadControls.RemoveAt(0);
                    if (DownloadControls.Count >0) {
                        a = XL.XL_CreateTask(DownloadDatas[0]);
                        XL.XL_StartTask(a);
                        T.Start();
                        DownloadControls[0].bors.MouseDown += Bors_MouseDown;
                    }
                }
            };
        }
        public static long GetRemoteHTTPFileSize(string sURL)
        {
            long size = 0L;
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sURL);
                request.Method = "HEAD";
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                size = response.ContentLength;
                response.Close();
            }
            catch
            {
                size = 0L;
            }
            return size;
        }
        private String HumanReadableFilesize(double size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }
        System.Windows.Forms.Timer T = new System.Windows.Forms.Timer();
        private void url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DownloadPath.Text = "D://"+System.IO.Path.GetFileName(DownloadUrl.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XL.XL_Init();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XL.XL_UnInit();
        }
        XL.DownTaskInfo info = new XL.DownTaskInfo();
        private IntPtr a = new IntPtr(0);
        private void CLOSE_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var b = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
            b.Completed += delegate { Environment.Exit(0); };
            BeginAnimation(OpacityProperty, b);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (Resources["OnMouseDown2"] as Storyboard).Begin();
            grid.Visibility = Visibility.Collapsed;
            FileInfo f = new FileInfo(DownloadPath.Text);
            if (f.Exists)
                f.Delete();
            String FileName = f.Name;
            String SavePath = f.DirectoryName;
            String url = DownloadUrl.Text;
            DownloadItems dl = new DownloadItems();
            dl.Size.Text =HumanReadableFilesize(GetRemoteHTTPFileSize(url));
            dl.name.Text = FileName;
            dl.SetIcon(FileName);
            dl.Width = sv.ActualWidth;
            dl.str = DownloadPath.Text;
            wpList.Children.Add(dl);
            DownloadControls.Add(dl);
            DownloadUrl.Text = "";
            DownloadPath.Text = "";
            XL.DownTaskParam p = new XL.DownTaskParam()
            {
                szTaskUrl = url,
                szFilename =FileName,
                szSavePath = SavePath
            };
            DownloadDatas.Add(p);
            if (DownloadDatas.Count == 1) {
                a = XL.XL_CreateTask(p);
                XL.XL_StartTask(a);
                T.Start();
                dl.bors.MouseDown += Bors_MouseDown;
            }
        }
        int i = 0;
        private void Bors_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (i == 0){
                i = 1;
                XL.XL_StopTask(a);
            }
            else {
                i = 0;
                XL.XL_StartTask(a);
                T.Start();
            }
            DownloadControls[0].SetDrow(i);
        }
    }
}