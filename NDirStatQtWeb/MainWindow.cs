using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.trolltech.qt.core;
using NDirStat;
using com.trolltech.qt.gui;
using com.trolltech.qt.webkit;

namespace NDirStatQtWeb
{
    class MainWindow : QMainWindow
    {
        QWebView webView;

        public MainWindow()
        {
            QAction quit = new QAction("&Quit", this);
            quit.triggered.connect(QApplication.instance(), "quit()");
            QAction open = new QAction("&Open", this);
            open.triggered.connect(this, "open()");
            QAction native = new QAction("&Use native object", this);
            native.triggered.connect(this, "native()");

            QMenu file = menuBar().addMenu("&File");
            file.addAction(open);
            file.addAction(native);
            file.addAction(quit);

            webView = new QWebView();

            webView.page().mainFrame().javaScriptWindowObjectCleared.connect(this, "frame_javaScriptWindowObjectCleared()");
            addNativeObject();

            webView.load(QUrl.fromLocalFile(@"C:\Users\chambers\Development\NDirStat\NDirStatQtWeb\NDirStat.html"));
            setCentralWidget(webView);
        }

        void addNativeObject()
        {
            QObject nativeObject = new QObject();
            nativeObject.setProperty("Year", 2011);
            webView.page().mainFrame().addToJavaScriptWindowObject("nativeObject", nativeObject);
        }

        void frame_javaScriptWindowObjectCleared()
        {
            addNativeObject();
        }

        private void native()
        {
            webView.page().mainFrame().evaluateJavaScript("alertNativeObject()");
        }

        private void open()
        {
            QFileDialog dialog = new QFileDialog();
            dialog.setAcceptMode(QFileDialog.AcceptMode.AcceptOpen);
            dialog.setFileMode(QFileDialog.FileMode.DirectoryOnly);

            dialog.setModal(true);
            dialog.show();
            if (dialog.exec() != 0)
            {
                string jsFunc = RunDirectoryScan((string)dialog.selectedFiles().get(0));
                webView.page().mainFrame().evaluateJavaScript(jsFunc);
            }
        }

        string RunDirectoryScan(string directory)
        {
            ModelBuilder builder = new ModelBuilder();
            DirStatModel model = builder.Build(new NDirInfo(directory));
            NDirStat.TreeModel treeModel = new NDirStat.TreeModel(model);

            string json = BuildJSON(treeModel.GetRoot());
            json = string.Format(@"{{ \""data\"": {0}}}", json);

            // call into script to 
            string func = "updateTree(\"" + json + "\")";
            return func;
        }

        string BuildJSON(TreeModelData data)
        {
            string childrenJson = string.Empty;
            foreach (var item in data.GetChildren())
            {
                if (childrenJson != string.Empty)
                    childrenJson += ", ";
                childrenJson += BuildJSON(item);
            }

            string children = string.Empty;
            if (!string.IsNullOrEmpty(childrenJson))
            {
                children = string.Format(@",\""children\"": [{0}]", childrenJson);
            }

            string json = string.Format(@"{{" +
                @"\""data\"": {{\""title\"": \""{0}\"", \""icon\"": \""{1}\""}}" +
                @"{2}" +
                @"}}",
                data.Name, data is FileTreeModelData ? "/" : "folder", children);

            return json;

        }
    }
}
