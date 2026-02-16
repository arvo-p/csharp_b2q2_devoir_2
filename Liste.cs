using System.Diagnostics;
using System.Runtime.InteropServices;

namespace winforms;

public partial class Liste : Form
{

	[DllImport("user32.dll", EntryPoint = "SendMessage")] public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam); 
	const int LB_SetItemData = 0x019A;  
	const int LB_GetItemData = 0x0199;  

	int last_secret_id = -1;

	GroupBox gbMain;
	ListBox lbPerson;
	TextBox tbNom;
	ComboBox cbQualite;

    public Liste()
    {
		Button btnConfirmer = new Button();
		Button btnAnnuler = new Button();

		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.ClientSize = new Size(410, 650);
		this.MinimizeBox = false;
		this.MaximizeBox = false;
		this.Text = "Gestionnaire";
		
		lbPerson = new ListBox();
		lbPerson.Size = new Size(390, 305);
		lbPerson.Location = new Point(10, 40);

		this.Controls.Add(AttachALabel("Nom de fichier", lbPerson));
		
		Control refCtrl = IncrementVerticalPosition(ButtonPos("Ouvrir", lbPerson),15);
		refCtrl.Click += DoEventOuvrir;

		gbMain = SetupGroupBox(refCtrl);	
		int gbmaxwidth = gbMain.Size.Width - 20;

		refCtrl = ButtonPos("Enregistrer", refCtrl);
		refCtrl.Click += DoEventEnregistrer;
		refCtrl = IncrementVerticalPosition(ButtonPos("Ajouter", refCtrl),15);
		refCtrl.Click += DoEventAjouter;
		refCtrl = ButtonPos("Supprimer", refCtrl);
		refCtrl.Click += DoEventSupprimer;
		refCtrl = ButtonPos("Modifier", refCtrl);
		refCtrl.Click += DoEventMod;

		cbQualite = new ComboBox();
		cbQualite.Location = new Point(10, 60);
		cbQualite.Size = new Size(gbmaxwidth, 40);
		cbQualite.Items.AddRange(new object[]{"Madame",
                        "Mademoiselle",
                        "Monsieur",
                        "Fnord",
						"Indéfini",
						});

		tbNom = new TextBox();
		tbNom.Location = new Point(cbQualite.Location.X, cbQualite.Location.Y + 70);
		tbNom.Size = new Size(gbmaxwidth, 40);

		btnConfirmer.Location = new Point(tbNom.Location.X, tbNom.Location.Y + 50);
		btnConfirmer.Size = new Size(gbmaxwidth/2-5, 33);
		btnConfirmer.Text = "Confirmer";
		btnConfirmer.Click += DoEventConfirmer;

		btnAnnuler.Location = new Point(tbNom.Location.X + 5 + gbmaxwidth/2, tbNom.Location.Y + 50);
		btnAnnuler.Size = new Size(gbmaxwidth/2-5, 33);
		btnAnnuler.Text = "Annuler";
		btnAnnuler.Click += DoEventAnnuler;

		gbMain.Controls.Add(tbNom);
		gbMain.Controls.Add(cbQualite);
		gbMain.Controls.Add(AttachALabel("Qualité", cbQualite));
		gbMain.Controls.Add(AttachALabel("Nom", tbNom));
		gbMain.Controls.Add(btnConfirmer);
		gbMain.Controls.Add(btnAnnuler);

		ActivateForm(true);

		this.Controls.Add(lbPerson);
	}

	private void SendSecretIndex(ListBox LBtarget, int index, int secret_index){
		SendMessage((IntPtr)LBtarget.Handle, LB_SetItemData, (IntPtr)index, (IntPtr)secret_index);
	}

	private long GetSecretIndex(ListBox LBtarget, int index){
		return SendMessage((IntPtr)LBtarget.Handle, LB_GetItemData, (IntPtr)index, 0);
	}

	private int GetPositionAlphabeticOrder(ListBox LBtarget, string name){
		bool place_found = false;
		foreach(var item in lbPerson.Items){
			int char_index_item = 0;
			string strItem = item.ToString().ToLower();
			strItem = strItem.Substring(strItem.IndexOf(" ")+1);
			
			int compare = string.Compare(name, strItem);
			if(compare <= 0) return LBtarget.Items.IndexOf(item);
		}

		return LBtarget.Items.Count;
	}

	private void DoEventAjouter(object sender, EventArgs e){
		ActivateForm(false);
	}

	private void DoEventSupprimer(object sender, EventArgs e){
		ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lbPerson);
		selectedItems = lbPerson.SelectedItems;

		if (lbPerson.SelectedIndex == -1) return;
		lbPerson.Items.Remove(selectedItems[0]);
	}

	private void DoEventConfirmer(object sender, EventArgs e){
		string text = cbQualite.SelectedItem.ToString() + " " + tbNom.Text;
		
		int appendPosition = GetPositionAlphabeticOrder(lbPerson, tbNom.Text);
		if(appendPosition == -1){
			lbPerson.Items.Add(text);
		}else{
			lbPerson.Items.Insert(appendPosition, text);
		}
	
		last_secret_id += 1;

		SendSecretIndex(lbPerson, appendPosition, last_secret_id);
		
		tbNom.Text = "";
		ActivateForm(true);
	}
	
	private void DoEventMod(object sender, EventArgs e){
		ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lbPerson);
		selectedItems = lbPerson.SelectedItems;
		
		if (lbPerson.SelectedIndex == -1) return;
		
		string[] words = selectedItems[0].ToString().Split(' ');

		cbQualite.SelectedItem = words[0];

		tbNom.Text = string.Join(" ", words, 1, words.Count()-1);
		lbPerson.Items.Remove(selectedItems[0]);

		ActivateForm(false);
	}

	private void DoEventAnnuler(object sender, EventArgs e){
		ActivateForm(true);
	}

	private void DoEventOuvrir(object sender, EventArgs e){
		var dlg = new OpenFileDialog();
		if(dlg.ShowDialog() != DialogResult.OK) return;
	
		string text = File.ReadAllText(dlg.FileName);
		string line;
		last_secret_id = 0;
		using (StringReader sr = new StringReader(text)) {
			int indexList = 0;
			while ((line = sr.ReadLine()) != null){
				string[] words = line.Split('#');
				int indexSecret = Convert.ToInt32(words[1]);
				if(indexSecret > last_secret_id) last_secret_id = indexSecret;
				lbPerson.Items.Add(words[0]);
				SendSecretIndex(lbPerson, indexList, indexSecret); 
				indexList++;
			}
		}
	}
	
	private void DoEventEnregistrer(object sender, EventArgs e){
		var dlg = new SaveFileDialog();
		if(dlg.ShowDialog() != DialogResult.OK) return;

		string destination_filename = dlg.FileName;
		string text = "";

		foreach(var item in lbPerson.Items){
			int indexSecret = (int)GetSecretIndex(lbPerson, lbPerson.Items.IndexOf(item));
			text += item.ToString() + "#" + indexSecret.ToString() + "\n";
		}

		File.WriteAllText(destination_filename, text);
	}

	private void ActivateForm(bool state){
		foreach (Control c in this.Controls){
			if(c == gbMain){
				c.Enabled = !state;
				continue;
			}
			c.Enabled = state;
		}
	}

	private GroupBox SetupGroupBox(Control ctrl){
		GroupBox cb = new GroupBox();
		cb.Location = new Point(ctrl.Location.X + ctrl.Size.Width + 20, ctrl.Location.Y);
		cb.Text = "Détail personne";
		cb.Size = new Size(this.ClientSize.Width - 10 - cb.Location.X, this.ClientSize.Height - cb.Location.Y - 20);

		this.Controls.Add(cb);
		return cb;
	}

	private Control IncrementVerticalPosition(Control element, int incY){
		element.Location = new Point(element.Location.X, element.Location.Y+incY);
		return element;
	}

	private Label AttachALabel(string display_text, Control element){
		Label lNew = new Label();
		lNew.Text = display_text;
		lNew.Location = new Point(element.Location.X, element.Location.Y - 30);
		lNew.AutoSize = true;

		return lNew;
	}

	private Button ButtonPos(string display_text, Control element){
		Button btn = new Button();
		btn.Text = display_text;
		btn.Size = new Size(120, 34);
		btn.Location = new Point(element.Location.X, element.Location.Y + element.Size.Height + 5);

		this.Controls.Add(btn);

		return btn;
	}
}
