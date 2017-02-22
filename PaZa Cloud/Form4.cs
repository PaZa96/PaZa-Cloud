using Dropbox.Api;
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
    public partial class Form4 : Form
    {
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);

        public Form4()
        {
            InitializeComponent();
            getSahredFiles();
        }
        private async void getSahredFiles()
        {
            var links = await client.Sharing.ListSharedLinksAsync();
            if (links != null)
            {

                listBox1.Items.Clear();

                foreach (var item in links.Links)
                {
                    listBox1.Items.Add(item.Name);
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
