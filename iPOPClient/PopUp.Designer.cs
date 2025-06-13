namespace iPOPClient
{
	partial class PopUp
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components=null;
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing&&(components!=null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._text=new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _text
			// 
			this._text.BackColor=System.Drawing.Color.Transparent;
			this._text.Dock=System.Windows.Forms.DockStyle.Fill;
			this._text.Font=new System.Drawing.Font("Arial",21.75F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(204)));
			this._text.ForeColor=System.Drawing.Color.White;
			this._text.Location=new System.Drawing.Point(0,0);
			this._text.Margin=new System.Windows.Forms.Padding(0);
			this._text.Name="_text";
			this._text.Padding=new System.Windows.Forms.Padding(24);
			this._text.Size=new System.Drawing.Size(256,128);
			this._text.TabIndex=0;
			this._text.TextAlign=System.Drawing.ContentAlignment.MiddleCenter;
			this._text.MouseClick+=new System.Windows.Forms.MouseEventHandler(this._mouse_click);
			// 
			// PopUp
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor=System.Drawing.Color.Black;
			this.BackgroundImage=global::iPOPClient.Properties.Resources.bg;
			this.ClientSize=new System.Drawing.Size(256,128);
			this.Controls.Add(this._text);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Name="PopUp";
			this.ShowIcon=false;
			this.ShowInTaskbar=false;
			this.SizeGripStyle=System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition=System.Windows.Forms.FormStartPosition.Manual;
			this.Text="PopUp";
			this.TopMost=true;
			this.Shown+=new System.EventHandler(this.FShown);
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.Label _text;
	}
}