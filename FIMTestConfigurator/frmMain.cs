using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public partial class frmMain : Form {
        private TreeNode mDragAndDropTarget = null;
        private List<string> lHiddenItems = null;
        // Para messagebox con checkbox
        //https://social.msdn.microsoft.com/Forums/en-US/b34fe98d-8254-4d15-a0b7-3e1a142bf7c0/quotdont-show-againquot-messagebox?forum=winforms
        //https://www.pinvoke.net/default.aspx/shlwapi/SHMessageBoxCheck.html
        //_______________________________________________________________________________________________________________________
        private bool InShowHideView => cTv.CheckBoxes;
        private string DBFile { get; set; }
        //_______________________________________________________________________________________________________________________
        public frmMain() {
            InitializeComponent();
            Text = (TestsHelper.IsAdministrator ? "Administrator: " : "") + "FIM Test Configurator";
            cPanelRecent.Dock = DockStyle.Fill;
            cPanelEdit.Dock = DockStyle.Fill;
            // el tag de lBack contiene una lista de sitios por los que ha pasado
            if (lBack.Tag == null) lBack.Tag = new List<string>();
            lBack.Links.Clear();
            LoadExternalTools();
            if (Environment.GetCommandLineArgs().Length == 2) OpenDB(Environment.GetCommandLineArgs()[1]);
            else ShowRecentPanel();

            //DESCOMENTAR PARA SELECCIONAR TIPO DE LETRA: 
            // FillFontList();
            }
        //_______________________________________________________________________________________________________________________
        //PARA SELECCIONAR TIPO DE LETRA 
        private void cLstFonts_SelectedIndexChanged(object sender, EventArgs e) {
            Font f = new Font(cLstFonts.SelectedItem.ToString(), (float)8.25);
            foreach (var c in cPanelEdit.Controls) {
                if (c.GetType() == lName.GetType() || c.GetType() == lDesc.GetType()) ((Label)c).Font = f;
                if (c.GetType() == cbDetail.GetType()) ((ComboBox)c).Font = f;
                if (c.GetType() == cName.GetType()) ((TextBox)c).Font = f;
                if (c.GetType() == cDelta.GetType()) ((CheckBox)c).Font = f;
                }
            cTv.Font = f;
            cEditDetail.Font = f;
            bSave.Font = f;
            bSaveItem.Font = f;
            }
        //_______________________________________________________________________________________________________________________
        private void FrmMain_KeyDown(object sender, KeyEventArgs e) {
            if (InShowHideView) return;
            if (e.Control && e.KeyCode == Keys.F && cFind.Enabled) cFind.Focus();
            if (e.KeyCode == Keys.F3 && cFind.Enabled && !string.IsNullOrEmpty(cFind.Text)) SearchText();
            }
        //_______________________________________________________________________________________________________________________
        private void cFind_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(cFind.Text)) SearchText(); }
        //_______________________________________________________________________________________________________________________
        private void bOpenDB_Click( object sender, EventArgs e ) { OpenDB(); }
        //_______________________________________________________________________________________________________________________
        private void lBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { LinkClicked(e.Link, e.Link.LinkData as string, true); }
        //_______________________________________________________________________________________________________________________
        //private void lDesc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { LinkClicked(e.Link, e.Link.LinkData as string); }
        private void lDesc_LinkClicked( object sender, LinkClickedEventArgs e ) { LinkClicked(null, e.LinkText.Split('#')[1]); }
        //_______________________________________________________________________________________________________________________
        private void lDesc_ContentsResized( object sender, ContentsResizedEventArgs e ) { ((RichTextBox)sender).Height = e.NewRectangle.Height + 5; }
        //_______________________________________________________________________________________________________________________
        private void cTv_AfterCheck(object sender, TreeViewEventArgs e) {
            if (!InShowHideView || e.Node.Parent != null || !cTv.Enabled) return;
            foreach (TreeNode n in e.Node.Nodes) n.Checked = e.Node.Checked;
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) { if (e.Node.Parent == null || e.Node.Tag == null || InShowHideView) e.CancelEdit = true; }
        //_______________________________________________________________________________________________________________________
        private void cTv_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) { TestsHelper.UpdateName(TestObjectBase.LinkedObject(e.Node.Tag), e.Label); ShowObjectInEditor(); }
        //_______________________________________________________________________________________________________________________
        private void cTv_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right || InShowHideView) return;
            Point p = new Point(e.X, e.Y);
            TreeNode node = cTv.GetNodeAt(p);
            if (node == null) return;

            cTv.SelectedNode = node;
            ConfigureCtxMenu(node, p);
            }
         //_______________________________________________________________________________________________________________________
        private void cTv_DragDrop(object sender, DragEventArgs e) {
            Point targetPoint = cTv.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = cTv.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            ResaltarTargetDrag(targetNode, false);
            if (!IsValidDrag(draggedNode, targetNode)) return;
            TestsHelper.CompleteDrop(TestObjectBase.LinkedObject(draggedNode.Tag), TestObjectBase.LinkedObject(targetNode.Tag));
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_ItemDrag(object sender, ItemDragEventArgs e) {
            if (InShowHideView) return;
            cTv.SelectedNode = (TreeNode)e.Item;
            TestObjectBase o = TestObjectBase.LinkedObject(((TreeNode)e.Item).Tag);
            if (o == null) return;
            if (o.GetType().Equals(typeof(Batch))) return;
            AtenuarNoTargetDrag(o, true);
            DoDragDrop(e.Item, DragDropEffects.Link);
            AtenuarNoTargetDrag(o);
            ResaltarTargetDrag(null, false);
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_DragOver(object sender, DragEventArgs e) {
            Point targetPoint = cTv.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = cTv.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (IsValidDrag(draggedNode, targetNode)) {
                e.Effect = DragDropEffects.Link;
                ResaltarTargetDrag(targetNode);
                }
            else { e.Effect = DragDropEffects.None; }//ResaltarTargetDrag(targetNode, false); 
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_AfterSelect(object sender, TreeViewEventArgs e) { if (InShowHideView) return; ShowObjectInEditor(); UpdateStBar(); lBack.Visible = (lBack.Links.Count > 0); }
        //_______________________________________________________________________________________________________________________
        private void cTv_KeyDown( object sender, KeyEventArgs e ) { KeyAccelerators(e); }
        //_______________________________________________________________________________________________________________________
        private void mCtxAdd_Click(object sender, EventArgs e) {
            if (!TestsHelper.Add((string)((ToolStripMenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxDelete_Click(object sender, EventArgs e) {
            if (!TestsHelper.Delete(((ToolStripMenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxDuplicate_Click( object sender, EventArgs e ) {
            if (!TestsHelper.Copy(((ToolStripMenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxUnhideSubItem_Click(object sender, EventArgs e) {
            if (!TestsHelper.deleteHiddenItem(((ToolStripMenuItem)sender).Tag)) return;
            lHiddenItems = null;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxSetSubItem_Click( object sender, EventArgs e ) {
            TestObjectBase[] objs = (TestObjectBase[])((ToolStripMenuItem)sender).Tag;
            if (!TestsHelper.CompleteDrop(objs[1], objs[0])) return;
            lHiddenItems = null;
            SaveObjectEdition(objs[0], false);
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxDeleteSubItem_Click(object sender, EventArgs e) {
            if (!TestsHelper.DeleteSubItem((object[])((ToolStripMenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxHide_Click(object sender, EventArgs e) {
            if (!TestsHelper.saveHiddenItem(((ToolStripMenuItem)sender).Tag)) return;
            lHiddenItems = null;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxRunTool_Click(object sender, EventArgs e) {
            string sTool = "" + ((ToolStripButton)sender).Tag;
            TestsHelper.LaunchTool(sTool, DBFile);
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxReorder_Click( object sender, EventArgs e ) { ShowReorderDlg((string)mCtxReorder.Tag); }
        //_______________________________________________________________________________________________________________________
        private void bDetail_Click(object sender, EventArgs e) {
            if (!TestsHelper.Detail(bDetail.Tag, out string value, out TestObjectBase o)) return;
            cEditDetail.Text = value;
            bSaveItem.Enabled = false;
            cEditDetail.Enabled = true;
            cEditDetail.Tag = o.Link;
            }
        //_______________________________________________________________________________________________________________________
        private void cEditDetail_TextChanged(object sender, EventArgs e) { bSaveItem.Enabled = true; }
        //_______________________________________________________________________________________________________________________
        private void bSaveItem_Click(object sender, EventArgs e) { SaveObjectEdition(TestObjectBase.LinkedObject(cEditDetail.Tag)); }
        //_______________________________________________________________________________________________________________________
        private void bSave_Click( object sender, EventArgs e ) { SaveObjectEdition(TestObjectBase.LinkedObject(bSave.Tag)); }
        //_______________________________________________________________________________________________________________________
        private void EditItem_Changed( object sender, EventArgs e ) { bSave.Enabled = true; }
        //_______________________________________________________________________________________________________________________
        private void bCloseDB_Click( object sender, EventArgs e ) {
            TestsHelper.Close();
            cTv.Enabled = bCloseDB.Enabled = bRunTest.Enabled = bCheckIntegrity.Enabled = bEditVariables.Enabled = bShowHide.Enabled = lFind.Enabled = cFind.Enabled = false;
            foreach (TreeNode t in cTv.Nodes) t.Nodes.Clear();
            ResetEditionPanel(true);
            Text = (TestsHelper.IsAdministrator ? "Administrator: " : "") + "FIM Test Configurator";
            ShowRecentPanel();
            ShowLasError();
            }
        //_______________________________________________________________________________________________________________________
        private void CheckButton_CheckedChanged( object sender, EventArgs e ) { bSave.Enabled = true; }
        //_______________________________________________________________________________________________________________________
        private void bCheckIntegrity_Click( object sender, EventArgs e ) { CheckTestsIntegrity(); }
        //_______________________________________________________________________________________________________________________
        private void bEditVariables_Click( object sender, EventArgs e ) { EditVariables(); }
        //_______________________________________________________________________________________________________________________
        private void bShowHide_Click(object sender, EventArgs e) {
            if (bShowHide.Text == "Show/Hide View") {
                bShowHide.Text = "Edition View";
                bOpenDB.Enabled = bCloseDB.Enabled = bRunTest.Enabled = bCheckIntegrity.Enabled = bEditVariables.Enabled = bExtTool1.Enabled = bExtTool2.Enabled = bExtTool3.Enabled = lFind.Enabled = cFind.Enabled = false;
                ResetEditionPanel(true);
                cTv.CheckBoxes = true;
                cTv.Enabled = false;
                ShowAllObjects(true);
                cTv.Enabled = true;
                }
            else {
                ModifyNodeByLink(cTv.Nodes, (n) => { if (n.Parent == null) return false; if (n.Checked) TestsHelper.deleteHiddenItem(n.Tag); else TestsHelper.saveHiddenItem(n.Tag); return false; });
                bShowHide.Text = "Show/Hide View";
                bOpenDB.Enabled = bCloseDB.Enabled = bRunTest.Enabled = bCheckIntegrity.Enabled = bEditVariables.Enabled = bExtTool1.Enabled = bExtTool2.Enabled = bExtTool3.Enabled = lFind.Enabled = cFind.Enabled = true;
                cTv.CheckBoxes = false;
                lHiddenItems = null;
                ShowAllObjects(true);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void cLstRecent_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (cLstRecent.SelectedItems.Count != 1) return;
            OpenDB((string)cLstRecent.SelectedItems[0].Tag);
            cPanelRecent.Visible = false;
            }
        //_______________________________________________________________________________________________________________________
        private void ShowAllObjects(bool ForceExpand = false) {
            if (!TestsHelper.Initialized) return;
            if (lHiddenItems == null) lHiddenItems = TestsHelper.getAllHidden();

            string PrevSelection = "" + cTv.SelectedNode?.Tag;
            string PrevNextSelection = "" + cTv.SelectedNode?.PrevNode?.Tag;

            foreach (TreeNode t in cTv.Nodes) t.Nodes.Clear();
            if (InShowHideView) foreach (TreeNode t in cTv.Nodes) t.Checked = true;
            long numObjs = 0;
            foreach (var g in TestsHelper.getAllBatches()) numObjs += ShowAnObject(cTv.Nodes["tvBatches"], g); 
            foreach (var g in TestsHelper.getAllGroups()) numObjs += ShowAnObject(cTv.Nodes["tvGroups"], g); 
            foreach (var g in TestsHelper.getAllTests()) numObjs += ShowAnObject(cTv.Nodes["tvTests"], g); 
            foreach (var g in TestsHelper.getAllConfig()) numObjs += ShowAnObject(cTv.Nodes["tvConfigs"], g); 
            foreach (var g in TestsHelper.getAllMAgent()) numObjs += ShowAnObject(cTv.Nodes["tvMAs"], g);
            foreach (var g in TestsHelper.getAllSource()) numObjs += ShowAnObject(cTv.Nodes["tvSources"], g); 
            foreach (var g in TestsHelper.getAllInputSet()) numObjs += ShowAnObject(cTv.Nodes["tvInputSets"], g);
            foreach (var g in TestsHelper.getAllOutputSet()) numObjs += ShowAnObject(cTv.Nodes["tvOutputSets"], g);
            foreach (var g in TestsHelper.getAllScript()) numObjs += ShowAnObject(cTv.Nodes["tvScripts"], g);

            if (ForceExpand) cTv.ExpandAll(); //foreach (TreeNode t in cTv.Nodes) t.Expand();//

            ResetEditionPanel();
            if (!TestObjectBase.IsValidLink(PrevSelection) || (TestObjectBase.LinkedObject(PrevSelection) != null && (!lHiddenItems.Contains(PrevSelection) || InShowHideView))) {
                if (!string.IsNullOrWhiteSpace(PrevSelection)) ModifyNodeByLink(cTv.Nodes, PrevSelection, ( x ) => cTv.SelectedNode = x);
                }
            else {
                if (!string.IsNullOrWhiteSpace(PrevNextSelection)) ModifyNodeByLink(cTv.Nodes, PrevNextSelection, ( x ) => cTv.SelectedNode = x);
                }
            cTv.SelectedNode?.EnsureVisible();
            UpdateStBar(numObjs);
            ShowLasError();
            }
        //_______________________________________________________________________________________________________________________
        private void ShowLasError() {
            string msg = TestsHelper.LastErrorMessages;
            bError.ToolTipText = msg;
            cErrorSep.Visible = bError.Visible = !string.IsNullOrWhiteSpace(msg);
            }
        //_______________________________________________________________________________________________________________________
        private long ShowAnObject(TreeNode t, TestObjectBase g) {
            if (lHiddenItems.Contains(g.Link) && !InShowHideView) return 1;
            TreeNode ng = t.Nodes.Add(g.Name);
            ng.Tag = g.Link;
            g.CheckResult();
            if (g.ContainsErrors) ng.ToolTipText = g.ErrorsMessages;
            ng.SelectedImageIndex = 9;
            ng.ImageIndex = t.ImageIndex;
            if (InShowHideView) { ng.Checked = !lHiddenItems.Contains(g.Link); if (!ng.Checked) t.Checked = false; }
            return 1;
            }
        //_______________________________________________________________________________________________________________________
        private void ModifyNodeByLink(TreeNodeCollection nodes, string sLnk, Action<TreeNode> modify) {
            foreach (TreeNode n in nodes) {
                if (sLnk.Equals("" + n.Tag)) { modify(n); return; }
                ModifyNodeByLink(n.Nodes, sLnk, modify);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void ModifyNodeByLink(TreeNodeCollection nodes, Func<TreeNode, bool> modify) {
            foreach (TreeNode n in nodes) {
                if (modify(n)) return;
                ModifyNodeByLink(n.Nodes, modify);
                }
            }
        //_______________________________________________________________________________________________________________________
        private TreeNode SearchNodeByLink( TreeNodeCollection nodes, string sLnk ) {
            foreach (TreeNode n in nodes) {
                if (sLnk.Equals("" + n.Tag)) { return n; }
                var r = SearchNodeByLink(n.Nodes, sLnk);
                if (r != null) return r;
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        private void ConfigureCtxMenu( TreeNode node, Point p ) {
            if (node.Parent == null) {
                mCtxAdd.Text = "New " + node.Text.Replace("es ", "").Replace("s ", "").Trim(); mCtxAdd.Visible = true; mCtxAdd.Tag = node.Name;
                mCtxDuplicate.Visible = false;
                mCtxSep.Visible = false;
                mCtxDelete.Visible = false;
                mCtxHide.Visible = false;
                mCtxReorder.Visible = false;
                ConfigureCtxMenuSubItems(null, (string)node.Tag);
                }
            else if (node.Tag != null) {
                mCtxDuplicate.Text = "Copy " + node.Text; mCtxDuplicate.Visible = true; mCtxDuplicate.Tag = node.Tag;
                mCtxDelete.Text = "Delete " + node.Text; mCtxDelete.Visible = true; mCtxDelete.Tag = node.Tag;
                mCtxHide.Text = "Hide " + node.Text; mCtxHide.Visible = true; mCtxHide.Tag = node.Tag;
                mCtxSep.Visible = true;
                mCtxAdd.Visible = false;
                mCtxReorder.Visible = (TestObjectBase.TypeOfLink(node.Tag).Equals(typeof(Batch)) || TestObjectBase.TypeOfLink(node.Tag).Equals(typeof(Group)));
                mCtxReorder.Text = "Reorder " + (TestObjectBase.TypeOfLink(node.Tag).Equals(typeof(Batch)) ? "Groups" : "Tests");
                mCtxReorder.Tag = node.Tag;
                mCtxReorder.Enabled = false;
                ConfigureCtxMenuSubItems(TestObjectBase.LinkedObject(node.Tag));
                }
            else return;
            ctxMenu.Show(cTv, p);
            }
        //_______________________________________________________________________________________________________________________
        private void ConfigureCtxMenuSubItems( object o, string sTipo = "" ) {
            mCtxSep2.Visible = false;
            //elimina menús de una ejecución anterior
            for (int i = ctxMenu.Items.Count - 1; i > 0; i--) if (ctxMenu.Items[i].Name.StartsWith("Remove")) ctxMenu.Items.RemoveAt(i);
            for (int i = ctxMenu.Items.Count - 1; i > 0; i--) if (ctxMenu.Items[i].Name.StartsWith("Unhide")) ctxMenu.Items.RemoveAt(i);
            if (o == null && lHiddenItems != null) {
                foreach (string lnk in lHiddenItems) AddMenuUnhide(lnk, sTipo);
                }
            if (o == null) return;

            // añade nuevos menus
            if (o.GetType().Equals(typeof(Batch))) {
                foreach (Group t in ((Batch)o).Groups) AddMenuRemove((Batch)o, t);
                mCtxReorder.Enabled = (((Batch)o).Groups.Count > 1);
                }
            if (o.GetType().Equals(typeof(Group))) {
                foreach (Test t in ((Group)o).Tests) AddMenuRemove((Group)o, t);
                mCtxReorder.Enabled = (((Group)o).Tests.Count > 1);
                }
            if (o.GetType().Equals(typeof(Test))) {
                if (((Test)o).Config != null) AddMenuRemove((Test)o, ((Test)o).Config);
                if (((Test)o).Script != null) AddMenuRemove((Test)o, ((Test)o).Script);
                if (((Test)o).InputSet != null) AddMenuRemove((Test)o, ((Test)o).InputSet);
                if (((Test)o).SourceManagementAgent != null) AddMenuRemove((Test)o, ((Test)o).SourceManagementAgent);
                foreach (OutputSet t in ((Test)o).OutputSets) AddMenuRemove((Test)o, t);
                }
            if (o.GetType().Equals(typeof(OutputSet))) {
                if (((OutputSet)o).DestinationManagementAgent != null) AddMenuRemove((OutputSet)o, ((OutputSet)o).DestinationManagementAgent);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void AddMenuUnhide(string lnk, string sTipo) {
            if (!lnk.StartsWith(sTipo)) return;
            var t = TestObjectBase.LinkedObject(lnk);
            if (t == null) return;
            var oMnu = new ToolStripMenuItem();
            oMnu.Name = "Unhide" + ctxMenu.Items.Count;
            oMnu.Tag = lnk;
            oMnu.Text = "Unhide " + t.Name;
            oMnu.Click += new EventHandler(mCtxUnhideSubItem_Click);
            ctxMenu.Items.Add(oMnu);
            mCtxSep2.Visible = true;
            }
        //_______________________________________________________________________________________________________________________
        private void AddMenuRemove(TestObjectBase oBase, TestObjectBase oChild) {
            var oMnu = new ToolStripMenuItem();
            oMnu.Name = "Remove" + ctxMenu.Items.Count;
            oMnu.Tag = new object[] { oBase, oChild };
            oMnu.Text = "Remove "+ oChild.Alias + "[" + oChild.Name + "] from "+ oBase.Alias;
            oMnu.Click += new EventHandler(mCtxDeleteSubItem_Click);
            ctxMenu.Items.Add(oMnu);
            mCtxSep2.Visible = true;
            }
        //_______________________________________________________________________________________________________________________
        private bool IsValidDrag( TreeNode draggedNode, TreeNode targetNode ) {
            if (draggedNode == null || targetNode?.Parent == null) return false;
            Type torg = TestObjectBase.TypeOfLink(draggedNode.Tag);
            Type tdst = TestObjectBase.TypeOfLink(targetNode.Tag);
            StBar1.Text = "" + torg.ToString() + "->" + tdst.ToString();
            Application.DoEvents();
            return TestsHelper.IsValidDrag(torg, tdst);
            }
        //_______________________________________________________________________________________________________________________
        private void SaveObjectDetail( TestObjectBase o, bool refreshTv = true ) {
            if (o == null) return;
            if (!bSaveItem.Enabled) return;
            TestsHelper.SaveObjectDetail(o, cEditDetail.Text);
            bSaveItem.Enabled = false;
            if(refreshTv) ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void SaveObjectEdition( TestObjectBase o, bool refreshTv = true ) {
            if (o == null) return;
            SaveObjectDetail(TestObjectBase.LinkedObject(cEditDetail.Tag), false);
            TestsHelper.UpdateName(o, cName.Text);
            if (o.GetType().Equals(typeof(Config))) TestsHelper.saveConfigDesc((Config)o, cDetail.Text);
            if (o.GetType().Equals(typeof(Test))) TestsHelper.saveTest((Test)o, cDelta.Checked ? 1 : 0, cCommit.Checked ? 1 : 0);
            if (o.GetType().Equals(typeof(Source))) TestsHelper.saveSource((Source)o, cName.Text, cServer.Text, cUser.Text, cPassword.Text, cAuthType.Text);
            if (o.GetType().Equals(typeof(InputSet))) TestsHelper.saveInputSetDN((InputSet)o, cDetail.Text);
            if (o.GetType().Equals(typeof(OutputSet))) TestsHelper.saveOutputSetType((OutputSet)o, cbDetail.Text);
            if (o.GetType().Equals(typeof(Script))) TestsHelper.saveScriptDesc((Script)o, cDetail.Text);
            bSaveItem.Enabled = false;
            if (refreshTv) ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void AtenuarNoTargetDrag(TestObjectBase org, bool op = false) {
            Type torg = org.GetType();
            if (torg.Equals(typeof(Test)))
                ModifyNodeByLink(cTv.Nodes, (n) => { if (n.Parent != null && !((string)(n.Tag)).StartsWith("g")) n.ForeColor = op ? Color.Gray : cTv.Nodes[0].ForeColor; return false; });
            if (torg.Equals(typeof(Config)) || torg.Equals(typeof(Script)) || torg.Equals(typeof(InputSet)) || torg.Equals(typeof(OutputSet)))
                ModifyNodeByLink(cTv.Nodes, (n) => { if (n.Parent != null && !((string)(n.Tag)).StartsWith("t")) n.ForeColor = op ? Color.Gray : cTv.Nodes[0].ForeColor; return false; });
            if (torg.Equals(typeof(ManagementAgentInfo)))
                ModifyNodeByLink(cTv.Nodes, (n) => { if (n.Parent != null && !((string)(n.Tag)).StartsWith("o") && !((string)(n.Tag)).StartsWith("t")) n.ForeColor = op ? Color.Gray : cTv.Nodes[0].ForeColor; return false; });
            if (torg.Equals(typeof(Source)))
                ModifyNodeByLink(cTv.Nodes, (n) => { if (n.Parent != null && !((string)(n.Tag)).StartsWith("m")) n.ForeColor = op ? Color.Gray : cTv.Nodes[0].ForeColor; return false; });
            }
        //_______________________________________________________________________________________________________________________
        private void ResaltarTargetDrag( TreeNode tn, bool op = true ) {
            if (mDragAndDropTarget != null && mDragAndDropTarget != tn) mDragAndDropTarget.ForeColor = cTv.Nodes[0].ForeColor; // quita marca
            mDragAndDropTarget = tn;
            if (tn == null) return;
            if (op) tn.ForeColor = Color.Red; //pone marca
            else tn.ForeColor = cTv.Nodes[0].ForeColor; // quita marca
            }
        //_______________________________________________________________________________________________________________________
        private void ResetEditionPanel( bool full = false ) {
            cEditDetail.Text = "";
            cEditDetail.ReadOnly = false;

            bDetail.Tag = bSave.Tag = null;

            cPanelEdit.Visible = cEditDetail.Enabled = cDetail.Visible = cDelta.Visible = cCommit.Visible = false;
            bSave.Enabled = bSaveItem.Enabled = false;
            bDetail.Visible = cbDetail.Visible = false;
            cServer.Visible = cUser.Visible = cPassword.Visible = cAuthType.Visible = false;
            lDetail.Visible = lUser.Visible = lPassword.Visible = lAuthType.Visible = lDesc.Visible = false;
            
            lName.Visible = cName.Visible = !full;
            bSave.Visible = bSaveItem.Visible = !full;
            }
        //_______________________________________________________________________________________________________________________
        private void FillTextBoxContentWithLinks( RichTextBoxEx tb, TestObjectBase o ) {
            if (o.AdditionalLinkedInfo.Count == 0) return;
            tb.Clear();
            string orgTxt = o.AdditionalInfo();
            int start = 0;
            foreach (var t in o.AdditionalLinkedInfo) {
                tb.AppendText(orgTxt.Substring(start, t.Item1 - start));
                tb.InsertLink(orgTxt.Substring(t.Item1, t.Item2), t.Item3);
                start = t.Item1 + t.Item2;
                }
            tb.AppendText(orgTxt.Substring(start));
            }
        //_______________________________________________________________________________________________________________________
        private void ShowObjectInEditor() {
            ResetEditionPanel();

            if (cTv.SelectedNode == null || cTv.SelectedNode.Parent == null || cTv.SelectedNode.Tag == null) return;
            TestObjectBase o = TestObjectBase.LinkedObject(cTv.SelectedNode.Tag);
            if (o == null) return;

            cName.Text = o.Name;
            lDesc.Text = o.AdditionalInfo();
            // Quitados los enlaces con LinkLabel porque lDesc.Links está limitado a 32 enlaces.
            //foreach (var t in o.AdditionalLinkedInfo) { if (lDesc.Links.Count > 30) break; lDesc.Links.Add(t.Item1, t.Item2, t.Item3); }
            FillTextBoxContentWithLinks(lDesc, o);

            if (o.GetType().Equals(typeof(Batch))) {
                lName.Text = "Batch:";
                lDesc.Visible = true;
                }
            else if (o.GetType().Equals(typeof(Group))) {
                lName.Text = "Group:";
                lDesc.Visible = true;
                }
            else if (o.GetType().Equals(typeof(Test))) {
                lName.Text = "Test:";
                cDelta.Checked = ((Test)o).Delta; cDelta.Visible = true;
                cCommit.Checked = ((Test)o).Commit; cCommit.Visible = true;
                lDesc.Visible = true;
                bDetail.Visible = true;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(Config))) {
                lName.Text = "File Name:";
                bDetail.Visible = true;
                lDetail.Text = "Description:";
                cDetail.Text = ((Config)o).Desc;
                cDetail.Visible = lDetail.Visible = true;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(Script))) {
                lName.Text = "Name:";
                bDetail.Visible = true;
                lDetail.Text = "Description:";
                cDetail.Text = ((Script)o).Desc;
                cDetail.Visible = lDetail.Visible = true;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(ManagementAgentInfo))) {
                lName.Text = "MA:";
                lDesc.Visible = true;
                }
            else if (o.GetType().Equals(typeof(Source))) {
                lName.Text = "Name:";
                cServer.Visible = cUser.Visible = cPassword.Visible = cAuthType.Visible = true;
                lUser.Visible = lPassword.Visible = lAuthType.Visible = true;
                lDetail.Text = "Server:";
                lDetail.Visible = true;
                cServer.Text = ((Source)o).Server;
                cUser.Text = ((Source)o).User;
                cPassword.Text = ((Source)o).Password;
                cAuthType.Text = ((Source)o).AuthType;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(InputSet))) {
                lName.Text = "Input Set:";
                lDetail.Visible = true; lDetail.Text = "DN:";
                cDetail.Visible = true; cDetail.Text = ((InputSet)o).DistinguishedName;
                bDetail.Visible = true; bDetail.Tag = o.Link;

                lDesc.Visible = true;

                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(OutputSet))) {
                lName.Text = "Output Set:";
                lDetail.Visible = true; lDetail.Text = "Type:";
                bDetail.Visible = true; bDetail.Tag = o.Link;
                cbDetail.Visible = true; cbDetail.Items.Clear(); cbDetail.Items.AddRange(TestsHelper.getOutputSetTypes());

                for (int i = 0; i < cbDetail.Items.Count; i++)
                    if (cbDetail.Items[i].ToString().iEquals(((OutputSet)o).Type)) cbDetail.SelectedIndex = i;

                lDesc.Visible = true;

                bDetail_Click(null, null);
                }
            else return;
            bSave.Tag = o.Link;
            cPanelEdit.Visible = true;
            bSave.Enabled = false;
            }
        //_______________________________________________________________________________________________________________________
        private void CheckTestsIntegrity() {
            ResetEditionPanel(true);
            cEditDetail.Text = "";
            cEditDetail.ReadOnly = false;
            cEditDetail.Enabled = true;
            // Clean broken links
            TestsHelper.cleanBrokenLinks();
            // Valida batches
            foreach (Batch o in TestsHelper.getAllBatches()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, ( x ) => x.ImageIndex = 8);
                }
            // Valida groups
            foreach (Group o in TestsHelper.getAllGroups()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida tests
            foreach (Test o in TestsHelper.getAllTests()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida inputsets
            foreach (InputSet o in TestsHelper.getAllInputSet()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida outputsets
            foreach (OutputSet o in TestsHelper.getAllOutputSet()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida ManagementAgentInfo
            foreach (ManagementAgentInfo o in TestsHelper.getAllMAgent()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida Source
            foreach (Source o in TestsHelper.getAllSource()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida configs
            foreach (Config o in TestsHelper.getAllConfig()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, (x) => x.ImageIndex = 8);
                }
            // Valida scripts
            foreach (Script o in TestsHelper.getAllScript()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Nodes, o.Link, ( x ) => x.ImageIndex = 8);
                }
            cEditDetail.Visible = true;
            cPanelEdit.Visible = true;
            bSave.Enabled = false;
            }
        //_______________________________________________________________________________________________________________________
        private void OpenDB() {
            string DbFile =Utilities.SelectFile("Browse DB Files", "db files (*.db)|*.db");
            if (DbFile != null) OpenDB(DbFile);
            }
        //_______________________________________________________________________________________________________________________
        private void OpenDB( string sDBName ) {
            DBFile = sDBName;
            Text = (TestsHelper.IsAdministrator ? "Administrator: " : "") + "FIM Test Configurator [" + DBFile + "]";
            AddToRecentFiles(DBFile);
            TestsHelper.Init(DBFile);
            cTv.Enabled = bCloseDB.Enabled = bRunTest.Enabled = bCheckIntegrity.Enabled = bEditVariables.Enabled = bShowHide.Enabled = lFind.Enabled = cFind.Enabled = true;
            cPanelRecent.Visible = false;
            lHiddenItems = null;
            foreach (TreeNode t in cTv.Nodes) t.Nodes.Clear();
            ShowAllObjects(true);
            cTv.SelectedNode = cTv.Nodes[0];
            }
        //_______________________________________________________________________________________________________________________
        private void MsgBar(string sTexto, bool PrimeraSeccion = true) {
            if (PrimeraSeccion) StBar1.Text = sTexto;
            else StBar2.Text = sTexto;
            }
        //_______________________________________________________________________________________________________________________
        private void UpdateStBar(long toltalObjs = -1) {
            if (cTv.SelectedNode != null) {
                long count = lHiddenItems.Where(x => x.StartsWith((string)cTv.SelectedNode.Tag)).Count();
                if (cTv.SelectedNode == null) MsgBar("");
                else if (cTv.SelectedNode.Parent == null) MsgBar(" Visible " + cTv.SelectedNode.Text.Trim() + ": " + cTv.SelectedNode.Nodes.Count + "   Hidden " + cTv.SelectedNode.Text.Trim() + ": " + count);
                else MsgBar(cTv.SelectedNode.Parent.Text.Trim() + ": " + cTv.SelectedNode.Text);
                }
            if (toltalObjs >= 0) MsgBar("Total: " + toltalObjs + "    Visible: " + (toltalObjs - lHiddenItems.Count) + "    Hidden: " + lHiddenItems.Count, false);
            }
        //_______________________________________________________________________________________________________________________
        private void ShowRecentPanel() {
            List<FileInfo> lRecent = new List<FileInfo>();
            LoadRecentFiles(lRecent);
            if (lRecent.Count == 0) return;
            cLstRecent.Items.Clear();
            cPanelRecent.Visible= true;
            lRecent = lRecent.OrderByDescending(f => f.LastWriteTime).ToList();
            foreach (FileInfo fi in lRecent) {
                var i = cLstRecent.Items.Add(fi.Name);
                i.SubItems.Add(fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                i.SubItems.Add(fi.DirectoryName);
                i.Tag = fi.FullName;
                }
            foreach (ColumnHeader c in cLstRecent.Columns) c.Width = -2;
            }
        //_______________________________________________________________________________________________________________________
        private void LoadRecentFiles(List<FileInfo> lRecent) {
            long i = 0;
            lRecent.Clear();
            while (!string.IsNullOrWhiteSpace(ConfigurationHelper.GetSetting("Recent" + i))) {
                string sPath = ConfigurationHelper.GetSetting("Recent" + i++);
                if (!File.Exists(sPath)) continue;
                foreach (FileInfo fi in lRecent) if (fi.FullName.iEquals(sPath)) continue;
                lRecent.Add(new FileInfo(sPath));
                }
            }
        //_______________________________________________________________________________________________________________________
        private void AddToRecentFiles(string sPath) {
            List<FileInfo> lRecent = new List<FileInfo>();
            LoadRecentFiles(lRecent);
            if (!File.Exists(sPath)) return;
            foreach (FileInfo fi in lRecent) if (fi.FullName.iEquals(sPath)) return;
            lRecent.Add(new FileInfo(sPath));
            long i = 0;
            foreach (FileInfo fi in lRecent) ConfigurationHelper.SetSetting("Recent" + i++, fi.FullName);
            }
        //_______________________________________________________________________________________________________________________
        private void ShowChildObjects( TestObjectBase o, ListView cLstNewOrder ) {
            cLstNewOrder.SmallImageList = cImg;
            if (o.GetType().Equals(typeof(Batch))) {
                foreach (Group t in ((Batch)o).Groups) {
                    ListViewItem i = new ListViewItem(t.Name, 0);
                    i.Tag = t.Id;
                    cLstNewOrder.Items.Add(i);
                    }
                }
            if (o.GetType().Equals(typeof(Group))) {
                foreach (Test t in ((Group)o).Tests) {
                    ListViewItem i = new ListViewItem(t.Name, 1);
                    i.Tag = t.Id;
                    cLstNewOrder.Items.Add(i);
                    }
                }
            if (cLstNewOrder.Items.Count == 0) return;
            cLstNewOrder.Items[0].Selected = true;
            foreach (ColumnHeader c in cLstNewOrder.Columns) c.Width = -2;
            }
        //_______________________________________________________________________________________________________________________
        private void ApplyNewOrder( TestObjectBase o, ListView cLstNewOrder ) {
            foreach (ListViewItem i in cLstNewOrder.Items) {
                long order = i.Index;
                long ID = (long)i.Tag;
                if (o.GetType().Equals(typeof(Batch))) TestsHelper.setBatchGroupOrder((Batch)o, ID, order);
                if (o.GetType().Equals(typeof(Group))) TestsHelper.setGroupTestOrder((Group)o, ID, order);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void LinkClicked(LinkLabel.Link link, string sLnk, bool isBack = false) {
            if (TestObjectBase.IsActionLink(sLnk)) { ProcessActionLink(link, sLnk); return; }
            TreeNode t = SearchNodeByLink(cTv.Nodes, sLnk);
            if (t == null && link != null) { link.Enabled = false; return; }
            // el tag de lBack contiene una lista de sitios por los que ha pasado
            List<string> lstBack = (List<string>)lBack.Tag;

            lBack.Links.Clear();
            if (isBack) {
                if (lstBack.Contains(sLnk)) lstBack.Remove(sLnk);
                if (lstBack.Count > 0) {
                    lBack.Links.Add(0, lBack.Text.Length, lstBack[lstBack.Count - 1]);
                    cToolTip.SetToolTip(lBack, "Go back to: " + TestObjectBase.LinkedObject(lstBack[lstBack.Count - 1]).Name);
                    }
                }
            else {
                lstBack.Add("" + cTv.SelectedNode.Tag);
                lBack.Links.Add(0, lBack.Text.Length, cTv.SelectedNode.Tag);
                cToolTip.SetToolTip(lBack, "Go back to: " + TestObjectBase.LinkedObject(cTv.SelectedNode.Tag).Name);
                }
            cTv.SelectedNode = t; // al actualizar el nodo activo ya se actualiza la visibilidad de lBack //lBack.Visible = (lBack.Links.Count > 0);
            }
        //_______________________________________________________________________________________________________________________
        private void ProcessActionLink( LinkLabel.Link link, string sLnk ) {
            string[] lLnks = sLnk.Split(':');
            var o1 = TestObjectBase.LinkedObject(lLnks[1]);
            var o2 = TestObjectBase.LinkedObject(lLnks[2]);
            if (TestObjectBase.IsRemoveActionLink(sLnk)) {
                if (!TestsHelper.DeleteSubItem(new object[] { o1, o2 })) return;
                SaveObjectEdition(o1, false);
                ShowAllObjects();
                }
            else if (TestObjectBase.IsAddActionLink(sLnk)||TestObjectBase.IsSetActionLink(sLnk)) {
                ctxMenuEdition.Items.Clear();
                List<string> connected = ConnectedSubItems(o1);//obtiene la lista de subitems conectados para no ofrecerlos en el menú
                string sTxt = TestObjectBase.IsAddActionLink(sLnk) ? "Add " : "Set "; 
                switch (lLnks[2]) {
                    case "groups": foreach (var g in TestsHelper.getAllGroups()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "test": foreach (var g in TestsHelper.getAllTests()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "config": foreach (var g in TestsHelper.getAllConfig()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "managementagent": foreach (var g in TestsHelper.getAllMAgent()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "source": foreach (var g in TestsHelper.getAllSource()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "inputset": foreach (var g in TestsHelper.getAllInputSet()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "outputset": foreach (var g in TestsHelper.getAllOutputSet()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    case "script": foreach (var g in TestsHelper.getAllScript()) AddItemToMenuEditon(o1, g, sTxt, connected); break;
                    }
                if (ctxMenuEdition.Items.Count > 0) ctxMenuEdition.Show(Cursor.Position);
                else link.Enabled = false;
                }
            }
        //_______________________________________________________________________________________________________________________
        private void AddItemToMenuEditon( TestObjectBase p, TestObjectBase t, string sTxt, List<string> connected ) {
            if (connected.Contains(t.Link)) return;
            var oMnu = new ToolStripMenuItem();
            oMnu.Name = sTxt.Trim() + ctxMenu.Items.Count;
            oMnu.Tag = new TestObjectBase[] { p, t };
            oMnu.Text = sTxt + t.Name;
            oMnu.Click += new EventHandler(mCtxSetSubItem_Click);
            ctxMenuEdition.Items.Add(oMnu);
            }
        //_______________________________________________________________________________________________________________________
        private List<string> ConnectedSubItems( TestObjectBase o ) {
            List<string> r = new List<string>();
            // añade nuevos menus
            if (o.GetType().Equals(typeof(Batch))) {
                foreach (Group t in ((Batch)o).Groups) r.Add(t.Link);
                }
            if (o.GetType().Equals(typeof(Group))) {
                foreach (Test t in ((Group)o).Tests) r.Add(t.Link);
                }
            if (o.GetType().Equals(typeof(Test))) {
                if (((Test)o).Config != null) r.Add(((Test)o).Config.Link);
                if (((Test)o).Script != null) r.Add(((Test)o).Script.Link);
                if (((Test)o).InputSet != null) r.Add(((Test)o).InputSet.Link); 
                if (((Test)o).SourceManagementAgent != null) r.Add(((Test)o).SourceManagementAgent.Link);
                foreach (OutputSet t in ((Test)o).OutputSets) r.Add(t.Link);
                }
            if (o.GetType().Equals(typeof(OutputSet))) {
                if (((OutputSet)o).DestinationManagementAgent != null) r.Add(((OutputSet)o).DestinationManagementAgent.Link); 
                }
            return r;
            }
        //_______________________________________________________________________________________________________________________
        //PARA SELECCIONAR TIPO DE LETRA
        private void FillFontList() {
            using (InstalledFontCollection fontsCollection = new InstalledFontCollection()) {
                FontFamily[] fontFamilies = fontsCollection.Families;
                foreach (FontFamily font in fontFamilies) cLstFonts.Items.Add(font.Name);
                }
            cLstFonts.Visible = true;
            }
        //_______________________________________________________________________________________________________________________
        private void SelectFindTextInEditor(TextBox t) {
            t.SelectionStart = t.Text.iIndexOf(cFind.Text);
            t.SelectionLength = cFind.Text.Length;
            t.Focus();
            if (t.Multiline) t.ScrollToCaret();
            string s = t.Text.ToLower();
            MsgBar(cTv.SelectedNode.Parent.Text.Trim() + ": " + cTv.SelectedNode.Text + ". Occurrences found: " + (s.Length - s.Replace(cFind.Text.ToLower(), "").Length) / cFind.Text.Length + ".");
            }
        //_______________________________________________________________________________________________________________________
        private void SearchText(TreeNode current = null) {
            int nodosNoBuscados = 0;
            if (string.IsNullOrEmpty(cFind.Text)) return;
            if (current == null) current = cTv.SelectedNode;
            bool empiezaABuscar = false;
            foreach (TreeNode n in cTv.Nodes) {
                if (current == n) empiezaABuscar = true;
                foreach (TreeNode sn in n.Nodes) {
                    if (current == sn) { empiezaABuscar = true; continue; }
                    if (!empiezaABuscar) { nodosNoBuscados++; continue; }
                    TestObjectBase o = TestObjectBase.LinkedObject(sn.Tag);

                    // cubre el nombre de todos y además Batch, Group y ManagementAgentInfo
                    if (o.Name.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cName); return; }

                    if (o.GetType().Equals(typeof(Config))) {
                        if (((Config)o).Desc.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cDetail); return; }
                        if (((Config)o).FileContent.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cEditDetail); return; }
                        }
                    if (o.GetType().Equals(typeof(Script))) {
                        if (((Script)o).Desc.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cDetail); return; }
                        if (((Script)o).FileContent.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cEditDetail); return; }
                        }
                    if (o.GetType().Equals(typeof(Test))) {
                        if (((Test)o).Detail.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cEditDetail); return; }
                        }
                    if (o.GetType().Equals(typeof(Source))) {
                        if (((Source)o).Server.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cServer); return; }
                        if (((Source)o).User.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cUser); return; }
                        if (((Source)o).AuthType.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cAuthType); return; }
                        }
                    if (o.GetType().Equals(typeof(InputSet))) {
                        if (((InputSet)o).DistinguishedName.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cDetail); return; }
                        if (((InputSet)o).Detail.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cEditDetail); return; }
                        }
                    if (o.GetType().Equals(typeof(OutputSet))) {
                        if (((OutputSet)o).Detail.iIndexOf(cFind.Text) >= 0) { cTv.SelectedNode = sn; SelectFindTextInEditor(cEditDetail); return; }
                        }
                    }
                }
            cTv.Focus();
            if (nodosNoBuscados > 0) {
                if (MessageBox.Show("Text not found [" + cFind.Text + "].\n¿Continue search from benining?", "Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
                SearchText(cTv.Nodes[0]);
                }
            else MessageBox.Show("Text not found [" + cFind.Text + "].", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        //_______________________________________________________________________________________________________________________
        private void ShowReorderDlg(string sLnk) {
            using (frmOrder f = new frmOrder()) {
                ShowChildObjects(TestObjectBase.LinkedObject(sLnk), f.cLst);
                if (f.cLst.Items.Count > 0 && f.ShowDialog(this) == DialogResult.OK) {
                    ApplyNewOrder(TestObjectBase.LinkedObject(sLnk), f.cLst);
                    ShowAllObjects();
                    }
                }
            }
        //_______________________________________________________________________________________________________________________
        private void KeyAccelerators(KeyEventArgs e) {
            if (cTv.SelectedNode == null) return;
            string tag = (string)cTv.SelectedNode.Tag;
            if (e.KeyCode == Keys.Delete) { if (!TestsHelper.Delete(tag)) return; }
            else if (e.KeyCode == Keys.Insert) { if (!TestsHelper.Add((cTv.SelectedNode.Parent == null) ? cTv.SelectedNode.Name : cTv.SelectedNode.Parent.Name)) return; }
            else if (e.KeyCode == Keys.D && e.Modifiers == Keys.Control) { if (!TestsHelper.Copy(tag)) return; }
            else if (e.KeyCode == Keys.H && e.Modifiers == Keys.Control) { if (!TestsHelper.saveHiddenItem(tag)) return; lHiddenItems = null; }
            else if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control && TestObjectBase.IsValidLink(tag)) { ShowReorderDlg(tag); return; }
            else return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void EditVariables() {
            using (frmVariables f = new frmVariables()) f.ShowDialog(this);
            }
        //_______________________________________________________________________________________________________________________
        private void LoadExternalTools() {
            string sTool;
            sTool = ConfigurationHelper.GetSetting("ExtTool1");
            if (!string.IsNullOrEmpty(sTool)) {
                bExtTool1.Text = sTool.Split('|')[0];
                bExtTool1.Visible = true;
                }
            sTool = ConfigurationHelper.GetSetting("ExtTool2");
            if (!string.IsNullOrEmpty(sTool)) {
                bExtTool2.Text = sTool.Split('|')[0];
                bExtTool2.Visible = true;
                }
            sTool = ConfigurationHelper.GetSetting("ExtTool3");
            if (!string.IsNullOrEmpty(sTool)) {
                bExtTool3.Text = sTool.Split('|')[0];
                bExtTool3.Visible = true;
                }
            }
        }
    }
