using System;
using System.Collections;
using System.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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

namespace ParsLogPlot
{
    public partial class MainForm : Form
    {
        public string Scaling { get { return zoomDropDown.Text; } }
        int testcounter = 0;
        int LEFTSCROLL = 0;
        /*
        public Int32 HEIGHT
        {
            get
            {
                long x = (long)((BASE - TOP) * ydpi * (Convert.ToDouble(zoomDropDown.Tag))); 
                if (x < 1) 
                    x = 1; 
                if (x > int.MaxValue)
                    x = int.MaxValue; 
                return (Int32)x;
            }
        }
         */
        public PictureBox CONTENT { get { return showbox; } }
        PictureBox CreateHeader(int count, int left)
        {
            PictureBox temp = new PictureBox();
            temp.Visible = true;
            temp.Width = 100;
            temp.BackColor = Color.Transparent;
            temp.ContextMenuStrip = boxcontextMenuStrip;
            temp.Height = headerbox.Height - 10;
            temp.Top = 5;
            temp.Tag = count.ToString();
            temp.Left = left;
            temp.SizeMode = PictureBoxSizeMode.StretchImage;
            temp.BorderStyle = BorderStyle.None;
            temp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            temp.MouseUp += new MouseEventHandler(pn_MouseUp);
            temp.MouseDown += new MouseEventHandler(pn_MouseDown);
            temp.MouseMove += new MouseEventHandler(pn_MouseMove);
            temp.Click += new EventHandler(pn_Click);
            temp.Paint += new PaintEventHandler(p_Paint);
            return temp;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (count >= 100)
                return;
            count++;
            String ta = ((ToolStripButton)sender).Tag.ToString();
            int c = Convert.ToInt32(ta) - 1;
            string caption = ((ToolStripItem)sender).Text.ToString() + (count + 1).ToString();

            int rights = 0;
            for (int i = 0; i < count; i++)
                if (p[i].Right > rights)
                    rights = p[i].Right;
            rights += 3;
            p[count] = CreateHeader(count, rights);
            u[count] = getColumn((ColumnType)c);

            u[count].lds = logplotDataSet1;
            u[count].Left = p[count].Left;
            u[count].Width = p[count].Width;
            u[count].ID = count;
            u[count].Name = caption;
            u[count].Type = (ColumnType)c;
            entityTableAdapter.Insert(caption, count, "", (short)c);
            this.entityTableAdapter.Fill(this.logplotDataSet1.entity);
            logplotDataSet1.entityDataTable dt = entityTableAdapter.GetData();
            logplotDataSet1.entityRow dr = (logplotDataSet1.entityRow)dt.Rows[dt.Rows.Count - 1];
            u[count].Frame = BorderStyle.FixedSingle;
            u[count].entity = dr.id;
            u[count].position = count;
            headerbox.Controls.Add(p[count]);
            ID = count;
        }
        private void histogramBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType != ListChangedType.Reset)
                if (ID > -1)
                {
                    u[ID].Invalidate();
                    fresh(ID);
                }
        }
        private void p_Paint(object sender, PaintEventArgs e)
        {
            String ta = ((PictureBox)sender).Tag.ToString();
            int c = Convert.ToInt32(ta);
            Rectangle rec = p[c].ClientRectangle;
            Color col = headerbox.BackColor; //ContainerControl.DefaultBackColor;
            col = Color.Red;
            col = Color.FromArgb(Math.Max(0, col.R - 5), Math.Max(0, col.G - 5), Math.Max(0, col.B - 5));
            if (c == ID)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Pink, 3), rec);
                e.Graphics.DrawRectangle(new Pen(Color.Red, 1), rec);
                col = Color.LightPink;
                rec.Inflate(-1, -1);
                e.Graphics.DrawRectangle(new Pen(Color.Pink, 1), rec);
            }
            else
            {
                rec.Inflate(-1, -1);
                e.Graphics.DrawRectangle(new Pen(Color.Pink, 1), rec);
                rec.Inflate(1, 1);
                rec.Offset(-1, -1);
                e.Graphics.DrawRectangle(new Pen(Color.Red, 1), rec);
                col = Color.Salmon;
            }
            e.Graphics.FillRectangle(new SolidBrush(col), rec);
            //            e.Graphics.DrawString(u[c].Name + "(" + u[c].redrawcount.ToString() + ")", new Font(FontFamily.GenericSansSerif, 8), Brushes.Black, 10f, 10f);
            string header = "null " + c.ToString();
            if (u[c] != null)
            {
                header = u[c].Name;
                if (u[c].Type == ColumnType.ScaleBar)
                {
                    if (((ScaleBarColumn)u[c]).Measure == MEASURE.Foot)
                        header += "(f)";
                    else
                        header += "(m)";
                }
            }
            int i = 20;
            Font _f = new Font(FontFamily.GenericSansSerif, i);
            Size s = TextRenderer.MeasureText(header, _f);
            while (((s.Width > rec.Width) | (s.Height > rec.Height)) & i > 7)
            {
                i--;
                _f = new Font(FontFamily.GenericSansSerif, i);
                s = TextRenderer.MeasureText(header, _f, new Size(p[c].ClientRectangle.Width, p[c].ClientRectangle.Height));
            }
            int x = (rec.Width - s.Width) / 2;
            int y = (rec.Height - s.Height) / 2;
            e.Graphics.DrawString(header, _f, Brushes.Black, new RectangleF(0, 0, p[c].ClientRectangle.Width, p[c].ClientRectangle.Height));
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (softquit)
                SaveData();
        }

        public void SaveData()
        {
            //logplotDataSet1.configurationDataTable cdt = configurationTableAdapter.GetData();
            logplotDataSet1.configuration.Rows[0][1] = TOP.ToString();
            logplotDataSet1.configuration.Rows[1][1] = BASE.ToString();
            logplotDataSet1.configuration.Rows[2][1] = WIDTH.ToString();
            logplotDataSet1.configuration.Rows[3][1] = AUTHOR.ToString();
            XmlSerializer ser = new XmlSerializer(header.GetType());
            StringBuilder sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            ser.Serialize(writer, header);
            logplotDataSet1.configuration.Rows[4][1] = sb.ToString();
            ser = new XmlSerializer(footer.GetType());
            sb = new StringBuilder();
            writer = new StringWriter(sb);
            ser.Serialize(writer, footer);
            logplotDataSet1.configuration.Rows[5][1] = sb.ToString();
            logplotDataSet1.configuration.Rows[6][1] = Calibration.ToString();
            //            logplotDataSet1.configuration.AcceptChanges();
            this.configurationTableAdapter.Update(logplotDataSet1.configuration);

            for (int i = 0; i <= count; i++)
                if (u[i] != null)
                    try
                    {
                        logplotDataSet1.entityDataTable dt = entityTableAdapter.GetData();
                        logplotDataSet1.entityRow[] dtr = (logplotDataSet1.entityRow[])dt.Select("id=" + u[i].entity.ToString());
                        logplotDataSet1.entityRow dr = dtr[0]; //.Rows[i];
                        dr.column = i;
                        dr.name = u[i].Name;
                        XmlSerializer mySerializer = new XmlSerializer(u[i].GetType());
                        MemoryStream ms = new MemoryStream();
                        mySerializer.Serialize(ms, u[i]);
                        byte[] bs = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(bs, 0, (int)ms.Length);
                        ASCIIEncoding a = new ASCIIEncoding();
                        String msb = new String(a.GetChars(bs));
                        dr.savedata = Convert.ToString(msb);
                        dr.type = (short)u[i].Type;

                        this.entityTableAdapter.Update(dr);

                        /*
                        logplotDataSet1.entityRow dr = (logplotDataSet1.entityRow)dt.Rows[i];
                        dr.column = i;
                        dr.name = u[i].Name;
                        XmlSerializer mySerializer = new XmlSerializer(u[i].GetType());
                        StringBuilder msb = new StringBuilder();
                        TextWriter mwriter = new StringWriter(msb);
                        mySerializer.Serialize(mwriter, u[i]);
                        dr.savedata = mwriter.ToString();
                         */
                    }
                    catch (Exception exc)
                    {
                        using (StreamWriter sw = new StreamWriter("TestFile.txt"))
                        {
                            // Add some text to the file.
                            sw.Write("This is the ");
                            sw.WriteLine("header for the file.");
                            sw.WriteLine("-------------------");
                            // Arbitrary objects can also be written to the file.
                            sw.Write(exc.Message);
                            sw.Write(exc.StackTrace);
                            sw.Write(exc.Source);
                            sw.Write(exc.ToString());
                            sw.Write("The date is: ");
                            sw.WriteLine(DateTime.Now);
                            sw.Close();
                        }
                    }

            this.tadpoleTableAdapter.Update(this.logplotDataSet1.tadpole);
            this.wellTableAdapter.Update(this.logplotDataSet1.well);
            this.percentTableAdapter.Update(this.logplotDataSet1.percent);

            this.lithologyTableAdapter.Update(this.logplotDataSet1.lithology);
            this.crossplotTableAdapter.Update(this.logplotDataSet1.crossplot);
            this.bitmapTableAdapter.Update(this.logplotDataSet1.bitmap);
            this.textTableAdapter.Update(this.logplotDataSet1.text);
            this.symbolTableAdapter.Update(this.logplotDataSet1.symbol);
            this.horizontalTableAdapter.Update(this.logplotDataSet1.horizontal);
            this.fillTableAdapter.Update(this.logplotDataSet1.fill);
            this.curveTableAdapter.Update(this.logplotDataSet1.curve);
            this.histogramTableAdapter.Update(this.logplotDataSet1.histogram);
            this.fossilTableAdapter.Update(this.logplotDataSet1.fossil);
            this.seaTableAdapter.Update(this.logplotDataSet1.sea);
            this.messageTableAdapter.Update(this.logplotDataSet1.message);
        }
        private void propertyGrid1_PropertyValueChanged_1(object s, PropertyValueChangedEventArgs e)
        {
            if (undocounter < 99)
            {
                undocounter++;
                if (undocounter >= 100)
                    undocounter = 0;
                undolist[undocounter] = ((Column)propertyGrid1.SelectedObject).serialize();
                undochange = true;
            }
            setupundoredo();

            //if (dodrag | rightresize | leftresize)                return;
            //if (e.ChangedItem.Label == "Width")
            u[ID].Invalidate();
            if ((e.ChangedItem.Label == "Start Point") | (e.ChangedItem.Label == "Width"))
            {
                synccolumns(u[ID].position);
            }
            if (splitContainer2.Panel1.HorizontalScroll.Visible == true)
                Refresh();
            if (e.ChangedItem.Label == "Type")
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = bs[(int)u[ID].Type];
            }
            if (e.ChangedItem.Label == "Profile Curve")
            {
                for (int i = 0; i <= count; i++)
                    if (u[i] != null)
                        if (u[i] is LithologyColumn)
                        {
                            LithologyColumn lc = (LithologyColumn)u[i];
                            lc.cc = null;
                            for (int j = 0; j < count; j++)
                                if (u[j] is CurveColumn)
                                    if (lc.ProfileCurve.Equals(u[j].Name))
                                    {
                                        lc.cc = (CurveColumn)u[j];
                                        break;
                                    }
                        }
            }
            if (e.ChangedItem.Label == "Name")
            {
                DataRowView drv = (DataRowView)entityBindingSource.Current;
                logplotDataSet1.entityRow dr = (logplotDataSet1.entityRow)drv.Row;
                dr.name = u[ID].Name;
                comboBox1.Refresh();
            }
            if (u[ID].Type == ColumnType.Percent)
                if (e.ChangedItem.Label == "LithologySetting")
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = bs[(int)u[ID].Type];
                    u[ID].activate(dataGridView1);
                }
            if (e.ChangedItem.Label == "LithologySetting")
                u[ID].Invalidate();
            fresh(ID);
        }
        private void entityBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (entityBindingSource.Current == null)
                ID = count;
            else
                ID = (Int32)((DataRowView)entityBindingSource.Current).Row.ItemArray[2];
            fresh(ID);
        }
        /*
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            const int const_WM_APP = 0x0014;
            if (m.Msg == const_WM_APP+1)
                {
                //do something
                m.Result = (IntPtr)1;
            }
            else
            base.WndProc(ref m);
        }
        */
        /*
        // Creates a  message filter.
        public class TestMessageFilter : IMessageFilter
        {
            /** @attribute SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)
             *
            public Boolean PreFilterMessage(
              
                           ref        Message m)
            {
                // Blocks all the messages relating to the left mouse button.
                if (m.get_Msg() >= 0x14 && m.get_Msg() <= 0x14)
                {

                    m.Result = 1;
                    return true;
                }
                return false;
            } //PreFilterMessage
        } //TestMessageFilter
        */

        //        TestMessageFilter myf;
        /*        protected override void WndProc(ref Message message)
                {
                    if (message.Msg == 20)
                    {
                        //WM_AMESSAGE Dispatched
                        //Let’s do something here
                        //...
                        message.Result = (IntPtr)1;
                    }
                    else
                        base.WndProc(ref message);
                }
         */
        private void initializingthread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //          Application.AddMessageFilter(myf);
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
            SetStyle(System.Windows.Forms.ControlStyles.Opaque, true);
            SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            int i = 0;
            for (i = 0; i <= count; i++)
            {
                headerbox.Controls.Add(p[i]);
            }
            ID = count;
            infooter = false;
            inheader = false;
            splash.Hide();
            toolStripContainer1.Show();
            ResumeLayout();
            fresh(-1);
            this.Cursor = Cursors.AppStarting;
        }
        public void idle(object sender, EventArgs e)
        {
            if (count > 0)
            {
                Panel p1 = headerbox;// splitContainer2.Panel1;
                PictureBox[] pic = new PictureBox[count + 1];
                for (int i = 0; i <= count; i++)
                    pic[i] = p[i];
                for (int i = 0; i <= count; i++)
                    for (int j = i + 1; j <= count; j++)
                        if (pic[i].Left > pic[j].Left)
                        {
                            PictureBox temp = pic[i];
                            pic[i] = pic[j];
                            pic[j] = temp;
                        }
                int top = 5;
                pic[0].Top = top;
                pic[0].Height = p1.Height - 10;
                for (int i = 0; i < count; i++)
                {
                    pic[i].Top = top;
                    if (pic[i + 1].Left < pic[i].Right)
                    {
                        top += pic[i].Height / 2 + 5;
                        pic[i].Height /= 2;
                        pic[i + 1].Height = pic[i].Height;
                    }
                    else
                    {
                        top = 5;
                        pic[i + 1].Height = p1.Height - 10;
                    }
                    pic[i + 1].Top = top;
                }
            }
        }
        Column getColumn(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.ScaleBar:
                    return new ScaleBarColumn();
                case ColumnType.Histogram:
                    return new HistogramColumn();
                case ColumnType.HistogramValue:
                    return new HistogramValueColumn();
                case ColumnType.Curve:
                    return new CurveColumn();
                case ColumnType.Tadpole:
                    return new TadpoleColumn();
                case ColumnType.CrossPlotCurve:
                    return new CrossPlotCurveColumn();
                case ColumnType.Percent:
                    return new PercentColumn();
                case ColumnType.LithologyPattern:
                    return new LithologyColumn();
                case ColumnType.Symbol:
                    return new SymbolColumn();
                case ColumnType.Bitmap:
                    return new BitmapColumn();
                case ColumnType.Fossil:
                    return new FossilColumn();
                case ColumnType.SeeLevelChange:
                    return new SeaColumn();
                case ColumnType.FillBar:
                    return new FillBarColumn();
                case ColumnType.WellConstruction:
                    return new WellColumn();
                case ColumnType.Text:
                    return new TextColumn();
                case ColumnType.VerticalText:
                    return new VerticalTextColumn();
                default:
                    return new Column();
            }
        }
        Type getType(ColumnType type)
        {

            switch (type)
            {
                case ColumnType.ScaleBar:
                    return new ScaleBarColumn().GetType();
                case ColumnType.Histogram:
                    return new HistogramColumn().GetType();
                case ColumnType.HistogramValue:
                    return new HistogramValueColumn().GetType();
                case ColumnType.Fossil:
                    return new FossilColumn().GetType();
                case ColumnType.SeeLevelChange:
                    return new SeaColumn().GetType();
                case ColumnType.FillBar:
                    return new FillBarColumn().GetType();
                case ColumnType.WellConstruction:
                    return new WellColumn().GetType();
                case ColumnType.Curve:
                    return new CurveColumn().GetType();
                case ColumnType.Tadpole:
                    return new TadpoleColumn().GetType();
                case ColumnType.CrossPlotCurve:
                    return new CrossPlotCurveColumn().GetType();
                case ColumnType.Percent:
                    return new PercentColumn().GetType();
                case ColumnType.LithologyPattern:
                    return new LithologyColumn().GetType();
                case ColumnType.Symbol:
                    return new SymbolColumn().GetType();
                case ColumnType.Bitmap:
                    return new BitmapColumn().GetType();
                case ColumnType.Text:
                    return new TextColumn().GetType();
                case ColumnType.VerticalText:
                    return new VerticalTextColumn().GetType();
                default:
                    return new Column().GetType();
            }
        }
        private void initializingthread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.messageTableAdapter.Fill(this.logplotDataSet1.message);
                this.configurationTableAdapter.Fill(this.logplotDataSet1.configuration);
                this.fossilTableAdapter.Fill(this.logplotDataSet1.fossil);
                this.seaTableAdapter.Fill(this.logplotDataSet1.sea);
                this.tadpoleTableAdapter.Fill(this.logplotDataSet1.tadpole);
                this.wellTableAdapter.Fill(this.logplotDataSet1.well);
                this.percentTableAdapter.Fill(this.logplotDataSet1.percent);

                this.lithologyTableAdapter.Fill(this.logplotDataSet1.lithology);
                this.crossplotTableAdapter.Fill(this.logplotDataSet1.crossplot);
                this.bitmapTableAdapter.Fill(this.logplotDataSet1.bitmap);
                this.textTableAdapter.Fill(this.logplotDataSet1.text);
                this.symbolTableAdapter.Fill(this.logplotDataSet1.symbol);
                this.horizontalTableAdapter.Fill(this.logplotDataSet1.horizontal);
                this.fillTableAdapter.Fill(this.logplotDataSet1.fill);
                this.curveTableAdapter.Fill(this.logplotDataSet1.curve);
                this.entityTableAdapter.Fill(this.logplotDataSet1.entity);
                this.histogramTableAdapter.Fill(this.logplotDataSet1.histogram);
            }
            catch
            {
            }
            bs[0] = entitylithologyBindingSource;
            bs[1] = entitylithologyBindingSource;
            bs[2] = null;
            bs[3] = entitycurveBindingSource;
            bs[4] = entitycrossplotBindingSource;
            bs[5] = entitypercentBindingSource;
            bs[6] = entityhistogramBindingSource;
            bs[7] = entityhistogramBindingSource;
            bs[8] = entitytextBindingSource;
            bs[9] = entitytextBindingSource;
            bs[10] = entitysymbolBindingSource;
            bs[11] = entityfillBindingSource;
            bs[12] = entitybitmapBindingSource;
            bs[13] = entitywellBindingSource;
            bs[14] = null;
            bs[15] = null;
            bs[16] = entitytadpoleBindingSource;
            bs[17] = null;
            bs[18] = entityfossilBindingSource;
            bs[19] = entityseaBindingSource;

            dataGridView1.AutoGenerateColumns = true;
            count = -1;
            logplotDataSet1.entityDataTable dt = entityTableAdapter.GetData();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    logplotDataSet1.entityRow dr = (logplotDataSet1.entityRow)dt.Rows[i];
                    if (dr.savedata == "")
                        continue;
                    count++;
                    p[count] = CreateHeader(count, 0);
                    XmlSerializer serializer = new XmlSerializer(getType((ColumnType)dr.type));
                    MemoryStream ms = new MemoryStream();
                    byte[] bys = new byte[dr.savedata.Length];
                    ASCIIEncoding a = new ASCIIEncoding();
                    bys = a.GetBytes(dr.savedata);
                    ms.Write(bys, 0, dr.savedata.Length);
                    ms.Position = 0;
                    object o = serializer.Deserialize(ms);
                    switch (dr.type)
                    {
                        case (short)ColumnType.Histogram:
                            u[count] = (HistogramColumn)o;
                            break;
                        case (short)ColumnType.Curve:
                            u[count] = (CurveColumn)o;
                            break;
                        case (short)ColumnType.Tadpole:
                            u[count] = (TadpoleColumn)o;
                            break;
                        case (short)ColumnType.Bitmap:
                            u[count] = (BitmapColumn)o;
                            break;
                        case (short)ColumnType.CrossPlotCurve:
                            u[count] = (CrossPlotCurveColumn)o;
                            break;
                        case (short)ColumnType.LithologyPattern:
                            u[count] = (LithologyColumn)o;
                            break;
                        case (short)ColumnType.Percent:
                            u[count] = (PercentColumn)o;
                            break;
                        case (short)ColumnType.ScaleBar:
                            u[count] = (ScaleBarColumn)o;
                            break;
                        case (short)ColumnType.Symbol:
                            u[count] = (SymbolColumn)o;
                            break;
                        default:
                            u[count] = (Column)o;
                            break;
                    }
                    if (u[count] == null)
                        u[count] = getColumn((ColumnType)dr.type);
                    ms.Position = 0;
                    u[count].entity = dr.id;
                    u[count].lds = logplotDataSet1;
                    u[count].position = count;
                    dr.column = i;
                    dr.name = u[count].Name;
                    p[i].SetBounds(u[i].Left, 0, u[i].Width, 0, BoundsSpecified.X | BoundsSpecified.Width);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
            for (int i = 0; i <= count; i++)
                if (u[i] != null)
                    if (u[i] is LithologyColumn)
                    {
                        LithologyColumn lc = (LithologyColumn)u[i];
                        lc.cc = null;
                        for (int j = 0; j < count; j++)
                            if (u[j] is CurveColumn)
                                if (lc.ProfileCurve.Equals(u[j].Name))
                                {
                                    lc.cc = (CurveColumn)u[j];
                                    break;
                                }
                    }
            try
            {
                TOP = Int32.Parse(((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[0]).value);
                BASE = Int32.Parse(((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[1]).value);
                WIDTH = Int32.Parse(((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[2]).value);
                AUTHOR = ((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[3]).value;

                XmlSerializer ser = new XmlSerializer(typeof(myCollection));
                string abc;
                TextReader reader;
                try
                {
                    abc = ((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[4]).value;
                    reader = new StringReader(abc);
                    header = (myCollection)ser.Deserialize(reader);
                    foreach (myobject o in header)
                        o.unselect();
                }
                catch { }
                try
                {
                    abc = ((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[5]).value;
                    reader = new StringReader(abc);

                    footer = (myCollection)ser.Deserialize(reader);
                    foreach (myobject o in footer)
                        o.unselect();
                }
                catch { }

                Calibration = Convert.ToInt32(((logplotDataSet1.configurationRow)logplotDataSet1.configuration.Rows[6]).value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Bitmap b = new Bitmap(10, 10);
            int i = -1;
            object o = e.Value;
            string s = "-1";
            if (o != null)
                s = e.Value.ToString();
            if (s.Equals(""))
                s = "-1";
            //fix it here
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Symbol")
                    if (Int32.TryParse(s.ToString(), out i))
                    {
                        if (e.Value == null)
                        {
                            e.Value = b;
                            return;
                        }

                        e.Value = GetResourceBMP(i);
                        e.FormattingApplied = true;
                    }
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Error")
                    if (Int32.TryParse(s.ToString(), out i))
                    {
                        if (i != 0)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                        else
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                        if (e.Value == null)
                        {
                            e.Value = "0";
                            return;
                        }
                        e.Value = i.ToString();
                        e.FormattingApplied = true;
                    }
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Distribution Of Fossiles")
                    if (Int32.TryParse(s.ToString(), out i))
                    {
                        switch (i)
                        {
                            case 0:
                                e.Value = "None";
                                break;
                            case 1:
                                e.Value = "Very Rare";
                                break;
                            case 2:
                                e.Value = "Rare";
                                break;
                            case 3:
                                e.Value = "Common";
                                break;
                            case 4:
                                e.Value = "Abandant";
                                break;
                            case 5:
                                e.Value = "Very Abandant";
                                break;
                            default:
                                e.Value = "Enter between 0 to 5";
                                break;
                        }
                        e.FormattingApplied = true;
                    }
                /*
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "(Contact)")
                    if (Int32.TryParse(s.ToString(), out i))
                    {
                        Bitmap b = new Bitmap(dataGridView1.Columns[e.ColumnIndex].Width, dataGridView1.Rows[e.RowIndex].Height);
                        Graphics g = Graphics.FromImage(b);
                        switch (i)
                        {
                            case 0:
                                e.Value = null;
                                break;
                            case 1:
                                g.DrawLine(Pens.Black, 5, b.Height / 2, b.Width - 5, b.Height / 2);
                                e.Value = b;
                                break;
                            case 2:
                                e.Value = "Rare";
                                break;
                            case 3:
                                e.Value = "Common";
                                break;
                            case 4:
                                e.Value = "Abandant";
                                break;
                            case 5:
                                e.Value = "Very Abandant";
                                break;
                            case 6:
                                e.Value = "Full";
                                break;
                            default:
                                e.Value = "Enter between 0 to 6";
                                break;
                        }
                        e.FormattingApplied = true;
                    }
                 */
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Lithology")
                    if (Int32.TryParse(s.ToString(), out i))
                    {
                        if (e.Value == null)
                        {
                            e.Value = b;
                            return;
                        }

                        e.Value = myImage.imageutilities.tile(GetResourceBMP(i), dataGridView1.Columns[e.ColumnIndex].Width, dataGridView1.Rows[e.RowIndex].Height);
                        e.FormattingApplied = true;
                    }
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Contact")
                    if (Int32.TryParse(s.ToString(), out i))
                    {
                        if (e.Value == null)
                        {
                            e.Value = b;
                            return;
                        }

                        b = GetResourceBMP(i);
                        e.Value = myImage.imageutilities.tile(b, dataGridView1.Columns[e.ColumnIndex].Width, b.Height);
                        e.FormattingApplied = true;
                    }
            }
            catch
            {
                this.Text = "101: " + (++testcounter).ToString() + " > " + s;
            }
        }
        public int GetResourceID(int rownum, int type)
        {
            logplotDataSet1.resourceDataTable dt = resourceTableAdapter.GetData();
            logplotDataSet1.resourceRow[] rrs = (logplotDataSet1.resourceRow[])dt.Select("type = " + type.ToString());
            return rrs[rownum].id;
        }
        public int GetResourceRow(int id, int type)
        {
            logplotDataSet1.resourceDataTable dt = resourceTableAdapter.GetData();
            logplotDataSet1.resourceRow[] rrs = (logplotDataSet1.resourceRow[])dt.Select("type = " + type.ToString());
            for (int i = 0; i < rrs.Length; i++)
                if (rrs[i].id == id)
                    return i;
            return 0;
        }
        public Bitmap GetResource(int id, int type)
        {
            return GetResourceBMP(GetResourceID(id, type));
        }
        public static Bitmap GetResourceBMP(int id)
        {
            logplotDataSet1TableAdapters.resourceTableAdapter rta = new ParsLogPlot.logplotDataSet1TableAdapters.resourceTableAdapter();
            logplotDataSet1.resourceDataTable dt = rta.GetData();
            logplotDataSet1.resourceRow[] rrs = (logplotDataSet1.resourceRow[])dt.Select("id = " + id.ToString());
            if (rrs.Length == 0)
            {
                rrs = (logplotDataSet1.resourceRow[])dt.Select();
            }
            logplotDataSet1.resourceRow rr = rrs[0];
            string s = rr.bitmap;
            int width = rr.width;
            int height = rr.height;
            int size = width * height;
            int offset = 0;
            int curwidth = 0;
            int curheight = 0;

            Bitmap bmp = new Bitmap(width, height);
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
            return bmp;
        }
        /*
        public Bitmap getpattern(string name)
        {
            return (Bitmap)resourcepool.ResourceManager.GetObject(name);
        }

        public Bitmap getsymbol(string name)
        {
            return (Bitmap)newsymbol.ResourceManager.GetObject(name);
        }
        */
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure?","Column Delete Confirmation", MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.No)
                return;           //this has to be fixed
            if (ID < 0) return;
            p[ID].Visible = false;
            u[ID].Visible = false;
            for (int i = ID; i < count; i++)
            {
                p[i] = p[i + 1];
                p[i].Tag = (Object)i;
                u[i] = u[i + 1];
            }
            count--;
            DataRowView drv = (DataRowView)entityBindingSource.Current;
            logplotDataSet1.entityRow er = (logplotDataSet1.entityRow)drv.Row;
            er.Delete();

            entityTableAdapter.Update(logplotDataSet1.entity);
            entityTableAdapter.Fill(logplotDataSet1.entity);
            comboBox1.Refresh();
            fresh(-2);
        }
        protected Int32 ydpi = 96;
        public Int32 Calibration { get { return ydpi; } set { ydpi = value; } }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            try
            {
                double d = Convert.ToDouble(mi.Tag);
                zoomDropDown.Tag = 1 / (0.0254 * d);
                zoomDropDown.Tag = zoomDropDown.Tag.ToString();
                zoomDropDown.Text = "Scale: " + mi.Text;
                fresh(-1);
            }
            catch
            {
                MessageBox.Show(mi.Tag.ToString());
            }
        }
        bool softquit = true;
        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            softquit = false;
            this.Close();
            //Application.Exit();
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (u[ID].Type == ColumnType.Percent)
            {
                double prosity = 100;
                DataGridViewCellCollection cc = dataGridView1.Rows[e.RowIndex].Cells;
                for (int i = 2; i < cc.Count - 2; i++)
                    if (cc[i].Value != null)
                        prosity -= Convert.ToDouble(string.Concat("0", cc[i].Value));
                //if (prosity < 0) prosity = 0;
                cc["Error"].Value = prosity;
            }
            u[ID].Invalidate();
            fresh(ID);
        }
        myCollection header = new myCollection(), footer = new myCollection();
        bool inheader = false, infooter = false;
        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {
            switch (toolStripComboBox2.SelectedIndex)
            {
                case 0:
                    splitContainer1.Panel2Collapsed = false;
                    toolStrip1.Visible = true;
                    splitContainer2.Panel2Collapsed = false;
                    splitContainer3.Panel2Collapsed = true;
                    splitContainer4.Panel1Collapsed = false;
                    splitContainer4.Panel2Collapsed = true;
                    break;
                case 1:
                    splitContainer1.Panel2Collapsed = false;
                    toolStrip1.Visible = true;
                    splitContainer2.Panel2Collapsed = false;
                    splitContainer3.Panel2Collapsed = true;
                    splitContainer4.Panel1Collapsed = true;
                    splitContainer4.Panel2Collapsed = false;
                    break;
                case 2:
                    splitContainer1.Panel2Collapsed = false;
                    toolStrip1.Visible = true;
                    splitContainer2.Panel2Collapsed = false;
                    splitContainer3.Panel2Collapsed = true;
                    splitContainer4.Panel1Collapsed = false;
                    splitContainer4.Panel2Collapsed = false;
                    break;
                case 3:
                    if (infooter)
                        footer = userControl11.collection;
                    userControl11.collection = header;
                    infooter = false;
                    inheader = true;

                    splitContainer1.Panel2Collapsed = true;
                    toolStrip1.Visible = false;
                    splitContainer2.Panel2Collapsed = true;
                    splitContainer3.Panel2Collapsed = false;
                    break;
                case 4:
                    if (inheader)
                        header = userControl11.collection;
                    userControl11.collection = footer;
                    inheader = false;
                    infooter = true;

                    splitContainer1.Panel2Collapsed = true;
                    toolStrip1.Visible = false;
                    splitContainer2.Panel2Collapsed = true;
                    splitContainer3.Panel2Collapsed = false;
                    break;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Symbol")
                {
                    int i = -1;
                    if (val != null)
                        if (!int.TryParse(val.ToString(), out i))
                            i = -1;
                    resourcemanager.Form1 rm = new resourcemanager.Form1(i, 1);
                    if (rm.ShowDialog() == DialogResult.OK)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rm.pattern;
                    }
                    rm.Dispose();
                }
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Font")
                {
                    FontConverter f = new FontConverter();
                    fontDialog1.Font = (Font)f.ConvertFromString(val.ToString());
                    if (fontDialog1.ShowDialog() == DialogResult.OK)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = f.ConvertToString(fontDialog1.Font);
                    }
                }
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Lithology")
                {
                    int i = -1;
                    if (val != null)
                        if (!int.TryParse(val.ToString(), out i))
                            i = -1;
                    resourcemanager.Form1 rm = new resourcemanager.Form1(i, 0);
                    if (rm.ShowDialog() == DialogResult.OK)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rm.pattern;
                    }
                    rm.Dispose();
                }
                /*
                if ((dataGridView1.Columns[e.ColumnIndex].Name == "Name") || (dataGridView1.Columns[e.ColumnIndex].Name == "Material"))
                {
                    int i = -1;
                    if (val != null)
                        if (!int.TryParse(val.ToString(), out i))
                            i = -1;

  //                  CollectionEditor ce = new CollectionEditor(typeof(LithologyCollection));
//                    ce.EditValue(new iserviceprovider,u[id].LithologySetting);
                    //u[id].LithologySetting
                    resourcemanager.Form1 rm = new resourcemanager.Form1(i, 0);
                    if (rm.ShowDialog() == DialogResult.OK)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rm.pattern;
                    }
                    rm.Dispose();
                }
                 */
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Contact")
                {
                    int i = -1;
                    if (val != null)
                        if (!int.TryParse(val.ToString(), out i))
                            i = -1;
                    resourcemanager.Form1 rm = new resourcemanager.Form1(i, 2);
                    if (rm.ShowDialog() == DialogResult.OK)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rm.pattern;
                    }
                    rm.Dispose();
                }
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Bitmap")
                {
                    openFileDialog1.ShowDialog();
                    if (openFileDialog1.FileName != "")
                    {
                        DataGridViewImageColumn c = (DataGridViewImageColumn)dataGridView1.Columns[e.ColumnIndex];
                        c.ImageLayout = DataGridViewImageCellLayout.Zoom;
                        dataGridView1.CurrentRow.Cells["Bitmap"].Value = Bitmap.FromFile(openFileDialog1.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void swapcolumn(int i, int j)
        {
            Column c = u[i];
            u[i] = u[j];
            u[j] = c;
            PictureBox _p = p[i];
            p[i] = p[j];
            p[j] = _p;
            /*
            int a1 = splitContainer2.Panel1.Controls.GetChildIndex(p[i]);
            int a2 = splitContainer2.Panel1.Controls.GetChildIndex(p[j]);
            splitContainer2.Panel1.Controls.SetChildIndex(p[i],a2);
            splitContainer2.Panel1.Controls.SetChildIndex(p[j],a1);
            */
        }
        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((ID >= 0) & (ID < count))
                swapcolumn(ID, count);
            ID = count;
            p[ID].BringToFront();
            fresh(-1);
        }
        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ID > 0)
                swapcolumn(ID, 0);
            ID = 0;
            p[ID].SendToBack();
            fresh(-1);
        }
        private void moveForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((ID >= 0) & (ID < count))
                swapcolumn(ID, ID + 1);
            ID = ID + 1;
            fresh(-1);
        }
        private void moveBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ID > 0)
                swapcolumn(ID, ID - 1);
            ID = ID - 1;
            fresh(-1);
        }
        //private void pictureBox2_Paint(object sender, PaintEventArgs e)
        //{
        //    if (b != null)
        //        e.Graphics.DrawImage(b, e.ClipRectangle);
        //}
        protected Bitmap b;
        [ReadOnly(true)]
        public Bitmap printable
        {
            get
            {
                //splitContainer4.Panel1.Refresh();
                if (b != null)
                {
                    Bitmap hb = header.bitmap();
                    Bitmap fb = footer.bitmap();
                    int j = b.Height + hb.Height + fb.Height;
                    int i = Math.Max(Math.Max(hb.Width, fb.Width), b.Width);
                    Bitmap bmp = new Bitmap(i, j);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImageUnscaled(hb, 0, 0);
                    g.TranslateTransform(0, hb.Height);
                    g.DrawImageUnscaled(b, 0, 0);
                    g.TranslateTransform(0, b.Height);
                    g.DrawImageUnscaled(fb, 0, 0);
                    return bmp;
                }
                else
                    return new Bitmap(resourcepool.arm);
            }
        }
        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try { e.Row.Cells["Font"].Value = "Microsoft Sans Serif; 8.25pt"; }
            catch { }
            if(u[ID].Type==ColumnType.Fossil)
                e.Row.Cells["Distribution Of Fossiles"].Value = 0;
            if (u[ID].Type==ColumnType.Symbol)
            {
                e.Row.Cells["Size"].Value = 1;
                e.Row.Cells["Count"].Value = 1;
            }

        }
        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            //save undo
            /*
            if (undochange)
            {
                undochange = false;
                undocounter++;
                if (undocounter >= 100)
                    undocounter = 0;
            }
             */
            if (undochange)
                undochange = false;
            else
                if (propertyGrid1.SelectedObject is Column)
                {
                    for (int i = 0; i <= undocounter; i++)
                        undolist[i] = null;
                    undocounter = 0;
                    undolist[undocounter] = ((Column)propertyGrid1.SelectedObject).serialize();
                    setupundoredo();
                }
        }
        #region undo/redo
        string[] undolist = new string[100];
        int undocounter = 0;
        bool undochange = false;
        protected void setupundoredo()
        {
            /*
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
            if (undocounter > 0)
                if (undolist[undocounter - 1] != null)
                    undoToolStripMenuItem.Enabled = true;
            if (undocounter < 99)
                if (undolist[undocounter + 1] != null)
                    redoToolStripMenuItem.Enabled = true;
             */
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undocounter == 0)
                return;
            if (undolist[undocounter - 1] == null)
                return;
            Column dr = (Column)propertyGrid1.SelectedObject;
            string savedata = undolist[undocounter - 1];
            XmlSerializer serializer = new XmlSerializer(getType((ColumnType)dr.Type));
            MemoryStream ms = new MemoryStream();
            byte[] bys = new byte[savedata.Length];
            ASCIIEncoding a = new ASCIIEncoding();
            bys = a.GetBytes(savedata);
            ms.Write(bys, 0, savedata.Length);
            ms.Position = 0;
            object o = serializer.Deserialize(ms);
            TypeConverter tc = new TypeConverter();
            switch (dr.Type)
            {
                case ColumnType.Histogram:
                    dr = (HistogramColumn)o;
                    break;
                case ColumnType.Curve:
                    dr = (CurveColumn)o;
                    break;
                case ColumnType.Tadpole:
                    dr = (TadpoleColumn)o;
                    break;
                case ColumnType.Bitmap:
                    dr = (BitmapColumn)o;
                    break;
                case ColumnType.CrossPlotCurve:
                    dr = (CrossPlotCurveColumn)o;
                    break;
                case ColumnType.LithologyPattern:
                    dr = (LithologyColumn)o;
                    break;
                case ColumnType.Percent:
                    dr = (PercentColumn)o;
                    break;
                case ColumnType.ScaleBar:
                    dr = (ScaleBarColumn)o;
                    break;
                case ColumnType.Symbol:
                    dr = (SymbolColumn)o;
                    break;
                default:
                    dr = (Column)o;
                    break;
            }
            //undo
            undocounter--;
            setupundoredo();
            undochange = true;
            propertyGrid1.SelectedObject = dr;
            dr.Invalidate();
            fresh(ID);
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undocounter == 99)
                return;
            if (undolist[undocounter + 1] == null)
                return;
            Column dr = (Column)propertyGrid1.SelectedObject;
            string savedata = undolist[undocounter + 1];
            XmlSerializer serializer = new XmlSerializer(getType((ColumnType)dr.Type));
            MemoryStream ms = new MemoryStream();
            byte[] bys = new byte[savedata.Length];
            ASCIIEncoding a = new ASCIIEncoding();
            bys = a.GetBytes(savedata);
            ms.Write(bys, 0, savedata.Length);
            ms.Position = 0;
            object o = serializer.Deserialize(ms);
            TypeConverter tc = new TypeConverter();
            switch (dr.Type)
            {
                case ColumnType.Histogram:
                    dr = (HistogramColumn)o;
                    break;
                case ColumnType.Curve:
                    dr = (CurveColumn)o;
                    break;
                case ColumnType.Tadpole:
                    dr = (TadpoleColumn)o;
                    break;
                case ColumnType.Bitmap:
                    dr = (BitmapColumn)o;
                    break;
                case ColumnType.CrossPlotCurve:
                    dr = (CrossPlotCurveColumn)o;
                    break;
                case ColumnType.LithologyPattern:
                    dr = (LithologyColumn)o;
                    break;
                case ColumnType.Percent:
                    dr = (PercentColumn)o;
                    break;
                case ColumnType.ScaleBar:
                    dr = (ScaleBarColumn)o;
                    break;
                case ColumnType.Symbol:
                    dr = (SymbolColumn)o;
                    break;
                default:
                    dr = (Column)o;
                    break;
            }
            //undo
            undocounter++;
            setupundoredo();
            undochange = true;
            propertyGrid1.SelectedObject = dr;
            dr.Invalidate();
            fresh(ID);
        }
        public void UNDO() { undoToolStripMenuItem_Click(this, new EventArgs()); }
        public void REDO() { redoToolStripMenuItem_Click(this, new EventArgs()); }
        #endregion
        #region movement
        private void pn_MouseUp(object sender, MouseEventArgs e)
        {
            if (dodrag | rightresize | leftresize)
                u[ID].Invalidate();
            rightresize = false;
            leftresize = false;
            dodrag = false;
            fresh();
        }
        private void pn_MouseDown(object sender, MouseEventArgs e)
        {
            String ta = ((PictureBox)sender).Tag.ToString();
            ID = Convert.ToInt32(ta);
            if (dodrag | rightresize | leftresize)
                u[ID].Invalidate();
            rightresize = false;
            leftresize = false;
            dodrag = false;
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
            fresh();
        }
        private void pn_Click(object sender, EventArgs e)
        {
            String ta = ((PictureBox)sender).Tag.ToString();
            ID = Convert.ToInt32(ta);
            if (dodrag | rightresize | leftresize)
                u[ID].Invalidate();
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
                    if (dodrag | rightresize | leftresize)
                        u[ID].Invalidate();
                    rightresize = false;
                    leftresize = false;
                    dodrag = false;
                    valrec(ID);
                }
            }
            if ((Math.Abs(e.X) < 10) | (Math.Abs(e.X - pic.Width) < 10) | rightresize | leftresize)
                pic.Cursor = Cursors.SizeWE;
            else
                pic.Cursor = Cursors.Hand;
            MouseEventArgs me = new MouseEventArgs(e.Button, e.Clicks, e.X + pic.Left, e.Y + pic.Top, e.Delta);
            Form1_MouseMove(sender, me);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int ex = e.X;// +LEFTSCROLL;
            if (ID >= 0)
            {
                if (e.Button == MouseButtons.None)
                {
                    if (dodrag | leftresize | rightresize)
                    {
                        u[ID].Invalidate();
                        rightresize = false;
                        leftresize = false;
                        dodrag = false;
                        valrec(ID);
                    }
                }
                else
                    if (dodrag | rightresize | leftresize)
                    {
                        Rectangle start = u[ID].rect(-LEFTSCROLL, showboxpanel.Height);
                        
                        if (dodrag)
                        {
                            u[ID].Left = ex - dx;
                        }
                        else
                            if (rightresize)
                            {
                                if (ex - u[ID].Left > 20)
                                {
                                    u[ID].Width = e.X - p[ID].Left;
                                }
                            }
                            else
                                if (leftresize)
                                {
                                    if (u[ID].Right > 20 + ex)
                                    {
                                        u[ID].Width = p[ID].Right - e.X;
                                        u[ID].Left = ex;
                                    }
                                }
                        Rectangle finish = u[ID].rect(-LEFTSCROLL, showboxpanel.Height);
                        if (!start.Equals(finish))
                        {
                            CONTENT.Invalidate(start);
                            CONTENT.Invalidate(finish);
                            //new
                            CONTENT.Invalidate();
                        }
                        synccolumns(ID);
                    }
                    else
                        u[ID].Invalidate();
            }
            fresh();
            //this.Text = LEFTSCROLL.ToString() + " > " + ex.ToString() + " > " + u[ID].Left.ToString() + " > " + p[ID].Left.ToString();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (dodrag | rightresize | leftresize)
                u[ID].Invalidate();
            dodrag = false;
            rightresize = false;
            leftresize = false;
            fresh();
        }
        #endregion
        #region update content
        public void valrec(Column p)
        {
            CONTENT.Invalidate(p.rect(0, CONTENT.Height));
//            CONTENT.Invalidate(p.rect(-LEFTSCROLL, CONTENT.Height));
//            CONTENT.Invalidate(u[id].rect(-LEFTSCROLL, CONTENT.Height));
        }
        public void valrec(int i)
        {
            if ((i >= 0) & (i <= count))
                valrec(u[i]);
        }
        private void fresh(int id)
        {
            //CONTENT.Invalidate();
            //return;
            if (id == -2)
                CONTENT.Invalidate();
            else
                if (id == -1)
                {
                    for (int i = 0; i < count; i++)
                        if (u[i] != null)
                            u[i].Invalidate();
                    CONTENT.Invalidate();
                }
                else
                    valrec(id);
            fresh();
        }
        private void fresh()
        {
            // CONTENT.Invalidate();
            try
            {
                CONTENT.Update();
                headerbox.Refresh();
                splitContainer2.Panel1.Refresh();
            }
            catch { }
        }

        public void synccolumns(int i)
        {            
            try
            {
                p[i].SetBounds(u[i].Left, 0, u[i].Width, 0, BoundsSpecified.X | BoundsSpecified.Width);
            }
            catch (Exception x) { MessageBox.Show(x.Message); }
            propertyGrid1.Refresh();
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox2.SelectedIndex = 0;
            this.Cursor = Cursors.WaitCursor;
            splash.SetBounds((this.Width - splash.Width) / 2, (this.Height - splash.Height) / 2, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            splash.Show();
            toolStripContainer1.Hide();
            this.SuspendLayout();
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
            toolStripButton20.DisplayStyle = toolStripButton1.DisplayStyle;
            toolStripButton21.DisplayStyle = toolStripButton1.DisplayStyle;

        }
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
        }
        private void imageTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        }
        public MainForm()
        {
            InitializeComponent();
        }
        string Connection = "";
        public MainForm(string connection)
        {
            Connection = connection;
            InitializeComponent();
            this.messageTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.configurationTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.fossilTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.seaTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.tadpoleTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.wellTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.percentTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);

            this.lithologyTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.crossplotTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.bitmapTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.textTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.symbolTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.horizontalTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.fillTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.curveTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.entityTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
            this.histogramTableAdapter.Connection = new System.Data.OleDb.OleDbConnection(connection);
        }
        public BindingSource[] bs = new BindingSource[20];
        PictureBox[] p = new PictureBox[100];
        Bitmap[] bmp = new Bitmap[100];
        public Column[] u = new Column[100];
        int count = -1;
        int dx = 0;
        bool dodrag = false;
        bool rightresize = false;
        bool leftresize = false;
        protected int id = -1;
        protected int _width = 500;
        protected int _top = 0;
        protected int _base = 500;
        protected string _author = "Unknown";
        public string AUTHOR { get { return _author; } set { _author = value; aUTHORTextBox.Text = value; } }
        public int WIDTH
        {
            get { if (_width < 1) return 1; else return _width; }
            set
            {
                int v = value;
                _width = v; wIDTHTextBox.Value = v;
            }
        }
        public int TOP { get { if (_top > _base) return BASE - 1; else return _top; } set { _top = value; tOPTextBox.Value = value; } }
        public int BASE { get { if (_base < 0) return 1; else return _base; } set { _base = value; bASETextBox.Value = value; } }
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
                propertyGrid1.SelectedObject = null;
                if (value != -1)
                {
                    p[value].BringToFront();
                    entityBindingSource.Position = entityBindingSource.Find("id", u[value].entity);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = bs[(Int32)u[value].Type];
                    logsetuppanel.Visible = (dataGridView1.DataSource == null);
                    u[value].activate(dataGridView1);
                    propertyGrid1.SelectedObject = u[value];
                }
                id = value;
                fresh();
            }
        }
        private void MoveSplitter(PropertyGrid propertyGrid, int x)
        {
            object propertyGridView = typeof(PropertyGrid).InvokeMember("gridView", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGrid, null);
            propertyGridView.GetType().InvokeMember("MoveSplitterTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGridView, new object[] { x });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AUTHOR = aUTHORTextBox.Text;
            WIDTH = (int)wIDTHTextBox.Value;
            TOP = (int)tOPTextBox.Value;
            BASE = (int)bASETextBox.Value;
            Calibration = (int)yDPITextBox.Value;
            fresh(-1);
        }

        #region paste
        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            pasteshowpanel.SetBounds((this.Width - pasteshowpanel.Width) / 2, (this.Height - pasteshowpanel.Height) / 2, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            pasteshowpanel.Show();
            SuspendLayout();

            string text = null;
            text = Clipboard.GetText(TextDataFormat.Text);
            string[] lines = text.Split(new Char[] { '\n' });
            StringCollection[] wordes = new StringCollection[lines.Length];
            logplotDataSet1.entityRow er = logplotDataSet1.entity.FindByid(u[ID].entity);
            Int32 i = 0;
            foreach (string line in lines)
            {
                Application.DoEvents();
                wordes[i] = new StringCollection();
                wordes[i++].AddRange(line.Split(new Char[] { '\t' }));
            }
            #region alaki
            switch (u[id].Type)
            {
                case ColumnType.Histogram:
                    histogramTableAdapter.Update(logplotDataSet1.histogram);
                    break;
                case ColumnType.Curve:
                    curveTableAdapter.Update(logplotDataSet1.curve);
                    break;
                case ColumnType.Tadpole:
                    tadpoleTableAdapter.Update(logplotDataSet1.tadpole);
                    break;
                case ColumnType.CrossPlotCurve:
                    crossplotTableAdapter.Update(logplotDataSet1.crossplot);
                    break;
                case ColumnType.Percent:
                    percentTableAdapter.Update(logplotDataSet1.percent);
                    break;
                case ColumnType.LithologyPattern:
                    lithologyTableAdapter.Update(logplotDataSet1.lithology);
                    break;
                case ColumnType.Fossil:
                    fossilTableAdapter.Update(logplotDataSet1.fossil);
                    break;
                case ColumnType.SeeLevelChange:
                    seaTableAdapter.Update(logplotDataSet1.sea);
                    break;
                case ColumnType.FillBar:
                    fillTableAdapter.Update(logplotDataSet1.fill);
                    break;
                case ColumnType.WellConstruction:
                    wellTableAdapter.Update(logplotDataSet1.well);
                    break;
                case ColumnType.Text:
                case ColumnType.VerticalText:
                    textTableAdapter.Update(logplotDataSet1.text);
                    break;
            }
            #endregion

            try
            {
                switch (u[id].Type)
                {
                    case ColumnType.Histogram:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            histogramTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]), Convert.ToDouble(words[2]));
                        }
                        break;
                    case ColumnType.Curve:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            curveTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]));
                        }
                        break;
                    case ColumnType.Tadpole:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            tadpoleTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]), Convert.ToDouble(words[2]), Convert.ToDouble(words[3]));
                        }
                        break;
                    case ColumnType.CrossPlotCurve:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            crossplotTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]), Convert.ToDouble(words[2]));
                        }
                        break;
                    case ColumnType.Percent:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            double a1 = 0;
                            double a2 = 0;
                            double a3 = 0;
                            double a4 = 0;
                            double a5 = 0;
                            double a6 = 0;
                            double a7 = 0;
                            double a8 = 0;
                            double a9 = 0;
                            double a10 = 0;
                            double a11 = 0;
                            double a12 = 0;
                            double a13 = 0;
                            double a14 = 0;
                            double a15 = 0;
                            double a16 = 0;
                            double a17 = 0;
                            double a18 = 0;
                            double a19 = 0;
                            double a20 = 0;
                            try
                            {
                                a1 = Convert.ToDouble(words[2]);
                                a2 = Convert.ToDouble(words[3]);
                                a3 = Convert.ToDouble(words[4]);
                                a4 = Convert.ToDouble(words[5]);
                                a5 = Convert.ToDouble(words[6]);
                                a6 = Convert.ToDouble(words[7]);
                                a7 = Convert.ToDouble(words[8]);
                                a8 = Convert.ToDouble(words[9]);
                                a9 = Convert.ToDouble(words[10]);
                                a10 = Convert.ToDouble(words[11]);
                                a11 = Convert.ToDouble(words[12]);
                                a12 = Convert.ToDouble(words[13]);
                                a13 = Convert.ToDouble(words[14]);
                                a14 = Convert.ToDouble(words[15]);
                                a15 = Convert.ToDouble(words[16]);
                                a16 = Convert.ToDouble(words[17]);
                                a17 = Convert.ToDouble(words[18]);
                                a18 = Convert.ToDouble(words[19]);
                                a19 = Convert.ToDouble(words[20]);
                                a20 = Convert.ToDouble(words[21]);
                            }
                            catch { }
                            percentTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]), a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, Convert.ToDouble(words[words.Count - 2]), Convert.ToInt32(words[words.Count - 1]));
                        }
                        break;
                    case ColumnType.LithologyPattern:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            lithologyTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[1]), 0, "", Convert.ToDouble(words[0]), words[3]);
                        }
                        break;
                    case ColumnType.Fossil:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            fossilTableAdapter.Insert(u[id].entity, Convert.ToInt32(words[0]), Convert.ToInt32(words[1]), Convert.ToInt32(words[2]));
                        }
                        break;
                    case ColumnType.SeeLevelChange:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            seaTableAdapter.Insert(u[id].entity, Convert.ToInt32(words[1]), Convert.ToInt32(words[0]), Convert.ToInt32(words[2]));
                        }
                        break;
                    case ColumnType.FillBar:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            fillTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]));
                        }
                        break;
                    case ColumnType.WellConstruction:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            wellTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]), Convert.ToDouble(words[2]), Convert.ToDouble(words[3]), words[4]);
                        }
                        break;
                    case ColumnType.Text:
                    case ColumnType.VerticalText:
                        foreach (StringCollection words in wordes)
                        {
                            Application.DoEvents();
                            textTableAdapter.Insert(u[id].entity, Convert.ToDouble(words[0]), Convert.ToDouble(words[1]), "Microsoft Sans Serif; 8.25pt", words[2]);
                        }
                        break;
                }
            }
            catch { }

            #region alaki
            switch (u[id].Type)
            {
                case ColumnType.Histogram:
                    histogramTableAdapter.Fill(logplotDataSet1.histogram);
                    break;
                case ColumnType.Curve:
                    curveTableAdapter.Fill(logplotDataSet1.curve);
                    break;
                case ColumnType.Tadpole:
                    tadpoleTableAdapter.Fill(logplotDataSet1.tadpole);
                    break;
                case ColumnType.CrossPlotCurve:
                    crossplotTableAdapter.Fill(logplotDataSet1.crossplot);
                    break;
                case ColumnType.Percent:
                    percentTableAdapter.Fill(logplotDataSet1.percent);
                    break;
                case ColumnType.LithologyPattern:
                    lithologyTableAdapter.Fill(logplotDataSet1.lithology);
                    break;
                case ColumnType.Fossil:
                    fossilTableAdapter.Fill(logplotDataSet1.fossil);
                    break;
                case ColumnType.SeeLevelChange:
                    seaTableAdapter.Fill(logplotDataSet1.sea);
                    break;
                case ColumnType.FillBar:
                    fillTableAdapter.Fill(logplotDataSet1.fill);
                    break;
                case ColumnType.WellConstruction:
                    wellTableAdapter.Fill(logplotDataSet1.well);
                    break;
                case ColumnType.Text:
                case ColumnType.VerticalText:
                    textTableAdapter.Fill(logplotDataSet1.text);
                    break;
            }
            #endregion
            pasteshowpanel.Hide();
            this.Cursor = Cursors.Default;
            ResumeLayout();
        }
        #endregion
        public float ZOOM
        {
            get
            {
                float zoom;
                if (!float.TryParse((string)zoomDropDown.Tag, out zoom))
                    zoom = 1;
                return zoom;
            }
        }
        public void drawit(float dpi)
        {
            float zoom = ZOOM * dpi;
            int a = (int)((BASE - TOP) * zoom);
            //if (a >700) a = 700;
            b = new Bitmap(WIDTH, (int)(a + 50));//-dpi
            Graphics g = Graphics.FromImage(b);
            g.DrawRectangle(Pens.CadetBlue, 0, 0, b.Width, b.Height);
            foreach (Column c in u)
                if (c != null)
                    if (c.Visible)
                    {
                        c.regenerate(TOP, BASE, zoom);
                        if (c.bmp != null)
                            try
                            {
                                g.DrawImageUnscaled(c.bmp, c.Left, 0);
                            }
                            catch { }
                    }
        }
        private void splitContainer2_Panel1_SizeChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(splitContainer2.Panel1.Width.ToString());
            //showboxpanel.Width = splitContainer2.Panel1.Width;
            headerbox.Height = splitContainer2.Panel1.Height-20;
        }

        private void showbox_Paint(object sender, PaintEventArgs e)
        {
            showboxpanel.HorizontalScroll.Visible = false;
            showbox.Width = WIDTH; // Math.Min(WIDTH, showboxpanel.Width);
            headerbox.Width = WIDTH;
            showbox.Height = (int)((BASE - TOP) * e.Graphics.DpiY * ZOOM + 50);//-e.Graphics.DpiY 
            drawit(e.Graphics.DpiY);
            idle(sender, new EventArgs());
            //      e.Graphics.TranslateTransform(-splitContainer4.Panel1.HorizontalScroll.Value, -splitContainer4.Panel1.VerticalScroll.Value);
            e.Graphics.DrawImage(b, e.Graphics.ClipBounds, e.ClipRectangle, GraphicsUnit.Pixel);
        }

        private void splitContainer4_Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            if (splitContainer4.Panel1.HorizontalScroll.Value != LEFTSCROLL)
            {
                LEFTSCROLL = splitContainer4.Panel1.HorizontalScroll.Value;
                splitContainer2.Panel1.HorizontalScroll.Value = LEFTSCROLL;
                Refresh();
            }
        }
        private void splitContainer2_Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            if (LEFTSCROLL != splitContainer2.Panel1.HorizontalScroll.Value)
            {
                LEFTSCROLL = splitContainer2.Panel1.HorizontalScroll.Value;
                splitContainer4.Panel1.HorizontalScroll.Value = LEFTSCROLL;
                Refresh();
            }
        }
        private void showbox_Resize(object sender, EventArgs e)
        {
            showboxpanel.Height = showbox.Height;
            showboxpanel.Width = showbox.Width;
            headerbox.Width = showbox.Width;
        }

        private void splitContainer2_Panel1_Resize(object sender, EventArgs e)
        {
            //showboxpanel.Width = splitContainer2.Panel1.Width;
            headerbox.Height = splitContainer2.Panel1.Height-20;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if(e.KeyChar==char.Parse("v"))                dataGridView1.GetClipboardContent
        }
    }
}
