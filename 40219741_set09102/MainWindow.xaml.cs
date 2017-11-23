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
        Regex hashtag = new Regex(@"#\w+");
        Regex mention = new Regex(@"@\w+");
        Regex url = new Regex(@"www.\w");
        SMS smsn = new SMS();
        Tweet tweetn = new Tweet();
        Email emailn = new Email();
        List <string> htlist=new List<string>();
        List<string> displayH = new List<string>();
        List<string> mlist = new List<string>();
        List<string> displayM = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            DirectoryInfo dinfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                listMessages.Items.Add(file.Name);
               
            }
        }

        private void txtSender_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (!(string.IsNullOrWhiteSpace(txtSender.Text)))
            {
                
                if (checksms.IsMatch(txtSender.Text) & checkMessegeSMS(lblMessageType.Content.ToString()) == true)
                {
                    lblMessageType.Content = "SMS";
                    lblCharcLimit.Content = "140 characters Max.";
                    
                }
                if (checkTweet.IsMatch(txtSender.Text) & checkMessegeTweet(lblMessageType.Content.ToString()) == true)
                {
                    lblMessageType.Content = "Twitter ID";
                    lblCharcLimit.Content = "140 characters Max.";
                }
                
                try
                {
                    MailAddress checkemail = new MailAddress(txtSender.Text);
                    lblMessageType.Content = "E-mail";
                    if (lblMessageType.Content.Equals("E-mail") & checkMessegeEmail(lblMessageType.Content.ToString()) == true)
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
        }
        public void messageIds()
        {
            Random mID = new Random();
            int range = mID.Next(000000000, 999999999);
            int res = checkMessageID(lblMessageType.Content.ToString(), range);
            if(res == 1 & (!(string.IsNullOrWhiteSpace(txtMessage.Text))))
            {
                txtMessageID.Text = "T" + range;
                smsn.MessageID = txtMessageID.Text;
            }
            if (res == 2 & (!(string.IsNullOrWhiteSpace(txtMessage.Text))))
            {
                txtMessageID.Text = "S" + range;
                tweetn.MessageID = txtMessageID.Text;
            }
            if(res == 3 && (!(string.IsNullOrWhiteSpace(txtMessage.Text))))
            {
                
                txtMessageID.Text = "E" + range;
            }
        }
        private void btnSender_Click(object sender, RoutedEventArgs e)
        {
            
            txtMessageID.IsEnabled = true;
                 
            if (!(string.IsNullOrWhiteSpace(txtSender.Text)))
            {

                if (checksms.IsMatch(txtSender.Text))
                {
                    txtSubject.IsEnabled = false;
                    txtMessage.IsEnabled = true;
                    
                    lblMessageType.Content = "SMS";
                    txtMessage.MaxLength = 140;
                    lblCharcLimit.Content = "140 characters Max.";
                    smsn.SenderID = txtSender.Text;
                    messageIds();
                    smsn.MessageTxt = txtMessage.Text;
                    smsn.MessageID = txtMessageID.Text;
                }
                if (checkTweet.IsMatch(txtSender.Text))
                {
                    txtSubject.IsEnabled = false;
                    txtMessage.IsEnabled = true;
                    lblMessageType.Content = "Twitter ID";
                    txtMessage.MaxLength = 140;
                    lblCharcLimit.Content = "140 characters Max.";
                    tweetn.SenderID = txtSender.Text;
                    messageIds();
                    tweetn.MessageTxt = txtMessage.Text;
                    tweetn.MessageID = txtMessageID.Text;
                    foreach (Match h in hashtag.Matches(txtMessage.Text))
                    {

                        htlist.Add(h.Value);
                    }
                    foreach (Match m in mention.Matches(txtMessage.Text))
                    {
                        mlist.Add(m.Value);

                    }
                    
                }
                if ((string.IsNullOrWhiteSpace(txtMessage.Text)))
                {
                    MessageBox.Show("Message boxes is empty. Please fill this first.");

                }

                    try
                {
                    
                    MailAddress checkemail = new MailAddress(txtSender.Text);
                    lblMessageType.Content = "E-mail";
                    txtMessage.MaxLength = 1028;
                    lblCharcLimit.Content = "1028 characters Max.";
                    messageIds();
                    emailn.SenderID = txtSender.Text;
                    emailn.Subject = txtSubject.Text;
                    emailn.MessageTxt = txtMessage.Text;
                    emailn.MessageID = txtMessageID.Text;
                }
                catch (FormatException)
                {

                }
                
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
            Trends showtrend = new Trends();
            string selectedfile = listMessages.SelectedItem.ToString();
            string fullFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedfile);
            //Process.Start(fullFileName);
            using (StreamReader files = File.OpenText(fullFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                if (selectedfile.StartsWith("T"))
                {
                    Tweet readtweet = (Tweet)serializer.Deserialize(files, typeof(Tweet));
                    txtSender.Text = readtweet.SenderID;
                    txtMessageID.Text = readtweet.MessageID;
                    txtMessage.Text = readtweet.MessageTxt;
                    foreach (Match h in hashtag.Matches(txtMessage.Text))
                    {

                        htlist.Add(h.Value);
                    }
                    var groupht = htlist.GroupBy(v => v);
                    foreach (var group in groupht)
                    {
                        displayH.Add("     Found " + group.Key + " : " + group.Count() + " trends");
                    }
                    foreach (Match m in mention.Matches(txtMessage.Text))
                    {

                        mlist.Add(m.Value);
                    }
                    var groupm = mlist.GroupBy(v => v);
                    foreach (var group in groupm)
                    {
                        displayM.Add("     Found " + group.Key + " : " + group.Count() + " trends");
                    }
                    string sp = "\n";
                    showtrend.txtMention.Text = string.Join(sp, displayH);
                    showtrend.txtHashtag.Text = string.Join(sp, displayM);
                    showtrend.Show();
                }
                if (selectedfile.StartsWith("S"))
                {
                    SMS readsms = (SMS)serializer.Deserialize(files, typeof(SMS));
                    txtSender.Text = readsms.SenderID;
                    txtMessageID.Text = readsms.MessageID;
                    txtMessage.Text = readsms.MessageTxt;
                }
                if(selectedfile.StartsWith("E"))
                {
                    Email reademail = (Email)serializer.Deserialize(files, typeof(Email));
                    txtSender.Text = reademail.SenderID;
                    txtMessageID.Text = reademail.MessageID;
                    txtSubject.Text = reademail.Subject;
                    txtMessage.Text = reademail.MessageTxt;
                }
                               
            }
        }

        private void btnTrends_Click(object sender, RoutedEventArgs e)
        {
            Trends showtrend = new Trends();
            foreach (Match h in hashtag.Matches(txtMessage.Text))
            {

                htlist.Add(h.Value);
            }
            var groupht = htlist.GroupBy(v => v);
            foreach (var group in groupht)
            {
                displayH.Add("     Found " + group.Key + " : " + group.Count() + " trends");
            }
            foreach (Match m in mention.Matches(txtMessage.Text))
            {

                mlist.Add(m.Value);
            }
            var groupm = mlist.GroupBy(v => v);
            foreach (var group in groupm)
            {
                displayM.Add("     Found " + group.Key + " : " + group.Count() + " mentions");
            }
            string sp = "\n";
            showtrend.txtMention.Text = string.Join(sp, displayH);
            showtrend.txtHashtag.Text = string.Join(sp, displayM);
            showtrend.Show();
        }
        public bool checkMessegeTweet(string ctweet)
        {
            if (ctweet == "Twitter ID")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkMessegeSMS(string csms)
        {
            if (csms == "SMS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkMessegeEmail(string cemail)
        {
            if (cemail == "E-mail")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int checkMessageID(string sender, int id)
        {
            if(sender == "Twitter ID")
            {
                txtMessageID.Text = "T" + id;
                return 1;
            }
            else if (sender == "SMS")
            {
                txtMessageID.Text = "S" + id;
                return 2;
            }
            else if (sender == "E-mail")
            {
                txtMessageID.Text = "E" + id;
                return 3;
            }
            else
            {
                txtMessageID.Text = " ";
                return 4;
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To send a message use the following format examples : \n - Phone No. +12335678911 \n - Twitter ID @NapierUK \n - Email napier123@mail.com");
        }
    }
}
