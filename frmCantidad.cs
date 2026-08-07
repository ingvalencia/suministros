using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Suministro
{
    public partial class frmCantidad : Form
    {
        public double cantidadR;
        public double cantidadF;
        public bool estado;

        public frmCantidad()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            estado = false;
            this.Close();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.mxt_cant.Text.Trim().Length > 0)
                {
                    if (cantidadF >= Convert.ToDouble(this.mxt_cant.Text.Trim()))
                    {
                        cantidadR = Convert.ToDouble(this.mxt_cant.Text.Trim());
                        estado = true;
                        this.Close();
                    }
                    else
                    {
                        estado = false;
                        MessageBox.Show("La cantidad ingresada excede a la facturada.", "Limite alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.mxt_cant.SelectAll();
                        this.mxt_cant.Focus();
                    }
                }
                else
                {
                    estado = false;
                    this.Close();
                }
            }
            catch
            {
                estado = false;
                this.Close();
            }
        }

        private void frmCantidad_Load(object sender, EventArgs e)
        {
            estado = false;
        }

        private void mxt_cant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '.' && !this.mxt_cant.Text.Contains('.'))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
                btn_ok_Click(sender, e);
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
