using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Suministro
{
    class clss_BD
    {
        public SqlConnection conexionDB_SQL;
        public String strConection;
        
        public SqlConnection GetConection_SQL(string BaseSAP)
        {
            // Obtiene y abre la conexión a la base de datos.
            try
            {
                strConection = "Data source=" + Properties.Settings.Default.SAP_Servidor +
                               ";Database=" + BaseSAP +
                               ";User ID=" + Properties.Settings.Default.SAP_Usuario +
                               ";Pwd=" + Properties.Settings.Default.SAP_Contrasena +
                               ";MultipleActiveResultSets=True;Connection Timeout=0";
               
                conexionDB_SQL = new SqlConnection(strConection);
                conexionDB_SQL.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Conexión fallida", MessageBoxButtons.OK);
                return null;
            }
            return conexionDB_SQL;
        }

        public void CloseConection_SQL(SqlCommand con)
        {
            // Cierra la conexión a la base de datos.
            if (con.Connection != null)
            {
                con.Connection.Close();
            }
        }      
    }
}
