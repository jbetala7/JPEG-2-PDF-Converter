using System;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPG_to_PDF
{
    public partial class Form1 : Form
    {
        string docFileName = string.Empty, defaultPath;
        PdfDocument document = null;
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            defaultPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            defaultPath = Path.Combine(defaultPath, "Desktop");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF Files|*.pdf";
            saveDialog.Title = "PDF file to be saved";
            saveDialog.InitialDirectory = defaultPath;
            saveDialog.FileName = Path.GetFileNameWithoutExtension(docFileName) + ".pdf";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (document.PageCount >= 0)
                {
                    document.Save(saveDialog.FileName);
                }
            }
            button2.Enabled = false;
            docFileName = string.Empty;
            MessageBox.Show("Successfully converted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = defaultPath;
            openDialog.Title = "Select image to be converted";
            openDialog.Filter = "JPEG Files|*.jpg";
            openDialog.Multiselect = true;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                document = new PdfDocument();
                docFileName = openDialog.FileName;
                button2.Enabled = true;
                foreach (string fileSpec in openDialog.FileNames)
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    DrawImage(gfx, fileSpec, 0, 0, (int)page.Width, (int)page.Height);
                }

            }
        }
    }
}
