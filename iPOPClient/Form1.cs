using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace iPOPClient
{
    public partial class Form1 : Form
    {
        private string SOUND = Application.StartupPath + @"\Sound\";
        private i.Net.Mail.POP.Manager M;
        private System.Threading.Thread T;
        private System.DateTime LAST_CHECK;
        private bool NeedBlinkIcon;
        private bool anti_new_message;
        public string LOGIN = @"";
        public string PASSWORD = @"";
        public string POPSERVER = @"pop.mail.ru";
        private WebBrowser web;
        public Form1()
        {
            InitializeComponent();
            M = new i.Net.Mail.POP.Manager(30000, Application.StartupPath + @"\data.bin", Application.StartupPath + @"\log.txt");
            M.CLIENT.LocalStatusChanged += delegate (i.Net.Mail.POP.Status.Local LS)
            {
                if (label1.InvokeRequired)
                {
                    System.Action<i.Net.Mail.POP.Status.Local> A1 = (i.Net.Mail.POP.Status.Local L) =>
                    {
                        this.label1.Text = L.ToString();
                    };
                    this.BeginInvoke(A1, new object[] { LS });
                }
                else
                {
                    this.label1.Text = LS.ToString();
                }
            };
            M.ProgressChanged += delegate (int P, int O)
            {
                int i1 = 100 * P / O;
                if (this._progress.InvokeRequired)
                {
                    System.Action<int> A1 = (int I) =>
                    {
                        this._progress.Value = I;
                    };
                    this.Invoke(A1, new object[] { i1 });
                }
                else
                {
                    this._progress.Value = i1;
                }
            };
        }
        private void button1_Click(object sender, EventArgs e)
        {
            T = new System.Threading.Thread(new System.Threading.ThreadStart(this.DO));
            T.SetApartmentState(ApartmentState.STA);
            T.Start();
        }
        private void SetBool(bool b)
        {
            if (_run.InvokeRequired)
            {
                System.Action<bool> A1 = (B) =>
                {
                    this._run.Enabled = B;
                };
                this.Invoke(A1, new object[] { b });
            }
            else
            {
                this._run.Enabled = b;
            }
        }
        private void AddControl(Control C)
        {
            if (this.InvokeRequired)
            {
                System.Action<Control> A1 = (B) =>
                {
                    this.Controls.Add(B);
                };
                this.Invoke(A1, new object[] { C });
            }
            else
            {
                this.Controls.Add(C);
            }
        }
        private void SetText(string t)
        {
            if (this.InvokeRequired)
            {
                System.Action<string> A1 = (T) =>
                {
                    this.Text = T;
                };
                this.Invoke(A1, new object[] { t });
            }
            else
            {
                this.Text = t;
            }
        }
        private static void Message(int Time, string Text, bool ToMail)
        {
            PopUp Pp = new PopUp(Time, Text, ToMail);
            Pp.ShowDialog();
        }
        private static void MessageAsync(int Time, string Text, bool ToMail)
        {
            var R2 = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                Form1.Message(Time, Text, ToMail);
            }));
            R2.Start();
        }
        private void DO()
        {
            if (this.M.CLIENT.global_status == i.Net.Mail.POP.Status.Global.DISCONNECTED)
            {
                SetBool(false);
                SetText("Check...");
                try
                {
                    var R1 = M.Check(POPSERVER, LOGIN, PASSWORD);
                    //GetInfo();
                    if (R1.Valid())
                    {
                        int N = (R1.NewMailCount - R1.OldMailCount);
                        if (N != 0 || R1.NewMailSize != R1.OldMailSize)
                        {
                            bool ismail = R1.NewMailCount > R1.OldMailCount || R1.NewMailSize > R1.OldMailSize;
                            MessageAsync((ismail ? 24 * 60 * 60 : 30) * 5000,
                                R1.OldMailCount.ToString() + " => " + R1.NewMailCount.ToString() +
                                "(" + (N >= 0 ? "+" : "") + N.ToString() + ")" + System.Environment.NewLine +
                                R1.OldDT.ToString("H:mm,") + " " + R1.OldDT.ToString("dd/MM"), ismail && N >= 0);
                            new System.Media.SoundPlayer(Properties.Resources.S0000).PlaySync();
                            if (N != 0)
                            {
                                GetInfo();
                                switch (N)
                                {
                                    case 1:
                                        new System.Media.SoundPlayer(SOUND + @"1.wav").Play();
                                        break;
                                    case 2:
                                        new System.Media.SoundPlayer(SOUND + @"2.wav").Play();
                                        break;
                                    case 3:
                                        new System.Media.SoundPlayer(SOUND + @"3.wav").Play();
                                        break;
                                    case 4:
                                        new System.Media.SoundPlayer(SOUND + @"4.wav").Play();
                                        break;
                                    case 5:
                                        new System.Media.SoundPlayer(SOUND + @"5.wav").Play();
                                        break;
                                    default:
                                        new System.Media.SoundPlayer(SOUND + @"more.wav").Play();
                                        break;
                                }
                            }
                            else
                            {
                                new System.Media.SoundPlayer(SOUND + @"deleted.wav").Play();
                            }
                        }
                    }
                    else
                    {
                        Form1.MessageAsync(5000, "First Run", false);
                    }
                    this.LAST_CHECK = System.DateTime.Now;
                }
                catch (System.IO.FileNotFoundException)
                {
                    Form1.MessageAsync(1000, "Звуков нет. Совсем нет!", false);
                }
                catch (System.Exception)
                {
                    this.M.CLIENT.DisConnect();
                }
                SetText("POPClient");
                SetBool(true);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.NeedBlinkIcon = true;
            var R2 = new System.Threading.Thread(new System.Threading.ThreadStart(this.DO));
            R2.SetApartmentState(ApartmentState.STA);
            R2.Start();
            _icon_timer_Tick(null, null);
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            var R1 = new System.Threading.Thread(new System.Threading.ThreadStart(this.DO));
            R1.SetApartmentState(ApartmentState.STA);
            R1.Start();
            var R2 = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                if ((System.DateTime.Now - this.LAST_CHECK).TotalMilliseconds >= System.Math.Max(30000, 3 * this._timer.Interval))
                {
                    SetBool(false);
                    SetText("Restart...");
                    System.Threading.Thread.Sleep(5000);
                    Application.Restart();
                }
            }));
            R2.Start();
        }
        private void FFormClosing(object sender, FormClosingEventArgs e)
        {
            this.M.CLIENT.ACTIVE = false;
            Program.LIST.ForEach((A) => A.ACTIVE = false);
            Application.Exit();
        }
        private void _cancel_Click(object sender, EventArgs e)
        {
            this.M.CLIENT.ACTIVE = false;
        }
        private void _icon_timer_Tick(object sender, EventArgs e)
        {
            this.NeedBlinkIcon = Program.LIST.Where((A) => A.ACTIVE == true && A.ToMail == true).Count() > 0;
            if (this.NeedBlinkIcon == true)
            {
                if (this.anti_new_message == false)
                {
                    this._icon.Icon = Properties.Resources.new_message_anti;
                }
                else
                {
                    this._icon.Icon = Properties.Resources.new_message;
                }
                this.anti_new_message = !this.anti_new_message;
            }
            else
            {
                this._icon.Icon = Properties.Resources.no_message;
            }
        }
        private void _icon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
            if (this.Visible)
            {
                this.Activate();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            Form1.MessageAsync(5 * 1000, "Last Check" + System.Environment.NewLine + this.LAST_CHECK.ToString("H:mm.ss"), false);
        }
        public void GetInfo()
        {
            web = new WebBrowser();
            web.Dock = System.Windows.Forms.DockStyle.Fill;
            web.Location = new System.Drawing.Point(0, 0);
            web.Margin = new System.Windows.Forms.Padding(4);
            web.MinimumSize = new System.Drawing.Size(30, 29);
            web.Name = "_browser";
            web.ScriptErrorsSuppressed = true;
            web.Size = new System.Drawing.Size(1024, 768);
            web.TabIndex = 0;
            web.ProgressChanged += mainpage;
            web.Navigate("http://www.mail.ru");
        }
        void mainpage(object sender, WebBrowserProgressChangedEventArgs e)
        {
            var web = sender as WebBrowser;
            try
            {
                var login = web.Document.GetElementsByTagName("input").Cast<HtmlElement>().First(A => A.Name == "Login");
                var pass = web.Document.GetElementsByTagName("input").Cast<HtmlElement>().First(A => A.Name == "Password");
                var enter = web.Document.GetElementsByTagName("input").Cast<HtmlElement>().First(A => A.GetAttribute("value") == "Войти");
                login.SetAttribute("value", LOGIN);
                pass.SetAttribute("value", PASSWORD);
                enter.InvokeMember("click");
                web.ProgressChanged -= mainpage;
                web.DocumentCompleted += list;
            }
            catch
            {
            }
        }
        void list(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var web = sender as WebBrowser;
            try
            {
                var messages = web.Document.GetElementsByTagName("div").Cast<HtmlElement>().Where(A => A.GetAttribute("class") == "msgLine msgUnread");
                var M1 = messages.Select(A => A.GetElementsByTagName("a")).Cast<HtmlElement>().Where(A => A.GetAttribute("class") == "msgList msg-Link");
                var Result = M1.Select(A =>
                    new Info()
                    {
                        Theme = A.GetElementsByTagName("span").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "msgList msg-Link")
                        .GetElementsByTagName("span").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "msg-S_in")
                        .GetElementsByTagName("u")[0].GetAttribute("title"),

                        AuthorEmail = A.GetElementsByTagName("span").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "msgList msg-F")
                        .GetElementsByTagName("u").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "m-form")
                        .GetAttribute("title"),
                        AuthorName = A.GetElementsByTagName("span").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "msgList msg-F")
                        .GetElementsByTagName("u").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "m-form")
                        .InnerText,
                        Message = A.GetElementsByTagName("span").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "msgList msg-W")
                        .GetElementsByTagName("span").Cast<HtmlElement>().First(B => B.GetAttribute("class") == "msg-S_in")
                        .InnerText,
                    });
                web.DocumentCompleted -= list;
                web.Dispose();
                ShowResult(Result.ToArray());
            }
            catch
            {
            }
        }
        public void ShowResult(Info[] infos)
        {
        }
        public struct Info
        {
            public string Theme;
            public string AuthorName;
            public string AuthorEmail;
            public string Message;
        }
    }
}