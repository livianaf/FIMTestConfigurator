using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FIMTestConfigurator;

//_______________________________________________________________________________________________________________________
namespace FIMTestWpfConfigurator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //_______________________________________________________________________________________________________________________
    public partial class MainWindow : Window {
        //_______________________________________________________________________________________________________________________
        private string DBFile { get; set; }
        private List<string> lHiddenItems = null;
        private TreeViewItem tvCurrentSelection = null;
        private TreeViewItem mDragAndDropTarget = null;
        private Point _lastMouseDown;
        private bool ExpandedEditTextPanel = false;
        private bool AllowedDrag = false;
        private bool FirstDragMoveEvent = true;
        private bool DragCorrectlyStarted = false;
        //_______________________________________________________________________________________________________________________
        private bool InShowHideView => (cLeftPanel.Tag as DockPanel)?.Name == "bShowHide";
        //_______________________________________________________________________________________________________________________
        public MainWindow() {
            InitializeComponent();
            cWinTitle.Text = (TestsHelper.IsAdministrator ? "Administrator: " : "") + cWinTitle.Text;
            MaxHeight = WpfScreen.GetScreenFrom(this).MaximizedCurrentScreenHeight;//.DeviceBounds.Height;// SystemParameters.MaximizedPrimaryScreenHeight-9;
            MaxWidth = WpfScreen.GetScreenFrom(this).MaximizedCurrentScreenWidth;//.DeviceBounds.Width;//SystemParameters.MaximizedPrimaryScreenWidth-9;
            HideUnusedObjects();
            if (Environment.GetCommandLineArgs().Length == 2) OpenDB(Environment.GetCommandLineArgs()[1]);
            else ShowRecentPanel();
            }
        //_______________________________________________________________________________________________________________________
        private void bClose_Click(object sender, RoutedEventArgs e) { this.Close(); }
        //_______________________________________________________________________________________________________________________
        private void bMaximize_Click(object sender, RoutedEventArgs e) { AdjustWindowSize(); }
        //_______________________________________________________________________________________________________________________
        private void bMinimize_Click(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
        //_______________________________________________________________________________________________________________________
        private void bDetail_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.Detail(bDetail.Tag, out string value, out TestObjectBase o)) return;
            cEditDetail.Text = value;
            bSaveItem.IsEnabled = false;
            cEditDetail.IsEnabled = true;
            cEditDetail.Tag = o.Link;
            cPanelEditText.Visibility = Visibility.Visible;
            }
        //_______________________________________________________________________________________________________________________
        private void bSaveItem_Click(object sender, RoutedEventArgs e) { SaveObjectDetail(TestObjectBase.LinkedObject(cEditDetail.Tag)); }
        //_______________________________________________________________________________________________________________________
        private void bSave_Click(object sender, RoutedEventArgs e) { SaveObjectEdition(TestObjectBase.LinkedObject(bSave.Tag)); }
        //_______________________________________________________________________________________________________________________
        private void bOpenDB_MouseUp(object sender, MouseButtonEventArgs e) { OpenDB(); }
        //_______________________________________________________________________________________________________________________
        private void bCheckIntegrity_MouseUp(object sender, MouseButtonEventArgs e) { CheckTestsIntegrity(); }
        //_______________________________________________________________________________________________________________________
        private void bTools_MouseUp(object sender, MouseButtonEventArgs e) {
            ContextMenu ctxMenuTools = this.FindResource("ctxMenuTools") as ContextMenu;
            ctxMenuTools.PlacementTarget = sender as Control;
            MenuItem mCtxRunTest = LogicalTreeHelper.FindLogicalNode(ctxMenuTools, "mCtxRunTest") as MenuItem;
            mCtxRunTest.IsEnabled = !string.IsNullOrWhiteSpace(DBFile) && File.Exists(DBFile);
            ctxMenuTools.IsOpen = true;
            }
        //_______________________________________________________________________________________________________________________
        private void CaptionBar_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                    this.WindowState = System.Windows.WindowState.Normal;
                else
                    this.WindowState = System.Windows.WindowState.Maximized;
                }
            else this.DragMove();
            }
        //_______________________________________________________________________________________________________________________
        private void LeftPanel_MouseDown(object sender, MouseButtonEventArgs e) {
            if (string.IsNullOrWhiteSpace(DBFile)) return;
            ((DockPanel)sender).Background = SystemColors.MenuHighlightBrush;
            if (!string.IsNullOrEmpty("" + cLeftPanel.Tag)) ((DockPanel)cLeftPanel.Tag).Background = Brushes.Transparent;
            cLeftPanel.Tag = sender;
            if (((DockPanel)cLeftPanel.Tag).Name == "bAll") ShowAllObjects();
            else if (((DockPanel)cLeftPanel.Tag).Name == "bShowHide") ShowAllObjects();
            else ShowContent(((DockPanel)cLeftPanel.Tag).Name);
            }
        //_______________________________________________________________________________________________________________________
        //_______________________________________________________________________________________________________________________
        private void cTvItem_MouseUp(object sender, MouseButtonEventArgs e) {
            if (InShowHideView) return;
            e.Handled = true;
            Point p = e.GetPosition((TreeViewItem)sender);
            if (p.X < 180 && e.ChangedButton == MouseButton.Left) return;
            ConfigureCtxMenu((TreeViewItem)sender);
            }
        //_______________________________________________________________________________________________________________________
        private void cTvItem_Selected(object sender, RoutedEventArgs e) {
            if (InShowHideView) return;
            if (ExpandedEditTextPanel) {
                Grid.SetColumn(cPanelEditText, 4);
                Grid.SetColumnSpan(cPanelEditText, 1);
                ExpandedEditTextPanel = false;
                }

            UpdateSelection((TreeViewItem)sender);
            ShowObjectInEditor();
            FirstDragMoveEvent = true;
            DragCorrectlyStarted = false;
            }
        //_______________________________________________________________________________________________________________________
        private void cTvItem_MouseLeave(object sender, MouseEventArgs e) {
            var childs = ((DockPanel)((TreeViewItem)sender).Header).Children;
            childs[childs.Count - 2].Visibility = Visibility.Hidden;
            }
        //_______________________________________________________________________________________________________________________
        private void cTvItem_MouseEnter(object sender, MouseEventArgs e) {
            var childs = ((DockPanel)((TreeViewItem)sender).Header).Children;
            childs[childs.Count - 2].Visibility = Visibility.Visible;
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) _lastMouseDown = e.GetPosition(cTv); }
        //_______________________________________________________________________________________________________________________
        private void cTv_MouseMove(object sender, MouseEventArgs e) {
            if (!AllowedDrag) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;
            Point currentPosition = e.GetPosition(cTv);
            if ((Math.Abs(currentPosition.X - _lastMouseDown.X) < 10.0) && (Math.Abs(currentPosition.Y - _lastMouseDown.Y) < 10.0)) return;
            if(FirstDragMoveEvent) {
                FirstDragMoveEvent = false;
                IInputElement dropNode = cTv.InputHitTest(currentPosition);
                if (dropNode == null || dropNode.GetType().Name.Equals("ScrollChrome") || dropNode.GetType().Name.Equals("Grid")) return;
                DragCorrectlyStarted = true;
                }
            if (!DragCorrectlyStarted) return;

            TreeViewItem draggedItem = (TreeViewItem)cTv.SelectedItem;
            if (draggedItem != null) {
                draggedItem.IsSelected = true;
                TestObjectBase o = TestObjectBase.LinkedObject(draggedItem.Tag);
                if (o == null) return;
                if (o.GetType().Equals(typeof(Batch))) return;
                AtenuarNoTargetDrag(o, true);
                DragDrop.DoDragDrop(cTv, draggedItem, DragDropEffects.Link);
                AtenuarNoTargetDrag(o);
                ResaltarTargetDrag(null, false);
                FirstDragMoveEvent = true;
                DragCorrectlyStarted = false;
                }
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_DragOver(object sender, DragEventArgs e) {
            TreeViewItem targetNode = (TreeViewItem)GetNearestContainer(e.OriginalSource as UIElement);
            TreeViewItem draggedNode = (TreeViewItem)e.Data.GetData(typeof(TreeViewItem));
            if (IsValidDrag(draggedNode, targetNode)) {
                e.Effects = DragDropEffects.Link;
                ResaltarTargetDrag(targetNode);
                }
            else { e.Effects = DragDropEffects.None; }//ResaltarTarg
            }
        //_______________________________________________________________________________________________________________________
        private void cTv_Drop(object sender, DragEventArgs e) {
            TreeViewItem targetNode = (TreeViewItem)GetNearestContainer(e.OriginalSource as UIElement);
            TreeViewItem draggedNode = (TreeViewItem)e.Data.GetData(typeof(TreeViewItem));
            ResaltarTargetDrag(targetNode, false);
            if (!IsValidDrag(draggedNode, targetNode)) return;
            TestsHelper.CompleteDrop(TestObjectBase.LinkedObject(draggedNode.Tag), TestObjectBase.LinkedObject(targetNode.Tag));
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void cTvCheck_Checked(object sender, RoutedEventArgs e) {
            CheckBox cb = (CheckBox)e.Source;
            if (cb.Tag == null) {
                foreach (TreeViewItem n in ((ItemsControl)((FrameworkElement)((FrameworkElement)sender).Parent).Parent).Items)
                    ((CheckBox)((Panel)n.Header).Children[0]).IsChecked = cb.IsChecked;
                return;
                }
            if (cb.IsChecked == true) TestsHelper.deleteHiddenItem(cb.Tag);
            else TestsHelper.saveHiddenItem(cb.Tag);
            lHiddenItems = null;
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxAdd_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.Add((string)((MenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxUnhideSubItem_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.deleteHiddenItem(((MenuItem)sender).Tag)) return;
            lHiddenItems = null;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxDeleteSubItem_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.DeleteSubItem((object[])((MenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxDelete_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.Delete(((MenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxDuplicate_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.Copy(((MenuItem)sender).Tag)) return;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxHide_Click(object sender, RoutedEventArgs e) {
            if (!TestsHelper.saveHiddenItem(((MenuItem)sender).Tag)) return;
            lHiddenItems = null;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxRunTool_Click(object sender, RoutedEventArgs e) {
            string sTool = "" + ((MenuItem)sender).Tag;
            TestsHelper.LaunchTool(sTool, DBFile);
            }
        //_______________________________________________________________________________________________________________________
        private void mCtxReorder_Click(object sender, RoutedEventArgs e) {
            frmOrder f = new frmOrder() { Owner = this };
            ShowChildObjects(TestObjectBase.LinkedObject(((MenuItem)sender).Tag), f.cLst);
            if (f.ShowDialog() == true) {
                ApplyNewOrder(TestObjectBase.LinkedObject(((MenuItem)sender).Tag), f.cLst);
                ShowAllObjects();
                }
            }
        //_______________________________________________________________________________________________________________________
        private void cEditDetail_TextChanged(object sender, TextChangedEventArgs e) { bSaveItem.IsEnabled = true; }
        //_______________________________________________________________________________________________________________________
        private void EditItem_Changed(object sender, TextChangedEventArgs e) { bSave.IsEnabled = true; }
        //_______________________________________________________________________________________________________________________
        private void cbDetail_SelectionChanged(object sender, SelectionChangedEventArgs e) { bSave.IsEnabled = true; }
        //_______________________________________________________________________________________________________________________
        private void Check_Checked(object sender, RoutedEventArgs e) { bSave.IsEnabled = true; }
        //_______________________________________________________________________________________________________________________
        private void cPassword_PasswordChanged(object sender, RoutedEventArgs e) { bSave.IsEnabled = true; }
        //_______________________________________________________________________________________________________________________
        private void cLstRecent_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected) {
                dynamic a = item.Content;
                OpenDB(a.FullName);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void lBack_RequestNavigate(object sender, RequestNavigateEventArgs e) { LinkClicked((Hyperlink)e.Source, e.Uri.ToString().Substring(11), true); }
        //_______________________________________________________________________________________________________________________
        private void lDesc_RequestNavigate(object sender, RequestNavigateEventArgs e) { LinkClicked((Hyperlink)e.Source, e.Uri.ToString().Substring(11)); }
        //_______________________________________________________________________________________________________________________
        private void AdjustWindowSize() {
            if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                SetImgSource(bMax, "newMax");
                }
            else {
                this.WindowState = WindowState.Maximized;
                SetImgSource(bMax, "newRestore");
                }
            }
        //_______________________________________________________________________________________________________________________
        private string GetIconName(string Name) { return "new" + Name.Substring(1) + "B"; }
        //_______________________________________________________________________________________________________________________
        private string GetTviName(string Name) { return "tv" + Name.Substring(1); }
        //_______________________________________________________________________________________________________________________
        private string GetNodeTitle( string Name ) { return Name.Substring(1); }
        //_______________________________________________________________________________________________________________________
        private string GetHiddenCount(string Name) {
            int count = lHiddenItems.Where(r => r.StartsWith(TestsHelper.GetTableName(Name))).Count();
            if (count == 0) return "";
            return "  (" + count + " hidden)";
            }
        //_______________________________________________________________________________________________________________________
        private void ShowAllObjects() {
            if (!TestsHelper.Initialized) return;
            if (lHiddenItems == null) lHiddenItems = TestsHelper.getAllHidden();

            string PrevSelection = "" + ((TreeViewItem)cTv.SelectedItem)?.Tag;

            ShowContent("bBatches");
            ShowContent("bGroups", false);
            ShowContent("bTests", false);
            ShowContent("bConfigs", false);
            ShowContent("bMAs", false);
            ShowContent("bSources", false);
            ShowContent("bInputSets", false);
            ShowContent("bOutputSets", false);
            ShowContent("bScripts", false);
            ResetEditionPanel();
            if (!string.IsNullOrWhiteSpace(PrevSelection)) ModifyNodeByLink(cTv.Items, PrevSelection, (x) => x.IsSelected = true);
            AllowedDrag = true;
            ShowLasError();
            }
        //_______________________________________________________________________________________________________________________
        private void ShowLasError() {
            string msg = TestsHelper.LastErrorMessages;
            bError.ToolTip = msg;
            bError.Visibility = string.IsNullOrWhiteSpace(msg) ? Visibility.Collapsed : Visibility.Visible;
            }
        //_______________________________________________________________________________________________________________________
        private long ShowAnObject(TreeViewItem t, TestObjectBase g, string Icon) {
            if (lHiddenItems.Contains(g.Link) && !InShowHideView) return 1;
            //ng.SelectedImageIndex = 9;
            //ng.ImageIndex = t.ImageIndex;
            var item = new TreeViewItem() { Header = CreateChildNode(Icon, g), Tag = g.Link };
            g.CheckResult();
            if (g.ContainsErrors) item.ToolTip = g.ErrorsMessages;
            item.MouseEnter += cTvItem_MouseEnter;
            item.MouseLeave += cTvItem_MouseLeave;
            item.Selected += cTvItem_Selected;
            item.MouseUp += cTvItem_MouseUp;
            t.Items.Add(item);
            return 1;
            }
        //_______________________________________________________________________________________________________________________
        private void ConfigureCtxMenu(TreeViewItem node) {
            ContextMenu ctxMenu = this.FindResource("ctxMenu") as ContextMenu;
            MenuItem mCtxAdd = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxAdd") as MenuItem;
            MenuItem mCtxDuplicate = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxDuplicate") as MenuItem;
            Separator mCtxSep = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxSep") as Separator;
            MenuItem mCtxDelete = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxDelete") as MenuItem;
            MenuItem mCtxHide = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxHide") as MenuItem;
            MenuItem mCtxReorder = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxReorder") as MenuItem;

            string text = ((TextBlock)((DockPanel)node.Header).Children[2]).Text;
            if (node.Parent as TreeView != null) {
                mCtxAdd.Header = "New " + (text+" ").Replace("es ", "").Replace("s ", "").Trim(); mCtxAdd.Visibility = Visibility.Visible; mCtxAdd.Tag = node.Name;
                mCtxDuplicate.Visibility = Visibility.Collapsed;
                mCtxSep.Visibility = Visibility.Collapsed;
                mCtxDelete.Visibility = Visibility.Collapsed;
                mCtxHide.Visibility = Visibility.Collapsed;
                mCtxReorder.Visibility = Visibility.Collapsed;
                ConfigureCtxMenuSubItems(null, (string)node.Tag);
                }
            else if (node.Tag != null) {
                mCtxDuplicate.Header = "Copy " + text; mCtxDuplicate.Visibility = Visibility.Visible; mCtxDuplicate.Tag = node.Tag;
                mCtxDelete.Header = "Delete " + text; mCtxDelete.Visibility = Visibility.Visible; mCtxDelete.Tag = node.Tag;
                mCtxHide.Header = "Hide " + text; mCtxHide.Visibility = Visibility.Visible; mCtxHide.Tag = node.Tag;
                mCtxSep.Visibility = Visibility.Visible;
                mCtxAdd.Visibility = Visibility.Collapsed;
                mCtxReorder.Visibility = (TestObjectBase.TypeOfLink(node.Tag).Equals(typeof(Batch)) || TestObjectBase.TypeOfLink(node.Tag).Equals(typeof(Group))) ? Visibility.Visible : Visibility.Collapsed;
                mCtxReorder.Header = "Reorder " + (TestObjectBase.TypeOfLink(node.Tag).Equals(typeof(Batch)) ? "Groups" : "Tests");
                mCtxReorder.Tag = node.Tag;
                mCtxReorder.IsEnabled = false;
                ConfigureCtxMenuSubItems(TestObjectBase.LinkedObject(node.Tag));
                }
            else return;
            ctxMenu.PlacementTarget = node as Control;
            ctxMenu.IsOpen = true;
            }
        //_______________________________________________________________________________________________________________________
        private void ConfigureCtxMenuSubItems(object o, string sTipo = "") {
            ContextMenu ctxMenu = this.FindResource("ctxMenu") as ContextMenu;
            MenuItem mCtxReorder = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxReorder") as MenuItem;
            Separator mCtxSep2 = LogicalTreeHelper.FindLogicalNode(ctxMenu, "mCtxSep2") as Separator;
            mCtxSep2.Visibility = Visibility.Collapsed;
            //elimina menús de una ejecución anterior
            for (int i = ctxMenu.Items.Count - 1; i > 0; i--) if ((ctxMenu.Items[i] as MenuItem)!=null && (ctxMenu.Items[i] as MenuItem).Header.ToString().StartsWith("Remove")) ctxMenu.Items.RemoveAt(i);
            for (int i = ctxMenu.Items.Count - 1; i > 0; i--) if ((ctxMenu.Items[i] as MenuItem) != null && (ctxMenu.Items[i] as MenuItem).Header.ToString().StartsWith("Unhide")) ctxMenu.Items.RemoveAt(i);
            if (o == null && lHiddenItems != null) {
                foreach (string lnk in lHiddenItems) AddMenuUnhide(ctxMenu, mCtxSep2, lnk, sTipo);
                }
            if (o == null) return;

            // añade nuevos menus
            if (o.GetType().Equals(typeof(Batch))) {
                foreach (Group t in ((Batch)o).Groups) AddMenuRemove(ctxMenu, mCtxSep2, (Batch)o, t);
                mCtxReorder.IsEnabled = (((Batch)o).Groups.Count > 1);
                }
            if (o.GetType().Equals(typeof(Group))) {
                foreach (Test t in ((Group)o).Tests) AddMenuRemove(ctxMenu, mCtxSep2, (Group)o, t);
                mCtxReorder.IsEnabled = (((Group)o).Tests.Count > 1);
                }
            if (o.GetType().Equals(typeof(Test))) {
                if (((Test)o).Config != null) AddMenuRemove(ctxMenu, mCtxSep2, (Test)o, ((Test)o).Config);
                if (((Test)o).Script != null) AddMenuRemove(ctxMenu, mCtxSep2, (Test)o, ((Test)o).Script);
                if (((Test)o).InputSet != null) AddMenuRemove(ctxMenu, mCtxSep2, (Test)o, ((Test)o).InputSet);
                if (((Test)o).SourceManagementAgent != null) AddMenuRemove(ctxMenu, mCtxSep2, (Test)o, ((Test)o).SourceManagementAgent);
                foreach (OutputSet t in ((Test)o).OutputSets) AddMenuRemove(ctxMenu, mCtxSep2, (Test)o, t);
                }
            if (o.GetType().Equals(typeof(OutputSet))) {
                if (((OutputSet)o).DestinationManagementAgent != null) AddMenuRemove(ctxMenu, mCtxSep2, (OutputSet)o, ((OutputSet)o).DestinationManagementAgent);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void AddMenuUnhide(ContextMenu ctxMenu, Separator mCtxSep2, string lnk, string sTipo) {
            if (!lnk.StartsWith(sTipo)) return;
            var t = TestObjectBase.LinkedObject(lnk);
            if (t == null) return;
            var oMnu = new MenuItem();
            oMnu.Name = "Unhide" + ctxMenu.Items.Count;
            oMnu.Tag = lnk;
            oMnu.Header = "Unhide " + t.Name;
            oMnu.Click += new RoutedEventHandler(mCtxUnhideSubItem_Click);
            ctxMenu.Items.Add(oMnu);
            mCtxSep2.Visibility = Visibility.Visible;
            }
        //_______________________________________________________________________________________________________________________
        private void AddMenuRemove(ContextMenu ctxMenu, Separator mCtxSep2, TestObjectBase oBase, TestObjectBase oChild) {
            var oMnu = new MenuItem();
            oMnu.Name = "Remove" + ctxMenu.Items.Count;
            oMnu.Tag = new object[] { oBase, oChild };
            oMnu.Header = "Remove " + oChild.Alias + "[" + oChild.Name + "] from " + oBase.Alias;
            oMnu.Click += new RoutedEventHandler(mCtxDeleteSubItem_Click);
            ctxMenu.Items.Add(oMnu);
            mCtxSep2.Visibility = Visibility.Visible;
            }
        //_______________________________________________________________________________________________________________________
        private void UpdateSelection(TreeViewItem tvi) {
            Image img = null;
            if (!tvi.IsSelected) return;
            if (tvCurrentSelection != null) {
                img = (Image)((DockPanel)tvCurrentSelection.Header).Children[0];
                if (!string.IsNullOrEmpty(""+img.Tag)) SetImgSource(img);
                }
            tvCurrentSelection = tvi;
            img = (Image)((DockPanel)tvi.Header).Children[0];
            if (!string.IsNullOrEmpty("" + img.Tag)) SetImgSource(img, "newArrow",false);
            }
        //_______________________________________________________________________________________________________________________
        private void ShowContent(string Name, bool ClearContent = true) {
            string icon = GetIconName(Name);
            string title = GetNodeTitle(Name) + GetHiddenCount(Name);
            if (ClearContent) cTv.Items.Clear();
            AllowedDrag = !ClearContent;
            var item = new TreeViewItem() { IsExpanded=true };
            item.MouseEnter += cTvItem_MouseEnter;
            item.MouseLeave += cTvItem_MouseLeave;
            item.Selected += cTvItem_Selected;
            item.MouseUp += cTvItem_MouseUp;
            item.Header = CreateRootNode(icon, title);
            item.Name = GetTviName(Name);
            item.Tag = TestsHelper.GetTableName(Name);
            cTv.Items.Add(item);
            switch (Name) {
                case "bBatches": foreach (var g in TestsHelper.getAllBatches()) ShowAnObject(item, g, icon); break;
                case "bGroups": foreach (var g in TestsHelper.getAllGroups()) ShowAnObject(item, g, icon); break;
                case "bTests": foreach (var g in TestsHelper.getAllTests()) ShowAnObject(item, g, icon); break;
                case "bConfigs":foreach (var g in TestsHelper.getAllConfig()) ShowAnObject(item, g, icon); break;
                case "bMAs": foreach (var g in TestsHelper.getAllMAgent()) ShowAnObject(item, g, icon); break;
                case "bSources": foreach (var g in TestsHelper.getAllSource()) ShowAnObject(item, g, icon); break;
                case "bInputSets": foreach (var g in TestsHelper.getAllInputSet()) ShowAnObject(item, g, icon); break;
                case "bOutputSets": foreach (var g in TestsHelper.getAllOutputSet()) ShowAnObject(item, g, icon); break;
                case "bScripts": foreach (var g in TestsHelper.getAllScript()) ShowAnObject(item, g, icon); break;
                }
            }
        //_______________________________________________________________________________________________________________________
        private object CreateRootNode(string Icon, string Name) {
            DockPanel dp = new DockPanel() { HorizontalAlignment = HorizontalAlignment.Left, Width = 190, Height=20, LastChildFill = true };
            DockPanel.SetDock(dp, Dock.Right);
            CheckBox chk = new CheckBox();
            if (InShowHideView) {
                chk.IsChecked = true;
                chk.Tag = null;
                chk.Checked += cTvCheck_Checked;
                chk.Unchecked += cTvCheck_Checked;
                }
            Image img = new Image() { Source = GetImgResource(Icon), Width = 20, Height = 20, HorizontalAlignment = HorizontalAlignment.Center, Tag = null };
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
            DockPanel.SetDock(img, Dock.Left);
            TextBlock txt = new TextBlock() { Padding = new Thickness(10,3,0,0), Background = Brushes.Transparent, FontSize = 12, HorizontalAlignment = HorizontalAlignment.Left, Foreground = Brushes.Black, Text = Name };
            TextBlock elip = new TextBlock() { Padding = new Thickness(0, 0, 0, 0), Background = Brushes.Transparent, FontSize = 10, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Top, FontWeight = FontWeights.UltraBold, Foreground = Brushes.Black, Text = "...", Visibility = Visibility.Hidden };
            DockPanel.SetDock(elip, Dock.Right);
            if (InShowHideView) dp.Children.Add(chk);
            dp.Children.Add(img);
            dp.Children.Add(elip);
            dp.Children.Add(txt);
            return dp;
            }
        //_______________________________________________________________________________________________________________________
        private object CreateChildNode( string Icon, TestObjectBase o ) {
            DockPanel dp = new DockPanel() { HorizontalAlignment = HorizontalAlignment.Left, Width = 188, Height = 18, LastChildFill = true };
            DockPanel.SetDock(dp, Dock.Right);
            CheckBox chk = new CheckBox();
            if (InShowHideView) {
                chk.IsChecked = !lHiddenItems.Contains(o.Link);
                chk.Tag = o.Link;
                chk.Checked += cTvCheck_Checked;
                chk.Unchecked += cTvCheck_Checked;
                }
            Image img = new Image() { Source = GetImgResource(Icon), Width = 18, Height = 18, HorizontalAlignment = HorizontalAlignment.Center, Tag = Icon };
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
            DockPanel.SetDock(img, Dock.Left);
            TextBlock txt = new TextBlock() { Padding = new Thickness(10, 3, 0, 0), Background = Brushes.Transparent, FontSize = 12, HorizontalAlignment = HorizontalAlignment.Left, Foreground = Brushes.Black, Text = o.Name };
            TextBlock elip = new TextBlock() { Padding = new Thickness(0, 0, 0, 0), Background = Brushes.Transparent, FontSize = 10, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Top, FontWeight = FontWeights.UltraBold, Foreground = Brushes.Black, Text = "...", Visibility = Visibility.Hidden };
            DockPanel.SetDock(elip, Dock.Right);
            if (InShowHideView) dp.Children.Add(chk);
            dp.Children.Add(img);
            dp.Children.Add(elip);
            dp.Children.Add(txt);
            return dp;
            }
        //_______________________________________________________________________________________________________________________
        private void OpenDB() {
            string DbFile = Utilities.SelectFile("Browse DB Files", "db files (*.db)|*.db");
            if (DbFile != null) OpenDB(DbFile);
            }
        //_______________________________________________________________________________________________________________________
        private void OpenDB(string sDBName) {
            DBFile = sDBName;
            cWinTitleDB.Text = DBFile;
            AddToRecentFiles(DBFile);
            TestsHelper.Init(DBFile);
            cTv.Visibility = bCheckIntegrity.Visibility = Visibility.Visible;
            bAll.Visibility = bBatches.Visibility = bGroups.Visibility = bTests.Visibility = bConfigs.Visibility = bMAs.Visibility = bSources.Visibility = bInputSets.Visibility = bOutputSets.Visibility = bScripts.Visibility = bShowHide.Visibility = Visibility.Visible;

            DockPanel.SetDock(bOpenDB, Dock.Bottom);

            Grid.SetColumn(cPanelRecent, 4);
            Grid.SetColumnSpan(cPanelRecent, 1);
            cPanelRecent.Visibility = Visibility.Collapsed;


            lHiddenItems = null;
            ShowAllObjects();
            //cTv.ExpandAll();
            }
        //_______________________________________________________________________________________________________________________
        private void ResetEditionPanel(bool full = false) {
            cEditDetail.Text = "";
            cEditDetail.IsReadOnly = false;

            bDetail.Tag = bSave.Tag = null;
            cEditDetail.IsEnabled = false;
            cPanelEdit.Visibility = cDetail.Visibility = cDelta.Visibility = cCommit.Visibility = Visibility.Hidden;
            bSave.IsEnabled = bSaveItem.IsEnabled = false;
            bDetail.Visibility = cbDetail.Visibility = Visibility.Hidden;
            cServer.Visibility = cUser.Visibility = cPassword.Visibility = cAuthType.Visibility = Visibility.Hidden;
            lDetail.Visibility = lUser.Visibility = lPassword.Visibility = lAuthType.Visibility = lDesc.Visibility = Visibility.Hidden;

            lName.Visibility = cName.Visibility = !full? Visibility.Visible : Visibility.Hidden;
            bSave.Visibility = bSaveItem.Visibility = !full ? Visibility.Visible : Visibility.Hidden;
            }
        //_______________________________________________________________________________________________________________________
        private void FillTextBoxContentWithLinks(TextBlock tb, TestObjectBase o ) {
            if (o.AdditionalLinkedInfo.Count == 0) return;
            tb.Inlines.Clear();
            string orgTxt = o.AdditionalInfo();
            int start = 0;
            foreach (var t in o.AdditionalLinkedInfo) {
                tb.Inlines.Add(orgTxt.Substring(start, t.Item1 - start));
                Hyperlink hyperLink = new Hyperlink() { NavigateUri = new Uri("c:\\" + t.Item3.ToString()) };
                hyperLink.Inlines.Add(orgTxt.Substring(t.Item1, t.Item2));
                hyperLink.RequestNavigate += lDesc_RequestNavigate;
                tb.Inlines.Add(hyperLink);
                start = t.Item1 + t.Item2;
                }
            tb.Inlines.Add(orgTxt.Substring(start));
            }
        //_______________________________________________________________________________________________________________________
        private void LinkClicked(Hyperlink link, string sLnk, bool isBack = false) {
            TreeViewItem t = SearchNodeByLink(cTv.Items, sLnk);
            if (t == null) { link.IsEnabled = false; return; }
            
            if (lBack.Tag == null) lBack.Tag = new List<string>();
            List<string> lstBack = (List<string>)lBack.Tag;

            lBack.Inlines.Clear();
            if (isBack) {
                if (lstBack.Contains(sLnk)) lstBack.Remove(sLnk);
                if (lstBack.Count > 0) {
                    Hyperlink hyperLink = new Hyperlink() { NavigateUri = new Uri("c:\\" + lstBack[lstBack.Count - 1]) };
                    hyperLink.Inlines.Add("Go back to: " + TestObjectBase.LinkedObject(lstBack[lstBack.Count - 1]).Name);
                    hyperLink.RequestNavigate += lBack_RequestNavigate;
                    lBack.Inlines.Add(hyperLink);
                    }
                }
            else {
                lstBack.Add("" + ((TreeViewItem)cTv.SelectedItem).Tag);

                Hyperlink hyperLink = new Hyperlink() { NavigateUri = new Uri("c:\\" + ((TreeViewItem)cTv.SelectedItem).Tag) };
                hyperLink.Inlines.Add("Go back to: " + TestObjectBase.LinkedObject(((TreeViewItem)cTv.SelectedItem).Tag).Name);
                hyperLink.RequestNavigate += lBack_RequestNavigate;
                lBack.Inlines.Add(hyperLink);
                }
            t.IsSelected = true;
            lBack.Visibility = (lBack.Inlines.Count > 0)?Visibility.Visible:Visibility.Collapsed;
            }
        //_______________________________________________________________________________________________________________________
        private void ShowObjectInEditor() {
            ResetEditionPanel();

            if (cTv.SelectedItem == null || ((TreeViewItem)cTv.SelectedItem).Parent == null || ((TreeViewItem)cTv.SelectedItem).Tag == null) return;
            TestObjectBase o = TestObjectBase.LinkedObject(((TreeViewItem)cTv.SelectedItem).Tag);
            if (o == null) return;

            cName.Text = o.Name;
            lDesc.Text = o.AdditionalInfo();
            FillTextBoxContentWithLinks(lDesc, o);

            if (o.GetType().Equals(typeof(Batch))) {
                lName.Text = "Batch:";
                lDesc.Visibility = Visibility.Visible;
                }
            else if (o.GetType().Equals(typeof(Group))) {
                lName.Text = "Group:";
                lDesc.Visibility = Visibility.Visible;
                }
            else if (o.GetType().Equals(typeof(Test))) {
                lName.Text = "Test:";
                cDelta.IsChecked = ((Test)o).Delta; cDelta.Visibility = Visibility.Visible;
                cCommit.IsChecked = ((Test)o).Commit; cCommit.Visibility = Visibility.Visible;
                lDesc.Visibility = Visibility.Visible;
                bDetail.Visibility = Visibility.Visible;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(Config))) {
                lName.Text = "File Name:";
                bDetail.Visibility = Visibility.Visible;
                lDetail.Text = "Description:";
                cDetail.Text = ((Config)o).Desc;
                cDetail.Visibility = lDetail.Visibility = Visibility.Visible;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(Script))) {
                lName.Text = "Name:";
                bDetail.Visibility = Visibility.Visible;
                lDetail.Text = "Description:";
                cDetail.Text = ((Script)o).Desc;
                cDetail.Visibility = lDetail.Visibility = Visibility.Visible;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(ManagementAgentInfo))) {
                lName.Text = "MA:";
                lDesc.Visibility = Visibility.Visible;
                }
            else if (o.GetType().Equals(typeof(Source))) {
                lName.Text = "Name:";
                cServer.Visibility = cUser.Visibility = cPassword.Visibility = cAuthType.Visibility = Visibility.Visible;
                lUser.Visibility = lPassword.Visibility = lAuthType.Visibility = Visibility.Visible;
                lDetail.Text = "Server:";
                lDetail.Visibility = Visibility.Visible;
                cServer.Text = ((Source)o).Server;
                cUser.Text = ((Source)o).User;
                cPassword.Password = ((Source)o).Password;
                cAuthType.Text = ((Source)o).AuthType;
                bDetail.Tag = o.Link;
                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(InputSet))) {
                lName.Text = "Input Set:";
                lDetail.Visibility = Visibility.Visible; lDetail.Text = "DN:";
                cDetail.Visibility = Visibility.Visible; cDetail.Text = ((InputSet)o).DistinguishedName;
                bDetail.Visibility = Visibility.Visible; bDetail.Tag = o.Link;

                lDesc.Visibility = Visibility.Visible;

                bDetail_Click(null, null);
                }
            else if (o.GetType().Equals(typeof(OutputSet))) {
                lName.Text = "Output Set:";
                lDetail.Visibility = Visibility.Visible; lDetail.Text = "Type:";
                bDetail.Visibility = Visibility.Visible; bDetail.Tag = o.Link;
                cbDetail.Visibility = Visibility.Visible; cbDetail.ItemsSource = TestsHelper.getOutputSetTypes();

                for (int i = 0; i < cbDetail.Items.Count; i++)
                    if (cbDetail.Items[i].ToString().iEquals(((OutputSet)o).Type))
                        cbDetail.SelectedIndex = i;

                lDesc.Visibility = Visibility.Visible;

                bDetail_Click(null, null);
                }
            else return;
            bSave.Tag = o.Link;
            cPanelEdit.Visibility = Visibility.Visible;
            bSave.IsEnabled = false;
            }
        //_______________________________________________________________________________________________________________________
        private void SaveObjectDetail(TestObjectBase o, bool refreshTv = true ) {
            if (o == null) return;
            if (!bSaveItem.IsEnabled) return;
            TestsHelper.SaveObjectDetail(o, cEditDetail.Text);
            bSaveItem.IsEnabled = false;
            if (refreshTv) ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void SaveObjectEdition(TestObjectBase o) {
            if (o == null) return;
            SaveObjectDetail(TestObjectBase.LinkedObject(cEditDetail.Tag), false);
            TestsHelper.UpdateName(o, cName.Text);
            if (o.GetType().Equals(typeof(Config))) TestsHelper.saveConfigDesc((Config)o, cDetail.Text);
            if (o.GetType().Equals(typeof(Test))) TestsHelper.saveTest((Test)o, (bool)cDelta.IsChecked ? 1 : 0, (bool)cCommit.IsChecked ? 1 : 0);
            if (o.GetType().Equals(typeof(Source))) TestsHelper.saveSource((Source)o, cName.Text, cServer.Text, cUser.Text, cPassword.Password, cAuthType.Text);
            if (o.GetType().Equals(typeof(InputSet))) TestsHelper.saveInputSetDN((InputSet)o, cDetail.Text);
            if (o.GetType().Equals(typeof(OutputSet))) TestsHelper.saveOutputSetType((OutputSet)o, cbDetail.Text);
            if (o.GetType().Equals(typeof(Script))) TestsHelper.saveScriptDesc((Script)o, cDetail.Text);
            bSaveItem.IsEnabled = false;
            ShowAllObjects();
            }
        //_______________________________________________________________________________________________________________________
        private void ModifyNodeByLink(ItemCollection nodes, string sLnk, Action<TreeViewItem> modify) {
            foreach (TreeViewItem n in nodes) {
                if (sLnk.Equals("" + n.Tag)) { modify(n); return; }
                ModifyNodeByLink(n.Items, sLnk, modify);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void ModifyNodeByLink(ItemCollection nodes, Func<TreeViewItem, bool> modify) {
            foreach (TreeViewItem n in nodes) {
                if (modify(n)) return;
                ModifyNodeByLink(n.Items, modify);
                }
            }
        //_______________________________________________________________________________________________________________________
        private TreeViewItem SearchNodeByLink(ItemCollection nodes, string sLnk) {
            foreach (TreeViewItem n in nodes) {
                if (sLnk.Equals("" + n.Tag)) { return n; }
                var r = SearchNodeByLink(n.Items, sLnk);
                if (r != null) return r;
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        private bool IsValidDrag(TreeViewItem draggedNode, TreeViewItem targetNode) {
            if (draggedNode == null || targetNode?.Parent == null) return false;
            Type torg = TestObjectBase.TypeOfLink(draggedNode.Tag);
            Type tdst = TestObjectBase.TypeOfLink(targetNode.Tag);
            return TestsHelper.IsValidDrag(torg, tdst);
            }
        //_______________________________________________________________________________________________________________________
        private TreeViewItem GetNearestContainer(UIElement element) {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null)) {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
                }
            return container;
            }
        //_______________________________________________________________________________________________________________________
        private void AtenuarNoTargetDrag(TestObjectBase org, bool op = false) {
            Type torg = org.GetType();
            if (torg.Equals(typeof(Test)))
                ModifyNodeByLink(cTv.Items, (n) => { if (n.Parent != null && !((string)("" + n.Tag)).StartsWith("g")) ((TextBlock)((DockPanel)n.Header).Children[2]).Foreground = op ? Brushes.Gray : Brushes.Black; return false; });//((TextBlock)((DockPanel)((TreeViewItem)cTv.Items[0]).Header).Children[2]).Foreground
            if (torg.Equals(typeof(Config)) || torg.Equals(typeof(Script)) || torg.Equals(typeof(InputSet)) || torg.Equals(typeof(OutputSet)))
                ModifyNodeByLink(cTv.Items, (n) => { if (n.Parent != null && !((string)("" + n.Tag)).StartsWith("t")) ((TextBlock)((DockPanel)n.Header).Children[2]).Foreground = op ? Brushes.Gray : Brushes.Black; return false; });
            if (torg.Equals(typeof(ManagementAgentInfo)))
                ModifyNodeByLink(cTv.Items, (n) => { if (n.Parent != null && !((string)("" + n.Tag)).StartsWith("o") && !((string)(n.Tag)).StartsWith("t")) ((TextBlock)((DockPanel)n.Header).Children[2]).Foreground = op ? Brushes.Gray : Brushes.Black; return false; });
            if (torg.Equals(typeof(Source)))
                ModifyNodeByLink(cTv.Items, (n) => { if (n.Parent != null && !((string)("" + n.Tag)).StartsWith("m")) ((TextBlock)((DockPanel)n.Header).Children[2]).Foreground = op ? Brushes.Gray : Brushes.Black; return false; });
            }
        //_______________________________________________________________________________________________________________________
        private void ResaltarTargetDrag(TreeViewItem tn, bool op = true) {
            if (mDragAndDropTarget != null && mDragAndDropTarget != tn) {
                // quita marca
                ((TextBlock)((DockPanel)mDragAndDropTarget.Header).Children[2]).Foreground = Brushes.Black;// ((TextBlock)((DockPanel)((TreeViewItem)cTv.Items[0]).Header).Children[2]).Foreground;
                }
            mDragAndDropTarget = tn;
            if (tn == null) return;
            if (op) {
                //pone marca
                ((TextBlock)((DockPanel)tn.Header).Children[2]).Foreground = Brushes.Red;
                }
            else {
                // quita marca
                ((TextBlock)((DockPanel)tn.Header).Children[2]).Foreground = Brushes.Black;//((TextBlock)((DockPanel)((TreeViewItem)cTv.Items[0]).Header).Children[2]).Foreground;
                }
            }
        //_______________________________________________________________________________________________________________________
        private void HideUnusedObjects() {
            cPanelEdit.Visibility = cPanelEditText.Visibility = cTv.Visibility = bCheckIntegrity.Visibility = Visibility.Hidden;
            bAll.Visibility = bBatches.Visibility = bGroups.Visibility = bTests.Visibility = bConfigs.Visibility = bMAs.Visibility = bSources.Visibility = bInputSets.Visibility = bOutputSets.Visibility = bScripts.Visibility = bShowHide.Visibility = Visibility.Collapsed;
            DockPanel.SetDock(bOpenDB, Dock.Top);
            }
        //_______________________________________________________________________________________________________________________
        private void CheckTestsIntegrity() {
            ResetEditionPanel(true);
            Grid.SetColumn(cPanelEditText, 2);
            Grid.SetColumnSpan(cPanelEditText, 3);
            ExpandedEditTextPanel = true;
            cPanelEditText.Visibility = Visibility.Visible;
            cEditDetail.Text = "";
            cEditDetail.IsReadOnly = false;
            cEditDetail.IsEnabled = true;
            // Clean broken links
            TestsHelper.cleanBrokenLinks();
            // Valida batches
            foreach (Batch o in TestsHelper.getAllBatches()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida groups
            foreach (Group o in TestsHelper.getAllGroups()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida tests
            foreach (Test o in TestsHelper.getAllTests()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida inputsets
            foreach (InputSet o in TestsHelper.getAllInputSet()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida outputsets
            foreach (OutputSet o in TestsHelper.getAllOutputSet()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida ManagementAgentInfo
            foreach (ManagementAgentInfo o in TestsHelper.getAllMAgent()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida Source
            foreach (Source o in TestsHelper.getAllSource()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida configs
            foreach (Config o in TestsHelper.getAllConfig()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            // Valida scripts
            foreach (Script o in TestsHelper.getAllScript()) {
                cEditDetail.Text += o.CheckResult();
                if (o.ContainsErrors) ModifyNodeByLink(cTv.Items, o.Link, (x) => SetImgSource((Image)((DockPanel)x.Header).Children[0], "newFailed"));
                }
            cEditDetail.Visibility = Visibility.Visible;
            cPanelEdit.Visibility = Visibility.Visible;
            bSave.IsEnabled = false;
            }
        //_______________________________________________________________________________________________________________________
        private void SetImgSource(Image img, string Name="", bool bSave = true) {
            if (string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace("" + img.Tag)) { Name = "" + img.Tag; bSave = false; }
            if (!string.IsNullOrWhiteSpace(Name)) {
                img.Source = GetImgResource(Name);
                if (bSave) img.Tag = Name;
                }
            }
        //_______________________________________________________________________________________________________________________
        private BitmapImage GetImgResource(string Name) { try { return new BitmapImage(new Uri("pack://application:,,,/FIMTestConfigurator;component/images/" + Name + ".png")); } catch (Exception) { return null; } }
        //_______________________________________________________________________________________________________________________
        private void ShowRecentPanel() {
            List<FileInfo> lRecent = new List<FileInfo>();
            LoadRecentFiles(lRecent);
            if (lRecent.Count == 0) return;
            cPanelRecent.Visibility = Visibility.Visible;
            Grid.SetColumn(cPanelRecent, 2);
            Grid.SetColumnSpan(cPanelRecent, 3);
            lRecent = lRecent.OrderByDescending(f => f.LastWriteTime).ToList();
            foreach (FileInfo fi in lRecent) 
                cLstRecent.Items.Add(new { Name = fi.Name, Date = fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"), Path = fi.DirectoryName, FullName = fi.FullName });
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
        private void ShowChildObjects(TestObjectBase o, ListView cLstNewOrder) {
            if (o.GetType().Equals(typeof(Batch))) {
                foreach (Group t in ((Batch)o).Groups) {
                    cLstNewOrder.Items.Add(t);
                    }
                }
            if (o.GetType().Equals(typeof(Group))) {
                foreach (Test t in ((Group)o).Tests) {
                    cLstNewOrder.Items.Add(t);
                    }
                }
            cLstNewOrder.SelectedItem = cLstNewOrder.Items[0];
            }
        //_______________________________________________________________________________________________________________________
        private void ApplyNewOrder(TestObjectBase o, ListView cLstNewOrder) {
            for (int i = 0; i < cLstNewOrder.Items.Count; i++) {
                long ID = ((TestObjectBase)cLstNewOrder.Items[i]).Id;
                if (o.GetType().Equals(typeof(Batch))) TestsHelper.setBatchGroupOrder((Batch)o, ID, i);
                if (o.GetType().Equals(typeof(Group))) TestsHelper.setGroupTestOrder((Group)o, ID, i);
                }
            }
        }
    }
