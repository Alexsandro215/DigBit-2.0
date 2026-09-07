namespace DigBit
{
    partial class editarLab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(editarLab));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cancelarLaboratorio = new DigBit.RJControls.RJButton();
            this.actualizarLaboratorio = new DigBit.RJControls.RJButton();
            this.txtLaboratorio = new DigBit.RJControls.RJTextBox();
            this.rjLaboratorioActualizar = new DigBit.RJControls.RJComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selecciona el laboratorio ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(187, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ingresa el nuevo nombre";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cancelarLaboratorio
            // 
            this.cancelarLaboratorio.BackColor = System.Drawing.Color.White;
            this.cancelarLaboratorio.BackgroundColor = System.Drawing.Color.White;
            this.cancelarLaboratorio.BorderColor = System.Drawing.Color.DarkGreen;
            this.cancelarLaboratorio.BorderRadius = 40;
            this.cancelarLaboratorio.BorderSize = 2;
            this.cancelarLaboratorio.FlatAppearance.BorderSize = 0;
            this.cancelarLaboratorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelarLaboratorio.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelarLaboratorio.ForeColor = System.Drawing.Color.DarkGreen;
            this.cancelarLaboratorio.Location = new System.Drawing.Point(244, 183);
            this.cancelarLaboratorio.Name = "cancelarLaboratorio";
            this.cancelarLaboratorio.Size = new System.Drawing.Size(150, 40);
            this.cancelarLaboratorio.TabIndex = 2;
            this.cancelarLaboratorio.Text = "Cancelar";
            this.cancelarLaboratorio.TextColor = System.Drawing.Color.DarkGreen;
            this.cancelarLaboratorio.UseVisualStyleBackColor = false;
            this.cancelarLaboratorio.Click += new System.EventHandler(this.rjButton2_Click);
            // 
            // actualizarLaboratorio
            // 
            this.actualizarLaboratorio.BackColor = System.Drawing.Color.White;
            this.actualizarLaboratorio.BackgroundColor = System.Drawing.Color.White;
            this.actualizarLaboratorio.BorderColor = System.Drawing.Color.DarkGreen;
            this.actualizarLaboratorio.BorderRadius = 40;
            this.actualizarLaboratorio.BorderSize = 2;
            this.actualizarLaboratorio.FlatAppearance.BorderSize = 0;
            this.actualizarLaboratorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.actualizarLaboratorio.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actualizarLaboratorio.ForeColor = System.Drawing.Color.DarkGreen;
            this.actualizarLaboratorio.Location = new System.Drawing.Point(12, 183);
            this.actualizarLaboratorio.Name = "actualizarLaboratorio";
            this.actualizarLaboratorio.Size = new System.Drawing.Size(150, 40);
            this.actualizarLaboratorio.TabIndex = 1;
            this.actualizarLaboratorio.Text = "Actualizar";
            this.actualizarLaboratorio.TextColor = System.Drawing.Color.DarkGreen;
            this.actualizarLaboratorio.UseVisualStyleBackColor = false;
            this.actualizarLaboratorio.Click += new System.EventHandler(this.actualizarLaboratorio_Click);
            // 
            // txtLaboratorio
            // 
            this.txtLaboratorio.BackColor = System.Drawing.SystemColors.Window;
            this.txtLaboratorio.BorderColor = System.Drawing.Color.GhostWhite;
            this.txtLaboratorio.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtLaboratorio.BorderRadius = 0;
            this.txtLaboratorio.BorderSize = 2;
            this.txtLaboratorio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLaboratorio.ForeColor = System.Drawing.Color.DimGray;
            this.txtLaboratorio.Location = new System.Drawing.Point(176, 124);
            this.txtLaboratorio.Margin = new System.Windows.Forms.Padding(4);
            this.txtLaboratorio.Multiline = false;
            this.txtLaboratorio.Name = "txtLaboratorio";
            this.txtLaboratorio.Padding = new System.Windows.Forms.Padding(7);
            this.txtLaboratorio.PasswordChar = false;
            this.txtLaboratorio.Size = new System.Drawing.Size(250, 31);
            this.txtLaboratorio.TabIndex = 0;
            this.txtLaboratorio.Texts = "";
            this.txtLaboratorio.UnderlinedStyle = false;
            // 
            // rjLaboratorioActualizar
            // 
            this.rjLaboratorioActualizar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.rjLaboratorioActualizar.BackColor = System.Drawing.Color.DarkGreen;
            this.rjLaboratorioActualizar.BackColor1 = System.Drawing.Color.WhiteSmoke;
            this.rjLaboratorioActualizar.BorderColor = System.Drawing.Color.DarkGreen;
            this.rjLaboratorioActualizar.BorderSize = 1;
            this.rjLaboratorioActualizar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rjLaboratorioActualizar.ForeColor = System.Drawing.Color.DimGray;
            this.rjLaboratorioActualizar.IconColor = System.Drawing.Color.DarkGreen;
            this.rjLaboratorioActualizar.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.rjLaboratorioActualizar.ListTextColor = System.Drawing.Color.DimGray;
            this.rjLaboratorioActualizar.Location = new System.Drawing.Point(12, 35);
            this.rjLaboratorioActualizar.MinimumSize = new System.Drawing.Size(200, 30);
            this.rjLaboratorioActualizar.Name = "rjLaboratorioActualizar";
            this.rjLaboratorioActualizar.Padding = new System.Windows.Forms.Padding(1);
            this.rjLaboratorioActualizar.Size = new System.Drawing.Size(200, 30);
            this.rjLaboratorioActualizar.TabIndex = 6;
            this.rjLaboratorioActualizar.Texts = "";
            // 
            // editarLab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(440, 235);
            this.Controls.Add(this.rjLaboratorioActualizar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelarLaboratorio);
            this.Controls.Add(this.actualizarLaboratorio);
            this.Controls.Add(this.txtLaboratorio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "editarLab";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "editarLab";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RJControls.RJTextBox txtLaboratorio;
        private RJControls.RJButton actualizarLaboratorio;
        private RJControls.RJButton cancelarLaboratorio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private RJControls.RJComboBox rjLaboratorioActualizar;
    }
}