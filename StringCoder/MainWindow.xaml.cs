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
        readonly char[] alphabetUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        int key = 1;

        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void EncodeText(object sender, RoutedEventArgs e)
        {
            if (EncodedText.Text != "")
            {
                EncodedText.Clear();
            }

            try
            {
                char[] text = YourText.Text.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();
                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j <= alphabet.Length && j <= alphabetUpper.Length; j++)
                    {
                        if (text[i].ToString() == alphabet[j].ToString() || text[i].ToString() == alphabetUpper[j].ToString())
                        {
                            char.ToLower(text[i]);

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
            if (DecodedText.Text != "")
            {
                DecodedText.Clear();
            }
            try
            {
                char[] text = EncodedText.Text.ToCharArray();
                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j <= alphabet.Length && j <= alphabetUpper.Length; j++)
                    {
                        if (text[i].ToString() == alphabet[j].ToString())
                        {
                            text[i] = Convert.ToChar(alphabet[(j - key) % alphabet.Length]);
                            DecodedText.Text += text[i].ToString();
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
    }
}
