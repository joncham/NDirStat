using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using NDirStat;

namespace NDirStatGtkSharp
{
    class MainWindow : Window
    {
        TreeView treeView;
        TreeView listView;
        TreeMapView treeMapView;

        public MainWindow(string title)
            : base(title)
        {

            // when this window is deleted, it'll run delete_event()
            DeleteEvent += delete_event;
            Shown += new EventHandler(window_Shown);

            // Add the button to the window and display everything
            MenuBar menuBar = new MenuBar();
            menuBar.Add(new MenuItem("File"));

            treeView = new TreeView();
            // Create a column for the artist name
            Gtk.TreeViewColumn nameColumn = new Gtk.TreeViewColumn();
            nameColumn.Title = "Name";

            // Create the text cell that will display the artist name
            Gtk.CellRendererText fileNameCell = new Gtk.CellRendererText();

            // Add the cell to the column
            nameColumn.PackStart(fileNameCell, true);

            // Add the columns to the TreeView
            treeView.AppendColumn(nameColumn);

            // Tell the Cell Renderers which items in the model to display
            nameColumn.AddAttribute(fileNameCell, "text", 0);

            listView = new TreeView();
            SetupListView();

            treeMapView = new TreeMapView();

            ScrolledWindow scrolledTreeView = new ScrolledWindow();
            scrolledTreeView.Add(treeView);
            ScrolledWindow scrolledListView = new ScrolledWindow();
            scrolledListView.Add(listView);

            HPaned hpaned = new HPaned();
            hpaned.Pack1(scrolledTreeView, true, true);
            hpaned.Pack2(scrolledListView, false, true);

            VPaned vpaned = new VPaned();
            vpaned.Pack1(hpaned, true, true);
            vpaned.Pack2(treeMapView, false, true);

            VBox vbox = new VBox(false, 1);
            vbox.PackStart(menuBar, false, true, 0);
            vbox.PackStart(vpaned, true, true, 0);

            Add(vbox);
        }

        void SetupListView()
        {
            string[] cols = new string[] { "Extension", "Color", "Description", "> Bytes", "% Bytes", "Files" };

            int i = 0;
            foreach (string col in cols)
            {
                Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
                column.Title = col;

                Gtk.CellRendererText textCell = new Gtk.CellRendererText();
                column.PackStart(textCell, true);

                listView.AppendColumn(column);
                column.AddAttribute(textCell, "text", i++);
            }
        }


        void window_Shown(object sender, EventArgs e)
        {
            FileChooserDialog dialog = new FileChooserDialog("Choose a directory", (Window)sender, FileChooserAction.SelectFolder);

            Button button = (Button)dialog.AddButton("OK", ResponseType.Ok);
            dialog.TransientFor = (Window)sender;

            ResponseType result = (ResponseType)dialog.Run();
            dialog.HideAll();

            if (result == ResponseType.Ok)
            {
                RunDirectoryScan(dialog.Filename);
            }
        }

        void RunDirectoryScan(string directory)
        {
            ModelBuilder builder = new ModelBuilder();
            DirStatModel model = builder.Build(new NDirInfo(directory));
            NDirStat.TreeModel treeModel = new NDirStat.TreeModel(model);

            Gtk.TreeStore fileListStore = new Gtk.TreeStore(typeof(string));
            BuildSubTree(fileListStore, treeModel.GetRoot());

            // Assign the model to the TreeView
            treeView.Model = fileListStore;
            treeMapView.SetModel(model);


            ListStore listStore = new ListStore(
                typeof(string),
                typeof(string),
                typeof(string),
                typeof(string),
                typeof(string),
                typeof(string)
                );
            BuildListStore(listStore, new ListModel(model));
            listView.Model = listStore;
        }

        static void BuildListStore(Gtk.ListStore listStore, ListModel model)
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
                listStore.AppendValues(values);
            }
        }

        static void BuildSubTree(Gtk.TreeStore fileListStore, TreeModelData data)
        {
            Gtk.TreeIter subIter = fileListStore.AppendValues(data.Name);
            foreach (var item in data.GetChildren())
            {
                BuildSubTree(fileListStore, subIter, item);
            }
        }

        static void BuildSubTree(Gtk.TreeStore fileListStore, Gtk.TreeIter iter, TreeModelData data)
        {
            Gtk.TreeIter subIter = fileListStore.AppendValues(iter, data.Name);
            foreach (var item in data.GetChildren())
            {
                BuildSubTree(fileListStore, subIter, item);
            }
        }


        // runs when the user deletes the window using the "close
        // window" widget in the window frame.
        void delete_event(object obj, DeleteEventArgs args)
        {
            Application.Quit();
        }

        // runs when the button is clicked.
        void hello(object obj, EventArgs args)
        {
            Application.Quit();
        }

    }
}
