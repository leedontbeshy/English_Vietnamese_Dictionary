using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SpeechLib;

namespace English___Vietnamese_Dictionary
{
    public partial class MyFavoriteWords : Form
    {
        #region Khai báo biến
        long startID, endID;
        
        Form1 frmMain;
        #endregion
        #region Khởi tạo và tải dữ liệu
        public MyFavoriteWords(Form1 frm)
        {
            InitializeComponent();
            frmMain = frm;
        }

        private void frmFavoriteWords_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (frmMain.dictIndexAV.Count == 0)
            {
                frmMain.LoadIndexFile();
            }
            foreach (String word in frmMain.fWordList)
            {
                ListViewItem it = new ListViewItem(word.Split(':')[0].Trim(), 0);
                it.SubItems.Add(word.Split(':')[1].Trim());
                listView1.Items.Add(it);
            }
        }
        #endregion
        #region Các hàm hỗ trợ
        public void HighLight()
        {
            string word = richTextBox1.Lines[0].Replace("/", "[").Substring(0, richTextBox1.Lines[0].Length - 1) + "]";
            string temp = "";
            for (int k = 1; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].Trim() != "")
                {
                    temp += Environment.NewLine + richTextBox1.Lines[k].Trim();
                }
            }
            richTextBox1.Text = word + temp;
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = listView1.SelectedItems[0].Text.Trim().Length;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);

            for (int k = 0; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].StartsWith("•"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);

                }
                else if (richTextBox1.Lines[k].StartsWith("-"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.TranslatedWordColor;

                }
                else if (richTextBox1.Lines[k].StartsWith("*"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.WordTypeColor;

                }
            }
            richTextBox1.Refresh();

        }

        public Point GetLineRange(int ID)
        {
            int i = 0;
            for (int k = 0; k < ID; k++)
            {
                i += richTextBox1.Lines[k].Length;
            }

            return new Point(i + ID + 1, richTextBox1.Lines[ID].Length);
        }
        #endregion
        #region Xử lý sự kiện 
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                toolStripStatusLabel1.Text = listView1.SelectedItems[0].Text;
                toolStripStatusLabel4.Text = (listView1.SelectedItems[0].Index + 1).ToString() + " of " + listView1.Items.Count.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = "No word selected";
                toolStripStatusLabel4.Text = "NaN";
                richTextBox1.Text = "";
            }
        }
      
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "/Database/anhviet_nghia.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/anhviet_nghia.txt");
                long i = -1;
                startID = long.Parse(frmMain.dictIndexAV[listView1.SelectedItems[0].Text].Split(',')[0].Trim());
                endID = startID + long.Parse(frmMain.dictIndexAV[listView1.SelectedItems[0].Text].Split(',')[1].Trim());
                //MessageBox.Show(startID.ToString() + ", " + endID.ToString());
                richTextBox1.Text = "";
                while (reader.Peek() >= 0)
                {
                    i += 1;
                    string s = reader.ReadLine();
                    if ((i >= startID) && (i <= endID))
                    {
                        richTextBox1.AppendText(s + Environment.NewLine);
                    }

                }
                reader.Close();
                //Highlight
                richTextBox1.Text = richTextBox1.Text.Trim().Replace("*", "•").Replace("+", Environment.NewLine + "+").Replace("-", "- ").Replace("=", "*  ");
                richTextBox1.Text = richTextBox1.Text.Substring(1, richTextBox1.Text.Length - 1);
                HighLight();
            }
        }
        // Chout phai list tu yeu thich
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = listView1.SelectedItems.Count > 0;
            removeToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled;
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem it in listView1.SelectedItems)
            {
                it.Remove();
            }
            //
            frmMain.fWordList.Clear();
            foreach (ListViewItem it in listView1.Items)
            {
                frmMain.fWordList.Add(it.Text + ":" + it.SubItems[1].Text);
            }
        }
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].Text);

        }

        // Nut noi
        private void button1_Click(object sender, EventArgs e)
        {
            //Create a TTS voice and speak.
            try
            {
                SpVoice Voice = new SpVoice();
                Voice.Voice = Voice.GetVoices().Item(Properties.Settings.Default.VoiceType);
                Voice.Volume = Properties.Settings.Default.Volume;
                String word = richTextBox1.Text.Split('[')[0].Trim();
                Voice.Speak(word);
            }
            catch
            {
                MessageBox.Show("Speak error", "SimpleTTS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Tên app
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Visible = richTextBox1.Text != "";
        }

        //chout phai nghia cua tu
        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem1.Enabled = richTextBox1.SelectionLength > 0;
            chonTâtCaToolStripMenuItem.Enabled = richTextBox1.Text != "";

        }        
        private void chonTatCaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Focus();
            richTextBox1.SelectAll();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Clipboard.SetText(listView1.SelectedItems[0].Text);
            }
        }
        #endregion
    }
}
