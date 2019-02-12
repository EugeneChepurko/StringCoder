using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] textData = Encoding.Default.GetBytes(text);
                    byte[] hash = sha.ComputeHash(textData);
                    EncodedText.Text = BitConverter.ToString(hash).Replace("-", string.Empty); 
                }
                current = EncodedText.Text;
            }

            //  Vigenere cipher
            if(Cipher.Text == "Vigenere cipher")
            {
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
                    for (int i = 0; i < text.Length; i++)
                    {
                        for (int j = 0; j <= alphabet.Length; j++)
                        {
                            for (int k = 0; k <= key.Length; k++)
                            {
                                //1st variant
                                if (text[i].ToString() == alphabet[j].ToString() && key[k].ToString() == alphabet[j].ToString())
                                {

                                    text[i] = key[k];

                                    EncodedText.Text += key[k];
                                    break;
                                }
                                continue;
                                //2nd variant
                                if (text[i].ToString() == Vigenere_Table[i, 0].ToString() && key[i].ToString() == Vigenere_Table[0, i].ToString())
                                {

                                }
                            }
                                                     
                        }
                        //i = Vigenere_Table[0, 0];
                    }



                    //for (int i = 0; i < text.Length; i++)
                    //{
                    //    for (int j = 0; j < VigenereCipher().Length; j++)
                    //    {
                    //        if(text[i] == )
                    //    }
                    //}
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
            if(Cipher.SelectedIndex == 2)
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
