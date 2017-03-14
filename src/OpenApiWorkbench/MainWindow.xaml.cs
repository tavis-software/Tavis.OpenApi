using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Tavis.OpenApi;

namespace OpenApiWorkbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            txtErrors.Text = "";
            txtInput.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtErrors.Text = "";
            OpenApiParser openApiParser = new OpenApiParser();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(this.txtInput.Text);
            writer.Flush();
            stream.Position = 0;
            var doc = openApiParser.Parse(stream);
            
            if (openApiParser.ParseErrors.Count == 0)
            {
                txtErrors.Text = "OK";

            }
            else
            {
                var errorReport = new StringBuilder();
                foreach (var error in openApiParser.ParseErrors)
                {
                    errorReport.AppendLine(error.ToString());
                }
                txtErrors.Text = errorReport.ToString();
            }
            var outputwriter = new OpenApiV3Writer(doc);
            var outputstream = new MemoryStream();
            outputwriter.Writer(outputstream);
            outputstream.Position = 0;
            txtOutput.Text = new StreamReader(outputstream).ReadToEnd();

        }
    }
}
