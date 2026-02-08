using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winforms;

public partial class LoadingBarUI : Form
{
	ProgressBar pBar1;
	ProgressBar pBar2;

	Button btn1;
	Button btn2;

	public LoadingBarUI(){
		this.Text = "Barre de progression";
		this.Size = new Size(400, 360);
		this.FormBorderStyle = FormBorderStyle.FixedDialog;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.StartPosition = FormStartPosition.CenterParent;
		this.ShowInTaskbar = false;

		SetupProgressBar(ref pBar1, new Point(40, 50), "principal");
		SetupProgressBar(ref pBar2, new Point(40, 130), "secondaire");

		btn1 = new Button();
		btn1.Text = "Executer";
		btn1.AutoSize = true;
		btn1.Location = new Point(40, 200);
		btn1.Click += start_prog;

		btn2 = new Button();
		btn2.Text = "Fermer";
		btn2.AutoSize = true;
		btn2.Location = new Point(140, 200);
		btn2.Click += exit_bye;

		this.Controls.Add(btn1);
		this.Controls.Add(btn2);
	}

	private void start_prog(object sender, EventArgs e){
		btn1.Enabled = false;
		btn2.Enabled = false;
		this.Cursor = Cursors.WaitCursor;
		Task task1 = LoadProgressBarAsync(pBar1, pBar2);
	}

	private void exit_bye(object sender, EventArgs e){
		this.Close();
	}

	private async Task LoadProgressBarAsync(ProgressBar b1, ProgressBar b2){
		while(b1.Value < 100){
			while(b2.Value < b2.Maximum){
				b2.Value += 1;
				await Task.Delay(100);
			}
			b2.Value = 0;
			b2.Maximum = new Random().Next(10,50);
			b1.Value += 25;
		}
		this.Cursor = Cursors.Default;

		btn1.Enabled = true;
		btn2.Enabled = true;
	}

	private void SetupProgressBar(ref ProgressBar b, Point p, string labeltext){
		
		Label newlbl = new Label();
		newlbl.Location = new Point(p.X, p.Y - 25);
		newlbl.Text = labeltext;

		b = new ProgressBar();
		b.Location = p;
		b.Size = new Size(300, 50);
        b.Minimum = 0;
        b.Maximum = 100;
        b.Value = 0;
        b.Step = 10;
	
		this.Controls.Add(b);
		this.Controls.Add(newlbl);
	}
}
