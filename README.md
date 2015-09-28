# 文件系统监听

FileWatcher 是使用 WPF 编写的文件系统监听程序，主要是通过`System.IO.FileSystemWatcher`接口实现的。

## 截图

![filewatcher-screenshot](https://cloud.githubusercontent.com/assets/14179733/10129144/882b2d76-65ed-11e5-8411-844bff8d0d7f.png)

## 更新日志

1. 程序使用了 Model-View-ViewModel 架构；
2. 支持监听指定文件类型；
3. 实现增强的 FileSystemWatcher，以解决 FileSystemWatcher 被多次触发的问题；
4. 支持图像预览功能，并处理 WPF BitmapImage，确保在图像预览期间该文件可被修改；
5. 添加 ListViewItem 右键菜单功能；
6. 添加监听列表信息保存为 html 格式文件。