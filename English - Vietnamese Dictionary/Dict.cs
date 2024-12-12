using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace English___Vietnamese_Dictionary
{
    internal class Dict
    {
        public List<String> lstAV = new List<string>();
        public Dictionary<String, String> dictIndexAV = new Dictionary<string, string>();
        //
        public List<String> lstVA = new List<string>();
        public Dictionary<String, String> dictIndexVA = new Dictionary<string, string>();

        public void LoadIndexFileVA()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (File.Exists(Application.StartupPath + "/Database/vietanh.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/vietanh.txt");
                while (reader.Peek() >= 0)
                {
                    String s = reader.ReadLine();
                    if (s.Trim() != "")
                    {
                        try
                        {
                            lstVA.Add(s.Split(':')[0].Trim());
                            dictIndexVA.Add(s.Trim().Split(':')[0].Trim(), s.Trim().Split(':')[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                reader.Close();
            }
        }

        public void LoadIndexFile()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (File.Exists(Application.StartupPath + "/Database/anhviet.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/anhviet.txt");
                while (reader.Peek() >= 0)
                {
                    String s = reader.ReadLine();
                    if (s.Trim() != "")
                    {
                        try
                        {
                            lstAV.Add(s.Split(':')[0].Trim());
                            dictIndexAV.Add(s.Trim().Split(':')[0].Trim(), s.Trim().Split(':')[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                reader.Close();
            }
        }

    }
}
