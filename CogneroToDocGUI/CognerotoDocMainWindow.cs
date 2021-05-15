using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CogneroToDoc
{
    public partial class CognerotoDocMainWindow : Form
    {
        public CognerotoDocMainWindow()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MainWindow_DragEnter);
            this.DragDrop += new DragEventHandler(MainWindow_DragDrop);
        }

        void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            bool errorOccured = false;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                string filenameNoExt = file.Substring(0, file.LastIndexOf('.'));

                try
                {
                    CogneroTest test = CogneroXMLReader.ParseFile(file);
                    Test2Doc.CreateDocsFromTest(test, filenameNoExt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to convert " + file + " to docx. Ensure it is a well-formed XML file and you have R/W permissions for the folder.\n\nException:\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorOccured = true;
                }
            }

            if (!errorOccured)
                ShowNonBlockingMessageBox("Operation Succeeded", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                ShowNonBlockingMessageBox("Errors occured during operation. Some or all file conversions may have failed.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowNonBlockingMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Thread t = new Thread(() => NonBlockingMessageBox(text, caption, buttons, icon));
            t.Start();
        }

        private void NonBlockingMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox.Show((string)text, (string)caption);
        }
    }
}
