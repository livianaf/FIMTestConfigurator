using System.Windows;
using System.Windows.Input;

//_______________________________________________________________________________________________________________________
namespace FIMTestWpfConfigurator {
    //_______________________________________________________________________________________________________________________
    public partial class frmOrder : Window {
        //_______________________________________________________________________________________________________________________
        public frmOrder() { InitializeComponent(); }
        //_______________________________________________________________________________________________________________________
        private void BSave_Click(object sender, RoutedEventArgs e) { DialogResult = true; }
        //_______________________________________________________________________________________________________________________
        private void BCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; }
        //_______________________________________________________________________________________________________________________
        private void BUp_Click(object sender, RoutedEventArgs e) {
            if (cLst.SelectedItems.Count != 1) return;
            var currentIndex = cLst.SelectedIndex;
            if (currentIndex == 0) return;
            var item = cLst.Items[currentIndex];
            cLst.Items.RemoveAt(currentIndex);
            cLst.Items.Insert(currentIndex - 1, item);
            cLst.SelectedItem = item;
            }
        //_______________________________________________________________________________________________________________________
        private void BDown_Click(object sender, RoutedEventArgs e) {
            if (cLst.SelectedItems.Count != 1) return;
            var currentIndex = cLst.SelectedIndex;
            if (currentIndex == cLst.Items.Count - 1) return;
            var item = cLst.Items[currentIndex];
            cLst.Items.RemoveAt(currentIndex);
            cLst.Items.Insert(currentIndex + 1, item);
            cLst.SelectedItem = item;
            }
        //_______________________________________________________________________________________________________________________
        private void CaptionBar_MouseDown(object sender, MouseButtonEventArgs e) { this.DragMove(); }
        }
    }
