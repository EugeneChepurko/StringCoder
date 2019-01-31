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

namespace StringCoder
{
    public partial class MainWindow : Window
    {
        readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncodeText(object sender, RoutedEventArgs e)
        {
            //YourText.Text.Replace(" ", null);
            if (EncodedText.Text != "")
            {
                EncodedText.Clear();
            }

            int key = 15;
            try
            {
                char[] text = YourText.Text.ToCharArray().Where(s => !Char.IsWhiteSpace(s)).ToArray();
                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j <= alphabet.Length; j++)
                    {
                        if (text[i].ToString() == alphabet[j].ToString())
                        {
                            text[i] = Convert.ToChar(alphabet[(j + key) % alphabet.Length]);
                            EncodedText.Text += text[i].ToString();
                            break;
                        }
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DecodeText(object sender, RoutedEventArgs e)
        {

        }
    }
}
