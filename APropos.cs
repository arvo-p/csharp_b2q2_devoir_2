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

public partial class APropos : Form
{
	Panel main_panel;
	public APropos(){
		this.Text = "A Propos";
		this.Size = new Size(500, 420);
		this.FormBorderStyle = FormBorderStyle.FixedDialog;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.StartPosition = FormStartPosition.CenterParent;
		this.ShowInTaskbar = false;
		
		main_panel = new Panel();
		main_panel.Location = new Point(15, 15);
		main_panel.Size = new Size(this.Width-50, 270);
		main_panel.BorderStyle = BorderStyle.Fixed3D;

		PictureBox pb = new PictureBox();
		pb.Image = Image.FromFile("logo.jpg");
		pb.Width = 130;
		pb.Height = 130;
		pb.SizeMode = PictureBoxSizeMode.Zoom;
		pb.Location = new Point(0, 0);
		main_panel.Controls.Add(pb);

		Font f1 = new Font("Segoe UI", 10, FontStyle.Bold);
		Font f2 = new Font("Segoe UI", 10, FontStyle.Regular);
		Font f3 = new Font("Segoe UI", 10, FontStyle.Italic);

		addLine("Premières manipulations", f2, 150);
		addLine("Copyleft Gabriel", f1, 150);
		addLine("", f1, 0);
		addLine("", f1, 0);
		addLine("Et l'application se dit qu'un jour,", f3, 40);
		addLine("elle deviendra aussi grande que .NET", f3, 40);
		
		Button btn = new Button();
		btn.Text = "Confirmer";
		btn.Click += exit_bye;
		btn.AutoSize = true;
		btn.Location = new Point(200, 300);

		this.Controls.Add(main_panel);
		this.Controls.Add(btn);
	}
	
	private void exit_bye(object sender, EventArgs e){
		this.Close();
	}
	
	int line_y = 0;
	private void addLine(string text, Font f, int x){
		Label lbl = new Label();
		lbl.Text = text;
		lbl.AutoSize = true;
		lbl.Font = f;
		
		line_y += 10 + lbl.Height;
    	lbl.Location = new Point(x, line_y);
		
		main_panel.Controls.Add(lbl);
	}
}
