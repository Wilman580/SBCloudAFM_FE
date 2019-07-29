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
            listaProductos = new List<Productos>();
            creadorProducto();

        }

        public void creadorProducto()
        {
            listaProductosString = new List<string>();
            listaProductosString.Add("GalletasRicas-0.50-0");
            listaProductosString.Add("GalletasSalticas-0.60-0");
            listaProductosString.Add("Lava-1.60-1");
            foreach (string producto in listaProductosString)
            {
                string[] vecpro = producto.Split('-');
                string nombre = (string)vecpro.GetValue(0);
                float precio = Convert.ToSingle(vecpro.GetValue(1));
                int iva = Convert.ToInt32(vecpro.GetValue(2));
                
                
                Productos p = new Productos();
                p.NombrePro = nombre;
                p.PrecioPro = precio;
                p.IvaPro = iva;
                listaProductos.Add(p);
            }
            foreach(Productos p in listaProductos)
            {
                cbxDetalle.Items.Add(p.NombrePro);
            }
            this.cbxDetalle.SelectedIndexChanged += new System.EventHandler(cargarPrecio);
        }

        private void cargarPrecio(object sender, System.EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedEmployee = (string)cbxDetalle.SelectedItem;
            int resultIndex = -1;
            resultIndex = cbxDetalle.FindStringExact(selectedEmployee);
            Productos pp = listaProductos[resultIndex];
            float precio = pp.PrecioPro;
            txtPrecio.Text = precio.ToString();
            int cant = int.Parse(txtCantidad.Text);
            float costo = cant * precio;
            float ivat = 0;
            if (pp.IvaPro == 0)
            {
                ivat = 0;
            }
            if (pp.IvaPro == 1)
            {
                ivat = (float)(costo * 0.12);
            }
            if (pp.IvaPro == 2)
            {
                ivat = (float)(costo * 0.14);
            }
            txtIVA.Text = ivat.ToString();
            txtImporte.Text = (costo+ivat).ToString();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

        }
    }
}
