namespace FIMTestConfigurator {
    partial class frmVariables {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lStBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.cDgEnv = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.cDgVars = new System.Windows.Forms.DataGridView();
            this.lError = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cDgEnv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cDgVars)).BeginInit();
            this.SuspendLayout();
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bClose.Location = new System.Drawing.Point(782, 3);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(97, 23);
            this.bClose.TabIndex = 1;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lStBar,
            this.lError});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lStBar
            // 
            this.lStBar.Name = "lStBar";
            this.lStBar.Size = new System.Drawing.Size(13, 17);
            this.lStBar.Text = "  ";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cDgEnv);
            this.splitContainer1.Panel1.Controls.Add(this.bClose);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.cDgVars);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.splitContainer1.Size = new System.Drawing.Size(884, 539);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Locations:";
            // 
            // cDgEnv
            // 
            this.cDgEnv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cDgEnv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.cDgEnv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cDgEnv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cDgEnv.DefaultCellStyle = dataGridViewCellStyle1;
            this.cDgEnv.Location = new System.Drawing.Point(0, 29);
            this.cDgEnv.MultiSelect = false;
            this.cDgEnv.Name = "cDgEnv";
            this.cDgEnv.Size = new System.Drawing.Size(882, 223);
            this.cDgEnv.TabIndex = 0;
            this.cDgEnv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CDgEnv_CellValueChanged);
            this.cDgEnv.SelectionChanged += new System.EventHandler(this.CDgEnv_SelectionChanged);
            this.cDgEnv.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.CDgEnv_UserDeletingRow);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Variables:";
            // 
            // cDgVars
            // 
            this.cDgVars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cDgVars.BackgroundColor = System.Drawing.SystemColors.Control;
            this.cDgVars.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cDgVars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cDgVars.Location = new System.Drawing.Point(0, 27);
            this.cDgVars.MultiSelect = false;
            this.cDgVars.Name = "cDgVars";
            this.cDgVars.Size = new System.Drawing.Size(882, 249);
            this.cDgVars.TabIndex = 0;
            this.cDgVars.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.CDgVars_CellFormatting);
            this.cDgVars.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.CDgVars_CellValidating);
            this.cDgVars.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CDgVars_CellValueChanged);
            this.cDgVars.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.CDgVars_EditingControlShowing);
            this.cDgVars.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.CDgVars_UserDeletingRow);
            // 
            // lError
            // 
            this.lError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lError.ForeColor = System.Drawing.Color.Red;
            this.lError.Name = "lError";
            this.lError.Size = new System.Drawing.Size(35, 17);
            this.lError.Text = "Error";
            this.lError.Visible = false;
            // 
            // frmVariables
            // 
            this.AcceptButton = this.bClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bClose;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmVariables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Variables Editor";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cDgEnv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cDgVars)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView cDgEnv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView cDgVars;
        private System.Windows.Forms.ToolStripStatusLabel lStBar;
        private System.Windows.Forms.ToolStripStatusLabel lError;
        }
    }