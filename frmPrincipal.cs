using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using iTextSharp.text.pdf;
using PdfDocument = iTextSharp.text.Document;

namespace Suministro
{
    public partial class frmPrincipal : Form
    {
        private string TablaH;
        private string TablaD;
        private string TablaCH;
        private string TablaCD;
        private string TipoMov;
        DataTable dtFact;
        DataTable dtDetFact;
        clss_Static Variable = new clss_Static();
        private int contador;
        private int t_contador;
        private int nRenglon;
        private int nColumna;
        private bool estatusProceso;
        private bool t_flagEstadoFacturas;
        //private bool t_flagEstadoFacturasPrelim;
        private string FechaIni;
        private string FechaFin;
        private clss_Funciones Func = new clss_Funciones();
        // Impresiones
        private long Pagina;
        private int aYPos, R;
        private int margen = 186;
        private int ConteoParcial = 0;
        private int numReg = 57;
        private string KeysPressedFirst = "";
        private int rowF = 0;
        private int i = 0;
        private Boolean Encontrado = false;


        private Panel pnlInformacionHistorica;
        private DataGridView dgvHistorialFacturas;
        private Label lblResumenCajas;
        private Label lblResumenBolsas;
        private Label lblResumenPlastico;
        private Label lblResumenPaquetes;
        private Label lblFacturaActual;
        private int splitterDistanceNormal = 157;
        private int splitterDistanceHistorial = 250;

        private FlowLayoutPanel pnlFacturasDinamicas;
        private Button btnCantidadFacturas;
        private List<TextBox> txtFacturasDinamicas = new List<TextBox>();
        private int cantidadFacturasTrabajo = 3;

        private Panel pnlBuscadorDetalle;
        private TextBox txtBuscarDetalle;
        private ComboBox cmbCampoBusqueda;
        private Button btnBuscarDetalle;
        private Button btnLimpiarBusqueda;
        private Label lblBuscarDetalle;


        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TablaH = "";
            TablaD = "";
            TablaCH = "";
            TablaCD = "";
            TipoMov = "";
            this.tsl_estatus.Text = "";
            this.txt_temp.Text = "";
            t_contador = 0;
            nRenglon = 0;
            estatusProceso = false;
            t_flagEstadoFacturas = false;
            this.tmr_tiempo.Enabled = false;

            FechaIni = DateTime.Now.ToString()
                .Replace(" p.m.", "")
                .Replace(" a.m.", "")
                .Replace(" p. m.", "")
                .Replace(" a. m.", "");

            FechaFin = DateTime.Now.ToString()
                .Replace(" p.m.", "")
                .Replace(" a.m.", "")
                .Replace(" p. m.", "")
                .Replace(" a. m.", "");

            this.txt_fechaini.Text = FechaIni;

            dtFact = new DataTable();
            dtDetFact = new DataTable();

            this.tsl_estatus.BackColor = Color.LightSteelBlue;

            LimpiaPantalla();

            txtCodeBar.Visible = false;

            this.btn_pdfDetalle.Image = CrearIconoPdf();
            this.btn_pdfResumen.Image = CrearIconoPdf();

            this.Top = 1;
            this.Left = 1;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;

            splitContainer1.SplitterDistance = splitterDistanceNormal;

            InicializarInformacionHistorica();

            InicializarCapturaFacturas();

            InicializarBuscadorDetalle();
        }

        private void InicializarCapturaFacturas()
        {
            Control contenedor = this.txt_fact1.Parent;

            int xInicial = this.txt_fact1.Left;
            int yInicial = this.txt_fact1.Top;
            int alto = this.txt_fact1.Height;

            this.txt_fact1.Visible = false;
            this.txt_fact2.Visible = false;
            this.txt_fact3.Visible = false;

            btnCantidadFacturas = new Button();

            btnCantidadFacturas.Name = "btnCantidadFacturas";
            btnCantidadFacturas.Text = "3 Facturas";
            btnCantidadFacturas.Font = new Font(
                "Segoe UI",
                8.5F,
                FontStyle.Bold);

            btnCantidadFacturas.Size =
                new Size(95, alto + 4);

            btnCantidadFacturas.Location =
                new Point(
                    xInicial,
                    yInicial - 2);

            btnCantidadFacturas.FlatStyle =
                FlatStyle.Flat;

            btnCantidadFacturas.FlatAppearance.BorderSize = 1;

            btnCantidadFacturas.BackColor =
                Color.FromArgb(31, 78, 121);

            btnCantidadFacturas.ForeColor =
                Color.White;

            btnCantidadFacturas.Cursor =
                Cursors.Hand;

            btnCantidadFacturas.Click +=
                btnCantidadFacturas_Click;

            pnlFacturasDinamicas =
                new FlowLayoutPanel();

            pnlFacturasDinamicas.Name =
                "pnlFacturasDinamicas";
            int inicioFacturas =
    xInicial;

            int margenDerecho =
                15;

            int segundaFilaY =
                btnCantidadFacturas.Bottom + 7;

            pnlFacturasDinamicas.Location =
                new Point(
                    inicioFacturas,
                    segundaFilaY);

            pnlFacturasDinamicas.Size =
    new Size(
        Math.Max(
            150,
            contenedor.ClientSize.Width -
            inicioFacturas -
            margenDerecho),
        58);

            pnlFacturasDinamicas.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            pnlFacturasDinamicas.FlowDirection =
                FlowDirection.LeftToRight;

            pnlFacturasDinamicas.WrapContents =
                false;

            pnlFacturasDinamicas.AutoScroll =
                true;

            pnlFacturasDinamicas.HorizontalScroll.Enabled =
                true;

            pnlFacturasDinamicas.HorizontalScroll.Visible =
                true;

            pnlFacturasDinamicas.VerticalScroll.Enabled =
                false;

            pnlFacturasDinamicas.VerticalScroll.Visible =
                false;

            pnlFacturasDinamicas.Padding =
                new Padding(
                    4,
                    2,
                    4,
                    16);

            pnlFacturasDinamicas.BackColor =
                Color.Transparent;

            contenedor.Controls.Add(
    btnCantidadFacturas);

            contenedor.Controls.Add(
                pnlFacturasDinamicas);

            btnCantidadFacturas.BringToFront();
            pnlFacturasDinamicas.BringToFront();

            this.btn_busq.Location =
                new Point(
                    btnCantidadFacturas.Right + 10,
                    btnCantidadFacturas.Top);

            this.btn_busq.Size =
                new Size(
                    82,
                    btnCantidadFacturas.Height);

            this.btn_busq.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left;

            this.btn_busq.BringToFront();

            CrearCamposFacturas(
                cantidadFacturasTrabajo);

            int margenEntreFacturasYDatos = 12;

            int nuevaPosicionDatos =
                pnlFacturasDinamicas.Bottom +
                margenEntreFacturasYDatos;

            this.gpb_fact.Top =
                nuevaPosicionDatos;

            int alturaNecesariaPanelSuperior =
                this.gpb_fact.Bottom + 8;

            splitterDistanceNormal =
                alturaNecesariaPanelSuperior;

            if (splitContainer1.Height >
                alturaNecesariaPanelSuperior + 100)
            {
                splitContainer1.SplitterDistance =
                    alturaNecesariaPanelSuperior;
            }

            this.gpb_fact.BringToFront();
        }

        private void InicializarBuscadorDetalle()
        {
            pnlBuscadorDetalle =
                new Panel();

            pnlBuscadorDetalle.Name =
                "pnlBuscadorDetalle";

            pnlBuscadorDetalle.Height =
                42;

            pnlBuscadorDetalle.Dock =
                DockStyle.Top;

            pnlBuscadorDetalle.BackColor =
                Color.FromArgb(
                    235,
                    241,
                    247);

            pnlBuscadorDetalle.Padding =
                new Padding(
                    8,
                    6,
                    8,
                    6);

            lblBuscarDetalle =
                new Label();

            lblBuscarDetalle.Text =
                "Buscar:";

            lblBuscarDetalle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            lblBuscarDetalle.ForeColor =
                Color.FromArgb(
                    45,
                    55,
                    65);

            lblBuscarDetalle.AutoSize =
                true;

            lblBuscarDetalle.Location =
                new Point(
                    10,
                    12);

            txtBuscarDetalle =
                new TextBox();

            txtBuscarDetalle.Name =
                "txtBuscarDetalle";

            txtBuscarDetalle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Regular);

            txtBuscarDetalle.BorderStyle =
                BorderStyle.FixedSingle;

            txtBuscarDetalle.BackColor =
                Color.White;

            txtBuscarDetalle.Location =
                new Point(
                    70,
                    8);

            txtBuscarDetalle.Size =
                new Size(
                    280,
                    27);

            txtBuscarDetalle.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left;

            txtBuscarDetalle.KeyDown +=
                txtBuscarDetalle_KeyDown;

            cmbCampoBusqueda =
                new ComboBox();

            cmbCampoBusqueda.Name =
                "cmbCampoBusqueda";

