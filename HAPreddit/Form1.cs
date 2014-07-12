using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace HAPreddit
{
    public partial class Form1 : Form
    {
        public static IEnumerable<Rekordy> Znajdz() 
        {
            var strona = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("iso-8859-2") };
            var dokument = strona.Load("http://www.reddit.com/r/csharp");
            var linki = dokument.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("class") 
                && x.Attributes["class"].Value.Contains("title may-blank")).ToList();
            var listaRekordow = new List<Rekordy>();
            foreach (var link in linki)
            {
                var rekord = new Rekordy();
                rekord.nazwa = link.InnerText;
                listaRekordow.Add(rekord);
            }

            return listaRekordow;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var listaRekordow = Znajdz();
            var licznik = 0;
            foreach (var rekordy in listaRekordow)
            {
                textBox1.Text += rekordy[licznik].nazwa; // + Environment.NewLine + Environment.NewLine;
            }
        }
    }
}
