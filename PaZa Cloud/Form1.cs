using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Users;
using Dropbox.Api.Files;
using System.Linq;

namespace PaZa_Cloud
{
    public partial class Form1 : Form
    {
        static string token = "8FP_ykdhlGAAAAAAAAAATkk6ay4PQvMhxnE5RiEI2zmnVnGI0a8ZTBa18N0LOxLW";
        static DropboxClient client = new DropboxClient(token);

        public  Form1()
        {
            getFullInfo();
            getSpaceUsage();
            getNode();
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

        public string[] getNodeName(string path, int pos)
        {
            string[] res = new string[2];
            int spos = path.LastIndexOf("/");
            res[1] = path.Substring(spos + 1, path.Length - spos - 1);
            path = path.Remove(spos, path.Length - spos);
            spos = path.LastIndexOf("/") + 1;
            res[0] = path.Substring(spos, path.Length - spos);
            return res;
        }

        public async void getNode()
        {
            ListFolderResult list = await client.Files.ListFolderAsync(new ListFolderArg(string.Empty, true));
            if (list != null)
            {
                TreeNode[] root = treeView1.Nodes.Find("root", false);
                root[0].Nodes.Clear();
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    int pos = item.PathDisplay.LastIndexOf("/");
                    if (pos == 0)
                    {
                        treeView1.BeginUpdate();
                        root[0].Nodes.Add(item.Name, item.Name);
                        treeView1.EndUpdate();
                    }
                    else
                    {
                        string[] names = getNodeName(item.PathDisplay, pos);
                        TreeNode[] node = root[0].Nodes.Find(names[0], true);
                        treeView1.BeginUpdate();
                        node[0].Nodes.Add(names[1], names[1]);
                        treeView1.EndUpdate();
                    }
                }

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    int pos = item.PathDisplay.LastIndexOf("/");
                    if (pos == 0)
                    {
                        treeView1.BeginUpdate();
                        root[0].Nodes.Add(item.Name, item.Name);
                        treeView1.EndUpdate();
                    }
                    else
                    {
                        string[] names = getNodeName(item.PathDisplay, pos);
                        TreeNode[] node = root[0].Nodes.Find(names[0], true);
                        treeView1.BeginUpdate();
                        node[0].Nodes.Add(names[1], names[1]);
                        treeView1.EndUpdate();
                    }
                    treeView1.ExpandAll();
                }
            }
        }

        private void Download()
        {

        }

    }
}
