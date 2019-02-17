using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StringCoder
{
    public partial class MainWindow : Window
    {
        readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        readonly char[] alphabetUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        readonly string[] charsToRemove = new string[] { ",", ".", ";", ":", "@", "!", "#", "$", "%", "*", "(", ")", "[", "]", "^", "?", "|", "+", "-", "&", @"\", "/", "'" };
        private static int key = 0;
        int Xkey, Ytext;
        string current = null;
        string prev = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncodeText(object sender, RoutedEventArgs e)
        {
            // Caesar cipher
            if (Cipher.Text == "Caesar cipher" && tbKey.Text != "")
            {
                lbDecText.Content = "  Your Decoded text";
                Decode.Content = "Decode";
                key = Convert.ToInt32(tbKey.Text);
                if (EncodedText.Text != "")
                {
                    EncodedText.Clear();
                }
                try
                {
                    foreach (var item in charsToRemove)
                    {
                        YourText.Text = YourText.Text.Replace(item, string.Empty);
                    }

                    char[] text = YourText.Text.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();

                    for (int i = 0; i < text.Length; i++)
                    {
                        for (int j = 0; j <= alphabet.Length && j <= alphabetUpper.Length; j++)
                        {
                            if (text[i].ToString() == alphabet[j].ToString() || text[i].ToString() == alphabetUpper[j].ToString())
                            {
                                char.ToLower(text[i]);

                                //text[i] = Convert.ToChar(alphabet[(j + key) % alphabet.Length]);    optimize :D
                                EncodedText.Text += Convert.ToChar(alphabet[(j + key) % alphabet.Length]);
                                break;
                            }
                            //continue; ?? need ?
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            // HASHFUNC
            if (Cipher.Text == "HashFunc")
            {
                lbDecText.Content = "  Comparable hash";
                Decode.Content = "Check!";
                string text = YourText.Text;

                if (string.IsNullOrEmpty(text))
                {
                    text = string.Empty;
                }

                using (SHA512Managed sha = new SHA512Managed())
                {
                    byte[] textData = Encoding.Default.GetBytes(text);
                    byte[] hash = sha.ComputeHash(textData);
                    EncodedText.Text = BitConverter.ToString(hash).Replace("-", string.Empty);
                }
                current = EncodedText.Text;
            }

            //  Vigenere cipher
            if (Cipher.Text == "Vigenere cipher")
            {
                lbDecText.Content = "  Your Decoded text";
                Decode.Content = "Decode";
                if (EncodedText.Text != "")
                {
                    EncodedText.Clear();
                }
                char[] text = YourText.Text.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();
                char[] key = tbKey.Text.ToCharArray();
                try
                {
                    char[,] Vigenere_Table = new char[26, 26];

                    int temp = 0;
                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        for (int j = 0; j < 26; j++)
                        {
                            temp = j + i;
                            if (temp >= 26)
                            {
                                temp = temp % 26;
                            }
                            Vigenere_Table[i, j] = alphabet[temp];
                        }
                    }

                    for (int t = 0, k = 0; t < text.Length || k < key.Length; t++, k++)
                    {
                        if (t >= text.Length)
                        {
                            break;
                        }
                        if (k == key.Length/*t % key.Length == 0*/)
                        {
                            k = 0;
                            for (int y = 0; y <= alphabet.Length; y++)
                            {
                                if (text[t].ToString() == alphabet[y].ToString())
                                {
                                    Ytext = y;
                                    for (int x = 0; x <= alphabet.Length; x++)
                                    {
                                        if (key[k].ToString() == alphabet[x].ToString())
                                        {
                                            Xkey = x;
                                            EncodedText.Text += Vigenere_Table[Ytext, Xkey].ToString();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int y = 0; y <= alphabet.Length; y++)
                            {
                                if (text[t].ToString() == alphabet[y].ToString())
                                {
                                    Ytext = y;
                                    for (int x = 0; x <= alphabet.Length; x++)
                                    {
                                        if (key[k].ToString() == alphabet[x].ToString())
                                        {
                                            Xkey = x;
                                            EncodedText.Text += Vigenere_Table[Ytext, Xkey].ToString();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DecodeText(object sender, RoutedEventArgs e)
        {
            // Caesar cipher 
            if (Cipher.Text == "Caesar cipher")
            {
                if (DecodedText.Text != "")
                {
                    DecodedText.Clear();
                }
                try
                {
                    int temp = 0;
                    char[] text = EncodedText.Text.ToCharArray();
                    for (int i = 0; i < text.Length; i++)
                    {
                        for (int j = 0; j <= alphabet.Length; j++)
                        {
                            if (text[i].ToString() == alphabet[j].ToString())
                            {
                                temp = j - key;

                                if (temp < 0)
                                    temp += alphabet.Length;
                                if (temp >= alphabet.Length)
                                    temp -= alphabet.Length;

                                DecodedText.Text += alphabet[temp];
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

            // HASHFUNC
            if (Cipher.Text == "HashFunc")
            {
                if (DecodedText.Text == "")
                {
                    DecodedText.Text = EncodedText.Text;
                }
                else
                {
                    prev = DecodedText.Text;
                    if (current == prev)
                    {
                        MessageBox.Show("Hashas is concurrence!");
                        prev = current;
                        tbTextfromDecrHash.Text = YourText.Text;
                    }
                    else
                    {
                        MessageBox.Show("Hashas is not concurrence!");
                    }
                }
            }
        }

        //  Vigenere cipher
        private char[,] VigenereCipher()
        {
            char[,] Virenege_Table = new char[26, 26];

            int temp = 0;
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    temp = j + i;
                    if (temp >= 26)
                    {
                        temp = temp % 26;
                    }
                    Virenege_Table[i, j] = alphabet[temp];
                    //Console.Write(" " + Virenege_Table[i, j]);
                }
                //Console.WriteLine();
            }
            ////test of table
            //Console.WriteLine(alf_Virenege[0, 0]);
            return Virenege_Table;
        }

        // events
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Cipher.SelectedIndex == 2)
            {
                TextBox textBox = sender as TextBox;
                e.Handled = Regex.IsMatch(e.Text, "[^a-z.]+");
            }
            else
            {
                TextBox textBox = sender as TextBox;
                e.Handled = Regex.IsMatch(e.Text, "[^0-9.]+");
            }
        }

        private void Cipher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EncodedText.Clear();
            DecodedText.Clear();
            tbTextfromDecrHash.Clear();
            tbKey.Clear();
            if (Cipher.SelectedIndex == 0 || Cipher.SelectedIndex == 2)
            {
                tbKey.IsHitTestVisible = true;
            }
            else
            {
                tbKey.IsHitTestVisible = false;
                tbKey.Clear();
            }
        }
    }
}
