using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LineCodeCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] fileextentions;
        string[] excludefilewithname;
        string startfolder;
        int _linescount;

        public MainWindow()
        {
            InitializeComponent();
            fileextentions = new string[] {
#region DotNet
                ".cs",
                ".vb",
#endregion
#region C, C++
                ".h",".cpp",".hpp",".c",".ld",".h++",
                ".cc",".cxx","hxx",
#endregion
#region ASP
                ".razor",
                ".html",
                ".htm",
                ".js",
                ".cshtml",
                ".vbhtml",
                ".sql",
                ".aspx",
                ".ascx",
                ".master",
                ".ashx",
                ".php",
                ".ts",
                ".tsx",
                ".jsx",
                ".java",
                ".css",
                ".scss",
                ".less",
#endregion
#region F#
                ".fs",
                ".fsi",
                ".fsx",
                ".fsscript",
#endregion

#region Assembly
                ".asm",".s",
#endregion

#region Ruby
                ".rb",".rbw",
#endregion

#region Rust
                ".rs"
#endregion

            };
            excludefilewithname = new string[] { ".Designer", ".NETFramework", ".g.i", ".g" };
        }

        private void OpenFolderClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    startfolder = dialog.SelectedPath;
                    textBox.Text = startfolder;
                }
            }
        }

        private void CountLines(object sender, RoutedEventArgs e)
        {
            _linescount = 0;
            listView.Items.Clear();
            CountLinesFiles(startfolder);
        }

        private void CountLinesFiles(string startfolder)
        {
            foreach (var file in Directory.GetFiles(startfolder))
            {
                var name = System.IO.Path.GetFileName(file);
                bool exsist = false;
                foreach (var ex in fileextentions)
                {
                    exsist = System.IO.Path.GetExtension(name) == ex && CheckName(name);
                    if (exsist) break;
                }
                if (exsist)
                {
                    var g = File.ReadAllLines(file).Length;
                    _linescount += g;
                    listView.Items.Add(new ListViewItem() { Content = string.Format("File: {0}, Lines:{1}", name, g) });
                }
                disp.Content = string.Format("Code Lines:{0} Files:{1}", _linescount, listView.Items.Count);
            }
            foreach (var g in Directory.GetDirectories(startfolder))
                CountLinesFiles(g);
        }

        private bool CheckName(string name)
        {
            bool valid = false;
            foreach (var remove in excludefilewithname)
            {
                valid = !name.Contains(remove);
                if (!valid) return valid;
            }
            return valid;
        }
    }

    internal class FileLineCount
    {
        string _name;
        int _lines;

        public FileLineCount(string name, int lines)
        {
            _name = name;
            _lines = lines;
        }

        public override string ToString()
        {
            return string.Format("File: {0}, Lines:{1}", _name, _lines);
        }
    }
}
