namespace DigBit
{
    partial class RegistroProfesores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistroProfesores));
            this.btnRegistro = new DigBit.RJControls.RJButton();
            this.btnCancelar = new DigBit.RJControls.RJButton();
            this.imgNombre = new System.Windows.Forms.PictureBox();
            this.txtNombre = new DigBit.RJControls.RJTextBox();
            this.imgMatricula = new System.Windows.Forms.PictureBox();
            this.imgApellidoP = new System.Windows.Forms.PictureBox();
            this.imgApellidoM = new System.Windows.Forms.PictureBox();
            this.txtApellidoM = new DigBit.RJControls.RJTextBox();
            this.txtMatricula = new DigBit.RJControls.RJTextBox();
            this.txtApellidoP = new DigBit.RJControls.RJTextBox();
            this.imgContraseña = new System.Windows.Forms.PictureBox();
            this.imgCarrera = new System.Windows.Forms.PictureBox();
            this.txtPassword = new DigBit.RJControls.RJTextBox();
            this.cbCarrera = new DigBit.RJControls.RJComboBox();
            this.imgCicloEscolar = new System.Windows.Forms.PictureBox();
            this.btnProfesores = new DigBit.RJControls.Btnradius();
            this.rjTcorreo = new DigBit.RJControls.RJTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgNombre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMatricula)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgApellidoP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgApellidoM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgContraseña)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCarrera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCicloEscolar)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRegistro
            // 
            this.btnRegistro.BackColor = System.Drawing.SystemColors.Window;
            this.btnRegistro.BackgroundColor = System.Drawing.SystemColors.Window;
            this.btnRegistro.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnRegistro.BorderRadius = 20;
            this.btnRegistro.BorderSize = 2;
            this.btnRegistro.FlatAppearance.BorderSize = 0;
            this.btnRegistro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistro.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistro.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnRegistro.Location = new System.Drawing.Point(298, 177);
            this.btnRegistro.Name = "btnRegistro";
            this.btnRegistro.Size = new System.Drawing.Size(92, 39);
            this.btnRegistro.TabIndex = 1;
            this.btnRegistro.Text = "REGISTRAR";
            this.btnRegistro.TextColor = System.Drawing.Color.DarkGreen;
            this.btnRegistro.UseVisualStyleBackColor = false;
            this.btnRegistro.Click += new System.EventHandler(this.RegistrarProfe_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.BackgroundColor = System.Drawing.Color.White;
            this.btnCancelar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(116)))), ((int)(((byte)(60)))));
            this.btnCancelar.BorderRadius = 20;
            this.btnCancelar.BorderSize = 2;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnCancelar.Location = new System.Drawing.Point(298, 219);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(92, 39);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.TextColor = System.Drawing.Color.DarkGreen;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // imgNombre
            // 
            this.imgNombre.BackColor = System.Drawing.Color.Transparent;
            this.imgNombre.Image = ((System.Drawing.Image)(resources.GetObject("imgNombre.Image")));
            this.imgNombre.Location = new System.Drawing.Point(5, 65);
            this.imgNombre.Name = "imgNombre";
            this.imgNombre.Size = new System.Drawing.Size(30, 34);
            this.imgNombre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgNombre.TabIndex = 15;
            this.imgNombre.TabStop = false;
            // 
            // txtNombre
            // 
            this.txtNombre.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNombre.BackColor = System.Drawing.SystemColors.Window;
            this.txtNombre.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtNombre.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtNombre.BorderRadius = 5;
            this.txtNombre.BorderSize = 2;
            this.txtNombre.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.ForeColor = System.Drawing.Color.Black;
            this.txtNombre.Location = new System.Drawing.Point(13, 79);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4);
            this.txtNombre.Multiline = true;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Padding = new System.Windows.Forms.Padding(90, 7, 50, 7);
            this.txtNombre.PasswordChar = false;
            this.txtNombre.Size = new System.Drawing.Size(271, 30);
            this.txtNombre.TabIndex = 14;
            this.txtNombre.Texts = "Nombre";
            this.txtNombre.UnderlinedStyle = false;
            this.txtNombre._TextChanged += new System.EventHandler(this.txtNombre__TextChanged);
            this.txtNombre.Enter += new System.EventHandler(this.txtNombre_Enter);
            this.txtNombre.Leave += new System.EventHandler(this.txtNombre_Leave);
            // 
            // imgMatricula
            // 
            this.imgMatricula.BackColor = System.Drawing.Color.Transparent;
            this.imgMatricula.Image = ((System.Drawing.Image)(resources.GetObject("imgMatricula.Image")));
            this.imgMatricula.Location = new System.Drawing.Point(1, 203);
            this.imgMatricula.Name = "imgMatricula";
            this.imgMatricula.Size = new System.Drawing.Size(34, 31);
            this.imgMatricula.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgMatricula.TabIndex = 26;
            this.imgMatricula.TabStop = false;
            // 
            // imgApellidoP
            // 
            this.imgApellidoP.BackColor = System.Drawing.Color.Transparent;
            this.imgApellidoP.Image = ((System.Drawing.Image)(resources.GetObject("imgApellidoP.Image")));
            this.imgApellidoP.Location = new System.Drawing.Point(5, 117);
            this.imgApellidoP.Name = "imgApellidoP";
            this.imgApellidoP.Size = new System.Drawing.Size(30, 34);
            this.imgApellidoP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgApellidoP.TabIndex = 25;
            this.imgApellidoP.TabStop = false;
            // 
            // imgApellidoM
            // 
            this.imgApellidoM.BackColor = System.Drawing.Color.Transparent;
            this.imgApellidoM.Image = ((System.Drawing.Image)(resources.GetObject("imgApellidoM.Image")));
            this.imgApellidoM.Location = new System.Drawing.Point(3, 158);
            this.imgApellidoM.Name = "imgApellidoM";
            this.imgApellidoM.Size = new System.Drawing.Size(30, 34);
            this.imgApellidoM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgApellidoM.TabIndex = 24;
            this.imgApellidoM.TabStop = false;
            // 
            // txtApellidoM
            // 
            this.txtApellidoM.BackColor = System.Drawing.SystemColors.Window;
            this.txtApellidoM.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtApellidoM.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtApellidoM.BorderRadius = 5;
            this.txtApellidoM.BorderSize = 2;
            this.txtApellidoM.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellidoM.ForeColor = System.Drawing.Color.Black;
            this.txtApellidoM.Location = new System.Drawing.Point(13, 173);
            this.txtApellidoM.Margin = new System.Windows.Forms.Padding(4);
            this.txtApellidoM.Multiline = true;
            this.txtApellidoM.Name = "txtApellidoM";
            this.txtApellidoM.Padding = new System.Windows.Forms.Padding(85, 7, 70, 7);
            this.txtApellidoM.PasswordChar = false;
            this.txtApellidoM.Size = new System.Drawing.Size(271, 30);
            this.txtApellidoM.TabIndex = 23;
            this.txtApellidoM.Texts = "Apellido Materno";
            this.txtApellidoM.UnderlinedStyle = false;
            this.txtApellidoM._TextChanged += new System.EventHandler(this.txtApellidoM__TextChanged);
            this.txtApellidoM.Enter += new System.EventHandler(this.txtApellidoM_Enter);
            this.txtApellidoM.Leave += new System.EventHandler(this.txtApellidoM_Leave);
            // 
            // txtMatricula
            // 
            this.txtMatricula.BackColor = System.Drawing.SystemColors.Window;
            this.txtMatricula.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtMatricula.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtMatricula.BorderRadius = 5;
            this.txtMatricula.BorderSize = 2;
            this.txtMatricula.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatricula.ForeColor = System.Drawing.Color.Black;
            this.txtMatricula.Location = new System.Drawing.Point(13, 218);
            this.txtMatricula.Margin = new System.Windows.Forms.Padding(4);
            this.txtMatricula.Multiline = true;
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Padding = new System.Windows.Forms.Padding(90, 7, 90, 7);
            this.txtMatricula.PasswordChar = false;
            this.txtMatricula.Size = new System.Drawing.Size(271, 30);
            this.txtMatricula.TabIndex = 22;
            this.txtMatricula.Texts = "Numero de empleado";
            this.txtMatricula.UnderlinedStyle = false;
            this.txtMatricula._TextChanged += new System.EventHandler(this.txtMatricula__TextChanged);
            this.txtMatricula.Enter += new System.EventHandler(this.txtMatricula_Enter);
            this.txtMatricula.Leave += new System.EventHandler(this.txtMatricula_Leave);
            // 
            // txtApellidoP
            // 
            this.txtApellidoP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApellidoP.BackColor = System.Drawing.SystemColors.Window;
            this.txtApellidoP.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtApellidoP.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtApellidoP.BorderRadius = 5;
            this.txtApellidoP.BorderSize = 2;
            this.txtApellidoP.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellidoP.ForeColor = System.Drawing.Color.Black;
            this.txtApellidoP.Location = new System.Drawing.Point(13, 129);
            this.txtApellidoP.Margin = new System.Windows.Forms.Padding(4);
            this.txtApellidoP.Multiline = true;
            this.txtApellidoP.Name = "txtApellidoP";
            this.txtApellidoP.Padding = new System.Windows.Forms.Padding(90, 7, 80, 7);
            this.txtApellidoP.PasswordChar = false;
            this.txtApellidoP.Size = new System.Drawing.Size(271, 30);
            this.txtApellidoP.TabIndex = 27;
            this.txtApellidoP.Texts = "Apellido Paterno";
            this.txtApellidoP.UnderlinedStyle = false;
            this.txtApellidoP._TextChanged += new System.EventHandler(this.txtApellidoP__TextChanged);
            this.txtApellidoP.Enter += new System.EventHandler(this.txtApellidoP_Enter);
            this.txtApellidoP.Leave += new System.EventHandler(this.txtApellidoP_Leave);
            // 
            // imgContraseña
            // 
            this.imgContraseña.BackColor = System.Drawing.Color.Transparent;
            this.imgContraseña.Image = ((System.Drawing.Image)(resources.GetObject("imgContraseña.Image")));
            this.imgContraseña.Location = new System.Drawing.Point(2, 296);
            this.imgContraseña.Name = "imgContraseña";
            this.imgContraseña.Size = new System.Drawing.Size(34, 31);
            this.imgContraseña.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgContraseña.TabIndex = 30;
            this.imgContraseña.TabStop = false;
            // 
            // imgCarrera
            // 
            this.imgCarrera.BackColor = System.Drawing.Color.Transparent;
            this.imgCarrera.Image = ((System.Drawing.Image)(resources.GetObject("imgCarrera.Image")));
            this.imgCarrera.Location = new System.Drawing.Point(1, 248);
            this.imgCarrera.Name = "imgCarrera";
            this.imgCarrera.Size = new System.Drawing.Size(34, 31);
            this.imgCarrera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCarrera.TabIndex = 29;
            this.imgCarrera.TabStop = false;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.Window;
            this.txtPassword.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtPassword.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtPassword.BorderRadius = 5;
            this.txtPassword.BorderSize = 2;
            this.txtPassword.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Black;
            this.txtPassword.Location = new System.Drawing.Point(14, 302);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Multiline = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(90, 7, 80, 7);
            this.txtPassword.PasswordChar = false;
            this.txtPassword.Size = new System.Drawing.Size(271, 30);
            this.txtPassword.TabIndex = 28;
            this.txtPassword.Texts = "Contraseña";
            this.txtPassword.UnderlinedStyle = false;
            this.txtPassword._TextChanged += new System.EventHandler(this.txtPassword__TextChanged);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // cbCarrera
            // 
            this.cbCarrera.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cbCarrera.BackColor = System.Drawing.Color.DarkGreen;
            this.cbCarrera.BackColor1 = System.Drawing.Color.WhiteSmoke;
            this.cbCarrera.BorderColor = System.Drawing.Color.DarkGreen;
            this.cbCarrera.BorderSize = 2;
            this.cbCarrera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCarrera.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCarrera.ForeColor = System.Drawing.Color.Black;
            this.cbCarrera.IconColor = System.Drawing.Color.DarkGreen;
            this.cbCarrera.Items.AddRange(new object[] {
            "Ing. En Sistemas Computacionales",
            "Ing. En Animación y Efectos Visuales"});
            this.cbCarrera.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.cbCarrera.ListTextColor = System.Drawing.Color.DimGray;
            this.cbCarrera.Location = new System.Drawing.Point(14, 255);
            this.cbCarrera.MinimumSize = new System.Drawing.Size(200, 30);
            this.cbCarrera.Name = "cbCarrera";
            this.cbCarrera.Padding = new System.Windows.Forms.Padding(2);
            this.cbCarrera.Size = new System.Drawing.Size(270, 40);
            this.cbCarrera.TabIndex = 31;
            this.cbCarrera.Texts = "Carrera";
            // 
            // imgCicloEscolar
            // 
            this.imgCicloEscolar.BackColor = System.Drawing.Color.Transparent;
            this.imgCicloEscolar.Image = ((System.Drawing.Image)(resources.GetObject("imgCicloEscolar.Image")));
            this.imgCicloEscolar.Location = new System.Drawing.Point(2, 340);
            this.imgCicloEscolar.Name = "imgCicloEscolar";
            this.imgCicloEscolar.Size = new System.Drawing.Size(34, 31);
            this.imgCicloEscolar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCicloEscolar.TabIndex = 33;
            this.imgCicloEscolar.TabStop = false;
            // 
            // btnProfesores
            // 
            this.btnProfesores.BackColor = System.Drawing.Color.White;
            this.btnProfesores.BackgroundColor = System.Drawing.Color.White;
            this.btnProfesores.Bordercolor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(116)))), ((int)(((byte)(60)))));
            this.btnProfesores.BorderRadius = 20;
            this.btnProfesores.Bordersize = 2;
            this.btnProfesores.FlatAppearance.BorderSize = 0;
            this.btnProfesores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfesores.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfesores.ForeColor = System.Drawing.Color.Black;
            this.btnProfesores.Location = new System.Drawing.Point(116, 12);
            this.btnProfesores.Name = "btnProfesores";
            this.btnProfesores.Size = new System.Drawing.Size(136, 43);
            this.btnProfesores.TabIndex = 1;
            this.btnProfesores.Text = "PROFESORES";
            this.btnProfesores.TextColor = System.Drawing.Color.Black;
            this.btnProfesores.UseVisualStyleBackColor = false;
            // 
            // rjTcorreo
            // 
            this.rjTcorreo.BackColor = System.Drawing.SystemColors.Window;
            this.rjTcorreo.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.rjTcorreo.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.rjTcorreo.BorderRadius = 5;
            this.rjTcorreo.BorderSize = 2;
            this.rjTcorreo.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjTcorreo.ForeColor = System.Drawing.Color.Black;
            this.rjTcorreo.Location = new System.Drawing.Point(14, 341);
            this.rjTcorreo.Margin = new System.Windows.Forms.Padding(4);
            this.rjTcorreo.Multiline = true;
            this.rjTcorreo.Name = "rjTcorreo";
            this.rjTcorreo.Padding = new System.Windows.Forms.Padding(90, 7, 80, 7);
            this.rjTcorreo.PasswordChar = false;
            this.rjTcorreo.Size = new System.Drawing.Size(271, 30);
            this.rjTcorreo.TabIndex = 34;
            this.rjTcorreo.Texts = "Correo";
            this.rjTcorreo.UnderlinedStyle = false;
            // 
            // RegistroProfesores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.imgCicloEscolar);
            this.Controls.Add(this.rjTcorreo);
            this.Controls.Add(this.btnProfesores);
            this.Controls.Add(this.imgContraseña);
            this.Controls.Add(this.imgCarrera);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.cbCarrera);
            this.Controls.Add(this.imgMatricula);
            this.Controls.Add(this.imgApellidoP);
            this.Controls.Add(this.imgApellidoM);
            this.Controls.Add(this.txtApellidoM);
            this.Controls.Add(this.txtMatricula);
            this.Controls.Add(this.txtApellidoP);
            this.Controls.Add(this.imgNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnRegistro);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RegistroProfesores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro Profesores";
            ((System.ComponentModel.ISupportInitialize)(this.imgNombre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMatricula)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgApellidoP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgApellidoM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgContraseña)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCarrera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCicloEscolar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private RJControls.RJButton btnRegistro;
        private RJControls.RJButton btnCancelar;
        private System.Windows.Forms.PictureBox imgNombre;
        private RJControls.RJTextBox txtNombre;
        private System.Windows.Forms.PictureBox imgMatricula;
        private System.Windows.Forms.PictureBox imgApellidoP;
        private System.Windows.Forms.PictureBox imgApellidoM;
        private RJControls.RJTextBox txtApellidoM;
        private RJControls.RJTextBox txtMatricula;
        private RJControls.RJTextBox txtApellidoP;
        private System.Windows.Forms.PictureBox imgContraseña;
        private System.Windows.Forms.PictureBox imgCarrera;
        private RJControls.RJTextBox txtPassword;
        private RJControls.RJComboBox cbCarrera;
        private System.Windows.Forms.PictureBox imgCicloEscolar;
        private RJControls.Btnradius btnProfesores;
        private RJControls.RJTextBox rjTcorreo;
    }
}
