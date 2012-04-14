using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ParsLogPlot
{
    public class frmContrast : System.Windows.Forms.Form
    {
        public int BarValue;
        public IWindowsFormsEditorService _wfes;
        public Controls.Development.ImageListBox imageListBox1;
        public ImageList patternimages;
        public ImageList lineimages;
        private IContainer components;

        public frmContrast()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmContrast));
            this.imageListBox1 = new Controls.Development.ImageListBox();
            this.lineimages = new System.Windows.Forms.ImageList(this.components);
            this.patternimages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageListBox1
            // 
            this.imageListBox1.BackColor = System.Drawing.Color.White;
            this.imageListBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageListBox1.DisplayMember = "BarValue";
            this.imageListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.imageListBox1.FormattingEnabled = true;
            this.imageListBox1.ImageList = this.patternimages;
            this.imageListBox1.ItemHeight = 16;
            this.imageListBox1.Location = new System.Drawing.Point(0, 0);
            this.imageListBox1.Name = "imageListBox1";
            this.imageListBox1.ScrollAlwaysVisible = true;
            this.imageListBox1.Size = new System.Drawing.Size(141, 132);
            this.imageListBox1.TabIndex = 4;
            this.imageListBox1.ValueMember = "BarValue";
            this.imageListBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imageListBox1_MouseClick);
            this.imageListBox1.SelectedIndexChanged += new System.EventHandler(this.imageListBox1_SelectedIndexChanged);
            this.imageListBox1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.imageListBox1_ControlAdded);
            // 
            // lineimages
            // 
            this.lineimages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lineimages.ImageStream")));
            this.lineimages.TransparentColor = System.Drawing.Color.Transparent;
            this.lineimages.Images.SetKeyName(0, "dash.bmp");
            this.lineimages.Images.SetKeyName(1, "dot.bmp");
            this.lineimages.Images.SetKeyName(2, "dotdash.bmp");
            this.lineimages.Images.SetKeyName(3, "line.bmp");
            this.lineimages.Images.SetKeyName(4, "mixed.bmp");
            this.lineimages.Images.SetKeyName(5, "none.bmp");
            // 
            // patternimages
            // 
            this.patternimages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.patternimages.ImageSize = new System.Drawing.Size(64, 16);
            this.patternimages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmContrast
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(141, 133);
            this.ControlBox = false;
            this.Controls.Add(this.imageListBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmContrast";
            this.ShowInTaskbar = false;
            this.Closed += new System.EventHandler(this.frmContrast_Closed);
            this.Load += new System.EventHandler(this.frmContrast_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmContrast_Closed(object sender, System.EventArgs e)
        {
            _wfes.CloseDropDown();
        }

        private void imageListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BarValue = imageListBox1.SelectedIndex;
        }
        Bitmap tile(Bitmap bmp, int width, int height)
        {
            Bitmap test = new Bitmap(width , height );
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    test.SetPixel(i, j, bmp.GetPixel(i % bmp.Width, j % bmp.Height));
            return test;
        }
        public void rearrange(int type)
        {
            patternimages.Images.Clear();
            logplotDataSetTableAdapters.resourceTableAdapter r = new ParsLogPlot.logplotDataSetTableAdapters.resourceTableAdapter();
            logplotDataSet.resourceDataTable dt = r.GetData();
            logplotDataSet.resourceRow[] rrs = (logplotDataSet.resourceRow[])dt.Select("type=0");

            for (int i = 0; i < rrs.Length; i++)
            {
                Bitmap bm = MainForm.GetResourceBMP(rrs[i].id);
                Bitmap cm = tile(bm, 64,24);
/*                    new Bitmap(64,16);
                for (int j = 0; j < cm.Width; j++)
                    for (int k = 0; k < cm.Height; k++)
                        cm.SetPixel(j, k, bm.GetPixel(j % bm.Width, k % bm.Height));
 */ 
                for (int k = 0; k < cm.Height; k++)
                {
                    cm.SetPixel(0, k, Color.Blue);
                    cm.SetPixel(cm.Width-1, k, Color.Blue);
                }
                for (int k = 0; k < cm.Width; k++)
                {
                    cm.SetPixel(k, 0, Color.Blue);
                    cm.SetPixel(k, cm.Height-1, Color.Blue);
                }
                patternimages.Images.Add(i.ToString(), cm);
                patternimages.Images[i].Tag = rrs[i].id.ToString();
            }
            imageListBox1.Items.Clear();
            if (type == 1)
            {
                imageListBox1.ImageList = patternimages;
            }
            else
            {
                imageListBox1.ImageList = lineimages;
            }
            for (int i = 0; i < imageListBox1.ImageList.Images.Count; i++)
                imageListBox1.Items.Add(new Controls.Development.ImageListBoxItem(imageListBox1.ImageList.Images[0].Tag.ToString(), i));
            imageListBox1.SelectedIndex = BarValue;
        }
        private void frmContrast_Load(object sender, EventArgs e)
        {
        }

        private void imageListBox1_ControlAdded(object sender, ControlEventArgs e)
        {
            imageListBox1.SelectedIndex = BarValue;
        }

        private void imageListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            BarValue = imageListBox1.SelectedIndex;
            _wfes.CloseDropDown();
        }

    }
}
