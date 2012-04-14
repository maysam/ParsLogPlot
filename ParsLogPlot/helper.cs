using System;
using System.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Reflection;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Security.Permissions;
using System.Diagnostics;

namespace ParsLogPlot
{
    public partial class MainForm : Form
    {
    #region movement
        private void pn_MouseUp(object sender, MouseEventArgs e)
        {
            rightresize = false;
            leftresize = false;
            dodrag = false;
            fresh();
        }
        private void pn_Move(object sender, EventArgs e)
        {

            String ta = ((PictureBox)sender).Tag.ToString();
            int c = Convert.ToInt32(ta);
            valrec(c);
            synccolumns(c, true);
            fresh(c);
        }
        private void pn_Resize(object sender, EventArgs e)
        {

            String ta = ((PictureBox)sender).Tag.ToString();
            int c = Convert.ToInt32(ta);
            valrec(c);
            synccolumns(c, true);
            fresh(c);
        }
        private void pn_MouseDown(object sender, MouseEventArgs e)
        {
            String ta = ((PictureBox)sender).Tag.ToString();
            ID = Convert.ToInt32(ta);
            rightresize = false;
            leftresize = false;
            dodrag = false;
            fresh();
        }
        private void pn_Click(object sender, EventArgs e)
        {
            String ta = ((PictureBox)sender).Tag.ToString();
            ID = Convert.ToInt32(ta);
            rightresize = false;
            leftresize = false;
            dodrag = false;
            fresh();
        }
        private void pn_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            if (e.Button == MouseButtons.None)
            {
                if (dodrag | leftresize | rightresize)
                {
                    rightresize = false;
                    leftresize = false;
                    dodrag = false;
                    valrec(ID);
                }
            }
            else
            {
                String ta = pic.Tag.ToString();
                ID = Convert.ToInt32(ta);
                if (!(dodrag | leftresize | rightresize))
                {
                    if (Math.Abs(e.X - p[ID].Width) < 10)
                    {
                        rightresize = true;
                    }
                    else
                        if (Math.Abs(e.X) < 10)
                        {
                            leftresize = true;
                        }
                        else
                        {
                            dodrag = true;
                            dx = e.X;
                        }

                }
            }
            MouseEventArgs me = new MouseEventArgs(e.Button, e.Clicks, e.X + pic.Left, e.Y + pic.Top, e.Delta);
            if (dodrag)
                pic.Cursor = Cursors.Hand;
            else
                if ((e.X < 10) | (e.X > pic.Width - 10) | rightresize | leftresize)
                    pic.Cursor = Cursors.SizeWE;
                else
                    pic.Cursor = Cursors.Hand;
            Form1_MouseMove(sender, me);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ID >= 0)
            {
                if (e.Button == MouseButtons.None)
                {
                    if (dodrag | leftresize | rightresize)
                    {
                        rightresize = false;
                        leftresize = false;
                        dodrag = false;
                        valrec(ID);
                    }
                }
                else
                    if (dodrag)
                    {
                        p[ID].SetBounds(e.X - dx, 0, 0, 0, BoundsSpecified.X);
                    }
                    else
                        if (rightresize)
                        {
                                if (e.X - p[ID].Left > 20)
                                    p[ID].SetBounds(0, 0, e.X - p[ID].Left, 0, BoundsSpecified.Width);
                        }
                        else
                            if (leftresize)
                                {
                                    if (p[ID].Right - e.X > 20)
                                        p[ID].SetBounds(e.X, 0, p[ID].Right - e.X, 0, BoundsSpecified.X | BoundsSpecified.Width);
                                }
            }
            fresh();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dodrag = false;
            rightresize = false;
            leftresize = false;
            fresh();
        }
