namespace DigBit
{
    partial class BorrarLabo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BorrarLabo));
            this.rjButton1 = new DigBit.RJControls.RJButton();
            this.rjButton2 = new DigBit.RJControls.RJButton();
            this.cbLaboratorio = new DigBit.RJControls.RJComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.White;
            this.rjButton1.BackgroundColor = System.Drawing.Color.White;
            this.rjButton1.BorderColor = System.Drawing.Color.DarkGreen;
            this.rjButton1.BorderRadius = 40;
            this.rjButton1.BorderSize = 2;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton1.ForeColor = System.Drawing.Color.DarkGreen;
            this.rjButton1.Location = new System.Drawing.Point(31, 173);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(150, 40);
            this.rjButton1.TabIndex = 1;
            this.rjButton1.Text = "Borrar";
            this.rjButton1.TextColor = System.Drawing.Color.DarkGreen;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.White;
            this.rjButton2.BackgroundColor = System.Drawing.Color.White;
            this.rjButton2.BorderColor = System.Drawing.Color.DarkGreen;
            this.rjButton2.BorderRadius = 40;
            this.rjButton2.BorderSize = 2;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton2.ForeColor = System.Drawing.Color.DarkGreen;
            this.rjButton2.Location = new System.Drawing.Point(244, 173);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(150, 40);
            this.rjButton2.TabIndex = 2;
            this.rjButton2.Text = "Cancelar";
            this.rjButton2.TextColor = System.Drawing.Color.DarkGreen;
            this.rjButton2.UseVisualStyleBackColor = false;
            this.rjButton2.Click += new System.EventHandler(this.rjButton2_Click);
            // 
            // cbLaboratorio
            // 
            this.cbLaboratorio.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cbLaboratorio.BackColor = System.Drawing.Color.Green;
            this.cbLaboratorio.BackColor1 = System.Drawing.Color.WhiteSmoke;
            this.cbLaboratorio.BorderColor = System.Drawing.Color.Green;
            this.cbLaboratorio.BorderSize = 2;
            this.cbLaboratorio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLaboratorio.ForeColor = System.Drawing.Color.DimGray;
            this.cbLaboratorio.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cbLaboratorio.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.cbLaboratorio.ListTextColor = System.Drawing.Color.DimGray;
            this.cbLaboratorio.Location = new System.Drawing.Point(76, 94);
            this.cbLaboratorio.MinimumSize = new System.Drawing.Size(200, 30);
            this.cbLaboratorio.Name = "cbLaboratorio";
            this.cbLaboratorio.Padding = new System.Windows.Forms.Padding(2);
            this.cbLaboratorio.Size = new System.Drawing.Size(287, 30);
            this.cbLaboratorio.TabIndex = 3;
            this.cbLaboratorio.Texts = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Seleccione el laboratorio que sera borrado";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // BorrarLabo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(442, 236);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLaboratorio);
            this.Controls.Add(this.rjButton2);
            this.Controls.Add(this.rjButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BorrarLabo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "borrarlab";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RJControls.RJButton rjButton1;
        private RJControls.RJButton rjButton2;
        private RJControls.RJComboBox cbLaboratorio;
        private System.Windows.Forms.Label label1;
    }
}