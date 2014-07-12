using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace HAPreddit
{
    public partial class Form1 : Form
    {
        public int licznik = 0;
        public static IEnumerable<Rekordy> Znajdz() 
        {
            var strona = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("iso-8859-2") };
            var dokument = strona.Load("http://www.reddit.com/r/csharp");
            var linki = dokument.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("class") 
                && x.Attributes["class"].Value.Contains("title may-blank")).ToList();
            var pictures = dokument.DocumentNode.Descendants("img").Where(x => x.Attributes.Contains("src")).ToList();

            var listaRekordow = new List<Rekordy>();
            foreach (var link in linki)
            {
                var rekord = new Rekordy() { nazwa = link.InnerText };
                listaRekordow.Add(rekord);
            }

            for (int i = 0; i < pictures.Count; i++)
            {
                for (int j = 0; j < listaRekordow.Count; j++)
                {
                    listaRekordow[j].picUrl = pictures[i].InnerText;
                }
            }

            return listaRekordow;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //1st
            var listaRekordow = Znajdz();
            var buforNazwa = new List<string>();
            var buforPic = new List<string>();
            richTextBox1.Clear();
            foreach (var rekord in listaRekordow)
            {
                buforNazwa.Add(rekord.nazwa);
                buforPic.Add(rekord.picUrl);
            }
            richTextBox1.Text = buforNazwa[licznik] + buforPic[licznik];
            pictureBox1.ImageLocation = buforPic[licznik];
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                licznik++;
                button1.PerformClick();
            }
            
        }

        private void prev_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                if (licznik > 0)
                {
                    licznik--;
                }
                else
                {
                    licznik = 0;
                }
                button1.PerformClick();
            }
            
        }
    }
}
