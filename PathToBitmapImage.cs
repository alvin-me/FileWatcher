using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;
using System.Windows;

namespace FileWatcher
{
    /// <summary>
    /// Disposing a WPF Image or BitmapImage so the Source picture file can be modified.
    /// from http://stackoverflow.com/questions/690150/delete-an-image-bound-to-a-control
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class PathToBitmapImage : IValueConverter
    {
        public static object ConvertToImage(string path)
        {
            if (!File.Exists(path))
                return "/FileWatcher;component/Resources/not_found.png";

            BitmapImage bitmapImage = null;
            bool result = true;
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    // check image or not
                    string ext = (Path.GetExtension(path) ?? string.Empty).ToLower();
                    string value = FileType.Dict["全部图像文件"];
                    string[] imgExts = value.Split(';');
                    if (!imgExts.Any(ext.Equals))
                        throw new NotSupportedException();

                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.StreamSource.Dispose();
                }
                catch (IOException)
                {
                    result = false;
                }
                catch (NotSupportedException)
                {
                    result = false;
                }
            }));

            if (result)
                return bitmapImage;
            else
                return "/FileWatcher;component/Resources/not_image.png";
        }

        #region IValueConverter Members

        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is string))
                return null;

            var path = value as string;

            return ConvertToImage(path);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
