namespace FIMTestConfigurator {
    partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Batches  ", 11, 11);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Groups  ", 0, 0);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Tests  ", 1, 1);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Configs  ", 10, 10);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("MAs  ", 3, 3);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Sources  ", 4, 4);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Input Sets   ", 5, 5);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Output Sets   ", 6, 6);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Scripts    ", 2, 2);
            this.cStatusStrip = new System.Windows.Forms.StatusStrip();
            this.StBar1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StBar2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cToolStrip = new System.Windows.Forms.ToolStrip();
            this.bOpenDB = new System.Windows.Forms.ToolStripButton();
            this.bShowHide = new System.Windows.Forms.ToolStripButton();
            this.bCheckIntegrity = new System.Windows.Forms.ToolStripButton();
            this.bEditVariables = new System.Windows.Forms.ToolStripButton();
            this.bCloseDB = new System.Windows.Forms.ToolStripButton();
            this.cFindSep = new System.Windows.Forms.ToolStripSeparator();
            this.lFind = new System.Windows.Forms.ToolStripLabel();
            this.cFind = new System.Windows.Forms.ToolStripTextBox();
            this.cRunSep = new System.Windows.Forms.ToolStripSeparator();
            this.bRunTest = new System.Windows.Forms.ToolStripButton();
            this.cToolsSep = new System.Windows.Forms.ToolStripSeparator();
            this.bOpenFIMSyncDiv = new System.Windows.Forms.ToolStripButton();
            this.bOpenFIMSyncNat = new System.Windows.Forms.ToolStripButton();
            this.bOpenConfigFiles = new System.Windows.Forms.ToolStripButton();
            this.cErrorSep = new System.Windows.Forms.ToolStripSeparator();
            this.bError = new System.Windows.Forms.ToolStripButton();
            this.cLstFonts = new System.Windows.Forms.ToolStripComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cTv = new System.Windows.Forms.TreeView();
            this.cImg = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cPanelEdit = new System.Windows.Forms.Panel();
            this.lBack = new System.Windows.Forms.LinkLabel();
            this.cCommit = new System.Windows.Forms.CheckBox();
            this.cDelta = new System.Windows.Forms.CheckBox();
            this.cbDetail = new System.Windows.Forms.ComboBox();
            this.bDetail = new System.Windows.Forms.Button();
            this.lAuthType = new System.Windows.Forms.Label();
            this.lPassword = new System.Windows.Forms.Label();
            this.lUser = new System.Windows.Forms.Label();
            this.lDetail = new System.Windows.Forms.Label();
            this.cDetail = new System.Windows.Forms.TextBox();
            this.lName = new System.Windows.Forms.Label();
            this.cAuthType = new System.Windows.Forms.TextBox();
            this.cPassword = new System.Windows.Forms.TextBox();
            this.cUser = new System.Windows.Forms.TextBox();
            this.cServer = new System.Windows.Forms.TextBox();
            this.cName = new System.Windows.Forms.TextBox();
            this.bSave = new System.Windows.Forms.Button();
            this.lDesc = new FIMTestConfigurator.RichTextBoxEx();
            this.cPanelRecent = new System.Windows.Forms.Panel();
            this.cLstRecent = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lRecent = new System.Windows.Forms.Label();
            this.cEditDetail = new System.Windows.Forms.TextBox();
            this.bSaveItem = new System.Windows.Forms.Button();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mCtxAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mCtxDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.mCtxHide = new System.Windows.Forms.ToolStripMenuItem();
            this.mCtxSep = new System.Windows.Forms.ToolStripSeparator();
            this.mCtxDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mCtxSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mCtxReorder = new System.Windows.Forms.ToolStripMenuItem();
            this.cToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ctxMenuEdition = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cStatusStrip.SuspendLayout();
            this.cToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.cPanelEdit.SuspendLayout();
            this.cPanelRecent.SuspendLayout();
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // cStatusStrip
            // 
            this.cStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StBar1,
            this.StBar2});
            this.cStatusStrip.Location = new System.Drawing.Point(0, 531);
            this.cStatusStrip.Name = "cStatusStrip";
            this.cStatusStrip.ShowItemToolTips = true;
            this.cStatusStrip.Size = new System.Drawing.Size(1045, 22);
            this.cStatusStrip.TabIndex = 1;
            this.cStatusStrip.Text = "statusStrip1";
            // 
            // StBar1
            // 
            this.StBar1.Name = "StBar1";
            this.StBar1.Size = new System.Drawing.Size(13, 17);
            this.StBar1.Text = "  ";
            // 
            // StBar2
            // 
            this.StBar2.Name = "StBar2";
            this.StBar2.Size = new System.Drawing.Size(1017, 17);
            this.StBar2.Spring = true;
            this.StBar2.Text = "  ";
            this.StBar2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cToolStrip
            // 
            this.cToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bOpenDB,
            this.bShowHide,
            this.bCheckIntegrity,
            this.bEditVariables,
            this.bCloseDB,
            this.cFindSep,
            this.lFind,
            this.cFind,
            this.cRunSep,
            this.bRunTest,
            this.cToolsSep,
            this.bOpenFIMSyncDiv,
            this.bOpenFIMSyncNat,
            this.bOpenConfigFiles,
            this.cErrorSep,
            this.bError,
            this.cLstFonts});
            this.cToolStrip.Location = new System.Drawing.Point(0, 0);
            this.cToolStrip.Name = "cToolStrip";
            this.cToolStrip.Size = new System.Drawing.Size(1045, 25);
            this.cToolStrip.TabIndex = 0;
            this.cToolStrip.Text = "toolStrip1";
            // 
            // bOpenDB
            // 
            this.bOpenDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bOpenDB.Image = ((System.Drawing.Image)(resources.GetObject("bOpenDB.Image")));
            this.bOpenDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenDB.Name = "bOpenDB";
            this.bOpenDB.Size = new System.Drawing.Size(86, 22);
            this.bOpenDB.Text = "&Open Test File";
            this.bOpenDB.Click += new System.EventHandler(this.bOpenDB_Click);
            // 
            // bShowHide
            // 
            this.bShowHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bShowHide.Enabled = false;
            this.bShowHide.Image = ((System.Drawing.Image)(resources.GetObject("bShowHide.Image")));
            this.bShowHide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bShowHide.Name = "bShowHide";
            this.bShowHide.Size = new System.Drawing.Size(98, 22);
            this.bShowHide.Text = "Show/Hide View";
            this.bShowHide.Click += new System.EventHandler(this.bShowHide_Click);
            // 
            // bCheckIntegrity
            // 
            this.bCheckIntegrity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bCheckIntegrity.Enabled = false;
            this.bCheckIntegrity.Image = ((System.Drawing.Image)(resources.GetObject("bCheckIntegrity.Image")));
            this.bCheckIntegrity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bCheckIntegrity.Name = "bCheckIntegrity";
            this.bCheckIntegrity.Size = new System.Drawing.Size(91, 22);
            this.bCheckIntegrity.Text = "Check &Integrity";
            this.bCheckIntegrity.Click += new System.EventHandler(this.bCheckIntegrity_Click);
            // 
            // bEditVariables
            // 
            this.bEditVariables.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bEditVariables.Enabled = false;
            this.bEditVariables.Image = ((System.Drawing.Image)(resources.GetObject("bEditVariables.Image")));
            this.bEditVariables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bEditVariables.Name = "bEditVariables";
            this.bEditVariables.Size = new System.Drawing.Size(81, 22);
            this.bEditVariables.Text = "Edit Variables";
            this.bEditVariables.Click += new System.EventHandler(this.bEditVariables_Click);
            // 
            // bCloseDB
            // 
            this.bCloseDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bCloseDB.Enabled = false;
            this.bCloseDB.Image = ((System.Drawing.Image)(resources.GetObject("bCloseDB.Image")));
            this.bCloseDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bCloseDB.Name = "bCloseDB";
            this.bCloseDB.Size = new System.Drawing.Size(58, 22);
            this.bCloseDB.Text = "&Close DB";
            this.bCloseDB.Click += new System.EventHandler(this.bCloseDB_Click);
            // 
            // cFindSep
            // 
            this.cFindSep.Name = "cFindSep";
            this.cFindSep.Size = new System.Drawing.Size(6, 25);
            // 
            // lFind
            // 
            this.lFind.Enabled = false;
            this.lFind.Name = "lFind";
            this.lFind.Size = new System.Drawing.Size(33, 22);
            this.lFind.Text = "Find:";
            this.lFind.ToolTipText = "CTRL+F: to focus search box.\r\nF3: to start search.";
            // 
            // cFind
            // 
            this.cFind.AcceptsReturn = true;
            this.cFind.Enabled = false;
            this.cFind.Name = "cFind";
            this.cFind.Size = new System.Drawing.Size(125, 25);
            this.cFind.ToolTipText = "CTRL+F: to focus search box.\r\nF3: to start search.\r\n";
            this.cFind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cFind_KeyUp);
            // 
            // cRunSep
            // 
            this.cRunSep.Name = "cRunSep";
            this.cRunSep.Size = new System.Drawing.Size(6, 25);
            // 
            // bRunTest
            // 
            this.bRunTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bRunTest.Enabled = false;
            this.bRunTest.Image = ((System.Drawing.Image)(resources.GetObject("bRunTest.Image")));
            this.bRunTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRunTest.Name = "bRunTest";
            this.bRunTest.Size = new System.Drawing.Size(62, 22);
            this.bRunTest.Tag = "FIMTestRunnerApp";
            this.bRunTest.Text = "&Run Tests";
            this.bRunTest.Click += new System.EventHandler(this.mCtxRunTool_Click);
            // 
            // cToolsSep
            // 
            this.cToolsSep.Name = "cToolsSep";
            this.cToolsSep.Size = new System.Drawing.Size(6, 25);
            // 
            // bOpenFIMSyncDiv
            // 
            this.bOpenFIMSyncDiv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bOpenFIMSyncDiv.Image = ((System.Drawing.Image)(resources.GetObject("bOpenFIMSyncDiv.Image")));
            this.bOpenFIMSyncDiv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenFIMSyncDiv.Name = "bOpenFIMSyncDiv";
            this.bOpenFIMSyncDiv.Size = new System.Drawing.Size(79, 22);
            this.bOpenFIMSyncDiv.Tag = "FIMSyncDivApp";
            this.bOpenFIMSyncDiv.Text = "FIM Sync Div";
            this.bOpenFIMSyncDiv.Click += new System.EventHandler(this.mCtxRunTool_Click);
            // 
            // bOpenFIMSyncNat
            // 
            this.bOpenFIMSyncNat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bOpenFIMSyncNat.Image = ((System.Drawing.Image)(resources.GetObject("bOpenFIMSyncNat.Image")));
            this.bOpenFIMSyncNat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenFIMSyncNat.Name = "bOpenFIMSyncNat";
            this.bOpenFIMSyncNat.Size = new System.Drawing.Size(81, 22);
            this.bOpenFIMSyncNat.Tag = "FIMSyncNatApp";
            this.bOpenFIMSyncNat.Text = "FIM Sync Nat";
            this.bOpenFIMSyncNat.Click += new System.EventHandler(this.mCtxRunTool_Click);
            // 
            // bOpenConfigFiles
            // 
            this.bOpenConfigFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bOpenConfigFiles.Image = ((System.Drawing.Image)(resources.GetObject("bOpenConfigFiles.Image")));
            this.bOpenConfigFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenConfigFiles.Name = "bOpenConfigFiles";
            this.bOpenConfigFiles.Size = new System.Drawing.Size(96, 22);
            this.bOpenConfigFiles.Tag = "FIMConfigFiles";
            this.bOpenConfigFiles.Text = "FIM Config Files";
            this.bOpenConfigFiles.Click += new System.EventHandler(this.mCtxRunTool_Click);
            // 
            // cErrorSep
            // 
            this.cErrorSep.Name = "cErrorSep";
            this.cErrorSep.Size = new System.Drawing.Size(6, 27);
            this.cErrorSep.Visible = false;
            // 
            // bError
            // 
            this.bError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bError.ForeColor = System.Drawing.Color.Red;
            this.bError.Image = ((System.Drawing.Image)(resources.GetObject("bError.Image")));
            this.bError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bError.Name = "bError";
            this.bError.Size = new System.Drawing.Size(64, 24);
            this.bError.Text = "Last Error";
            this.bError.ToolTipText = " ";
            this.bError.Visible = false;
            // 
            // cLstFonts
            // 
            this.cLstFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cLstFonts.Name = "cLstFonts";
            this.cLstFonts.Size = new System.Drawing.Size(121, 25);
            this.cLstFonts.Visible = false;
            this.cLstFonts.SelectedIndexChanged += new System.EventHandler(this.cLstFonts_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cTv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1045, 506);
            this.splitContainer1.SplitterDistance = 159;
            this.splitContainer1.TabIndex = 2;
            // 
            // cTv
            // 
            this.cTv.AllowDrop = true;
            this.cTv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTv.Enabled = false;
            this.cTv.HideSelection = false;
            this.cTv.ImageIndex = 0;
            this.cTv.ImageList = this.cImg;
            this.cTv.LabelEdit = true;
            this.cTv.Location = new System.Drawing.Point(0, 0);
            this.cTv.Name = "cTv";
            treeNode1.ImageIndex = 11;
            treeNode1.Name = "tvBatches";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode1.SelectedImageIndex = 11;
            treeNode1.Tag = "batch";
            treeNode1.Text = "Batches  ";
            treeNode2.ImageIndex = 0;
            treeNode2.Name = "tvGroups";
            treeNode2.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode2.SelectedImageIndex = 0;
            treeNode2.Tag = "groups";
            treeNode2.Text = "Groups  ";
            treeNode3.ImageIndex = 1;
            treeNode3.Name = "tvTests";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode3.SelectedImageIndex = 1;
            treeNode3.Tag = "test";
            treeNode3.Text = "Tests  ";
            treeNode4.ImageIndex = 10;
            treeNode4.Name = "tvConfigs";
            treeNode4.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode4.SelectedImageIndex = 10;
            treeNode4.Tag = "config";
            treeNode4.Text = "Configs  ";
            treeNode5.ImageIndex = 3;
            treeNode5.Name = "tvMAs";
            treeNode5.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode5.SelectedImageIndex = 3;
            treeNode5.Tag = "managementagent";
            treeNode5.Text = "MAs  ";
            treeNode6.ImageIndex = 4;
            treeNode6.Name = "tvSources";
            treeNode6.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode6.SelectedImageIndex = 4;
            treeNode6.Tag = "source";
            treeNode6.Text = "Sources  ";
            treeNode7.ImageIndex = 5;
            treeNode7.Name = "tvInputSets";
            treeNode7.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode7.SelectedImageIndex = 5;
            treeNode7.Tag = "inputset";
            treeNode7.Text = "Input Sets   ";
            treeNode8.ImageIndex = 6;
            treeNode8.Name = "tvOutputSets";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode8.SelectedImageIndex = 6;
            treeNode8.Tag = "outputset";
            treeNode8.Text = "Output Sets   ";
            treeNode9.ImageIndex = 2;
            treeNode9.Name = "tvScripts";
            treeNode9.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode9.SelectedImageIndex = 2;
            treeNode9.Tag = "script";
            treeNode9.Text = "Scripts    ";
            this.cTv.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this.cTv.SelectedImageIndex = 0;
            this.cTv.ShowLines = false;
            this.cTv.ShowNodeToolTips = true;
            this.cTv.Size = new System.Drawing.Size(157, 504);
            this.cTv.TabIndex = 0;
            this.cTv.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.cTv_BeforeLabelEdit);
            this.cTv.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.cTv_AfterLabelEdit);
            this.cTv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.cTv_AfterCheck);
            this.cTv.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.cTv_ItemDrag);
            this.cTv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.cTv_AfterSelect);
            this.cTv.DragDrop += new System.Windows.Forms.DragEventHandler(this.cTv_DragDrop);
            this.cTv.DragOver += new System.Windows.Forms.DragEventHandler(this.cTv_DragOver);
            this.cTv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cTv_KeyDown);
            this.cTv.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cTv_MouseUp);
            // 
            // cImg
            // 
            this.cImg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("cImg.ImageStream")));
            this.cImg.TransparentColor = System.Drawing.Color.Transparent;
            this.cImg.Images.SetKeyName(0, "groups.ico");
            this.cImg.Images.SetKeyName(1, "test.ico");
            this.cImg.Images.SetKeyName(2, "script.ico");
            this.cImg.Images.SetKeyName(3, "MA.ico");
            this.cImg.Images.SetKeyName(4, "sources.ico");
            this.cImg.Images.SetKeyName(5, "input.ico");
            this.cImg.Images.SetKeyName(6, "output.ico");
            this.cImg.Images.SetKeyName(7, "MA2-16.png");
            this.cImg.Images.SetKeyName(8, "TestFail-16.png");
            this.cImg.Images.SetKeyName(9, "Arrow-16.png");
            this.cImg.Images.SetKeyName(10, "config.ico");
            this.cImg.Images.SetKeyName(11, "batches.ico");
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cPanelEdit);
            this.splitContainer2.Panel1.Controls.Add(this.cPanelRecent);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.cEditDetail);
            this.splitContainer2.Panel2.Controls.Add(this.bSaveItem);
            this.splitContainer2.Size = new System.Drawing.Size(882, 506);
            this.splitContainer2.SplitterDistance = 534;
            this.splitContainer2.TabIndex = 3;
            // 
            // cPanelEdit
            // 
            this.cPanelEdit.Controls.Add(this.lBack);
            this.cPanelEdit.Controls.Add(this.cCommit);
            this.cPanelEdit.Controls.Add(this.cDelta);
            this.cPanelEdit.Controls.Add(this.cbDetail);
            this.cPanelEdit.Controls.Add(this.bDetail);
            this.cPanelEdit.Controls.Add(this.lAuthType);
            this.cPanelEdit.Controls.Add(this.lPassword);
            this.cPanelEdit.Controls.Add(this.lUser);
            this.cPanelEdit.Controls.Add(this.lDetail);
            this.cPanelEdit.Controls.Add(this.cDetail);
            this.cPanelEdit.Controls.Add(this.lName);
            this.cPanelEdit.Controls.Add(this.cAuthType);
            this.cPanelEdit.Controls.Add(this.cPassword);
            this.cPanelEdit.Controls.Add(this.cUser);
            this.cPanelEdit.Controls.Add(this.cServer);
            this.cPanelEdit.Controls.Add(this.cName);
            this.cPanelEdit.Controls.Add(this.bSave);
            this.cPanelEdit.Controls.Add(this.lDesc);
            this.cPanelEdit.Location = new System.Drawing.Point(26, 23);
            this.cPanelEdit.Name = "cPanelEdit";
            this.cPanelEdit.Size = new System.Drawing.Size(532, 504);
            this.cPanelEdit.TabIndex = 0;
            this.cPanelEdit.Visible = false;
            // 
            // lBack
            // 
            this.lBack.AutoSize = true;
            this.lBack.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBack.Location = new System.Drawing.Point(5, 5);
            this.lBack.Name = "lBack";
            this.lBack.Size = new System.Drawing.Size(50, 16);
            this.lBack.TabIndex = 13;
            this.lBack.TabStop = true;
            this.lBack.Text = "go back";
            this.lBack.Visible = false;
            this.lBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lBack_LinkClicked);
            // 
            // cCommit
            // 
            this.cCommit.AutoSize = true;
            this.cCommit.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cCommit.Location = new System.Drawing.Point(205, 68);
            this.cCommit.Name = "cCommit";
            this.cCommit.Size = new System.Drawing.Size(116, 20);
            this.cCommit.TabIndex = 11;
            this.cCommit.Text = "Commit Changes";
            this.cCommit.UseVisualStyleBackColor = true;
            this.cCommit.CheckedChanged += new System.EventHandler(this.CheckButton_CheckedChanged);
            // 
            // cDelta
            // 
            this.cDelta.AutoSize = true;
            this.cDelta.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cDelta.Location = new System.Drawing.Point(100, 68);
            this.cDelta.Name = "cDelta";
            this.cDelta.Size = new System.Drawing.Size(82, 20);
            this.cDelta.TabIndex = 10;
            this.cDelta.Text = "Delta Sync";
            this.cDelta.UseVisualStyleBackColor = true;
            this.cDelta.CheckedChanged += new System.EventHandler(this.CheckButton_CheckedChanged);
            // 
            // cbDetail
            // 
            this.cbDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDetail.FormattingEnabled = true;
            this.cbDetail.Location = new System.Drawing.Point(97, 66);
            this.cbDetail.Name = "cbDetail";
            this.cbDetail.Size = new System.Drawing.Size(386, 21);
            this.cbDetail.TabIndex = 3;
            this.cbDetail.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // bDetail
            // 
            this.bDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bDetail.Location = new System.Drawing.Point(490, 40);
            this.bDetail.Name = "bDetail";
            this.bDetail.Size = new System.Drawing.Size(28, 20);
            this.bDetail.TabIndex = 2;
            this.bDetail.Text = ">>";
            this.bDetail.UseVisualStyleBackColor = true;
            this.bDetail.Click += new System.EventHandler(this.bDetail_Click);
            // 
            // lAuthType
            // 
            this.lAuthType.AutoSize = true;
            this.lAuthType.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAuthType.Location = new System.Drawing.Point(15, 147);
            this.lAuthType.Name = "lAuthType";
            this.lAuthType.Size = new System.Drawing.Size(62, 16);
            this.lAuthType.TabIndex = 10;
            this.lAuthType.Text = "AuthType:";
            // 
            // lPassword
            // 
            this.lPassword.AutoSize = true;
            this.lPassword.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPassword.Location = new System.Drawing.Point(15, 121);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(61, 16);
            this.lPassword.TabIndex = 8;
            this.lPassword.Text = "Password:";
            // 
            // lUser
            // 
            this.lUser.AutoSize = true;
            this.lUser.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lUser.Location = new System.Drawing.Point(15, 95);
            this.lUser.Name = "lUser";
            this.lUser.Size = new System.Drawing.Size(34, 16);
            this.lUser.TabIndex = 7;
            this.lUser.Text = "User:";
            // 
            // lDetail
            // 
            this.lDetail.AutoSize = true;
            this.lDetail.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDetail.Location = new System.Drawing.Point(16, 69);
            this.lDetail.Name = "lDetail";
            this.lDetail.Size = new System.Drawing.Size(46, 16);
            this.lDetail.TabIndex = 4;
            this.lDetail.Text = "Config:";
            // 
            // cDetail
            // 
            this.cDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cDetail.Location = new System.Drawing.Point(97, 66);
            this.cDetail.Name = "cDetail";
            this.cDetail.Size = new System.Drawing.Size(386, 20);
            this.cDetail.TabIndex = 8;
            this.cDetail.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(16, 43);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(44, 16);
            this.lName.TabIndex = 1;
            this.lName.Text = "Group:";
            // 
            // cAuthType
            // 
            this.cAuthType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cAuthType.Location = new System.Drawing.Point(97, 144);
            this.cAuthType.Name = "cAuthType";
            this.cAuthType.Size = new System.Drawing.Size(386, 20);
            this.cAuthType.TabIndex = 9;
            this.cAuthType.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // cPassword
            // 
            this.cPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cPassword.Location = new System.Drawing.Point(97, 118);
            this.cPassword.Name = "cPassword";
            this.cPassword.PasswordChar = '*';
            this.cPassword.Size = new System.Drawing.Size(386, 20);
            this.cPassword.TabIndex = 7;
            this.cPassword.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // cUser
            // 
            this.cUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cUser.Location = new System.Drawing.Point(97, 92);
            this.cUser.Name = "cUser";
            this.cUser.Size = new System.Drawing.Size(386, 20);
            this.cUser.TabIndex = 5;
            this.cUser.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // cServer
            // 
            this.cServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cServer.Location = new System.Drawing.Point(97, 66);
            this.cServer.Name = "cServer";
            this.cServer.Size = new System.Drawing.Size(386, 20);
            this.cServer.TabIndex = 4;
            this.cServer.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // cName
            // 
            this.cName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cName.Location = new System.Drawing.Point(97, 40);
            this.cName.Name = "cName";
            this.cName.Size = new System.Drawing.Size(386, 20);
            this.cName.TabIndex = 0;
            this.cName.TextChanged += new System.EventHandler(this.EditItem_Changed);
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSave.Location = new System.Drawing.Point(400, 6);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(118, 22);
            this.bSave.TabIndex = 11;
            this.bSave.Text = "Save Changes";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // lDesc
            // 
            this.lDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lDesc.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.lDesc.Location = new System.Drawing.Point(17, 95);
            this.lDesc.Name = "lDesc";
            this.lDesc.ReadOnly = true;
            this.lDesc.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.lDesc.ShortcutsEnabled = false;
            this.lDesc.Size = new System.Drawing.Size(463, 16);
            this.lDesc.TabIndex = 14;
            this.lDesc.Text = "";
            this.lDesc.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.lDesc_ContentsResized);
            this.lDesc.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.lDesc_LinkClicked);
            // 
            // cPanelRecent
            // 
            this.cPanelRecent.Controls.Add(this.cLstRecent);
            this.cPanelRecent.Controls.Add(this.lRecent);
            this.cPanelRecent.Location = new System.Drawing.Point(3, 108);
            this.cPanelRecent.Name = "cPanelRecent";
            this.cPanelRecent.Size = new System.Drawing.Size(530, 312);
            this.cPanelRecent.TabIndex = 12;
            this.cPanelRecent.Visible = false;
            // 
            // cLstRecent
            // 
            this.cLstRecent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cLstRecent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.cLstRecent.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLstRecent.FullRowSelect = true;
            this.cLstRecent.GridLines = true;
            this.cLstRecent.HideSelection = false;
            this.cLstRecent.Location = new System.Drawing.Point(4, 45);
            this.cLstRecent.Name = "cLstRecent";
            this.cLstRecent.Size = new System.Drawing.Size(523, 264);
            this.cLstRecent.TabIndex = 1;
            this.cLstRecent.UseCompatibleStateImageBehavior = false;
            this.cLstRecent.View = System.Windows.Forms.View.Details;
            this.cLstRecent.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cLstRecent_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Path";
            // 
            // lRecent
            // 
            this.lRecent.AutoSize = true;
            this.lRecent.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRecent.Location = new System.Drawing.Point(4, 17);
            this.lRecent.Name = "lRecent";
            this.lRecent.Size = new System.Drawing.Size(78, 16);
            this.lRecent.TabIndex = 0;
            this.lRecent.Text = "Recent Files";
            // 
            // cEditDetail
            // 
            this.cEditDetail.AcceptsReturn = true;
            this.cEditDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cEditDetail.Enabled = false;
            this.cEditDetail.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cEditDetail.Location = new System.Drawing.Point(0, 33);
            this.cEditDetail.Multiline = true;
            this.cEditDetail.Name = "cEditDetail";
            this.cEditDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.cEditDetail.Size = new System.Drawing.Size(342, 471);
            this.cEditDetail.TabIndex = 0;
            this.cEditDetail.WordWrap = false;
            this.cEditDetail.TextChanged += new System.EventHandler(this.cEditDetail_TextChanged);
            // 
            // bSaveItem
            // 
            this.bSaveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSaveItem.Enabled = false;
            this.bSaveItem.Location = new System.Drawing.Point(283, 3);
            this.bSaveItem.Name = "bSaveItem";
            this.bSaveItem.Size = new System.Drawing.Size(56, 21);
            this.bSaveItem.TabIndex = 1;
            this.bSaveItem.Text = "Save";
            this.bSaveItem.UseVisualStyleBackColor = true;
            this.bSaveItem.Click += new System.EventHandler(this.bSaveItem_Click);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mCtxAdd,
            this.mCtxDuplicate,
            this.mCtxHide,
            this.mCtxSep,
            this.mCtxDelete,
            this.mCtxSep2,
            this.mCtxReorder});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(167, 126);
            // 
            // mCtxAdd
            // 
            this.mCtxAdd.Name = "mCtxAdd";
            this.mCtxAdd.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.mCtxAdd.Size = new System.Drawing.Size(166, 22);
            this.mCtxAdd.Text = "&Add New";
            this.mCtxAdd.Click += new System.EventHandler(this.mCtxAdd_Click);
            // 
            // mCtxDuplicate
            // 
            this.mCtxDuplicate.Name = "mCtxDuplicate";
            this.mCtxDuplicate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mCtxDuplicate.Size = new System.Drawing.Size(166, 22);
            this.mCtxDuplicate.Text = "D&uplicate";
            this.mCtxDuplicate.Click += new System.EventHandler(this.mCtxDuplicate_Click);
            // 
            // mCtxHide
            // 
            this.mCtxHide.Name = "mCtxHide";
            this.mCtxHide.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.mCtxHide.Size = new System.Drawing.Size(166, 22);
            this.mCtxHide.Text = "Hide";
            this.mCtxHide.Click += new System.EventHandler(this.mCtxHide_Click);
            // 
            // mCtxSep
            // 
            this.mCtxSep.Name = "mCtxSep";
            this.mCtxSep.Size = new System.Drawing.Size(163, 6);
            // 
            // mCtxDelete
            // 
            this.mCtxDelete.Name = "mCtxDelete";
            this.mCtxDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mCtxDelete.Size = new System.Drawing.Size(166, 22);
            this.mCtxDelete.Text = "&Delete";
            this.mCtxDelete.Click += new System.EventHandler(this.mCtxDelete_Click);
            // 
            // mCtxSep2
            // 
            this.mCtxSep2.Name = "mCtxSep2";
            this.mCtxSep2.Size = new System.Drawing.Size(163, 6);
            // 
            // mCtxReorder
            // 
            this.mCtxReorder.Name = "mCtxReorder";
            this.mCtxReorder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mCtxReorder.Size = new System.Drawing.Size(166, 22);
            this.mCtxReorder.Text = "Reorder";
            this.mCtxReorder.Click += new System.EventHandler(this.mCtxReorder_Click);
            // 
            // ctxMenuEdition
            // 
            this.ctxMenuEdition.Name = "ctxMenuEdition";
            this.ctxMenuEdition.Size = new System.Drawing.Size(61, 4);
            // 
            // frmMain
            // 
            this.AcceptButton = this.bSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 553);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cToolStrip);
            this.Controls.Add(this.cStatusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "FIM Tests Configurator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.cStatusStrip.ResumeLayout(false);
            this.cStatusStrip.PerformLayout();
            this.cToolStrip.ResumeLayout(false);
            this.cToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.cPanelEdit.ResumeLayout(false);
            this.cPanelEdit.PerformLayout();
            this.cPanelRecent.ResumeLayout(false);
            this.cPanelRecent.PerformLayout();
            this.ctxMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.StatusStrip cStatusStrip;
        private System.Windows.Forms.ToolStrip cToolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox cEditDetail;
        private System.Windows.Forms.Button bSaveItem;
        private System.Windows.Forms.ToolStripButton bOpenDB;
        private System.Windows.Forms.TreeView cTv;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem mCtxAdd;
        private System.Windows.Forms.ToolStripMenuItem mCtxDelete;
        private System.Windows.Forms.ToolStripMenuItem mCtxDuplicate;
        private System.Windows.Forms.ToolStripSeparator mCtxSep;
        private System.Windows.Forms.ToolStripStatusLabel StBar1;
        private System.Windows.Forms.Panel cPanelEdit;
        private System.Windows.Forms.ComboBox cbDetail;
        private System.Windows.Forms.Button bDetail;
        private System.Windows.Forms.Label lAuthType;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.Label lUser;
        private System.Windows.Forms.Label lDetail;
        private System.Windows.Forms.TextBox cDetail;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.TextBox cAuthType;
        private System.Windows.Forms.TextBox cPassword;
        private System.Windows.Forms.TextBox cUser;
        private System.Windows.Forms.TextBox cServer;
        private System.Windows.Forms.TextBox cName;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.ToolStripButton bCloseDB;
        private System.Windows.Forms.ToolStripButton bCheckIntegrity;
        private System.Windows.Forms.ToolStripSeparator mCtxSep2;
        private System.Windows.Forms.ImageList cImg;
        private System.Windows.Forms.CheckBox cCommit;
        private System.Windows.Forms.CheckBox cDelta;
        private System.Windows.Forms.ToolStripMenuItem mCtxHide;
        private System.Windows.Forms.ToolStripStatusLabel StBar2;
        private System.Windows.Forms.ToolStripButton bRunTest;
        private System.Windows.Forms.ToolStripButton bOpenFIMSyncDiv;
        private System.Windows.Forms.ToolStripButton bOpenFIMSyncNat;
        private System.Windows.Forms.Panel cPanelRecent;
        private System.Windows.Forms.ListView cLstRecent;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label lRecent;
        private System.Windows.Forms.ToolStripButton bOpenConfigFiles;
        private System.Windows.Forms.ToolStripButton bError;
        private System.Windows.Forms.ToolStripMenuItem mCtxReorder;
        private System.Windows.Forms.ToolStripButton bShowHide;
        private System.Windows.Forms.LinkLabel lBack;
        private System.Windows.Forms.ToolTip cToolTip;
        private System.Windows.Forms.ToolStripComboBox cLstFonts;
        private System.Windows.Forms.ToolStripSeparator cFindSep;
        private System.Windows.Forms.ToolStripLabel lFind;
        private System.Windows.Forms.ToolStripTextBox cFind;
        private System.Windows.Forms.ToolStripSeparator cRunSep;
        private System.Windows.Forms.ToolStripSeparator cToolsSep;
        private System.Windows.Forms.ToolStripSeparator cErrorSep;
        private System.Windows.Forms.ContextMenuStrip ctxMenuEdition;
        private RichTextBoxEx lDesc;
        private System.Windows.Forms.ToolStripButton bEditVariables;
        }
    }

