using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Dotnetrix_Samples
{
    /// <summary>
    /// Summary description for ComboBox.
    /// </summary>
    public class ComboBox : System.Windows.Forms.ComboBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ComboBox()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitComponent call

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if( components != null )
                    components.Dispose();
                if( this.EditBox != null)
                    this.EditBox.ReleaseHandle();
            }
            base.Dispose( disposing );
        }


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        private const int WM_SHOWWINDOW = 0x18;
        internal const int WM_CONTEXTMENU = 0x7B;
        private const int WM_CTLCOLOREDIT = 0x133;

        private ComboEdit EditBox;
        private ContextMenu cMenu;

        //Disassociate the ContextMenu with the control so we don't get default handling.
        public override ContextMenu ContextMenu
        {
            get
            {
                return cMenu;
            }
            set
            {
                cMenu = value;
                base.ContextMenu = null;
                this.OnContextMenuChanged(EventArgs.Empty);
            }
        }


        protected override void OnContextMenuChanged(EventArgs e)
        {
            base.OnContextMenuChanged(e);
            if (this.EditBox == null)
                return;
            this.EditBox.ContextMenu = cMenu;
        }


        [PermissionSetAttribute(SecurityAction.Demand, Name="FullTrust")]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_CTLCOLOREDIT && cMenu != null)
            {
                if (this.EditBox == null)
                    this.EditBox = new ComboEdit(m.LParam, this.Parent);
            }
            else if (m.Msg == WM_CONTEXTMENU && cMenu != null)
                this.cMenu.Show(this.Parent, this.Parent.PointToClient(new Point(m.LParam.ToInt32())));
        }

        internal class ComboEdit : NativeWindow
        {
            public ComboEdit(IntPtr hWnd, Control comboParent) : base()
            {
                this.AssignHandle(hWnd);
                this.ComboParent = comboParent;
            }


            public System.Windows.Forms.ContextMenu ContextMenu;
            private System.Windows.Forms.Control ComboParent;

            [PermissionSetAttribute(SecurityAction.Demand, Name="FullTrust")]
            protected override void WndProc(ref Message m)
            {
                if (this.ContextMenu != null)
                    if (m.Msg == WM_CONTEXTMENU)
                        this.ContextMenu.Show(ComboParent, ComboParent.PointToClient(new Point(m.LParam.ToInt32())));
                base.WndProc (ref m);
            }


        }

    }

}

Create an Image Combo

The following class modifies the Item collection to return a collection of Custom items.
The Property Browser uses the Collection Editor rather than the StringCollection Editor.
The DrawMode property has been hidden, since this property should not be changed.

RightToLeft support has not been implemented in order to keep things simple, but I'm sure you can figure that out.

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.ComponentModel.Design

Namespace Dotnetrix_Samples

    Public Class ImageCombo
        Inherits System.Windows.Forms.ComboBox

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            MyBase.DrawMode = DrawMode.OwnerDrawFixed

        End Sub

        'UserControl overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
                If Not (Me.Items Is Nothing) Then
                    For Each o As Object In Me.Items
                        If TypeOf o Is ImageComboItem Then
                            DirectCast(o, ImageComboItem).Dispose()
                        End If
                    Next
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

#End Region

        Private currentIndex As Int32 = -1

        <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
        Public Shadows ReadOnly Property DrawMode() As DrawMode
            Get
                Return DrawMode.OwnerDrawFixed
            End Get
        End Property

        <Editor(GetType(ImageComboItemEditor), GetType(UITypeEditor)), _
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        Public Shadows ReadOnly Property Items() As ObjectCollection
            Get
                Return MyBase.Items
            End Get
        End Property

        Private Class ImageComboItemEditor
            Inherits CollectionEditor

            Public Sub New(ByVal type As Type)
                MyBase.New(type)
            End Sub

            Protected Overrides Function CreateCollectionItemType() As System.Type
                Return GetType(ImageComboItem)
            End Function

        End Class

        Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)

            If e.Index = -1 OrElse e.Index > Me.Items.Count - 1 Then Return

            e.DrawBackground()

            Dim imageRect As Rectangle = New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height)
            Dim textRectF As RectangleF = RectangleF.FromLTRB(imageRect.Right + 2, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom)

            If TypeOf Items(e.Index) Is ImageComboItem Then

                Dim Item As ImageComboItem = DirectCast(Items(e.Index), ImageComboItem)

                If Not (Item.Image Is Nothing) Then
                    e.Graphics.DrawImage(Item.Image, imageRect)
                End If
            End If

            Dim TextBrush As New SolidBrush(Me.ForeColor)
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                TextBrush.Color = SystemColors.HighlightText
            End If

            Dim sf As New StringFormat(StringFormatFlags.NoWrap)
            sf.LineAlignment = StringAlignment.Center
            sf.Trimming = StringTrimming.EllipsisCharacter

            e.Graphics.DrawString(Items(e.Index).ToString, Me.Font, TextBrush, textRectF, sf)
            TextBrush.Dispose()

        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
            If Me.SelectedIndex <> currentIndex Then
                currentIndex = Me.SelectedIndex
                MyBase.RefreshItem(Me.SelectedIndex)
            Else
                MyBase.OnSelectedIndexChanged(e)
            End If
        End Sub

    End Class

    <DesignTimeVisible(False)> _
    Public Class ImageComboItem
        Inherits Component

        Private m_object As Object
        Private m_Image As Image

        <TypeConverter(GetType(StringConverter))> _
        Public Property [Item]() As Object
            Get
                Return m_object
            End Get
            Set(ByVal Value As Object)
                m_object = Value
            End Set
        End Property

        Public Property [Image]() As Image
            Get
                Return m_Image
            End Get
            Set(ByVal Value As Image)
                m_Image = Value
            End Set
        End Property

        Public Overrides Function ToString() As String
            If m_object Is Nothing Then
                Return String.Empty
            Else
                Return m_object.ToString
            End If
        End Function

    End Class

End Namespace

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.Drawing.Design;
using System.ComponentModel.Design;

namespace Dotnetrix_Samples
{
    /// <summary>
    /// Summary description for ImageCombo.
    /// </summary>
    public class ImageCombo : System.Windows.Forms.ComboBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ImageCombo()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            base.DrawMode = DrawMode.OwnerDrawVariable;

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
                if (this.Items != null)
                {
                    foreach (Object o in this.Items)
                    {
                        if (o is ImageComboItem)
                            ((ImageComboItem)o).Dispose();
                    }
                }
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        private int currentIndex = -1;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new DrawMode DrawMode
        {
            get{return DrawMode.OwnerDrawFixed;}
        }


