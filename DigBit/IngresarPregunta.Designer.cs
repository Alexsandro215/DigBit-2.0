namespace DigBit
{
    partial class IngresarPregunta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IngresarPregunta));
            this.lblPregunta = new System.Windows.Forms.Label();
            this.txtIngresarPregunta = new DigBit.RJControls.RJTextBox();
            this.btnIngresarPregunta = new DigBit.RJControls.RJButton();
            this.btnCancelar = new DigBit.RJControls.RJButton();
            this.SuspendLayout();
            // 
            // lblPregunta
            // 
            this.lblPregunta.AutoSize = true;
            this.lblPregunta.BackColor = System.Drawing.Color.Transparent;
            this.lblPregunta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPregunta.Location = new System.Drawing.Point(80, 62);
            this.lblPregunta.Name = "lblPregunta";
            this.lblPregunta.Size = new System.Drawing.Size(309, 19);
            this.lblPregunta.TabIndex = 0;
            this.lblPregunta.Text = "Ingresar nueva pregunta de seguridad";
            // 
            // txtIngresarPregunta
            // 
            this.txtIngresarPregunta.BackColor = System.Drawing.SystemColors.Window;
            this.txtIngresarPregunta.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtIngresarPregunta.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtIngresarPregunta.BorderRadius = 10;
            this.txtIngresarPregunta.BorderSize = 2;
            this.txtIngresarPregunta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIngresarPregunta.ForeColor = System.Drawing.Color.DimGray;
            this.txtIngresarPregunta.Location = new System.Drawing.Point(107, 103);
            this.txtIngresarPregunta.Margin = new System.Windows.Forms.Padding(4);
            this.txtIngresarPregunta.Multiline = false;
            this.txtIngresarPregunta.Name = "txtIngresarPregunta";
            this.txtIngresarPregunta.Padding = new System.Windows.Forms.Padding(7);
            this.txtIngresarPregunta.PasswordChar = false;
            this.txtIngresarPregunta.Size = new System.Drawing.Size(250, 34);
            this.txtIngresarPregunta.TabIndex = 1;
            this.txtIngresarPregunta.Texts = "Ingrese la pregunta";
            this.txtIngresarPregunta.UnderlinedStyle = false;
            // 
            // btnIngresarPregunta
            // 
            this.btnIngresarPregunta.BackColor = System.Drawing.Color.White;
            this.btnIngresarPregunta.BackgroundColor = System.Drawing.Color.White;
            this.btnIngresarPregunta.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnIngresarPregunta.BorderRadius = 20;
            this.btnIngresarPregunta.BorderSize = 2;
            this.btnIngresarPregunta.FlatAppearance.BorderSize = 0;
            this.btnIngresarPregunta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresarPregunta.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresarPregunta.ForeColor = System.Drawing.Color.Black;
            this.btnIngresarPregunta.Location = new System.Drawing.Point(84, 155);
            this.btnIngresarPregunta.Name = "btnIngresarPregunta";
            this.btnIngresarPregunta.Size = new System.Drawing.Size(119, 45);
            this.btnIngresarPregunta.TabIndex = 2;
            this.btnIngresarPregunta.Text = "Ingresar pregunta";
            this.btnIngresarPregunta.TextColor = System.Drawing.Color.Black;
            this.btnIngresarPregunta.UseVisualStyleBackColor = false;
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
            this.btnCancelar.Location = new System.Drawing.Point(238, 155);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(119, 45);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextColor = System.Drawing.Color.Black;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // IngresarPregunta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(444, 236);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnIngresarPregunta);
            this.Controls.Add(this.txtIngresarPregunta);
            this.Controls.Add(this.lblPregunta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IngresarPregunta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ingresar Pregunta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPregunta;
        private RJControls.RJTextBox txtIngresarPregunta;
        private RJControls.RJButton btnIngresarPregunta;
        private RJControls.RJButton btnCancelar;
    }
}