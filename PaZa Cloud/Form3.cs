using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaZa_Cloud
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string link)
        {
            InitializeComponent();
            this.label1.Width = 50;
            label1.Text = link;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label1.Text);
            Close();
            MessageBox.Show("Ссылка скопированна в буфер обмена", "Информация", MessageBoxButtons.OK);
        }
    }
}
