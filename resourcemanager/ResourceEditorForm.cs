using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace resourcemanager
{
    public partial class ResourceEditorForm : Form
    {
        ParsLogPlotDataSet.resourceRow row;
        public ParsLogPlotDataSet.resourceRow Row
        { get { return row; } set { row = value; } }
        string original;
        string original_name;
        Bitmap bmp;
        public ResourceEditorForm(ParsLogPlotDataSet.resourceRow _row)
        {
            InitializeComponent();
            row = _row;
            textBox1.Text = row.name;
            rebuildbitmap();
            original = row.bitmap;
            original_name = row.name;
        }
        void rebuildbitmap()
        {
            string s = row.bitmap;
            int width = row.width;
            int height = row.height;
            int size = width * height;
            int offset = 0;
            int curwidth = 0;
            int curheight = 0;
            bmp = new Bitmap(width, height);
            while (offset < size)
            {
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
        }
        #region utilities
        bool abit(ref string s, int i)
        {
            s = s.PadRight(i + 1, '0');
            return (s[i] == '1');

        }
        void asetbit(ref string s, int i, string val)
        {
            s = s.PadRight(i+1, '0');
            char c = s[i];
            s = s.Remove(i, 1);
            s = s.Insert(i, val);
        }
        Bitmap aresize(Bitmap bmp, int zoom)
        {
            Bitmap test = new Bitmap(bmp.Width * zoom, bmp.Height * zoom);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                    for (int k = 0; k < zoom; k++)
                        for (int h = 0; h < zoom; h++)
                            test.SetPixel(i * zoom + k, j * zoom + h, bmp.GetPixel(i, j));
            return test;
        }
        bool aisin(int a, int b, int c)
        {
            return ((a >= b) & (a <= c));
        }
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
        }
        #endregion
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            int zoom = trackBar1.Value;
            Graphics g = e.Graphics;
            string s = row.bitmap;
            int width = row.width;
            int height = row.height;

            int left = pictureBox2.Width / 2 - width * zoom / 2;
            int top = pictureBox2.Height / 2 - height * zoom / 2;
            Rectangle rf = new Rectangle(left, top, zoom * width, zoom * height);
            Bitmap test = myImage.imageutilities.resize(bmp, zoom);
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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int widthfactor = pictureBox1.Width/row.width;
            int heightfactor = pictureBox1.Height/row.height;
            string s = row.bitmap;
            for (int i = 0; i < row.width; i++)
                for (int j = 0; j < row.height; j++)
                {
                    if (myImage.imageutilities.bit(ref s, i + row.width * j))
                        e.Graphics.FillRectangle(Brushes.Black, i * widthfactor, j * heightfactor, widthfactor, heightfactor);
                    e.Graphics.DrawRectangle(Pens.LightGray, i * widthfactor, j * heightfactor, widthfactor, heightfactor);
                }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            refresh(2);
        }

        private void fillbutton_Click(object sender, EventArgs e)
        {
            int size = row.width * row.height;
            string s = new string('1', size);
            row.bitmap = s;
            rebuildbitmap();
            refresh(3);
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            int size = row.width * row.height;
            string s = new string('0', size);
            row.bitmap = s;
            rebuildbitmap();
            refresh(3);
        }
        private void restorebutton_Click(object sender, EventArgs e)
        {
            row.bitmap = original;
            row.name = original_name;
            rebuildbitmap();
            refresh(3);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            row.name = textBox1.Text;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int widthfactor = pictureBox1.Width / row.width;
            int heightfactor = pictureBox1.Height / row.height;
            string s = row.bitmap;
            for (int i = 0; i < row.width; i++)
                for (int j = 0; j < row.height; j++)
                    if (myImage.imageutilities.isin(e.X, i * widthfactor, i * widthfactor + widthfactor) & myImage.imageutilities.isin(e.Y, j * heightfactor, j * heightfactor + heightfactor))
                    {
                        //if (bit(ref s, i + row.width * j))
                        if (e.Button == MouseButtons.Right)
                            myImage.imageutilities.setbit(ref s, i + row.width * j, "0");
//                        else
                        if (e.Button == MouseButtons.Left)
                            myImage.imageutilities.setbit(ref s, i + row.width * j, "1");
                        row.bitmap = s;
                        rebuildbitmap();
                        refresh(3);
                        return;
                    }
        }
    }
}
/*
namespace System.ComponentModel
{

    /// <summary>
    ///   Editor Attribute for classes. 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class EditorAttribute : Attribute
    {

        string name;
        string basename;

        public EditorAttribute()
        {
            this.name = string.Empty;
        }

        public EditorAttribute(string typeName, string baseTypeName)
        {
            name = typeName;
            basename = baseTypeName;
        }

        public EditorAttribute(string typeName, Type baseType)
            : this(typeName, baseType.AssemblyQualifiedName)
        {
        }

        public EditorAttribute(Type type, Type baseType)
            : this(type.AssemblyQualifiedName, baseType.AssemblyQualifiedName)
        {
        }

        public string EditorBaseTypeName
        {
            get
            {
                return basename;
            }
        }

        public string EditorTypeName
        {
            get
            {
                return name;
            }
        }

        public override object TypeId
        {
            get
            {
                return this.GetType();
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EditorAttribute))
                return false;

            return ((EditorAttribute)obj).EditorBaseTypeName.Equals(basename) &&
                ((EditorAttribute)obj).EditorTypeName.Equals(name);
        }

        public override int GetHashCode()
        {
            return string.Concat(name, basename).GetHashCode();
        }
    }
}
*/