            cmbCampoBusqueda.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCampoBusqueda.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Regular);

            cmbCampoBusqueda.Location =
                new Point(
                    360,
                    8);

            cmbCampoBusqueda.Size =
                new Size(
                    150,
                    27);

            cmbCampoBusqueda.Items.Add(
                "Todos");

            cmbCampoBusqueda.Items.Add(
                "Factura");

            cmbCampoBusqueda.Items.Add(
                "Documento");

            cmbCampoBusqueda.Items.Add(
                "Código Barras");

            cmbCampoBusqueda.Items.Add(
                "Código Artículo");

            cmbCampoBusqueda.Items.Add(
                "Descripción");

            cmbCampoBusqueda.SelectedIndex =
                0;

            btnBuscarDetalle =
                new Button();

            btnBuscarDetalle.Name =
                "btnBuscarDetalle";

            btnBuscarDetalle.Text =
                "Buscar";

            btnBuscarDetalle.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold);

            btnBuscarDetalle.Size =
                new Size(
                    80,
                    27);

            btnBuscarDetalle.Location =
                new Point(
                    520,
                    7);

            btnBuscarDetalle.FlatStyle =
                FlatStyle.Flat;

            btnBuscarDetalle.FlatAppearance.BorderSize =
                0;

            btnBuscarDetalle.BackColor =
                Color.FromArgb(
                    31,
                    78,
                    121);

            btnBuscarDetalle.ForeColor =
                Color.White;

            btnBuscarDetalle.Cursor =
                Cursors.Hand;

            btnBuscarDetalle.Click +=
                btnBuscarDetalle_Click;

            btnLimpiarBusqueda =
                new Button();

            btnLimpiarBusqueda.Name =
                "btnLimpiarBusqueda";

            btnLimpiarBusqueda.Text =
                "Limpiar";

            btnLimpiarBusqueda.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Regular);

            btnLimpiarBusqueda.Size =
                new Size(
                    75,
                    27);

            btnLimpiarBusqueda.Location =
                new Point(
                    608,
                    7);

            btnLimpiarBusqueda.FlatStyle =
                FlatStyle.Flat;

            btnLimpiarBusqueda.FlatAppearance.BorderColor =
                Color.FromArgb(
                    180,
                    190,
                    200);

            btnLimpiarBusqueda.BackColor =
                Color.White;

            btnLimpiarBusqueda.ForeColor =
                Color.FromArgb(
                    45,
                    45,
                    45);

            btnLimpiarBusqueda.Cursor =
                Cursors.Hand;

            btnLimpiarBusqueda.Click +=
                btnLimpiarBusqueda_Click;

            pnlBuscadorDetalle.Controls.Add(
                lblBuscarDetalle);

            pnlBuscadorDetalle.Controls.Add(
                txtBuscarDetalle);

            pnlBuscadorDetalle.Controls.Add(
                cmbCampoBusqueda);

            pnlBuscadorDetalle.Controls.Add(
                btnBuscarDetalle);

            pnlBuscadorDetalle.Controls.Add(
                btnLimpiarBusqueda);

            splitContainer1.Panel2.Controls.Add(
                pnlBuscadorDetalle);

            pnlBuscadorDetalle.BringToFront();

            int diferencia =
                pnlBuscadorDetalle.Height;

            dgv_fact.Top =
                diferencia + 3;

            dgv_fact.Height =
                Math.Max(
                    100,
                    dgv_fact.Height -
                    diferencia);

            dgv_fact.BringToFront();
            pnlBuscadorDetalle.BringToFront();
        }

        private void btnBuscarDetalle_Click(
            object sender,
            EventArgs e)
                {
                    BuscarEnDetalle();
        }

        private void txtBuscarDetalle_KeyDown(
        object sender,
        KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;

                    BuscarEnDetalle();
                }
        }


        private void BuscarEnDetalle()
        {
            if (dgv_fact == null ||
                dgv_fact.Rows.Count == 0)
            {
                MessageBox.Show(
                    "No hay información para buscar.",
                    "Buscar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string texto =
                txtBuscarDetalle.Text
                    .Trim();

            if (texto == "")
            {
                MessageBox.Show(
                    "Capture un dato para buscar.",
                    "Buscar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtBuscarDetalle.Focus();

                return;
            }

            string textoBusqueda =
                texto.ToUpper();

            string campoSeleccionado =
                cmbCampoBusqueda.SelectedItem == null
                    ? "Todos"
                    : cmbCampoBusqueda
                        .SelectedItem
                        .ToString();

            int filaInicial =
                0;

            if (dgv_fact.CurrentRow != null)
            {
                filaInicial =
                    dgv_fact.CurrentRow.Index + 1;

                if (filaInicial >=
                    dgv_fact.Rows.Count)
                {
                    filaInicial = 0;
                }
            }

            int filaEncontrada =
                BuscarCoincidenciaDetalle(
                    filaInicial,
                    dgv_fact.Rows.Count,
                    textoBusqueda,
                    campoSeleccionado);

            if (filaEncontrada < 0 &&
                filaInicial > 0)
            {
                filaEncontrada =
                    BuscarCoincidenciaDetalle(
                        0,
                        filaInicial,
                        textoBusqueda,
                        campoSeleccionado);
            }

            if (filaEncontrada < 0)
            {
                System.Media.SystemSounds.Beep.Play();

                MessageBox.Show(
                    "No se encontró ninguna coincidencia para:\n\n" +
                    texto,
                    "Buscar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtBuscarDetalle.SelectAll();
                txtBuscarDetalle.Focus();

                return;
            }

            dgv_fact.ClearSelection();

            DataGridViewRow fila =
                dgv_fact.Rows[
                    filaEncontrada];

            fila.Selected =
                true;

            DataGridViewCell celdaDestino =
                ObtenerCeldaBusqueda(
                    fila,
                    campoSeleccionado);

            if (celdaDestino == null)
            {
                celdaDestino =
                    fila.Cells["Factura"];
            }

            dgv_fact.CurrentCell =
                celdaDestino;

            try
            {
                dgv_fact.FirstDisplayedScrollingRowIndex =
                    filaEncontrada;
            }
            catch
            {
            }

            tsl_estatus.BackColor =
                Color.FromArgb(
                    0,
                    120,
                    90);

            tsl_estatus.ForeColor =
                Color.White;

            tsl_estatus.Text =
                "Registro encontrado: " +
                Convert.ToString(
                    fila.Cells["Factura"].Value) +
                " | " +
                Convert.ToString(
                    fila.Cells["Descripcion"].Value);
        }

        private int BuscarCoincidenciaDetalle(
    int inicio,
    int fin,
    string textoBusqueda,
    string campoSeleccionado)
        {
            for (int i = inicio;
                 i < fin;
                 i++)
            {
                DataGridViewRow fila =
                    dgv_fact.Rows[i];

                if (fila.IsNewRow)
                {
                    continue;
                }

                bool encontrado =
                    false;

                if (campoSeleccionado ==
                    "Factura")
                {
                    encontrado =
                        CeldaContiene(
                            fila,
                            "Factura",
                            textoBusqueda);
                }
                else if (campoSeleccionado ==
                         "Documento")
                {
                    encontrado =
                        CeldaContiene(
                            fila,
                            "Documento",
                            textoBusqueda);
                }
                else if (campoSeleccionado ==
                         "Código Barras")
                {
                    encontrado =
                        CeldaContiene(
                            fila,
                            "CodigoBar",
                            textoBusqueda);
                }
                else if (campoSeleccionado ==
                         "Código Artículo")
                {
                    encontrado =
                        CeldaContiene(
                            fila,
                            "CodigoArt",
                            textoBusqueda);
                }
                else if (campoSeleccionado ==
                         "Descripción")
                {
                    encontrado =
                        CeldaContiene(
                            fila,
                            "Descripcion",
                            textoBusqueda);
                }
                else
                {
                    encontrado =
                        CeldaContiene(
                            fila,
                            "Factura",
                            textoBusqueda) ||

                        CeldaContiene(
                            fila,
                            "Documento",
                            textoBusqueda) ||

                        CeldaContiene(
                            fila,
                            "CodigoBar",
                            textoBusqueda) ||

                        CeldaContiene(
                            fila,
                            "CodigoArt",
                            textoBusqueda) ||

                        CeldaContiene(
                            fila,
                            "Descripcion",
                            textoBusqueda);
                }

                if (encontrado)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool CeldaContiene(
    DataGridViewRow fila,
    string nombreColumna,
    string textoBusqueda)
        {
            if (fila == null)
            {
                return false;
            }

            if (!dgv_fact.Columns.Contains(
                nombreColumna))
            {
                return false;
            }

            object valor =
                fila.Cells[
                    nombreColumna].Value;

            if (valor == null ||
                valor == DBNull.Value)
            {
                return false;
            }

            string contenido =
                valor
                    .ToString()
                    .Trim()
                    .ToUpper();

            return contenido.Contains(
                textoBusqueda);
        }

        private DataGridViewCell ObtenerCeldaBusqueda(
    DataGridViewRow fila,
    string campoSeleccionado)
        {
            if (fila == null)
            {
                return null;
            }

            string nombreColumna =
                "Factura";

            if (campoSeleccionado ==
                "Documento")
            {
                nombreColumna =
                    "Documento";
            }
            else if (campoSeleccionado ==
                     "Código Barras")
            {
                nombreColumna =
                    "CodigoBar";
            }
            else if (campoSeleccionado ==
                     "Código Artículo")
            {
                nombreColumna =
                    "CodigoArt";
            }
            else if (campoSeleccionado ==
                     "Descripción")
            {
                nombreColumna =
                    "Descripcion";
            }

            if (!dgv_fact.Columns.Contains(
                nombreColumna))
            {
                return null;
            }

            return fila.Cells[
                nombreColumna];
        }

        private void btnLimpiarBusqueda_Click(
        object sender,
        EventArgs e)
            {
                txtBuscarDetalle.Text =
                    "";

                cmbCampoBusqueda.SelectedIndex =
                    0;

                dgv_fact.ClearSelection();

                tsl_estatus.Text =
                    "";

                tsl_estatus.BackColor =
                    Color.LightSteelBlue;

                txtBuscarDetalle.Focus();
        }


        private void btnCantidadFacturas_Click(
            object sender,
            EventArgs e)
        {
            using (Form ventana = new Form())
            {
                ventana.Text =
                    "Cantidad de facturas";

                ventana.StartPosition =
                    FormStartPosition.CenterParent;

                ventana.FormBorderStyle =
                    FormBorderStyle.FixedDialog;

                ventana.MaximizeBox = false;
                ventana.MinimizeBox = false;

                ventana.ClientSize =
                    new Size(360, 180);

                ventana.BackColor =
                    Color.White;

                Label titulo =
                    new Label();

                titulo.Text =
                    "¿Cuántas facturas va a trabajar?";

                titulo.Font =
                    new Font(
                        "Segoe UI",
                        11F,
                        FontStyle.Bold);

                titulo.AutoSize = false;

                titulo.TextAlign =
                    ContentAlignment.MiddleCenter;

                titulo.Location =
                    new Point(
                        20,
                        20);

                titulo.Size =
                    new Size(
                        320,
                        30);

                NumericUpDown cantidad =
                    new NumericUpDown();

                cantidad.Minimum = 1;
                cantidad.Maximum = 15;

                cantidad.Value =
                    cantidadFacturasTrabajo;

                cantidad.Font =
                    new Font(
                        "Segoe UI",
                        14F,
                        FontStyle.Bold);

                cantidad.TextAlign =
                    HorizontalAlignment.Center;

                cantidad.Location =
                    new Point(
                        125,
                        65);

                cantidad.Size =
                    new Size(
                        110,
                        32);

                Button aceptar =
                    new Button();

                aceptar.Text =
                    "Aceptar";

                aceptar.Font =
                    new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold);

                aceptar.BackColor =
                    Color.FromArgb(
                        31,
                        78,
                        121);

                aceptar.ForeColor =
                    Color.White;

                aceptar.FlatStyle =
                    FlatStyle.Flat;

                aceptar.Location =
                    new Point(
                        125,
                        115);

                aceptar.Size =
                    new Size(
                        110,
                        35);

                aceptar.DialogResult =
                    DialogResult.OK;

                ventana.Controls.Add(
                    titulo);

                ventana.Controls.Add(
                    cantidad);

                ventana.Controls.Add(
                    aceptar);

                ventana.AcceptButton =
                    aceptar;

                if (ventana.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    cantidadFacturasTrabajo =
                        Convert.ToInt32(
                            cantidad.Value);

                    btnCantidadFacturas.Text =
                        cantidadFacturasTrabajo == 1
                            ? "1 Factura"
                            : cantidadFacturasTrabajo +
                              " Facturas";

                    CrearCamposFacturas(
                        cantidadFacturasTrabajo);
                }
            }
        }

        private void CrearCamposFacturas(
    int cantidad)
        {
            List<string> valoresActuales =
                new List<string>();

            foreach (TextBox txt in
                txtFacturasDinamicas)
            {
                if (txt.Text.Trim() != "")
                {
                    valoresActuales.Add(
                        txt.Text.Trim());
                }
            }

            pnlFacturasDinamicas.Controls.Clear();
            txtFacturasDinamicas.Clear();

            for (int i = 0; i < cantidad; i++)
            {
                TextBox txt =
                    new TextBox();

                txt.Name =
                    "txt_factura_dinamica_" +
                    (i + 1);

                txt.Width = 105;

                txt.AutoSize = false;
                txt.Height = 30;

                txt.Font =
                    new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Regular);

                txt.TextAlign =
                    HorizontalAlignment.Center;

                txt.BorderStyle =
                    BorderStyle.FixedSingle;

                txt.BackColor =
                    Color.White;

                txt.ForeColor =
                    Color.FromArgb(
                        35,
                        45,
                        55);

                txt.MaxLength = 30;

                txt.Margin =
                    new Padding(
                        5,
                        3,
                        5,
                        3);

                if (i < valoresActuales.Count)
                {
                    txt.Text =
                        valoresActuales[i];
                }

                txt.Tag = i;

                txt.KeyPress +=
                    txtFacturaDinamica_KeyPress;

                txt.TextChanged +=
                    txtFacturaDinamica_TextChanged;

                txt.Enter +=
                    delegate
                    {
                        txt.BackColor =
                            Color.FromArgb(
                                245,
                                250,
                                255);
                    };

                txt.Leave +=
                    delegate
                    {
                        txt.BackColor =
                            Color.White;
                    };

                txtFacturasDinamicas.Add(
                    txt);

                pnlFacturasDinamicas.Controls.Add(
                    txt);
            }

            if (txtFacturasDinamicas.Count > 0)
            {
                txtFacturasDinamicas[0].Focus();
            }
        }

        private void txtFacturaDinamica_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) ||
                e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (e.KeyChar ==
                     (char)Keys.Enter)
            {
                e.Handled = true;

                TextBox actual =
                    sender as TextBox;

                int indice =
                    txtFacturasDinamicas.IndexOf(
                        actual);

                if (indice >= 0 &&
                    indice <
                    txtFacturasDinamicas.Count - 1)
                {
                    txtFacturasDinamicas[
                        indice + 1].Focus();
                }
                else
                {
                    btn_busq_Click(
                        sender,
                        EventArgs.Empty);
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtFacturaDinamica_TextChanged(
            object sender,
            EventArgs e)
        {
            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";

            if (dtFact != null)
            {
                dtFact.Clear();
            }

            if (dtDetFact != null)
            {
                dtDetFact.Clear();
            }
        }

        private List<string> ObtenerFacturasCapturadas()
        {
            List<string> facturas =
                new List<string>();

            if (txtFacturasDinamicas == null)
            {
                return facturas;
            }

            foreach (TextBox txt in
                txtFacturasDinamicas)
            {
                string factura =
                    txt.Text.Trim();

                if (factura != "" &&
                    factura.Length > 2 &&
                    !facturas.Contains(factura))
                {
                    facturas.Add(
                        factura);
                }
            }

            return facturas;
        }

        private string ObtenerFacturasSql()
        {
            List<string> facturas =
                ObtenerFacturasCapturadas();

            List<string> resultado =
                new List<string>();

            foreach (string factura in facturas)
            {
                resultado.Add(
                    "'" +
                    factura.Replace(
                        "'",
                        "''") +
                    "'");
            }

            return string.Join(
                ",",
                resultado.ToArray());
        }

        private string ObtenerFacturaActual()
        {
            List<string> facturas =
                ObtenerFacturasCapturadas();

            if (facturas.Count == 0)
            {
                return "";
            }

            return facturas[
                facturas.Count - 1];
        }

        private void btn_busq_Click(
    object sender,
    EventArgs e)
        {
            List<string> facturas =
                ObtenerFacturasCapturadas();

            string t_facturas =
                ObtenerFacturasSql();

            int t_contFact =
                facturas.Count;

            if (TablaH == "")
            {
                MessageBox.Show(
                    "¡Seleccione el tipo de movimiento de mercancía!",
                    "Movimiento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }

            if (t_contFact == 0)
            {
                MessageBox.Show(
                    "Capture al menos una factura.",
                    "Facturas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }

            if (t_contFact !=
                cantidadFacturasTrabajo)
            {
                MessageBox.Show(
                    "Indicó que trabajará " +
                    cantidadFacturasTrabajo +
                    " factura(s), pero solamente capturó " +
                    t_contFact +
                    ".",
                    "Facturas incompletas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }

            Cursor.Current =
                Cursors.WaitCursor;

            try
            {
                if (ValidaFacturaConfirmada(
                    t_facturas,
                    t_contFact))
                {
                    estatusProceso = false;

                    this.btn_imprimir.Visible =
                        true;

                    ConsultaFacturaConfirmada(
                        t_facturas,
                        t_contFact);

                    MostrarInformacionHistorica(
                        t_facturas);
                }
                else
                {
                    if (t_flagEstadoFacturas)
                    {
                        estatusProceso = true;

                        this.btn_imprimir.Visible =
                            false;

                        this.btn_pdfDetalle.Enabled =
                            false;

                        this.btn_pdfResumen.Enabled =
                            false;

                        DespliegaFactura(
                            t_facturas,
                            t_contFact);

                        GrabaPreliminar();

                        MostrarInformacionHistorica(
                            t_facturas);
                    }
                    else
                    {
                        OcultarInformacionHistorica();

                        MessageBox.Show(
                            "¡No todas las facturas ingresadas están confirmadas!",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                }
            }
            finally
            {
                Cursor.Current =
                    Cursors.Default;
            }
        }


        private void MostrarInformacionHistorica(
    string facturasSql)
        {
            if (string.IsNullOrWhiteSpace(facturasSql))
            {
                OcultarInformacionHistorica();
                return;
            }

            string facturaActual =
            ObtenerFacturaActual();

            clss_Query consultaHistorial =
                new clss_Query();

            consultaHistorial.AsignaBase(
                Properties.Settings.Default.BaseRS);

            consultaHistorial.AsignaSQL(
                "SELECT " +
                "NumFac AS [Factura], " +
                "MIN(FechaIni) AS [Inicio], " +
                "MAX(FechaFin) AS [Fin] " +
                "FROM " +
                Properties.Settings.Default.CONFIRMACIONES + " " +
                "WHERE NumFac IN (" + facturasSql + ") " +
                "AND Tipo = '" + TablaCH + "' " +
                "GROUP BY NumFac " +
                "ORDER BY NumFac");

            consultaHistorial.Execute_DT();

            DataTable historial =
                consultaHistorial.ObtieneTabla();

            dgvHistorialFacturas.DataSource =
                historial;

            if (lblFacturaActual != null)
            {
                lblFacturaActual.Text =
                    facturaActual == ""
                        ? "EN PROCESO"
                        : "EN PROCESO: " + facturaActual;
            }

            if (dgvHistorialFacturas.Columns.Contains(
                "Factura"))
            {
                dgvHistorialFacturas
                    .Columns["Factura"]
                    .FillWeight = 60;
            }

            if (dgvHistorialFacturas.Columns.Contains(
                "Inicio"))
            {
                dgvHistorialFacturas
                    .Columns["Inicio"]
                    .FillWeight = 120;

                dgvHistorialFacturas
                    .Columns["Inicio"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm:ss";
            }

            if (dgvHistorialFacturas.Columns.Contains(
                "Fin"))
            {
                dgvHistorialFacturas
                    .Columns["Fin"]
                    .FillWeight = 120;

                dgvHistorialFacturas
                    .Columns["Fin"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm:ss";
            }

            foreach (DataGridViewRow fila in dgvHistorialFacturas.Rows)
            {
                string facturaFila =
                    fila.Cells["Factura"].Value == null
                        ? ""
                        : fila.Cells["Factura"]
                            .Value
                            .ToString()
                            .Trim();

                if (facturaFila.Equals(
                    facturaActual,
                    StringComparison.OrdinalIgnoreCase))
                {
                    fila.DefaultCellStyle.BackColor =
                        Color.FromArgb(
                            220,
                            245,
                            232);

                    fila.DefaultCellStyle.ForeColor =
                        Color.FromArgb(
                            20,
                            85,
                            55);

                    fila.DefaultCellStyle.Font =
                        new Font(
                            "Segoe UI",
                            8F,
                            FontStyle.Bold);

                    fila.DefaultCellStyle.SelectionBackColor =
                        Color.FromArgb(
                            190,
                            230,
                            208);

                    fila.Selected =
                        true;
                }
            }

            MostrarResumenEmpaques(
                facturasSql);

            bool tieneInformacion =
                historial != null &&
                historial.Rows.Count > 0;

            pnlInformacionHistorica.Visible =
                tieneInformacion;

            if (tieneInformacion)
            {
                int alturaNecesaria =
                    gpb_fact.Bottom +
                    pnlInformacionHistorica.Height +
                    6;

                if (alturaNecesaria <
                    splitterDistanceNormal +
                    pnlInformacionHistorica.Height)
                {
                    alturaNecesaria =
                        splitterDistanceNormal +
                        pnlInformacionHistorica.Height;
                }

                if (alturaNecesaria <
                    splitContainer1.Height - 100)
                {
                    splitContainer1.SplitterDistance =
                        alturaNecesaria;
                }

                pnlInformacionHistorica.BringToFront();
            }
        }


        private void MostrarResumenEmpaques(
    string facturasSql)
        {
            int cantidadCajas = 0;
            int cantidadBolsas = 0;
            int cantidadPlastico = 0;
            int cantidadPaquetes = 0;

            HashSet<string> cajasContadas =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            HashSet<string> bolsasContadas =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            HashSet<string> plasticosContados =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            HashSet<string> paquetesContados =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            if (dgv_fact != null)
            {
                foreach (DataGridViewRow fila in
                    dgv_fact.Rows)
                {
                    if (fila.IsNewRow)
                    {
                        continue;
                    }

                    string factura =
                        fila.Cells["Factura"].Value == null
                            ? ""
                            : fila.Cells["Factura"]
                                .Value
                                .ToString()
                                .Trim()
                                .ToUpper();

                    string noCaja =
                        fila.Cells["Caja"].Value == null
                            ? ""
                            : fila.Cells["Caja"]
                                .Value
                                .ToString()
                                .Trim()
                                .ToUpper();

                    if (noCaja == "" ||
                        noCaja == "0")
                    {
                        continue;
                    }

                    noCaja =
                        noCaja
                            .Replace(";", ",")
                            .Replace("/", ",")
                            .Replace("\\", ",");

                    string[] empaquesIndividuales =
                        noCaja.Split(
                            new char[] { ',' },
                            StringSplitOptions.RemoveEmptyEntries);

                    foreach (string empaqueBase in
                        empaquesIndividuales)
                    {
                        string empaque =
                            empaqueBase
                                .Trim()
                                .Replace(" ", "")
                                .ToUpper();

                        if (empaque == "" ||
                            empaque == "0")
                        {
                            continue;
                        }

                        string clave = empaque;

                        if (empaque.StartsWith("CP"))
                        {
                            if (!plasticosContados.Contains(
                                clave))
                            {
                                plasticosContados.Add(
                                    clave);

                                cantidadPlastico++;
                            }

                            continue;
                        }

                        if (empaque.StartsWith("C"))
                        {
                            if (!cajasContadas.Contains(
                                clave))
                            {
                                cajasContadas.Add(
                                    clave);

                                cantidadCajas++;
                            }

                            continue;
                        }

                        if (empaque.StartsWith("B"))
                        {
                            if (!bolsasContadas.Contains(
                                clave))
                            {
                                bolsasContadas.Add(
                                    clave);

                                cantidadBolsas++;
                            }

                            continue;
                        }

                        if (empaque.StartsWith("P"))
                        {
                            if (!paquetesContados.Contains(
                                clave))
                            {
                                paquetesContados.Add(
                                    clave);

                                cantidadPaquetes++;
                            }

                            continue;
                        }
                    }
                }
            }

            lblResumenCajas.Text =
                cantidadCajas.ToString();

            lblResumenBolsas.Text =
                cantidadBolsas.ToString();

            lblResumenPlastico.Text =
                cantidadPlastico.ToString();

            lblResumenPaquetes.Text =
                cantidadPaquetes.ToString();
        }

        private void OcultarInformacionHistorica()
        {
            if (dgvHistorialFacturas != null)
            {
                dgvHistorialFacturas.DataSource =
                    null;
            }

            if (lblResumenCajas != null)
            {
                lblResumenCajas.Text =
                    "0";
            }

            if (lblResumenBolsas != null)
            {
                lblResumenBolsas.Text =
                    "0";
            }

            if (lblResumenPlastico != null)
            {
                lblResumenPlastico.Text =
                    "0";
            }

            if (lblResumenPaquetes != null)
            {
                lblResumenPaquetes.Text =
                    "0";
            }

            if (pnlInformacionHistorica != null)
            {
                pnlInformacionHistorica.Visible =
                    false;
            }

            if (splitContainer1 != null &&
                splitContainer1.Height >
                splitterDistanceNormal + 100)
            {
                splitContainer1.SplitterDistance =
                    splitterDistanceNormal;
            }
        }

        private void rbn_sal_CheckedChanged(object sender, EventArgs e)
        {
            TablaH = Properties.Settings.Default.OINV;
            TablaD = Properties.Settings.Default.INV1;
            TablaCH = Properties.Settings.Default.SUMINISTRO_CAB;
            TablaCD = Properties.Settings.Default.SUMINISTRO_DET;
            TipoMov = Properties.Settings.Default.SUMINISTRO;
            this.dgv_fact.Columns["Caja"].Visible = true;
            this.dgv_fact.Columns["Factura"].Visible = true;
            this.dgv_fact.Columns["Documento"].Visible = true;
            this.btn_confirmar.Text = "Confirmar Suministro";
            LimpiaPantalla();
        }

        private void LimpiaPantalla()
        {
            this.txt_fact1.Text = "";
            this.txt_fact2.Text = "";
            this.txt_fact3.Text = "";

            if (txtFacturasDinamicas != null)
            {
                foreach (TextBox txt in
                    txtFacturasDinamicas)
                {
                    txt.Text = "";
                }
            }
            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";

            if (txtFacturasDinamicas != null &&
                txtFacturasDinamicas.Count > 0)
            {
                txtFacturasDinamicas[0].Focus();
            }
            else
            {
                this.txt_fact1.Focus();
            }

            this.gpb_fact.Visible = true;

            this.btn_confirmar.Enabled = false;
            this.btn_imprimir.Visible = false;

            this.rbn_sal.Checked = true;

            this.txt_prov.Enabled = false;
            this.txt_sub.Enabled = false;
            this.txt_imp.Enabled = false;
            this.txt_tot.Enabled = false;

            OcultarInformacionHistorica();
        }

        private void InicializarInformacionHistorica()
        {
            pnlInformacionHistorica = new Panel();

            pnlInformacionHistorica.Name = "pnlInformacionHistorica";
            pnlInformacionHistorica.Dock = DockStyle.Bottom;
            pnlInformacionHistorica.Height = 110;
            pnlInformacionHistorica.BackColor = Color.FromArgb(242, 246, 250);
            pnlInformacionHistorica.Padding = new Padding(6, 4, 6, 4);
            pnlInformacionHistorica.Visible = false;

            TableLayoutPanel contenedorPrincipal =
                new TableLayoutPanel();

            contenedorPrincipal.Dock = DockStyle.Fill;
            contenedorPrincipal.ColumnCount = 2;
            contenedorPrincipal.RowCount = 1;
            contenedorPrincipal.BackColor = Color.Transparent;

            contenedorPrincipal.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    64F));

            contenedorPrincipal.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    36F));

            Panel pnlHistorial =
                new Panel();

            pnlHistorial.Dock = DockStyle.Fill;
            pnlHistorial.BackColor = Color.White;
            pnlHistorial.Margin = new Padding(0, 0, 5, 0);

            Panel pnlTituloHistorial =
                new Panel();

            pnlTituloHistorial.Dock = DockStyle.Top;
            pnlTituloHistorial.Height = 24;
            pnlTituloHistorial.BackColor =
                Color.FromArgb(31, 78, 121);

            Label lblTituloHistorial =
                new Label();

            lblTituloHistorial.Dock = DockStyle.Fill;
            lblTituloHistorial.ForeColor = Color.White;

            lblTituloHistorial.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold);

            lblTituloHistorial.Text =
                "  HISTORIAL DE FACTURAS";

            lblTituloHistorial.TextAlign =
                ContentAlignment.MiddleLeft;

            lblFacturaActual =
                new Label();

            lblFacturaActual.Dock =
                DockStyle.Right;

            lblFacturaActual.Width =
                235;

            lblFacturaActual.BackColor =
                Color.FromArgb(0, 120, 90);

            lblFacturaActual.ForeColor =
                Color.White;

            lblFacturaActual.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold);

            lblFacturaActual.Text =
                "EN PROCESO:";

            lblFacturaActual.TextAlign =
                ContentAlignment.MiddleCenter;

            pnlTituloHistorial.Controls.Add(
                lblTituloHistorial);

            pnlTituloHistorial.Controls.Add(
                lblFacturaActual);

            dgvHistorialFacturas =
                new DataGridView();

            dgvHistorialFacturas.Name =
                "dgvHistorialFacturas";

            dgvHistorialFacturas.Dock =
                DockStyle.Fill;

            dgvHistorialFacturas.BackgroundColor =
                Color.White;

            dgvHistorialFacturas.BorderStyle =
                BorderStyle.None;

            dgvHistorialFacturas.AllowUserToAddRows =
                false;

            dgvHistorialFacturas.AllowUserToDeleteRows =
                false;

            dgvHistorialFacturas.AllowUserToResizeRows =
                false;

            dgvHistorialFacturas.ReadOnly =
                true;

            dgvHistorialFacturas.MultiSelect =
                false;

            dgvHistorialFacturas.RowHeadersVisible =
                false;

            dgvHistorialFacturas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvHistorialFacturas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistorialFacturas.EnableHeadersVisualStyles =
                false;

            dgvHistorialFacturas.ColumnHeadersHeight =
                22;

            dgvHistorialFacturas.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(45, 95, 145);

            dgvHistorialFacturas.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvHistorialFacturas.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold);

            dgvHistorialFacturas.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvHistorialFacturas.DefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Regular);

            dgvHistorialFacturas.DefaultCellStyle.BackColor =
                Color.White;

            dgvHistorialFacturas.DefaultCellStyle.ForeColor =
                Color.FromArgb(45, 45, 45);

            dgvHistorialFacturas.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(221, 235, 247);

            dgvHistorialFacturas.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(30, 30, 30);

            dgvHistorialFacturas.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvHistorialFacturas.GridColor =
                Color.FromArgb(220, 225, 230);

            dgvHistorialFacturas.RowTemplate.Height =
                20;

            pnlHistorial.Controls.Add(
                dgvHistorialFacturas);

            pnlHistorial.Controls.Add(
                pnlTituloHistorial);

            Panel pnlResumen =
                new Panel();

            pnlResumen.Dock = DockStyle.Fill;
            pnlResumen.BackColor = Color.White;
            pnlResumen.Margin = new Padding(5, 0, 0, 0);

            Label lblTituloResumen =
                new Label();

            lblTituloResumen.Dock =
                DockStyle.Top;

            lblTituloResumen.Height =
                24;

            lblTituloResumen.BackColor =
                Color.FromArgb(31, 78, 121);

            lblTituloResumen.ForeColor =
                Color.White;

            lblTituloResumen.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold);

            lblTituloResumen.Text =
                "  RESUMEN DE EMPAQUE";

            lblTituloResumen.TextAlign =
                ContentAlignment.MiddleLeft;

            TableLayoutPanel tarjetas =
                new TableLayoutPanel();

            tarjetas.Dock = DockStyle.Fill;
            tarjetas.ColumnCount = 4;
            tarjetas.RowCount = 1;
            tarjetas.Padding = new Padding(3);
            tarjetas.BackColor = Color.White;

            tarjetas.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    25F));

            tarjetas.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    25F));

            tarjetas.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    25F));

            tarjetas.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    25F));

            Panel tarjetaCajas =
                CrearTarjetaResumen(
                    "CAJAS",
                    out lblResumenCajas);

            Panel tarjetaBolsas =
                CrearTarjetaResumen(
                    "BOLSAS",
                    out lblResumenBolsas);

            Panel tarjetaPlastico =
                CrearTarjetaResumen(
                    "C. PLÁSTICO",
                    out lblResumenPlastico);

            Panel tarjetaPaquetes =
                CrearTarjetaResumen(
                    "PAQUETES",
                    out lblResumenPaquetes);

            tarjetas.Controls.Add(
                tarjetaCajas,
                0,
                0);

            tarjetas.Controls.Add(
                tarjetaBolsas,
                1,
                0);

            tarjetas.Controls.Add(
                tarjetaPlastico,
                2,
                0);

            tarjetas.Controls.Add(
                tarjetaPaquetes,
                3,
                0);

            pnlResumen.Controls.Add(
                tarjetas);

            pnlResumen.Controls.Add(
                lblTituloResumen);

            contenedorPrincipal.Controls.Add(
                pnlHistorial,
                0,
                0);

            contenedorPrincipal.Controls.Add(
                pnlResumen,
                1,
                0);

            pnlInformacionHistorica.Controls.Add(
                contenedorPrincipal);

            splitContainer1.Panel1.Controls.Add(
                pnlInformacionHistorica);

            pnlInformacionHistorica.BringToFront();
        }

        private Panel CrearTarjetaResumen(
    string titulo,
    out Label etiquetaValor)
        {
            Panel tarjeta =
                new Panel();

            tarjeta.Dock =
                DockStyle.Fill;

            tarjeta.BackColor =
                Color.FromArgb(248, 250, 252);

            tarjeta.Margin =
                new Padding(2);

            Label etiquetaTitulo =
                new Label();

            etiquetaTitulo.Dock =
                DockStyle.Top;

            etiquetaTitulo.Height =
                19;

            etiquetaTitulo.ForeColor =
                Color.FromArgb(80, 90, 100);

            etiquetaTitulo.Font =
                new Font(
                    "Segoe UI",
                    7F,
                    FontStyle.Bold);

            etiquetaTitulo.Text =
                titulo;

            etiquetaTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            etiquetaValor =
                new Label();

            etiquetaValor.Dock =
                DockStyle.Fill;

            etiquetaValor.ForeColor =
                Color.FromArgb(31, 78, 121);

            etiquetaValor.Font =
                new Font(
                    "Segoe UI",
                    16F,
                    FontStyle.Bold);

            etiquetaValor.Text =
                "0";

            etiquetaValor.TextAlign =
                ContentAlignment.MiddleCenter;

            tarjeta.Controls.Add(
                etiquetaValor);

            tarjeta.Controls.Add(
                etiquetaTitulo);

            return tarjeta;
        }

        private void splitContainer1_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            this.tsl_estatus.Text = "";
            this.tsl_estatus.BackColor = Color.LightSteelBlue;
        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            this.tsl_estatus.Text = "";
            this.tsl_estatus.BackColor = Color.LightSteelBlue;
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.tmr_tiempo.Enabled = false;
            this.Close();
            Application.Exit();
        }

        private void dgv_fact_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtCodeBar.Visible = true;
            KeysPressedFirst = e.KeyChar.ToString();
            txtCodeBar.Focus();

            //try
            //{
            //    if (estatusProceso)
            //    {
            //string KeysPressed = Variable.ObtieneTeclaPulsada();
            //DateTime LastKeyPress = Variable.ObtieneFechaTeclaPulsada();
            //string caracterFinal = Variable.ObtieneTeclaFinal();
            //        if (char.IsNumber(e.KeyChar))
            //        {
            //            if (DateTimeExtension.DateDiff(DateInterval.Milisecond, LastKeyPress, DateTime.Now) >= 250)
            //            {
            //                Variable.ColocaTeclaPulsada("");
            //                KeysPressed = e.KeyChar.ToString();
            //            }
            //            else
            //            {
            //                KeysPressed += e.KeyChar.ToString();
            //            }
            //            LastKeyPress = DateTime.Now;
            //            caracterFinal = e.KeyChar.ToString();
            //            Variable.ColocaTeclaPulsada(KeysPressed);
            //            Variable.ColocaFechaTeclaPulsada(LastKeyPress);
            //            Variable.ColocaTeclaFinal(caracterFinal);
            //        }
            //        else if ((Keys)e.KeyChar == Keys.Enter)
            //        {
            //            caracterFinal = "#";
            //            Variable.ColocaTeclaFinal(caracterFinal);
            //        }

            //        if ((Keys)e.KeyChar == Keys.Enter)
            //        {
            //            foreach (DataGridViewRow row in this.dgv_fact.Rows)
            //            {
            //                string cadena;
            //                string KeysPressedU;
            //                KeysPressedU = "";
            //                cadena = dgv_fact["CodigoPaq", row.Index].Value.ToString().ToUpper();
            //                cadena = cadena.Replace("'", "");
            //                KeysPressedU = KeysPressed.ToUpper();
            //                if (cadena == KeysPressedU && caracterFinal == "#")
            //                {
            //                    this.dgv_fact.Rows[row.Index].Selected = true;
            //                    this.dgv_fact.CurrentCell = this.dgv_fact.Rows[row.Index].Cells["CodigoPaq"];
            //                    this.tsl_estatus.BackColor = Color.Green;
            //                    this.tsl_estatus.Text = "Artículo encontrado.";
            //                    //MessageBox.Show("Articulo encontrado", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //                    SumaCantidad(row.Index);
            //                    MarcaRenglon();
            //                    GrabaLineaPreliminar(this.dgv_fact.Rows[row.Index].HeaderCell.Value.ToString(), this.dgv_fact.Rows[row.Index].Cells["Factura"].Value.ToString(), this.dgv_fact.Rows[row.Index].Cells["CodigoPaq"].Value.ToString(), this.dgv_fact.Rows[row.Index].Cells["CantidadR"].Value.ToString(), "C");
            //                    break;
            //                }
            //                else
            //                {
            //                    dgv_fact.Rows[row.Index].Selected = false;
            //                    if (row.Index == this.dgv_fact.Rows.Count - 1)
            //                    {
            //                        //this.tsl_estatus.BackColor = Color.Red;
            //                        //this.tsl_estatus.Text = "Error: Artículo no encontrado.";
            //                        MessageBox.Show("Articulo no encontrado", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            //                    }
            //                }
            //                this.dgv_fact.Focus();
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //}
        }

        private void SumaCantidad(int rowB)
        {
            double CantidadR;
            double CantidadF;

            CantidadF = Convert.ToDouble(this.dgv_fact["CantidadF", rowB].Value);

            if (this.dgv_fact["CantidadR", rowB].Value == null || this.dgv_fact["CantidadR", rowB].Value.ToString() == "")
            {
                CantidadR = 0;
            }
            else
            {
                CantidadR = Convert.ToDouble(this.dgv_fact["CantidadR", rowB].Value);
            }

            if (CantidadR < CantidadF)
            {
                this.dgv_fact["CantidadR", rowB].Value = CantidadR + 1;
            }
            else
            {
                MessageBox.Show("Cantidad máxima de artículos.", "Limite alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgv_fact_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0 && estatusProceso)
                {
                    frmCantidad fc = new frmCantidad();
                    fc.cantidadF = Convert.ToDouble(this.dgv_fact["CantidadF", e.RowIndex].Value);
                    fc.ShowDialog();
                    if (fc.estado)
                    {
                        this.dgv_fact["CantidadR", e.RowIndex].Value = fc.cantidadR;
                        MarcaRenglon();
                        GrabaLineaPreliminar(this.dgv_fact.Rows[e.RowIndex].HeaderCell.Value.ToString(), this.dgv_fact.Rows[e.RowIndex].Cells["Factura"].Value.ToString(), this.dgv_fact.Rows[e.RowIndex].Cells["CodigoPaq"].Value.ToString(), this.dgv_fact.Rows[e.RowIndex].Cells["CantidadR"].Value.ToString(), "C");
                    }
                }
            }
        }

        private void btn_confirmar_Click(
    object sender,
    EventArgs e)
        {
            if (this.txt_temp.Visible)
            {
                this.tmr_tiempo.Enabled =
                    false;

                ColocaCaja(
                    nRenglon);

                MarcaRenglon();
            }

            if (this.dgv_fact.IsCurrentCellInEditMode)
            {
                this.dgv_fact.EndEdit();
            }

            this.dgv_fact.CommitEdit(
                DataGridViewDataErrorContexts.Commit);

            bool estatusConfirmar =
                true;

            bool estatusConfirmar2 =
                true;

            foreach (DataGridViewRow row in
                this.dgv_fact.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                double cantidadFacturada =
                    Convert.ToDouble(
                        this.dgv_fact[
                            "CantidadF",
                            row.Index]
                            .Value);

                double cantidadRecibida =
                    Convert.ToDouble(
                        this.dgv_fact[
                            "CantidadR",
                            row.Index]
                            .Value);

                if (cantidadFacturada !=
                    cantidadRecibida)
                {
                    MessageBox.Show(
                        "Linea No. " +
                        this.dgv_fact.Rows[
                            row.Index]
                            .HeaderCell
                            .Value
                            .ToString() +
                        " : Cantidad incompleta.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);

                    this.dgv_fact.CurrentCell =
                        this.dgv_fact.Rows[
                            row.Index]
                            .Cells["CantidadR"];

                    this.dgv_fact.Rows[
                        row.Index]
                        .Selected =
                        true;

                    this.dgv_fact.Focus();

                    estatusConfirmar =
                        false;

                    break;
                }
            }

            if (!estatusConfirmar)
            {
                return;
            }

            foreach (DataGridViewRow row in
                this.dgv_fact.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                try
                {
                    string numeroCaja =
                        this.dgv_fact[
                            "Caja",
                            row.Index]
                            .Value == null
                            ? ""
                            : this.dgv_fact[
                                "Caja",
                                row.Index]
                                .Value
                                .ToString()
                                .Trim();

                    if (numeroCaja == "" ||
                        numeroCaja == "0")
                    {
                        MessageBox.Show(
                            "Linea No. " +
                            this.dgv_fact.Rows[
                                row.Index]
                                .HeaderCell
                                .Value
                                .ToString() +
                            " : No tiene número de caja asignada.",
                            "Advertencia",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);

                        this.dgv_fact.CurrentCell =
                            this.dgv_fact.Rows[
                                row.Index]
                                .Cells["Caja"];

                        this.dgv_fact.Rows[
                            row.Index]
                            .Selected =
                            true;

                        this.dgv_fact.Focus();

                        estatusConfirmar2 =
                            false;

                        break;
                    }
                }
                catch
                {
                    MessageBox.Show(
                        "Número de caja inválido.",
                        "",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    this.dgv_fact.Focus();

                    estatusConfirmar2 =
                        false;

                    break;
                }
            }

            if (!estatusConfirmar2)
            {
                return;
            }

            FechaFin =
                DateTime.Now
                    .ToString()
                    .Replace(
                        " p.m.",
                        "")
                    .Replace(
                        " a.m.",
                        "")
                    .Replace(
                        " p. m.",
                        "")
                    .Replace(
                        " a. m.",
                        "");

            this.txt_fechafin.Text =
                FechaFin;

            GrabaConfirmacion();
        }

        private bool ValidaFacturaConfirmada(string NumFact, int TotFact)
        {
            clss_Query QryFactBusq = new clss_Query();

            QryFactBusq.AsignaBase(Properties.Settings.Default.BaseRS);
            QryFactBusq.AsignaSQL("SELECT COUNT(*) FROM " + Properties.Settings.Default.CONFIRMACIONES +
                                  " WHERE NumFac IN (" + NumFact + ") AND Estatus IN ('" + Properties.Settings.Default.STS_TOTAL + "','" + Properties.Settings.Default.STS_PRELI + "')");
            QryFactBusq.Execute_SC();

            if ((int)QryFactBusq.ObtieneConsulta() == TotFact)
            {
                return true;
            }
            else if ((int)QryFactBusq.ObtieneConsulta() == 0)
            {
                t_flagEstadoFacturas = true;
                return false;
            }
            else
            {
                t_flagEstadoFacturas = false;
                return false;
            }
        }

        private void DespliegaFactura(
    string NumFact,
    int TotFact)
        {
            clss_Query QryFact =
                new clss_Query();

            clss_Query QryDetFact =
                new clss_Query();

            string documentos = "";

            string[] facturas =
                NumFact
                    .Replace(
                        "'",
                        "")
                    .Split(',');

            List<string> condiciones =
                new List<string>();

            foreach (string facturaBase in
                facturas)
            {
                string factura =
                    facturaBase.Trim();

                if (factura.Length < 2)
                {
                    continue;
                }

                string serie =
                    factura.Substring(
                        0,
                        1)
                        .Replace(
                            "'",
                            "''");

                string numero =
                    factura.Substring(1)
                        .Replace(
                            "'",
                            "''");

                condiciones.Add(
                    "(U_SERIE = '" +
                    serie +
                    "' AND U_NUMDOC = '" +
                    numero +
                    "')");
            }

            if (condiciones.Count == 0)
            {
                MessageBox.Show(
                    "No se capturaron facturas válidas.",
                    "Documento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }

            QryFact.AsignaBase(
                Properties.Settings.Default.BaseSAP);

            QryFact.AsignaSQL(
                "SELECT DISTINCT " +
                "DocNum," +
                "CardCode," +
                "CardName," +
                "DocTotal-VatSum," +
                "VatSum," +
                "DocTotal " +
                "FROM " +
                TablaH +
                " WHERE YEAR(DocDate)>=2017 " +
                "AND (" +
                string.Join(
                    " OR ",
                    condiciones.ToArray()) +
                ")");

            QryFact.Execute_DT();

            dtFact =
                QryFact.ObtieneTabla();

            for (int i = 0;
                 i <= dtFact.Rows.Count - 1;
                 i++)
            {
                documentos +=
                    "'" +
                    dtFact.Rows[i][0].ToString() +
                    "'";
            }

            documentos =
                documentos.Replace(
                    "''",
                    "','");

            if (QryFact.ObtieneRegistros() > 0)
            {
                if (dtFact.Rows.Count !=
                    TotFact)
                {
                    Cursor.Current =
                        Cursors.Default;

                    MessageBox.Show(
                        "Se solicitaron " +
                        TotFact +
                        " factura(s), pero SAP encontró " +
                        dtFact.Rows.Count +
                        ". Verifique los números capturados.",
                        "Facturas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);

                    return;
                }

                this.txt_prov.Text = "";
                this.txt_sub.Text = "";
                this.txt_imp.Text = "";
                this.txt_tot.Text = "";

                for (int i = 0;
                     i <= dtFact.Rows.Count - 1;
                     i++)
                {
                    this.txt_prov.Text +=
                        dtFact.Rows[i][1].ToString() +
                        "  -  " +
                        dtFact.Rows[i][2]
                            .ToString()
                            .Replace(
                                ":",
                                "") +
                        ":" +
                        char.ConvertFromUtf32(13) +
                        char.ConvertFromUtf32(10);

                    this.txt_sub.Text +=
                        "$ " +
                        string.Format(
                            "{0:00.00}",
                            dtFact.Rows[i][3]) +
                        ":" +
                        char.ConvertFromUtf32(13) +
                        char.ConvertFromUtf32(10);

                    this.txt_imp.Text +=
                        "$ " +
                        string.Format(
                            "{0:00.00}",
                            dtFact.Rows[i][4]) +
                        ":" +
                        char.ConvertFromUtf32(13) +
                        char.ConvertFromUtf32(10);

                    this.txt_tot.Text +=
                        "$ " +
                        string.Format(
                            "{0:00.00}",
                            dtFact.Rows[i][5]) +
                        ":" +
                        char.ConvertFromUtf32(13) +
                        char.ConvertFromUtf32(10);
                }

                QryDetFact.AsignaBase(
                    Properties.Settings.Default.BaseSAP);

                QryDetFact.AsignaSQL(
                    "SELECT DISTINCT " +
                    "CAST(T0.U_Serie AS NVARCHAR(1))+" +
                    "CAST(T0.U_NumDoc AS NVARCHAR(10)) 'Factura'," +
                    "T0.DocNum 'Documento'," +
                    "ISNULL(T2.U_COD_BAR_PAQ,T1.CodeBars) 'CodigoPaq'," +
                    "T1.CodeBars 'CodigoBar'," +
                    "T1.ItemCode 'CodigoArt'," +
                    "T1.Dscription 'Descripcion'," +
                    "T1.Quantity 'CantidadF'," +
                    "0.000000 'CantidadR'," +
                    "T1.LineTotal 'Subtotal'," +
                    "T1.AcctCode 'Cuenta'," +
                    "T1.Project 'Proyecto'," +
                    "'0' 'Caja'," +
                    "T1.LineNum+1 'Linea'," +
                    "'' 'UnidadMed' " +
                    "FROM " +
                    TablaH +
                    " T0 " +
                    "INNER JOIN " +
                    TablaD +
                    " T1 ON T0.DocEntry = T1.DocEntry " +
                    "INNER JOIN OITM T2 ON T2.ItemCode = T1.ItemCode " +
                    "WHERE T0.DocNum IN (" +
                    documentos +
                    ") " +
                   "ORDER BY T0.DocNum,T1.LineNum+1");

                QryDetFact.Execute_DT();

                dtDetFact =
                    QryDetFact.ObtieneTabla();

                dtDetFact =
                    EliminarDuplicadosDetalleFactura(
                        dtDetFact);

                this.dgv_fact.DataSource =
                    dtDetFact;

                MessageBox.Show(
                    "Consulta terminada.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);

                this.gpb_fact.Visible =
                    true;

                foreach (DataGridViewRow row in
                    dgv_fact.Rows)
                {
                    if (dgv_fact.Rows[
                        row.Index].Selected)
                    {
                        dgv_fact.Rows[
                            row.Index].Selected =
                            false;
                    }

                    dgv_fact.Rows[
                        row.Index]
                        .HeaderCell.Value =
                        this.dgv_fact[
                            "Linea",
                            row.Index]
                            .Value
                            .ToString();
                }

                FechaIni =
                    DateTime.Now
                        .ToString()
                        .Replace(
                            " p.m.",
                            "")
                        .Replace(
                            " a.m.",
                            "")
                        .Replace(
                            " p. m.",
                            "")
                        .Replace(
                            " a. m.",
                            "");

                this.btn_confirmar.Enabled =
                    true;

                this.btn_imprimir.Enabled =
                    true;

                this.btn_pdfDetalle.Enabled =
                    true;

                this.btn_pdfResumen.Enabled =
                    true;

                this.dgv_fact.Focus();

                Cursor.Current =
                    Cursors.Default;
            }
            else
            {
                Cursor.Current =
                    Cursors.Default;

                MessageBox.Show(
                    "Documento(s) No. " +
                    NumFact +
                    " no encontrado(s). Verifique información.",
                    "Documento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                LimpiaPantalla();
            }
        }

        private DataTable EliminarDuplicadosDetalleFactura(DataTable datos)
        {
            if (datos == null)
            {
                return new DataTable();
            }

            if (datos.Rows.Count == 0)
            {
                return datos;
            }

            DataTable resultado =
                datos.Clone();

            HashSet<string> registrosProcesados =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (DataRow fila in datos.Rows)
            {
                string factura =
                    fila["Factura"] == DBNull.Value
                        ? ""
                        : fila["Factura"].ToString().Trim();

                string documento =
                    fila["Documento"] == DBNull.Value
                        ? ""
                        : fila["Documento"].ToString().Trim();

                string codigoArt =
                    fila["CodigoArt"] == DBNull.Value
                        ? ""
                        : fila["CodigoArt"].ToString().Trim();

                string codigoBar =
                    fila["CodigoBar"] == DBNull.Value
                        ? ""
                        : fila["CodigoBar"].ToString().Trim();

                string descripcion =
                    fila["Descripcion"] == DBNull.Value
                        ? ""
                        : fila["Descripcion"].ToString().Trim();

                string clave =
                    factura + "|" +
                    documento + "|" +
                    codigoArt + "|" +
                    codigoBar + "|" +
                    descripcion;

                if (!registrosProcesados.Contains(clave))
                {
                    registrosProcesados.Add(clave);

                    resultado.ImportRow(fila);
                }
            }

            return resultado;
        }

        private void ConsultaFacturaConfirmada(string NumFact, int TotFact)
        {
            clss_Query QryHeadFac = new clss_Query();
            clss_Query QryFact = new clss_Query();
            clss_Query QryDetFact = new clss_Query();
            clss_Query QryPrelim = new clss_Query();

            QryHeadFac.AsignaBase(Properties.Settings.Default.BaseRS);
            QryHeadFac.AsignaSQL("SELECT TOP 1 FechaIni,FechaFin " +
                                 "FROM " + Properties.Settings.Default.CONFIRMACIONES + " WHERE NumFac IN (" + NumFact + ") " +
                                 "AND Tipo='" + TablaCH + "'");
            QryHeadFac.Execute_DT();
            dtFact = QryHeadFac.ObtieneTabla();
            this.txt_fechaini.Text = dtFact.Rows[0][0].ToString();
            this.txt_fechafin.Text = dtFact.Rows[0][1].ToString();

            QryFact.AsignaBase(Properties.Settings.Default.BaseRS);
            QryFact.AsignaSQL("SELECT DocNum,SocioNegocio,Subtotal,Impuesto,TotalFac " +
                              "FROM " + TablaCH + " WHERE NumFac IN (" + NumFact + ") ");
            QryFact.Execute_DT();
            dtFact = QryFact.ObtieneTabla();

            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";
            for (int i = 0; i <= dtFact.Rows.Count - 1; i++)
            {
                this.txt_prov.Text += dtFact.Rows[i][1].ToString().Replace(char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10), "") + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                this.txt_sub.Text += "$ " + string.Format("{0:00.00}", dtFact.Rows[i][2]) + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                this.txt_imp.Text += "$ " + string.Format("{0:00.00}", dtFact.Rows[i][3]) + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                this.txt_tot.Text += "$ " + string.Format("{0:00.00}", dtFact.Rows[i][4]) + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
            }

            QryDetFact.AsignaBase(Properties.Settings.Default.BaseRS);
            QryDetFact.AsignaSQL("SELECT NumFac 'Factura',Documento,CodigoPaq,CodigoBar,CodigoArt,Descripcion,CantidadF,CantidadR,TotalLin 'Subtotal',Cuenta,Proyecto,NoCaja 'Caja',Linea 'Linea', UnidadMed 'UnidadMed' " +
                                 "FROM " + TablaCD + " WHERE NumFac IN (" + NumFact + ") ORDER BY NoCaja ASC,Documento ASC,Linea ASC");
            QryDetFact.Execute_DT();

            dtDetFact =
                QryDetFact.ObtieneTabla();

            dtDetFact =
                EliminarDuplicadosDetalleFactura(
                    dtDetFact);

            this.dgv_fact.DataSource =
                dtDetFact;

            this.btn_pdfDetalle.Enabled = true;
            this.btn_pdfResumen.Enabled = true;
            //this.tsl_estatus.BackColor = Color.Green;
            //this.tsl_estatus.Text = "Consulta terminada.";           
            MessageBox.Show("Consulta terminada.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.gpb_fact.Visible = true;

            contador = 1;
            foreach (DataGridViewRow row in dgv_fact.Rows)
            {
                if (dgv_fact.Rows[row.Index].Selected == true)
                {
                    dgv_fact.Rows[row.Index].Selected = false;
                }
                dgv_fact.Rows[row.Index].HeaderCell.Value = this.dgv_fact["Linea", row.Index].Value.ToString();
                contador += 1;
            }
            MarcaRenglon();

            //Detecta que tipo de consulta realiza Preliminar o Confirmada.
            QryPrelim.AsignaBase(Properties.Settings.Default.BaseRS);
            QryPrelim.AsignaSQL("SELECT TOP 1 Estatus " +
                                 "FROM " + Properties.Settings.Default.CONFIRMACIONES + " WHERE NumFac IN (" + NumFact + ") " +
                                 "AND Tipo='" + TablaCH + "'");
            QryPrelim.Execute_SC();

            if (QryPrelim.ObtieneConsulta().ToString() == Properties.Settings.Default.STS_TOTAL)
            {
                this.btn_confirmar.Enabled = false;
                this.btn_imprimir.Enabled = true;
                estatusProceso = false;
                this.dgv_fact.Focus();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Documento(s) No. " + NumFact.Replace("'", "").Replace(",", " ") + " confirmado(s).", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else // Es Preliminar
            {
                this.btn_confirmar.Enabled = true;
                this.btn_imprimir.Enabled = false;
                estatusProceso = true;
                this.dgv_fact.Focus();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Documento(s) No. " + NumFact.Replace("'", "").Replace(",", " ") + " preliminar(es).", "Guardado preliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GrabaConfirmacion()
        {
            //string Parcialidad = "";
            frmAutenticacion f = new frmAutenticacion();
            f.ShowDialog();

            if (f.estado)
            {
                clss_Query QryFactConf = new clss_Query();
                //clss_Query QryDetFactConf = new clss_Query();
                string g_Factura = "";
                string t_Factura = "";
                string p_Factura = "";
                string g_Documento = "";

                Cursor.Current = Cursors.WaitCursor;
                //Parcialidad = "1"; // ObtieneParcialidad(NumFact);

                foreach (DataGridViewRow row in dgv_fact.Rows)
                {
                    g_Factura = this.dgv_fact["Factura", row.Index].Value.ToString();
                    g_Documento = this.dgv_fact["Documento", row.Index].Value.ToString();

                    //QryDetFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                    //QryDetFactConf.AsignaSQL("INSERT INTO " + TablaCD + " VALUES (" + (row.Index + 1) + "," + Parcialidad + ",'" + g_Factura + "','" + this.dgv_fact["CodigoPaq", row.Index].Value +
                    //                         "','" + this.dgv_fact["CodigoBar", row.Index].Value + "','" + this.dgv_fact["CodigoArt", row.Index].Value +
                    //                         "','" + this.dgv_fact["Descripcion", row.Index].Value.ToString().Replace("'"," ") + "'," + this.dgv_fact["CantidadF", row.Index].Value +
                    //                         "," + this.dgv_fact["CantidadR", row.Index].Value + "," + this.dgv_fact["Subtotal", row.Index].Value +
                    //                         ",'" + this.dgv_fact["Cuenta", row.Index].Value + "','" + this.dgv_fact["Proyecto", row.Index].Value +
                    //                         "','" + Properties.Settings.Default.STS_TOTAL + "','" + this.dgv_fact["Caja", row.Index].Value + "','" + this.dgv_fact["Documento", row.Index].Value + "','')");
                    //QryDetFactConf.Execute_IDU();

                    if (g_Factura != t_Factura)
                    {
                        //QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                        //QryFactConf.AsignaSQL("INSERT INTO " + Properties.Settings.Default.CONFIRMACIONES + " VALUES ('" + g_Factura + "','" + FechaIni +
                        //                      "','" + FechaFin + "','" + TablaCH + "','" + Properties.Settings.Default.STS_TOTAL + "','','')");
                        //QryFactConf.Execute_IDU();

                        //QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                        //QryFactConf.AsignaSQL("INSERT INTO " + TablaCH + " VALUES (" + Parcialidad + ",'" + g_Factura + "','" + g_Documento +
                        //                      "','" + this.txt_prov.Text.Split(':')[g_contador] + "'," + this.txt_sub.Text.Split(':')[g_contador] +
                        //                      "," + this.txt_imp.Text.Split(':')[g_contador] + "," + this.txt_tot.Text.Split(':')[g_contador] + ",'" + f.usuario +
                        //                      "','" + Properties.Settings.Default.STS_TOTAL + "','','')");
                        //QryFactConf.Execute_IDU();

                        QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                        QryFactConf.AsignaSQL("UPDATE " + Properties.Settings.Default.CONFIRMACIONES +
                                              " SET Estatus = '" + Properties.Settings.Default.STS_TOTAL + "'" +
                                              " WHERE NumFac = '" + g_Factura + "'");
                        QryFactConf.Execute_IDU();
                        QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                        QryFactConf.AsignaSQL("UPDATE " + TablaCH +
                                              " SET EstatusFac = '" + Properties.Settings.Default.STS_TOTAL + "',Usuario = '" + f.usuario + "'" +
                                              " WHERE NumFac = '" + g_Factura + "'");
                        QryFactConf.Execute_IDU();
                        QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                        QryFactConf.AsignaSQL("UPDATE " + TablaCD +
                                              " SET EstatusLinea = '" + Properties.Settings.Default.STS_TOTAL + "'" +
                                              " WHERE NumFac = '" + g_Factura + "'");
                        QryFactConf.Execute_IDU();

                        t_Factura = g_Factura;
                        p_Factura += " " + g_Factura;
                    }
                }

                this.dgv_fact.Focus();
                Cursor.Current = Cursors.Default;
                //this.tsl_estatus.BackColor = Color.Green;
                //this.tsl_estatus.Text = "Documento(s) confirmado(s).";
                DialogResult resp = MessageBox.Show("Documento(s) No. " + p_Factura + " confirmado(s). ¿Desea imprimir ahora el comprobante?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resp == DialogResult.Yes)
                {
                    dtDetFact.DefaultView.Sort = "Caja ASC";
                    MarcaRenglon();
                    Pagina = 1;
                    R = 0;
                    ConteoParcial = 0;
                    ImprimeConfirmacion();
                }
                LimpiaPantalla();
            }
        }

        private void GrabaPreliminar()
        {
            string Parcialidad = "";

            clss_Query QryFactConf = new clss_Query();
            clss_Query QryDetFactConf = new clss_Query();
            string g_Factura = "";
            string t_Factura = "";
            string p_Factura = "";
            string g_Documento = "";
            int g_contador = 0;

            string fechaIniSql = DateTime.Parse(FechaIni).ToString("yyyyMMdd HH:mm:ss");
            string fechaFinSql = DateTime.Parse(FechaFin).ToString("yyyyMMdd HH:mm:ss");


            foreach (DataGridViewRow row in dgv_fact.Rows)
            {
                g_Factura = this.dgv_fact["Factura", row.Index].Value.ToString();
                g_Documento = this.dgv_fact["Documento", row.Index].Value.ToString();

                if (g_Factura != t_Factura)
                {
                    Parcialidad = ObtieneParcialidad(g_Factura);
                }

                QryDetFactConf.AsignaBase(Properties.Settings.Default.BaseRS);


                QryDetFactConf.AsignaSQL("INSERT INTO " + TablaCD + " VALUES (" + this.dgv_fact["Linea", row.Index].Value + "," + Parcialidad + ",'" + g_Factura + "','" + this.dgv_fact["CodigoPaq", row.Index].Value +
                                         "','" + this.dgv_fact["CodigoBar", row.Index].Value + "','" + this.dgv_fact["CodigoArt", row.Index].Value +
                                         "','" + this.dgv_fact["Descripcion", row.Index].Value.ToString().Replace("'", " ") + "'," + this.dgv_fact["CantidadF", row.Index].Value +
                                         "," + this.dgv_fact["CantidadR", row.Index].Value + "," + this.dgv_fact["Subtotal", row.Index].Value +
                                         ",'" + this.dgv_fact["Cuenta", row.Index].Value + "','" + this.dgv_fact["Proyecto", row.Index].Value +
                                         "','" + Properties.Settings.Default.STS_PRELI + "','" + this.dgv_fact["Caja", row.Index].Value + "','" + this.dgv_fact["Documento", row.Index].Value + "','','" + this.dgv_fact["UnidadMed", row.Index].Value.ToString() + "')");
                QryDetFactConf.Execute_IDU();

                if (g_Factura != t_Factura)
                {
                    QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                    QryFactConf.AsignaSQL(
                    "INSERT INTO " + Properties.Settings.Default.CONFIRMACIONES +
                    " VALUES ('" + g_Factura +
                    "','" + fechaIniSql +
                    "','" + fechaFinSql +
                    "','" + TablaCH +
                    "','" + Properties.Settings.Default.STS_PRELI +
                    "','','')");
                    QryFactConf.Execute_IDU();

                    QryFactConf.AsignaBase(Properties.Settings.Default.BaseRS);
                    QryFactConf.AsignaSQL("INSERT INTO " + TablaCH + " VALUES (" + Parcialidad + ",'" + g_Factura + "','" + g_Documento +
                                          "','" + this.txt_prov.Text.Split(':')[g_contador] + "'," + this.txt_sub.Text.Split(':')[g_contador] +
                                          "," + this.txt_imp.Text.Split(':')[g_contador] + "," + this.txt_tot.Text.Split(':')[g_contador] + ",'" +
                                          "','" + Properties.Settings.Default.STS_PRELI + "','','')");
                    QryFactConf.Execute_IDU();

                    t_Factura = g_Factura;
                    p_Factura += " " + g_Factura;
                    g_contador += 1;
                }
            }
            //MessageBox.Show("¡Graba Preliminar!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
        }

        private void MarcaRenglon()
        {
            foreach (DataGridViewRow row in dgv_fact.Rows)
            {
                if (Convert.ToDouble(this.dgv_fact["CantidadF", row.Index].Value) == Convert.ToDouble(this.dgv_fact["CantidadR", row.Index].Value) && this.dgv_fact["Caja", row.Index].Value.ToString() != "0" && this.dgv_fact["Caja", row.Index].Value.ToString().Trim() != "")
                {
                    this.dgv_fact.Rows[row.Index].DefaultCellStyle.BackColor = Color.LimeGreen;
                }
                else
                {
                    this.dgv_fact.Rows[row.Index].DefaultCellStyle.BackColor = Color.White;
                }
                this.dgv_fact["CantidadR", row.Index].Style.BackColor = ColorTranslator.FromHtml("#C0FFC0");
                this.dgv_fact["Caja", row.Index].Style.BackColor = ColorTranslator.FromHtml("#FFC0C0");
                this.dgv_fact[13, row.Index].Style.BackColor = Color.LightSteelBlue;
            }
        }

        private void btn_imprimir_Click(object sender, EventArgs e)
        {
            Pagina = 1;
            R = 0;
            ConteoParcial = 0;
            ImprimeConfirmacion();
        }

        private void ImprimeConfirmacion()
        {
            // Definimos Los Margenes De La Hoja Para Tamaño Carta.
            printDocument1.DefaultPageSettings.Margins.Left = 200;
            printDocument1.DefaultPageSettings.Margins.Top = 200;
            printDocument1.DefaultPageSettings.Margins.Right = 200;
            printDocument1.DefaultPageSettings.Margins.Bottom = 200;
            printDocument1.DefaultPageSettings.Landscape = true;
            printDocument1.DocumentName =
            ObtenerFacturaActual();
            //Se Imprime Documento Y Oculto La Ventana De Mensaje De Impresion
            //Mediante El recargado del Controlador De Impresion a Standard.
            try
            {
                StandardPrintController pc = new StandardPrintController();
                printDocument1.PrintController = pc;
                printDocument1.Print();
            }
            catch
            {
            }
        }

        private void ImprimeCadena(string Cadena, PrintPageEventArgs ev, int lineas)
        {
            Font myFontCabecera = new Font("Courier New", 8, FontStyle.Regular);
            int Inicio;

            Inicio = 0;
            while (Inicio < Cadena.Length)
            {
                if (Cadena.Length - Inicio > margen)
                {
                    ev.Graphics.DrawString(Cadena.Substring(Inicio, margen), myFontCabecera, Brushes.Black, 1, aYPos);
                    aYPos += myFontCabecera.Height; //(lineas * myFontCabecera.Height)
                    Inicio += margen;
                }
                else
                {
                    ev.Graphics.DrawString(Cadena.Substring(Inicio, Cadena.Length - Inicio), myFontCabecera, Brushes.Black, 1, aYPos);
                    break;
                }
            }
            if (lineas == 0)
            {
                lineas = 1;
            }
            aYPos += (lineas * myFontCabecera.Height);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x;

            try
            {
                Encabezado(e);
                if (ConteoParcial > dgv_fact.RowCount - 1)
                {
                    PiePagina(e);
                    e.HasMorePages = false;
                    MessageBox.Show("Impresión finalizada.", "Aviso de impresión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                for (x = ConteoParcial; x <= dgv_fact.RowCount - 1; x++)
                {
                    if (R > numReg)
                    {
                        R = 0;
                        ConteoParcial = x;
                        Pagina += 1;
                        e.HasMorePages = true;
                        return;
                    }
                    ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena(this.dgv_fact["Factura", x].Value.ToString(), 8, " ", 'D') + " " +
                                  Func.CompletaCadena(this.dgv_fact["Documento", x].Value.ToString(), 8, " ", 'D') + " " +
                                  //Func.CompletaCadena(this.dgv_fact["CodigoPaq", x].Value.ToString(), 15, " ", 'D') + "  " +
                                  //Func.CompletaCadena(this.dgv_fact["CodigoBar", x].Value.ToString(), 15, " ", 'D') + "  " +
                                  Func.CompletaCadena(this.dgv_fact["CodigoArt", x].Value.ToString(), 8, " ", 'D') + "  " +
                                  Func.CompletaCadena(this.dgv_fact["Descripcion", x].Value.ToString(), 42, " ", 'D').Substring(0, 42) + "  " +
                                  Func.CompletaCadena(string.Format("{0:f}", this.dgv_fact["CantidadF", x].Value), 12, " ", 'I') + "  " +
                                  Func.CompletaCadena(string.Format("{0:f}", this.dgv_fact["CantidadR", x].Value), 12, " ", 'I') + "  " +
                                  Func.CompletaCadena(this.dgv_fact["Caja", x].Value.ToString(), 11, " ", 'D') + "  " +
                                  Func.CompletaCadena(this.dgv_fact["UnidadMed", x].Value.ToString(), 15, " ", 'D'), e, 1);
                    R += 1;
                    if (x == dgv_fact.RowCount - 1)
                    {
                        if (R + 9 > numReg)
                        {
                            ConteoParcial = x + 1;
                            Pagina += 1;
                            e.HasMorePages = true;
                            return;
                        }
                        else
                        {
                            PiePagina(e);
                            e.HasMorePages = false;
                            MessageBox.Show("Impresión finalizada.", "Aviso de impresión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch
            {
                e.HasMorePages = false;
                MessageBox.Show("Impresión incorrecta.", "Error de impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Encabezado(PrintPageEventArgs e)
        {
            aYPos = 1;
            ImprimeCadena(" ", e, 3);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena("=", 139, "=", 'D'), e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "VENTAS, SERVICIOS Y ESPECTACULOS RECREATIVOS S.A DE C.V" + Func.CompletaCadena("", 60, " ", 'D') + "SUMINISTRO DE MERCANCIAS", e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena("=", 139, "=", 'D'), e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena("", 127, " ", 'D') + "Página: " + Func.CompletaCadena(String.Format("{0:0.#}", Pagina), 3, " ", 'I'), e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "Fecha de Impresión: " + String.Format("{0:g}", DateTime.Now) + Func.CompletaCadena("", 20, " ", 'D') + "Confirmó: " + ObtieneQuienAutorizo(
    ObtenerFacturaActual()), e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "Fecha de Confirmación: " + String.Format("{0:g}", this.txt_fechafin.Text), e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena("-", 139, "-", 'D'), e, 1);
            R = 10;
            //ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "NO.      NO.      CODIGO           CODIGO           CODIGO    DESCRIPCION                                     CANTIDAD      CANTIDAD   NO.", e, 1);
            //ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "FACTURA  DOCTO.   BARRAS           ARTICULO                                                                   FACTURADA     RECIBIDA   CAJA", e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "NO.      NO.      CODIGO    DESCRIPCION                                     CANTIDAD      CANTIDAD  NO.         UNIDAD", e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "FACTURA  DOCTO.                                                             FACTURADA     RECIBIDA  CAJA        MED/EMP", e, 1);
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena("-", 139, "-", 'D'), e, 2);
            R += 4;
        }

        private void PiePagina(PrintPageEventArgs e)
        {
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + Func.CompletaCadena("-", 139, "-", 'D'), e, 5);
            ImprimeCadena(Func.CompletaCadena("", 28, " ", 'D') + Func.CompletaCadena("_", 34, "_", 'D') + Func.CompletaCadena("", 36, " ", 'D') + Func.CompletaCadena("_", 34, "_", 'D'), e, 2);
            ImprimeCadena(Func.CompletaCadena("", 42, " ", 'D') + "ENTREGA              " + Func.CompletaCadena("", 50, " ", 'D') + "RECIBE", e, 1);
            R += 8;
        }

        private void dgv_fact_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if ((e.ColumnIndex == 11 && estatusProceso) || (e.ColumnIndex == 13 && estatusProceso))
                {
                    Rectangle rec;
                    rec = this.dgv_fact.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    this.txt_temp.Size = new Size(rec.Size.Width, rec.Size.Height);
                    this.txt_temp.Location = new Point(rec.Location.X + this.dgv_fact.Location.X, rec.Location.Y + this.dgv_fact.Location.Y);
                    t_contador = 0;
                    nRenglon = e.RowIndex;
                    nColumna = e.ColumnIndex;
                    this.txt_temp.Visible = true;

                    this.txt_temp.Text =
                        this.dgv_fact[
                            e.ColumnIndex,
                            e.RowIndex]
                            .Value
                            .ToString();

                    this.txt_temp.BringToFront();

                    this.txt_temp.Focus();

                    this.txt_temp.SelectionStart =
                        this.txt_temp.Text.Length;

                    this.txt_temp.SelectionLength =
    0;

                    this.tmr_tiempo.Enabled =
                        false;
                }
                else
                {
                    this.txt_temp.Visible = false;

                    MostrarResumenEmpaques(
                        ObtenerFacturasSql());

                    this.dgv_fact.Focus();
                }
            }
        }

        private void txt_temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '-' || e.KeyChar == ',' || e.KeyChar == '_' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'a' & e.KeyChar <= 'z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'A' & e.KeyChar <= 'Z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
                this.tmr_tiempo.Enabled = false;
                ColocaCaja(nRenglon);
                MarcaRenglon();
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tmr_tiempo_Tick(object sender, EventArgs e)
        {
            if (t_contador == 15)
            {
                this.tmr_tiempo.Enabled = false;
                ColocaCaja(nRenglon);
                MarcaRenglon();
            }
            t_contador += 1;
        }

        private void txt_temp_MouseLeave(object sender, EventArgs e)
        {
            this.tmr_tiempo.Enabled = false;
            ColocaCaja(nRenglon);
            MarcaRenglon();
        }


        private void ColocaCaja(int valor)
        {
            if (this.txt_temp.Text.Trim() != "")
            {
                if (nColumna == 11)
                    this.dgv_fact["Caja", valor].Value = this.txt_temp.Text;
                else if (nColumna == 13)
                    this.dgv_fact["UnidadMed", valor].Value = this.txt_temp.Text;
            }
            else
            {
                if (nColumna == 11)
                    this.dgv_fact["Caja", valor].Value = 0;
                else if (nColumna == 13)
                    this.dgv_fact["UnidadMed", valor].Value = 0;
            }

            if (nColumna == 11)
                GrabaLineaPreliminar(this.dgv_fact.Rows[valor].HeaderCell.Value.ToString(), this.dgv_fact.Rows[valor].Cells["Factura"].Value.ToString(), this.dgv_fact.Rows[valor].Cells["CodigoPaq"].Value.ToString(), this.dgv_fact.Rows[valor].Cells["Caja"].Value.ToString(), "X");
            else if (nColumna == 13)
                GrabaLineaPreliminar(this.dgv_fact.Rows[valor].HeaderCell.Value.ToString(), this.dgv_fact.Rows[valor].Cells["Factura"].Value.ToString(), this.dgv_fact.Rows[valor].Cells["CodigoPaq"].Value.ToString(), this.dgv_fact.Rows[valor].Cells["UnidadMed"].Value.ToString(), "M");
            this.txt_temp.Visible = false;
            this.dgv_fact.Focus();
        }

        private void GrabaLineaPreliminar(string p_indice, string p_factura, string p_codigo, string p_valor, string p_tipo)
        {
            clss_Query QryLineaPre = new clss_Query();
            QryLineaPre.AsignaBase(Properties.Settings.Default.BaseRS);
            if (p_tipo == "C") // 'C' Cantidad
            {
                QryLineaPre.AsignaSQL("UPDATE " + TablaCD +
                                      " SET CantidadR = " + p_valor +
                                      " WHERE NumFac = '" + p_factura + "' AND CodigoPaq = '" + p_codigo + "' AND Linea = " + p_indice);
            }
            else if (p_tipo == "X") // 'X' Caja
            {
                QryLineaPre.AsignaSQL("UPDATE " + TablaCD +
                                      " SET NoCaja = '" + p_valor + "'" +
                                      " WHERE NumFac = '" + p_factura + "' AND CodigoPaq = '" + p_codigo + "' AND Linea = " + p_indice);
            }
            else if (p_tipo == "M") // 'M' Medida
            {
                QryLineaPre.AsignaSQL("UPDATE " + TablaCD +
                                      " SET UnidadMed = '" + p_valor + "'" +
                                      " WHERE NumFac = '" + p_factura + "' AND CodigoPaq = '" + p_codigo + "' AND Linea = " + p_indice);
            }

            QryLineaPre.Execute_IDU();
        }

        public string ObtieneParcialidad(string NumFact)
        {
            string valor;
            clss_Query QryParcialidad = new clss_Query();

            QryParcialidad.AsignaBase(Properties.Settings.Default.BaseRS);
            QryParcialidad.AsignaSQL("SELECT ISNULL(MAX(Parcialidad),0) FROM " + TablaCH + " WHERE NumFac='" + NumFact + "'");
            QryParcialidad.Execute_SC();

            if (QryParcialidad.ObtieneConsulta().ToString() == "")
            {
                valor = "0";
            }
            else
            {
                valor = ((int)QryParcialidad.ObtieneConsulta() + 1).ToString();
            }

            return valor;
        }

        public string ObtieneQuienAutorizo(string NumFact)
        {
            string valor;
            clss_Query QryParcialidad = new clss_Query();

            QryParcialidad.AsignaBase(Properties.Settings.Default.BaseRS);
            QryParcialidad.AsignaSQL("SELECT Usuario FROM " + TablaCH + " WHERE NumFac='" + NumFact + "'");
            QryParcialidad.Execute_SC();

            if (QryParcialidad.ObtieneConsulta().ToString() == "")
            {
                valor = "";
            }
            else
            {
                valor = QryParcialidad.ObtieneConsulta().ToString();
            }

            return valor;
        }

        private void txt_fact1_TextChanged(object sender, EventArgs e)
        {
            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";
            dtFact.Rows.Clear();
            dtDetFact.Rows.Clear();
        }

        private void txt_fact2_TextChanged(object sender, EventArgs e)
        {
            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";
            dtFact.Rows.Clear();
            dtDetFact.Rows.Clear();
        }

        private void txt_fact3_TextChanged(object sender, EventArgs e)
        {
            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";
            dtFact.Rows.Clear();
            dtDetFact.Rows.Clear();
        }

        private void txt_fact1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' & e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'a' & e.KeyChar <= 'z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'A' & e.KeyChar <= 'Z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
                btn_busq_Click(sender, e);
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txt_fact2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' & e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'a' & e.KeyChar <= 'z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'A' & e.KeyChar <= 'Z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
                btn_busq_Click(sender, e);
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txt_fact3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' & e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'a' & e.KeyChar <= 'z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar >= 'A' & e.KeyChar <= 'Z')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
                btn_busq_Click(sender, e);
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtCodeBar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                KeysPressedFirst = KeysPressedFirst + txtCodeBar.Text;
                txtCodeBar.Text = KeysPressedFirst;

            DeNuevo:
                if (rowF == 0)
                { i = 0; }
                else
                { i = rowF + 1; }

                for (; i <= (this.dgv_fact.Rows.Count - 1); i++)
                {
                    string cadena;
                    string paquete;

                    cadena = Convert.ToString(Convert.ToInt64(dgv_fact["CodigoBar", i].Value.ToString().ToUpper()));
                    cadena = cadena.Replace("'", "").Replace("#", "");

                    paquete = Convert.ToString(Convert.ToInt64(dgv_fact["CodigoPaq", i].Value.ToString().ToUpper()));
                    paquete = paquete.Replace("'", "").Replace("#", "");

                    if (cadena == Convert.ToString(Convert.ToInt64(KeysPressedFirst)) || paquete == Convert.ToString(Convert.ToInt64(KeysPressedFirst)))
                    {
                        Encontrado = true;
                        rowF = i;
                        this.dgv_fact.Rows[i].Selected = true;
                        this.dgv_fact.CurrentCell = this.dgv_fact.Rows[i].Cells["CodigoBar"];
                        txtCodeBar.Visible = false;
                        MessageBox.Show("Articulo encontrado", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        SumaCantidad(i);
                        MarcaRenglon();
                        GrabaLineaPreliminar(this.dgv_fact.Rows[i].HeaderCell.Value.ToString(), this.dgv_fact.Rows[i].Cells["Factura"].Value.ToString(), this.dgv_fact.Rows[i].Cells["CodigoPaq"].Value.ToString(), this.dgv_fact.Rows[i].Cells["CantidadR"].Value.ToString(), "C");
                        break;
                    }
                    else
                    {
                        dgv_fact.Rows[i].Selected = false;
                        if (i == this.dgv_fact.Rows.Count - 1)
                        {
                            if (!Encontrado)
                            {
                                rowF = 0;
                                txtCodeBar.Visible = false;
                                MessageBox.Show("***** ARTICULO NO ENCONTRADO *****", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                rowF = 0;
                                Encontrado = false;
                                goto DeNuevo;
                            }
                        }
                    }
                    this.dgv_fact.Focus();
                }
                txtCodeBar.Text = "";
                txtCodeBar.Visible = false;
                dgv_fact.Focus();
            }
        }


        private void btn_pdfDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                string facturasSql = ObtenerFacturasReporte();

                if (facturasSql == "")
                {
                    MessageBox.Show("Capture y consulte al menos una factura.", "Reporte de embarque", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                DataTable datos = ConsultarDetalleEmbarque(facturasSql);

                if (datos == null || datos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró información para generar el detalle de embarque.", "Reporte de embarque", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog dialogo = new SaveFileDialog())
                {
                    dialogo.Title = "Guardar detalle de embarque";
                    dialogo.Filter = "Archivo PDF (*.pdf)|*.pdf";
                    dialogo.DefaultExt = "pdf";
                    dialogo.AddExtension = true;
                    dialogo.FileName = "Detalle_Embarque_" + ObtenerNombreFacturasReporte() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                    if (dialogo.ShowDialog() == DialogResult.OK)
                    {
                        GenerarPdfDetalleEmbarque(datos, dialogo.FileName);
                        MessageBox.Show("El detalle de embarque se generó correctamente.", "Reporte generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible generar el detalle de embarque.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btn_pdfResumen_Click(object sender, EventArgs e)
        {
            try
            {
                string facturasSql = ObtenerFacturasReporte();

                if (facturasSql == "")
                {
                    MessageBox.Show("Capture y consulte al menos una factura.", "Reporte de embarque", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                DataTable datos = ConsultarResumenEmbarque(facturasSql);

                if (datos == null || datos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró información para generar el resumen de embarque.", "Reporte de embarque", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog dialogo = new SaveFileDialog())
                {
                    dialogo.Title = "Guardar resumen de embarque";
                    dialogo.Filter = "Archivo PDF (*.pdf)|*.pdf";
                    dialogo.DefaultExt = "pdf";
                    dialogo.AddExtension = true;
                    dialogo.FileName = "Resumen_Embarque_" + ObtenerNombreFacturasReporte() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                    if (dialogo.ShowDialog() == DialogResult.OK)
                    {
                        GenerarPdfResumenEmbarque(datos, dialogo.FileName);
                        MessageBox.Show("El resumen de embarque se generó correctamente.", "Reporte generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible generar el resumen de embarque.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string ObtenerFacturasReporte()
        {
            return ObtenerFacturasSql();
        }

        private string ObtenerNombreFacturasReporte()
        {
            List<string> facturas =
                ObtenerFacturasCapturadas();

            return string.Join(
                "_",
                facturas.ToArray());
        }



        private DataTable ConsultarDetalleEmbarque(string facturasSql)
        {
            clss_Query consulta = new clss_Query();
            consulta.AsignaBase(Properties.Settings.Default.BaseRS);
            consulta.AsignaSQL(
                "SELECT " +
                "ENC.SocioNegocio AS Cliente, " +
                "CAST(CON.FechaIni AS DATE) AS [Fecha inicio], " +
                "CAST(CON.FechaFin AS DATE) AS [Fecha termina], " +
                "ENC.NumFac AS [Docto Fiscal], " +
                "ENC.DocNum AS [Docto SAP], " +
                "DET.Linea AS [No. Partida], " +
                "DET.CodigoArt AS [Codigo producto], " +
                "DET.CodigoBar AS [Codigo de Barras], " +
                "DET.Descripcion AS Descripcion, " +
                "DET.CantidadF AS [Cantidad Facturada], " +
                "DET.CantidadR AS [Cantidad Embarcada], " +
                "DET.NoCaja AS [Tipo y numero de empaque], " +
                "CASE " +
                "WHEN DET.NoCaja LIKE '%C%' THEN 'Caja' " +
                "WHEN DET.NoCaja LIKE '%B%' THEN 'Bulto' " +
                "WHEN DET.NoCaja LIKE '%P%' THEN 'Paquete' " +
                "ELSE 'Otros' " +
                "END AS [Nombre emp], " +
                "DET.UnidadMed AS [Observaciones 1], " +
                "DET.Especial2 AS [Observaciones 2] " +
                "FROM dbo.RS_CONFIRMACIONES CON " +
                "INNER JOIN dbo.RS_SUMINISTRO_CAB ENC ON CON.NumFac = ENC.NumFac " +
                "INNER JOIN dbo.RS_SUMINISTRO_DET DET ON DET.NumFac = ENC.NumFac " +
                "WHERE ENC.NumFac IN (" + facturasSql + ") " +
                "ORDER BY DET.NumFac, DET.NoCaja, DET.CodigoBar");
            consulta.Execute_DT();
            return consulta.ObtieneTabla();
        }

        private DataTable ConsultarResumenEmbarque(string facturasSql)
        {
            clss_Query consulta = new clss_Query();
            consulta.AsignaBase(Properties.Settings.Default.BaseRS);
            consulta.AsignaSQL(
                "SELECT DISTINCT " +
                "ENC.SocioNegocio AS Cliente, " +
                "CAST(CON.FechaIni AS DATE) AS [Fecha inicio], " +
                "ENC.NumFac AS [Docto Fiscal], " +
                "ENC.DocNum AS [Docto SAP], " +
                "DET.NoCaja AS [Tipo y numero de empaque], " +
                "CASE " +
                "WHEN DET.NoCaja LIKE '%C%' THEN 'Caja' " +
                "WHEN DET.NoCaja LIKE '%B%' THEN 'Bulto' " +
                "WHEN DET.NoCaja LIKE '%P%' THEN 'Paquete' " +
                "ELSE 'Otros' " +
                "END AS [Nombre emp], " +
                "DET.UnidadMed AS [Observaciones 1] " +
                "FROM dbo.RS_CONFIRMACIONES CON " +
                "INNER JOIN dbo.RS_SUMINISTRO_CAB ENC ON CON.NumFac = ENC.NumFac " +
                "INNER JOIN dbo.RS_SUMINISTRO_DET DET ON DET.NumFac = ENC.NumFac " +
                "WHERE ENC.NumFac IN (" + facturasSql + ") " +
                "ORDER BY ENC.NumFac, DET.NoCaja");
            consulta.Execute_DT();
            return consulta.ObtieneTabla();
        }

            private void GenerarPdfDetalleEmbarque(DataTable datos, string rutaArchivo)
            {
                iTextSharp.text.Rectangle pagina = iTextSharp.text.PageSize.A3.Rotate();

                using (FileStream archivo = new FileStream(
                    rutaArchivo,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None))
                {
                    using (PdfDocument documento = new PdfDocument(
                        pagina,
                        24f,
                        24f,
                        32f,
                        30f))
                    {
                        PdfWriter.GetInstance(documento, archivo);

                        documento.AddTitle("Detalle de Embarque");
                        documento.AddCreator("Suministro de mercancía");
                        documento.Open();

                        AgregarEncabezadoPdf(
                            documento,
                            "DETALLE DE EMBARQUE",
                            ObtenerNombreFacturasReporte());

                        float[] anchos =
                        {
                    18f,
                    7f,
                    7f,
                    7f,
                    6f,
                    5f,
                    7f,
                    8f,
                    20f,
                    7f,
                    7f,
                    8f,
                    7f,
                    8f,
                    8f
                };

                        PdfPTable tabla = new PdfPTable(anchos);
                        tabla.WidthPercentage = 100f;
                        tabla.HeaderRows = 1;
                        tabla.SplitLate = false;
                        tabla.SplitRows = true;

                        string[] encabezados =
                        {
                    "Cliente",
                    "Fecha inicio",
                    "Fecha termina",
                    "Docto Fiscal",
                    "Docto SAP",
                    "Partida",
                    "Código producto",
                    "Código barras",
                    "Descripción",
                    "Cant. facturada",
                    "Cant. embarcada",
                    "Tipo / No. empaque",
                    "Nombre empaque",
                    "Observaciones 1",
                    "Observaciones 2"
                };

                        foreach (string encabezado in encabezados)
                        {
                            tabla.AddCell(
                                CrearCeldaEncabezado(encabezado, 6.5f));
                        }

                        string facturaAnterior = "";
                        string documentoAnterior = "";

                        foreach (DataRow fila in datos.Rows)
                        {
                            string clienteActual =
                                ValorTexto(fila["Cliente"]);

                            string fechaInicioActual =
                                ValorFecha(fila["Fecha inicio"]);

                            string fechaTerminaActual =
                                ValorFecha(fila["Fecha termina"]);

                            string facturaActual =
                                ValorTexto(fila["Docto Fiscal"]);

                            string documentoActual =
                                ValorTexto(fila["Docto SAP"]);

                            string empaqueActual =
                                ValorTexto(fila["Tipo y numero de empaque"]);

                            string nombreEmpaqueActual =
                                ValorTexto(fila["Nombre emp"]);

                            string observacion1Actual =
                                ValorTexto(fila["Observaciones 1"]);

                            string observacion2Actual =
                                ValorTexto(fila["Observaciones 2"]);

                            bool nuevaFactura =
                                facturaActual != facturaAnterior ||
                                documentoActual != documentoAnterior;

                            tabla.AddCell(
                                CrearCeldaDato(
                                    nuevaFactura ? clienteActual : "",
                                    6f,
                                    iTextSharp.text.Element.ALIGN_LEFT));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    nuevaFactura ? fechaInicioActual : "",
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    nuevaFactura ? fechaTerminaActual : "",
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    nuevaFactura ? facturaActual : "",
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    nuevaFactura ? documentoActual : "",
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    ValorTexto(fila["No. Partida"]),
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    ValorTexto(fila["Codigo producto"]),
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    ValorTexto(fila["Codigo de Barras"]),
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    ValorTexto(fila["Descripcion"]),
                                    6f,
                                    iTextSharp.text.Element.ALIGN_LEFT));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    ValorCantidad(fila["Cantidad Facturada"]),
                                    6f,
                                    iTextSharp.text.Element.ALIGN_RIGHT));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    ValorCantidad(fila["Cantidad Embarcada"]),
                                    6f,
                                    iTextSharp.text.Element.ALIGN_RIGHT));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    empaqueActual,
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    nombreEmpaqueActual,
                                    6f,
                                    iTextSharp.text.Element.ALIGN_CENTER));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    observacion1Actual,
                                    6f,
                                    iTextSharp.text.Element.ALIGN_LEFT));

                            tabla.AddCell(
                                CrearCeldaDato(
                                    observacion2Actual,
                                    6f,
                                    iTextSharp.text.Element.ALIGN_LEFT));

                            facturaAnterior = facturaActual;
                            documentoAnterior = documentoActual;
                        }

                        documento.Add(tabla);
                        AgregarPiePdf(documento);
                    }
                }
            }

        private void GenerarPdfResumenEmbarque(DataTable datos, string rutaArchivo)
        {
            iTextSharp.text.Rectangle pagina = iTextSharp.text.PageSize.A4;

            using (FileStream archivo = new FileStream(
                rutaArchivo,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                using (PdfDocument documento = new PdfDocument(
                    pagina,
                    22f,
                    22f,
                    18f,
                    18f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(documento, archivo);

                    documento.AddTitle("Entrega de Pedido");
                    documento.AddCreator("Suministro de mercancía");
                    documento.Open();

                    AgregarFormatoEntregaPedido(
                        documento,
                        datos,
                        "ORIGINAL");

                    documento.Add(
                        CrearLineaCorte());

                    AgregarFormatoEntregaPedido(
                        documento,
                        datos,
                        "COPIA");

                    documento.Close();
                }
            }
        }

        private void AgregarFormatoEntregaPedido(
    PdfDocument documento,
    DataTable datos,
    string tipoCopia)
        {
            string cliente = "";
            string fecha = "";
            string facturas = "";
            string documentosSap = "";
            string observaciones = "";

            List<string> listaFacturas = new List<string>();
            List<string> listaDocumentosSap = new List<string>();
            List<string> listaObservacionesDetalle = new List<string>();

            int cantidadCajas = 0;
            int cantidadPaquetes = 0;
            int cantidadBolsas = 0;
            int cantidadOtros = 0;

            List<string> empaquesContados = new List<string>();

            foreach (DataRow fila in datos.Rows)
            {
                string clienteFila =
                    ValorTexto(fila["Cliente"]);

                string fechaFila =
                    ValorFecha(fila["Fecha inicio"]);

                string facturaFila =
                    ValorTexto(fila["Docto Fiscal"]);

                string documentoFila =
                    ValorTexto(fila["Docto SAP"]);

                string noEmpaque =
                    ValorTexto(fila["Tipo y numero de empaque"]);

                string nombreEmpaque =
                    ValorTexto(fila["Nombre emp"]);

                string observacionFila =
                    ValorTexto(fila["Observaciones 1"]);

                if (cliente == "")
                {
                    cliente = clienteFila;
                }

                if (fecha == "")
                {
                    fecha = fechaFila;
                }

                if (facturaFila != "" &&
                    !listaFacturas.Contains(facturaFila))
                {
                    listaFacturas.Add(facturaFila);
                }

                if (documentoFila != "" &&
                    !listaDocumentosSap.Contains(documentoFila))
                {
                    listaDocumentosSap.Add(documentoFila);
                }

                if (noEmpaque != "" &&
                    observacionFila != "" &&
                    observacionFila.Trim().ToUpper().Contains("LUJO"))
                {
                    string observacionLujo =
                        noEmpaque + " - LUJO";

                    if (!listaObservacionesDetalle.Contains(
                        observacionLujo))
                    {
                        listaObservacionesDetalle.Add(
                            observacionLujo);
                    }
                }

                if (noEmpaque != "" &&
                    !empaquesContados.Contains(noEmpaque))
                {
                    empaquesContados.Add(noEmpaque);

                    string tipo =
                        nombreEmpaque
                            .Trim()
                            .ToUpper();

                    if (tipo == "CAJA")
                    {
                        cantidadCajas++;
                    }
                    else if (tipo == "BULTO")
                    {
                        cantidadBolsas++;
                    }
                    else if (tipo == "PAQUETE")
                    {
                        cantidadPaquetes++;
                    }
                    else if (tipo == "BOLSA")
                    {
                        cantidadBolsas++;
                    }
                    else
                    {
                        cantidadOtros++;
                    }
                }
            }

            facturas =
                string.Join(
                    " / ",
                    listaFacturas.ToArray());

            documentosSap =
                string.Join(
                    " / ",
                    listaDocumentosSap.ToArray());

            observaciones =
                string.Join(
                    "\n",
                    listaObservacionesDetalle.ToArray());

            iTextSharp.text.BaseColor negro =
                iTextSharp.text.BaseColor.BLACK;

            iTextSharp.text.Font fuenteTitulo =
                iTextSharp.text.FontFactory.GetFont(
                    iTextSharp.text.FontFactory.HELVETICA_BOLD,
                    17f,
                    negro);

            iTextSharp.text.Font fuenteSubtitulo =
                iTextSharp.text.FontFactory.GetFont(
                    iTextSharp.text.FontFactory.HELVETICA_BOLD,
                    8f,
                    negro);

            iTextSharp.text.Font fuenteNormal =
                iTextSharp.text.FontFactory.GetFont(
                    iTextSharp.text.FontFactory.HELVETICA,
                    8f,
                    negro);

            iTextSharp.text.Font fuenteNormalGrande =
                iTextSharp.text.FontFactory.GetFont(
                    iTextSharp.text.FontFactory.HELVETICA,
                    9f,
                    negro);

            PdfPTable encabezado =
                new PdfPTable(
                    new float[]
                    {
                18f,
                67f,
                15f
                    });

            encabezado.WidthPercentage = 100f;

            PdfPCell celdaLogo =
                new PdfPCell();

            celdaLogo.Border =
                iTextSharp.text.Rectangle.BOX;

            celdaLogo.BorderWidth = 1.2f;

            celdaLogo.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            celdaLogo.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celdaLogo.PaddingTop = 3f;
            celdaLogo.PaddingBottom = 3f;
            celdaLogo.PaddingLeft = 3f;
            celdaLogo.PaddingRight = 3f;

            using (MemoryStream msLogo = new MemoryStream())
            {
                Properties.Resources.VESER.Save(
                    msLogo,
                    System.Drawing.Imaging.ImageFormat.Png);

                iTextSharp.text.Image logoVeser =
                    iTextSharp.text.Image.GetInstance(
                        msLogo.ToArray());

                logoVeser.ScaleToFit(
                    85f,
                    52f);

                logoVeser.Alignment =
                    iTextSharp.text.Element.ALIGN_CENTER;

                celdaLogo.AddElement(
                    logoVeser);
            }

            encabezado.AddCell(
                celdaLogo);

            PdfPCell celdaTitulo =
                new PdfPCell(
                    new iTextSharp.text.Phrase(
                        "ENTREGA DE PEDIDO",
                        fuenteTitulo));

            celdaTitulo.Border =
                iTextSharp.text.Rectangle.BOX;

            celdaTitulo.BorderWidth = 1.2f;

            celdaTitulo.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            celdaTitulo.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celdaTitulo.PaddingTop = 12f;
            celdaTitulo.PaddingBottom = 12f;

            encabezado.AddCell(
                celdaTitulo);

            PdfPCell celdaTipo =
                new PdfPCell(
                    new iTextSharp.text.Phrase(
                        tipoCopia,
                        fuenteSubtitulo));

            celdaTipo.Border =
                iTextSharp.text.Rectangle.BOX;

            celdaTipo.BorderWidth = 1.2f;

            celdaTipo.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            celdaTipo.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celdaTipo.Padding = 5f;

            encabezado.AddCell(
                celdaTipo);

            documento.Add(
                encabezado);

            PdfPTable informacion =
                new PdfPTable(
                    new float[]
                    {
                16f,
                34f,
                16f,
                34f
                    });

            informacion.WidthPercentage = 100f;

            informacion.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "LOCAL:",
                    fuenteSubtitulo));

            informacion.AddCell(
                CrearCeldaFormatoValor(
                    cliente,
                    fuenteNormalGrande));

            informacion.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "FECHA:",
                    fuenteSubtitulo));

            informacion.AddCell(
                CrearCeldaFormatoValor(
                    fecha,
                    fuenteNormalGrande));

            informacion.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "No. DE FACTURAS:",
                    fuenteSubtitulo));

            informacion.AddCell(
                CrearCeldaFormatoValor(
                    facturas,
                    fuenteNormalGrande));

            informacion.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "DOCTO. SAP:",
                    fuenteSubtitulo));

            informacion.AddCell(
                CrearCeldaFormatoValor(
                    documentosSap,
                    fuenteNormalGrande));

            documento.Add(
                informacion);

            PdfPTable tablaContenido =
                new PdfPTable(
                    new float[]
                    {
                34f,
                16f,
                16f,
                17f,
                17f
                    });

            tablaContenido.WidthPercentage = 100f;

            tablaContenido.AddCell(
                CrearCeldaFormatoEncabezado(
                    "CONTENIDO",
                    fuenteSubtitulo));

            tablaContenido.AddCell(
                CrearCeldaFormatoEncabezado(
                    "PEDIDO",
                    fuenteSubtitulo));

            tablaContenido.AddCell(
                CrearCeldaFormatoEncabezado(
                    "COMPL. 1",
                    fuenteSubtitulo));

            tablaContenido.AddCell(
                CrearCeldaFormatoEncabezado(
                    "COMPL. 2",
                    fuenteSubtitulo));

            tablaContenido.AddCell(
                CrearCeldaFormatoEncabezado(
                    "TOTAL",
                    fuenteSubtitulo));

            AgregarFilaEmpaque(
                tablaContenido,
                "C. DE CARTON",
                cantidadCajas,
                fuenteNormalGrande);

            AgregarFilaEmpaque(
                tablaContenido,
                "C. DE PLASTICO",
                0,
                fuenteNormalGrande);

            AgregarFilaEmpaque(
                tablaContenido,
                "PAQUETES",
                cantidadPaquetes,
                fuenteNormalGrande);

            AgregarFilaEmpaque(
                tablaContenido,
                "BOLSAS",
                cantidadBolsas,
                fuenteNormalGrande);

            if (cantidadOtros > 0)
            {
                AgregarFilaEmpaque(
                    tablaContenido,
                    "OTROS",
                    cantidadOtros,
                    fuenteNormalGrande);
            }

            documento.Add(
                tablaContenido);

            PdfPTable observacionesTabla =
                new PdfPTable(
                    new float[]
                    {
                20f,
                80f
                    });

            observacionesTabla.WidthPercentage = 100f;

            observacionesTabla.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "OBSERVACIONES:",
                    fuenteSubtitulo));

            PdfPCell celdaObservaciones =
                CrearCeldaFormatoValor(
                    observaciones,
                    fuenteNormalGrande);

            celdaObservaciones.MinimumHeight = 55f;

            observacionesTabla.AddCell(
                celdaObservaciones);

            documento.Add(
                observacionesTabla);

            PdfPTable firmas =
                new PdfPTable(
                    new float[]
                    {
                12f,
                38f,
                12f,
                38f
                    });

            firmas.WidthPercentage = 100f;

            firmas.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "ENTREGA:",
                    fuenteSubtitulo));

            PdfPCell entrega =
                CrearCeldaFormatoValor(
                    "",
                    fuenteNormal);

            entrega.MinimumHeight = 26f;

            firmas.AddCell(
                entrega);

            firmas.AddCell(
                CrearCeldaFormatoEtiqueta(
                    "RECIBE:",
                    fuenteSubtitulo));

            PdfPCell recibe =
                CrearCeldaFormatoValor(
                    "",
                    fuenteNormal);

            recibe.MinimumHeight = 26f;

            firmas.AddCell(
                recibe);

            documento.Add(
                firmas);
        }

        private void AgregarFilaEmpaque(
    PdfPTable tabla,
    string descripcion,
    int cantidad,
    iTextSharp.text.Font fuente)
        {
            PdfPCell celdaDescripcion =
                CrearCeldaFormatoValor(
                    descripcion,
                    fuente);

            celdaDescripcion.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_LEFT;

            tabla.AddCell(celdaDescripcion);

            PdfPCell celdaPedido =
                CrearCeldaFormatoValor(
                    cantidad > 0
                        ? cantidad.ToString()
                        : "",
                    fuente);

            celdaPedido.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            tabla.AddCell(celdaPedido);

            PdfPCell celdaComplemento1 =
                CrearCeldaFormatoValor(
                    "",
                    fuente);

            celdaComplemento1.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            tabla.AddCell(celdaComplemento1);

            PdfPCell celdaComplemento2 =
                CrearCeldaFormatoValor(
                    "",
                    fuente);

            celdaComplemento2.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            tabla.AddCell(celdaComplemento2);

            PdfPCell celdaTotal =
                CrearCeldaFormatoValor(
                    cantidad > 0
                        ? cantidad.ToString()
                        : "",
                    fuente);

            celdaTotal.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            tabla.AddCell(celdaTotal);
        }

        private PdfPCell CrearCeldaFormatoEtiqueta(
            string texto,
            iTextSharp.text.Font fuente)
        {
            PdfPCell celda =
                new PdfPCell(
                    new iTextSharp.text.Phrase(
                        texto,
                        fuente));

            celda.Border =
                iTextSharp.text.Rectangle.BOX;

            celda.BorderWidth = 0.8f;

            celda.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_LEFT;

            celda.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celda.PaddingTop = 4f;
            celda.PaddingBottom = 4f;
            celda.PaddingLeft = 4f;
            celda.PaddingRight = 4f;

            celda.BackgroundColor =
                new iTextSharp.text.BaseColor(
                    245,
                    245,
                    245);

            return celda;
        }

        private PdfPCell CrearCeldaFormatoValor(
            string texto,
            iTextSharp.text.Font fuente)
        {
            PdfPCell celda =
                new PdfPCell(
                    new iTextSharp.text.Phrase(
                        texto,
                        fuente));

            celda.Border =
                iTextSharp.text.Rectangle.BOX;

            celda.BorderWidth = 0.8f;

            celda.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_LEFT;

            celda.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celda.PaddingTop = 4f;
            celda.PaddingBottom = 4f;
            celda.PaddingLeft = 5f;
            celda.PaddingRight = 5f;

            return celda;
        }

        private PdfPCell CrearCeldaFormatoEncabezado(
            string texto,
            iTextSharp.text.Font fuente)
        {
            PdfPCell celda =
                new PdfPCell(
                    new iTextSharp.text.Phrase(
                        texto,
                        fuente));

            celda.Border =
                iTextSharp.text.Rectangle.BOX;

            celda.BorderWidth = 0.8f;

            celda.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            celda.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celda.PaddingTop = 5f;
            celda.PaddingBottom = 5f;
            celda.PaddingLeft = 3f;
            celda.PaddingRight = 3f;

            celda.BackgroundColor =
                new iTextSharp.text.BaseColor(
                    235,
                    235,
                    235);

            return celda;
        }

        private iTextSharp.text.pdf.PdfPTable CrearLineaCorte()
        {
            PdfPTable tabla =
                new PdfPTable(1);

            tabla.WidthPercentage = 100f;
            tabla.SpacingBefore = 7f;
            tabla.SpacingAfter = 7f;

            iTextSharp.text.Font fuente =
                iTextSharp.text.FontFactory.GetFont(
                    iTextSharp.text.FontFactory.HELVETICA,
                    7f,
                    new iTextSharp.text.BaseColor(
                        100,
                        100,
                        100));

            PdfPCell celda =
                new PdfPCell(
                    new iTextSharp.text.Phrase(
                        "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -   CORTAR AQUI   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -",
                        fuente));

            celda.Border =
                iTextSharp.text.Rectangle.NO_BORDER;

            celda.HorizontalAlignment =
                iTextSharp.text.Element.ALIGN_CENTER;

            celda.VerticalAlignment =
                iTextSharp.text.Element.ALIGN_MIDDLE;

            celda.PaddingTop = 2f;
            celda.PaddingBottom = 2f;

            tabla.AddCell(
                celda);

            return tabla;
        }

        private void AgregarEncabezadoPdf(PdfDocument documento, string titulo, string facturas)
        {
            iTextSharp.text.BaseColor azulCorporativo = new iTextSharp.text.BaseColor(31, 78, 121);
            iTextSharp.text.BaseColor grisTexto = new iTextSharp.text.BaseColor(80, 80, 80);
            iTextSharp.text.Font fuenteTitulo = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 18f, azulCorporativo);
            iTextSharp.text.Font fuenteSubtitulo = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9f, grisTexto);
            iTextSharp.text.Paragraph parrafoTitulo = new iTextSharp.text.Paragraph(titulo, fuenteTitulo);
            parrafoTitulo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            parrafoTitulo.SpacingAfter = 4f;
            documento.Add(parrafoTitulo);
            iTextSharp.text.Paragraph informacion = new iTextSharp.text.Paragraph("Factura(s): " + facturas + "    |    Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fuenteSubtitulo);
            informacion.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            informacion.SpacingAfter = 12f;
            documento.Add(informacion);
        }

        private PdfPCell CrearCeldaEncabezado(string texto, float tamañoFuente)
        {
            iTextSharp.text.BaseColor azulCorporativo = new iTextSharp.text.BaseColor(31, 78, 121);
            iTextSharp.text.Font fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, tamañoFuente, iTextSharp.text.BaseColor.WHITE);
            PdfPCell celda = new PdfPCell(new iTextSharp.text.Phrase(texto, fuente));
            celda.BackgroundColor = azulCorporativo;
            celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            celda.PaddingTop = 6f;
            celda.PaddingBottom = 6f;
            celda.PaddingLeft = 3f;
            celda.PaddingRight = 3f;
            celda.BorderColor = iTextSharp.text.BaseColor.WHITE;
            return celda;
        }

        private PdfPCell CrearCeldaDato(string texto, float tamañoFuente, int alineacion)
        {
            iTextSharp.text.Font fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, tamañoFuente, new iTextSharp.text.BaseColor(45, 45, 45));
            PdfPCell celda = new PdfPCell(new iTextSharp.text.Phrase(texto, fuente));
            celda.HorizontalAlignment = alineacion;
            celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            celda.Padding = 4f;
            celda.BorderColor = new iTextSharp.text.BaseColor(210, 215, 220);
            return celda;
        }

        private void AgregarPiePdf(PdfDocument documento)
        {
            iTextSharp.text.Font fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_OBLIQUE, 7f, new iTextSharp.text.BaseColor(110, 110, 110));
            iTextSharp.text.Paragraph pie = new iTextSharp.text.Paragraph("Documento generado por el sistema Suministro de Mercancía.", fuente);
            pie.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            pie.SpacingBefore = 10f;
            documento.Add(pie);
        }

        private string ValorTexto(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return "";

            return valor.ToString().Trim();
        }

        private string ValorFecha(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return "";

            DateTime fecha;

            if (DateTime.TryParse(valor.ToString(), out fecha))
                return fecha.ToString("dd/MM/yyyy");

            return valor.ToString();
        }

        private string ValorCantidad(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return "0";

            decimal cantidad;

            if (decimal.TryParse(valor.ToString(), out cantidad))
                return cantidad.ToString("0.######");

            return valor.ToString();
        }

        private System.Drawing.Image CrearIconoPdf()
        {
            Bitmap imagen = new Bitmap(36, 36);

            using (Graphics grafico = Graphics.FromImage(imagen))
            {
                grafico.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                grafico.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                grafico.Clear(Color.Transparent);

                Rectangle cuerpo = new Rectangle(5, 2, 26, 32);

                using (System.Drawing.Drawing2D.GraphicsPath ruta = CrearRectanguloRedondeado(cuerpo, 4))
                using (SolidBrush fondo = new SolidBrush(Color.FromArgb(198, 40, 40)))
                {
                    grafico.FillPath(fondo, ruta);
                }

                Point[] doblez =
                {
                    new Point(21, 2),
                    new Point(31, 12),
                    new Point(21, 12)
                };

                using (SolidBrush fondoDoblez = new SolidBrush(Color.FromArgb(239, 83, 80)))
                {
                    grafico.FillPolygon(fondoDoblez, doblez);
                }

                using (Pen linea = new Pen(Color.FromArgb(255, 255, 255), 1.2f))
                {
                    grafico.DrawLine(linea, 9, 15, 27, 15);
                }

                using (Font fuente = new Font("Segoe UI", 7.5f, FontStyle.Bold, GraphicsUnit.Point))
                using (SolidBrush texto = new SolidBrush(Color.White))
                {
                    SizeF medida = grafico.MeasureString("PDF", fuente);
                    grafico.DrawString("PDF", fuente, texto, 18f - (medida.Width / 2f), 18f);
                }
            }

            return imagen;
        }

        private System.Drawing.Drawing2D.GraphicsPath CrearRectanguloRedondeado(Rectangle rectangulo, int radio)
        {
            System.Drawing.Drawing2D.GraphicsPath ruta = new System.Drawing.Drawing2D.GraphicsPath();
            int diametro = radio * 2;

            ruta.AddArc(rectangulo.X, rectangulo.Y, diametro, diametro, 180, 90);
            ruta.AddArc(rectangulo.Right - diametro, rectangulo.Y, diametro, diametro, 270, 90);
            ruta.AddArc(rectangulo.Right - diametro, rectangulo.Bottom - diametro, diametro, diametro, 0, 90);
            ruta.AddArc(rectangulo.X, rectangulo.Bottom - diametro, diametro, diametro, 90, 90);
            ruta.CloseFigure();

            return ruta;
        }

    }
}
