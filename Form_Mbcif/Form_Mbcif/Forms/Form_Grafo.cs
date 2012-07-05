using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_Mbcif.Forms
{
    public partial class Form_Grafo : Form
    {
        public generagrafo gg;
        public Form_Grafo(generagrafo gg)
        {
            this.gg = gg;
            InitializeComponent();
        }

        private void Form_Grafo_Load(Object sender, EventArgs e)
        {
            UserControl1 sc = new UserControl1(this.gg);
            this.elementHost1.Child = sc;
            this.Controls.Add(elementHost1);
        }

        public void cerrarventana()
        {
            this.Visible = false;
        }
        public void refrescar()
        {

            this.Refresh();
        }
    }
}
