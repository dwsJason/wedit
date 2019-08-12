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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wedit));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpriteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpriteFileAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mode = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mouseXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.animEditorBox = new System.Windows.Forms.GroupBox();
            this.buttonPrevAnim = new System.Windows.Forms.Button();
            this.buttonNextAnim = new System.Windows.Forms.Button();
            this.cmdListView = new BrightIdeasSoftware.ObjectListView();
            this.animLineNoColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.animCmdColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.animArgColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.animImageColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.labelLineNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelAnimNo = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.animListView = new BrightIdeasSoftware.ObjectListView();
            this.animNoColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.animNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.objectFramesView = new BrightIdeasSoftware.ObjectListView();
            this.ImageColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.NameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pinCheckBox = new System.Windows.Forms.CheckBox();
            this.openSpriteFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.RedButton = new System.Windows.Forms.Button();
            this.GreenButton = new System.Windows.Forms.Button();
            this.BlueButton = new System.Windows.Forms.Button();
            this.WhiteButton = new System.Windows.Forms.Button();
            this.PurpleButton = new System.Windows.Forms.Button();
            this.animButton = new System.Windows.Forms.Button();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.zoomInButton = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.handCheckBox = new System.Windows.Forms.CheckBox();
            this.selectCheckBox = new System.Windows.Forms.CheckBox();
            this.AddAnimButton = new System.Windows.Forms.Button();
            this.anchorCheckBox = new System.Windows.Forms.CheckBox();
            this.openPaletteFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveSpriteFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.centerButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.animEditorBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmdListView)).BeginInit();
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
            this.newToolStripMenuItem,
            this.loadSpriteFileToolStripMenuItem,
            this.saveSpriteFileAsToolStripMenuItem,
            this.importFramesToolStripMenuItem,
            this.importPaletteToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadSpriteFileToolStripMenuItem
            // 
            this.loadSpriteFileToolStripMenuItem.Name = "loadSpriteFileToolStripMenuItem";
            this.loadSpriteFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadSpriteFileToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.loadSpriteFileToolStripMenuItem.Text = "Load Sprite File";
            this.loadSpriteFileToolStripMenuItem.Click += new System.EventHandler(this.loadSpriteFileToolStripMenuItem_Click);
            // 
            // saveSpriteFileAsToolStripMenuItem
            // 
            this.saveSpriteFileAsToolStripMenuItem.Name = "saveSpriteFileAsToolStripMenuItem";
            this.saveSpriteFileAsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveSpriteFileAsToolStripMenuItem.Text = "Save Sprite File As...";
            this.saveSpriteFileAsToolStripMenuItem.Click += new System.EventHandler(this.saveSpriteFileAsToolStripMenuItem_Click);
            // 
            // importFramesToolStripMenuItem
            // 
            this.importFramesToolStripMenuItem.Name = "importFramesToolStripMenuItem";
            this.importFramesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.importFramesToolStripMenuItem.Text = "Import Frames";
            this.importFramesToolStripMenuItem.Click += new System.EventHandler(this.importFramesToolStripMenuItem_Click);
            // 
            // importPaletteToolStripMenuItem
            // 
            this.importPaletteToolStripMenuItem.Name = "importPaletteToolStripMenuItem";
            this.importPaletteToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.importPaletteToolStripMenuItem.Text = "Import Palette";
            this.importPaletteToolStripMenuItem.Click += new System.EventHandler(this.importPaletteToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.mode,
            this.toolStripStatusLabel2,
            this.mouseXY});
            this.statusStrip1.Location = new System.Drawing.Point(0, 399);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(710, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(41, 19);
            this.toolStripStatusLabel1.Text = "Mode:";
            // 
            // mode
            // 
            this.mode.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(38, 19);
            this.mode.Text = "VIEW";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(46, 19);
            this.toolStripStatusLabel2.Text = "Mouse:";
            // 
            // mouseXY
            // 
            this.mouseXY.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mouseXY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.mouseXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mouseXY.Name = "mouseXY";
            this.mouseXY.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mouseXY.Size = new System.Drawing.Size(29, 19);
            this.mouseXY.Text = "0, 0";
            this.mouseXY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.splitContainer1.Size = new System.Drawing.Size(710, 375);
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
            this.splitContainer2.Panel1.Controls.Add(this.animEditorBox);
            this.splitContainer2.Panel1.Controls.Add(this.animListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objectFramesView);
            this.splitContainer2.Size = new System.Drawing.Size(235, 375);
            this.splitContainer2.SplitterDistance = 187;
            this.splitContainer2.TabIndex = 0;
            // 
            // animEditorBox
            // 
            this.animEditorBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.animEditorBox.Controls.Add(this.buttonPrevAnim);
            this.animEditorBox.Controls.Add(this.buttonNextAnim);
            this.animEditorBox.Controls.Add(this.cmdListView);
            this.animEditorBox.Controls.Add(this.labelLineNo);
            this.animEditorBox.Controls.Add(this.label2);
            this.animEditorBox.Controls.Add(this.labelAnimNo);
            this.animEditorBox.Controls.Add(this.stopButton);
            this.animEditorBox.Controls.Add(this.playButton);
            this.animEditorBox.Controls.Add(this.loopCheckBox);
            this.animEditorBox.Location = new System.Drawing.Point(3, 3);
            this.animEditorBox.Name = "animEditorBox";
            this.animEditorBox.Size = new System.Drawing.Size(229, 181);
            this.animEditorBox.TabIndex = 1;
            this.animEditorBox.TabStop = false;
            this.animEditorBox.Text = "Animation Editor";
            // 
            // buttonPrevAnim
            // 
            this.buttonPrevAnim.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.buttonPrevAnim.Location = new System.Drawing.Point(0, 19);
            this.buttonPrevAnim.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPrevAnim.Name = "buttonPrevAnim";
            this.buttonPrevAnim.Size = new System.Drawing.Size(20, 23);
            this.buttonPrevAnim.TabIndex = 8;
            this.buttonPrevAnim.Text = "<";
            this.buttonPrevAnim.UseVisualStyleBackColor = true;
            this.buttonPrevAnim.Click += new System.EventHandler(this.buttonPrevAnim_Click);
            // 
            // buttonNextAnim
            // 
            this.buttonNextAnim.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.buttonNextAnim.Location = new System.Drawing.Point(47, 19);
            this.buttonNextAnim.Margin = new System.Windows.Forms.Padding(0);
            this.buttonNextAnim.Name = "buttonNextAnim";
            this.buttonNextAnim.Size = new System.Drawing.Size(20, 23);
            this.buttonNextAnim.TabIndex = 7;
            this.buttonNextAnim.Text = ">>>";
            this.buttonNextAnim.UseVisualStyleBackColor = true;
            this.buttonNextAnim.Click += new System.EventHandler(this.buttonNextAnim_Click);
            // 
            // cmdListView
            // 
            this.cmdListView.AllColumns.Add(this.animLineNoColumn);
            this.cmdListView.AllColumns.Add(this.animCmdColumn);
            this.cmdListView.AllColumns.Add(this.animArgColumn);
            this.cmdListView.AllColumns.Add(this.animImageColumn);
            this.cmdListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdListView.AutoArrange = false;
            this.cmdListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.cmdListView.CellEditEnterChangesRows = true;
            this.cmdListView.CellEditUseWholeCell = false;
            this.cmdListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.animLineNoColumn,
            this.animCmdColumn,
            this.animArgColumn,
            this.animImageColumn});
            this.cmdListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdListView.FullRowSelect = true;
            this.cmdListView.GridLines = true;
            this.cmdListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.cmdListView.HideSelection = false;
            this.cmdListView.Location = new System.Drawing.Point(0, 66);
            this.cmdListView.MultiSelect = false;
            this.cmdListView.Name = "cmdListView";
            this.cmdListView.ShowGroups = false;
            this.cmdListView.Size = new System.Drawing.Size(229, 119);
            this.cmdListView.TabIndex = 6;
            this.cmdListView.UseCompatibleStateImageBehavior = false;
            this.cmdListView.View = System.Windows.Forms.View.Details;
            // 
            // animLineNoColumn
            // 
            this.animLineNoColumn.AspectName = "m_lineNo";
            this.animLineNoColumn.IsEditable = false;
            this.animLineNoColumn.Text = "#";
            this.animLineNoColumn.Width = 26;
            // 
            // animCmdColumn
            // 
            this.animCmdColumn.AspectName = "m_cmd";
            this.animCmdColumn.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.animCmdColumn.CellEditUseWholeCell = true;
            this.animCmdColumn.Text = "Command";
            this.animCmdColumn.Width = 77;
            // 
            // animArgColumn
            // 
            this.animArgColumn.AspectName = "m_arg";
            this.animArgColumn.CellEditUseWholeCell = true;
            this.animArgColumn.Text = "Arg";
            // 
            // animImageColumn
            // 
            this.animImageColumn.Text = "Image";
            // 
            // labelLineNo
            // 
            this.labelLineNo.AutoSize = true;
            this.labelLineNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelLineNo.Location = new System.Drawing.Point(136, 50);
            this.labelLineNo.Name = "labelLineNo";
            this.labelLineNo.Size = new System.Drawing.Size(43, 13);
            this.labelLineNo.TabIndex = 5;
            this.labelLineNo.Text = "lineNo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "animName";
            // 
            // labelAnimNo
            // 
            this.labelAnimNo.AutoSize = true;
            this.labelAnimNo.Location = new System.Drawing.Point(22, 23);
            this.labelAnimNo.Margin = new System.Windows.Forms.Padding(0);
            this.labelAnimNo.Name = "labelAnimNo";
            this.labelAnimNo.Size = new System.Drawing.Size(25, 13);
            this.labelAnimNo.TabIndex = 3;
            this.labelAnimNo.Text = "000";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(144, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(40, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(186, 19);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(43, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.AutoSize = true;
            this.loopCheckBox.Location = new System.Drawing.Point(80, 23);
            this.loopCheckBox.Margin = new System.Windows.Forms.Padding(1);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(61, 17);
            this.loopCheckBox.TabIndex = 0;
            this.loopCheckBox.Text = "Repeat";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // animListView
            // 
            this.animListView.AllColumns.Add(this.animNoColumn);
            this.animListView.AllColumns.Add(this.animNameColumn);
            this.animListView.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.animListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.animListView.AutoArrange = false;
            this.animListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.animListView.CellEditUseWholeCell = false;
            this.animListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.animNoColumn,
            this.animNameColumn});
            this.animListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.animListView.FullRowSelect = true;
            this.animListView.GridLines = true;
            this.animListView.HideSelection = false;
            this.animListView.Location = new System.Drawing.Point(3, 3);
            this.animListView.Name = "animListView";
            this.animListView.ShowGroups = false;
            this.animListView.Size = new System.Drawing.Size(229, 181);
            this.animListView.TabIndex = 0;
            this.animListView.UseAlternatingBackColors = true;
            this.animListView.UseCompatibleStateImageBehavior = false;
            this.animListView.View = System.Windows.Forms.View.Details;
            // 
            // animNoColumn
            // 
            this.animNoColumn.AspectName = "m_animNo";
            this.animNoColumn.IsEditable = false;
            this.animNoColumn.Text = "#";
            this.animNoColumn.Width = 44;
            // 
            // animNameColumn
            // 
            this.animNameColumn.AspectName = "m_name";
            this.animNameColumn.CellEditUseWholeCell = true;
            this.animNameColumn.Text = "Animation Name";
            this.animNameColumn.Width = 164;
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
            this.objectFramesView.HideSelection = false;
            this.objectFramesView.Location = new System.Drawing.Point(3, 3);
            this.objectFramesView.MultiSelect = false;
            this.objectFramesView.Name = "objectFramesView";
            this.objectFramesView.ShowGroups = false;
            this.objectFramesView.ShowImagesOnSubItems = true;
            this.objectFramesView.Size = new System.Drawing.Size(229, 178);
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
            this.NameColumn.IsTileViewColumn = true;
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
            this.pictureBox.Size = new System.Drawing.Size(465, 369);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // pinCheckBox
            // 
            this.pinCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.pinCheckBox.AutoCheck = false;
            this.pinCheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pinCheckBox.BackgroundImage")));
            this.pinCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pinCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(228)))), ((int)(((byte)(247)))));
            this.pinCheckBox.Location = new System.Drawing.Point(599, 1);
            this.pinCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.pinCheckBox.Name = "pinCheckBox";
            this.pinCheckBox.Size = new System.Drawing.Size(25, 25);
            this.pinCheckBox.TabIndex = 16;
            this.pinCheckBox.ThreeState = true;
            this.pinCheckBox.UseVisualStyleBackColor = true;
            // 
            // openSpriteFileDialog
            // 
            this.openSpriteFileDialog.Filter = "Sprite|*.sp";
            this.openSpriteFileDialog.RestoreDirectory = true;
            this.openSpriteFileDialog.Title = "Open Sprite File";
            this.openSpriteFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openSpriteFileDialog_FileOk);
            // 
            // RedButton
            // 
            this.RedButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.RedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RedButton.ForeColor = System.Drawing.Color.Snow;
            this.RedButton.Location = new System.Drawing.Point(310, 1);
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
            this.GreenButton.Location = new System.Drawing.Point(330, 1);
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
            this.BlueButton.Location = new System.Drawing.Point(350, 1);
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
            this.WhiteButton.Location = new System.Drawing.Point(390, 1);
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
            this.PurpleButton.Location = new System.Drawing.Point(370, 1);
            this.PurpleButton.Name = "PurpleButton";
            this.PurpleButton.Size = new System.Drawing.Size(19, 23);
            this.PurpleButton.TabIndex = 9;
            this.PurpleButton.Text = "#";
            this.PurpleButton.UseVisualStyleBackColor = false;
            this.PurpleButton.Click += new System.EventHandler(this.PurpleButton_Click);
            // 
            // animButton
            // 
            this.animButton.Location = new System.Drawing.Point(242, 1);
            this.animButton.Name = "animButton";
            this.animButton.Size = new System.Drawing.Size(60, 23);
            this.animButton.TabIndex = 10;
            this.animButton.Text = "Sel Anim";
            this.animButton.UseVisualStyleBackColor = true;
            this.animButton.Click += new System.EventHandler(this.animButton_Click);
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.Filter = "png|*.png|gif|*.gif|bmp|*.bmp";
            this.openImageFileDialog.RestoreDirectory = true;
            this.openImageFileDialog.Title = "Import Frames";
            // 
            // zoomInButton
            // 
            this.zoomInButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("zoomInButton.BackgroundImage")));
            this.zoomInButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomInButton.Location = new System.Drawing.Point(524, 1);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(25, 25);
            this.zoomInButton.TabIndex = 13;
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("zoomOutButton.BackgroundImage")));
            this.zoomOutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomOutButton.Location = new System.Drawing.Point(549, 1);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(25, 25);
            this.zoomOutButton.TabIndex = 14;
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // handCheckBox
            // 
            this.handCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.handCheckBox.AutoCheck = false;
            this.handCheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("handCheckBox.BackgroundImage")));
            this.handCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.handCheckBox.Checked = true;
            this.handCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.handCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(228)))), ((int)(((byte)(247)))));
            this.handCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.handCheckBox.Location = new System.Drawing.Point(449, 1);
            this.handCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.handCheckBox.Name = "handCheckBox";
            this.handCheckBox.Size = new System.Drawing.Size(25, 25);
            this.handCheckBox.TabIndex = 15;
            this.handCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.handCheckBox.UseVisualStyleBackColor = true;
            // 
            // selectCheckBox
            // 
            this.selectCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.selectCheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectCheckBox.BackgroundImage")));
            this.selectCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.selectCheckBox.Location = new System.Drawing.Point(474, 1);
            this.selectCheckBox.Name = "selectCheckBox";
            this.selectCheckBox.Size = new System.Drawing.Size(25, 25);
            this.selectCheckBox.TabIndex = 16;
            this.selectCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddAnimButton
            // 
            this.AddAnimButton.Location = new System.Drawing.Point(174, 1);
            this.AddAnimButton.Name = "AddAnimButton";
            this.AddAnimButton.Size = new System.Drawing.Size(65, 23);
            this.AddAnimButton.TabIndex = 17;
            this.AddAnimButton.Text = "New Anim";
            this.AddAnimButton.UseVisualStyleBackColor = true;
            this.AddAnimButton.Click += new System.EventHandler(this.AddAnimButton_Click);
            // 
            // anchorCheckBox
            // 
            this.anchorCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.anchorCheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("anchorCheckBox.BackgroundImage")));
            this.anchorCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.anchorCheckBox.Location = new System.Drawing.Point(499, 1);
            this.anchorCheckBox.Name = "anchorCheckBox";
            this.anchorCheckBox.Size = new System.Drawing.Size(25, 25);
            this.anchorCheckBox.TabIndex = 18;
            this.anchorCheckBox.UseVisualStyleBackColor = true;
            // 
            // openPaletteFileDialog
            // 
            this.openPaletteFileDialog.Filter = "pal|*.pal";
            this.openPaletteFileDialog.RestoreDirectory = true;
            this.openPaletteFileDialog.Title = "Import Palette";
            // 
            // saveSpriteFileDialog
            // 
            this.saveSpriteFileDialog.Filter = "Sprite|*.sp";
            this.saveSpriteFileDialog.RestoreDirectory = true;
            this.saveSpriteFileDialog.Title = "Save Sprite File";
            // 
            // centerButton
            // 
            this.centerButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("centerButton.BackgroundImage")));
            this.centerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.centerButton.Location = new System.Drawing.Point(424, 1);
            this.centerButton.Name = "centerButton";
            this.centerButton.Size = new System.Drawing.Size(25, 25);
            this.centerButton.TabIndex = 19;
            this.centerButton.UseVisualStyleBackColor = true;
            this.centerButton.Click += new System.EventHandler(this.centerButton_Click);
            // 
            // wedit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 423);
            this.Controls.Add(this.centerButton);
            this.Controls.Add(this.pinCheckBox);
            this.Controls.Add(this.anchorCheckBox);
            this.Controls.Add(this.AddAnimButton);
            this.Controls.Add(this.selectCheckBox);
            this.Controls.Add(this.handCheckBox);
            this.Controls.Add(this.zoomOutButton);
            this.Controls.Add(this.zoomInButton);
            this.Controls.Add(this.animButton);
            this.Controls.Add(this.PurpleButton);
            this.Controls.Add(this.WhiteButton);
            this.Controls.Add(this.BlueButton);
            this.Controls.Add(this.GreenButton);
            this.Controls.Add(this.RedButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "wedit";
            this.Text = "World Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.animEditorBox.ResumeLayout(false);
            this.animEditorBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmdListView)).EndInit();
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
        private System.Windows.Forms.GroupBox animEditorBox;
        private BrightIdeasSoftware.ObjectListView cmdListView;
        private System.Windows.Forms.Label labelLineNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelAnimNo;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.CheckBox loopCheckBox;
        private BrightIdeasSoftware.OLVColumn animLineNoColumn;
        private BrightIdeasSoftware.OLVColumn animCmdColumn;
        private BrightIdeasSoftware.OLVColumn animArgColumn;
        private BrightIdeasSoftware.OLVColumn animImageColumn;
        private System.Windows.Forms.Button animButton;
        private System.Windows.Forms.Button buttonPrevAnim;
        private System.Windows.Forms.Button buttonNextAnim;
        private System.Windows.Forms.ToolStripMenuItem importFramesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openImageFileDialog;
        private System.Windows.Forms.Button zoomInButton;
        private System.Windows.Forms.Button zoomOutButton;
        private System.Windows.Forms.CheckBox handCheckBox;
        private System.Windows.Forms.CheckBox selectCheckBox;
        private System.Windows.Forms.ToolStripStatusLabel mouseXY;
        private System.Windows.Forms.ToolStripStatusLabel mode;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Button AddAnimButton;
        private System.Windows.Forms.ToolStripMenuItem saveSpriteFileAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPaletteToolStripMenuItem;
        private System.Windows.Forms.CheckBox anchorCheckBox;
        private System.Windows.Forms.CheckBox pinCheckBox;
        private System.Windows.Forms.OpenFileDialog openPaletteFileDialog;
        private System.Windows.Forms.SaveFileDialog saveSpriteFileDialog;
        private System.Windows.Forms.Button centerButton;
    }
}

