using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace iPOPClient
{
	public partial class PopUp:Form
	{
		public bool ACTIVE;
		public int Time;
		public bool ToMail;
		public static readonly System.Drawing.Size SIZE=new Size(256,128);
		public static System.Drawing.Point NeedLocation(int i)
		{
			var NL=new System.Drawing.Point(System.Windows.Forms.SystemInformation.WorkingArea.Right-PopUp.SIZE.Width,System.Windows.Forms.SystemInformation.WorkingArea.Bottom-(i+1)*PopUp.SIZE.Height);
			while(NL.Y<PopUp.SIZE.Height/2)
			{
				NL.Y+=PopUp.SIZE.Height;
			}
			return NL;
		}
		public static void SetLocation(PopUp PU,int x,int y)
		{
			if(PU.InvokeRequired)
			{
				System.Action<int,int> A1=(X,Y) =>
				{
					PU.Location=new System.Drawing.Point(X,Y);
				};
				PU.Invoke(A1,new object[] { x,y });
			}
			else
			{
				PU.Location=new System.Drawing.Point(x,y);
			}
		}
		public PopUp(int Time,string Text,bool ToMail)
		{
			this.ACTIVE=true;
			InitializeComponent();
			lock(Program.LIST)
			{
				Program.LIST.Add(this);
				var NL=PopUp.NeedLocation(Program.LIST.IndexOf(this));
				if(this.Location!=NL)
				{
					this.Location=NL;
				}
			}
			this.Time=Time;
			this._text.Text=Text;
			this.ToMail=ToMail;
		}
		private void FShown(object sender,EventArgs e)
		{
			this.Activate();
			System.DateTime Start=DateTime.Now;
			while(this.ACTIVE && (DateTime.Now-Start).TotalMilliseconds<this.Time)
			{
				Application.DoEvents();
				System.Threading.Thread.Sleep(15);
			}
			this.Close();
			this.Dispose();
		}
		public new void Close()
		{
			if(this.ACTIVE)
			{
				//lock(Program.LIST)
				{
					Program.LIST.Remove(this);
					for(int i1=Program.LIST.Count-1;i1>=0;i1--)
					{
						if(Program.LIST[i1].ACTIVE==true)
						{
							var F=Program.LIST[i1];
							var NL=PopUp.NeedLocation(i1);
							if(F.Location!=NL)
							{
								SetLocation(F,NL.X,NL.Y);
							}
						}
					}
				}
			}
			base.Close();
		}
		private void _mouse_click(object sender,MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left||e.Button==MouseButtons.Right)
			{
				if(e.Button==MouseButtons.Left && this.ToMail)
				{
					var R2=new System.Threading.Thread(new System.Threading.ThreadStart(delegate()
					{
						System.Diagnostics.Process.Start(@"http://win.mail.ru/cgi-bin/start");
					}));
					R2.Start();
				}
				this.Close();
				this.Dispose();
			}
		}
	}
}