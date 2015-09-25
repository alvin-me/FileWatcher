using System;
using System.Collections.Generic;

namespace FileWatcher
{
    public class FileType
    {
        public static Dictionary<string, string> Dict = new Dictionary<string, string>()
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

    public class FileInformation
    {
        /// <summary>
        /// Gets or Sets Unique integer ID for the File
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets Watching date time of the File
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Gets or Sets Watching event type of the File
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or Sets Watching File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets Watching File path
        /// </summary>
        public string FilePath { get; set; }
    }
}
