﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Users;
using Dropbox.Api.Files;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PaZa_Cloud
{
    public partial class Form1 : Form
    {
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);
        private string CurrentPath = "";
        private FileStream DownloadFileStream = null;
        private BinaryWriter DownloadWriter = null;
        private long UploadingFileLength = 0;

        public Form1()
        {
            InitializeComponent();
            getFullInfo();
            getSpaceUsage();
            getFile(string.Empty);
            
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
            this.progressBar1.Value = 0;
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

        private async void Download(string path)
        {
            var response = await client.Files.DownloadAsync(path);

            this.progressBar1.Value = 0;
            response.GetContentAsStringAsync();
            this.DownloadFileStream = new FileStream(this.saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
            this.progressBar1.Value = 100;
            this.DownloadFileStream.Close();
        }

        private async void Upload(string path)
        {
            if (this.openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) { return; }

    
            var fs = new FileStream(this.openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // get file length for progressbar
            this.UploadingFileLength = fs.Length;
            this.progressBar1.Value = 0;
        }

        private async void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string myStream;
            string filePath;

           
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.FileName) != null)
                {
                    var mem = new FileStream(this.openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var uploadFileEntry = await client.Files.UploadAsync(CurrentPath, WriteMode.Overwrite.Instance, body: mem);
                 
                }

            }
        }

        private async void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) { return; }

            string file = Convert.ToString(listBox1.SelectedItem);            

            if (file == "..")
            {
                // this.CurrentPath = (this.CurrentPath).Replace("//", "/");
                this.CurrentPath = Path.GetDirectoryName(this.CurrentPath).Replace("\\","/");
                if (this.CurrentPath == "/") { this.CurrentPath = ""; }
                getFile(CurrentPath);
            }
            else
            {
                if (client.Files.GetMetadataAsync(CurrentPath + "/" + file).Result.IsFolder)
                {
                    getFile(CurrentPath + "/" + file);
                }
                else
                {
                    this.saveFileDialog1.FileName = Path.GetFileName(CurrentPath + "/" + file);

                    if (this.saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }

                    Download(CurrentPath + "/" + file);
                }        
            }
        }

        public async void Delete(string path)
        {
            Metadata data = await client.Files.DeleteAsync(path);
            if (data != null)
            {
                MessageBox.Show("Удаление", "Файл удалён", MessageBoxButtons.OK);
            }
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fr2 = new Form2(CurrentPath);
            if (fr2.ShowDialog(this) == DialogResult.OK)
            {
                return;
            }
            getFile(CurrentPath);
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
               
            }
        }
    }
}
