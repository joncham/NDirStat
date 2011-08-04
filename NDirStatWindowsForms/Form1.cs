using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NDirStat;

namespace NDirStatWindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SetupListView();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                RunDirectoryScan(dialog.SelectedPath);
            }
        }

        void SetupListView()
        {
            listView1.Columns.Add("ext", "Extension");
            listView1.Columns.Add("color", "Color");
            listView1.Columns.Add("description", "Description");
            listView1.Columns.Add("bytes", "> Bytes");
            listView1.Columns.Add("percent", "% Bytes");
            listView1.Columns.Add("files", "Files");

        }

        void RunDirectoryScan(string directory)
        {
            ModelBuilder builder = new ModelBuilder();
            DirStatModel model = builder.Build(new NDirInfo(directory));
            TreeModel treeModel = new TreeModel(model);
            treeView1.Nodes.Clear();

            BuildSubTree(treeView1.Nodes, treeModel.GetRoot());

            ListModel listModel = new ListModel(model);
            BuildListView(listView1.Items, listModel);

            treeMap1.SetModel(model);
        }

        void BuildListView( ListView.ListViewItemCollection items,  ListModel model)
        {
            foreach (var item in model.GetItems())
            {
                string[] values = new string[] {
                    item.Extension,
                    item.Color.ToString(),
                    item.Description,
                    ListModel.FormatSizeString(item.Bytes),
                    string.Format("{0:P1}", (item.PercentBytes)),
                    item.FileCount.ToString()
                };
                ListViewItem current = new ListViewItem(values);
                items.Add(current);
            }
        }

        void BuildSubTree(TreeNodeCollection nodes, TreeModelData data)
        {
            TreeNode subNode = nodes.Add(data.Name);
            foreach (var item in data.GetChildren())
            {
                BuildSubTree(subNode.Nodes, item);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
