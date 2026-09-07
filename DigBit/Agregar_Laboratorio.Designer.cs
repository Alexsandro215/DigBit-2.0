namespace DigBit
{
    partial class Agregar_Laboratorio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Agregar_Laboratorio));
            this.nombreLaboratoriotxt = new DigBit.RJControls.RJTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnIngresarLaboratorio = new DigBit.RJControls.RJButton();
            this.btnCancelarLaboratorio = new DigBit.RJControls.RJButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // nombreLaboratoriotxt
            // 
            this.nombreLaboratoriotxt.BackColor = System.Drawing.SystemColors.Window;
            this.nombreLaboratoriotxt.BorderColor = System.Drawing.Color.DarkGreen;
            this.nombreLaboratoriotxt.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.nombreLaboratoriotxt.BorderRadius = 20;
            this.nombreLaboratoriotxt.BorderSize = 2;
            this.nombreLaboratoriotxt.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreLaboratoriotxt.ForeColor = System.Drawing.Color.Gray;
            this.nombreLaboratoriotxt.Location = new System.Drawing.Point(35, 106);
            this.nombreLaboratoriotxt.Margin = new System.Windows.Forms.Padding(4);
            this.nombreLaboratoriotxt.Multiline = true;
            this.nombreLaboratoriotxt.Name = "nombreLaboratoriotxt";
            this.nombreLaboratoriotxt.Padding = new System.Windows.Forms.Padding(7);
            this.nombreLaboratoriotxt.PasswordChar = false;
            this.nombreLaboratoriotxt.Size = new System.Drawing.Size(371, 36);
            this.nombreLaboratoriotxt.TabIndex = 0;
            this.nombreLaboratoriotxt.Texts = "            Ingresa el nombre del laboratorio";
            this.nombreLaboratoriotxt.UnderlinedStyle = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(53, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(333, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnIngresarLaboratorio
            // 
            this.btnIngresarLaboratorio.BackColor = System.Drawing.Color.White;
            this.btnIngresarLaboratorio.BackgroundColor = System.Drawing.Color.White;
            this.btnIngresarLaboratorio.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnIngresarLaboratorio.BorderRadius = 20;
            this.btnIngresarLaboratorio.BorderSize = 3;
            this.btnIngresarLaboratorio.FlatAppearance.BorderSize = 0;
            this.btnIngresarLaboratorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresarLaboratorio.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresarLaboratorio.ForeColor = System.Drawing.Color.Black;
            this.btnIngresarLaboratorio.Location = new System.Drawing.Point(53, 171);
            this.btnIngresarLaboratorio.Name = "btnIngresarLaboratorio";
            this.btnIngresarLaboratorio.Size = new System.Drawing.Size(122, 41);
            this.btnIngresarLaboratorio.TabIndex = 2;
            this.btnIngresarLaboratorio.Text = "INGRESAR";
            this.btnIngresarLaboratorio.TextColor = System.Drawing.Color.Black;
            this.btnIngresarLaboratorio.UseVisualStyleBackColor = false;
            // 
            // btnCancelarLaboratorio
            // 
            this.btnCancelarLaboratorio.BackColor = System.Drawing.Color.White;
            this.btnCancelarLaboratorio.BackgroundColor = System.Drawing.Color.White;
            this.btnCancelarLaboratorio.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnCancelarLaboratorio.BorderRadius = 20;
            this.btnCancelarLaboratorio.BorderSize = 3;
            this.btnCancelarLaboratorio.FlatAppearance.BorderSize = 0;
            this.btnCancelarLaboratorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarLaboratorio.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarLaboratorio.ForeColor = System.Drawing.Color.Black;
            this.btnCancelarLaboratorio.Location = new System.Drawing.Point(248, 171);
            this.btnCancelarLaboratorio.Name = "btnCancelarLaboratorio";
            this.btnCancelarLaboratorio.Size = new System.Drawing.Size(122, 41);
            this.btnCancelarLaboratorio.TabIndex = 3;
            this.btnCancelarLaboratorio.Text = "CANCELAR";
            this.btnCancelarLaboratorio.TextColor = System.Drawing.Color.Black;
            this.btnCancelarLaboratorio.UseVisualStyleBackColor = false;
            this.btnCancelarLaboratorio.Click += new System.EventHandler(this.btnCancelarLaboratorio_Click);
            // 
            // Agregar_Laboratorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(444, 236);
            this.Controls.Add(this.btnCancelarLaboratorio);
            this.Controls.Add(this.btnIngresarLaboratorio);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.nombreLaboratoriotxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Agregar_Laboratorio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar_Laboratorio";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RJControls.RJTextBox nombreLaboratoriotxt;
        private System.Windows.Forms.PictureBox pictureBox1;
        private RJControls.RJButton btnIngresarLaboratorio;
        private RJControls.RJButton btnCancelarLaboratorio;
    }
}