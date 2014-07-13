using System;
using System.Collections.Generic;
using System.Drawing;
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
            var pkt = dokument.DocumentNode.Descendants("div").Where(x => x.Attributes.Contains("class")
                && x.Attributes["class"].Value.Contains("score unvoted")).ToList();
            
            var listaRekordow = new List<Rekordy>();
            foreach (var link in linki)
            {
                var url = link.Attributes["href"].Value;
                var rekord = new Rekordy() { nazwa = link.InnerText, adres = url };
                listaRekordow.Add(rekord);
            }

            for (int i = 0; i < listaRekordow.Count; i++)
            {
                if (!listaRekordow[i].adres.Contains("/r/csharp/"))
                {
                    listaRekordow[i].picUrl = pictures[0].Attributes["src"].Value;
                    pictures.RemoveAt(0);
                }
            }

            for (int i = 0; i < pkt.Count; i++)
            {
                listaRekordow[i].punkty = pkt[i].InnerText;
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
            var buforAdres = new List<string>();
            var buforPunkty = new List<string>();
            richTextBox1.Clear();

            foreach (var rekord in listaRekordow)
            {
                buforNazwa.Add(rekord.nazwa);
                buforPic.Add(rekord.picUrl);
                buforAdres.Add(rekord.adres);
                buforPunkty.Add(rekord.punkty);
            }

            if (buforAdres[licznik].Contains("/r/csharp/"))
            {
                //selfpost
                pictureBox2.BackColor = Color.Lime;
                pictureBox3.BackColor = Color.Red;

                richTextBox1.Text = "(" + buforPunkty[licznik] + ")" + buforNazwa[licznik];
                pictureBox1.ImageLocation = buforPic[licznik];
            }
            else
            {
                //non-reddit link
                pictureBox2.BackColor = Color.Red;
                pictureBox3.BackColor = Color.Lime;

                richTextBox1.Text = "(" + buforPunkty[licznik] + ")" 
                    + buforNazwa[licznik] + "\n" + buforAdres[licznik]; // \n -> Environment.NewLine
                pictureBox1.ImageLocation = buforPic[licznik];
            }

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
