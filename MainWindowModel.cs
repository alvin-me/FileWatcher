using System;
using System.Collections.Generic;

namespace FileWatcher
{
    public class MainWindowModel
    {
        public static Dictionary<string, string> FileTypeDict = new Dictionary<string, string>()
            {
                {"位图(*.bmp;*.dib)", "*.bmp;*.dib"},
                {"JPEG(*.jpg;*.jpeg;*.jpe;*.jfif)", "*.jpg;*.jpeg;*.jpe;*.jfif"},
                {"GIF(*.jif)", "*.jif"},
                {"TIFF(*.tif;*.tiff)", "*.tif;*.tiff"},
                {"PNG(*.png)", "*.png"},
                {"全部图像文件", "*.bmp;*.dib;*.jpg;*.jpeg;*.jpe;*.jfif;*.jif;*.tif;*.tiff;*.png"},
                {"全部文件", "*.*"},
            };
    }
}
