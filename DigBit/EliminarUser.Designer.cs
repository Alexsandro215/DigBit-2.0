namespace DigBit
{
    partial class EliminarUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EliminarUser));
            this.txtIngresaMatricula = new System.Windows.Forms.Label();
            this.btnEliminarUsuario = new DigBit.RJControls.RJButton();
            this.btnCancelar = new DigBit.RJControls.RJButton();
            this.txtApellidoPaterno = new DigBit.RJControls.RJTextBox();
            this.txtApellidoMaterno = new DigBit.RJControls.RJTextBox();
            this.txtNombre = new DigBit.RJControls.RJTextBox();
            this.btnBuscar = new DigBit.RJControls.RJButton();
            this.txtMatricula = new DigBit.RJControls.RJTextBox();
            this.SuspendLayout();
            // 
            // txtIngresaMatricula
            // 
            this.txtIngresaMatricula.AutoSize = true;
            this.txtIngresaMatricula.BackColor = System.Drawing.Color.Transparent;
            this.txtIngresaMatricula.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIngresaMatricula.Location = new System.Drawing.Point(18, 54);
            this.txtIngresaMatricula.Name = "txtIngresaMatricula";
            this.txtIngresaMatricula.Size = new System.Drawing.Size(200, 16);
            this.txtIngresaMatricula.TabIndex = 6;
            this.txtIngresaMatricula.Text = "Ingresa matricula del usuario";
            // 
            // btnEliminarUsuario
            // 
            this.btnEliminarUsuario.BackColor = System.Drawing.Color.White;
            this.btnEliminarUsuario.BackgroundColor = System.Drawing.Color.White;
            this.btnEliminarUsuario.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnEliminarUsuario.BorderRadius = 20;
            this.btnEliminarUsuario.BorderSize = 2;
            this.btnEliminarUsuario.FlatAppearance.BorderSize = 0;
            this.btnEliminarUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarUsuario.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminarUsuario.ForeColor = System.Drawing.Color.Black;
            this.btnEliminarUsuario.Location = new System.Drawing.Point(343, 190);
            this.btnEliminarUsuario.Name = "btnEliminarUsuario";
            this.btnEliminarUsuario.Size = new System.Drawing.Size(91, 32);
            this.btnEliminarUsuario.TabIndex = 13;
            this.btnEliminarUsuario.Text = "Eliminar";
            this.btnEliminarUsuario.TextColor = System.Drawing.Color.Black;
            this.btnEliminarUsuario.UseVisualStyleBackColor = false;
            this.btnEliminarUsuario.Click += new System.EventHandler(this.btnEliminarUsuario_Click);
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
            this.btnCancelar.Location = new System.Drawing.Point(245, 190);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(91, 32);
            this.btnCancelar.TabIndex = 12;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextColor = System.Drawing.Color.Black;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtApellidoPaterno
            // 
            this.txtApellidoPaterno.BackColor = System.Drawing.SystemColors.Window;
            this.txtApellidoPaterno.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtApellidoPaterno.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtApellidoPaterno.BorderRadius = 20;
            this.txtApellidoPaterno.BorderSize = 2;
            this.txtApellidoPaterno.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellidoPaterno.ForeColor = System.Drawing.Color.Black;
            this.txtApellidoPaterno.Location = new System.Drawing.Point(259, 89);
            this.txtApellidoPaterno.Margin = new System.Windows.Forms.Padding(4);
            this.txtApellidoPaterno.Multiline = false;
            this.txtApellidoPaterno.Name = "txtApellidoPaterno";
            this.txtApellidoPaterno.Padding = new System.Windows.Forms.Padding(20, 7, 7, 7);
            this.txtApellidoPaterno.PasswordChar = false;
            this.txtApellidoPaterno.Size = new System.Drawing.Size(174, 34);
            this.txtApellidoPaterno.TabIndex = 11;
            this.txtApellidoPaterno.Texts = "Apellido Paterno";
            this.txtApellidoPaterno.UnderlinedStyle = false;
            this.txtApellidoPaterno._TextChanged += new System.EventHandler(this.txtApellidoPaterno__TextChanged);
            // 
            // txtApellidoMaterno
            // 
            this.txtApellidoMaterno.BackColor = System.Drawing.SystemColors.Window;
            this.txtApellidoMaterno.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtApellidoMaterno.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtApellidoMaterno.BorderRadius = 20;
            this.txtApellidoMaterno.BorderSize = 2;
            this.txtApellidoMaterno.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellidoMaterno.ForeColor = System.Drawing.Color.Black;
            this.txtApellidoMaterno.Location = new System.Drawing.Point(259, 131);
            this.txtApellidoMaterno.Margin = new System.Windows.Forms.Padding(4);
            this.txtApellidoMaterno.Multiline = false;
            this.txtApellidoMaterno.Name = "txtApellidoMaterno";
            this.txtApellidoMaterno.Padding = new System.Windows.Forms.Padding(20, 7, 7, 7);
            this.txtApellidoMaterno.PasswordChar = false;
            this.txtApellidoMaterno.Size = new System.Drawing.Size(174, 34);
            this.txtApellidoMaterno.TabIndex = 10;
            this.txtApellidoMaterno.Texts = "Apellido Materno";
            this.txtApellidoMaterno.UnderlinedStyle = false;
            // 
            // txtNombre
            // 
            this.txtNombre.BackColor = System.Drawing.SystemColors.Window;
            this.txtNombre.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtNombre.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtNombre.BorderRadius = 20;
            this.txtNombre.BorderSize = 2;
            this.txtNombre.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.ForeColor = System.Drawing.Color.Black;
            this.txtNombre.Location = new System.Drawing.Point(259, 47);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4);
            this.txtNombre.Multiline = false;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Padding = new System.Windows.Forms.Padding(50, 7, 7, 7);
            this.txtNombre.PasswordChar = false;
            this.txtNombre.Size = new System.Drawing.Size(174, 34);
            this.txtNombre.TabIndex = 9;
            this.txtNombre.Texts = "Nombre";
            this.txtNombre.UnderlinedStyle = false;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.White;
            this.btnBuscar.BackgroundColor = System.Drawing.Color.White;
            this.btnBuscar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnBuscar.BorderRadius = 20;
            this.btnBuscar.BorderSize = 2;
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.ForeColor = System.Drawing.Color.Black;
            this.btnBuscar.Location = new System.Drawing.Point(70, 132);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(91, 32);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextColor = System.Drawing.Color.Black;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtMatricula
            // 
            this.txtMatricula.BackColor = System.Drawing.SystemColors.Window;
            this.txtMatricula.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtMatricula.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtMatricula.BorderRadius = 10;
            this.txtMatricula.BorderSize = 2;
            this.txtMatricula.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatricula.ForeColor = System.Drawing.Color.DimGray;
            this.txtMatricula.Location = new System.Drawing.Point(31, 87);
            this.txtMatricula.Margin = new System.Windows.Forms.Padding(4);
            this.txtMatricula.Multiline = false;
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Padding = new System.Windows.Forms.Padding(50, 7, 7, 7);
            this.txtMatricula.PasswordChar = false;
            this.txtMatricula.Size = new System.Drawing.Size(174, 34);
            this.txtMatricula.TabIndex = 7;
            this.txtMatricula.Texts = "Matricula";
            this.txtMatricula.UnderlinedStyle = false;
            // 
            // EliminarUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(444, 236);
            this.Controls.Add(this.btnEliminarUsuario);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.txtApellidoPaterno);
            this.Controls.Add(this.txtApellidoMaterno);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtMatricula);
            this.Controls.Add(this.txtIngresaMatricula);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EliminarUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eliminar Usuario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RJControls.RJTextBox txtApellidoPaterno;
        private RJControls.RJTextBox txtApellidoMaterno;
        private RJControls.RJTextBox txtNombre;
        private RJControls.RJButton btnBuscar;
        private RJControls.RJTextBox txtMatricula;
        private System.Windows.Forms.Label txtIngresaMatricula;
        private RJControls.RJButton btnCancelar;
        private RJControls.RJButton btnEliminarUsuario;
    }
}