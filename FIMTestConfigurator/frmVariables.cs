using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public partial class frmVariables : Form {
        private string sLnkCurrentLocation { get; set; }
        //_______________________________________________________________________________________________________________________
        public frmVariables() { InitializeComponent(); LoadLocations(); }
        //_______________________________________________________________________________________________________________________
        private void LoadLocations() {
            List<Location> locs = TestsHelper.getAllLocations();
            bool machineIsPresent = false;
            foreach (Location l in locs) if(l.Name.iEquals(System.Environment.MachineName)) machineIsPresent = true;
            if (!machineIsPresent) TestsHelper.saveLocation(TestObjectBase.NO_ID, System.Environment.MachineName);

            locs = TestsHelper.getAllLocations();
            DataTable dt = locs.ConvertToDataTable(new List<string>() { "Id", "Name", "Desc" });

            cDgEnv.DataSource = dt;
            cDgEnv.Columns[0].Visible = false;
            cDgEnv.Columns[cDgEnv.Columns.Count - 2].Width = this.Width / 3;
            cDgEnv.Columns[cDgEnv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cDgEnv.Rows[0].Selected = true;
            }
        //_______________________________________________________________________________________________________________________
        private void CDgEnv_SelectionChanged(object sender, EventArgs e) {
            if (cDgEnv.SelectedCells.Count != 1 && cDgEnv.SelectedRows.Count != 1) return;
            if (cDgEnv.Rows[cDgEnv.SelectedCells[0].RowIndex].IsNewRow) return;
            if (cDgEnv.Rows[cDgEnv.SelectedCells[0].RowIndex].Cells[0].Value.Equals(DBNull.Value)) return;
            Location loc = TestsHelper.getLocationByID((long)cDgEnv.Rows[cDgEnv.SelectedCells[0].RowIndex].Cells[0].Value);
            if (loc == null) return;
            sLnkCurrentLocation = loc.Link;
            DataTable dt = loc.Variables.ConvertToDataTable(new List<string>() { "Id", "Name", "Value", "Desc" });

            cDgVars.DataSource = dt;
            cDgVars.Columns[0].Visible = false;
            cDgVars.Columns[cDgVars.Columns.Count - 3].Width = this.Width / 4;
            cDgVars.Columns[cDgVars.Columns.Count - 2].Width = this.Width / 3;
            cDgVars.Columns[cDgVars.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cDgVars.Rows[0].Selected = true;
            lStBar.Text = $"Selected Location: {loc.Name} ({loc.Desc}). Number of Variables: {loc.Variables.Count}.";
            }
        //_______________________________________________________________________________________________________________________
        private void CDgEnv_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (cDgEnv.Rows[e.RowIndex].Cells[0].Value.Equals(DBNull.Value)) {
                if (e.ColumnIndex == 1) {
                    long id = TestsHelper.saveLocation(TestObjectBase.NO_ID, cDgEnv.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cDgEnv.Rows[e.RowIndex].Cells[0].Value = id;
                    }
                else if (e.ColumnIndex == 2 && cDgEnv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "") {
                    string name = "new location " + cDgEnv.Rows.Count;
                    long id = TestsHelper.saveLocation(TestObjectBase.NO_ID, name);
                    TestsHelper.saveLocationDetail(TestsHelper.getLocationByID(id), cDgEnv.Rows[e.RowIndex].Cells[2].Value.ToString());
                    cDgEnv.Rows[e.RowIndex].Cells[0].Value = id;
                    cDgEnv.Rows[e.RowIndex].Cells[1].Value = name;
                    }
                return;
                }
            if (e.ColumnIndex == 1) TestsHelper.saveLocation((long)cDgEnv.Rows[e.RowIndex].Cells[0].Value, cDgEnv.Rows[e.RowIndex].Cells[1].Value.ToString());
            else if (e.ColumnIndex == 2) TestsHelper.saveLocationDetail(TestsHelper.getLocationByID((long)cDgEnv.Rows[e.RowIndex].Cells[0].Value), cDgEnv.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        //_______________________________________________________________________________________________________________________
        private void CDgVars_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            Location loc = (Location)TestObjectBase.LinkedObject(sLnkCurrentLocation);
            if (loc == null) return;
            if (cDgVars.Rows[e.RowIndex].Cells[0].Value.Equals(DBNull.Value)) {
                if (e.ColumnIndex == 1) {
                    long id = TestsHelper.saveVariable(TestObjectBase.NO_ID, cDgVars.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cDgVars.Rows[e.RowIndex].Cells[0].Value = id;
                    }
                else if (e.ColumnIndex == 2 && cDgVars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "") {
                    string name = "$tstVar" + cDgVars.Rows.Count;
                    long id = TestsHelper.saveVariable(TestObjectBase.NO_ID, name);
                    TestsHelper.setLocationVariableValue(loc, id, cDgVars.Rows[e.RowIndex].Cells[2].Value.ToString());
                    cDgVars.Rows[e.RowIndex].Cells[0].Value = id;
                    cDgVars.Rows[e.RowIndex].Cells[1].Value = name;
                    }
                else if (e.ColumnIndex == 3 && cDgVars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "") {
                    string name = "$tstVar" + cDgVars.Rows.Count;
                    long id = TestsHelper.saveVariable(TestObjectBase.NO_ID, name);
                    TestsHelper.saveVariableDetail(TestsHelper.getVariableByID(id), cDgVars.Rows[e.RowIndex].Cells[3].Value.ToString());
                    cDgVars.Rows[e.RowIndex].Cells[0].Value = id;
                    cDgVars.Rows[e.RowIndex].Cells[1].Value = name;
                    }
                return;
                }
            if (e.ColumnIndex == 1) TestsHelper.saveVariable((long)cDgVars.Rows[e.RowIndex].Cells[0].Value, cDgVars.Rows[e.RowIndex].Cells[1].Value.ToString());
            else if (e.ColumnIndex == 2) {
                TestsHelper.saveLocationVariable(loc, TestsHelper.getVariableByID((long)cDgVars.Rows[e.RowIndex].Cells[0].Value));
                TestsHelper.setLocationVariableValue(loc, (long)cDgVars.Rows[e.RowIndex].Cells[0].Value, cDgVars.Rows[e.RowIndex].Cells[2].Value.ToString());
                }
            else if (e.ColumnIndex == 3) TestsHelper.saveVariableDetail(TestsHelper.getVariableByID((long)cDgVars.Rows[e.RowIndex].Cells[0].Value), cDgVars.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        //_______________________________________________________________________________________________________________________
        private void CDgVars_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
            if (e.ColumnIndex == cDgVars.Columns["Name"].Index) {
                cDgVars.Rows[e.RowIndex].ErrorText = "";

                // Don't try to validate the 'new row' until finished editing since there is not any point in validating its initial value. 
                if (cDgVars.Rows[e.RowIndex].IsNewRow) { return; }
                if (!e.FormattedValue.ToString().StartsWith("$tst")){
                    e.Cancel = true;
                    cDgVars.Rows[e.RowIndex].ErrorText = "The variable name must starts with $tst!";
                    return;
                    }
                for (int i = 0; i < cDgVars.Rows.Count; i++) 
                    if (i != e.RowIndex && e.FormattedValue.ToString().iEquals(cDgVars.Rows[i].Cells[1].FormattedValue.ToString())) {
                        e.Cancel = true;
                        cDgVars.Rows[e.RowIndex].ErrorText = "the variable name must be unique!";
                        }
                }
            }
        //_______________________________________________________________________________________________________________________
        private void CDgEnv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) { TestsHelper.deleteLocation(TestsHelper.getLocationByID((long)e.Row.Cells[0].Value)); }
        //_______________________________________________________________________________________________________________________
        private void CDgVars_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) { TestsHelper.deleteVariable(TestsHelper.getVariableByID((long)e.Row.Cells[0].Value)); }
        //_______________________________________________________________________________________________________________________
        private void CDgVars_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.ColumnIndex == 2 & cDgVars.Rows[e.RowIndex].Cells[1].Value  != null && !cDgVars.Rows[e.RowIndex].Cells[1].Value.Equals(DBNull.Value) && cDgVars.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower().Contains("password") && e.Value != null) {
                cDgVars.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('\u25CF', e.Value.ToString().Length);
                }
            }
        //_______________________________________________________________________________________________________________________
        private void CDgVars_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (cDgVars.CurrentCell.ColumnIndex == 2 && cDgVars.Rows[cDgVars.CurrentCell.RowIndex].Cells[1].Value.ToString().ToLower().Contains("password")){
                TextBox textBox = e.Control as TextBox;
                if (textBox != null) {
                    textBox.UseSystemPasswordChar = true;
                    if (!string.IsNullOrWhiteSpace(textBox.Text) && textBox.Text.Contains('\u25CF')) textBox.Text =(string) cDgVars.Rows[cDgVars.CurrentCell.RowIndex].Tag;
                    }
                }
            else {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null) textBox.UseSystemPasswordChar = false;
                }
            }
        }
    }
