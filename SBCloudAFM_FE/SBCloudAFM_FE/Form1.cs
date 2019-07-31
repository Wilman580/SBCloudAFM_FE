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
using Microsoft.Azure.ServiceBus;

namespace SBCloudAFM_FE
{
    public partial class Form1 : Form
    {
        List<string> listaProductosString;
        List<Productos> listaProductos;
        int codigoDet = 1;
        Factura factura;
        List<Detalle> listaDetalles;
        public Form1()
        {
            InitializeComponent();
            btnTerminar.Enabled = false;
            btnVerificar.Enabled = false;
            btnGuardar.Enabled = false;
            listaProductos = new List<Productos>();
            listaDetalles = new List<Detalle>();
            creadorProducto();
            factura = new Factura();
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
            txtImporte.Text = (costo).ToString();
            
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            float subtotal = 0;
            float ivatotal = 0;
            float total = 0;
            Detalle detalle = new Detalle();
            detalle.CodigoD = codigoDet;
            detalle.CantidadD = int.Parse(txtCantidad.Text);
            detalle.ProductoD = cbxDetalle.Text;
            detalle.PrecioD = Convert.ToSingle(txtPrecio.Text);
            detalle.IvaD = Convert.ToSingle(txtIVA.Text);
            detalle.ImporteD = Convert.ToSingle(txtImporte.Text);
            listaDetalles.Add(detalle);
            BindingSource bs = new BindingSource(listaDetalles,"");
            tblDatos.DataSource = bs;
            codigoDet++;
            foreach(Detalle d in listaDetalles)
            {
                subtotal += d.ImporteD;
                ivatotal += d.IvaD;
                total = subtotal + ivatotal;
            }
            txtSubtotal.Text = subtotal.ToString();
            txtIvaTotal.Text = ivatotal.ToString();
            txtTotal.Text = total.ToString();
            btnTerminar.Enabled = true;
        }

        private void btnTerminar_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(txtCliente.Text) && !String.IsNullOrEmpty(txtRuc.Text) && !String.IsNullOrEmpty(txtDireccion.Text) && !String.IsNullOrEmpty(txtTelefono.Text))
            {
                
                factura.CodigoFact = lblNumerof.Text;
                factura.Cliente = txtCliente.Text;
                factura.RucCI = int.Parse(txtRuc.Text);
                factura.FechaFac = DateTime.Parse(txtFecha.Text);
                factura.Direccion = txtDireccion.Text;
                factura.Telefono = txtTelefono.Text;
                factura.SubTotal = Convert.ToSingle(txtSubtotal.Text);
                factura.IvaTotal = Convert.ToSingle(txtIvaTotal.Text);
                factura.Total = Convert.ToSingle(txtTotal.Text);
                factura.ListaDet = listaDetalles;
                BindingSource bs = new BindingSource(listaDetalles, "");
                tblDatos.DataSource = bs;
                btnVerificar.Enabled = true;
                btnTerminar.Enabled = false;
                btnAgregar.Enabled = false;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Datos Incompletos\nRellene el Formulario","Error",MessageBoxButtons.OK);
                
            }
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            const string SBConectionString = "Endpoint=sb://sbcloud-afm.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iWoY+fomhRSsoLHZikXH3YqA1qEqy2u06iDwAXyO7eo=";
            //Conexion mediante Cola
            const string queueName = "afm-queues";
            IQueueClient queueClient=new QueueClient(SBConectionString, queueName);
            var objetoFE = Newtonsoft.Json.JsonConvert.SerializeObject(factura);
            //
            try
            {
                var paqueteFE = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(objetoFE));
                queueClient.SendAsync(paqueteFE);
                MessageBox.Show("Factura Enviada para su Comprobación");
            } catch (Exception ex)
            {
                MessageBox.Show("Error al enviar la Factura"+ex.Message);
            }


            //Cerrando conexiones
            queueClient.CloseAsync();
        }
    }
}
