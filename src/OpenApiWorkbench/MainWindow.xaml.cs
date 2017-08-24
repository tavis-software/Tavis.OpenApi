using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                txtErrors.Text = "";
                OpenApiParser openApiParser = new OpenApiParser();
                MemoryStream stream = CreateStream(this.txtInput.Text);


                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var doc = openApiParser.Parse(stream);
                stopwatch.Stop();
                this.txtParseTime.Text = $"{stopwatch.ElapsedMilliseconds} ms";

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

                stopwatch.Reset();
                stopwatch.Start();
                txtOutput.Text = WriteContents(doc);
                stopwatch.Stop();

                this.txtRenderTime.Text = $"{stopwatch.ElapsedMilliseconds} ms";
            }
            catch (Exception ex)
            {
                txtErrors.Text = "Failed to parse input: " + ex.Message;
            }
        }

        private string WriteContents(Tavis.OpenApi.Model.OpenApiDocument doc)
        {
            var outputwriter = new OpenApiV3Writer(doc);
            var outputstream = new MemoryStream();
            outputwriter.Write(outputstream);
            outputstream.Position = 0;

            return new StreamReader(outputstream).ReadToEnd();
        }

        private MemoryStream CreateStream(string text)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
