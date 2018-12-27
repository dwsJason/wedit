namespace wedit
{
    partial class wedit
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.animListView = new BrightIdeasSoftware.ObjectListView();
            this.objectFramesView = new BrightIdeasSoftware.ObjectListView();
            this.ImageColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.NameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.openSpriteFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.comboBoxFrameNo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RedButton = new System.Windows.Forms.Button();
            this.GreenButton = new System.Windows.Forms.Button();
            this.BlueButton = new System.Windows.Forms.Button();
            this.WhiteButton = new System.Windows.Forms.Button();
            this.PurpleButton = new System.Windows.Forms.Button();
            this.animNoColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.animNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectFramesView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(710, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSpriteFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadSpriteFileToolStripMenuItem
            // 
            this.loadSpriteFileToolStripMenuItem.Name = "loadSpriteFileToolStripMenuItem";
            this.loadSpriteFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadSpriteFileToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.loadSpriteFileToolStripMenuItem.Text = "Load Sprite File";
            this.loadSpriteFileToolStripMenuItem.Click += new System.EventHandler(this.loadSpriteFileToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 401);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(710, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(710, 377);
            this.splitContainer1.SplitterDistance = 235;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.animListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objectFramesView);
            this.splitContainer2.Size = new System.Drawing.Size(235, 377);
            this.splitContainer2.SplitterDistance = 159;
            this.splitContainer2.TabIndex = 0;
            // 
            // animListView
            // 
            this.animListView.AllColumns.Add(this.animNoColumn);
            this.animListView.AllColumns.Add(this.animNameColumn);
            this.animListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.animListView.AutoArrange = false;
            this.animListView.CellEditUseWholeCell = false;
            this.animListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.animNoColumn,
            this.animNameColumn});
            this.animListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.animListView.GridLines = true;
            this.animListView.Location = new System.Drawing.Point(3, 3);
            this.animListView.Name = "animListView";
            this.animListView.ShowGroups = false;
            this.animListView.Size = new System.Drawing.Size(229, 153);
            this.animListView.TabIndex = 0;
            this.animListView.UseCompatibleStateImageBehavior = false;
            this.animListView.View = System.Windows.Forms.View.Details;
            // 
            // objectFramesView
            // 
            this.objectFramesView.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.objectFramesView.AllColumns.Add(this.ImageColumn);
            this.objectFramesView.AllColumns.Add(this.NameColumn);
            this.objectFramesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectFramesView.CellEditUseWholeCell = false;
            this.objectFramesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ImageColumn,
            this.NameColumn});
            this.objectFramesView.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectFramesView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.objectFramesView.Location = new System.Drawing.Point(3, 3);
            this.objectFramesView.MultiSelect = false;
            this.objectFramesView.Name = "objectFramesView";
            this.objectFramesView.ShowGroups = false;
            this.objectFramesView.ShowImagesOnSubItems = true;
            this.objectFramesView.Size = new System.Drawing.Size(229, 208);
            this.objectFramesView.TabIndex = 0;
            this.objectFramesView.TileSize = new System.Drawing.Size(64, 64);
            this.objectFramesView.UseCompatibleStateImageBehavior = false;
            this.objectFramesView.View = System.Windows.Forms.View.Tile;
            // 
            // ImageColumn
            // 
            this.ImageColumn.Text = "";
            // 
            // NameColumn
            // 
            this.NameColumn.AspectName = "m_name";
            this.NameColumn.Text = "";
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(465, 371);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // openSpriteFileDialog
            // 
            this.openSpriteFileDialog.Filter = "Sprite|*.sp";
            this.openSpriteFileDialog.RestoreDirectory = true;
            this.openSpriteFileDialog.Title = "Open Sprite File";
            this.openSpriteFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openSpriteFileDialog_FileOk);
            // 
            // comboBoxFrameNo
            // 
            this.comboBoxFrameNo.FormattingEnabled = true;
            this.comboBoxFrameNo.Location = new System.Drawing.Point(242, 0);
            this.comboBoxFrameNo.Name = "comboBoxFrameNo";
            this.comboBoxFrameNo.Size = new System.Drawing.Size(79, 21);
            this.comboBoxFrameNo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(196, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Frame";
            // 
            // RedButton
            // 
            this.RedButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.RedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RedButton.ForeColor = System.Drawing.Color.Snow;
            this.RedButton.Location = new System.Drawing.Point(328, 1);
            this.RedButton.Name = "RedButton";
            this.RedButton.Size = new System.Drawing.Size(19, 23);
            this.RedButton.TabIndex = 5;
            this.RedButton.Text = "#";
            this.RedButton.UseVisualStyleBackColor = false;
            this.RedButton.Click += new System.EventHandler(this.RedButton_Click);
            // 
            // GreenButton
            // 
            this.GreenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(128)))), ((int)(((byte)(34)))));
            this.GreenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GreenButton.ForeColor = System.Drawing.Color.Snow;
            this.GreenButton.Location = new System.Drawing.Point(348, 1);
            this.GreenButton.Name = "GreenButton";
            this.GreenButton.Size = new System.Drawing.Size(19, 23);
            this.GreenButton.TabIndex = 6;
            this.GreenButton.Text = "#";
            this.GreenButton.UseVisualStyleBackColor = false;
            this.GreenButton.Click += new System.EventHandler(this.GreenButton_Click);
            // 
            // BlueButton
            // 
            this.BlueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(110)))), ((int)(((byte)(169)))));
            this.BlueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BlueButton.ForeColor = System.Drawing.Color.Snow;
            this.BlueButton.Location = new System.Drawing.Point(368, 1);
            this.BlueButton.Name = "BlueButton";
            this.BlueButton.Size = new System.Drawing.Size(19, 23);
            this.BlueButton.TabIndex = 7;
            this.BlueButton.Text = "#";
            this.BlueButton.UseVisualStyleBackColor = false;
            this.BlueButton.Click += new System.EventHandler(this.BlueButton_Click);
            // 
            // WhiteButton
            // 
            this.WhiteButton.BackColor = System.Drawing.Color.Snow;
            this.WhiteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WhiteButton.Location = new System.Drawing.Point(408, 1);
            this.WhiteButton.Name = "WhiteButton";
            this.WhiteButton.Size = new System.Drawing.Size(19, 23);
            this.WhiteButton.TabIndex = 8;
            this.WhiteButton.Text = "#";
            this.WhiteButton.UseVisualStyleBackColor = false;
            this.WhiteButton.Click += new System.EventHandler(this.WhiteButton_Click);
            // 
            // PurpleButton
            // 
            this.PurpleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.PurpleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PurpleButton.ForeColor = System.Drawing.Color.Snow;
            this.PurpleButton.Location = new System.Drawing.Point(388, 1);
            this.PurpleButton.Name = "PurpleButton";
            this.PurpleButton.Size = new System.Drawing.Size(19, 23);
            this.PurpleButton.TabIndex = 9;
            this.PurpleButton.Text = "#";
            this.PurpleButton.UseVisualStyleBackColor = false;
            this.PurpleButton.Click += new System.EventHandler(this.PurpleButton_Click);
            // 
            // animNoColumn
            // 
            this.animNoColumn.AspectName = "m_animNo";
            this.animNoColumn.Text = "#";
            this.animNoColumn.Width = 44;
            // 
            // animNameColumn
            // 
            this.animNameColumn.AspectName = "m_name";
            this.animNameColumn.Text = "Anim Name";
            this.animNameColumn.Width = 178;
            // 
            // wedit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 423);
            this.Controls.Add(this.PurpleButton);
            this.Controls.Add(this.WhiteButton);
            this.Controls.Add(this.BlueButton);
            this.Controls.Add(this.GreenButton);
            this.Controls.Add(this.RedButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxFrameNo);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "wedit";
            this.Text = "World Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.animListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectFramesView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSpriteFileToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxFrameNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openSpriteFileDialog;
        private System.Windows.Forms.Button RedButton;
        private System.Windows.Forms.Button GreenButton;
        private System.Windows.Forms.Button BlueButton;
        private System.Windows.Forms.Button WhiteButton;
        private System.Windows.Forms.Button PurpleButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BrightIdeasSoftware.ObjectListView animListView;
        private BrightIdeasSoftware.ObjectListView objectFramesView;
        private BrightIdeasSoftware.OLVColumn ImageColumn;
        private BrightIdeasSoftware.OLVColumn NameColumn;
        private BrightIdeasSoftware.OLVColumn animNoColumn;
        private BrightIdeasSoftware.OLVColumn animNameColumn;
    }
}

