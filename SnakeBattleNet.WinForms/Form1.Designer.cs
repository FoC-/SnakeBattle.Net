namespace SnakeBattleNet.WinForms
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.boardGrid = new System.Windows.Forms.DataGridView();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.boardGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // boardGrid
            // 
            this.boardGrid.AllowUserToAddRows = false;
            this.boardGrid.AllowUserToDeleteRows = false;
            this.boardGrid.AllowUserToResizeColumns = false;
            this.boardGrid.AllowUserToResizeRows = false;
            this.boardGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.boardGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.boardGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.boardGrid.ColumnHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.boardGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.boardGrid.Location = new System.Drawing.Point(3, 2);
            this.boardGrid.MultiSelect = false;
            this.boardGrid.Name = "boardGrid";
            this.boardGrid.ReadOnly = true;
            this.boardGrid.RowHeadersVisible = false;
            this.boardGrid.RowHeadersWidth = 23;
            this.boardGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.boardGrid.RowTemplate.Height = 100;
            this.boardGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.boardGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.boardGrid.ShowCellErrors = false;
            this.boardGrid.ShowCellToolTips = false;
            this.boardGrid.ShowEditingIcon = false;
            this.boardGrid.ShowRowErrors = false;
            this.boardGrid.Size = new System.Drawing.Size(408, 458);
            this.boardGrid.TabIndex = 0;
            this.boardGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.BoardGridCellPainting);
            this.boardGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(417, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Act";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(417, 31);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(70, 23);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Timer";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(489, 465);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.boardGrid);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SnakeBattleNet";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.boardGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView boardGrid;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Timer timer1;
    }
}

