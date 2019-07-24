using SBCloudAFM_FE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBCloudAFM_FE
{
    public partial class Form1 : Form
    {
        List<string> listaProductosString;
        List<Productos> listaProductos;

        public Form1()
        {
            InitializeComponent();
            creadorProducto();

        }

        public void creadorProducto()
        {
            listaProductosString = new List<string>();
            listaProductosString.Add("GalletasRicas-0,50");
            listaProductosString.Add("GalletasSalticas-0,60");
            foreach (string producto in listaProductosString)
            {
                string[] vecpro = producto.Split('-');
                string nombre = (string)vecpro.GetValue(0);
                float precio = (float)vecpro.GetValue(1);
                Productos p = new Productos();
                p.nombrePro = nombre;
                p.precioPro = precio;
                listaProductos.Add(p);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}
