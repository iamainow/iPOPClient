using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace iPOPClient
{
	public static class Program
	{
		public static System.Collections.Generic.List<PopUp> LIST=new List<PopUp>(256);
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
