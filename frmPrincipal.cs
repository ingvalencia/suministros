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
            FechaIni = DateTime.Now.ToString().Replace(" p.m.", "").Replace(" a.m.", "").Replace(" p. m.", "").Replace(" a. m.", "");
            FechaFin = DateTime.Now.ToString().Replace(" p.m.", "").Replace(" a.m.", "").Replace(" p. m.", "").Replace(" a. m.", "");
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

            splitContainer1.SplitterDistance = 157;
        }

        private void btn_busq_Click(object sender, EventArgs e)
        {
            string t_facturas = "";
            int t_contFact = 0;

            if (TablaH == "" || (this.txt_fact1.Text.Trim() == "" && this.txt_fact2.Text.Trim() == "" && this.txt_fact3.Text.Trim() == ""))
            {
                MessageBox.Show("¡Seleccione el tipo de movimiento de mercancía!", "Movimiento", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //this.tsl_estatus.BackColor = Color.Red;
                //this.tsl_estatus.Text = "Error: Seleccione movimiento de mercancía.";
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.txt_fact1.Text.Trim() != "" && this.txt_fact1.Text.Trim().Length > 2)
                {
                    t_facturas += "'" + this.txt_fact1.Text + "'";
                    t_contFact += 1;
                }
                if (this.txt_fact2.Text.Trim() != "" && this.txt_fact2.Text.Trim().Length > 2)
                {
                    t_facturas += "'" + this.txt_fact2.Text + "'";
                    t_contFact += 1;
                }
                if (this.txt_fact3.Text.Trim() != "" && this.txt_fact3.Text.Trim().Length > 2)
                {
                    t_facturas += "'" + this.txt_fact3.Text + "'";
                    t_contFact += 1;
                }
                t_facturas = t_facturas.Replace("''", "','");

                if (ValidaFacturaConfirmada(t_facturas, t_contFact))
                {
                    estatusProceso = false;
                    this.btn_imprimir.Visible = true;
                    ConsultaFacturaConfirmada(t_facturas, t_contFact);
                }
                else
                {
                    if (t_flagEstadoFacturas)
                    {
                        estatusProceso = true;
                        this.btn_imprimir.Visible = false;
                        this.btn_pdfDetalle.Enabled = false;
                        this.btn_pdfResumen.Enabled = false;
                        DespliegaFactura(t_facturas, t_contFact);
                        GrabaPreliminar();
                    }
                    else
                    {
                        MessageBox.Show("¡No todas las facturas ingresadas están confirmadas!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //this.tsl_estatus.BackColor = Color.Red;
                        //this.tsl_estatus.Text = "Error: Sólo puede consultar documentos con el mismo estado.";
                    }
                }
                Cursor.Current = Cursors.Default;
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
            this.txt_prov.Text = "";
            this.txt_sub.Text = "";
            this.txt_imp.Text = "";
            this.txt_tot.Text = "";
            this.txt_fact1.Focus();
            this.gpb_fact.Visible = true;
            this.btn_confirmar.Enabled = false;
            this.btn_imprimir.Visible = false;
            this.rbn_sal.Checked = true;
            this.txt_prov.Enabled = false;
            this.txt_sub.Enabled = false;
            this.txt_imp.Enabled = false;
            this.txt_tot.Enabled = false;
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

        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            bool estatusConfirmar = true;
            bool estatusConfirmar2 = true;

            foreach (DataGridViewRow row in this.dgv_fact.Rows)
            {
                if (Convert.ToDouble(this.dgv_fact["CantidadF", row.Index].Value) != Convert.ToDouble(this.dgv_fact["CantidadR", row.Index].Value))
                {
                    MessageBox.Show("Linea No. " + this.dgv_fact.Rows[row.Index].HeaderCell.Value.ToString() + " : Cantidad incompleta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //this.tsl_estatus.Text = "Linea No. " + this.dgv_fact.Rows[row.Index].HeaderCell.Value.ToString() + " : Cantidad incompleta.";
                    //this.tsl_estatus.BackColor = Color.Red;
                    this.dgv_fact.Focus();
                    estatusConfirmar = false;
                    break;
                }
            }

            if (estatusConfirmar)
            {
                foreach (DataGridViewRow row in this.dgv_fact.Rows)
                {
                    try
                    {
                        if (this.dgv_fact["Caja", row.Index].Value.ToString() == "0" || this.dgv_fact["Caja", row.Index].Value.ToString().Trim() == "")
                        {
                            MessageBox.Show("Linea No. " + this.dgv_fact.Rows[row.Index].HeaderCell.Value.ToString() + " : No tiene número de caja asignada.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            //this.tsl_estatus.Text = "Linea No. " + this.dgv_fact.Rows[row.Index].HeaderCell.Value.ToString() + " : No tiene número de caja asignada.";
                            //this.tsl_estatus.BackColor = Color.Red;
                            this.dgv_fact.Focus();
                            estatusConfirmar2 = false;
                            break;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Número de caja inválido.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //this.tsl_estatus.Text = "Número de caja inválido.";
                        //this.tsl_estatus.BackColor = Color.Red;
                        this.dgv_fact.Focus();
                        estatusConfirmar2 = false;
                        break;
                    }
                }
                if (estatusConfirmar2)
                {
                    FechaFin = DateTime.Now.ToString().Replace(" p.m.", "").Replace(" a.m.", "").Replace(" p. m.", "").Replace(" a. m.", "");
                    this.txt_fechafin.Text = FechaFin;
                    GrabaConfirmacion();
                }
            }
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

        private void DespliegaFactura(string NumFact, int TotFact)
        {
            clss_Query QryFact = new clss_Query();
            clss_Query QryDetFact = new clss_Query();
            string documentos = "";

            QryFact.AsignaBase(Properties.Settings.Default.BaseSAP);
            //QryFact.AsignaSQL("SELECT DocNum,CardCode,CardName,DocTotal-VatSum,VatSum,DocTotal " +
            //                  "FROM " + TablaH + " WHERE U_SERIE = '" + NumFact.Substring(0, 1) +
            //                  "' AND U_NUMDOC = '" + NumFact.Substring(1) + "' ");
            switch (TotFact)
            {
                case 1:
                    QryFact.AsignaSQL("SELECT DocNum,CardCode,CardName,DocTotal-VatSum,VatSum,DocTotal " +
                                      "FROM " + TablaH + " WHERE YEAR(DocDate)>=2017 AND U_SERIE = '" + NumFact.Replace("'", "").Split(',')[0].Substring(0, 1) + "' " +
                                      "AND U_NUMDOC = '" + NumFact.Replace("'", "").Split(',')[0].Substring(1) + "' ");
                    break;
                case 2:
                    QryFact.AsignaSQL("SELECT DocNum,CardCode,CardName,DocTotal-VatSum,VatSum,DocTotal " +
                                      "FROM " + TablaH + " WHERE YEAR(DocDate)>=2017 AND (U_SERIE = '" + NumFact.Replace("'", "").Split(',')[0].Substring(0, 1) + "' " +
                                      "AND U_NUMDOC = '" + NumFact.Replace("'", "").Split(',')[0].Substring(1) + "') " +
                                      "OR (U_SERIE = '" + NumFact.Replace("'", "").Split(',')[1].Substring(0, 1) + "' " +
                                      "    AND U_NUMDOC = '" + NumFact.Replace("'", "").Split(',')[1].Substring(1) + "') ");
                    break;
                case 3:
                    QryFact.AsignaSQL("SELECT DocNum,CardCode,CardName,DocTotal-VatSum,VatSum,DocTotal " +
                                      "FROM " + TablaH + " WHERE YEAR(DocDate)>=2017 AND (U_SERIE = '" + NumFact.Replace("'", "").Split(',')[0].Substring(0, 1) + "' " +
                                      "AND U_NUMDOC = '" + NumFact.Replace("'", "").Split(',')[0].Substring(1) + "') " +
                                      "OR (U_SERIE = '" + NumFact.Replace("'", "").Split(',')[1].Substring(0, 1) + "' " +
                                      "    AND U_NUMDOC = '" + NumFact.Replace("'", "").Split(',')[1].Substring(1) + "') " +
                                      "OR (U_SERIE = '" + NumFact.Replace("'", "").Split(',')[2].Substring(0, 1) + "' " +
                                      "    AND U_NUMDOC = '" + NumFact.Replace("'", "").Split(',')[2].Substring(1) + "') ");
                    break;
                default:
                    break;
            }
            QryFact.Execute_DT();
            dtFact = QryFact.ObtieneTabla();

            for (int i = 0; i <= dtFact.Rows.Count - 1; i++)
            {
                documentos += "'" + dtFact.Rows[i][0].ToString() + "'";
            }
            documentos = documentos.Replace("''", "','");

            if (QryFact.ObtieneRegistros() > 0)
            {
                this.txt_prov.Text = "";
                this.txt_sub.Text = "";
                this.txt_imp.Text = "";
                this.txt_tot.Text = "";
                for (int i = 0; i <= dtFact.Rows.Count - 1; i++)
                {
                    this.txt_prov.Text += dtFact.Rows[i][1].ToString() + "  -  " + dtFact.Rows[i][2].ToString().Replace(":", "") + ":" + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                    this.txt_sub.Text += "$ " + string.Format("{0:00.00}", dtFact.Rows[i][3]) + ":" + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                    this.txt_imp.Text += "$ " + string.Format("{0:00.00}", dtFact.Rows[i][4]) + ":" + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                    this.txt_tot.Text += "$ " + string.Format("{0:00.00}", dtFact.Rows[i][5]) + ":" + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
                }
                QryDetFact.AsignaBase(Properties.Settings.Default.BaseSAP);
                QryDetFact.AsignaSQL("SELECT CAST(T0.U_Serie AS NVARCHAR(1))+CAST(T0.U_NumDoc AS NVARCHAR (10)) 'Factura',T0.DocNum 'Documento',ISNULL(T2.U_COD_BAR_PAQ,T1.CodeBars) 'CodigoPaq',T1.CodeBars 'CodigoBar',T1.ItemCode 'CodigoArt',T1.Dscription 'Descripcion', " +
                                     "T1.Quantity 'CantidadF', 0.000000 'CantidadR',T1.LineTotal 'Subtotal',T1.AcctCode 'Cuenta',T1.Project 'Proyecto', '0' 'Caja',T1.LineNum+1 'Linea', '' 'UnidadMed'  " +
                                     "FROM " + TablaH + " T0, " + TablaD + " T1, OITM T2 " +
                                     "WHERE T0.DocEntry = T1.DocEntry AND T2.ItemCode = T1.ItemCode AND T0.DocNum IN (" + documentos + ") ORDER BY T0.DocNum,T1.LineNum");
                QryDetFact.Execute_DT();
                dtDetFact = QryDetFact.ObtieneTabla();
                this.dgv_fact.DataSource = dtDetFact;
                //this.tsl_estatus.BackColor = Color.Green;
                //this.tsl_estatus.Text = "Consulta terminada.";
                MessageBox.Show("Consulta terminada.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.gpb_fact.Visible = true;

                //contador = 1;
                foreach (DataGridViewRow row in dgv_fact.Rows)
                {
                    if (dgv_fact.Rows[row.Index].Selected == true)
                    {
                        dgv_fact.Rows[row.Index].Selected = false;
                    }
                    dgv_fact.Rows[row.Index].HeaderCell.Value = this.dgv_fact["Linea", row.Index].Value.ToString();
                    //contador += 1;
                }

                FechaIni = DateTime.Now.ToString().Replace(" p.m.", "").Replace(" a.m.", "").Replace(" p. m.", "").Replace(" a. m.", "");
                this.btn_confirmar.Enabled = true;
                this.btn_imprimir.Enabled = true;
                this.btn_pdfDetalle.Enabled = true;
                this.btn_pdfResumen.Enabled = true;
                this.dgv_fact.Focus();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Documento(s) No. " + NumFact + " no encontrado(s). Verifique información.", "Documento", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //this.tsl_estatus.Text = "Documento(s) no encontrado(s).";
                //this.tsl_estatus.BackColor = Color.Red;
                LimpiaPantalla();
            }
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
            dtDetFact = QryDetFact.ObtieneTabla();
            this.dgv_fact.DataSource = dtDetFact;
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
            string Parcialidad = "1";

            clss_Query QryFactConf = new clss_Query();
            clss_Query QryDetFactConf = new clss_Query();
            string g_Factura = "";
            string t_Factura = "";
            string p_Factura = "";
            string g_Documento = "";
            int g_contador = 0;

            foreach (DataGridViewRow row in dgv_fact.Rows)
            {
                g_Factura = this.dgv_fact["Factura", row.Index].Value.ToString();
                g_Documento = this.dgv_fact["Documento", row.Index].Value.ToString();

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
                    QryFactConf.AsignaSQL("INSERT INTO " + Properties.Settings.Default.CONFIRMACIONES + " VALUES ('" + g_Factura + "','" + FechaIni +
                                           "','" + FechaFin + "','" + TablaCH + "','" + Properties.Settings.Default.STS_PRELI + "','','')");
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
            printDocument1.DocumentName = this.txt_fact1.Text;
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
            ImprimeCadena(Func.CompletaCadena("", 10, " ", 'D') + "Fecha de Impresión: " + String.Format("{0:g}", DateTime.Now) + Func.CompletaCadena("", 20, " ", 'D') + "Confirmó: " + ObtieneQuienAutorizo(this.txt_fact1.Text), e, 1);
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
                    this.txt_temp.Text = this.dgv_fact[e.ColumnIndex, e.RowIndex].Value.ToString();
                    this.txt_temp.Focus();
                    this.tmr_tiempo.Enabled = true;
                }
                else
                {
                    this.txt_temp.Visible = false;
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
            List<string> facturas = new List<string>();

            if (this.txt_fact1.Text.Trim() != "")
                facturas.Add("'" + this.txt_fact1.Text.Trim().Replace("'", "''") + "'");

            if (this.txt_fact2.Text.Trim() != "")
                facturas.Add("'" + this.txt_fact2.Text.Trim().Replace("'", "''") + "'");

            if (this.txt_fact3.Text.Trim() != "")
                facturas.Add("'" + this.txt_fact3.Text.Trim().Replace("'", "''") + "'");

            return string.Join(",", facturas.ToArray());
        }

        private string ObtenerNombreFacturasReporte()
        {
            List<string> facturas = new List<string>();

            if (this.txt_fact1.Text.Trim() != "")
                facturas.Add(this.txt_fact1.Text.Trim());

            if (this.txt_fact2.Text.Trim() != "")
                facturas.Add(this.txt_fact2.Text.Trim());

            if (this.txt_fact3.Text.Trim() != "")
                facturas.Add(this.txt_fact3.Text.Trim());

            return string.Join("_", facturas.ToArray());
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

                    string clienteAnterior = "";
                    string fechaInicioAnterior = "";
                    string fechaTerminaAnterior = "";
                    string facturaAnterior = "";
                    string documentoAnterior = "";
                    string empaqueAnterior = "";

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

                        bool nuevaFactura =
                            facturaActual != facturaAnterior ||
                            documentoActual != documentoAnterior;

                        bool nuevoEmpaque =
                            nuevaFactura ||
                            empaqueActual != empaqueAnterior;

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
                                nuevoEmpaque ? empaqueActual : "",
                                6f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevoEmpaque
                                    ? ValorTexto(fila["Nombre emp"])
                                    : "",
                                6f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevoEmpaque
                                    ? ValorTexto(fila["Observaciones 1"])
                                    : "",
                                6f,
                                iTextSharp.text.Element.ALIGN_LEFT));

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevoEmpaque
                                    ? ValorTexto(fila["Observaciones 2"])
                                    : "",
                                6f,
                                iTextSharp.text.Element.ALIGN_LEFT));

                        clienteAnterior = clienteActual;
                        fechaInicioAnterior = fechaInicioActual;
                        fechaTerminaAnterior = fechaTerminaActual;
                        facturaAnterior = facturaActual;
                        documentoAnterior = documentoActual;
                        empaqueAnterior = empaqueActual;
                    }

                    documento.Add(tabla);
                    AgregarPiePdf(documento);
                }
            }
        }

        private void GenerarPdfResumenEmbarque(DataTable datos, string rutaArchivo)
        {
            iTextSharp.text.Rectangle pagina = iTextSharp.text.PageSize.A4.Rotate();

            using (FileStream archivo = new FileStream(
                rutaArchivo,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                using (PdfDocument documento = new PdfDocument(
                    pagina,
                    32f,
                    32f,
                    36f,
                    30f))
                {
                    PdfWriter.GetInstance(documento, archivo);

                    documento.AddTitle("Resumen de Embarque");
                    documento.AddCreator("Suministro de mercancía");
                    documento.Open();

                    AgregarEncabezadoPdf(
                        documento,
                        "RESUMEN DE EMBARQUE",
                        ObtenerNombreFacturasReporte());

                    float[] anchos =
                    {
                26f,
                10f,
                10f,
                10f,
                14f,
                12f,
                18f
            };

                    PdfPTable tabla = new PdfPTable(anchos);
                    tabla.WidthPercentage = 100f;
                    tabla.HeaderRows = 1;
                    tabla.SpacingBefore = 8f;
                    tabla.SplitLate = false;
                    tabla.SplitRows = true;

                    string[] encabezados =
                    {
                "Cliente",
                "Fecha inicio",
                "Docto Fiscal",
                "Docto SAP",
                "Tipo y número de empaque",
                "Nombre empaque",
                "Observaciones 1"
            };

                    foreach (string encabezado in encabezados)
                    {
                        tabla.AddCell(
                            CrearCeldaEncabezado(encabezado, 8f));
                    }

                    string clienteAnterior = "";
                    string fechaInicioAnterior = "";
                    string facturaAnterior = "";
                    string documentoAnterior = "";

                    foreach (DataRow fila in datos.Rows)
                    {
                        string clienteActual =
                            ValorTexto(fila["Cliente"]);

                        string fechaInicioActual =
                            ValorFecha(fila["Fecha inicio"]);

                        string facturaActual =
                            ValorTexto(fila["Docto Fiscal"]);

                        string documentoActual =
                            ValorTexto(fila["Docto SAP"]);

                        bool nuevaFactura =
                            facturaActual != facturaAnterior ||
                            documentoActual != documentoAnterior;

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevaFactura ? clienteActual : "",
                                8f,
                                iTextSharp.text.Element.ALIGN_LEFT));

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevaFactura ? fechaInicioActual : "",
                                8f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevaFactura ? facturaActual : "",
                                8f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                nuevaFactura ? documentoActual : "",
                                8f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                ValorTexto(fila["Tipo y numero de empaque"]),
                                8f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                ValorTexto(fila["Nombre emp"]),
                                8f,
                                iTextSharp.text.Element.ALIGN_CENTER));

                        tabla.AddCell(
                            CrearCeldaDato(
                                ValorTexto(fila["Observaciones 1"]),
                                8f,
                                iTextSharp.text.Element.ALIGN_LEFT));

                        clienteAnterior = clienteActual;
                        fechaInicioAnterior = fechaInicioActual;
                        facturaAnterior = facturaActual;
                        documentoAnterior = documentoActual;
                    }

                    documento.Add(tabla);
                    AgregarPiePdf(documento);
                }
            }
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