        [Editor(typeof(ImageComboItemEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ObjectCollection Items
        {
            get{return base.Items;}
        }


        private class ImageComboItemEditor : CollectionEditor
        {
            public ImageComboItemEditor(Type type):base(type){}

            protected override Type CreateCollectionItemType()
            {
                return typeof(ImageComboItem);
            }


        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index == -1 || e.Index > this.Items.Count - 1)
                return;

            e.DrawBackground();

            Rectangle imageRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
            RectangleF textRectF = RectangleF.FromLTRB(imageRect.Right + 2, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom);
            
            if (Items[e.Index] is ImageComboItem )
            {
                ImageComboItem Item = (ImageComboItem)Items[e.Index];
                
                if (Item.Image != null)
                    e.Graphics.DrawImage(Item.Image, imageRect);
            }

            SolidBrush TextBrush = new SolidBrush(this.ForeColor);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                TextBrush.Color = SystemColors.HighlightText;

            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            e.Graphics.DrawString(Items[e.Index].ToString(), this.Font, TextBrush, textRectF, sf);
            TextBrush.Dispose();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedIndex != this.currentIndex)
            {
                this.currentIndex = this.SelectedIndex;
                base.RefreshItem(this.SelectedIndex);
            }
            else
            {
                base.OnSelectedIndexChanged (e);
            }

        }

    }

    [DesignTimeVisible(false)]
    public class ImageComboItem : Component
    {

        private Object m_object;
        private Image m_Image;

        [TypeConverter(typeof(StringConverter))]
        public Object Item
        {
            get{return m_object;}
            set{m_object = value;}
        }

        public Image Image
        {
            get{return m_Image;}
            set{m_Image = value;}
        }

        public override string ToString()
        {
            if (m_object == null)
                return String.Empty;
            else
                return m_object.ToString();
        }


    }

}

using System;
using System.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Collections.Specialized;

namespace ParsLogPlot1
{
    
    /*
    public class SymbolDataColumn : DataGridViewColumn
    {
        public SymbolDataColumn()
            : base(new SymbolDataCell())
        {
        }

        private Bitmap m_DefaultImage = newsymbol.s1;

        public Bitmap DefaultImage
        {
            get { return m_DefaultImage; }
            set { m_DefaultImage = value; }
        }

        public override object Clone()
        {
            //    return base.Clone();
            SymbolDataColumn col = base.Clone() as SymbolDataColumn;
            col.DefaultImage = m_DefaultImage;
            return col;
        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if ((value == null) || !(value is SymbolDataCell))
                {
                    throw new ArgumentException("Invalid cell type, SymbolColumns can only contain SymbolCells");
                }
                else
                    base.CellTemplate = value;
            }
        }
    }
    */
    [PropertyTabAttribute(typeof(TypeCategoryTab), PropertyTabScope.Document)]
    public class TypeCategoryTabComponent : System.ComponentModel.Component
    {
        public TypeCategoryTabComponent()
        {
        }
    }

    public class TypeCategoryTab : PropertyTab
    {
        [BrowsableAttribute(true)]
        // Der String enthält ein "Base-64 encoded" Image für das PropertyTab.
        private string img = "AAEAAAD/////AQAAAAAAAAAMAgAAAFRTeXN0ZW0uRHJhd2luZywgVmVyc2lvbj0xLjAuMzMwMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWIwM2Y1ZjdmMTFkNTBhM2EFAQAAABVTeXN0ZW0uRHJhd2luZy5CaXRtYXABAAAABERhdGEHAgIAAAAJAwAAAA8DAAAA9gAAAAJCTfYAAAAAAAAANgAAACgAAAAIAAAACAAAAAEAGAAAAAAAAAAAAMQOAADEDgAAAAAAAAAAAAD///////////////////////////////////9ZgABZgADzPz/zPz/zPz9AgP//////////gAD/gAD/AAD/AAD/AACKyub///////+AAACAAAAAAP8AAP8AAP9AgP////////9ZgABZgABz13hz13hz13hAgP//////////gAD/gACA/wCA/wCA/wAA//////////+AAACAAAAAAP8AAP8AAP9AgP////////////////////////////////////8L";

        // Rückgabe der Properties aus der Komponente.
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            PropertyDescriptorCollection props;

            if (attributes == null)
                props = TypeDescriptor.GetProperties(component);
            else
                props = TypeDescriptor.GetProperties(component, attributes);

            PropertyDescriptor[] propArray = new PropertyDescriptor[props.Count];

            for (int i = 0; i < props.Count; i++)
            {
                propArray[i] = TypeDescriptor.CreateProperty(props[i].ComponentType, props[i], new CategoryAttribute(props[i].PropertyType.Name));
            }

            return new PropertyDescriptorCollection(propArray);
        }

        public override PropertyDescriptorCollection GetProperties(object component)
        {
            return this.GetProperties(component, null);
        }

        // Name für das PropertyTab.
        public override string TabName
        {
            get
            {
                return "Mein PropertyTab";
            }
        }

        // Image für das PropertyTab.
        public override Bitmap Bitmap
        {
            get
            {
                Bitmap bmp = new Bitmap(DeserializeFromBase64Text(img));
                return bmp;
            }
        }

