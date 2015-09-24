using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileWatcher
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            InitializeSettingFromConfigFile();

            this.btnSetWatchFolder.Click += new RoutedEventHandler(setWatchFolder);
            this.btnSetLogFolder.Click += new RoutedEventHandler(setLogFolder);
        }

        private void InitializeSettingFromConfigFile()
        {
            this.txtWatchFolder.Text = Properties.Settings.Default.WatchFolder;
            this.txtLogFolder.Text = Properties.Settings.Default.LogFolder;
            this.ckWatchSubDir.IsChecked = Properties.Settings.Default.WatchSubDir;

            foreach(var item in MainWindowModel.FileTypeDict)
            {
                this.cmbWatchFileType.Items.Add(item.Key);
            }
            int idx = Properties.Settings.Default.WatchFileType;
            if (idx >= 0 && idx < MainWindowModel.FileTypeDict.Count)
                this.cmbWatchFileType.SelectedIndex = idx;
            else
                this.cmbWatchFileType.SelectedIndex = MainWindowModel.FileTypeDict.Count - 1;
        }

        private void saveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WatchFolder = this.txtWatchFolder.Text;
            Properties.Settings.Default.LogFolder = this.txtLogFolder.Text;
            Properties.Settings.Default.WatchSubDir = (this.ckWatchSubDir.IsChecked == true);
            Properties.Settings.Default.WatchFileType = this.cmbWatchFileType.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if ((Properties.Settings.Default.WatchFolder != this.txtWatchFolder.Text)
                || (Properties.Settings.Default.LogFolder != this.txtLogFolder.Text)
                || (Properties.Settings.Default.WatchSubDir != (this.ckWatchSubDir.IsChecked == true))
                || (Properties.Settings.Default.WatchFileType != this.cmbWatchFileType.SelectedIndex))
            {
                saveSettings(null, null);
            }
            base.OnClosing(e);
        }

        void setWatchFolder(object sender, RoutedEventArgs e)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.Description = "请选择要监控的文件目录:";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtWatchFolder.Text = fbd.SelectedPath;
            }
        }

        void setLogFolder(object sender, RoutedEventArgs e)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.Description = "请选择日志保存目录:";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtLogFolder.Text = fbd.SelectedPath;
            }
        }

    }
}
