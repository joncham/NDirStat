using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.trolltech.qt.gui;
using NDirStat;
using java.util;

namespace NDirStatQtJambi
{
    class MainWindow : QMainWindow
    {
        QTreeView treeView;
        QTreeView listView;
        TreeMapWidget treeMap;

        private void open()
        {
            QFileDialog dialog = new QFileDialog();
            dialog.setAcceptMode(QFileDialog.AcceptMode.AcceptOpen);
            dialog.setFileMode(QFileDialog.FileMode.DirectoryOnly);

            dialog.setModal(true);
            dialog.show();
            if (dialog.exec() != 0)
            {
                RunDirectoryScan((string)dialog.selectedFiles().get(0));

            }
        }

        void RunDirectoryScan(string directory)
        {
            ModelBuilder builder = new ModelBuilder();
            DirStatModel model = builder.Build(new NDirInfo(directory));
            NDirStat.TreeModel treeModel = new NDirStat.TreeModel(model);

            QStandardItemModel itemModel = new QStandardItemModel();
            BuildSubTree(itemModel, treeModel.GetRoot());
            // Assign the model to the TreeView
            treeView.setModel(itemModel);

            QStandardItemModel listModel = new QStandardItemModel();
            BuildListView(listModel, new ListModel(model));
            listView.setModel(listModel);


            treeMap.SetModel(model);

        }


        void BuildListView(QStandardItemModel listModel, ListModel model)
        {
            listModel.setRowCount(model.GetItems().Count);
            listModel.setColumnCount(6);
            ArrayList list = new ArrayList();
            list.add("Extension");
            list.add("Color");
            list.add("Description");
            list.add("> Bytes");
            list.add("% Bytes");
            list.add("Files");
            listModel.setHorizontalHeaderLabels(list);
            int i = 0;
            foreach (var item in model.GetItems())
            {
                com.trolltech.qt.core.QModelIndex index = listModel.index(i, 0);
                listModel.setData(i, 0, item.Extension);
                listModel.setData(i, 1, item.Color.ToString());
                listModel.setData(i, 2, item.Description);
                listModel.setData(i, 3, ListModel.FormatSizeString(item.Bytes));
                listModel.setData(i, 4, string.Format("{0:P1}", (item.PercentBytes)));
                listModel.setData(i, 5, item.FileCount.ToString());

                i++;
            }
        }

        void BuildSubTree(QStandardItemModel model, TreeModelData data)
        {
            QStandardItem current = new QStandardItem(data.Name);
            model.appendRow(current);
            foreach (var item in data.GetChildren())
            {
                BuildSubTree(current, item);
            }
        }
        void BuildSubTree(QStandardItem parent, TreeModelData data)
        {
            QStandardItem current = new QStandardItem(data.Name);
            parent.setChild(parent.rowCount(), current);
            foreach (var item in data.GetChildren())
            {
                BuildSubTree(current, item);
            }
        }

        public MainWindow()
        {
            QAction quit = new QAction("&Quit", this);
            quit.triggered.connect(QApplication.instance(), "quit()");
            QAction open = new QAction("&Open", this);
            open.triggered.connect(this, "open()");

            QMenu file = menuBar().addMenu("&File");
            file.addAction(open);
            file.addAction(quit);

            treeView = new QTreeView();
            listView = new QTreeView();
            listView.setUniformRowHeights(true);
            listView.setRootIsDecorated(false);
            listView.setAllColumnsShowFocus(true);

            QDockWidget dockWidget = new QDockWidget("Tree");
            dockWidget.setWidget(treeView);
            addDockWidget(com.trolltech.qt.core.Qt.DockWidgetArea.TopDockWidgetArea, dockWidget);

            QDockWidget listDockWidget = new QDockWidget("List View");
            listDockWidget.setWidget(listView);
            addDockWidget(com.trolltech.qt.core.Qt.DockWidgetArea.TopDockWidgetArea, listDockWidget);

            treeMap = new TreeMapWidget();
            setCentralWidget(treeMap);
        }

    }
}
