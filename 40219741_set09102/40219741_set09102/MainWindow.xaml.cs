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
using System.Text.RegularExpressions;
using System.Net.Mail;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace _40219741_set09102
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Regex checksms = new Regex(@"\+\d{11,15}");
        Regex checkTweet = new Regex(@"^@\d{0,15}");
        Regex hashtag = new Regex(@"^#");
        SMS smsn = new SMS();
        Tweet tweetn = new Tweet();
        Email emailn = new Email();

        public MainWindow()
        {
            InitializeComponent();
            DirectoryInfo dinfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                listMessages.Items.Add(file.Name);
                if (file.Name.StartsWith("T"))
                {
                    
                    /*using (StreamReader files = File.OpenText(file.Name))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            Tweet readtweet = (Tweet)serializer.Deserialize(files, typeof(Tweet));
                            
                        }
                    */
                }
            }
        }

        private void txtSender_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (!(string.IsNullOrWhiteSpace(txtSender.Text)))
            {
                
                if (checksms.IsMatch(txtSender.Text))
                {
                    lblMessageType.Content = "SMS";
                    lblCharcLimit.Content = "140 characters Max.";
                }
                if (checkTweet.IsMatch(txtSender.Text))
                {
                    lblMessageType.Content = "Twitter ID";
                    lblCharcLimit.Content = "140 characters Max.";
                }
                
                try
                {
                    MailAddress checkemail = new MailAddress(txtSender.Text);
                    lblMessageType.Content = "E-mail";
                    if (lblMessageType.Content.Equals("E-mail"))
                    {
                        txtSubject.IsEnabled = true;
                        txtMessage.IsEnabled = true;
                    }
                    lblCharcLimit.Content = "1028 characters Max.";

                }
                catch (FormatException)
                {
                    
                }
            }
            else
            {
                lblMessageType.Content = " ";
                lblCharcLimit.Content = " ";
            }
            
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSender.Clear();
            txtMessageID.Clear();
            txtMessage.Clear();
            txtSubject.Clear();
            txtMessageID.IsEnabled = false;
            txtMessage.IsEnabled = false;
            txtSubject.IsEnabled = false;
        }

        private void btnSender_Click(object sender, RoutedEventArgs e)
        {
            Random mID = new Random();
            txtMessageID.IsEnabled = true;
                 
            if (!(string.IsNullOrWhiteSpace(txtSender.Text)))
            {
                if (checksms.IsMatch(txtSender.Text))
                {
                    txtSubject.IsEnabled = false;
                    txtMessage.IsEnabled = true;
                    int range = mID.Next(000000000, 999999999);
                    lblMessageType.Content = "SMS";
                    txtMessage.MaxLength = 140;
                    lblCharcLimit.Content = "140 characters Max.";
                    txtMessageID.Text = "S" + range;
                    smsn.SenderID = txtSender.Text;
                    smsn.MessageTxt = txtMessage.Text;
                    
                    smsn.MessageID = txtMessageID.Text;
                    MessageBox.Show(smsn.MessageTxt);

                }
                if (checkTweet.IsMatch(txtSender.Text))
                {
                    txtSubject.IsEnabled = false;
                    txtMessage.IsEnabled = true;
                    int range = mID.Next(000000000, 999999999);
                    lblMessageType.Content = "Twitter ID";
                    txtMessage.MaxLength = 140;
                    lblCharcLimit.Content = "140 characters Max.";
                    txtMessageID.Text = "T" + range;
                    tweetn.SenderID = txtSender.Text;
                    tweetn.MessageTxt = txtMessage.Text;
                    foreach (Match match in hashtag.Matches(tweetn.MessageTxt))
                    {
                        MessageBox.Show(match.Value);
                    }
                    tweetn.MessageID = txtMessageID.Text;
                }

                try
                {
                    
                    int range = mID.Next(000000000, 999999999);
                    MailAddress checkemail = new MailAddress(txtSender.Text);
                    lblMessageType.Content = "E-mail";
                    txtMessage.MaxLength = 1028;
                    lblCharcLimit.Content = "1028 characters Max.";
                    txtMessageID.Text = "E" + range;
                    
                    emailn.SenderID = txtSender.Text;
                    emailn.Subject = txtSubject.Text;
                    emailn.MessageTxt = txtMessage.Text;
                    emailn.MessageID = txtMessageID.Text;

                }
                catch (FormatException)
                {

                }
                //C:\Users\alext\source\repos\40219741_set09102
                
            }
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSave_Click(object sndr, RoutedEventArgs e)
        {
            if(lblMessageType.Content.Equals("SMS"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + smsn.MessageID + ".txt", JsonConvert.SerializeObject(smsn));
                listMessages.Items.Add(smsn.MessageID + ".txt");
            }
            if (lblMessageType.Content.Equals("Twitter ID"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + tweetn.MessageID + ".txt", JsonConvert.SerializeObject(tweetn));
                listMessages.Items.Add(tweetn.MessageID + ".txt");
            }
            if (lblMessageType.Content.Equals("E-mail"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + emailn.MessageID + ".txt", JsonConvert.SerializeObject(emailn));
                listMessages.Items.Add(emailn.MessageID + ".txt");
            }

        }

        private void listMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedfile = listMessages.SelectedItem.ToString();
            string fullFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedfile);
            Process.Start(fullFileName);
        }
    }
}
