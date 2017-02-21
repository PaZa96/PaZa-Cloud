using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Users;
using Dropbox.Api.Files;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace PaZa_Cloud
{
    public partial class Form1 : Form
    {
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);
        private string CurrentPath = "";

        public Form1()
        {
            getFullInfo();
            getSpaceUsage();
            getFile(string.Empty);
            InitializeComponent();
        }

        private async void getFullInfo()
        {
            FullAccount info = await client.Users.GetCurrentAccountAsync();
            if (info != null)
            {
                label1.Text = info.Name.DisplayName;
                label2.Text = "Email: " + info.Email;
                label3.Text = "Страна: " + info.Country;
                label4.Text = "Подписка: " + (info.AccountType.IsBasic ? "Basic" : info.AccountType.IsBusiness ? "Business" : info.AccountType.IsPro ? "Pro" : "Unknown");
            }
        }

        private async void getSpaceUsage()
        {
            SpaceUsage space = await client.Users.GetSpaceUsageAsync();
            if (space != null)
            {
                label5.Text = "Используется: " + (space.Used / 1000000).ToString() + "MB";
                label6.Text = "Всего: " + (space.Allocation.AsIndividual.Value.Allocated / 1000000).ToString() + "MB";
            }
        }

        private async void getFile(string file)
        {
            var list = await client.Files.ListFolderAsync(file);

            if (list != null)
            {
                
                listBox1.Items.Clear();
                CurrentPath = (file);
                if (CurrentPath != "")
                {
                    listBox1.Items.Add("..");
                    
                }
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    listBox1.Items.Add(item.Name);
                }

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    listBox1.Items.Add(item.Name);
                }
                
            }
            
        }

        private void Download()
        {

        }

        private void Upload()
        {

        }

        private async void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string myStream;
            string filePath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.FileName) != null)
                {

                    //  MemoryStream ms = await client.Files.UploadAsync(myStream, WriteMode.Overwrite.Instance, );
                }

            }
        }

        private async void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) { return; }

            string file = Convert.ToString(listBox1.SelectedItem);

            if(file == "..")
            {
                // this.CurrentPath = (this.CurrentPath).Replace("//", "/");
                this.CurrentPath = Path.GetDirectoryName(this.CurrentPath).Replace("\\","/");
                if (this.CurrentPath == "/") { this.CurrentPath = ""; }
                getFile(CurrentPath);
            }
            else
            {
                
                if ()
                getFile(CurrentPath + "/" + file);
            }
          
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
