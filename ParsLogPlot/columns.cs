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
    [Serializable(), TypeConverterAttribute(typeof(GridStyleConverter)), DescriptionAttribute("Expand to see the Grid Style options.")]
    public class GridStyle
    {
        public GridStyle()
        {
            _majorlinestyle.ForeColor = Color.Black;
            _minorlinestyle.ForeColor = Color.Gray;
        }
        float _min = 0;
        float _max = 100;
        ScaleType _linear = ScaleType.Linear;
        bool _majorinterval = true;
        int _majordivision = 50;
        LineStyle _majorlinestyle = new LineStyle();
        bool _minorinterval = true;
        int _minordivision = 10;
        LineStyle _minorlinestyle = new LineStyle();

        public ScaleType Linear { get { return _linear; } set { _linear = value; } }
        public float Minimum { get { return _min; } set { _min = value; } }
        public float Maximum { get { return _max; } set { _max = value; } }
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
        GridStyle _valuegridstyle = null;
        GridStyle _depthgridstyle = null;
        public bool PlotValueGrid { get { return _plotvaluegrid; } set { _plotvaluegrid = value; } }
        public bool PlotDepthGrid { get { return _plotdepthgrid; } set { _plotdepthgrid = value; } }
        public bool GridOnTop { get { return _gridontop; } set { _gridontop = value; } }
        [XmlIgnore(), Browsable(false)]
        public GridStyle ValueGridStyle { get { return _valuegridstyle; } set { _valuegridstyle = value; } }
        [XmlIgnore(), Browsable(false)]
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
        IntervalText = 17,
        Fossil = 18,
        SeeLevelChange = 19
    }

    [Serializable()]
    public class LithologyTemplate
    {

        protected string _name;
        protected int _pattern;
        protected int _zoom = 1;
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

        [DisplayName("Size")]
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                maketexture();
            }
        }

        protected TextureBrush tb;

        [XmlIgnore, Browsable(false)]
        public TextureBrush Texture { get { return tb; } set { tb = value; } }

        protected void maketexture()
        {
            Bitmap bs = MainForm.GetResourceBMP(_pattern);
            bs = myImage.imageutilities.recolor(bs, new Color[] { Color.Black, Color.Transparent }, new Color[] { _forecolor, _backcolor });
            bs = myImage.imageutilities.resize(bs, _zoom);
            tb = new TextureBrush(bs);
        }

        [Editor(typeof(PatternEditor), typeof(System.Drawing.Design.UITypeEditor))]
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
            if (destinationType == typeof(KeyValueFilledColumn)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is KeyValueFilledColumn)
            {
                KeyValueFilledColumn so = (KeyValueFilledColumn)value;
                return "Key-Value column settings";
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
                        KeyValueFilledColumn so = new KeyValueFilledColumn();
                        return so;
                    }
                }
                catch { throw new ArgumentException("Can not convert '" + (string)value + "' to type KeyValueFilledColumn"); }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public enum ScaleType { Linear, Logarithmic }
    public enum FitStyleType { Scale, Trim, Wrap, Wrap10 }
    public enum LowHighStyleType { LowToHigh, HighToLow }
    public enum LeftRightStyleType { LeftToRight, RightToLeft }
    [Serializable()]
    public class KeyValueFilledColumn : ValueFilledColumn
    {
        protected FitStyleType _fitstyle = FitStyleType.Trim;
        protected LowHighStyleType _lowhighstyle = LowHighStyleType.LowToHigh;
        protected LeftRightStyleType _leftrightstyle = LeftRightStyleType.LeftToRight;
        //        [Category("Settings")]
        //        public FitStyleType FitStyle { get { return _fitstyle; } set { _fitstyle = value; } }
        [Category("Settings")]
        public LowHighStyleType LowHighStyle { get { return _lowhighstyle; } set { _lowhighstyle = value; } }
        [Category("Settings")]
        public LeftRightStyleType LeftRightStyle { get { return _leftrightstyle; } set { _leftrightstyle = value; } }
        //        public int 1 { get { return _1; } set { _1 = value; } }
    }
    [Serializable()]
    public class CurveColumn : KeyValueFilledColumn
    {
        protected bool _filled = false;
        [Category("Settings"), DisplayName("Filled")]
        public bool Filled { get { return _filled; } set { _filled = value; } }
        protected bool _points = false;
        [Category("Settings"), DisplayName("Show points")]
        public bool Points { get { return _points; } set { _points = value; } }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.curve.Select(" entity = " + entity.ToString() + " and depth<=" + BottomLimit.ToString() + " and depth>=" + (TopLimit).ToString(), "depth");
            if (drs.Length < 2)
                return -1;
            int i = 0;
            int length = drs.Length;
            PointF[] ps = new PointF[length];
            KeyValueFilledColumn kv = (KeyValueFilledColumn)this;
            //int repeat = 1;
            foreach (logplotDataSet1.curveRow dr in drs)
            {
                double val = dr.Value;
                
                if (_gs.Linear == ScaleType.Logarithmic)
                    val = _width * (Math.Log10(val) - Math.Log10(_gs.Minimum)) / (Math.Log10(_gs.Maximum) - Math.Log10(_gs.Minimum));                    
                else
                    val = (val - _gs.Minimum) * _width / (_gs.Maximum - _gs.Minimum);
                if (this.LowHighStyle == LowHighStyleType.HighToLow)
                    val = _width - val;
                ps[i] = new PointF((float)Math.Max(_gs.Minimum, val), (float)(dr.Depth - TopLimit) * zoom);
                retval += (int)(val + dr.Depth);
                //if (ps[i].X != 0)                    repeat = Math.Max(repeat, (int)(_gs.Maximum / ps[i].X));
                #region tick
                RectangleF rec = new RectangleF(ps[i], new Size(4, 4));
                rec.Offset(-2, -2);
                /*
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X, ps[i].Y + 2));
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X, ps[i].Y - 2));
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X - 2, ps[i].Y));
                g.DrawLine(new Pen(B), ps[i], new PointF(ps[i].X + 2, ps[i].Y));
                g.DrawEllipse(new Pen(B), rec);
                 */
                #endregion
                i++;
            }
            //for (; repeat > 0; repeat--)            {
            //Array.Copy(ps, sp, length);
            if (_filled)
            {
                float _x = 0;
                if (this.LeftRightStyle == LeftRightStyleType.RightToLeft)
                    _x = _width;
                int size = ps.Length;
                Array.Resize<PointF>(ref ps, size * 2);
                for (i = size; i < size * 2; i++)
                {
                    ps[i] = new PointF(_x, ps[2 * size - i - 1].Y);
                }
                g.FillPolygon(B, ps);
            }
            else
                g.DrawLines(new Pen(ForeColor), ps);
            if (_points)
                foreach (PointF pf in ps)
                    g.DrawRectangle(Pens.Red, pf.X - 1, pf.Y - 1, 2, 2);


            //}
            return retval;
        }
    }
    public class simplecurveConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(simplecurve)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is simplecurve)
            {
                simplecurve so = (simplecurve)value;
                return "curve options";
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
            return base.ConvertFrom(context, culture, value);
        }
    }
    [Serializable(), TypeConverterAttribute(typeof(simplecurveConverter)), DescriptionAttribute("Expand to see the curve options.")]
    public class simplecurve
    {
        protected LeftRightStyleType _l2r = LeftRightStyleType.LeftToRight;
        //[DisplayName("Left To Right")]        public LeftRightStyleType L2R { get { return _l2r; } set { _l2r = value; } }
        protected LowHighStyleType _lh = LowHighStyleType.LowToHigh;
        [DisplayName("Low To High")]
        public LowHighStyleType LH { get { return _lh; } set { _lh = value; } }
    }
    [Serializable()]
    public class CrossPlotCurveColumn : ValueFilledColumn
    {
        protected simplecurve c1=new simplecurve();
        protected simplecurve c2=new simplecurve();
        [DisplayName("Curve 1")]
        public simplecurve C1 { get { return c1; } set { c1 = C1; } }
        [DisplayName("Curve 2")]
        public simplecurve C2 { get { return c2; } set { c2 = C2; } }

        protected GridStyle _gs2 = new GridStyle();
        [DisplayName("Secondary ScaleBar")]
        public GridStyle HorizontalScaleBar2 { get { return _gs2; } set { _gs2 = value; } }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            if (g == null)
                g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            PointF[] ps = new PointF[lds.crossplot.Count];
            PointF[] pt = new PointF[lds.crossplot.Count];
            PointF[] tpt = new PointF[lds.crossplot.Count];

            int i = 0;
            int retval = -1;
            DataRow[] drs = lds.crossplot.Select(" entity = " + entity.ToString() + " and depth<=" + BottomLimit.ToString() + " and depth>=" + TopLimit.ToString() , "depth");
            if (drs.Length < 2)
                return -1;
            foreach (logplotDataSet1.crossplotRow dr in drs)
            {
                double val = dr.CrossPlot1;
                if (_gs.Linear == ScaleType.Logarithmic)
                    val = _width * (Math.Log10(val) - Math.Log10(_gs.Minimum)) / (Math.Log10(_gs.Maximum) - Math.Log10(_gs.Minimum));
                else
                    val = (val - _gs.Minimum) * _width / (_gs.Maximum - _gs.Minimum);

                if (c1.LH == LowHighStyleType.HighToLow)
                    val = _width - val;
                ps[i] = new PointF((float)val, (float)(dr.Depth - TopLimit) * zoom);
                val = dr.CrossPlot2;
                if (_gs2.Linear == ScaleType.Logarithmic)
                    val = _width * (Math.Log10(val) - Math.Log10(_gs2.Minimum)) / (Math.Log10(_gs2.Maximum) - Math.Log10(_gs2.Minimum));
                else
                    val = (val - _gs2.Minimum) * _width / (_gs2.Maximum - _gs2.Minimum);
                val = _width - val;
                if (c2.LH == LowHighStyleType.HighToLow)
                    val = _width - val;
                pt[i++] = new PointF((float)val, (float)(dr.Depth - TopLimit) * zoom);
            }
            if (i > 2)
            {
                Array.Resize<PointF>(ref ps, i);
                Array.Resize<PointF>(ref pt, i);
                Array.Resize<PointF>(ref tpt, i * 2);
                Array.Sort<PointF>(ps, new PointFComparer());
                Array.Sort<PointF>(pt, new PointFComparer());
                //g.DrawCurve(Pens.Black, ps);
                //g.DrawCurve(Pens.Black, pt);
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
                g.FillPolygon(B, tpt);
                /*
                g.FillPolygon(br, ps, FillMode.Alternate);
                g.FillPolygon(br, pt, FillMode.Alternate);
                 */
                //                          g.DrawLines(Pens.DarkKhaki, ps);
            }
            return retval;
        }
    }

    public enum FillStyle { SingleColor, Pattern, Gradient, Rainbow, Hatch, Extra }
    public class PatternEditor : UITypeEditor
    {
        //public PatternEditor()
        //    : base()
        //{
        //}
        /*
        protected string Connection = "";
        public PatternEditor(string connection)
            : base()
        {
            Connection = connection;
        }
        */
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            Bitmap newImage = MainForm.GetResourceBMP((int)e.Value);
            TextureBrush tb = new TextureBrush(newImage);
            e.Graphics.FillRectangle(tb, e.Bounds);
        }
        public override UITypeEditorEditStyle GetEditStyle(
        ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
        IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;
            if (wfes != null)
            {
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
    public class FontEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(
        ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context,
        IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;
            if (wfes != null)
            {
                FontDialog fd = new FontDialog();
                fd.Font = (Font)value;
                fd.ShowEffects = true;
                //    fd.ShowColor = true;
                fd.FontMustExist = true;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    value = fd.Font;
                }
                fd.Dispose();
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
        protected Color _StartColor = Color.LightBlue, _EndColor = Color.LightGreen;
        protected HatchStyle _Hatch;
        public HatchStyle Hatch { get { return _Hatch; } set { _Hatch = value; } }
        [Editor(typeof(PatternEditor), typeof(System.Drawing.Design.UITypeEditor))]
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
        [DisplayName("Fill Style"), CategoryAttribute("Appearance"), DescriptionAttribute("How to fill the column?")]
        public TotalFillStyle FillStyle { get { return tfs; } set { tfs = value; } }
    }
    [Serializable()]
    public class ValueFilledColumn : ValueColumn
    {
        protected TotalFillStyle tfs = new TotalFillStyle();
        [CategoryAttribute("Appearance"), DescriptionAttribute("How to fill the column?")]
        public TotalFillStyle FillStyle { get { return tfs; } set { tfs = value; } }
    }
    [Serializable()]
    public class HistogramColumn : KeyValueFilledColumn
    {
        protected bool _showbox = false;
        [DisplayName("Bars in Box")]
        public bool showbox { get { return _showbox; } set { value = _showbox; } }

        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.histogram.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            double val = -1;
            double depth = -1;
            foreach (logplotDataSet1.histogramRow dr in drs)
            {
                g.TranslateTransform(0, (float)math.pos(dr.Top - TopLimit) * zoom);

                if (this.LeftRightStyle == LeftRightStyleType.LeftToRight)
                {
                    double newval = dr.Value;
                    newval = _width * (newval - _gs.Minimum) / (_gs.Maximum - _gs.Minimum);
                    if (_gs.Linear == ScaleType.Logarithmic)
                        newval = Math.Log10(newval);
                    if (LowHighStyle == LowHighStyleType.HighToLow)
                        newval = _width - newval;

                    //  if(this.FitStyle==FitStyleType.

                    g.FillRectangle(B, 0, 0, (float)newval, (float)(dr.Base - dr.Top) * zoom);

                    if (tfs.FillMethod == ParsLogPlot.FillStyle.Pattern)
                    {
                        if (val >= 0)
                        {
                            if (depth == dr.Top)
                            {
                                g.DrawLine(Pens.Black, (float)val, 0, (float)newval, 0);
                            }
                            else
                            {
                                g.DrawLine(Pens.Black, 0, (float)(depth - dr.Top) * zoom, (float)val, (float)(depth - dr.Top) * zoom);
                                g.DrawLine(Pens.Black, 0, 0, 0, (float)(depth - dr.Top) * zoom);
                                g.DrawLine(Pens.Black, 0, 0, (float)newval, 0);
                            }
                        }
                        g.DrawLine(Pens.Black, (float)newval, 0, (float)newval, (float)(dr.Base - dr.Top) * zoom);
                    }
                    if (_showbox)
                        g.DrawRectangle(Pens.Black, 0, 0, (float)newval, (float)(dr.Base - dr.Top) * zoom);

                    val = newval;
                }
                else
                {
                    double newval = dr.Value;
                    newval = _width - _width * (newval - _gs.Minimum) / (_gs.Maximum - _gs.Minimum);
                    if (LowHighStyle == LowHighStyleType.HighToLow)
                        newval = _width - newval;

                    g.FillRectangle(B, (float)newval, 0, _width, (float)(dr.Base - dr.Top) * zoom);


                    if (tfs.FillMethod == ParsLogPlot.FillStyle.Pattern)
                    {
                        if (val >= 0)
                        {
                            if (depth == dr.Top)
                            {
                                g.DrawLine(Pens.Black, (float)val, 0, (float)newval, 0);
                            }
                            else
                            {
                                g.DrawLine(Pens.Black, _width, (float)(depth - dr.Top) * zoom, (float)val, (float)(depth - dr.Top) * zoom);
                                g.DrawLine(Pens.Black, _width, 0, _width, (float)(depth - dr.Top) * zoom);
                                g.DrawLine(Pens.Black, _width, 0, (float)newval, 0);
                            }
                        }
                        g.DrawLine(Pens.Black, (float)newval, 0, (float)newval, (float)(dr.Base - dr.Top) * zoom);
                    }
                    val = newval;
                }
                g.TranslateTransform(0, -(float)math.pos(dr.Top - TopLimit) * zoom);
                retval += (int)(dr.Top + dr.Base + 1);
                depth = dr.Base;
            }
            return retval;
        }
    }
    [Serializable()]
    public class HistogramValueColumn : FilledColumn
    {
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            DataRow[] drs = lds.histogram.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.histogramRow dr in drs)
            {
                g.DrawRectangle(new Pen(_forecolor), 1, (float)((dr.Top - TopLimit) * zoom) + 1, _width - 2, (float)((dr.Base - dr.Top) * zoom) - 2);
                g.DrawString(dr.Value.ToString(), new Font(FontFamily.GenericSansSerif, 13), B, new PointF(2, (float)(dr.Top - TopLimit + (dr.Base - dr.Top) / 2) * zoom));
                retval += (int)(dr.Top + dr.Base + dr.Value);
            }
            return retval;
        }
    }
    [Serializable()]
    public class LithologyColumn : FilledColumn
    {
        protected string _curve = "";
        [DisplayName("Profile Curve")]
        public string ProfileCurve { get { return _curve; } set { _curve = value; } }

        protected CurveColumn _cc = null;
        [Browsable(false), XmlIgnore()]
        public CurveColumn cc { get { return _cc; } set { _cc = value; } }

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
                //                dgv.Columns.Insert(i, sc);
            }
            s = "Contact";
            if (dgv.Columns.Contains(s))
            {
                DataGridViewImageColumn sc = new DataGridViewImageColumn();
                sc.DataPropertyName = s;
                sc.HeaderText = s;
                sc.Name = s;
                sc.ImageLayout = DataGridViewImageCellLayout.Normal;
                int i = dgv.Columns[s].Index;
                dgv.Columns.Remove(s);
                dgv.Columns.Insert(i, sc);
            }
        }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);

            int i = 0;
            Bitmap b;
            DataRow[] drs = lds.lithology.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.lithologyRow dr in drs)
            {
                LithologyTemplate lt = getLithologyTemplate(LithologySetting, dr.Name);
                TextureBrush tb;
                if (lt != null)
                {
                    tb = lt.Texture;
                }
                else
                {
                    try
                    {
                        b = MainForm.GetResourceBMP(0);
                        b = myImage.imageutilities.recolor(b, new Color[] { Color.Black, Color.Transparent }, new Color[] { lt.ForeColor, lt.BackColor });
                        tb = new TextureBrush(b);
                    }
                    catch
                    {
                        tb = new TextureBrush(new Bitmap(1, 1));
                    }
                }
                g.TranslateTransform(0, (float)math.pos(dr.Top - TopLimit) * zoom);
                g.FillRectangle(tb, 0, 0, _width - 0, (float)(dr.Base - dr.Top) * zoom);
                try
                {
                    b = MainForm.GetResourceBMP(dr.Contact);
                    b = myImage.imageutilities.tile(b, _width, b.Height);
                    g.DrawImage(b, 0, (float)(dr.Base - dr.Top) * zoom - b.Height/2);
                }
                catch { }
                g.TranslateTransform(-0, -(float)math.pos(dr.Top - TopLimit) * zoom);
            }
            if (cc != null)
            {
                float end = _width;
                DataRow[] curve_drs = lds.curve.Select(" entity = '" + cc.entity.ToString() + "' and depth>=" + TopLimit.ToString() + " and depth<=" + BottomLimit.ToString(), "depth");
                if (curve_drs.Length > 1)
                {
                    if (cc.LeftRightStyle == LeftRightStyleType.RightToLeft)
                        end = 0;
                    i = 0;
                    int size = curve_drs.Length;
                    PointF[] sp = new PointF[size * 2];
                    PointF[] ps = new PointF[size];
                    GridStyle _gs = _cc.HorizontalScaleBar;
                    foreach (logplotDataSet1.curveRow cdr in curve_drs)
                    {
                        float val = (float)cdr.Value;
                        if (_gs != null)
                            if (_gs.Maximum > _gs.Minimum)
                                val = (val - _gs.Minimum) * _width / (_gs.Maximum - _gs.Minimum);
                        if (cc.LowHighStyle == LowHighStyleType.HighToLow)
                            val = _width - val;
                        sp[i] = new PointF(val, (float)(cdr.Depth - TopLimit) * zoom);
                        ps[i] = new PointF(val, (float)(cdr.Depth - TopLimit) * zoom);
                        sp[2 * size - i - 1] = new PointF(end, (float)(cdr.Depth - TopLimit) * zoom);
                        i++;
                    }
                    g.FillPolygon(new SolidBrush(Color.White), sp);
                    g.FillPolygon(new SolidBrush(BackColor), sp);
                    g.DrawLines(new Pen(ForeColor), ps);
                }
            }
            return retval;
        }
    }
    public static class math
    {
        public static int pos(int i)
        {
            if (i < 0)
            {
                return 0;
            }
            else
            {
                return i;
            }
        }
        public static double pos(double i)
        {
            if (i < 0)
            {
                return 0;
            }
            else
            {
                return i;
            }
        }
        public static int pos(int i, out int j)
        {
            if (i < 0)
            {
                j = -i;
                return 0;
            }
            else
            {
                j = 0;
                return i;
            }
        }
        public static double pos(double i, out double j)
        {
            if (i < 0)
            {
                j = -i;
                return 0;
            }
            else
            {
                j = 0;
                return i;
            }
        }
    }
    [Serializable()]
    public class FillBarColumn : FilledColumn
    {
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.fill.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.fillRow dr in drs)
            {
                g.TranslateTransform(0, (float)math.pos(dr.Top - TopLimit) * zoom);
                g.FillRectangle(B, 0, 0, _width - 0, (float)(dr.Base - dr.Top) * zoom);
                g.TranslateTransform(-0, -(float)math.pos(dr.Top - TopLimit) * zoom);
            }
            return retval;
        }
    }
    [Serializable()]
    public class SymbolColumn : Column
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }
        //ignore list

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
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            DataRow[] drs = lds.symbol.Select(" entity = " + entity.ToString() + " and (depth-size*10)<=" + BottomLimit.ToString() + " and (depth+size*10)>=" + TopLimit.ToString(), "depth");
            foreach (logplotDataSet1.symbolRow dr in drs)
            {
                Bitmap b = MainForm.GetResourceBMP(dr.Symbol);
                b = myImage.imageutilities.recolor(b, new Color[] { Color.Black, Color.Transparent }, new Color[] { _forecolor, _backcolor });
                float zoomit = dr.Size;
                if (zoomit == 0)
                    zoomit = 1;
                zoomit *= zoom;
                b = myImage.imageutilities.resize(b, zoomit);
                int w = b.Width;
                int h = b.Height;
                w *= dr.Count;
                b = myImage.imageutilities.tile(b, w, h);
                g.DrawImageUnscaled(b, new Point((int)(_width - b.Width) / 2, (int)((dr.Depth - TopLimit) * zoom - b.Height / 2)));
               // retval += dr.Symbol + dr.Size;
            }
            return retval;
        }
    }
    public enum SIDE { Left = 0, Both = 1, Right = 2 }
    public enum MEASURE { Meter = 0, Foot = 1 }
    [Serializable()]
    public class ScaleBarColumn : Column
    {
        protected bool _majtick = true, _mintick = true;
        protected bool _majlabel = true, _minlabel = true;
        protected int _majinterval = 100, _mininterval = 20;
        protected SIDE _majside = SIDE.Right, _minside = SIDE.Left;
        protected MEASURE _measure = MEASURE.Meter;
        protected Font _majfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), _minfont = new Font(FontFamily.GenericSansSerif, 8);

        [Category("Tick Style"), DisplayName("Major Label")]
        public bool MajorLabel { get { return _majlabel; } set { _majlabel = value; if (_majlabel) _majtick = true; } }
        [Category("Tick Style"), DisplayName("Major Tick")]
        public bool MajorTick { get { return _majtick; } set { _majtick = value; if (!_majtick)_majlabel = false; } }
        [Category("Tick Style"), DisplayName("Major Tick Interval")]
        public int MajorInterval { get { return _majinterval; } set { _majinterval = value; } }
        [Category("Tick Style"), DisplayName("Major Tick Side")]
        public SIDE MajorSide { get { return _majside; } set { _majside = value; } }
        [XmlIgnore, Category("Tick Style"), DisplayName("Major Tick Font"), Editor(typeof(FontEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Font MajorFont { get { return _majfont; } set { _majfont = value; } }
        [Browsable(false)]
        public string majorfonts
        {
            get { FontConverter f = new FontConverter(); return f.ConvertToString(_majfont); }
            set { FontConverter f = new FontConverter(); _majfont = (Font)f.ConvertFromString(value); }
        }

        [Category("Tick Style"), DisplayName("Minor Label")]
        public bool MinorLabel { get { return _minlabel; } set { _minlabel = value; if (_minlabel) _mintick = true; } }
        [Category("Tick Style"), DisplayName("Minor Tick")]
        public bool MinorTick { get { return _mintick; } set { _mintick = value; if (!_mintick)_minlabel = false; } }
        [Category("Tick Style"), DisplayName("Minor Tick Interval")]
        public int MinorInterval { get { return _mininterval; } set { _mininterval = value; } }
        [Category("Tick Style"), DisplayName("Minor Tick Side")]
        public SIDE MinorSide { get { return _minside; } set { _minside = value; } }
        [XmlIgnore, Category("Tick Style"), DisplayName("Minor Tick Font"), Editor(typeof(FontEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Font MinorFont { get { return _minfont; } set { _minfont = value; } }
        [Browsable(false)]
        public string minorfonts
        {
            get { FontConverter f = new FontConverter(); return f.ConvertToString(_minfont); }
            set { FontConverter f = new FontConverter(); _minfont = (Font)f.ConvertFromString(value); }
        }

        [Category("Tick Style"), DisplayName("Unit")]
        public MEASURE Measure { get { return _measure; } set { _measure = value; } }

        [Browsable(false)]
        public new LithologyCollection LithologySetting { get { return _ls; } set { _ls = value; } }
        [Browsable(false)]
        public new TwoGridStyle TotalGridStyle { get { return _tgs; } set { _tgs = value; } }

        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            float usezoom = zoom;
            if (Measure == MEASURE.Foot)
                usezoom *= 0.3048F;
            Pen pen = new Pen(_forecolor);
            Brush brush = new SolidBrush(_forecolor);
            g.FillRectangle(new SolidBrush(_backcolor), new Rectangle(0, 0, _width, (int)(BottomLimit - TopLimit)));
            int x = (int)_width / 2;
            if ((_majside == SIDE.Right) & (_minside == SIDE.Right))
                x = 5;
            if ((_majside == SIDE.Left) & (_minside == SIDE.Left))
                x = _width-5;
            g.DrawLine(pen, x, 0, x, (int)((BottomLimit-TopLimit)*zoom));
            int y = 0;
            string text = "";
            int ty = 0;
            Size s = new Size();
            int h = -1;           
            for (float i = (float)TopLimit; i <= BottomLimit+(BottomLimit-TopLimit+2)*zoom; i+=usezoom)
            {
                y = (int)(i - TopLimit);
                h = (int)Math.Round((i - TopLimit) / usezoom + TopLimit*zoom/usezoom);
                text = h.ToString();
                s = TextRenderer.MeasureText(text, _majfont);
                ty = (int)(y - s.Height / 2);
                if (_majtick && ((h % _majinterval) == 0))
                {
                    switch (_majside)
                    {
                        case SIDE.Both:
                            g.DrawLine(pen, x - 10, y, x + 10, y);
                            if (_majlabel)
                                g.DrawString(text, _majfont, brush, 10 + x, ty);
                            break;
                        case SIDE.Left:
                            g.DrawLine(pen, x - 10, y, x, y);
                            if (_majlabel)
                                g.DrawString(text, _majfont, brush, x - s.Width, ty);
                            break;
                        default:
                            g.DrawLine(pen, x, y, x + 10, y);
                            if (_majlabel)
                                g.DrawString(text, _majfont, brush, 10 + x, ty);
                            break;
                    }
                }
                else
                    if (((h % _mininterval) == 0) & _mintick)
                    {
                        switch (_minside)
                        {
                            case SIDE.Both:
                                g.DrawLine(pen, x - 5, y, x + 5, y);
                                if (_minlabel)
                                    g.DrawString(text, _minfont, brush, 5 + x, ty);
                                break;
                            case SIDE.Left:
                                g.DrawLine(pen, x - 5, y, x, y);
                                if (_minlabel)
                                    g.DrawString(text, _minfont, brush, x - s.Width, ty);
                                break;
                            default:
                                g.DrawLine(pen, x, y, x + 5, y);
                                if (_minlabel)
                                    g.DrawString(text, _minfont, brush, 5 + x, ty);
                                break;
                        }
                    }
            }
            switch (_majside)
            {
                case SIDE.Both:
                    g.DrawLine(pen, x - 10, y, x + 10, y);
                    if (_majlabel)
                        g.DrawString(text, _majfont, brush, 10 + x, ty);
                    break;
                case SIDE.Left:
                    g.DrawLine(pen, x - 10, y, x, y);
                    if (_majlabel)
                        g.DrawString(text, _majfont, brush, x - s.Width, ty);
                    break;
                default:
                    g.DrawLine(pen, x, y, x + 10, y);
                    if (_majlabel)
                        g.DrawString(text, _majfont, brush, 10 + x, ty);
                    break;
            }
            return retval;
        }
    }
    [Serializable()]
    public class BitmapColumn : Column
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }

        public override void activate(DataGridView dgv)
        {
            base.activate(dgv);
            string s = "Bitmap";
            if (dgv.Columns.Contains(s))
            {
                DataGridViewImageColumn sc = (DataGridViewImageColumn)dgv.Columns[s];
                sc.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            DataRow[] drs = lds.bitmap.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.bitmapRow dr in drs)
                if (dr.Bitmap != null)
                {
                    Stream st = new MemoryStream(dr.Bitmap);
                    Image img = Image.FromStream(st);
                    if (dr.Stretch)
                        g.DrawImage(img, new Rectangle(0, (int)(math.pos(dr.Top - TopLimit) * zoom), _width, (int)((dr.Base - dr.Top) * zoom)));
                    else
                        g.DrawImageUnscaledAndClipped(img, new Rectangle(_width / 2 - img.Width / 2, (int)(math.pos(dr.Top - TopLimit) * zoom), _width / 2 + img.Width / 2, (int)((dr.Base - TopLimit) * zoom)));
                    // centered                    g.DrawImageUnscaled(img, new Point(_width / 2 - img.Width / 2, (int)((dr.Top - TopLimit + (dr.Base - dr.Top) / 2) * zoom - img.Height / 2)));//, _width, )
                    retval += (int)(dr.Top + dr.Base);
                }
            return retval;
        }
    }
    [Serializable()]
    public class PercentColumn : Column
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        //ignore list
        public override void activate(DataGridView dgv)
        {
            base.activate(dgv);
            int max = this.LithologySetting.Count + 2;
            if (max > dgv.ColumnCount - 1)
                max = dgv.ColumnCount - 1;
            for (int i = 2; i < max; i++)
                dgv.Columns[i].HeaderText = this.LithologySetting[i - 2].Name;
            for (int i = dgv.ColumnCount - 4; i >= max; i--)
                dgv.Columns.RemoveAt(i);
        }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            int i;
            TextureBrush[] tbs = new TextureBrush[LithologySetting.Count];
            for (i = 0; i < LithologySetting.Count; i++)
                tbs[i] = LithologySetting[i].Texture;
            DataRow[] drs = lds.percent.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.percentRow dr in drs)
            {
                g.TranslateTransform(0, (float)(math.pos(dr.Top - TopLimit) * zoom));
                float left = (float)Convert.ToDouble(string.Concat("0", dr["a1"].ToString())) * _width / 100;
                if (LithologySetting.Count > 0)
                    g.FillRectangle(tbs[0], 0, 0, left, (float)(dr.Base - dr.Top) * zoom);
                for (i = 2; i < LithologySetting.Count + 1; i++)
                {
                    double dval = 0;
                    dval = Convert.ToDouble(string.Concat("0", dr["a" + i.ToString()].ToString()));
                    g.FillRectangle(tbs[i - 1], left, 0, (float)(dval * _width / 100), (float)(dr.Base - dr.Top) * zoom);
                    left += (float)(dval * _width / 100);
                    retval += (int)dval;
                }
                g.FillRectangle(B, left, 0, _width, (float)(dr.Base - dr.Top) * zoom);
                g.TranslateTransform(0, -(float)(math.pos(dr.Top - TopLimit) * zoom));
                retval += 0;
            }
            return retval;
        }
    }

    [Serializable()]
    public class Column
    {
        public string serialize()
        {
            XmlSerializer mySerializer = new XmlSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, this);
            byte[] bs = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bs, 0, (int)ms.Length);
            ASCIIEncoding a = new ASCIIEncoding();
            String sb = new String(a.GetChars(bs));
            return Convert.ToString(sb);
        }

        [XmlIgnore]
        public logplotDataSet1 lds = null;
        internal LithologyTemplate getLithologyTemplate(LithologyCollection ls, int pattern)
        {
            foreach (LithologyTemplate lt in ls)
                if (lt.pattern == pattern)
                    return lt;
            return null;
        }
        internal LithologyTemplate getLithologyTemplate(LithologyCollection ls, string name)
        {
            foreach (LithologyTemplate lt in ls)
                if (lt.Name == name)
                    return lt;
            return null;
        }
        protected void getBrush(TotalFillStyle _fillstyle, int HEIGHT, out Brush B)
        {
            int i = 0;
            switch (_fillstyle.FillMethod)
            {
                case FillStyle.Rainbow:
                    B = new LinearGradientBrush(new Rectangle(0, 0, _width, HEIGHT), Color.Black, Color.Black, 0, false);
                    ColorBlend cb = new ColorBlend();
                    cb.Positions = new float[7];
                    i = 0;
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
        protected virtual int generate(double TopLimit, double BottomLimit, float zoom, Brush B) { return -1; }
        [XmlIgnore]
        public int hash = -1;
        protected bool _changed = true;
        public void Invalidate()
        {
            _changed = true;
        }
        protected Graphics g;
        public int regenerate(Int32 top, Int32 bottom, float zoom)
        {            
            int retval = (int)(top + zoom);
            if (!_changed)
                return retval;
            if ((zoom <= 0) | (_width <= 0))
                return retval;
            Rectangle columnrect = new Rectangle(0, -2, _width, (Int32)((bottom - top) * zoom + 4));
            bmp = new Bitmap(Width, (Int32)((bottom-top)*zoom)+50);
            Random r = new Random();
            g = Graphics.FromImage(bmp);
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (this is ValueColumn)
                #region above scalebar sample
                {
                    if (this is CrossPlotCurveColumn)
                    {

                        GridStyle gstemp = ((CrossPlotCurveColumn)this).HorizontalScaleBar2;
                        Font _font = new Font(FontFamily.GenericSansSerif, 8);
                        string text;
                        Size s;

                        if (gstemp.Linear == ScaleType.Linear)
                        {
                            text = gstemp.Maximum.ToString();
                            s = TextRenderer.MeasureText(text, _font);
                            g.DrawString(text, _font, Brushes.Black, 0, 0);
                            float lpos = _width / 2;
                            text = ((gstemp.Minimum + gstemp.Maximum) / 2).ToString();
                            s = TextRenderer.MeasureText(text, _font);
                            if (lpos > s.Width / 2)
                                lpos -= s.Width / 2;
                            g.DrawString(text, _font, Brushes.Black, lpos, 0);
                            if (_width >= 150)
                            {
                                #region quarters
                                lpos = _width / 4;
                                text = (gstemp.Minimum + (gstemp.Maximum - gstemp.Minimum) *0.75).ToString();
                                s = TextRenderer.MeasureText(text, _font);
                                if (lpos > s.Width / 2)
                                    lpos -= s.Width / 2;
                                g.DrawString(text, _font, Brushes.Black, lpos, 0);

                                lpos = _width * 3 / 4;
                                text = (gstemp.Minimum + (gstemp.Maximum - gstemp.Minimum) * 0.25).ToString();
                                s = TextRenderer.MeasureText(text, _font);
                                if (lpos > s.Width / 2)
                                    lpos -= s.Width / 2;
                                g.DrawString(text, _font, Brushes.Black, lpos, 0);
                                #endregion quarters
                            }
                            text = gstemp.Minimum.ToString();
                            s = TextRenderer.MeasureText(text, _font);
                            g.DrawString(text, _font, Brushes.Black, _width - s.Width, 0);
                        }
                        else
                        {
                            int i = (int)(Math.Ceiling(Math.Log10(gstemp.Minimum)));
                            int j = (int)(Math.Floor(Math.Log10(gstemp.Maximum)));
                            if (j > i)
                            {
                                gstemp.MajorDivision = (int)((gstemp.Maximum - gstemp.Minimum) / (j - i));
                                gstemp.MinorDivision = (int)((gstemp.Maximum - gstemp.Minimum) / (2 * (j - i)));
                                for (int k = i; k <= j; k++)
                                {
                                    text = Math.Pow(10, k).ToString();
                                    s = TextRenderer.MeasureText(text, _font);
                                    double lpos = _width - _width * (k - Math.Log10(gstemp.Minimum)) / (Math.Log10(gstemp.Maximum) - Math.Log10(gstemp.Minimum));
                                    lpos = Math.Max(lpos, 0);
                                    lpos = Math.Min(lpos, _width - s.Width);
                                    g.DrawString(text, _font, Brushes.Black, (float)lpos, 0);
                                }
                            }
                        }
                        g.DrawLine(Pens.Black, 0f, 12.5f, _width, 12.5f);
                        float it = 0;
                        if (gstemp.MinorInterval)
                        {
                            Pen minpen = new Pen(gstemp.MinorLineStyle.ForeColor, gstemp.MinorLineStyle.Thickness);
                            if (gstemp.MinorDivision > 0)
                                for (it = gstemp.Minimum; it <= gstemp.Maximum; it += gstemp.MinorDivision)
                                {
                                    float iti = it;
                                    iti = _width - (iti - gstemp.Minimum) * _width / (gstemp.Maximum - gstemp.Minimum);
                                    if (iti <= 0) iti = 0;
                                    if (iti >= _width) iti = _width;
                                    g.DrawLine(minpen, iti, 7.5f, iti, 12.5f);
                                }
                            g.DrawLine(minpen, _width, 7.5f, _width, 12.5f);
                        }
                        if (gstemp.MajorInterval)
                        {
                            Pen majpen = new Pen(gstemp.MajorLineStyle.ForeColor, gstemp.MajorLineStyle.Thickness);
                            if (gstemp.MajorDivision > 0)
                                for (it = gstemp.Minimum; it <= gstemp.Maximum; it += gstemp.MajorDivision)
                                {
                                    float iti = it;
                                    iti = _width - (iti - gstemp.Minimum) * _width / (gstemp.Maximum - gstemp.Minimum);
                                    if (iti <= 0) iti = 0;
                                    if (iti >= _width) iti = _width;
                                    g.DrawLine(majpen, iti, 5f, iti, 12.5f);
                                }
                            g.DrawLine(majpen, _width, 5f, _width, 12.5f);
                        }
                    }
                    g.TranslateTransform(0, 20);
                    {
                        GridStyle gstemp = ((ValueColumn)this).HorizontalScaleBar;
                        Font _font = new Font(FontFamily.GenericSansSerif, 8);
                        string text;
                        Size s;

                        if (gstemp.Linear == ScaleType.Linear)
                        {
                            text = gstemp.Minimum.ToString();
                            s = TextRenderer.MeasureText(text, _font);
                            g.DrawString(text, _font, Brushes.Black, 0, -5);
                            float lpos = _width / 2;
                            text = ((gstemp.Minimum + gstemp.Maximum) / 2).ToString();
                            s = TextRenderer.MeasureText(text, _font);
                            if (lpos > s.Width / 2)
                                lpos -= s.Width / 2;
                            g.DrawString(text, _font, Brushes.Black, lpos, -5);
                            if (_width >= 150)
                            {
                                #region quarters
                                lpos = _width / 4;
                                text = (gstemp.Minimum + (gstemp.Maximum - gstemp.Minimum) *0.25).ToString();
                                s = TextRenderer.MeasureText(text, _font);
                                if (lpos > s.Width / 2)
                                    lpos -= s.Width / 2;
                                g.DrawString(text, _font, Brushes.Black, lpos, -5);

                                lpos = _width * 3 / 4;
                                text = (gstemp.Minimum + (gstemp.Maximum - gstemp.Minimum) * 0.75).ToString();
                                s = TextRenderer.MeasureText(text, _font);
                                if (lpos > s.Width / 2)
                                    lpos -= s.Width / 2;
                                g.DrawString(text, _font, Brushes.Black, lpos, -5);
                                #endregion quarters
                            }
                            text = gstemp.Maximum.ToString();
                            s = TextRenderer.MeasureText(text, _font);
                            g.DrawString(text, _font, Brushes.Black, _width - s.Width, -5);
                        }
                        else
                        {
                            int i = (int)(Math.Ceiling(Math.Log10(gstemp.Minimum)));
                            int j = (int)(Math.Floor(Math.Log10(gstemp.Maximum)));
                            if (j > i)
                            {
                                gstemp.MajorDivision = (int)((gstemp.Maximum - gstemp.Minimum) / (j - i));
                                gstemp.MinorDivision = (int)((gstemp.Maximum - gstemp.Minimum) / (2 * (j - i)));
                                for (int k = i; k <= j; k++)
                                {
                                    text = Math.Pow(10, k).ToString();
                                    s = TextRenderer.MeasureText(text, _font);
                                    double lpos = _width * (k - Math.Log10(gstemp.Minimum)) / (Math.Log10(gstemp.Maximum) - Math.Log10(gstemp.Minimum));
                                    lpos = Math.Max(lpos, 0);
                                    lpos = Math.Min(lpos, _width - s.Width);
                                    g.DrawString(text, _font, Brushes.Black, (float)lpos, -5);
                                }
                            }
                        }
                        g.DrawLine(Pens.Black, 0f, 12.5f, _width, 12.5f);
                        float it = 0;
                        if (gstemp.MinorInterval)
                        {
                            Pen minpen = new Pen(gstemp.MinorLineStyle.ForeColor, gstemp.MinorLineStyle.Thickness);
                            if (gstemp.MinorDivision > 0)
                                for (it = gstemp.Minimum; it <= gstemp.Maximum; it += gstemp.MinorDivision)
                                {
                                    float iti = it;
                                    iti = _width - (iti - gstemp.Minimum) * _width / (gstemp.Maximum - gstemp.Minimum);
                                    if (iti <= 0) iti = 0;
                                    if (iti >= _width) iti = _width;
                                    g.DrawLine(minpen, iti, 7.5f, iti, 12.5f);
                                }
                            g.DrawLine(minpen, _width, 7.5f, _width, 12.5f);
                        }
                        if (gstemp.MajorInterval)
                        {
                            Pen majpen = new Pen(gstemp.MajorLineStyle.ForeColor, gstemp.MajorLineStyle.Thickness);
                            if (gstemp.MajorDivision > 0)
                                for (it = gstemp.Minimum; it <= gstemp.Maximum; it += gstemp.MajorDivision)
                                {
                                    float iti = it;
                                    iti = _width - (iti - gstemp.Minimum) * _width / (gstemp.Maximum - gstemp.Minimum);
                                    if (iti <= 0) iti = 0;
                                    if (iti >= _width) iti = _width;
                                    g.DrawLine(majpen, iti, 5f, iti, 12.5f);
                                }
                            g.DrawLine(majpen, _width, 5f, _width, 12.5f);
                        }
                    }
                    g.TranslateTransform(0, 15);
                }
                else
                    g.TranslateTransform(0, 35);
                #endregion

                //height -= 35;
                if (_backcolor != Color.Transparent)
                    g.FillRectangle(new SolidBrush(_backcolor), columnrect);
                if (!TotalGridStyle.GridOnTop)
                    DrawGrid(columnrect, g);
                Matrix m = g.Transform;
                retval += generate(top, bottom, zoom, new SolidBrush(_forecolor));
                g.Transform = m;
                if (TotalGridStyle.GridOnTop)
                    DrawGrid(columnrect, g);
                if (_Border == BorderStyle.Fixed3D)
                {
                    columnrect.Offset(1, 1);
                    g.DrawRectangle(Pens.Black, columnrect);
                }
                else
                    if (_Border != BorderStyle.None)
                    {
                        columnrect.Inflate(-1, -1);
                        g.DrawRectangle(Pens.Black, columnrect);
                    }
                g.TranslateTransform(0, -35);
            }
            catch (Exception ex)
            {
                g.DrawString(ex.StackTrace, SystemFonts.DefaultFont, new SolidBrush(Color.Purple), new RectangleF(0, 0, _width, 100));
            }

            hash = retval;
            _changed = false;
            return retval;
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
        protected Boolean _visible = true;
        protected ColumnType _type = ColumnType.Histogram;
        protected BorderStyle _Border = BorderStyle.None;

        public Boolean Visible { get { return _visible; } set { _visible = value; } }

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
        [ReadOnly(true), Browsable(false)]
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
        [CategoryAttribute("Position"), DisplayName("Start Point"), DescriptionAttribute("Column Left Side")]
        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }
        [XmlIgnore, Browsable(false), CategoryAttribute("Position"), DescriptionAttribute("Column Right Side")]
        public int Right
        {
            get { return _left + _width; }
            set { _width = value - _left; }
        }

        [CategoryAttribute("Position"), DescriptionAttribute("Column Width")]
        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                    Invalidate();
                _width = value;
            }
        }

        [CategoryAttribute("Appearance"), DisplayName("Border"), DescriptionAttribute("Has border around it?")]
        public BorderStyle Frame
        {
            get { return _Border; }
            set { _Border = value; }
        }

        [CategoryAttribute("General"), DescriptionAttribute("Type of Column?")]
        [ReadOnly(true), Browsable(false)]
        public ColumnType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [CategoryAttribute("General"), DescriptionAttribute("Type of Column?")]
        [ReadOnly(true), Browsable(true),DisplayName("Type")]
        public String TypeName
        {
            get { return _type.ToString(); }
        }

        protected virtual void DrawGrid(Rectangle r, Graphics g)
        {
            if (this is ValueColumn)
                if (_tgs.PlotValueGrid)
                {
                    GridStyle gs = ((ValueColumn)this).HorizontalScaleBar;
                    if (gs != null)
                    {
                        if (gs.MinorInterval)
                        {
                            LineStyle ls = gs.MinorLineStyle;
                            Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                            _p.DashStyle = ls.Style;
                            if (gs.MinorDivision > 0)
                                for (float i = gs.Minimum; i <= gs.Maximum; i += gs.MinorDivision)
                                {
                                    float iti = i;
                                    //if (gs.Linear == ScaleType.Logarithmic)                                        iti = (float)Math.Pow(10, iti);
                                    iti = (iti - gs.Minimum) * _width / (gs.Maximum - gs.Minimum);
                                    if (iti <= 0)
                                        iti = 0;
                                    if (iti >= _width)
                                        iti = _width;
                                    g.DrawLine(_p, iti, 0, iti, r.Height);
                                }
                        }
                        if (gs.MajorInterval)
                        {
                            LineStyle ls = gs.MajorLineStyle;
                            Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                            _p.DashStyle = ls.Style;
                            if (gs.MajorDivision > 0)
                                for (float i = gs.Minimum; i <= gs.Maximum; i += gs.MajorDivision)
                                {
                                    float iti = i;
                                    //if (gs.Linear == ScaleType.Logarithmic)                                        iti = (float)Math.Pow(10, iti);
                                    iti = (iti - gs.Minimum) * _width / (gs.Maximum - gs.Minimum);
                                    if (iti <= 0)
                                        iti = 0;
                                    if (iti >= _width)
                                        iti = _width;
                                    g.DrawLine(_p, iti, 0, iti, r.Height);
                                }
                        }
                    }
                }
            if (_tgs.PlotDepthGrid)
            {
                GridStyle gs = _tgs.DepthGridStyle;
                if (gs != null)
                {
                    if (gs.MinorInterval)
                    {
                        LineStyle ls = gs.MinorLineStyle;
                        Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                        _p.DashStyle = ls.Style;
                        if (gs.MinorDivision > 0)
                            for (int i = 1; i < r.Height; i += gs.MinorDivision)
                                g.DrawLine(_p, 0, i, r.Width, i);
                    }
                    if (gs.MajorInterval)
                    {
                        LineStyle ls = gs.MajorLineStyle;
                        Pen _p = new Pen(ls.ForeColor, ls.Thickness);
                        _p.DashStyle = ls.Style;
                        if (gs.MajorDivision > 0)
                            for (int i = 1; i < r.Height; i += gs.MajorDivision)
                                g.DrawLine(_p, 0, i, r.Width, i);
                    }
                }
            }
        }
        public string changes()
        {
            if (_changed)
                return _name + "has changed" + Environment.NewLine;
            return null;
        }
        public Rectangle rect(int left, int height)
        {
            return new Rectangle(_left + left, 0, _width, height+35);
        }
    }
    [Serializable()]
    public class TadpoleColumn : ValueColumn
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }
        //ignore list
        public TadpoleColumn()
            : base()
        {
            _gs.Minimum = 0;
            _gs.Maximum = 90;
        }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            DataRow[] drs = lds.tadpole.Select(" entity = " + entity.ToString() + " and (depth-value)<=" + BottomLimit.ToString() + " and (depth+value)>=" + TopLimit.ToString() , "depth");
            foreach (logplotDataSet1.tadpoleRow dr in drs)
            {
                if (dr.Dip < 0)
                    dr.Dip += Math.Ceiling(-dr.Dip / 90) * 90;
                if (dr.Dip > 90)
                    dr.Dip %= 90;
                int tadcenter = (int)dr.Dip * _width / 90;
                g.FillEllipse(new SolidBrush(ForeColor), (float)(tadcenter - dr.Value), (float)((dr.Depth - TopLimit) * zoom - dr.Value), (float)dr.Value * 2, (float)dr.Value * 2);
                //g.DrawEllipse(new Pen(ForeColor), tadcenter - 10, (int)((dr.Depth - TopLimit) * zoom) - 10, 20, 20);
                g.TranslateTransform(tadcenter, (int)((dr.Depth - TopLimit) * zoom));
                g.RotateTransform((float)dr.Azimuth - 90);
                g.DrawLine(new Pen(ForeColor), (float)dr.Value, 0, 2 * (float)dr.Value, 0);
                g.RotateTransform(-(float)dr.Azimuth + 90);
                g.TranslateTransform(-tadcenter, -(int)((dr.Depth - TopLimit) * zoom));
            }
            return retval;
        }
    }
    [Serializable()]
    public class ValueColumn : Column
    {
        protected GridStyle _gs = new GridStyle();
        [DisplayName("Horizontal ScaleBar")]
        public GridStyle HorizontalScaleBar { get { return _gs; } set { _gs = value; } }
        //protected override void DrawGrid(Rectangle r, Graphics g)        {            base.DrawGrid(r, g);        }
    }

    [Serializable()]
    public class WellColumn : ValueFilledColumn
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        //ignore list

        protected double _diameter = 12;
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                if (value > 0) _diameter = value;
                _gs.Minimum = (int)-_diameter / 2;
                _gs.Maximum = (int)_diameter / 2;
            }
        }

        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            GraphicsState transState = g.Save();
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.well.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.wellRow dr in drs)
            {
                LithologyTemplate lt = getLithologyTemplate(LithologySetting, dr.Material);
                TextureBrush tb;
                if (lt != null)
                {
                    tb = lt.Texture;
                }
                else
                {
                    Bitmap b = MainForm.GetResourceBMP(lt.pattern);
                    b = myImage.imageutilities.recolor(b, new Color[] { Color.Black, Color.Transparent }, new Color[] { lt.ForeColor, lt.BackColor });
                    tb = new TextureBrush(b);
                }
                B = tb;

                if (dr.Outer < dr.Inner)
                    continue;
                float x1 = (float)((_diameter - dr.Outer) * _width / 2 / _diameter);
                float x2 = (float)((_diameter - dr.Inner) * _width / 2 / _diameter);
                float x3 = (float)(_width - x2);
                float x4 = (float)(_width - x1);
                Pen p = new Pen(_forecolor);

                g.TranslateTransform(x1, (float)math.pos(dr.Top - TopLimit) * zoom);
                g.FillRectangle(B, 0, 0, x2 - x1, (float)(dr.Base - dr.Top) * zoom);
                g.DrawLine(p, 0, 0, 0, (float)(dr.Base - dr.Top) * zoom);
                g.DrawLine(p, x2 - x1, 0, x2 - x1, (float)(dr.Base - dr.Top) * zoom);
                g.TranslateTransform(-x1 + x3, 0);
                g.FillRectangle(B, 0, 0, x2 - x1, (float)(dr.Base - dr.Top) * zoom);
                g.DrawLine(p, 0, 0, 0, (float)(dr.Base - dr.Top) * zoom);
                g.DrawLine(p, x2 - x1, 0, x2 - x1, (float)(dr.Base - dr.Top) * zoom);
                g.TranslateTransform(-x3, -(float)math.pos(dr.Top - TopLimit) * zoom);
                retval += (int)(dr.Top + dr.Base + dr.Outer + dr.Inner);
            }
            return retval;
        }
    }

    [Serializable()]
    public class FossilColumn : FilledColumn
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }
        //ignore list

        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            GraphicsState transState = g.Save();
            getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.fossil.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.fossilRow dr in drs)
            {
                g.TranslateTransform(0, (float)math.pos((dr.Top - TopLimit) * zoom));
                Pen p = new Pen(B);
                p.Width = 3;
                if (dr.Distribution_Of_Fossiles < 0)
                    dr.Distribution_Of_Fossiles = 0;
                if (dr.Distribution_Of_Fossiles > 6)
                    dr.Distribution_Of_Fossiles = 6;
                /*
                                switch (dr.Distribution_Of_Fossiles)
                                {
                                    case 1:
                                        //   p.DashStyle = DashStyle.Dot;
                                        //   g.DrawLine(p, _width / 2, 0, _width / 2, (float)(dr.Base - dr.Top) * zoom);
                                        g.FillRectangle(B, _width / 2 - _width / 32, 0, _width / 16, (float)(dr.Base - dr.Top) * zoom);
                                        break;
                                    case 2:
                                        //   g.DrawLine(p, _width / 2, 0, _width / 2, (float)(dr.Base - dr.Top) * zoom);
                                        g.FillRectangle(B, _width / 2 - _width / 16, 0, _width / 8, (float)(dr.Base - dr.Top) * zoom);
                                        break;
                                    case 3:
                                        g.FillRectangle(B, _width / 2 - _width / 8, 0, _width / 4, (float)(dr.Base - dr.Top) * zoom);
                                        break;
                                    case 4:
                                        g.FillRectangle(B, _width / 2 - _width / 6, 0, _width / 3, (float)(dr.Base - dr.Top) * zoom);
                                        break;
                                    case 5:
                                        g.FillRectangle(B, _width / 4, 0, _width / 2, (float)(dr.Base - dr.Top) * zoom);
                                        break;
                                    case 6:
                                        g.FillRectangle(B, _width / 8, 0, _width * 3 / 4, (float)(dr.Base - dr.Top) * zoom);
                                        break;
                                } 
                 */
                int zarib = (int)(_width/12);
                if (zarib < 1) zarib = 1;
                switch (dr.Distribution_Of_Fossiles)
                {
                    case 1:
                        g.FillRectangle(B, _width / 2 - 1 * zarib, 0, 2 * zarib, (float)(dr.Base - dr.Top) * zoom);
                        break;
                    case 2:
                        g.FillRectangle(B, _width / 2 - 2 * zarib, 0, 4 * zarib, (float)(dr.Base - dr.Top) * zoom);
                        break;
                    case 3:
                        g.FillRectangle(B, _width / 2 - 3 * zarib, 0, 6 * zarib, (float)(dr.Base - dr.Top) * zoom);
                        break;
                    case 4:
                        g.FillRectangle(B, _width / 2 - 4 * zarib, 0, 8 * zarib, (float)(dr.Base - dr.Top) * zoom);
                        break;
                    case 5:
                        g.FillRectangle(B, _width / 2 - 5 * zarib, 0, 10 * zarib, (float)(dr.Base - dr.Top) * zoom);
                        break;
                    case 6:
                        g.FillRectangle(B, _width / 2 - 6 * zarib, 0, 12 * zarib, (float)(dr.Base - dr.Top) * zoom);
                        break;
                }
                g.TranslateTransform(0, -(float)math.pos((dr.Top - TopLimit) * zoom));
                retval += (int)(dr.Top + dr.Base + dr.Distribution_Of_Fossiles);
            }
            return retval;
        }
    }

    [Serializable()]
    public class SeaColumn : ValueColumn
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }
        //ignore list

        protected int _levels = 7;
        public int Levels
        {
            get { return _levels; }
            set
            {
                if (value > 1)
                {
                    _levels = value;
                    _gs.Minimum = 0;
                    _gs.Maximum = value;
                }
            }
        }
        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            GraphicsState transState = g.Save();
            int index = 0;

            //getBrush(tfs, (int)(BottomLimit - TopLimit), out B);
            DataRow[] drs = lds.sea.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            
            if (drs.Length > 0)
            {
                PointF[] pf = new PointF[drs.Length * 3+1];
                foreach (logplotDataSet1.seaRow dr in drs)
                {
                    if (index > 0)
                        if ((dr.Value + 1) * _width / (_levels + 2) < pf[index - 1].X)
                            index--;
                    pf[index].X = (dr.Value + 1) * _width / (_levels + 2);
                    pf[index].Y = zoom * dr.Top;
                    index++;

                    pf[index].X = (dr.Value + 1) * _width / (_levels + 2);
                    pf[index].Y = zoom * (dr.Top + dr.Base) / 2;
                    index++;

                    pf[index].X = (dr.Value + 1) * _width / (_levels + 2);
                    pf[index].Y = zoom * dr.Base;
                    index++;
                }
                try
                {
                    pf[index - 1].X = (0 + 1) * _width / (_levels + 2);
                    pf[index].X = (0) * _width / (_levels + 2);
                    pf[index].Y = pf[index - 1].Y;
                }
                catch
                {
                    MessageBox.Show(pf.Length.ToString()+">"+index.ToString());
                }
                index++;
                int last = 0;
                int i = 1;
                g.TranslateTransform(0, -(float)(TopLimit * zoom));
                for (i = 1; i < index; i++)
                {
                    if (pf[i].X > pf[i - 1].X)
                    {
                        g.DrawCurve(Pens.Blue, pf, last, i - 1 - last, 0.1f);
                        g.DrawLine(Pens.Red, pf[i], pf[i - 1]);
                        last = i;
                    }
                }
                g.DrawCurve(Pens.Blue, pf, last, index - 1 - last, 0.3f);
            }
            return retval;
        }
    }

    [Serializable()]
    public class TextColumn : Column
    {
        //ignore list
        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }


        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {
            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            GraphicsState transState = g.Save();
            FontConverter f = new FontConverter();
            DataRow[] drs = lds.text.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            foreach (logplotDataSet1.textRow dr in drs)
            {
                Font _font = (Font)f.ConvertFromString(dr.Font);
                string text = dr.Text;
                float _depth = (float)(dr.Base - dr.Top);
                Size s = TextRenderer.MeasureText(text, _font);
                g.TranslateTransform(0, (float)(dr.Top - TopLimit) * zoom);
                //g.TranslateTransform(_width / 2 - s.Width / 2, (float)(dr.Top - TopLimit) * zoom - s.Height / 2);
                StringFormat sf = new StringFormat(StringFormatFlags.FitBlackBox);
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                RectangleF rectf = new RectangleF(0, 0, _width, _depth);
                g.DrawString(text, _font, B, rectf, sf);
                g.DrawRectangle(new Pen(ForeColor), 0, 0, _width, _depth);
                g.ResetTransform();
                g.Restore(transState);
                retval += (int)(_depth);
            }
            return retval;
        }
    }

    [Serializable()]
    public class VerticalTextColumn : Column
    {
        protected bool _reverse = false;
        public bool Reserve { get { return _reverse; } set { _reverse = value; } }
        protected bool _arrow = true;
        public bool Arrow { get { return _arrow; } set { _arrow = value; } }

        [Browsable(false), XmlIgnore()]
        public new TwoGridStyle TotalGridStyle { get { return null; } set { } }
        [Browsable(false), XmlIgnore(), DisplayName("Lithology Style")]
        public new LithologyCollection LithologySetting { get { return null; } set { } }

        protected override int generate(double TopLimit, double BottomLimit, float zoom, Brush B)
        {

            int retval = -1;
            if (g == null)
                g = Graphics.FromImage(bmp);
            GraphicsState transState = g.Save();
            //            DataRow[] drs = lds.text.Select(" entity = " + entity.ToString() + " and not ( depth<=" + TopLimit.ToString() + " and depth>=" + BottomLimit.ToString() + ")","depth");
            DataRow[] drs = lds.text.Select(" entity = " + entity.ToString() + " and top>=" + TopLimit.ToString() + " and base<=" + BottomLimit.ToString(), "top");
            Size s;
            FontConverter f = new FontConverter();
            foreach (logplotDataSet1.textRow dr in drs)
            {
                Font _font = (Font)f.ConvertFromString(dr.Font);
                string text = dr.Text;
                double Depth = (dr.Base + dr.Top) / 2;
                s = TextRenderer.MeasureText(g, text, _font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);

                float leng = (float)((zoom * (dr.Base - dr.Top) - s.Width) / 2) - 2; //(dr.Base - dr.Top) / 2;

                g.TranslateTransform(_width / 2, (float)(dr.Top - TopLimit) * zoom);

                if (_arrow)
                {
                    g.DrawLine(Pens.Black, 1 - _width / 2, 0, _width / 2 - 1, 0);
                    g.DrawLine(Pens.Black, 0, 1, 0, leng);
                    g.DrawLine(Pens.Black, 0, 1, +5, +5);
                    g.DrawLine(Pens.Black, 0, 1, -5, +5);
                }
                //   g.DrawRectangle(Pens.Red, -_width / 2, 0, s.Height, s.Width);
                g.TranslateTransform(-_width / 2, -(float)(dr.Top - TopLimit) * zoom);

                g.TranslateTransform(_width / 2, (float)(dr.Base - TopLimit) * zoom);

                if (_arrow)
                {
                    g.DrawLine(Pens.Black, 1 - _width / 2, 0, _width / 2 - 1, 0);
                    g.DrawLine(Pens.Black, 0, -1, 0, -leng);
                    g.DrawLine(Pens.Black, 0, -1, -5, -5);
                    g.DrawLine(Pens.Black, 0, -1, +5, -5);
                }
                g.TranslateTransform(-_width / 2, -(float)(dr.Base - TopLimit) * zoom);

                if (_reverse)
                {
                    g.TranslateTransform(_width / 2 - s.Height / 2, (float)((Depth - TopLimit) * zoom + s.Width / 2));
                    g.RotateTransform(-90);
                    g.DrawString(text, _font, B, -2, 0);
                    g.RotateTransform(90);
                    g.TranslateTransform(-(_width / 2 - s.Height / 2), -(float)((Depth - TopLimit) * zoom + s.Width / 2));
                }
                else
                {
                    g.TranslateTransform(_width / 2 + s.Height / 2, (float)((Depth - TopLimit) * zoom - s.Width / 2));
                    g.RotateTransform(90);
                    g.DrawString(text, _font, B, -2, 0);
                    g.RotateTransform(-90);
                    g.TranslateTransform(-(_width / 2 + s.Height / 2), -(float)((Depth - TopLimit) * zoom - s.Width / 2));
                }
                retval += (int)(Depth);
            }
            return retval;
        }
    }

    public class PointFComparer : IComparer<PointF>
    {
        public int Compare(PointF x, PointF y)
        {
            //if (x.Y == y.Y)                return 0;
            if (x.Y < y.Y)
                return -1;
            if (x.Y > y.Y)
                return 1;
            return 0;
        }
    }
}
