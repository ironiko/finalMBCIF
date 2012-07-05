using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Form_Mbcif;

namespace Form_Mbcif.Forms
{
    public partial class Form_Elementos : Form
    {
        Sistema sistema;
        DataGridView tablaElementos, tablaRelaciones;
        generagrafo gg;

        public Form_Elementos(Sistema sistema, DataGridView tablaElementos, DataGridView tablaRelaciones, generagrafo gg)
        {
            InitializeComponent();
            this.gg = gg;
            this.sistema = sistema;
            this.tablaElementos = tablaElementos;
            this.tablaRelaciones = tablaRelaciones;

           
        }
        //Al cargar la ventana.
        private void Form_Elementos_Load(object sender, EventArgs e)
        {
            set_combobox_niveles();
            set_List_elementos();
            comboBox1.SelectedItem = null;
            button1.Enabled = false;
        }

        public void llenarTablas()
        {

            tablaElementos.Rows.Clear();
            //llenar tabla elementos
            for (int i = 0; i < sistema.niveles.Count; i++)
            {
                tablaElementos.Rows.Add("Nivel " + i + " :" + sistema.niveles[i].nombre, " ",
                           " ");
                for (int j = 0; j < sistema.niveles[i].listaElementos.Count; j++)
                {
                    tablaElementos.Rows.Add(sistema.niveles[i].nombre, sistema.niveles[i].listaElementos[j].nombre,
                        sistema.niveles[i].listaElementos[j].valor);
                }

            }

            tablaRelaciones.Rows.Clear();
            //llenar tabla relaciones
            for (int a = 0; a < sistema.niveles.Count; a++)
            {
                tablaRelaciones.Rows.Add("Nivel " + a + " :" + sistema.niveles[a].nombre, " ",
                           " ");
                for (int b = 0; b < sistema.niveles[a].matriz.Count; b++)
                {
                    for (int c = 0; c < sistema.niveles[a].matriz[b].Count; c++)
                        if (sistema.niveles[a].matriz[b][c] != null)
                            if (sistema.niveles[a].matriz[b][c].Regla.operador != null)
                                tablaRelaciones.Rows.Add(sistema.niveles[a].nombre,
                                sistema.niveles[a].listaElementos[b].nombre, sistema.niveles[a].listaElementos[c].nombre,
                                sistema.niveles[a].matriz[b][c].funcion, sistema.niveles[a].matriz[b][c].Regla.iteracion,
                                sistema.niveles[a].matriz[b][c].Regla.operador + sistema.niveles[a].matriz[b][c].Regla.valor);
                            else
                                tablaRelaciones.Rows.Add(sistema.niveles[a].nombre,
                                sistema.niveles[a].listaElementos[b].nombre, sistema.niveles[a].listaElementos[c].nombre,
                                sistema.niveles[a].matriz[b][c].funcion, sistema.niveles[a].matriz[b][c].Regla.iteracion,
                                "-");
                }
            }

        }

        //A presionar aceptar guarda y se sale.
        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Close();
          
        }
        //Al presionar cancelar se cierra.
        private void button2_Click(object sender, EventArgs e)
        {   
            //guardar los datos de la tabla

            foreach (Nivel n in sistema.niveles)
            {
                if (n.nombre.Equals(comboBox1.SelectedItem.ToString()))
                {
                    Elemento elemento = new Elemento(textBox1.Text, double.Parse(textBox2.Text));
                    n.agregarElemento(elemento);
                    gg.CreateElemento(elemento, n);
                }
            }
            limpia_text(sender, e);
            llenarTablas();
        }

        

        private void limpia_text(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedItem = null;
            Form_Elementos_Load(sender, e);
        }
        private void set_combobox_niveles()
        {
            comboBox1.Items.Clear();
            foreach (Nivel x in sistema.niveles)
            {
                comboBox1.Items.Add(x.nombre);
            }
        }

        private void set_List_elementos()
        {
            listBox1.Items.Clear();
            foreach (Nivel x in sistema.niveles)
            {
                foreach (Elemento y in x.listaElementos)
                {
                    listBox1.Items.Add(y.nombre);
                }
                
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
            string elemento_value = listBox1.SelectedItem.ToString();
           foreach (Nivel x in sistema.niveles)
           {
               foreach (Elemento l in x.listaElementos)
               {
                   if(l.nombre.Equals(elemento_value))
                   {
                       textBox1.Text = elemento_value;
                       textBox2.Text = l.valor.ToString();
                   }
               }
           }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            if (comboBox1.SelectedItem != null)
            {
                string nombre = comboBox1.SelectedItem.ToString();
                foreach (Nivel n in sistema.niveles)
                {
                    if (n.nombre.Equals(nombre))
                    {
                        foreach (Elemento l in n.listaElementos)
                        {
                            listBox2.Items.Add(l.nombre);
                        }
                    }

                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string value = listBox2.SelectedItem.ToString();
            int indice = 0;
            foreach (Nivel n in sistema.niveles)
            {

                if (n.nombre.Equals(comboBox1.SelectedItem.ToString()))
                {

                    int i = 0;
                    foreach (Elemento l in n.listaElementos)
                    {

                        if (l.nombre.Equals(value))
                        {
                            indice = i;

                        }
                        i++;
                    }

                    n.eliminarElemento(indice);
                    gg.removeelemento(value);
                }

            }
            limpia_text(sender, e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

      
    }
}
