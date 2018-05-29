using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
namespace DNA
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class Client : Window
    {
        public Client()
        {
            InitializeComponent();
        }

        public void open_file(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.ShowDialog();
            Stream fileContent = dialog.OpenFile();
            byte[] dna_sequence; 
            using (fileContent) {
                dna_sequence = new byte[fileContent.Length];
                fileContent.ReadAsync(dna_sequence, 0, (int)fileContent.Length);
                dna.Text = System.Text.Encoding.UTF8.GetString(dna_sequence);

            }



        }
    }
}