        private Image DeserializeFromBase64Text(string text)
        {
            Image img = null;
            byte[] memBytes = Convert.FromBase64String(text);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(memBytes);
            img = (Image)formatter.Deserialize(stream);
            stream.Close();
            return img;
        }
    }

    [TypeConverterAttribute(typeof(SpellingOptionsConverter)),
    DescriptionAttribute("Expand to see the spelling options for the application.")]
    public class SpellingOptions
    {
        private Color spellCheckWhileTyping;
        private bool spellCheckCAPS = false;
        private bool suggestCorrections = true;

        [DefaultValueAttribute(true)]
        public Color SpellCheckWhileTyping
        {
            get { return spellCheckWhileTyping; }
            set { spellCheckWhileTyping = value; }
        }

        [DefaultValueAttribute(false)]
        public bool SpellCheckCAPS
        {
            get { return spellCheckCAPS; }
            set { spellCheckCAPS = value; }
        }
        [DefaultValueAttribute(true)]
        public bool SuggestCorrections
        {
            get { return suggestCorrections; }
            set { suggestCorrections = value; }
        }
    }
    public class SpellingOptionsConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                      System.Type destinationType)
        {
            if (destinationType == typeof(SpellingOptions))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is SpellingOptions)
            {

                SpellingOptions so = (SpellingOptions)value;

                return "Check while typing:" + so.SpellCheckWhileTyping.ToString() +
                       ", check CAPS: " + so.SpellCheckCAPS.ToString() +
                       ", suggest corrections: " + so.SuggestCorrections.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context,
                              System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                              CultureInfo culture, object value)
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
                        string checkWhileTyping = s.Substring(colon + 1,
                                                        (comma - colon - 1));

                        colon = s.IndexOf(':', comma + 1);
                        comma = s.IndexOf(',', comma + 1);

                        string checkCaps = s.Substring(colon + 1,
                                                        (comma - colon - 1));

                        colon = s.IndexOf(':', comma + 1);

                        string suggCorr = s.Substring(colon + 1);

                        SpellingOptions so = new SpellingOptions();

                        so.SpellCheckWhileTyping = Color.FromName(checkWhileTyping); //Boolean.Parse(checkWhileTyping);
                        so.SpellCheckCAPS = Boolean.Parse(checkCaps);
                        so.SuggestCorrections = Boolean.Parse(suggCorr);

                        return so;
                    }
                }
                catch
                {
                    throw new ArgumentException(
                        "Can not convert '" + (string)value +
                                           "' to type SpellingOptions");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public class FileNameConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(
                                   ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection
                             GetStandardValues(ITypeDescriptorContext context)
        {
            //maybe read from a file            
            return new StandardValuesCollection(new string[]{"New File", 
                                                     "File1", 
                                                     "Document1"});
        }

        public override bool GetStandardValuesExclusive(
                                   ITypeDescriptorContext context)
        {
            return false;
        }

    }
    public class aziz
    {
        public int a, b, c;
    }
    public class GradeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs pe)
        {
            string bmpName = null;
            int g = (int)pe.Value;

            //Load SampleResources file
            string m = this.GetType().Module.Name;
            m = m.Substring(0, m.Length - 4);
            ResourceManager resourceManager = new ResourceManager(m + ".Resource1", Assembly.GetExecutingAssembly());


            if (g > 80)
            {
                bmpName = "best";
            }
            else if (g > 60)
            {
                bmpName = "ok";
            }
            else
            {
                bmpName = "bad";
            }


            //Draw the corresponding image
            Bitmap newImage = (Bitmap)resourceManager.GetObject(bmpName);
            Rectangle destRect = pe.Bounds;
            //destRect.Width *= 4;            
            newImage.MakeTransparent();
            pe.Graphics.DrawImage(newImage, destRect);

            //      Bitmap b = new Bitmap(typeof(GradeEditor), bmpName);            
            //            pe.Graphics.DrawImage(Image.FromFile(Environment.CurrentDirectory + "\\" + bmpName), pe.Bounds);
            //      pe.Graphics.DrawImage(b, pe.Bounds);
            //      b.Dispose();
        }
    }
    public class SelEditor : System.Drawing.Design.UITypeEditor
    {
        //this is a container for strings, which can be picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        //this is a string array for drop-down list
        public static string[] strList;

        public SelEditor()
        {
            Box1.BorderStyle = BorderStyle.None;
            //add event handler for drop-down box when item will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle
(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue
(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object
value)
        {
            Box1.Items.Clear();
            Box1.Items.AddRange(strList);
            Box1.Height = Box1.PreferredHeight;
            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof
(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(Box1);
                return Box1.SelectedItem;

            }
            return value;
        }

        private void Box1_Click(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();
        }
    }



    public class ctrlPwControl : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newpw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox confirm;
        private System.Windows.Forms.TextBox oldpw;
        private System.Windows.Forms.Button Cancel;

        private string _password;
        private IWindowsFormsEditorService _myEditorSvc;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ctrlPwControl()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //


        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.Cancel = new System.Windows.Forms.Button();
            this.oldpw = new System.Windows.Forms.TextBox();
            this.confirm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.newpw = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(312, 128);
            this.Cancel.Name = "Cancel";
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "&Cancel";
            this.Cancel.Click += new System.EventHandler(this.CancelClick);
            // 
            // oldpw
            // 
            this.oldpw.Location = new System.Drawing.Point(136, 24);
            this.oldpw.Name = "oldpw";
            this.oldpw.PasswordChar = '*';
            this.oldpw.Size = new System.Drawing.Size(248, 21);
            this.oldpw.TabIndex = 3;
            this.oldpw.Text = "";
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(136, 88);
            this.confirm.Name = "confirm";
            this.confirm.PasswordChar = '*';
            this.confirm.Size = new System.Drawing.Size(248, 21);
            this.confirm.TabIndex = 5;
            this.confirm.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "New Password";
            // 
            // newpw
            // 
            this.newpw.Location = new System.Drawing.Point(136, 56);
            this.newpw.Name = "newpw";
            this.newpw.PasswordChar = '*';
            this.newpw.Size = new System.Drawing.Size(248, 21);
            this.newpw.TabIndex = 4;
            this.newpw.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Old Password";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(216, 128);
            this.OK.Name = "OK";
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OKClick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Confirm";
            // 
            // ctrlPwControl
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.newpw);
            this.Controls.Add(this.oldpw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ctrlPwControl";
            this.Size = new System.Drawing.Size(416, 168);
            this.ResumeLayout(false);
        }
        #endregion

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public IWindowsFormsEditorService EditorSvc
        {
            get { return _myEditorSvc; }
            set { _myEditorSvc = value; }

        }

        void OKClick(object sender, System.EventArgs e)
        {
            if (oldpw.Text == _password && newpw.Text == confirm.Text)
            {
                _password = newpw.Text;
                _myEditorSvc.CloseDropDown();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Wrong Input!");
            }
        }

        void CancelClick(object sender, System.EventArgs e)
        {
            _myEditorSvc.CloseDropDown();
        }
    }
    public class DropDownEditor : System.Drawing.Design.UITypeEditor
    {
        ctrlPwControl ui = null;

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider sp, object value)
        {
            // get the editor service.
            IWindowsFormsEditorService edSvc =
                (IWindowsFormsEditorService)
                sp.GetService(typeof(IWindowsFormsEditorService));
            // create UI
            if (ui == null)
            {
                ui = new ctrlPwControl();
            }

            // initialize the ui 
            ui.Password = (string)value;
            ui.EditorSvc = edSvc;
            edSvc.DropDownControl(ui);
            return ui.Password;
        }
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
                return System.Drawing.Design.UITypeEditorEditStyle.Modal;

            return base.GetEditStyle(context);
        }

    }
    public class Password
    {

        private string _password;

        public Password()
        {
            _password = "";
        }

        public Password(string pw)
        {
            _password = pw;
        }

        public override string ToString()
        {
            // return ((new System.Text.StringBuilder()).Append('*',_password.Length)).ToString();
            return ((_password == null) || (_password == "")) ? "" : "*****";
        }

        public string Pw
        {
            get { return _password; }
            set { _password = value; }
        }
    }
    public class frmPasswordForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newpw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox confirm;
        private System.Windows.Forms.TextBox oldpw;
        private System.Windows.Forms.Button Cancel;

        private Password _password;
        private IWindowsFormsEditorService _myEditorSvc;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmPasswordForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.Cancel = new System.Windows.Forms.Button();
            this.oldpw = new System.Windows.Forms.TextBox();
            this.confirm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.newpw = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(312, 128);
            this.Cancel.Name = "Cancel";
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "&Cancel";
            this.Cancel.Click += new System.EventHandler(this.CancelClick);
            // 
            // oldpw
            // 
            this.oldpw.Location = new System.Drawing.Point(136, 24);
            this.oldpw.Name = "oldpw";
            this.oldpw.PasswordChar = '*';
            this.oldpw.Size = new System.Drawing.Size(248, 21);
            this.oldpw.TabIndex = 3;
            this.oldpw.Text = "";
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(136, 88);
            this.confirm.Name = "confirm";
            this.confirm.PasswordChar = '*';
            this.confirm.Size = new System.Drawing.Size(248, 21);
            this.confirm.TabIndex = 5;
            this.confirm.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "New Password";
            // 
            // newpw
            // 
            this.newpw.Location = new System.Drawing.Point(136, 56);
            this.newpw.Name = "newpw";
            this.newpw.PasswordChar = '*';
            this.newpw.Size = new System.Drawing.Size(248, 21);
            this.newpw.TabIndex = 4;
            this.newpw.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Old Password";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(216, 128);
            this.OK.Name = "OK";
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OKClick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Confirm";
            // 
            // frmPasswordForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(408, 166);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.newpw);
            this.Controls.Add(this.oldpw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPasswordForm";
            this.ResumeLayout(false);
        }
        #endregion

        public Password Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public IWindowsFormsEditorService EditorSvc
        {
            get { return _myEditorSvc; }
            set { _myEditorSvc = value; }

        }

        void OKClick(object sender, System.EventArgs e)
        {
            if (oldpw.Text == _password.Pw && newpw.Text == confirm.Text)
            {
                if (_password.Pw == newpw.Text)
                {
                    System.Windows.Forms.MessageBox.Show("Old Pw is the same as NewPw !\nNot Allowd !");
                    return;
                }
                _password.Pw = newpw.Text;
                DialogResult = DialogResult.OK;
                Hide();

                // remove the input
                oldpw.Text = "";
                newpw.Text = "";
                confirm.Text = "";

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Wrong Input!");
            }
        }

        void CancelClick(object sender, System.EventArgs e)
        {
            Hide();
        }
    }
    public class PopUpPasswordEditor : System.Drawing.Design.UITypeEditor
    {
        frmPasswordForm ui = null;

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider sp, object value)
        {
            // get the editor service.
            IWindowsFormsEditorService edSvc =
                (IWindowsFormsEditorService)
                sp.GetService(typeof(IWindowsFormsEditorService));

            // create UI
            if (ui == null)
            {
                ui = new frmPasswordForm();
            }

            // initialize the ui 
            ui.Password = (Password)value;
            ui.EditorSvc = edSvc;

            // show the Dialog
            if (System.Windows.Forms.DialogResult.OK == edSvc.ShowDialog(ui))
            {
                Console.Out.WriteLine(ui.Password.Pw);
                return ui.Password;

            }
            return base.EditValue(context, sp, value);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
                return System.Drawing.Design.UITypeEditorEditStyle.Modal;

            return base.GetEditStyle(context);
        }
    }
    public class frmPwForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newpw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox confirm;
        private System.Windows.Forms.TextBox oldpw;
        private System.Windows.Forms.Button Cancel;

        private string _password;
        private IWindowsFormsEditorService _myEditorSvc;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmPwForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //


        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.Cancel = new System.Windows.Forms.Button();
            this.oldpw = new System.Windows.Forms.TextBox();
            this.confirm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.newpw = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(312, 128);
            this.Cancel.Name = "Cancel";
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "&Cancel";
            this.Cancel.Click += new System.EventHandler(this.CancelClick);
            // 
            // oldpw
            // 
            this.oldpw.Location = new System.Drawing.Point(136, 24);
            this.oldpw.Name = "oldpw";
            this.oldpw.PasswordChar = '*';
            this.oldpw.Size = new System.Drawing.Size(248, 21);
            this.oldpw.TabIndex = 3;
            this.oldpw.Text = "";
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(136, 88);
            this.confirm.Name = "confirm";
            this.confirm.PasswordChar = '*';
            this.confirm.Size = new System.Drawing.Size(248, 21);
            this.confirm.TabIndex = 5;
            this.confirm.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "New Password";
            // 
            // newpw
            // 
            this.newpw.Location = new System.Drawing.Point(136, 56);
            this.newpw.Name = "newpw";
            this.newpw.PasswordChar = '*';
            this.newpw.Size = new System.Drawing.Size(248, 21);
            this.newpw.TabIndex = 4;
            this.newpw.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Old Password";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(216, 128);
            this.OK.Name = "OK";
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OKClick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Confirm";
            // 
            // frmPwForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(408, 166);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.newpw);
            this.Controls.Add(this.oldpw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPwForm";
            this.ResumeLayout(false);
        }
        #endregion

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public IWindowsFormsEditorService EditorSvc
        {
            get { return _myEditorSvc; }
            set { _myEditorSvc = value; }

        }

        void OKClick(object sender, System.EventArgs e)
        {
            if (oldpw.Text == _password && newpw.Text == confirm.Text)
            {
                _password = newpw.Text;
                DialogResult = DialogResult.OK;
                Hide();

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Wrong Input!");
            }
        }

        void CancelClick(object sender, System.EventArgs e)
        {
            Hide();
        }
    }
    public class PopUpPwEditor : System.Drawing.Design.UITypeEditor
    {
        frmPwForm ui = null;

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider sp, object value)
        {
            // get the editor service.
            IWindowsFormsEditorService edSvc =
                (IWindowsFormsEditorService)
                sp.GetService(typeof(IWindowsFormsEditorService));

            // create UI
            if (ui == null)
            {
                ui = new frmPwForm();
            }

            // initialize the ui 
            ui.Password = (string)value;
            ui.EditorSvc = edSvc;

            // show the Dialog
            if (System.Windows.Forms.DialogResult.OK == edSvc.ShowDialog(ui))
            {
                return ui.Password;
            }
            return base.EditValue(context, sp, value);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
                return System.Drawing.Design.UITypeEditorEditStyle.Modal;

            return base.GetEditStyle(context);
        }

        /*
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context){
            return (true);
        }

        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e){
            object o = e.Value;
            Brush b = new SolidBrush(Color.Red);
            Font f = new Font("Times New Roman", 10);
            Graphics g = e.Graphics;
            g.DrawString("***********************", f, b, e.Bounds.X, e.Bounds.Y);
        }
		
		
        */






    }

    [PropertyTab(typeof(System.Windows.Forms.Design.EventsTab), PropertyTabScope.Component)]
    [Designer(typeof(MyDesigner))]
    public class Customer : VerbHoster.VerbHost
    {
        private int _id;
        private string _name;

        private string strPw;
        private Password Pw;

        [Description("Das ist das Passwort. ReadOnly."), Category("Custom"), ReadOnly(true)]
        public string Password_01_Plain
        {
            get { return strPw; }
            set { strPw = value; }
        }

        [Description("Das ist ein einfach geschtzter Passwortdialog."), Category("Custom")]
        public string Password_02_Simple
        {
            get { return ((new System.Text.StringBuilder()).Append('*', strPw.Length)).ToString(); }
            set { strPw = value; }
        }

        [Description("Das ist ein Passwort-DropDown-Dialog."), Category("Custom"), Editor(typeof(DropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Password_03_DropDown
        {
            get { return strPw; }
            set { strPw = value; }
        }

        [Description("Das ist ein Passwort-PopUp-Dialog"), Category("Custom"), Editor(typeof(PopUpPwEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Password_04_PopUp
        {
            get { return strPw; }
            set { strPw = value; }
        }


        //[Browsable(false)] 
        [Description("Das ist ein Passwort-PopUp-Hiddden-Dialog"), Category("Extra"), Editor(typeof(PopUpPasswordEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Password Password_05_SpecialPwClass
        {
            get { return Pw; }
            set { Pw = value; }
        }

        [Browsable(false)]
        public string Password_06_ExternOnly
        {
            get { return strPw; }
            set { strPw = value; }
        }

        StringDictionary sd1 = new StringDictionary();

        public StringDictionary mysg
        {
            get { return sd1; }
            set { sd1 = value; }
        }

        StringCollection sc = new StringCollection();

        public StringCollection mysgs
        {
            get { return sc; }
            set { sc = value; }
        }


        private string _pass;
        private int _age;
        private DateTime _dateofbirth;
        private string _address;
        private string _email;
        private bool _frequentbuyer;
        private Image _f;
        private Color _backcolor;
        private Fruit _fruit;
        private SpellingOptions _so = new SpellingOptions();
        private string defaultFileName;
        private Point p;
        private ListViewItem z;
        public enum Fruit
        {
            Apple, Banana, Orange, Peach, Pear
        }
        string[] Str1 = { "AAA", "BBB", "CCC", "DDDD" };
        string[] Str2 = { "WW", "EEE" };
        string s1, s2;
        [EditorAttribute(typeof(SelEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string STR_1
        {
            get { SelEditor.strList = Str1; return s1; }
            set { s1 = value; }
        }
        [EditorAttribute(typeof(SelEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string STR_2
        {
            get { SelEditor.strList = Str2; return s2; }
            set { s2 = value; }
        }
        [TypeConverter(typeof(FileNameConverter)),
        CategoryAttribute("Document Settings")]
        public string DefaultFileName
        {
            get { return defaultFileName; }
            set { defaultFileName = value; }
        }
        [TypeConverter(typeof(PointConverter)),
        CategoryAttribute("Document Settings")]
        public Point äÞØå
        {
            get { return p; }
            set { p = value; }
        }
        [CategoryAttribute("Document Settings")]
        public ListViewItem Z
        {
            get { return z; }
            set { z = value; }
        }
        [CategoryAttribute("Document Settings")]
        [DisplayNameAttribute("Document Settings")]
        public PictureBox Y
        {
            get { return new PictureBox(); }
            set { }
        }
        [CategoryAttribute("Other Settings"), DescriptionAttribute("Font of the customer")]
        public SpellingOptions SO
        {
            get
            {
                return _so;
            }
            set
            {
                _so = value;
            }
        }
        [CategoryAttribute("Other Settings"), DescriptionAttribute("Font of the customer")]
        public Fruit MIVE
        {
            get
            {
                return _fruit;
            }
            set
            {
                _fruit = value;
            }
        }
        [CategoryAttribute("Other Settings"), DescriptionAttribute("Font of the customer")]
        public Image f
        {
            get
            {
                return _f;
            }
            set
            {
                _f = value;
            }
        }
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Name of the customer")]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Name of the customer")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Name of the customer")]
        public string password
        {
            get
            {
                return "***";
            }
            set
            {
                _pass = value;
            }
        }
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Address of the customer")]
        public string address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Name of the customer (Optional)")]
        public DateTime DateOfBirth
        {
            get
            {
                return _dateofbirth;
            }
            set
            {
                _dateofbirth = value;
            }
        }
        [Editor(typeof(GradeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Age of the customer")]
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }
        }
        [TypeConverter(typeof(YesNoTypeConverter)), CategoryAttribute("Market Settings"), DescriptionAttribute("If the customer has bought more than 10 items, this is set to true")]
        public bool FrequentBuyer
        {
            get
            {
                return _frequentbuyer;
            }
            set
            {
                _frequentbuyer = value;
            }
        }





        bool _cb;



        [Editor(typeof(CheckBoxInPropertyGridEditor), typeof(System.Drawing.Design.UITypeEditor))]

        public bool Cb
        {

            get { return _cb; }

            set { _cb = value; }

        }



        public class CheckBoxInPropertyGridEditor : UITypeEditor
        {

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {

                return true;

            }



            public override void PaintValue(PaintValueEventArgs e)
            {

                ControlPaint.DrawCheckBox(e.Graphics, e.Bounds, ((Customer)e.Context.Instance).Cb ? ButtonState.Checked : ButtonState.Normal);

            }



        }


        DashCap _d1;
        public DashCap d1
        {
            get { return _d1; }
            set { _d1 = value; }
        }
        LineCap _d2;
        public LineCap d2
        {
            get { return _d2; }
            set { _d2 = value; }
        }
        DashStyle _d3;
        public DashStyle d3
        {
            get { return _d3; }
            set { _d3 = value; }
        }
        ContentAlignment _d4;
        public ContentAlignment d4
        {
            get { return _d4; }
            set { _d4 = value; }
        }





        [CategoryAttribute("Market Settings"), DescriptionAttribute("Most current email of the customer")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        [CategoryAttribute("LogPlot"), DescriptionAttribute("Colors & Tips")]
        public Color BackColor
        {
            get
            {
                return _backcolor;
            }
            set
            {
                _backcolor = value;
            }
        }

        internal class MyDesigner : ComponentDesigner
        {
            DesignerVerbCollection m_Verbs;
            // DesignerVerbCollection is overridden from ComponentDesigner
            public override DesignerVerbCollection Verbs
            {
                get
                {
                    if (m_Verbs == null)
                    {
                        // Create and initialize the collection of verbs
                        m_Verbs = new DesignerVerbCollection();

                        m_Verbs.Add(new DesignerVerb("First Designer Verb", new EventHandler(OnFirstItemSelected)));
                        m_Verbs.Add(new DesignerVerb("Second Designer Verb", new EventHandler(OnSecondItemSelected)));
                    }
                    return m_Verbs;
                }
            }
            private void OnFirstItemSelected(object sender, EventArgs args)
            {
                // Display a message
                System.Windows.Forms.MessageBox.Show("The first designer verb was invoked.");
            }
            private void OnSecondItemSelected(object sender, EventArgs args)
            {
                // Display a message
                System.Windows.Forms.MessageBox.Show("The second designer verb was invoked.");
            }
        }

        public enum HE_SourceType { sandstone, claystone, siltstone, mixed, conglomerate }
        private string _Rule;
        private HE_SourceType _SourceType;
        private int _Contrast, _lp;
        public class RuleConverter : StringConverter
        {

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                //true means show a combobox
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                //true will limit to list. false will show the list, but allow free-form entry
                return true;
            }

            public override
                System.ComponentModel.TypeConverter.StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                string[] s = new string[4] { "Óä", "ÂÌÑ", "Âå˜", "äã˜" };
                return new StandardValuesCollection(s);
            }

        }
        [TypeConverter(typeof(RuleConverter))]
        public string Rule
        {
            //When first loaded set property with the first item in the rule list.
            get
            {
                string S = "";
                if (_Rule != null)
                {
                    S = _Rule;
                }
                else
                {
                    /*                if (HE_GlobalVars._ListofRules.Length > 0)
                                    {
                                        //Sort the list before displaying it
                                        Array.Sort(HE_GlobalVars._ListofRules);

                                        S = HE_GlobalVars._ListofRules[0];
                                    }
                     */
                }

                return S;
            }
            set { _Rule = value; }

        }

        public class SourceTypePropertyGridEditor : UITypeEditor
        {
            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                //Set to true to implement the PaintValue method
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                //Load SampleResources file
                string m = this.GetType().Module.Name;
                m = m.Substring(0, m.Length - 4);
                ResourceManager resourceManager = new ResourceManager(m + ".Resource1", Assembly.GetExecutingAssembly());


                int i = (int)e.Value;
                string _SourceName = "";
                switch (i)
                {
                    case ((int)HE_SourceType.sandstone): _SourceName = "sandstone"; break;
                    case ((int)HE_SourceType.claystone): _SourceName = "claystone"; break;
                    case ((int)HE_SourceType.siltstone): _SourceName = "siltstone"; break;
                    case ((int)HE_SourceType.mixed): _SourceName = "mixed"; break;
                    case ((int)HE_SourceType.conglomerate): _SourceName = "conglomerate"; break;
                }

                //Draw the corresponding image
                Bitmap newImage = (Bitmap)resourceManager.GetObject(_SourceName);
                Rectangle destRect = e.Bounds;
                destRect.Width *= 4;

                newImage.MakeTransparent();
                e.Graphics.DrawImage(newImage, destRect);

            }
        }


        [Editor(typeof(SourceTypePropertyGridEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public HE_SourceType SourceType
        {
            get { return _SourceType; }
            set { _SourceType = value; }
        }


        public class ContrastEditor : UITypeEditor
        {
            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                //Set to true to implement the PaintValue method
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                //Load SampleResources file              
                string m = this.GetType().Module.Name;
                m = m.Substring(0, m.Length - 4);
                ResourceManager resourceManager = new ResourceManager(m + ".Resource1", Assembly.GetExecutingAssembly());

                int i = 1 + (int)e.Value;
                if (i <= 17)
                {
                    string _SourceName = "p" + i.ToString();
                    //Draw the corresponding image
                    Bitmap newImage = (Bitmap)resourceManager.GetObject(_SourceName);
                    Rectangle destRect = e.Bounds;
                    destRect.Width *= 4;

                    newImage.MakeTransparent();
                    e.Graphics.DrawImage(newImage, destRect);
                }
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
                    frmContrast _frmContrast = new frmContrast();

                    //				_frmContrast.trackBar1.Value = (int) value;
                    //				_frmContrast.BarValue = _frmContrast.trackBar1.Value;

                    _frmContrast.BarValue = (int)value;
                    _frmContrast._wfes = wfes;
                    _frmContrast.rearrange(1);

                    //				_frmContrast.BarValue = _frmContrast.trackBar1.Value;
                    //_frmContrast.imageListBox1.SelectedIndex =1+ (int)value;
                    wfes.DropDownControl(_frmContrast.imageListBox1);
                    value = _frmContrast.BarValue;

                }
                return value;
            }
        }
        public class LineEditor : UITypeEditor
        {
            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                //Set to true to implement the PaintValue method
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                //Load SampleResources file              
                string m = this.GetType().Module.Name;
                m = m.Substring(0, m.Length - 4);
                ResourceManager resourceManager = new ResourceManager(m + ".Resource1", Assembly.GetExecutingAssembly());

                int i = 1 + (int)e.Value;
                if (i <= 6)
                {
                    string _SourceName = "l" + i.ToString();
                    //Draw the corresponding image
                    Bitmap newImage = (Bitmap)resourceManager.GetObject(_SourceName);
                    Rectangle destRect = e.Bounds;
                    destRect.Width *= 4;

                    newImage.MakeTransparent();
                    e.Graphics.DrawImage(newImage, destRect);
                }
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
                    frmContrast _frmContrast = new frmContrast();

                    //				_frmContrast.trackBar1.Value = (int) value;
                    //				_frmContrast.BarValue = _frmContrast.trackBar1.Value;

                    _frmContrast.BarValue = (int)value;
                    _frmContrast._wfes = wfes;
                    _frmContrast.rearrange(2);

                    //				_frmContrast.BarValue = _frmContrast.trackBar1.Value;
                    //_frmContrast.imageListBox1.SelectedIndex =1+ (int)value;
                    wfes.DropDownControl(_frmContrast.imageListBox1);
                    value = _frmContrast.BarValue;

                }
                return value;
            }
        }

        [Editor(typeof(ContrastEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public int FillPattern
        {
            get { return _Contrast; }
            set { _Contrast = value; }
        }
        [Editor(typeof(LineEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public int LinePattern
        {
            get { return _lp; }
            set { _lp = value; }
        }

        VerbHoster.VerbList vl = new VerbHoster.VerbList();
        public override VerbHoster.VerbList GetVerbs()
        {
            return vl;
        }
        public Customer()
            : base()
        {
            sd1.Add("Name", "Mustermann");
            sd1.Add("Vorname", "Peter");
            sd1.Add("Strasse", "Beispielgasse 2");
            sd1.Add("Ort", "Musterbstadt");
            VerbHoster.Verb v = new VerbHoster.Verb("open excell");
            v.CallBack += openexcell;
            vl.Add(v);
        }
        private void openexcell(VerbHoster.Verb v)
        {
            MessageBox.Show("EXCELL OPENED");
        }
    }
    internal class MyPropertyGrid : System.Windows.Forms.PropertyGrid
    {

        private System.ComponentModel.Container components = null;
        public MyPropertyGrid()
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


        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion


        public void ShowEvents(bool show)
        {
            ShowEventsButton(show);
        }
        public bool DrawFlat
        {
            get { return DrawFlatToolbar; }
            set { DrawFlatToolbar = value; }
        }



    }
    [Designer(typeof(ColorLabelDesigner))]
    public class ColorLabel : System.Windows.Forms.Label
    {
        private bool colorLockedValue = false;

        public bool ColorLocked
        {
            get
            {
                return colorLockedValue;
            }
            set
            {
                colorLockedValue = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (ColorLocked)
                    return;
                else
                    base.BackColor = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (ColorLocked)
                    return;
                else
                    base.ForeColor = value;
            }
        }
    }
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class ColorLabelDesigner :
             System.Windows.Forms.Design.ControlDesigner
    {
        private DesignerActionListCollection actionLists;

        // Use pull model to populate smart tag menu.
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (null == actionLists)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(
                        new ColorLabelActionList(this.Component));
                }
                return actionLists;
            }
        }
    }
    public class ColorLabelActionList :
              System.ComponentModel.Design.DesignerActionList
    {
        private ColorLabel colLabel;

        private DesignerActionUIService designerActionUISvc = null;

        //The constructor associates the control 
        //with the smart tag list.
        public ColorLabelActionList(IComponent component)
            : base(component)
        {
            this.colLabel = component as ColorLabel;

            // Cache a reference to DesignerActionUIService, so the
            // DesigneractionList can be refreshed.
            this.designerActionUISvc =
                GetService(typeof(DesignerActionUIService))
                as DesignerActionUIService;
        }

        // Helper method to retrieve control properties. Use of 
        // GetProperties enables undo and menu updates to work properly.
        private PropertyDescriptor GetPropertyByName(String propName)
        {
            PropertyDescriptor prop;
            prop = TypeDescriptor.GetProperties(colLabel)[propName];
            if (null == prop)
                throw new ArgumentException(
                     "Matching ColorLabel property not found!",
                      propName);
            else
                return prop;
        }

        // Properties that are targets of DesignerActionPropertyItem entries.
        public Color BackColor
        {
            get
            {
                return colLabel.BackColor;
            }
            set
            {
                GetPropertyByName("BackColor").SetValue(colLabel, value);
            }
        }

        public Color ForeColor
        {
            get
            {
                return colLabel.ForeColor;
            }
            set
            {
                GetPropertyByName("ForeColor").SetValue(colLabel, value);
            }
        }


        // Boolean properties are automatically displayed with binary 
        // UI (such as a checkbox).
        public bool LockColors
        {
            get
            {
                return colLabel.ColorLocked;
            }
            set
            {
                GetPropertyByName("ColorLocked").SetValue(colLabel, value);

                // Refresh the list.
                this.designerActionUISvc.Refresh(this.Component);
            }
        }

        public String Text
        {
            get
            {
                return colLabel.Text;
            }
            set
            {
                GetPropertyByName("Text").SetValue(colLabel, value);
            }
        }

        // Method that is target of a DesignerActionMethodItem
        public void InvertColors()
        {
            Color currentBackColor = colLabel.BackColor;
            BackColor = Color.FromArgb(
                255 - currentBackColor.R,
                255 - currentBackColor.G,
                255 - currentBackColor.B);

            Color currentForeColor = colLabel.ForeColor;
            ForeColor = Color.FromArgb(
                255 - currentForeColor.R,
                255 - currentForeColor.G,
                255 - currentForeColor.B);
        }

        // Implementation of this abstract method creates smart tag  
        // items, associates their targets, and collects into list.
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            //Define static section header entries.
            items.Add(new DesignerActionHeaderItem("Appearance"));
            items.Add(new DesignerActionHeaderItem("Information"));

            //Boolean property for locking color selections.
            items.Add(new DesignerActionPropertyItem("LockColors",
                             "Lock Colors", "Appearance",
                             "Locks the color properties."));
            if (!LockColors)
            {
                items.Add(new DesignerActionPropertyItem("BackColor",
                                 "Back Color", "Appearance",
                                 "Selects the background color."));
                items.Add(new DesignerActionPropertyItem("ForeColor",
                                 "Fore Color", "Appearance",
                                 "Selects the foreground color."));

                //This next method item is also added to the context menu 
                // (as a designer verb).

                items.Add(new DesignerActionMethodItem(this,
                                 "InvertColors", "Invert 4 Colors",
                                 "Appearance",
                                 "Inverts the fore and background colors.",
                                  true));
                items.Add(new DesignerActionMethodItem(this,
                                 "InvertColors", "Invert Colors 3",
                                 "Appearance",
                                 "Inverts the fore and background colors.",
                                  true));
            }
            items.Add(new DesignerActionPropertyItem("Text",
                             "Text String", "Appearance",
                             "Sets the display text."));

            //Create entries for static Information section.
            StringBuilder location = new StringBuilder("Location: ");
            location.Append(colLabel.Location);
            StringBuilder size = new StringBuilder("Size: ");
            size.Append(colLabel.Size);
            items.Add(new DesignerActionTextItem(location.ToString(),
                             "Information"));
            items.Add(new DesignerActionTextItem(size.ToString(),
                             "Information"));

            return items;
        }
    }
    public class Column { }
    public class LithologyPatternColumn : Column
    {
        string ProfileCurve = "";
        double min = 0, max = 0;
        bool low2high = true, left2right = true;
    }
    public class LithologyDescriptionColumn : Column
    {
        ContentAlignment Alignment = ContentAlignment.MiddleCenter;
        Padding Margin = new Padding(1);
        Font Font = new Font(FontFamily.GenericSansSerif, 1);
        Color Color = Color.Black;
        int Decimals = 0;
        bool IntervalTop = false;
        bool IntervalBase = false;
        bool PositiveDepth = true;
        bool Paranthesis = true;
        bool NewLine = false;
        bool Keyword = false;
        bool Colon = false;
        bool Description = false;
        bool OffsetDescription = false;
        bool MoveUp = false;
        bool IntervalBorder = true;
    }
    public enum TextOrientation { StraightUp, Rightward, StraightDown, Leftward }
    //public enum TickStyle { Left, Both, Right, None }
    public class TickMark
    {
        int Frequency = 10;
        TickStyle Style = TickStyle.Both;
        float Size = 0.1f;

    }
    public class ScaleBarColumn : Column
    {
        bool Style = false;
        bool Meter = false;
        bool Feet = false;
        TextOrientation Orientation = TextOrientation.Rightward;
        Font Font = new Font(FontFamily.GenericSansSerif, 1);
        Color Color = Color.Black;
        int Decimals = 0;
        int Angle = 0;
        bool PrintAdjust = true;
        TickMark Major = new TickMark(), Minor = new TickMark();

    }
    public enum CurveStyle { Line, Symbol, LineSymbol, Blocked, Filled }
    public enum WrapStyle { None, Truncate, Wrap, Wrap10x }
    public class CurveColumn : Column
    {
        bool DisplayTrueDepth = false;
        TextOrientation Orientation = TextOrientation.Rightward;
        Object Downholedata;
        bool Scaling = false; //Linear or Logarithmic
        double min = 0, max = 0;
        CurveStyle CurveStyle = CurveStyle.Line;
        Color FillColor = Color.Linen;
        Bitmap Symbol = symbol.s1;
        Color SymbolColor = Color.Black;
        int SymbolSize = 1;
        //go page 80 in manual
        WrapStyle wrap = WrapStyle.None;
        GridStyle gs = new GridStyle();
    }
    public class GridStyle { }

    public class CrossPlot : CurveColumn
    {
        CurveColumn SecondCurve = new CurveColumn();


    }

    // we need to talk about truedepthcurves
    public class SampleColumn : Column
    {
    }
    public class YesNoTypeConverter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {

            if (sourceType == typeof(string))

                return true;

            return base.CanConvertFrom(context, sourceType);

        }



        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {

            if (destinationType == typeof(string))

                return true;

            return base.CanConvertTo(context, destinationType);

        }



        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {

            if (value.GetType() == typeof(string))
            {

                if (((string)value).ToLower() == "yes")

                    return true;

                if (((string)value).ToLower() == "no")

                    return false;

                throw new Exception("Values must be \"Yes\" or \"No\"");

            }

            return base.ConvertFrom(context, culture, value);

        }



        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {

            if (destinationType == typeof(string))
            {

                return (((bool)value) ? "Yes" : "No");

            }

            return base.ConvertTo(context, culture, value, destinationType);

        }



        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {

            return true;

        }



        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {

            bool[] bools = new bool[] { true, false };

            System.ComponentModel.TypeConverter.StandardValuesCollection svc = new System.ComponentModel.TypeConverter.StandardValuesCollection(bools);

            return svc;

        }


        public class FtpUser
        {

            private string _username;
            private string _homeDirectory;
            private bool _restrictHome;
            private string _password;

            /// <summary>
            /// Default Constructor is need for the PropertyGrid
            /// </summary>
            public FtpUser()
            {
                _username = "NewUser";
                _homeDirectory = null;
                _password = null;
                _restrictHome = true;
            }

            public override string ToString()
            {
                return _username;
            }


            public string Username
            {
                get { return _username; }
                set { _username = value; }
            }

            public string Password
            {
                get { return (_password != null) ? ((new System.Text.StringBuilder()).Append('*', _password.Length)).ToString() : "*"; }
                set { _password = value; }
            }

            [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
            public string HomeDirectory
            {
                get { return _homeDirectory; }
                set { _homeDirectory = value; }
            }

            public bool RestrictHomeDirectory
            {
                get { return _restrictHome; }
                set { _restrictHome = value; }
            }
        }

        /// <summary>
        /// Helper Object for the Property Grid Configurator.
        /// </summary>
        public class FtpUserCollection : System.Collections.CollectionBase
        {
            public FtpUserCollection()
                : base()
            {

            }

            public void Add(FtpUser str)
            {
                base.List.Add(str);
            }

            public bool Contains(FtpUser str)
            {
                return base.List.Contains(str);
            }

            public void Remove(FtpUser str)
            {
                base.List.Remove(str);
            }

            public FtpUser this[int index]
            {
                get { return (FtpUser)base.List[index]; }
                set { base.List[index] = value; }
            }

            public override string ToString()
            {
                return base.List.Count + " User";
            }
        }

        public class FtpSettings
        {

            private FtpUserCollection _users;

            public FtpSettings()
            {
                _users = new FtpUserCollection();
            }

            public FtpUserCollection Users
            {
                get { return _users; }
                set { _users = value; }
            }
        }
    }
}