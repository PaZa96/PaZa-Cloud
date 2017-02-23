using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaZa_Cloud
{
    public partial class Form6 : Form
    {
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);
        private string CurrentPath = "";
        private string moveTo;
        private string moveFrom;

        public Form6()
        {
            InitializeComponent();
        }

        public Form6(string path, string name)
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

            copy(moveFrom, CurrentPath + "/" + moveTo);
            Close();
        }

        private async void copy(string pathFrom, string pathTo)
        {
            var moveTo = await client.Files.CopyAsync(pathFrom, pathTo);

            if (moveTo != null)
            {
                MessageBox.Show("Копирование выполнено успешно", "Копирование", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Ууупс, что-то пошло не так", "Копирование", MessageBoxButtons.OK);
            }
        }

    }
}
