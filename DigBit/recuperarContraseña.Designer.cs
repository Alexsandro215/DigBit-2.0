namespace DigBit
{
    partial class recuperarContraseña
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(recuperarContraseña));
            this.imgRecuperarContraseña = new System.Windows.Forms.PictureBox();
            this.btnCancelar = new DigBit.RJControls.RJButton();
            this.imgUsuario = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMatricula = new System.Windows.Forms.TextBox();
            this.imgCheck = new System.Windows.Forms.PictureBox();
            this.txtRespuesta = new System.Windows.Forms.TextBox();
            this.lblPasswd = new System.Windows.Forms.Label();
            this.btnAceptar = new DigBit.RJControls.RJButton();
            this.cboxPreguntaSeguridad = new DigBit.RJControls.RJComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgRecuperarContraseña)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgUsuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // imgRecuperarContraseña
            // 
            this.imgRecuperarContraseña.BackgroundImage = global::DigBit.Properties.Resources.fondoRecContr;
            this.imgRecuperarContraseña.Image = ((System.Drawing.Image)(resources.GetObject("imgRecuperarContraseña.Image")));
            this.imgRecuperarContraseña.Location = new System.Drawing.Point(-10, -1);
            this.imgRecuperarContraseña.Name = "imgRecuperarContraseña";
            this.imgRecuperarContraseña.Size = new System.Drawing.Size(322, 408);
            this.imgRecuperarContraseña.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgRecuperarContraseña.TabIndex = 0;
            this.imgRecuperarContraseña.TabStop = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancelar.BackgroundColor = System.Drawing.SystemColors.Window;
            this.btnCancelar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnCancelar.BorderRadius = 20;
            this.btnCancelar.BorderSize = 2;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnCancelar.Location = new System.Drawing.Point(12, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(107, 30);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.TextColor = System.Drawing.Color.DarkGreen;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // imgUsuario
            // 
            this.imgUsuario.BackColor = System.Drawing.Color.White;
            this.imgUsuario.Image = ((System.Drawing.Image)(resources.GetObject("imgUsuario.Image")));
            this.imgUsuario.Location = new System.Drawing.Point(9, 112);
            this.imgUsuario.Name = "imgUsuario";
            this.imgUsuario.Size = new System.Drawing.Size(53, 58);
            this.imgUsuario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgUsuario.TabIndex = 2;
            this.imgUsuario.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ForeColor = System.Drawing.Color.DarkGreen;
            this.label1.Location = new System.Drawing.Point(64, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "________________________";
            // 
            // txtMatricula
            // 
            this.txtMatricula.BackColor = System.Drawing.Color.White;
            this.txtMatricula.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMatricula.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatricula.ForeColor = System.Drawing.Color.DarkGreen;
            this.txtMatricula.Location = new System.Drawing.Point(66, 138);
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Size = new System.Drawing.Size(151, 19);
            this.txtMatricula.TabIndex = 4;
            this.txtMatricula.Text = "Ingrese la matricula";
            this.txtMatricula.Enter += new System.EventHandler(this.txtMatricula_Enter);
            this.txtMatricula.Leave += new System.EventHandler(this.txtMatricula_Leave);
            // 
            // imgCheck
            // 
            this.imgCheck.BackColor = System.Drawing.Color.White;
            this.imgCheck.Image = ((System.Drawing.Image)(resources.GetObject("imgCheck.Image")));
            this.imgCheck.Location = new System.Drawing.Point(9, 232);
            this.imgCheck.Name = "imgCheck";
            this.imgCheck.Size = new System.Drawing.Size(56, 47);
            this.imgCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCheck.TabIndex = 6;
            this.imgCheck.TabStop = false;
            // 
            // txtRespuesta
            // 
            this.txtRespuesta.BackColor = System.Drawing.Color.White;
            this.txtRespuesta.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRespuesta.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRespuesta.ForeColor = System.Drawing.Color.DarkGreen;
            this.txtRespuesta.Location = new System.Drawing.Point(69, 249);
            this.txtRespuesta.Name = "txtRespuesta";
            this.txtRespuesta.Size = new System.Drawing.Size(170, 19);
            this.txtRespuesta.TabIndex = 7;
            this.txtRespuesta.Text = "Ingrese la respuesta";
            this.txtRespuesta.Enter += new System.EventHandler(this.txtRespuesta_Enter);
            this.txtRespuesta.Leave += new System.EventHandler(this.txtRespuesta_Leave);
            // 
            // lblPasswd
            // 
            this.lblPasswd.AutoSize = true;
            this.lblPasswd.BackColor = System.Drawing.Color.Transparent;
            this.lblPasswd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswd.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblPasswd.Location = new System.Drawing.Point(64, 261);
            this.lblPasswd.Name = "lblPasswd";
            this.lblPasswd.Size = new System.Drawing.Size(182, 16);
            this.lblPasswd.TabIndex = 8;
            this.lblPasswd.Text = "_________________________";
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.SystemColors.Window;
            this.btnAceptar.BackgroundColor = System.Drawing.SystemColors.Window;
            this.btnAceptar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnAceptar.BorderRadius = 20;
            this.btnAceptar.BorderSize = 2;
            this.btnAceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAceptar.FlatAppearance.BorderSize = 0;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnAceptar.Location = new System.Drawing.Point(181, 349);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(107, 30);
            this.btnAceptar.TabIndex = 9;
            this.btnAceptar.Text = "ACEPTAR";
            this.btnAceptar.TextColor = System.Drawing.Color.DarkGreen;
            this.btnAceptar.UseVisualStyleBackColor = false;
            // 
            // cboxPreguntaSeguridad
            // 
            this.cboxPreguntaSeguridad.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboxPreguntaSeguridad.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboxPreguntaSeguridad.BackColor = System.Drawing.Color.DarkGreen;
            this.cboxPreguntaSeguridad.BackColor1 = System.Drawing.Color.WhiteSmoke;
            this.cboxPreguntaSeguridad.BorderColor = System.Drawing.Color.DarkGreen;
            this.cboxPreguntaSeguridad.BorderSize = 2;
            this.cboxPreguntaSeguridad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPreguntaSeguridad.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxPreguntaSeguridad.ForeColor = System.Drawing.Color.Green;
            this.cboxPreguntaSeguridad.IconColor = System.Drawing.Color.DarkGreen;
            this.cboxPreguntaSeguridad.Items.AddRange(new object[] {
            "",
            "¿Cuál es el nombre de tu perro?",
            "¿Cuál es el nombre de tu mamá?",
            "¿Cuál es tu fecha de cumpleaños?",
            "¿Cuál es el nombre de tu papá?",
            "¿Cuál es tu número de teléfono?"});
            this.cboxPreguntaSeguridad.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.cboxPreguntaSeguridad.ListTextColor = System.Drawing.Color.DimGray;
            this.cboxPreguntaSeguridad.Location = new System.Drawing.Point(12, 182);
            this.cboxPreguntaSeguridad.MinimumSize = new System.Drawing.Size(200, 30);
            this.cboxPreguntaSeguridad.Name = "cboxPreguntaSeguridad";
            this.cboxPreguntaSeguridad.Padding = new System.Windows.Forms.Padding(2);
            this.cboxPreguntaSeguridad.Size = new System.Drawing.Size(264, 40);
            this.cboxPreguntaSeguridad.TabIndex = 10;
            this.cboxPreguntaSeguridad.Texts = "Selecciona la pregunta de seguridad";
            // 
            // recuperarContraseña
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Controls.Add(this.cboxPreguntaSeguridad);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.lblPasswd);
            this.Controls.Add(this.txtRespuesta);
            this.Controls.Add(this.imgCheck);
            this.Controls.Add(this.txtMatricula);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imgUsuario);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.imgRecuperarContraseña);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "recuperarContraseña";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Recuperar Contraseña";
            this.Load += new System.EventHandler(this.recuperarContraseña_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgRecuperarContraseña)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgUsuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imgRecuperarContraseña;
        private RJControls.RJButton btnCancelar;
        private System.Windows.Forms.PictureBox imgUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMatricula;
        private System.Windows.Forms.PictureBox imgCheck;
        private System.Windows.Forms.TextBox txtRespuesta;
        private System.Windows.Forms.Label lblPasswd;
        private RJControls.RJButton btnAceptar;
        private RJControls.RJComboBox cboxPreguntaSeguridad;
    }
}