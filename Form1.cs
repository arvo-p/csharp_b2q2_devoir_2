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

public partial class Form1 : Form
{
	private void Exit_Click(object sender, EventArgs e){
		System.Environment.Exit(1);
	}

	private void OuvrirListe(object sender, EventArgs e){
		(new Liste()).Show();
	}

	private void OuvrirAPropos(object sender, EventArgs e){
		(new APropos()).ShowDialog();
	}

	private void OuvrirLoadingBar(object sender, EventArgs e){
		LoadingBarUI lbu = new LoadingBarUI();
		lbu.ShowDialog();
	}

    public Form1()
    {
        InitializeComponent();
		MenuStrip MainMenu = new MenuStrip();

		ToolStripMenuItem controlsMenuItem = new ToolStripMenuItem("&Contrôles");
		ToolStripMenuItem applicationsMenuItem = new ToolStripMenuItem("&Applications");
		ToolStripMenuItem helpMenuItem = new ToolStripMenuItem("&Aide");
	
		ToolStripMenuItem listItem = new ToolStripMenuItem("&Liste");
			listItem.Click += OuvrirListe;
		ToolStripMenuItem progressionbarItem = new ToolStripMenuItem("&Barre de progression");
			progressionbarItem.Click += OuvrirLoadingBar;
		ToolStripMenuItem exitItem = new ToolStripMenuItem("&Quitter");
			exitItem.Click += Exit_Click;
		ToolStripMenuItem editorItem = new ToolStripMenuItem("&Editeur");
		ToolStripMenuItem aboutItem = new ToolStripMenuItem("&A Propos");
			aboutItem.Click += OuvrirAPropos;

		MainMenu.Items.Add(controlsMenuItem);
			controlsMenuItem.DropDownItems.Add(listItem);
			controlsMenuItem.DropDownItems.Add(progressionbarItem);
			controlsMenuItem.DropDownItems.Add("-");
			controlsMenuItem.DropDownItems.Add(exitItem);

		MainMenu.Items.Add(applicationsMenuItem);
			applicationsMenuItem.DropDownItems.Add(editorItem);
			
		MainMenu.Items.Add(helpMenuItem);
			helpMenuItem.DropDownItems.Add(aboutItem);
	

		Controls.Add(MainMenu);
		this.MainMenuStrip = MainMenu;
	}	
}
