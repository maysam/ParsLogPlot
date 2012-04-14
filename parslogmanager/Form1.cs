using System;
using System.Resources;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace parslogmanager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               
                ParsLogPlot.MainForm mf = new ParsLogPlot.MainForm("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + openFileDialog1.FileName);
                mf.MdiParent = this;                
                mf.Text = openFileDialog1.FileName;
                mf.Tag = openFileDialog1.FileName;
                mf.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            Application.Idle += ((ParsLogPlot.MainForm)this.ActiveMdiChild).idle;
            Application.Exit();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void arrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void aboutParsLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RIPI product for "+Environment.NewLine+"well logging purposes", "About Pars Log Software");
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
//                logplotDataSet1.BeginInit
                using (FileStream sw = File.Create( saveFileDialog1.FileName))
                {
                    // Add some text to the file.
                    //Application.ExecutablePath
                    
                    //ResourceManager rm = 
                    sw.Write(Resources.template, 0, Resources.template.Length);
                    sw.Flush();
                    sw.Close();
                }

                ParsLogPlot.MainForm mf = new ParsLogPlot.MainForm("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + saveFileDialog1.FileName);
                mf.MdiParent = this;
                mf.Text = saveFileDialog1.FileName;
                mf.Tag = saveFileDialog1.FileName;
                mf.Show();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ParsLogPlot.MainForm)this.ActiveMdiChild).SaveData();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    string source = this.ActiveMdiChild.Tag.ToString();
                    File.Copy(source, saveFileDialog1.FileName, true);
                    this.ActiveMdiChild.Close();
                    ParsLogPlot.MainForm mf = new ParsLogPlot.MainForm("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + saveFileDialog1.FileName);
                    mf.MdiParent = this;
                    mf.Text = saveFileDialog1.FileName;
                    mf.Tag = saveFileDialog1.FileName;
                    mf.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
        int pagenum = 0;
        Bitmap b = null;

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            pagenum = 0;
            b = null;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
//            MessageBox.Show(e.Graphics.DpiY.ToString());
            if(pagenum==0)
                e.Graphics.DrawString(((ParsLogPlot.MainForm)this.ActiveMdiChild).Scaling, SystemFonts.DefaultFont, Brushes.Black, 0, 0);
            if (b == null)
            {
                ((ParsLogPlot.MainForm)this.ActiveMdiChild).drawit(e.Graphics.DpiY);
                b = ((ParsLogPlot.MainForm)this.ActiveMdiChild).printable;
            }
            int wpage = (int)Math.Floor((double)(b.Width / e.MarginBounds.Width)) + 1;
            int hpage = (int)Math.Floor((double)(b.Height / e.MarginBounds.Height))+1;
            int totalpage = wpage * hpage;
            int wthis = pagenum % wpage;
            int hthis = (int)(pagenum / wpage);
            Bitmap bp = new Bitmap(e.MarginBounds.Width, e.MarginBounds.Height);
            Graphics g = Graphics.FromImage(bp);
            g.DrawImage(b, -wthis* e.MarginBounds.Width, -hthis*e.MarginBounds.Height);
            e.Graphics.DrawImageUnscaled(bp, e.MarginBounds.X, e.MarginBounds.Y);
            pagenum++;
            if (pagenum < totalpage)
                e.HasMorePages = true;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ParsLogPlot.MainForm)this.ActiveMdiChild).UNDO();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ParsLogPlot.MainForm)this.ActiveMdiChild).REDO();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ((ParsLogPlot.MainForm)this.ActiveMdiChild).SaveData();
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //not yet
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveimageDialog.ShowDialog()==DialogResult.OK)
                ((ParsLogPlot.MainForm)this.ActiveMdiChild).printable.Save(saveimageDialog.FileName);
        }
    }
}