#endregion
    #region update content
        public void valrec(int min, int max)
        {
            if(max<min)return;
            CONTENT.Invalidate(new Rectangle(min, 0, max-min, CONTENT.Height));
        }
        public void valrec(Column p)
        {
            valrec(p.Left, p.Right);
        }
        public void valrec(int i)
        {
            if ((i >= 0) & (i <= count))
            {
                valrec(u[i]);
                idl = i;
            }
        }
    #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            splash.SetBounds((this.Width - splash.Width) / 2, (this.Height - splash.Height) / 2, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            splash.Show();
            SuspendLayout();
            initializingthread.RunWorkerAsync();
        }
        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
        }
        private void toolStripButton1_DisplayStyleChanged(object sender, EventArgs e)
        {
            toolStripButton2.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton3.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton4.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton5.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton6.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton7.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton8.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton9.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton10.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton11.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton12.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton13.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton14.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton15.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton16.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton17.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton18.DisplayStyle = toolStripButton1.DisplayStyle;
        }
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
        }
        private void imageTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        }
        #region temp
        public static void OutputGradientImage()
        {
            using (Bitmap bitmap = new Bitmap(25, 75))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Rectangle(0, 0, 25, 75),
                Color.OliveDrab,
                Color.LightGreen,
                LinearGradientMode.Vertical))
            {
                brush.SetSigmaBellShape(0.25f, 0.75f);
                graphics.FillRectangle(brush, new Rectangle(0, 0, 25, 75));
                bitmap.Save("gradient.jpg", ImageFormat.Jpeg);
            }
        }
        private void cacheDisplayOrder()
        {
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForAssembly();
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("DisplayCache", FileMode.Create, isoFile))
            {
                int[] displayIndices = new int[dataGridView1.ColumnCount];
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    displayIndices[i] = dataGridView1.Columns[i].DisplayIndex;
                }
                XmlSerializer ser = new XmlSerializer(typeof(int[]));
                ser.Serialize(isoStream, displayIndices);
            }
        }
        private void SetDisplayOrder()
        {
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForAssembly();
            string[] fileNames = isoFile.GetFileNames("*");
            bool found = false;
            foreach (string filename in fileNames)
            {
                if (filename == "DisplayCache")
                    found = true;
            }
            if (!found)
                return;
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("DisplayCache", FileMode.Open, isoFile))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(int[]));
                    int[] displayIndices = (int[])ser.Deserialize(isoStream);
                    for (int i = 0; i < displayIndices.Length; i++)
                    {
                        dataGridView1.Columns[i].DisplayIndex = displayIndices[i];
                    }
                }
                catch { }
            }
        }
        public class PropertyGridManipulator
        {
            public static void MoveSplitter(PropertyGrid propertyGrid, int x)
            {
                object propertyGridView = typeof(PropertyGrid).InvokeMember("gridView", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGrid, null);
                propertyGridView.GetType().InvokeMember("MoveSplitterTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGridView, new object[] { x });
            }
            public static void MoveSplitterToLongestDisplayName(PropertyGrid propertyGrid, int iPadding)
            {
                try
                {
                    Type pgObjectType = propertyGrid.SelectedObject.GetType();
                    string longestDisplayName = "";
                    // Iterate through all the properties of the class. 
                    foreach (PropertyInfo mInfo in pgObjectType.GetProperties())
                    {
                        // Iterate through all the Attributes for each property. 
                        foreach (Attribute attr in mInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false))
                        {
                            // Check for the AnimalType attribute. 
                            if (attr.GetType() == typeof(DisplayNameAttribute))
                            {
                                DisplayNameAttribute displayNameAttr = (DisplayNameAttribute)attr;
                                if (displayNameAttr.DisplayName.Length > longestDisplayName.Length)
                                {
                                    longestDisplayName = displayNameAttr.DisplayName;
                                }
                            }
                        }
                    }
                    Size textSize = TextRenderer.MeasureText(longestDisplayName, propertyGrid.Font);
                    PropertyGridManipulator.MoveSplitter(propertyGrid, textSize.Width + iPadding);
                }
                catch (Exception exception1)
                {
                    MessageBox.Show(exception1.Message);
                }
            }//end public static void MoveSplitterToLongestDisplayName(PropertyGrid propertyGrid, int iPadding) 
        }
        #endregion
        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            PropertyGridManipulator.MoveSplitterToLongestDisplayName(propertyGrid1, 10);
        }
        public MainForm()
        {
            InitializeComponent();
        }
        public BindingSource[] bs = new BindingSource[18];
        PictureBox[] p = new PictureBox[100];
        Bitmap[] bmp = new Bitmap[100];
        Column[] u = new Column[100];
        int count = -1;
        int dx = 0;
        bool dodrag = false;
        bool rightresize = false;
        bool leftresize = false;
        public int id = -1;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                if (value == id) return;
                if (value > count) return;
                if (u[value] == null) return;
                dodrag = false;
                rightresize = false;
                leftresize = false;
                fresh();
                if (value == -1)
                {
                    propertyGrid1.SelectedObject = null;
                }
                else
                {
                    p[value].BringToFront();
                    entityBindingSource.Position = entityBindingSource.Find("id", u[value].entity);
                    dataGridView1.DataSource = bs[(Int32)u[value].Type];
                    propertyGrid1.SelectedObject = u[value];
                }
                id = value;
                u[id].activate(dataGridView1);
                fresh();
            }
        }

        private void MoveSplitter(PropertyGrid propertyGrid, int x)
        {
            object propertyGridView = typeof(PropertyGrid).InvokeMember("gridView", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGrid, null);
            propertyGridView.GetType().InvokeMember("MoveSplitterTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGridView, new object[] { x });
        }

        private void fresh(int id)
        {
            valrec(id);
            if (id == -1)
                CONTENT.Invalidate();
            fresh();
        }
        private void fresh()
        {
            CONTENT.Update();
            splitContainer2.Panel1.Refresh();            
        }

        public void synccolumns(int i, bool n2u)
        {
            valrec(Math.Min(u[i].Left, p[i].Left), Math.Max(u[i].Right, p[i].Right));
            CONTENT.Update();
            if (n2u)
            {
                u[i].Left = p[i].Left;
                u[i].Width = p[i].Width;
                propertyGrid1.Refresh();
            }
            else
            {
                p[i].SetBounds(u[i].Left, 0, u[i].Width, 0, BoundsSpecified.X | BoundsSpecified.Width);
            }
        }
    }

    [Serializable(), TypeConverterAttribute(typeof(GridStyleConverter)), DescriptionAttribute("Expand to see the Grid Style options.")]
    public class GridStyle
    {
        bool _majorinterval = true;
        int _majordivision = 50;
        LineStyle _majorlinestyle = new LineStyle();
        bool _minorinterval = true;
        int _minordivision = 10;
        LineStyle _minorlinestyle = new LineStyle();
        public bool MajorInterval { get { return _majorinterval; } set { _majorinterval = value; } }
        public int MajorDivision { get { return _majordivision; } set { _majordivision = value; } }
        public bool MinorInterval { get { return _minorinterval; } set { _minorinterval = value; } }
        public int MinorDivision { get { return _minordivision; } set { _minordivision = value; } }
        public LineStyle MajorLineStyle { get { return _majorlinestyle; } set { _majorlinestyle = value; } }
        public LineStyle MinorLineStyle { get { return _minorlinestyle; } set { _minorlinestyle = value; } }
    }
    public class GridStyleConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(GridStyle)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is GridStyle)
            {
                GridStyle so = (GridStyle)value;
                return "Major Interval: " + so.MajorInterval.ToString() +
                       ", Major Devision:  " + so.MajorDivision.ToString() +
                        ", Minor Interval: " + so.MinorInterval.ToString() +
                       ", Minor Devision: " + so.MinorDivision.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    int colon = s.IndexOf(':');
                    int comma = s.IndexOf(',');
                    if (colon != -1 && comma != -1)
                    {
                        string s1 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        comma = s.IndexOf(',', comma + 1);
                        string s2 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        comma = s.IndexOf(',', comma + 1);
                        string s3 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        string s4 = s.Substring(colon + 1);
                        GridStyle so = new GridStyle();
                        so.MajorInterval = bool.Parse(s1);
                        so.MajorDivision = Int32.Parse(s2);
                        so.MinorInterval = bool.Parse(s3);
                        so.MinorDivision = Int32.Parse(s4);
                        return so;
                    }
                }
                catch { throw new ArgumentException("Can not convert '" + (string)value + "' to type GridStyle"); }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    [Serializable(), TypeConverterAttribute(typeof(LineStyleConverter)), DescriptionAttribute("Expand to see the Line Style options.")]
    public class LineStyle
    {
        int _Thickness = 1;
        Color _forecolor;
        DashStyle _Dash = DashStyle.Solid;
        public int Thickness { get { return _Thickness; } set { _Thickness = value; } }
        [XmlIgnore]
        public Color ForeColor { get { return _forecolor; } set { _forecolor = value; } }
        [Browsable(false)]
        public Int32 _ForeColor { get { return _forecolor.ToArgb(); } set { _forecolor = Color.FromArgb(value); } }
        public DashStyle Style { get { return _Dash; } set { _Dash = value; } }
    }
    public class LineStyleConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(LineStyle)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is LineStyle)
            {
                LineStyle so = (LineStyle)value;
                return "Thickness: " + so.Thickness.ToString() +
                       ", ForeColor: " + so.ForeColor.ToString() +
                        ", DashStyle: " + so.Style.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    int colon = s.IndexOf(':');
                    int comma = s.IndexOf(',');
                    if (colon != -1 && comma != -1)
                    {
                        string s1 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        comma = s.IndexOf(',', comma + 1);
                        string s2 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        string s3 = s.Substring(colon + 1);
                        LineStyle so = new LineStyle();
                        so.Thickness = Int32.Parse(s1);
                        so.ForeColor = Color.FromName(s2);
                        so.Style = (DashStyle)Int32.Parse(s3);
                        return so;
                    }
                }
                catch { throw new ArgumentException("Can not convert '" + (string)value + "' to type LineStyle"); }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    [Serializable(), TypeConverterAttribute(typeof(TwoGridStyleConverter)), DescriptionAttribute("Expand to see the Complete Grid Style options.")]
    public class TwoGridStyle
    {
        bool _plotvaluegrid = true;
        bool _plotdepthgrid = true;
        bool _gridontop = false;
        GridStyle _valuegridstyle = new GridStyle();
        GridStyle _depthgridstyle = new GridStyle();
        public bool PlotValueGrid { get { return _plotvaluegrid; } set { _plotvaluegrid = value; } }
        public bool PlotDepthGrid { get { return _plotdepthgrid; } set { _plotdepthgrid = value; } }
        public bool GridOnTop { get { return _gridontop; } set { _gridontop = value; } }
        public GridStyle ValueGridStyle { get { return _valuegridstyle; } set { _valuegridstyle = value; } }
        public GridStyle DepthGridStyle { get { return _depthgridstyle; } set { _depthgridstyle = value; } }
    }
    public class TwoGridStyleConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(TwoGridStyle)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is TwoGridStyle)
            {
                TwoGridStyle so = (TwoGridStyle)value;
                return "Plot Value Grid: " + so.PlotValueGrid.ToString() +
                       ", Plot Depth Grid: " + so.PlotDepthGrid.ToString() +
                        ", Grid On Top: " + so.GridOnTop.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    int colon = s.IndexOf(':');
                    int comma = s.IndexOf(',');
                    if (colon != -1 && comma != -1)
                    {
                        string s1 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        comma = s.IndexOf(',', comma + 1);
                        string s2 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        string s3 = s.Substring(colon + 1);
                        TwoGridStyle so = new TwoGridStyle();
                        so.PlotValueGrid = Boolean.Parse(s1);
                        so.PlotDepthGrid = Boolean.Parse(s1);
                        so.GridOnTop = Boolean.Parse(s1);
                        return so;
                    }
                }
                catch { throw new ArgumentException("Can not convert '" + (string)value + "' to type Complete GridStyle"); }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public enum ColumnType
    {
        LithologyPattern = 0,
        LithologyDescription = 1,
        ScaleBar = 2,
        Curve = 3,
        CrossPlotCurve = 4,
        Percent = 5,
        Histogram = 6,
        HistogramValue = 7,
        Text = 8,
        VerticalText = 9,
        Symbol = 10,
        FillBar = 11,
        Bitmap = 12,
        WellConstruction = 13,
        HorizontalLogBody = 14,
        VerticalLogBody = 15,
        Tadpole = 16,
        IntervalText = 17
    }
    
    [Serializable()]
    public class LithologyTemplate
    {

        protected string _name;
        protected int _pattern;
        protected Color _forecolor;
        protected Color _backcolor;


        public LithologyTemplate()
        {
            _name = "";
            _pattern = 0;
            _forecolor = Color.Black;
            _backcolor = Color.Transparent;
            maketexture();
        }

        public override string ToString()
        {
            return _name;
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected TextureBrush tb;

        [XmlIgnore,Browsable(false)]
        public TextureBrush Texture { get { return tb; } set { tb = value; } }

        protected void maketexture()
        {
            Bitmap bs = MainForm.GetResourceBMP(_pattern);
            bs = myImage.imageutilities.recolor(bs, new Color[] { Color.Black, Color.Transparent }, new Color[] { _forecolor, _backcolor });
            tb = new TextureBrush(bs);
        }

        [Editor(typeof(ContrastEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public int pattern { get { return _pattern; } set { _pattern = value; maketexture(); } }
        [XmlIgnore]
        public Color ForeColor { get { return _forecolor; } set { _forecolor = value; maketexture(); } }
        [XmlIgnore]
        public Color BackColor { get { return _backcolor; } set { _backcolor = value; maketexture(); } }
        [Browsable(false)]
        public Int32 iForeColor { get { return _forecolor.ToArgb(); } set { _forecolor = Color.FromArgb(value); maketexture(); } }
        [Browsable(false)]
        public Int32 iBackColor { get { return _backcolor.ToArgb(); } set { _backcolor = Color.FromArgb(value); maketexture(); } }
        public bool Fill { get { return _forecolor == _backcolor; } set { if (value) _backcolor = _forecolor; maketexture(); } }
    }
    [Serializable()]
    public class LithologyCollection : System.Collections.CollectionBase
    {
        public LithologyCollection()
            : base()
        {

        }

        public void Add(LithologyTemplate str)
        {
            base.List.Add(str);
        }

        public bool Contains(LithologyTemplate str)
        {
            return base.List.Contains(str);
        }

        public void Remove(LithologyTemplate str)
        {
            base.List.Remove(str);
        }

        public LithologyTemplate this[int index]
        {
            get { return (LithologyTemplate)base.List[index]; }
            set { base.List[index] = value; }
        }

        public override string ToString()
        {
            return base.List.Count + " LithologyTemplates";
        }
    }
    [Serializable(), TypeConverterAttribute(typeof(ExpandableObjectConverter)), DescriptionAttribute("How to paint each lithology.")]
    public class LithologySettings
    {

        protected LithologyCollection _lits;

        public LithologySettings()
        {
            _lits = new LithologyCollection();
        }

        public LithologyCollection Lithologies
        {
            get { return _lits; }
            set { _lits = value; }
        }
    }
    public class KeyValueColumnConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(KeyValueColumn)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is KeyValueColumn)
            {
                KeyValueColumn so = (KeyValueColumn)value;
                return "Min: " + so.Min.ToString() + ", Max: " + so.Max.ToString();
                //+ ", ForeColor: " + so.ForeColor.ToString() + ", DashStyle: " + so.Style.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    int colon = s.IndexOf(':');
                    int comma = s.IndexOf(',');
                    if (colon != -1 && comma != -1)
                    {
                        string s1 = s.Substring(colon + 1, (comma - colon - 1));
                        colon = s.IndexOf(':', comma + 1);
                        string s2 = s.Substring(colon + 1);
                        KeyValueColumn so = new KeyValueColumn();
                        so.Min = Int32.Parse(s1);
                        so.Max = Int32.Parse(s2);
                        return so;
                    }
                }
                catch { throw new ArgumentException("Can not convert '" + (string)value + "' to type KeyValueColumn"); }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public enum FitStyleType { Scale, Trim, Wrap, Wrap10 }
    public enum LowHighStyleType { LowToHigh, HighToLow }
    public enum LeftRightStyleType { LeftToRight, RightToLeft }
    [Serializable()]
    public class KeyValueColumn : FilledColumn
    {
        protected int _min = 0, _max = 100;
        protected FitStyleType _fitstyle = FitStyleType.Trim;
        protected LowHighStyleType _lowhighstyle = LowHighStyleType.LowToHigh;
        protected LeftRightStyleType _leftrightstyle = LeftRightStyleType.LeftToRight;
        [Category("Settings"), PasswordPropertyText(false)]
        public int Min { get { return _min; } set { _min = value; } }
        [Category("Settings")]
        public int Max { get { return _max; } set { _max = value; } }
        [Category("Settings")]
        public FitStyleType FitStyle { get { return _fitstyle; } set { _fitstyle = value; } }
        [Category("Settings")]
        public LowHighStyleType LowHighStyle { get { return _lowhighstyle; } set { _lowhighstyle = value; } }
        [Category("Settings")]
        public LeftRightStyleType LeftRightStyle { get { return _leftrightstyle; } set { _leftrightstyle = value; } }
        //        public int 1 { get { return _1; } set { _1 = value; } }
    }
    [Serializable()]
    public class CurveColumn : KeyValueColumn
    {
        protected bool _filled = false;
        [Category("Settings"), DisplayName("Grid Style")]
        public bool Filled { get { return _filled; } set { _filled = value; } }
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.curve.Select(" entity = " + entity.ToString() + " and depth<=" + BottomLimit.ToString() + " and depth>=" + TopLimit.ToString());
            int i = 0;
            int length = drs.Length;
            if (_filled)
                length += 2;
            PointF[] ps = new PointF[length];
            foreach (logplotDataSet.curveRow dr in drs)
            {
                if (i == 0)
                    if (_filled)
                        ps[i++] = new PointF(_min, (float)dr.Depth * zoom);
                ps[i] = new PointF((float)dr.Value, (float)dr.Depth * zoom);
                KeyValueColumn kv = (KeyValueColumn)this;
                if (kv.Min > ps[i].X)
                    ps[i].X = kv.Min;
                if (kv.Max < ps[i].X)
                    ps[i].X = kv.Max;
                RectangleF rec = new RectangleF(ps[i], new Size(4, 4));
                rec.Offset(-2, -2);
                /*
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X, ps[i].Y + 2));
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X, ps[i].Y - 2));
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X - 2, ps[i].Y));
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X + 2, ps[i].Y));
                g.DrawEllipse(new Pen(B), rec);
                 */ 
                i++;
                if (i == length-1)
                    if (_filled)
                        ps[i++] = new PointF(_min, (float)dr.Depth * zoom);
            }
            //                        Array.Resize<PointF>(ref ps, i);
            Array.Sort<PointF>(ps, new PointFComparer());
            if (i >= 1)
            //                            e.Graphics.DrawCurve(Pens.ForestGreen, ps);
            {
                if (_filled)
                {
                    
                    g.FillClosedCurve(B, ps, FillMode.Alternate, 1.5f);
                }
                else
                    g.DrawCurve(new Pen(B), ps);
            }
            //g.DrawLines(Pens.DarkKhaki, ps);

        }
    }
    [Serializable()]
    public class CrossPlotCurveColumn : KeyValueColumn
    {
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            PointF[] ps = new PointF[lds.crossplot.Count];
            PointF[] pt = new PointF[lds.crossplot.Count];
            PointF[] tpt = new PointF[lds.crossplot.Count];
            int i = 0;
            foreach (logplotDataSet.crossplotRow dr in lds.crossplot.Select(" entity = " + entity.ToString() + " and depth<=" + BottomLimit.ToString() + " and depth>=" + TopLimit.ToString()))
            {
                ps[i] = new PointF((float)dr.CrossPlot1, (float)dr.Depth * zoom);
                pt[i++] = new PointF((float)dr.CrossPlot2, (float)dr.Depth * zoom);
            }
            if (i > 2)
            {
                Array.Resize<PointF>(ref ps, i);
                Array.Resize<PointF>(ref pt, i);
                Array.Resize<PointF>(ref tpt, i * 2);
                Array.Sort<PointF>(ps, new PointFComparer());
                Array.Sort<PointF>(pt, new PointFComparer());
                g.DrawCurve(Pens.Black, ps);
                g.DrawCurve(Pens.Black, pt);
                //                            Array.Reverse(pt);
                //                        PointF[] pq;
                //Array.Copy(pt, ps, pt.Length);
                for (int counter = 0; counter < i; counter++)
                {
                    tpt[counter] = ps[counter];
                    tpt[i + counter] = pt[i - 1 - counter];
                    //                                tpt[i + counter] = pt[i - counter];
                }
                //tpt[2 * i] = tpt[0];
                // if (i >= 2)
                //  e.Graphics.DrawCurve(Pens.ForestGreen, ps);
                //g.FillClosedCurve(br, ps);
                //g.DrawLines(Pens.Red, tpt);
                //g.FillPolygon(br, tpt, FillMode.Winding);
                g.FillClosedCurve(B, tpt);
                /*
                g.FillPolygon(br, ps, FillMode.Alternate);
                g.FillPolygon(br, pt, FillMode.Alternate);
                 */
                //                          g.DrawLines(Pens.DarkKhaki, ps);
            }
        }
    }

    public enum FillStyle { SingleColor, Pattern, Gradient, Rainbow, Hatch, Extra }
public class ContrastEditor : UITypeEditor
{
public override bool GetPaintValueSupported(ITypeDescriptorContext context)
{
return true;
}

public override void PaintValue(PaintValueEventArgs e)
{
Bitmap newImage = MainForm.GetResourceBMP((int)e.Value);
//(Bitmap)resourcepool.ResourceManager.GetObject("pat" + e.Value);
TextureBrush tb = new TextureBrush(newImage);
//    e.Graphics.DrawImage(newImage, e.Bounds);
//                e.Graphics.FillRectangle(tb, new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width * 4, e.Bounds.Height)));
e.Graphics.FillRectangle(tb, e.Bounds);
}
public override UITypeEditorEditStyle GetEditStyle(
ITypeDescriptorContext context)
{
return UITypeEditorEditStyle.DropDown;
}

public override object EditValue(ITypeDescriptorContext context,
IServiceProvider provider, object value)
{
IWindowsFormsEditorService wfes = provider.GetService(
    typeof(IWindowsFormsEditorService)) as
    IWindowsFormsEditorService;

if (wfes != null)
{
/*
* frmContrast _frmContrast = new frmContrast();
    _frmContrast.BarValue = (int)value;
    _frmContrast._wfes = wfes;
    _frmContrast.rearrange(1);
    wfes.DropDownControl(_frmContrast.imageListBox1);
    value = _frmContrast.BarValue;
*/
                resourcemanager.Form1 rm = new resourcemanager.Form1((int)value, 0);
                if (rm.ShowDialog() == DialogResult.OK)
                {
                    value = rm.pattern;
                }
                rm.Dispose();
            }
            return value;
        }
    }
    [Serializable(), TypeConverterAttribute(typeof(ExpandableObjectConverter)), DescriptionAttribute("Expand to see the Complete Fill Style options.")]
    public class TotalFillStyle
    {
        protected FillStyle _fillstyle = FillStyle.SingleColor;
        [DisplayName("Fill Style")]
        public FillStyle FillMethod { get { return _fillstyle; } set { _fillstyle = value; } }
        protected int _pattern;
        protected int _zoom = 1;
        protected Color _StartColor = Color.Blue, _EndColor = Color.Red;
        protected HatchStyle _Hatch;
        public HatchStyle Hatch { get { return _Hatch; } set { _Hatch = value; } }
        [Editor(typeof(ContrastEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public int Pattern { get { return _pattern; } set { _pattern = value; } }
        public int Zoom { get { return _zoom; } set { _zoom = value; } }
        [XmlIgnore]
        public Color PrimaryColor { get { return _StartColor; } set { _StartColor = value; } }
        [XmlIgnore]
        public Color SecondaryColor { get { return _EndColor; } set { _EndColor = value; } }
        [Browsable(false)]
        public Int32 iStartColor { get { return _StartColor.ToArgb(); } set { _StartColor = Color.FromArgb(value); } }
        [Browsable(false)]
        public Int32 iEndColor { get { return _EndColor.ToArgb(); } set { _EndColor = Color.FromArgb(value); } }
    }
    [Serializable()]
    public class FilledColumn : Column
    {
        protected TotalFillStyle tfs = new TotalFillStyle();
        [CategoryAttribute("Appearance"), DescriptionAttribute("How to fill the column?")]
        public TotalFillStyle FillStyle { get { return tfs; } set { tfs = value; } }
    }
    [Serializable()]
    public class HistogramColumn : FilledColumn
    {
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            foreach (logplotDataSet.histogramRow dr in lds.histogram.Select(" entity = " + entity.ToString() + " and top<=" + BottomLimit.ToString() + " and base>=" + TopLimit.ToString()))
                g.FillRectangle(B, 0, (float)dr.Top * zoom, (float)dr.Value, (float)(dr.Base - dr.Top) * zoom);
       }
    }
    [Serializable()]
    public class HistogramValueColumn : FilledColumn
    {
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            foreach (logplotDataSet.histogramRow dr in lds.histogram.Select(" entity = " + entity.ToString() + " and top<=" + BottomLimit.ToString() + " and base>=" + TopLimit.ToString()))
            {
                g.DrawRectangle(new Pen(_forecolor), 1, (float)(dr.Top * zoom)+1, _width-2, (float)((dr.Base - dr.Top) * zoom)-2);
                g.DrawString(dr.Value.ToString(), new Font(FontFamily.GenericSansSerif, 13), B, new PointF(0, (float)(dr.Top + (dr.Base - dr.Top) / 2) * zoom));
            }
        }
    }
    [Serializable()]
    public class LithologyColumn : FilledColumn
    {
        public override void activate(DataGridView dgv)
        {
            base.activate(dgv);
            string s = "Lithology";
            if (dgv.Columns.Contains(s))
            {
                DataGridViewImageColumn sc = new DataGridViewImageColumn();
                sc.DataPropertyName = s;
                sc.HeaderText = s;
                sc.Name = s;
                sc.ImageLayout = DataGridViewImageCellLayout.Zoom;
                int i = dgv.Columns[s].Index;
                dgv.Columns.Remove(s);
                dgv.Columns.Insert(i, sc);
            }
        }
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            {
                foreach (logplotDataSet.lithologyRow dr in lds.lithology.Select(" entity = " + entity.ToString() + " and top<=" + BottomLimit.ToString() + " and base>=" + TopLimit.ToString()))
                {
                    {
                        LithologyTemplate lt = getLithologyTemplate(LithologySetting, Int32.Parse(dr.Lithology));
                        TextureBrush tb;
                        if (lt != null)
                            tb = lt.Texture;
                        else
                        {
                            Bitmap b = MainForm.GetResourceBMP(int.Parse(dr.Lithology));
                            b = myImage.imageutilities.recolor(b, new Color[] { Color.Black, Color.Transparent }, new Color[] { _forecolor, _backcolor });
                            tb = new TextureBrush(b);
                        }
                        g.FillRectangle(tb, 20, (float)dr.Top * zoom, _width-20, (float)(dr.Base - dr.Top) * zoom);
                        g.DrawString(dr.Name, new Font(FontFamily.GenericSansSerif, 8,FontStyle.Bold), new SolidBrush(_forecolor), new RectangleF(0, (float)dr.Top * zoom, _width, (float)(dr.Base - dr.Top) * zoom));
                    }
                }
            }
        }
    }
    [Serializable()]
    public class SymbolColumn : Column
    {
        public override void activate(DataGridView dgv)
        {
            base.activate(dgv);
            string s = "Symbol";
            if (dgv.Columns.Contains(s))
            {
                DataGridViewImageColumn sc = new DataGridViewImageColumn();
                sc.DataPropertyName = s;
                sc.HeaderText = s;
                sc.Name = s;
                sc.ImageLayout = DataGridViewImageCellLayout.Zoom;
                int i = dgv.Columns[s].Index;
                dgv.Columns.Remove(s);
                dgv.Columns.Insert(i, sc);
            }
        }
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            foreach (logplotDataSet.symbolRow dr in lds.symbol.Select(" entity = " + entity.ToString() + " and (depth-size)<=" + BottomLimit.ToString() + " and (depth+size)>=" + TopLimit.ToString()))
            {
                {
                    Bitmap b = MainForm.GetResourceBMP(dr.Symbol);
                    b = myImage.imageutilities.recolor(b, new Color[] { Color.Black, Color.Transparent }, new Color[] { _forecolor, _backcolor });
                    int zoomit = ((int)dr.Size);
                    if (zoomit < 1)
                        zoomit = 1;
                    zoomit *= 3;
                    b = myImage.imageutilities.resize(b, zoomit);
                    g.DrawImage(b, new Point((int)(_width - b.Width) / 2, (int)((dr.Depth - b.Height / 2) * zoom)));
                }
            }
        }
    }
    public enum SIDE { Left=0, Both=1, Right=2 }
    public enum MEASURE { Meter=0, Foot=1 }
    [Serializable()]
    public class ScaleBarColumn : Column
    {
        protected bool _majtick = true, _mintick = true;
        protected int _majinterval = 100, _mininterval = 20;
        protected SIDE _majside = SIDE.Right, _minside = SIDE.Left;
        protected MEASURE _measure = MEASURE.Meter;
        protected Font _majfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), _minfont = new Font(FontFamily.GenericSansSerif, 8);

        [Category("Tick Style"), DisplayName("Major Tick")]
        public bool MajorTick { get { return _majtick; } set { _majtick = value; } }
        [Category("Tick Style"), DisplayName("Major Tick Interval")]
        public int MajorInterval { get { return _majinterval; } set { _majinterval = value; } }
        [Category("Tick Style"), DisplayName("Major Tick Side")]
        public SIDE MajorSide { get { return _majside; } set { _majside = value; } }
        //      [Category("Tick Style"), DisplayName("Major Tick Font")]
        //      public Font MajorFont { get { return _majfont; } set { _majfont = value; } }

        [Category("Tick Style"), DisplayName("Minor Tick")]
        public bool MinorTick { get { return _mintick; } set { _mintick = value; } }
        [Category("Tick Style"), DisplayName("Minor Tick Interval")]
        public int MinorInterval { get { return _mininterval; } set { _mininterval = value; } }
        [Category("Tick Style"), DisplayName("Minor Tick Side")]
        public SIDE MinorSide { get { return _minside; } set { _minside = value; } }
        //       [Category("Tick Style"), DisplayName("Minor Tick Font")]
        //      public Font MinorFont { get { return _minfont; } set { _minfont = value; } }

        [Category("Tick Style"), DisplayName("Measurement")]
        public MEASURE Measure { get { return _measure; } set { _measure = value; } }

        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(_forecolor);
            Brush brush = new SolidBrush(_forecolor);
            int step = 5;
            int x = (int)_width / 2;
            if ((_majside == SIDE.Right) & (_minside == SIDE.Right))
                x = 0;
            g.DrawLine(pen, x, 0, x, (int)BottomLimit * zoom - (int)TopLimit * zoom);
            for (int i = (int)TopLimit; i <= BottomLimit; i++)
            {
                Size s = TextRenderer.MeasureText(i.ToString(), _majfont);
                int y = i - (int)TopLimit - s.Height / 2;
                if (((i % _majinterval) == 0) & _majtick)
                {
                    switch (_majside)
                    {
                        case SIDE.Both:
                            g.DrawLine(pen, x - 20, y, x + 20, y);
                            g.DrawString(i.ToString(), _majfont, brush, 20 + x, y);
                            break;
                        case SIDE.Left:
                            g.DrawLine(pen, x - 20, y, x, y);
                            g.DrawString(i.ToString(), _majfont, brush, x - s.Width, y);
                            break;
                        default:
                            g.DrawLine(pen, x, y, x + 20, y);
                            g.DrawString(i.ToString(), _majfont, brush, 20 + x, y);
                            break;
                    }
                }
                else
                    if (((i % _mininterval) == 0) & _mintick)
                    {
                        switch (_minside)
                        {
                            case SIDE.Both:
                                g.DrawLine(pen, x - 10, y, x + 10, y);
                                g.DrawString(i.ToString(), _minfont, brush, 10 + x, y);
                                break;
                            case SIDE.Left:
                                g.DrawLine(pen, x - 10, y, x, y);
                                g.DrawString(i.ToString(), _minfont, brush, x - s.Width, y);
                                break;
                            default:
                                g.DrawLine(pen, x, y, x + 10, y);
                                g.DrawString(i.ToString(), _minfont, brush, 10 + x, y);
                                break;
                        }
                    }
                    else
                        if ((i % step) == 0)
                        {
                            g.DrawLine(pen, x, y, x + 5, y);
                        }
            }

        }
    }
    [Serializable()]
    public class BitmapColumn : Column
    {
        public override void activate(DataGridView dgv)
        {
            base.activate(dgv);
            string s = "Bitmap";
            if (dgv.Columns.Contains(s))
            {
                DataGridViewImageColumn sc =(DataGridViewImageColumn ) dgv.Columns[s];
                sc.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            foreach (logplotDataSet.bitmapRow dr in lds.bitmap.Select(" entity = " + entity.ToString() + " and top<=" + BottomLimit.ToString() + " and base>=" + TopLimit.ToString()))
            {
                Stream st = new MemoryStream(dr.Bitmap);
                //B = new TextureBrush(Image.FromStream(st));
                g.DrawImage(Image.FromStream(st), new Rectangle(0, (int)(dr.Top * zoom), _width, (int)((dr.Base - dr.Top) * zoom)));
            }
        }
    }
    [Serializable()]
    public class PercentColumn : Column
    {
        public override void activate(DataGridView dgv)
        {
            base.activate(dgv);
            int max = this.LithologySetting.Count + 2;
            if (max > dgv.ColumnCount - 1)
                max = dgv.ColumnCount - 1;
            for (int i = 2; i < max; i++)
                dgv.Columns[i].HeaderText = this.LithologySetting[i - 2].Name;
            for (int i = dgv.ColumnCount - 3; i >= max; i--)
                dgv.Columns.RemoveAt(i);
        }
        protected override void generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            Graphics g = Graphics.FromImage(bmp);
            int i;
            Bitmap[] bs = new Bitmap[1];
            TextureBrush[] tbs = new TextureBrush[1];
            Array.Resize(ref bs, LithologySetting.Count);
            Array.Resize(ref tbs, LithologySetting.Count);
            for (i = 0; i < LithologySetting.Count; i++)
            {
                LithologyTemplate lt = LithologySetting[i];
                tbs[i] = lt.Texture;
            }
            foreach (logplotDataSet.percentRow dr in lds.percent.Select(" entity = " + entity.ToString() + " and top<=" + BottomLimit.ToString() + " and base>=" + TopLimit.ToString()))
            {
                if (dr.entity == entity)
                    if (dr.Top < BottomLimit & dr.Base > TopLimit)
                    {
                        float left = (float)dr.a1 * _width / 100;
                        if (LithologySetting.Count > 0)
                            g.FillRectangle(tbs[0], 0, (float)dr.Top * zoom, left, (float)(dr.Base - dr.Top) * zoom);
                        for (i = 1; i < LithologySetting.Count; i++)
                        {
                            g.FillRectangle(tbs[i], left, (float)dr.Top * zoom, (float)((double)dr.ItemArray.GetValue(i + 4)) * _width / 100, (float)(dr.Base - dr.Top) * zoom);
                            left += (float)((double)dr.ItemArray.GetValue(i + 4)) * _width / 100;
                        }
                        g.FillRectangle(B, left, (float)dr.Top * zoom, _width, (float)(dr.Base - dr.Top) * zoom);
                    }
            }
        }
    }
    [Serializable()]
    public class Column
    {
        [XmlIgnore]
        public logplotDataSet lds = null;
        internal LithologyTemplate getLithologyTemplate(LithologyCollection ls, int pattern)
        {
            foreach (LithologyTemplate lt in ls)
                if (lt.pattern == pattern)
                    return lt;
            return null;
        }
        internal void getBrush(TotalFillStyle _fillstyle,int HEIGHT, out Brush B)
        {
            int i = 0;
            switch (_fillstyle.FillMethod)
            {
                case FillStyle.Rainbow:
                    B = new LinearGradientBrush(new Rectangle(0, 0, _width, HEIGHT), Color.Black, Color.Black, 0, false);
                    ColorBlend cb = new ColorBlend();
                    cb.Positions = new float[7];
                    i = 0;
                    for (float f = 0; f <= 1; f += 1.0f / 6)
                        cb.Positions[i++] = f;
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
                    B = new LinearGradientBrush(new Rectangle(0, 0, _width, HEIGHT), _fillstyle.PrimaryColor, _fillstyle.SecondaryColor, LinearGradientMode.Horizontal);
                    break;
                case FillStyle.Hatch:
                    B = new HatchBrush(_fillstyle.Hatch, _fillstyle.PrimaryColor, _fillstyle.SecondaryColor);
                    break;
                case FillStyle.Extra:
                    B = new PathGradientBrush(new Point[] { new Point(0, 0), new Point(_width, 0), new Point(_width, HEIGHT), new Point(0, HEIGHT) });
                    ((PathGradientBrush)B).SurroundColors = new Color[] { Color.Red, Color.Green, Color.Blue };
                    ((PathGradientBrush)B).CenterColor = Color.Gray;
                    break;
                case FillStyle.SingleColor:
                    B = new SolidBrush(_fillstyle.PrimaryColor);
                    break;
                default:
                    B = new SolidBrush(BackColor);
                    break;
            }
        }
        protected virtual void generate(double TopLimit, double BottomLimit, float zoom, Brush B){}
        public void regenerate(int top, int height, float zoom)
        {
            if ((zoom <= 0)|(_width<=0))
                return;
            int HEIGHT = (int)(height / zoom);
            bmp = new Bitmap(Width, HEIGHT);
            try
            {
                Random r = new Random();
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Brush B = new SolidBrush(_backcolor);
                Rectangle columnrect = new Rectangle(0, 0, _width, HEIGHT);
                g.FillRectangle(B, columnrect);
                if (_Border != BorderStyle.None)
                {
                    columnrect.Inflate(-1, -1);
                    g.DrawRectangle(Pens.Black, columnrect);
                }
                if (_Border == BorderStyle.Fixed3D)
                {
                    columnrect.Offset(1,1);
                    g.DrawRectangle(Pens.Black, columnrect);
                }
                B = new SolidBrush(_forecolor);
                if (!TotalGridStyle.GridOnTop)
                    DrawGrid(columnrect, g);
                double TopLimit = top/zoom; // e.ClipRectangle.Top / zoom;
                double BottomLimit = TopLimit + height / zoom;// e.ClipRectangle.Bottom / zoom;
                generate(TopLimit, BottomLimit, zoom, B);
                if (TotalGridStyle.GridOnTop)
                    DrawGrid(columnrect, g);
            }
            catch { }
        }

        [XmlIgnore]
        public Bitmap bmp = null;
        protected Color _forecolor = Color.Black;
        [XmlIgnore]
        [CategoryAttribute("Appearance"), DescriptionAttribute("ForeColor")]
        public Color ForeColor { get { return _forecolor; } set { _forecolor = value; } }
        [Browsable(false)]
        public Int32 _ForeColor { get { return _forecolor.ToArgb(); } set { _forecolor = Color.FromArgb(value); } }

        protected Color _backcolor = Color.White;
        [XmlIgnore]
        [CategoryAttribute("Appearance"), DescriptionAttribute("BackColor")]
        public Color BackColor { get { return _backcolor; } set { _backcolor = value; } }
        [Browsable(false)]
        public Int32 _BackColor { get { return _backcolor.ToArgb(); } set { _backcolor = Color.FromArgb(value); } }

        protected LithologyCollection _ls = new LithologyCollection();

        [DisplayName("Lithology Style")]
        public LithologyCollection LithologySetting { get { return _ls; } set { _ls = value; } }

        [XmlIgnore]
        public int entity = -1;
        [XmlIgnore]
        public int position = -1;
        protected int _id;
        protected string _name = "";
        protected int _left;
        protected int _width;
        protected ColumnType _type = ColumnType.Histogram;
        protected BorderStyle _Border = BorderStyle.None;

        public virtual void activate(DataGridView dgv)
        {
            while (dgv.Columns.Contains("id"))
                dgv.Columns.Remove("id");
            while (dgv.Columns.Contains("entity"))
                dgv.Columns.Remove("entity");
        }

        protected TwoGridStyle _tgs = new TwoGridStyle();
        [DisplayName("Grid Style"), CategoryAttribute("Appearance"), DescriptionAttribute("GridStyle")]
        public TwoGridStyle TotalGridStyle { get { return _tgs; } set { _tgs = value; } }

        [CategoryAttribute("General"), DescriptionAttribute("Column ID")]
        [ReadOnly(true),Browsable(false)]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [CategoryAttribute("General"), DescriptionAttribute("Column Name")]
        [ReadOnly(false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [CategoryAttribute("Location"), DescriptionAttribute("Column Left Side")]
        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }
        [XmlIgnore, Browsable(false), CategoryAttribute("Location"), DescriptionAttribute("Column Right Side")]
        public int Right
        {
            get { return _left+_width; }
            set { _width = value-_left; }
        }

        [CategoryAttribute("Location"), DescriptionAttribute("Column Right Side")]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }


        [CategoryAttribute("Appearance"), DescriptionAttribute("Has border around it?")]
        public BorderStyle Frame
        {
            get { return _Border; }
            set { _Border = value; }
        }

        [CategoryAttribute("General"), DescriptionAttribute("Type of Column?")]
        [ReadOnly(true)]
        public ColumnType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        protected void DrawGrid(Rectangle r, Graphics g)
        {
            if (_tgs.PlotValueGrid)
            {
                GridStyle gs = _tgs.ValueGridStyle;
                if (gs.MinorInterval)
                {
                    LineStyle ls = gs.MinorLineStyle;
                    Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                    _p.DashStyle = ls.Style;
                    for (int i = gs.MinorDivision; i < r.Width; i += gs.MinorDivision)
                        g.DrawLine(_p, i, 0, i, r.Height);
                }
                if (gs.MajorInterval)
                {
                    LineStyle ls = gs.MajorLineStyle;
                    Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                    _p.DashStyle = ls.Style;
                    for (int i = gs.MajorDivision; i < r.Width; i += gs.MajorDivision)
                        g.DrawLine(_p, i, 0, i, r.Height);
                }
            }
            if (_tgs.PlotDepthGrid)
            {
                GridStyle gs = _tgs.DepthGridStyle;
                if (gs.MinorInterval)
                {
                    LineStyle ls = gs.MinorLineStyle;
                    Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                    _p.DashStyle = ls.Style;
                    for (int i = gs.MinorDivision; i < r.Height; i += gs.MinorDivision)
                        g.DrawLine(_p, 0, i, r.Width, i);
                }
                if (gs.MajorInterval)
                {
                    LineStyle ls = gs.MajorLineStyle;
                    Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                    _p.DashStyle = ls.Style;
                    for (int i = gs.MajorDivision; i < r.Height; i += gs.MajorDivision)
                        g.DrawLine(_p, 0, i, r.Width, i);
                }
            }

        }
    }

    public class PointFComparer : IComparer<PointF>
    {
        public int Compare(PointF x, PointF y)
        {
            if (x.Y == y.Y)
                return 0;
            if (x.Y < y.Y)
                return -1;
            if (x.Y > y.Y)
                return 1;
            return 0;
        }
    }
}
