namespace DigBit
{
    partial class Bitacora_profesor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bitacora_profesor));
            this.tableLista = new System.Windows.Forms.TableLayoutPanel();
            this.lblMatricula = new System.Windows.Forms.Label();
            this.mySqlCommand1 = new MySqlConnector.MySqlCommand();
            this.btnBuscar = new DigBit.RJControls.RJButton();
            this.txtCodigo = new DigBit.RJControls.RJTextBox();
            this.txtBienvenido = new DigBit.RJControls.RJTextBox();
            this.txtTiempoRestante = new DigBit.RJControls.RJTextBox();
            this.btnCerrar = new DigBit.RJControls.RJButton();
            this.btnDescargarBitacora = new DigBit.RJControls.RJButton();
            this.SuspendLayout();
            // 
            // tableLista
            // 
            this.tableLista.AutoScroll = true;
            this.tableLista.BackColor = System.Drawing.Color.White;
            this.tableLista.ColumnCount = 4;
            this.tableLista.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLista.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLista.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLista.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLista.Location = new System.Drawing.Point(15, 82);
            this.tableLista.Name = "tableLista";
            this.tableLista.RowCount = 1;
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLista.Size = new System.Drawing.Size(693, 349);
            this.tableLista.TabIndex = 11;
            // 
            // lblMatricula
            // 
            this.lblMatricula.AutoSize = true;
            this.lblMatricula.Location = new System.Drawing.Point(672, 2);
            this.lblMatricula.Name = "lblMatricula";
            this.lblMatricula.Size = new System.Drawing.Size(35, 13);
            this.lblMatricula.TabIndex = 20;
            this.lblMatricula.Text = "label1";
            // 
            // mySqlCommand1
            // 
            this.mySqlCommand1.CommandTimeout = 0;
            this.mySqlCommand1.Connection = null;
            this.mySqlCommand1.Transaction = null;
            this.mySqlCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
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
            this.btnBuscar.Location = new System.Drawing.Point(212, 17);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(88, 38);
            this.btnBuscar.TabIndex = 22;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextColor = System.Drawing.Color.Black;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtCodigo
            // 
            this.txtCodigo.BackColor = System.Drawing.SystemColors.Window;
            this.txtCodigo.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtCodigo.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtCodigo.BorderRadius = 20;
            this.txtCodigo.BorderSize = 2;
            this.txtCodigo.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigo.ForeColor = System.Drawing.Color.Black;
            this.txtCodigo.Location = new System.Drawing.Point(13, 17);
            this.txtCodigo.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigo.Multiline = false;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Padding = new System.Windows.Forms.Padding(7, 12, 7, 7);
            this.txtCodigo.PasswordChar = false;
            this.txtCodigo.Size = new System.Drawing.Size(186, 36);
            this.txtCodigo.TabIndex = 21;
            this.txtCodigo.Texts = "Ingrese el código";
            this.txtCodigo.UnderlinedStyle = false;
            this.txtCodigo.Enter += new System.EventHandler(this.txtCodigo_Enter);
            this.txtCodigo.Leave += new System.EventHandler(this.txtCodigo_Leave);
            // 
            // txtBienvenido
            // 
            this.txtBienvenido.BackColor = System.Drawing.SystemColors.Window;
            this.txtBienvenido.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtBienvenido.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtBienvenido.BorderRadius = 15;
            this.txtBienvenido.BorderSize = 2;
            this.txtBienvenido.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBienvenido.ForeColor = System.Drawing.Color.Black;
            this.txtBienvenido.Location = new System.Drawing.Point(531, 19);
            this.txtBienvenido.Margin = new System.Windows.Forms.Padding(4);
            this.txtBienvenido.Multiline = true;
            this.txtBienvenido.Name = "txtBienvenido";
            this.txtBienvenido.Padding = new System.Windows.Forms.Padding(7, 12, 7, 7);
            this.txtBienvenido.PasswordChar = false;
            this.txtBienvenido.Size = new System.Drawing.Size(176, 36);
            this.txtBienvenido.TabIndex = 19;
            this.txtBienvenido.Texts = "Bienvenid@";
            this.txtBienvenido.UnderlinedStyle = false;
            // 
            // txtTiempoRestante
            // 
            this.txtTiempoRestante.BackColor = System.Drawing.SystemColors.Window;
            this.txtTiempoRestante.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtTiempoRestante.BorderFocusColor = System.Drawing.Color.DarkGreen;
            this.txtTiempoRestante.BorderRadius = 20;
            this.txtTiempoRestante.BorderSize = 2;
            this.txtTiempoRestante.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTiempoRestante.ForeColor = System.Drawing.Color.Black;
            this.txtTiempoRestante.Location = new System.Drawing.Point(328, 17);
            this.txtTiempoRestante.Margin = new System.Windows.Forms.Padding(4);
            this.txtTiempoRestante.Multiline = false;
            this.txtTiempoRestante.Name = "txtTiempoRestante";
            this.txtTiempoRestante.Padding = new System.Windows.Forms.Padding(7, 12, 7, 7);
            this.txtTiempoRestante.PasswordChar = false;
            this.txtTiempoRestante.Size = new System.Drawing.Size(186, 36);
            this.txtTiempoRestante.TabIndex = 18;
            this.txtTiempoRestante.Texts = "15:00 minutos restantes";
            this.txtTiempoRestante.UnderlinedStyle = false;
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
            this.btnCerrar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.Black;
            this.btnCerrar.Location = new System.Drawing.Point(381, 437);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(133, 52);
            this.btnCerrar.TabIndex = 17;
            this.btnCerrar.Text = "Cerrar bitacora";
            this.btnCerrar.TextColor = System.Drawing.Color.Black;
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnDescargarBitacora
            // 
            this.btnDescargarBitacora.BackColor = System.Drawing.Color.White;
            this.btnDescargarBitacora.BackgroundColor = System.Drawing.Color.White;
            this.btnDescargarBitacora.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnDescargarBitacora.BorderRadius = 20;
            this.btnDescargarBitacora.BorderSize = 2;
            this.btnDescargarBitacora.FlatAppearance.BorderSize = 0;
            this.btnDescargarBitacora.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescargarBitacora.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDescargarBitacora.ForeColor = System.Drawing.Color.Black;
            this.btnDescargarBitacora.Location = new System.Drawing.Point(202, 437);
            this.btnDescargarBitacora.Name = "btnDescargarBitacora";
            this.btnDescargarBitacora.Size = new System.Drawing.Size(136, 52);
            this.btnDescargarBitacora.TabIndex = 16;
            this.btnDescargarBitacora.Text = "Descargar bitacora";
            this.btnDescargarBitacora.TextColor = System.Drawing.Color.Black;
            this.btnDescargarBitacora.UseVisualStyleBackColor = false;
            this.btnDescargarBitacora.Click += new System.EventHandler(this.btnDescargarBitacora_Click);
            // 
            // Bitacora_profesor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(720, 496);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblMatricula);
            this.Controls.Add(this.txtBienvenido);
            this.Controls.Add(this.txtTiempoRestante);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnDescargarBitacora);
            this.Controls.Add(this.tableLista);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Bitacora_profesor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitacora Profesor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLista;
        private RJControls.RJButton btnDescargarBitacora;
        private RJControls.RJButton btnCerrar;
        private RJControls.RJTextBox txtTiempoRestante;
        private RJControls.RJTextBox txtBienvenido;
        private System.Windows.Forms.Label lblMatricula;
        private RJControls.RJTextBox txtCodigo;
        private RJControls.RJButton btnBuscar;
        private MySqlConnector.MySqlCommand mySqlCommand1;
    }
}
