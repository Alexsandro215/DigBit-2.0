namespace DigBit
{
    partial class Alumno_Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alumno_Principal));
            this.imgGobierno = new System.Windows.Forms.PictureBox();
            this.imgTeschi = new System.Windows.Forms.PictureBox();
            this.btnEditarPerfil = new DigBit.RJControls.RJButton();
            this.btnSalir = new DigBit.RJControls.RJButton();
            this.btnIngresarCodigoAcceso = new DigBit.RJControls.RJButton();
            this.txtBienvenido = new DigBit.RJControls.RJTextBox();
            this.btnCambiarUsuario = new DigBit.RJControls.RJButton();
            ((System.ComponentModel.ISupportInitialize)(this.imgGobierno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgTeschi)).BeginInit();
            this.SuspendLayout();
            // 
            // imgGobierno
            // 
            this.imgGobierno.BackColor = System.Drawing.Color.Transparent;
            this.imgGobierno.Image = ((System.Drawing.Image)(resources.GetObject("imgGobierno.Image")));
            this.imgGobierno.Location = new System.Drawing.Point(3, 12);
            this.imgGobierno.Name = "imgGobierno";
            this.imgGobierno.Size = new System.Drawing.Size(280, 50);
            this.imgGobierno.TabIndex = 2;
            this.imgGobierno.TabStop = false;
            // 
            // imgTeschi
            // 
            this.imgTeschi.BackColor = System.Drawing.Color.Transparent;
            this.imgTeschi.Image = ((System.Drawing.Image)(resources.GetObject("imgTeschi.Image")));
            this.imgTeschi.Location = new System.Drawing.Point(198, 359);
            this.imgTeschi.Name = "imgTeschi";
            this.imgTeschi.Size = new System.Drawing.Size(308, 142);
            this.imgTeschi.TabIndex = 5;
            this.imgTeschi.TabStop = false;
            // 
            // btnEditarPerfil
            // 
            this.btnEditarPerfil.BackColor = System.Drawing.Color.GhostWhite;
            this.btnEditarPerfil.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btnEditarPerfil.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnEditarPerfil.BorderRadius = 20;
            this.btnEditarPerfil.BorderSize = 2;
            this.btnEditarPerfil.FlatAppearance.BorderSize = 0;
            this.btnEditarPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditarPerfil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditarPerfil.ForeColor = System.Drawing.Color.Black;
            this.btnEditarPerfil.Location = new System.Drawing.Point(587, 436);
            this.btnEditarPerfil.Name = "btnEditarPerfil";
            this.btnEditarPerfil.Size = new System.Drawing.Size(106, 52);
            this.btnEditarPerfil.TabIndex = 10;
            this.btnEditarPerfil.Text = "Editar perfil";
            this.btnEditarPerfil.TextColor = System.Drawing.Color.Black;
            this.btnEditarPerfil.UseVisualStyleBackColor = false;
            this.btnEditarPerfil.Click += new System.EventHandler(this.btnEditarPerfil_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.White;
            this.btnSalir.BackgroundColor = System.Drawing.Color.White;
            this.btnSalir.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnSalir.BorderRadius = 20;
            this.btnSalir.BorderSize = 2;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.Black;
            this.btnSalir.Location = new System.Drawing.Point(240, 274);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(240, 31);
            this.btnSalir.TabIndex = 7;
            this.btnSalir.Text = "Salir";
            this.btnSalir.TextColor = System.Drawing.Color.Black;
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnIngresarCodigoAcceso
            // 
            this.btnIngresarCodigoAcceso.BackColor = System.Drawing.Color.White;
            this.btnIngresarCodigoAcceso.BackgroundColor = System.Drawing.Color.White;
            this.btnIngresarCodigoAcceso.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnIngresarCodigoAcceso.BorderRadius = 20;
            this.btnIngresarCodigoAcceso.BorderSize = 2;
            this.btnIngresarCodigoAcceso.FlatAppearance.BorderSize = 0;
            this.btnIngresarCodigoAcceso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresarCodigoAcceso.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresarCodigoAcceso.ForeColor = System.Drawing.Color.Black;
            this.btnIngresarCodigoAcceso.Location = new System.Drawing.Point(240, 116);
            this.btnIngresarCodigoAcceso.Name = "btnIngresarCodigoAcceso";
            this.btnIngresarCodigoAcceso.Size = new System.Drawing.Size(240, 31);
            this.btnIngresarCodigoAcceso.TabIndex = 6;
            this.btnIngresarCodigoAcceso.Text = "Ingresar código de acceso";
            this.btnIngresarCodigoAcceso.TextColor = System.Drawing.Color.Black;
            this.btnIngresarCodigoAcceso.UseVisualStyleBackColor = false;
            this.btnIngresarCodigoAcceso.Click += new System.EventHandler(this.btnIngresarCodigoAcceso_Click);
            // 
            // txtBienvenido
            // 
            this.txtBienvenido.BackColor = System.Drawing.SystemColors.Window;
            this.txtBienvenido.BorderColor = System.Drawing.Color.DarkGreen;
            this.txtBienvenido.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtBienvenido.BorderRadius = 20;
            this.txtBienvenido.BorderSize = 2;
            this.txtBienvenido.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBienvenido.ForeColor = System.Drawing.Color.Black;
            this.txtBienvenido.Location = new System.Drawing.Point(465, 13);
            this.txtBienvenido.Margin = new System.Windows.Forms.Padding(7, 4, 4, 4);
            this.txtBienvenido.Multiline = false;
            this.txtBienvenido.Name = "txtBienvenido";
            this.txtBienvenido.Padding = new System.Windows.Forms.Padding(7);
            this.txtBienvenido.PasswordChar = false;
            this.txtBienvenido.Size = new System.Drawing.Size(235, 33);
            this.txtBienvenido.TabIndex = 4;
            this.txtBienvenido.Texts = "BIENVENID@_________";
            this.txtBienvenido.UnderlinedStyle = false;
            // 
            // btnCambiarUsuario
            // 
            this.btnCambiarUsuario.BackColor = System.Drawing.Color.GhostWhite;
            this.btnCambiarUsuario.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btnCambiarUsuario.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnCambiarUsuario.BorderRadius = 20;
            this.btnCambiarUsuario.BorderSize = 2;
            this.btnCambiarUsuario.FlatAppearance.BorderSize = 0;
            this.btnCambiarUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarUsuario.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarUsuario.ForeColor = System.Drawing.Color.Black;
            this.btnCambiarUsuario.Location = new System.Drawing.Point(240, 194);
            this.btnCambiarUsuario.Name = "btnCambiarUsuario";
            this.btnCambiarUsuario.Size = new System.Drawing.Size(240, 31);
            this.btnCambiarUsuario.TabIndex = 0;
            this.btnCambiarUsuario.Text = "Cambiar de usuario";
            this.btnCambiarUsuario.TextColor = System.Drawing.Color.Black;
            this.btnCambiarUsuario.UseVisualStyleBackColor = false;
            this.btnCambiarUsuario.Click += new System.EventHandler(this.btnCambiarUsuario_Click);
            // 
            // Alumno_Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(721, 500);
            this.Controls.Add(this.btnEditarPerfil);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnIngresarCodigoAcceso);
            this.Controls.Add(this.imgTeschi);
            this.Controls.Add(this.txtBienvenido);
            this.Controls.Add(this.imgGobierno);
            this.Controls.Add(this.btnCambiarUsuario);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Alumno_Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alumno_Principal";
            ((System.ComponentModel.ISupportInitialize)(this.imgGobierno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgTeschi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RJControls.RJButton btnCambiarUsuario;
        private System.Windows.Forms.PictureBox imgGobierno;
        private RJControls.RJTextBox txtBienvenido;
        private System.Windows.Forms.PictureBox imgTeschi;
        private RJControls.RJButton btnSalir;
        private RJControls.RJButton btnEditarPerfil;
        private RJControls.RJButton btnIngresarCodigoAcceso;
    }
}