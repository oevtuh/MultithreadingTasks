using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSearchApp
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource tokenSource;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void SetFileName(string file)
        {
            listFiles.Items.Add(file);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDir.Text))
            {
                listFiles.Items.Clear();
                tokenSource = new CancellationTokenSource();
                var searcher = new DirSearcher(tokenSource);
                searcher.StartSearch(txtDir.Text, term.Text);
                searcher.SenFileName += SetFileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        private void term_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
