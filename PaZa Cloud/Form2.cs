using Dropbox.Api;
using Dropbox.Api.Files;
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
    public partial class Form2 : Form
    {
        public string currentPath;
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string currentPath)
        {
            InitializeComponent();
            this.currentPath = currentPath;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            FolderMetadata metadata = await client.Files.CreateFolderAsync(new CreateFolderArg(currentPath + "/" + textBox1.Text));
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
