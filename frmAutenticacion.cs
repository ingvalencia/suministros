using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;

namespace Suministro
{
    public partial class frmAutenticacion : Form
    {
        public bool estado;
        public string usuario;

        public frmAutenticacion()
        {
            InitializeComponent();
        }

        private void frmAutenticacion_Load(object sender, EventArgs e)
        {
            estado = false;
        }

        private void btn_no_Click(object sender, EventArgs e)
        {
            estado = false;
            this.Close();
        }

        private void btn_si_Click(object sender, EventArgs e)
        {
            if (Autenticacion(this.txt_usr.Text, this.txt_pwd.Text))
            {
                estado = true;
                usuario = this.txt_usr.Text;
                this.Close();
            }
            else
            {
                estado = false;
                MessageBox.Show("Credenciales inválidas, acceso no autorizado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txt_usr.SelectAll();
                this.txt_usr.Focus();
            }
        }

        public bool Autenticacion(string usr, string pwd)
        {
            bool f_validacion = false;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string rutaLDAP = Properties.Settings.Default.SERVER;
                DirectoryEntry Directorio = new DirectoryEntry(rutaLDAP, usr, pwd);
                object Credencial = Directorio.NativeObject; // 

                f_validacion = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo de autenticación:\n" + ex.Message, "Error LDAP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            return f_validacion;
        }





    }
}
