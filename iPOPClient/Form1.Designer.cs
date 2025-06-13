namespace iPOPClient
{
	partial class Form1
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
			this.components=new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this._run=new System.Windows.Forms.Button();
			this.label1=new System.Windows.Forms.Label();
			this._progress=new System.Windows.Forms.ProgressBar();
			this._timer=new System.Windows.Forms.Timer(this.components);
			this._cancel=new System.Windows.Forms.Button();
			this._icon=new System.Windows.Forms.NotifyIcon(this.components);
			this._icon_timer=new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// _run
			// 
			this._run.Location=new System.Drawing.Point(12,12);
			this._run.Name="_run";
			this._run.Size=new System.Drawing.Size(87,26);
			this._run.TabIndex=0;
			this._run.Text="Run";
			this._run.UseVisualStyleBackColor=true;
			this._run.Click+=new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Font=new System.Drawing.Font("Microsoft Sans Serif",9.75F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(204)));
			this.label1.Location=new System.Drawing.Point(105,12);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(175,26);
			this.label1.TabIndex=2;
			this.label1.TextAlign=System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Click+=new System.EventHandler(this.label1_Click);
			// 
			// _progress
			// 
			this._progress.Location=new System.Drawing.Point(108,44);
			this._progress.Name="_progress";
			this._progress.Size=new System.Drawing.Size(172,26);
			this._progress.TabIndex=3;
			// 
			// _timer
			// 
			this._timer.Enabled=true;
			this._timer.Interval=60000;
			this._timer.Tick+=new System.EventHandler(this._timer_Tick);
			// 
			// _cancel
			// 
			this._cancel.Location=new System.Drawing.Point(12,44);
			this._cancel.Name="_cancel";
			this._cancel.Size=new System.Drawing.Size(87,26);
			this._cancel.TabIndex=4;
			this._cancel.Text="Cancel";
			this._cancel.UseVisualStyleBackColor=true;
			this._cancel.Click+=new System.EventHandler(this._cancel_Click);
			// 
			// _icon
			// 
			this._icon.Visible=true;
			this._icon.MouseClick+=new System.Windows.Forms.MouseEventHandler(this._icon_MouseClick);
			// 
			// _icon_timer
			// 
			this._icon_timer.Enabled=true;
			this._icon_timer.Interval=1000;
			this._icon_timer.Tick+=new System.EventHandler(this._icon_timer_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(290,78);
			this.Controls.Add(this._cancel);
			this.Controls.Add(this._progress);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._run);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Name="Form1";
			this.Text="POPClient";
			this.FormClosing+=new System.Windows.Forms.FormClosingEventHandler(this.FFormClosing);
			this.Load+=new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.Button _run;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar _progress;
		private System.Windows.Forms.Timer _timer;
		private System.Windows.Forms.Button _cancel;
		private System.Windows.Forms.NotifyIcon _icon;
		private System.Windows.Forms.Timer _icon_timer;
	}
}