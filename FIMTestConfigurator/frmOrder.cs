using System;
using System.Windows.Forms;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public partial class frmOrder : Form {
        //_______________________________________________________________________________________________________________________
        public frmOrder() {
            InitializeComponent();
            }
        //_______________________________________________________________________________________________________________________
        private void bUp_Click( object sender, EventArgs e ) {
            if (cLst.SelectedItems.Count != 1) return;
            if (cLst.SelectedItems[0].Index == 0) return;
            var currentIndex = cLst.SelectedItems[0].Index;
            var item = cLst.Items[currentIndex];
            cLst.Items.RemoveAt(currentIndex);
            cLst.Items.Insert(currentIndex - 1, item);
            }
        //_______________________________________________________________________________________________________________________
        private void bDown_Click( object sender, EventArgs e ) {
            if (cLst.SelectedItems.Count != 1) return;
            if (cLst.SelectedItems[0].Index == cLst.Items.Count - 1) return;
            var currentIndex = cLst.SelectedItems[0].Index;
            var item = cLst.Items[currentIndex];
            cLst.Items.RemoveAt(currentIndex);
            cLst.Items.Insert(currentIndex + 1, item);
            }
        //_______________________________________________________________________________________________________________________
        private void frmOrder_Resize( object sender, EventArgs e ) {
            foreach (ColumnHeader c in cLst.Columns) c.Width = -2;
            }
        }
    }
