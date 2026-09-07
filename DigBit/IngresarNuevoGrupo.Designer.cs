namespace DigBit
{
    partial class IngresarNuevoGrupo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IngresarNuevoGrupo));
            this.lblIngresarGrupo = new System.Windows.Forms.Label();
            this.btnCancelar = new DigBit.RJControls.RJButton();
            this.btnGuardar = new DigBit.RJControls.RJButton();
            this.txtGrupo = new DigBit.RJControls.RJTextBox();
            this.cbCarrera = new DigBit.RJControls.RJComboBox();
            this.SuspendLayout();
            // 
            // lblIngresarGrupo
            // 
            this.lblIngresarGrupo.AutoSize = true;
            this.lblIngresarGrupo.BackColor = System.Drawing.Color.Transparent;
            this.lblIngresarGrupo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIngresarGrupo.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblIngresarGrupo.Location = new System.Drawing.Point(140, 39);
            this.lblIngresarGrupo.Name = "lblIngresarGrupo";
            this.lblIngresarGrupo.Size = new System.Drawing.Size(170, 19);
            this.lblIngresarGrupo.TabIndex = 0;
            this.lblIngresarGrupo.Text = "Ingresa nuevo grupo";
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.BackgroundColor = System.Drawing.Color.White;
            this.btnCancelar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnCancelar.BorderRadius = 20;
            this.btnCancelar.BorderSize = 2;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Black;
            this.btnCancelar.Location = new System.Drawing.Point(41, 172);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(119, 40);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextColor = System.Drawing.Color.Black;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.White;
            this.btnGuardar.BackgroundColor = System.Drawing.Color.White;
            this.btnGuardar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnGuardar.BorderRadius = 20;
            this.btnGuardar.BorderSize = 2;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.Color.Black;
            this.btnGuardar.Location = new System.Drawing.Point(281, 172);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(119, 40);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.TextColor = System.Drawing.Color.Black;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // txtGrupo
            // 
            this.txtGrupo.BackColor = System.Drawing.SystemColors.Window;
            this.txtGrupo.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtGrupo.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtGrupo.BorderRadius = 10;
            this.txtGrupo.BorderSize = 2;
            this.txtGrupo.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrupo.ForeColor = System.Drawing.Color.DimGray;
            this.txtGrupo.Location = new System.Drawing.Point(100, 109);
            this.txtGrupo.Margin = new System.Windows.Forms.Padding(4);
            this.txtGrupo.Multiline = false;
            this.txtGrupo.Name = "txtGrupo";
            this.txtGrupo.Padding = new System.Windows.Forms.Padding(100, 7, 7, 7);
            this.txtGrupo.PasswordChar = false;
            this.txtGrupo.Size = new System.Drawing.Size(250, 31);
            this.txtGrupo.TabIndex = 2;
            this.txtGrupo.Texts = "Grupo";
            this.txtGrupo.UnderlinedStyle = false;
            // 
            // cbCarrera
            // 
            this.cbCarrera.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cbCarrera.BackColor = System.Drawing.Color.DarkGreen;
            this.cbCarrera.BackColor1 = System.Drawing.Color.WhiteSmoke;
            this.cbCarrera.BorderColor = System.Drawing.Color.DarkGreen;
            this.cbCarrera.BorderSize = 2;
            this.cbCarrera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCarrera.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCarrera.ForeColor = System.Drawing.Color.DimGray;
            this.cbCarrera.IconColor = System.Drawing.Color.DarkGreen;
            this.cbCarrera.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.cbCarrera.ListTextColor = System.Drawing.Color.DimGray;
            this.cbCarrera.Location = new System.Drawing.Point(91, 72);
            this.cbCarrera.MinimumSize = new System.Drawing.Size(200, 30);
            this.cbCarrera.Name = "cbCarrera";
            this.cbCarrera.Padding = new System.Windows.Forms.Padding(2);
            this.cbCarrera.Size = new System.Drawing.Size(265, 30);
            this.cbCarrera.TabIndex = 1;
            this.cbCarrera.Texts = "Seleccione carrera";
            // 
            // IngresarNuevoGrupo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(444, 236);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtGrupo);
            this.Controls.Add(this.cbCarrera);
            this.Controls.Add(this.lblIngresarGrupo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IngresarNuevoGrupo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ingresar nuevo grupo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIngresarGrupo;
        private RJControls.RJComboBox cbCarrera;
        private RJControls.RJTextBox txtGrupo;
        private RJControls.RJButton btnGuardar;
        private RJControls.RJButton btnCancelar;
    }
}