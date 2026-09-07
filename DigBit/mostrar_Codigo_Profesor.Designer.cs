namespace DigBit
{
    partial class mostrar_Codigo_Profesor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mostrar_Codigo_Profesor));
            this.txtCodigoGenerado = new DigBit.RJControls.RJTextBox();
            this.btnCopiar = new DigBit.RJControls.RJButton();
            this.btnCerrar = new DigBit.RJControls.RJButton();
            this.SuspendLayout();
            // 
            // txtCodigoGenerado
            // 
            this.txtCodigoGenerado.BackColor = System.Drawing.SystemColors.Window;
            this.txtCodigoGenerado.BorderColor = System.Drawing.Color.DarkGreen;
            this.txtCodigoGenerado.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtCodigoGenerado.BorderRadius = 20;
            this.txtCodigoGenerado.BorderSize = 2;
            this.txtCodigoGenerado.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoGenerado.ForeColor = System.Drawing.Color.DimGray;
            this.txtCodigoGenerado.Location = new System.Drawing.Point(37, 13);
            this.txtCodigoGenerado.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigoGenerado.Multiline = false;
            this.txtCodigoGenerado.Name = "txtCodigoGenerado";
            this.txtCodigoGenerado.Padding = new System.Windows.Forms.Padding(7);
            this.txtCodigoGenerado.PasswordChar = false;
            this.txtCodigoGenerado.Size = new System.Drawing.Size(228, 34);
            this.txtCodigoGenerado.TabIndex = 0;
            this.txtCodigoGenerado.Texts = "";
            this.txtCodigoGenerado.UnderlinedStyle = false;
            // 
            // btnCopiar
            // 
            this.btnCopiar.BackColor = System.Drawing.Color.White;
            this.btnCopiar.BackgroundColor = System.Drawing.Color.White;
            this.btnCopiar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnCopiar.BorderRadius = 20;
            this.btnCopiar.BorderSize = 2;
            this.btnCopiar.FlatAppearance.BorderSize = 0;
            this.btnCopiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopiar.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopiar.ForeColor = System.Drawing.Color.Black;
            this.btnCopiar.Location = new System.Drawing.Point(12, 65);
            this.btnCopiar.Name = "btnCopiar";
            this.btnCopiar.Size = new System.Drawing.Size(82, 23);
            this.btnCopiar.TabIndex = 3;
            this.btnCopiar.Text = "Copiar";
            this.btnCopiar.TextColor = System.Drawing.Color.Black;
            this.btnCopiar.UseVisualStyleBackColor = false;
            this.btnCopiar.Click += new System.EventHandler(this.btnCopiar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.White;
            this.btnCerrar.BackgroundColor = System.Drawing.Color.White;
            this.btnCerrar.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnCerrar.BorderRadius = 20;
            this.btnCerrar.BorderSize = 2;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.Black;
            this.btnCerrar.Location = new System.Drawing.Point(206, 65);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(82, 23);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.TextColor = System.Drawing.Color.Black;
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // mostrar_Codigo_Profesor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(300, 100);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnCopiar);
            this.Controls.Add(this.txtCodigoGenerado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mostrar_Codigo_Profesor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mostrar_Codigo_Profesor";
            this.ResumeLayout(false);

        }

        #endregion

        private RJControls.RJTextBox txtCodigoGenerado;
        private RJControls.RJButton btnCopiar;
        private RJControls.RJButton btnCerrar;
    }
}