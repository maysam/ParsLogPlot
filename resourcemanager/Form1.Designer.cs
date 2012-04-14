namespace resourcemanager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.resourceTableAdapter = new resourcemanager.ParsLogPlotDataSetTableAdapters.resourceTableAdapter();
            this.parsLogPlotDataSet = new resourcemanager.ParsLogPlotDataSet();
            this.resourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.editbutton = new System.Windows.Forms.Button();
            this.okbutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            this.restorebutton = new System.Windows.Forms.Button();
            this.newbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.clonebutton = new System.Windows.Forms.Button();
            this.deletebutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.parsLogPlotDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceTableAdapter
            // 
            this.resourceTableAdapter.ClearBeforeFill = true;
            // 
            // parsLogPlotDataSet
            // 
            this.parsLogPlotDataSet.DataSetName = "ParsLogPlotDataSet";
            this.parsLogPlotDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // resourceBindingSource
            // 
            this.resourceBindingSource.DataMember = "resource";
            this.resourceBindingSource.DataSource = this.parsLogPlotDataSet;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 479);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(412, 42);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Value = 3;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(412, 469);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Location = new System.Drawing.Point(430, 65);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(250, 250);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            // 
            // editbutton
            // 
            this.editbutton.Location = new System.Drawing.Point(479, 427);
            this.editbutton.Name = "editbutton";
            this.editbutton.Size = new System.Drawing.Size(75, 23);
            this.editbutton.TabIndex = 4;
            this.editbutton.Text = "Edit";
            this.editbutton.UseVisualStyleBackColor = true;
            this.editbutton.Click += new System.EventHandler(this.editbutton_Click);
            // 
            // okbutton
            // 
            this.okbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbutton.Location = new System.Drawing.Point(437, 479);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(75, 23);
            this.okbutton.TabIndex = 5;
            this.okbutton.Text = "Ok";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton.Location = new System.Drawing.Point(599, 479);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.cancelbutton.TabIndex = 6;
            this.cancelbutton.Text = "Cancel";
            this.cancelbutton.UseVisualStyleBackColor = true;
            // 
            // restorebutton
            // 
            this.restorebutton.Location = new System.Drawing.Point(518, 479);
            this.restorebutton.Name = "restorebutton";
            this.restorebutton.Size = new System.Drawing.Size(75, 23);
            this.restorebutton.TabIndex = 7;
            this.restorebutton.Text = "Restore";
            this.restorebutton.UseVisualStyleBackColor = true;
            this.restorebutton.Click += new System.EventHandler(this.restorebutton_Click);
            // 
            // newbutton
            // 
            this.newbutton.Location = new System.Drawing.Point(479, 398);
            this.newbutton.Name = "newbutton";
            this.newbutton.Size = new System.Drawing.Size(75, 23);
            this.newbutton.TabIndex = 8;
            this.newbutton.Text = "New";
            this.newbutton.UseVisualStyleBackColor = true;
            this.newbutton.Click += new System.EventHandler(this.newbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(430, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Name :";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(477, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(203, 20);
            this.textBox1.TabIndex = 10;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(511, 370);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(43, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Tile";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(430, 321);
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(250, 42);
            this.trackBar2.TabIndex = 13;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar2.Value = 1;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(433, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 28);
            this.label2.TabIndex = 14;
            this.label2.Text = "Resource Manager";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clonebutton
            // 
            this.clonebutton.Location = new System.Drawing.Point(560, 398);
            this.clonebutton.Name = "clonebutton";
            this.clonebutton.Size = new System.Drawing.Size(75, 23);
            this.clonebutton.TabIndex = 15;
            this.clonebutton.Text = "Clone";
            this.clonebutton.UseVisualStyleBackColor = true;
            this.clonebutton.Click += new System.EventHandler(this.clonebutton_Click);
            // 
            // deletebutton
            // 
            this.deletebutton.Location = new System.Drawing.Point(560, 428);
            this.deletebutton.Name = "deletebutton";
            this.deletebutton.Size = new System.Drawing.Size(75, 23);
            this.deletebutton.TabIndex = 16;
            this.deletebutton.Text = "Delete";
            this.deletebutton.UseVisualStyleBackColor = true;
            this.deletebutton.Click += new System.EventHandler(this.deletebutton_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.okbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelbutton;
            this.ClientSize = new System.Drawing.Size(692, 519);
            this.Controls.Add(this.deletebutton);
            this.Controls.Add(this.clonebutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newbutton);
            this.Controls.Add(this.restorebutton);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.okbutton);
            this.Controls.Add(this.editbutton);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.trackBar1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resource Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.parsLogPlotDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ParsLogPlotDataSet parsLogPlotDataSet;
        private System.Windows.Forms.BindingSource resourceBindingSource;
        private resourcemanager.ParsLogPlotDataSetTableAdapters.resourceTableAdapter resourceTableAdapter;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button editbutton;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.Button restorebutton;
        private System.Windows.Forms.Button newbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clonebutton;
        private System.Windows.Forms.Button deletebutton;
    }
}

