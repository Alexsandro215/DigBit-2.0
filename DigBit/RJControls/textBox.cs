using System;
using System.Windows.Forms;
using System.Drawing;

namespace CustomControls
{
    public class UnderlineTextBox : Panel
    {
        private TextBox _textBox;
        private Color _underlineColor = Color.Black;

        public TextBox TextBoxControl
        {
            get { return _textBox; }
        }

        public Color UnderlineColor
        {
            get { return _underlineColor; }
            set
            {
                if (_underlineColor != value)
                {
                    _underlineColor = value;
                    Invalidate();
                }
            }
        }

        public UnderlineTextBox()
        {
            // Crear el TextBox interno
            _textBox = new TextBox();
            _textBox.BorderStyle = BorderStyle.None;
            _textBox.Dock = DockStyle.Fill;
            _textBox.Parent = this;
            _textBox.TextChanged += TextBox_TextChanged;

            // Agregar el evento Paint para dibujar la línea inferior
            this.Paint += UnderlineTextBox_Paint;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Pasar el evento TextChanged del TextBox interno al control contenedor
            OnTextChanged(e);
        }

        private void UnderlineTextBox_Paint(object sender, PaintEventArgs e)
        {
            // Dibujar la línea inferior
            using (var pen = new Pen(UnderlineColor))
            {
                e.Graphics.DrawLine(pen, new Point(0, _textBox.Bottom), new Point(Width, _textBox.Bottom));
            }
        }
    }
}
