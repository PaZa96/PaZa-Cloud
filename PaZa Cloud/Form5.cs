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
using Dropbox.Api.Files;
using System.IO;

namespace PaZa_Cloud
{
    public partial class Form5 : Form
    {
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);
        private string CurrentPath = "";
        private string moveTo;
        private string moveFrom;

        public Form5()
        {
            InitializeComponent();
        }

        public Form5(string path, string name)
        {
            this.moveFrom = path;
            this.moveTo = name;
            InitializeComponent();
            getFile(string.Empty);
            
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

        private async void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) { return; }

            string file = Convert.ToString(listBox1.SelectedItem);

            if (file == "..")
            {
                this.CurrentPath = Path.GetDirectoryName(this.CurrentPath).Replace("\\", "/");
                if (this.CurrentPath == "/") { this.CurrentPath = ""; }
                getFile(CurrentPath);
            }
            else
            {
                if (client.Files.GetMetadataAsync(CurrentPath + "/" + file).Result.IsFolder)
                {
                    getFile(CurrentPath + "/" + file);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            move(moveFrom, CurrentPath + "/" + moveTo);
            Close();
        }

        private async void move(string pathFrom, string pathTo)
        {
            var moveTo = await client.Files.MoveAsync(pathFrom, pathTo);

            if(moveTo !=null)
            {
                MessageBox.Show("Перемещение выполнено успешно", "Перемещение", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Ууупс, что-то пошло не так", "Перемещение", MessageBoxButtons.OK);
            }
        }

    }
}
