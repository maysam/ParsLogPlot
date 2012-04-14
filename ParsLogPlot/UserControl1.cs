using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using System.Resources;
using System.Globalization;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Collections.Specialized;

namespace ParsLogPlot
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            //this.ParentForm
            InitializeComponent();
            obj.Capacity = 100;
        }
        public UserControl1(MainForm _mf)
        {
            InitializeComponent();
            obj.Capacity = 100;
            mf = _mf;
        }
        const int SELECT = 0;
        const int LINE = 1;
        const int RECT = 2;
        const int OVAL = 3;
        const int PIC = 4;
        const int TEXT = 5;

        const int INSERT = 1;
        const int RESIZE = 2;

        const int LEFT = 4;
        const int RIGHT = 5;
        const int TOP = 6;
        const int BOTTOM = 7;

        public float DX = 0, DY = 0;

        int current = SELECT;
        int mode = SELECT;
        action direction = action.None;

        myobject cur = null;
        float dx = 0;
        float dy = 0;
        Image tmpimage = null;

        protected myCollection obj = new myCollection();
        public myCollection collection
        {
            get
            {
                foreach (myobject o in obj)
                    o.unselect();
                return obj;
            }
            set
            {
                if (value == null)
                    return;
                current = SELECT;
                mode = SELECT;
                direction = action.None;
                obj = value;
                foreach (myobject o in obj)
                    o.sethostinfo(pictureBox1,obj);
                pictureBox1.Refresh();
            }
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            current = Convert.ToInt32(((ToolStripButton)sender).Tag);
            mode = INSERT;
            if (cur != null)
                cur.unselect();
            if (current == SELECT)
                mode = SELECT;
            foreach (ToolStripButton tsb in toolStrip2.Items)
                tsb.Checked = false;
            switch (current)
            {
                case SELECT:
                    selectbutton.Checked = true;
                    break;
                case LINE:
                    linebutton.Checked = true;
                    break;
                case RECT:
                    rectbutton.Checked = true;
                    break;
                case OVAL:
                    ovalbutton.Checked = true;
                    break;
                case PIC:
                    picbutton.Checked = true;
                    break;
                case TEXT:
                    textbutton.Checked = true;
                    break;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (cur != null)
            {
                cur.select();
                if (cur is myline)
                    direction = ((myline)cur).getresize(e.X, e.Y);
                if (direction != action.None)
                {
                    mode = RESIZE;
                    dx = e.X;
                    dy = e.Y;
                    return;
                }
                cur.unselect();
            }
            if (mode == SELECT)
            {
                cur = null;
                myobject mine=null;
                for (int k = 0; k < obj.Count; k++) 
                {
                    mine = obj[obj.Count - 1 - k];
                    if (mine is mytext)
                    {
                        if (((mytext)mine).has(e.X, e.Y))
                        {
                            cur = mine;
                            break;
                        }
                    }
                    else
                        if (mine is mypic)
                        {
                            if (((mypic)mine).has(e.X, e.Y))
                            {
                                cur = mine;
                                break;
                            }
                        }
                        else
                            if (mine is myoval)
                            {
                                if (((myoval)mine).has(e.X, e.Y))
                                {
                                    cur = mine;
                                    break;
                                }
                            }
                            else
                                if (mine is myrect)
                                {
                                    if (((myrect)mine).has(e.X, e.Y))
                                    {
                                        cur = mine;
                                        break;
                                    }
                                }
                                else
                                    if (mine is myline)
                                    {
                                        if (((myline)mine).has(e.X, e.Y))
                                        {
                                            cur = mine;
                                            break;
                                        }
                                        }
                }
                if (cur != null)
                    cur.select();
            }
            myobject tmp = null;
            if (mode == INSERT)
            {
                switch (current)
                {
                    case SELECT:
                        break;
                    case LINE:
                        tmp = new myline(e.X, e.Y);
                        break;
                    case RECT:
                        tmp = new myrect(e.X, e.Y);
                        break;
                    case OVAL:
                        tmp = new myoval(e.X, e.Y);
                        break;
                    case PIC:
                        tmp = new mypic(e.X, e.Y);
                        if (tmpimage != null)
                        {
                            (tmp as mypic).Image = tmpimage;
                            tmpimage = null;
                        }
                        break;
                    case TEXT:
                        tmp = new mytext(e.X, e.Y);
                        break;
                }
                tmp.sethostinfo( pictureBox1,obj);
                obj.Add(tmp);
                cur = tmp;
            }
            propertyGrid1.SelectedObject = cur;
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
                if (mode == INSERT)
                {
                    foreach (ToolStripButton tsb in toolStrip2.Items)
                        tsb.Checked = false;
                    selectbutton.Checked = true;
                }
            mode = SELECT;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                if (cur != null)
                {
                    action a = action.None;
                    a=((myline)cur).getresize(e.X, e.Y);
                    switch (a)
                    {
                        case action.Center:
                            pictureBox1.Cursor = Cursors.SizeAll;
                            break;
                        case action.Left:
                        case action.Right:
                            pictureBox1.Cursor = Cursors.SizeWE;
                            break;
                        case action.Top:
                        case action.Down:
                            pictureBox1.Cursor = Cursors.SizeNS;
                            break;
                        case action.RightTop:
                        case action.DownLeft:
                            pictureBox1.Cursor = Cursors.SizeNESW;
                            break;
                        case action.LeftTop:
                        case action.RightDown:
                            pictureBox1.Cursor = Cursors.SizeNWSE;
                            break;
                        default:
                            pictureBox1.Cursor = Cursors.Arrow;                            break;
                   }
                }
                return;
            }
            if (mode == RESIZE)
            {
                ((myline)cur).resize(direction,e.X - dx, e.Y - dy);
                dx = e.X;
                dy = e.Y;
            }
            if (mode == INSERT)
            {
                if (cur != null)
                    (cur as myline).reset(e.X, e.Y);
            }
            pictureBox1.Refresh();
            propertyGrid1.Refresh();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(DX, DY);
            foreach (myobject ob in obj)
                obj.draw(e.Graphics, ob);
            foreach (myline ob in obj)
                ob.drawselection(e.Graphics);
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            StringBuilder sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            ser.Serialize(writer, obj);
//            textBox1.Text = sb.ToString();
            /*
             * 
             * 
        XmlSerializer ser = new XmlSerializer(typeof(Automobile));

        TextReader reader = new StringReader(data);

        return (Automobile) ser.Deserialize(reader);
             * 
             * 
            */
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tmpimage = resourcepool.fossil_column;
            toolStripButton6_Click(sender, e);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            tmpimage = resourcepool.arm;
            toolStripButton6_Click(sender, e);
        }
        public MainForm mf = null;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (mf != null)
            {
                foreach (Column c in mf.u)
                    if(c!=null)
                {
                    mytext mt = new mytext(c.Left, 10);
                    mt.Text = c.Name;
                    mt.reset(c.Width + c.Left, 60);
                    mt.unselect();
                    obj.Add(mt);
                }
            }
        }
    }
    public enum action { None=0, Left=4, DownLeft=7, Down=8, RightDown=9, Right=6, RightTop=3, Top=2, LeftTop=1, Center=5 }
    internal class MyDesigner : ComponentDesigner
        {
            DesignerVerbCollection m_Verbs;
            public override DesignerVerbCollection Verbs { get { if (m_Verbs == null) { m_Verbs = new DesignerVerbCollection(); } return m_Verbs; } }
        }
    [PropertyTab(typeof(System.Windows.Forms.Design.EventsTab), PropertyTabScope.Component)]
    [Designer(typeof(MyDesigner))]
    [Serializable, XmlInclude(typeof(myline)), XmlInclude(typeof(myrect)), XmlInclude(typeof(myoval)), XmlInclude(typeof(mypic)), XmlInclude(typeof(mytext))]
    public class myobject : VerbHoster.VerbHost
    {
        VerbHoster.VerbList vl = new VerbHoster.VerbList();
        public override VerbHoster.VerbList GetVerbs() { return vl; }
        protected myCollection myc = null;
        protected PictureBox mypic = null;
        public void sethostinfo(PictureBox pic, myCollection col)
        {
            myc = col;
            mypic = pic;
        }
        private void delete(VerbHoster.Verb v)
        {
            if (myc != null)
                myc.Remove(this);
            if (mypic != null)
                mypic.Refresh();
        }

        private void bringtofront(VerbHoster.Verb v)
        {
            if (myc != null)
            {
                myc.Remove(this);
                myc.Add(this);
            }
            if (mypic != null)
                mypic.Refresh();
        }

        private void sendtoback(VerbHoster.Verb v)
        {
            if (myc != null)
            {
                int i = 0;
                myCollection m = new myCollection();
                m.Add(this);
                for (i = 0; i < myc.Count ; i++)
                    if (myc[i] != this)
                        m.Add(myc[i]);
                myc.Clear();
                for (i = 0; i < m.Count ; i++)
                    myc.Add(m[i]);
            }
            if (mypic != null)
                mypic.Refresh();
        }

        public myobject()
        {
            VerbHoster.Verb v = new VerbHoster.Verb("Delete");
            v.CallBack += delete;
            vl.Add(v);
            v = new VerbHoster.Verb("Bring To Front");
            v.CallBack += bringtofront;
            vl.Add(v);
            v = new VerbHoster.Verb("Send To Back");
            v.CallBack += sendtoback;
            vl.Add(v);
        }
        protected bool selected = true;
        public void select() { selected = true; }
        public void unselect() { selected = false; }
        public virtual void draw(Graphics g)
        { g.DrawString("place holder", new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(Color.BlanchedAlmond), new PointF(0, 0));
        }
        public virtual bool has(float x, float y)
        { return false; }
        protected Color _forecolor = Color.Black;
        [XmlIgnore]
        public Color ForeColor { get { return _forecolor; } set { _forecolor = value; } }
        [Browsable(false)]
        public Int32 _ForeColor { get { return _forecolor.ToArgb(); } set { _forecolor = Color.FromArgb(value); } }
    }
    [Serializable, XmlInclude(typeof(myrect)), XmlInclude(typeof(myoval)), XmlInclude(typeof(mypic)), XmlInclude(typeof(mytext))]
    public class myline : myobject
    {
        public action getresize(float x, float y)
        {
            int i = 0;
            if (selected)
                foreach (float b in new float[] { Math.Min(y1, y2), Math.Abs(y1 + y2) / 2, Math.Max(y1, y2) })
                    foreach (float a in new float[] { Math.Min(x1, x2), Math.Abs(x1 + x2) / 2, Math.Max(x1, x2) })
                    {
                        i++;
                        if ((x > a - 10) && (x < a + 10) && (y > b - 10) && (y < b + 10))
                            return (action)i;
                    }
            return action.None;
        }
        public void resize(action act, float dx, float dy)
        {
            //correct it
            switch (act)
            {
                case action.Center:
                    x1 += dx;
                    x2 += dx;
                    y1 += dy;
                    y2 += dy;
                    break;
                case action.Left:
                    x1 += dx;
                    //                    x2 += dx;
                    break;
                case action.Right:
                    x2 += dx;
                    break;
                case action.Top:
                    y1 += dy;
                    break;
                case action.Down:
                    y2 += dy;
                    break;
                case action.RightTop:
                    y1 += dy;
                    x2 += dx;
                    break;
                case action.RightDown:
                    y2 += dy;
                    x2 += dx;
                    break;
                case action.LeftTop:
                    y1 += dy;
                    x1 += dx;
                    break;
                case action.DownLeft:
                    x1 += dx;
                    y2 += dy;
                    break;

            }
        }
        public new bool has(float x, float y)
        {
            if ((x >= Math.Min(x1, x2)) & (x <= Math.Max(x1, x2)) & (y >= Math.Min(y1, y2)) & (y <= Math.Max(y1, y2)))
            {
                if ((y != y2) && (y2 != y1))
                    if (Math.Abs((x2 - x) / (y2 - y) - (x2 - x1) / (y2 - y1)) < 0.1F)
                        return true;
                if ((x != x2) && (x2 != x1))
                    if (Math.Abs((y2 - y) / (x2 - x) - (y2 - y1) / (x2 - x1)) < 0.1F)
                        return true;
            }

            return false;
        }
        public myline() { }
        public myline(float a, float b)
        {
            x1 = a;
            y1 = b;
        }
        public void reset(float c, float d)
        {
            x2 = c;
            y2 = d;
        }
        public Rectangle rect() { return new Rectangle((int)Math.Min(x1, x2), (int)Math.Min(y1, y2), (int)Width, (int)Height); }
        public RectangleF rectF() { return new RectangleF(Math.Min(x1, x2), Math.Min(y1, y2), Width, Height); }
        public void drawselection(Graphics g)
        {
            if (selected)
                foreach(float a in new float[]{Math.Min(x1, x2),Math.Abs(x1 + x2) / 2,Math.Max(x1, x2)})
                    foreach (float b in new float[] { Math.Min(y1, y2), Math.Abs(y1 + y2) / 2, Math.Max(y1, y2) })
                        g.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(a - 5, b - 5, 10, 10));
        }
        public new void draw(Graphics g)
        {
            Pen p = new Pen(_forecolor);
            if (selected) p.Width = 2;
            p.DashStyle = ds;
            g.DrawLine(p, x1, y1, x2, y2);
        }
        protected float x1, x2, y1, y2;
        [XmlElement("order=1")]
        public float X1 { get { return x1; } set { x1 = value; } }
        public float Y1 { get { return y1; } set { y1 = value; } }
        public float X2 { get { return x2; } set { x2 = value; } }
        public float Y2 { get { return y2; } set { y2 = value; } }
        [XmlIgnore]
        public float Width { get { return Math.Abs(x2 - x1); } set { if (x2 >= x1) x2 = x1 + value; else x1 = x2 + value; } }
        [XmlIgnore]
        public float Height { get { return Math.Abs(y2 - y1); } set { if (y2 >= y1) y2 = y1 + value; else y1 = y2 + value; } }
        protected PointF[] points() { return new PointF[] { new PointF(x1, y1), new PointF(x2, y1), new PointF(x2, y2), new PointF(x1, y2) }; }
        protected DashStyle ds = DashStyle.Solid;
        public DashStyle Style { get { return ds; } set { ds = value; } }
    }
    [Serializable, XmlInclude(typeof(myoval))]
    public class myrect : myline
    {
        public new bool has(float x, float y)
        {
            if ((x >= Math.Min(x1, x2)) & (x <= Math.Max(x1, x2)) & (y >= Math.Min(y1, y2)) & (y <= Math.Max(y1, y2)))
                return true;
            else
                return false;
        }
        public myrect() { }
        public myrect(float a, float b)
        {
            x1 = a;
            y1 = b;
        }
        protected void getBrush(TotalFillStyle _fillstyle, float HEIGHT, out Brush B)
        {
            switch (_fillstyle.FillMethod)
            {
                case FillStyle.Rainbow:
                    B = new LinearGradientBrush(rectF(), Color.Black, Color.Black, 0, false);
                    ColorBlend cb = new ColorBlend();
                    cb.Positions = new float[7];
                    int i = 0;
                    for (float f = 0; f <= 1; f += .1666f)
                    {
                        cb.Positions[i++] = f;
                    }
                    cb.Positions[6] = 1;
                    cb.Colors = new Color[] { Color.Violet, Color.Indigo, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Red };
                    ((LinearGradientBrush)B).InterpolationColors = cb;
                    break;
                case FillStyle.Pattern:
                    Bitmap b = MainForm.GetResourceBMP(_fillstyle.Pattern);
                    b = myImage.imageutilities.recolor(b, new Color[] { Color.Black, Color.Transparent }, new Color[] { _fillstyle.PrimaryColor, _fillstyle.SecondaryColor });
                    int _zoom = _fillstyle.Zoom;
                    if (_zoom != 1)
                        b = myImage.imageutilities.resize(b, _zoom);// new Bitmap(b, b.Width * _zoom, b.Height * _zoom);

                    B = new TextureBrush(b, WrapMode.Tile);
                    break;
                case FillStyle.Gradient:
                    B = new LinearGradientBrush(rectF(), _fillstyle.PrimaryColor, _fillstyle.SecondaryColor, LinearGradientMode.Horizontal);
                    break;
                case FillStyle.Hatch:
                    B = new HatchBrush(_fillstyle.Hatch, _fillstyle.PrimaryColor, _fillstyle.SecondaryColor);
                    break;
                case FillStyle.Extra:
                    B = new PathGradientBrush(new PointF[] { new PointF(x1, y1), new PointF(x2, y1), new PointF(x2, y2), new PointF(x1, y2) });
                    ((PathGradientBrush)B).SurroundColors = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Green };
                    ((PathGradientBrush)B).CenterColor = Color.Plum;
                    break;
                case FillStyle.SingleColor:
                    B = new SolidBrush(_fillstyle.PrimaryColor);
                    break;
                default:
                    B = new SolidBrush(_fillstyle.SecondaryColor);
                    break;
            }
        }
        public new void draw(Graphics g)
        {
            if (fill)
            {
                Brush b = new SolidBrush(_forecolor);
                getBrush(tfs, Height, out b);
                g.FillRectangle(b, rect());
            }
            Pen p = new Pen(_forecolor);
            if (selected) p.Width = 2;
            p.DashStyle = ds;
            g.DrawRectangle(p, rect());
        }
        protected bool fill = true;
        public bool Fill { get { return fill; } set { fill = value; } }
        protected TotalFillStyle tfs = new TotalFillStyle();
        [DisplayName("Fill Style")]
        public TotalFillStyle Tfs { get { return tfs; } set { tfs = value; } }
    }
    [Serializable]
    public class myoval : myrect
    {
        public new bool has(float x, float y)
        {
            if ((x >= Math.Min(x1, x2)) & (x <= Math.Max(x1, x2)) & (y >= Math.Min(y1, y2)) & (y <= Math.Max(y1, y2)))
                return true;
            else
                return false;
        }
        public myoval() { }
        public myoval(float a, float b)
        {
            x1 = a;
            y1 = b;
        }
        public new void draw(Graphics g)
        {
            if (fill)
            {
                Brush b = new SolidBrush(_forecolor);
                getBrush(tfs, Height, out b);
                g.FillEllipse(b, rectF());
            }
            Pen p = new Pen(_forecolor);
            if (selected) p.Width = 2;
            p.DashStyle = ds;
            g.DrawEllipse(p, rectF());
        }
    }
    [Serializable]
    public class mypic : myline
    {
        public new bool has(float x, float y)
        {
            if ((x >= Math.Min(x1, x2)) & (x <= Math.Max(x1, x2)) & (y >= Math.Min(y1, y2)) & (y <= Math.Max(y1, y2)))
                return true;
            else
                return false;
        }
        public mypic() { }
        public mypic(float a, float b)
        {
            x1 = a;
            y1 = b;
        }
        protected bool _resize = true;
        public bool Resize { get { return _resize; } set { _resize = value; } }
        public new void draw(Graphics g)
        {
            if (_resize)
                g.DrawImage(_image, rect());
            else
                g.DrawImageUnscaledAndClipped(_image, rect());
            Pen p = new Pen(_forecolor);
            if (selected) p.Width = 2;
            p.DashStyle = ds;
            g.DrawRectangle(p, rect());
        }

        protected Image _image = new Bitmap(10, 10);
        [XmlIgnore]
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
        [XmlElementAttribute("Picture")]
        [Browsable(false)]
        public byte[] PictureByteArray
        {
            get
            {
                if (_image != null)
                {
                    TypeConverter BitmapConverter =
                        TypeDescriptor.GetConverter(_image.GetType());
                    return (byte[])BitmapConverter.ConvertTo(_image, typeof(byte[]));
                }
                else
                    return null;
            }

            set
            {
                if (value != null)
                    _image = new Bitmap(new MemoryStream(value));
                else
                    _image = null;
            }
        }
    }
    [Serializable]
    public class mytext : myline
    {
        public mytext() { }
        public new bool has(float x, float y)
        {
            if ((x >= Math.Min(x1, x2)) & (x <= Math.Max(x1, x2)) & (y >= Math.Min(y1, y2)) & (y <= Math.Max(y1, y2)))
                return true;
            else
                return false;
        }
        public mytext(float a, float b)
        {
            x1 = a;
            y1 = b;
        }
        public new void draw(Graphics g)
        {
            if (horizontal == false)
            {
                g.TranslateTransform(Math.Min(x1, x2), Math.Max(y1, y2));
                g.RotateTransform(-90);
                g.DrawString(text, _font, new SolidBrush(_forecolor), new RectangleF(0, 0, Height, Width));
                g.RotateTransform(90);
                g.TranslateTransform(-Math.Min(x1, x2), -Math.Max(y1, y2));
            }
            else
            {
                g.DrawString(text, _font, new SolidBrush(_forecolor), rect());
            }
            Pen p = new Pen(_forecolor);
            if (selected) p.Width = 2;
            p.DashStyle = ds;
            g.DrawRectangle(p, rect());
        }
        protected Font _font = new Font(FontFamily.GenericSansSerif, 10);
        [XmlIgnore, DisplayName("Font"), Editor(typeof(FontEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Font _Font { get { return _font; } set { _font = value; } }
        [Browsable(false)]
        public string fonts
        {
            get { FontConverter f = new FontConverter(); return f.ConvertToString(_font); }
            set { FontConverter f = new FontConverter(); _font = (Font)f.ConvertFromString(value); }
        }
        private string text = "text1";
        public string Text { get { return text; } set { text = value; } }
        protected bool horizontal = true;
        public bool Horizontal
        {
            get { return horizontal; }
            set { horizontal = value; }
        }

    }
    [Serializable]
    public class myCollection : System.Collections.CollectionBase
    {
        public myCollection() : base() {}
        public void Add(myobject str) {
            foreach (myobject k in List)
                k.unselect();
            base.List.Add(str);
        }
        public Bitmap bitmap()
        {
            Rectangle rect = new Rectangle(0, 0, 1, 1);
            foreach (myline k in List)
                rect = Rectangle.Union(rect, k.rect());
            Bitmap b = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(b);
            foreach (myobject ob in List)
                draw(g, ob);
            //g.DrawRectangle(Pens.Black, rect);
            return b;
        }
        public void draw(Graphics g, myobject mine)
        {
            if (mine is mytext)
                ((mytext)mine).draw(g);
            else
                if (mine is mypic)
                    ((mypic)mine).draw(g);
                else
                    if (mine is myoval)
                        ((myoval)mine).draw(g);
                    else
                        if (mine is myrect)
                            ((myrect)mine).draw(g);
                        else
                            if (mine is myline)
                                ((myline)mine).draw(g);
                            else
                                mine.draw(g);
        }
        public bool Contains(myobject str) { return base.List.Contains(str); }
        public void Remove(myobject str) { base.List.Remove(str); }
        [XmlArray("items"), XmlArrayItem("item", typeof(myobject))]
        public myobject this[int index] { get { return (myobject)base.List[index]; } set { base.List[index] = value; } }
        public override string ToString() { return base.List.Count + " objects"; }
    }

}
