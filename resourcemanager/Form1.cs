using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace resourcemanager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int pattern = -1;
        public int Pattern { get { return pattern; } set { pattern = value; } }
//        public Form1(int _id, short _type, string connection)
        public Form1(int _id, short _type)
        {
            InitializeComponent();
//            if(!connection.Trim().Equals(""))                this.resourceTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            original = _id;
            type = _type;
            ParsLogPlotDataSet.resourceDataTable dt = resourceTableAdapter.GetData();
            ParsLogPlotDataSet.resourceRow[] rrs = (ParsLogPlotDataSet.resourceRow[])dt.Select("type = " + type.ToString());
            count = rrs.Length;
            for (int i = 0; i < count; i++)
            {
                ParsLogPlotDataSet.resourceRow rr = rrs[i];
                if ((rr.id == _id)|(_id==-1))
                {
                    selected = i;
                    if (_id > -1)
                        original = i;
                    break;
                }
            }
        }
        int selected = -1;
        short type = 1;
        Data[] data;
        int count = 0;
        int zoom = 5;
        void refresh(int i)
        {
            if (i % 2 == 1)
            {
                pictureBox1.Invalidate();
                pictureBox1.Update();
            }
            if (i / 2 % 2 == 1)
            {
                pictureBox2.Invalidate();
                pictureBox2.Update();
            }
            if (selected >= 0)
            {
                if (data[selected] != null)
                    textBox1.Text = data[selected].row.name;
            }
            else
                textBox1.Text = "";
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ParsLogPlotDataSet.resourceDataTable dt = resourceTableAdapter.GetData();
            ParsLogPlotDataSet.resourceRow[] rrs = (ParsLogPlotDataSet.resourceRow[])dt.Select("type=" + type.ToString());
            int hs = 10;
            int vs = 10;
            int left = hs;
            int top = vs;
            int maxheight = 0;
            count = rrs.Length;
            Array.Resize(ref data, count);
            for (int i = 0; i < count; i++)
            {
                ParsLogPlotDataSet.resourceRow rr = rrs[i];
                if (data[i] == null)
                    data[i] = new Data();
                data[i].row = rr;
                string s = rr.bitmap;
                int width = rr.width;
                int height = rr.height;
                int size = width * height;
                int offset = 0;
                int curwidth = 0;
                int curheight = 0;

                if (maxheight < height * zoom)
                    maxheight = height * zoom;
                if (left + width * zoom + vs >= pictureBox1.Width)
                {
                    left = hs;
                    top += maxheight + vs;
                    maxheight = 0;
                }
                data[i].start = new Point(left, top);
                data[i].size = new Size(width * zoom, height * zoom);
                Rectangle rf = new Rectangle(left, top, zoom * width, zoom * height);
                Bitmap bmp = new Bitmap(width, height);
                while (offset < size)
                {
                    Rectangle rect = new Rectangle(curwidth * zoom + left, curheight * zoom + top, zoom - 1, zoom - 1);
                    if (e.ClipRectangle.IntersectsWith(rect))
                        if (myImage.imageutilities.bit(ref s, offset))
                            bmp.SetPixel(curwidth, curheight, Color.Black);
                        else
                            bmp.SetPixel(curwidth, curheight, Color.Transparent);
                    offset++;
                    curwidth++;
                    if (curwidth >= width)
                    {
                        curwidth = 0;
                        curheight++;
                    }
                }
                data[i].bmp = bmp;
                TextureBrush tb = new TextureBrush(bmp);
                Bitmap test = myImage.imageutilities.resize(bmp, zoom);
                g.DrawImageUnscaled(test, left, top);
                if (e.ClipRectangle.IntersectsWith(rf))
                    if (selected == i)
                        g.DrawRectangle(Pens.Blue, rf);
                    else
                        g.DrawRectangle(Pens.Gray, rf);
                left += width * zoom + hs;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.resourceTableAdapter.Fill(this.parsLogPlotDataSet.resource);
            ParsLogPlotDataSet.resourceDataTable dt = resourceTableAdapter.GetData();
            count = dt.Count;
            data = new Data[count];
            zoom = trackBar1.Value;
            Form1_Paint((object)pictureBox1, new PaintEventArgs(pictureBox1.CreateGraphics(), pictureBox1.Bounds));
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < count; i++)
                if (myImage.imageutilities.isin(e.X, data[i].start.X, data[i].start.X + data[i].size.Width) & myImage.imageutilities.isin(e.Y, data[i].start.Y, data[i].start.Y + data[i].size.Height))
                {
                    if (selected != i)
                    {
                        selected = i;
                        refresh(3);
                        textBox1.Text = data[i].row.name;
                    }
                    return;
                }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            zoom = trackBar1.Value;
            refresh(1);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (selected < 0)
                return;
            if (data[selected] ==null)
                return;
            int zoom = trackBar2.Value;
            Graphics g = e.Graphics;
            ParsLogPlotDataSet.resourceRow rr = data[selected].row;
            string s = rr.bitmap;
            int width = rr.width;
            int height = rr.height;
            int left = pictureBox2.Width / 2 - width * zoom / 2;
            int top = pictureBox2.Height / 2 - height * zoom / 2;
            Rectangle rf = new Rectangle(left, top, zoom * width, zoom * height);
            Bitmap test = myImage.imageutilities.resize(data[selected].bmp, zoom);
            if (checkBox1.Checked)
            {
                TextureBrush tb = new TextureBrush(test);
                g.FillRectangle(tb, new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height));
            }
            else
            {
                g.DrawImageUnscaled(test, left, top);
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            refresh(2);
        }

        private void editbutton_Click(object sender, EventArgs e)
        {
            if (selected >= 0)
            {
                ResourceEditorForm refs = new ResourceEditorForm(data[selected].row);
                if(refs.ShowDialog()==DialogResult.OK)
                {
                    data[selected].row = refs.Row;
                    resourceTableAdapter.Update(data[selected].row);
                    refresh(3);
                }
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            editbutton_Click(sender, new EventArgs());
        }
        int original=-1;
        private void restorebutton_Click(object sender, EventArgs e)
        {
            selected = original;
            refresh(3);
        }

        private void newbutton_Click(object sender, EventArgs e)
        {
            short width = 8, height = 8;
            if (type == 1)
            {
                width = 19;
                height = 19;
            }
            if (type == 2)
            {
                width = 32;
                height = 8;
            }
            resourceTableAdapter.Insert("", width, height, "0", type);
            refresh(3);
            selected = count - 1;
            editbutton_Click(sender, new EventArgs());
        }

        private void clonebutton_Click(object sender, EventArgs e)
        {
            if (selected < 0)
                return;
            resourceTableAdapter.Insert(data[selected].row.name, data[selected].row.width, data[selected].row.height, data[selected].row.bitmap, data[selected].row.type);
            refresh(3);
            selected = count - 1;
            editbutton_Click(sender, new EventArgs());
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            if (selected == -1)
                DialogResult = DialogResult.Cancel;
            pattern = data[selected].row.id;
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            if(selected>=0)
            {
                data[selected].row.Delete();
                resourceTableAdapter.Update(data[selected].row);
                selected = -1;
                refresh(3);
            }
        }
    }
    public class Data
    {
        public ParsLogPlotDataSet.resourceRow row;
        public Size size;
        public Point start;
        public Bitmap bmp;
    }
}