using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Suministro
{
    class clss_Query
    {
        // Atributos
        private string SQL;
        private string Base;
        private bool TipoQuery;
        private DataTable Tabla;
        private int Registros;
        private object Consulta;
        
        // Constructores
        public clss_Query() { }
        public clss_Query(string t_sql, string t_base, bool t_tipo)
        {
            SQL = t_sql;
            Base = t_base;
            TipoQuery = t_tipo;
        }

        // Métodos
        public void AsignaSQL(string t_sql)
        {
            SQL = t_sql;
        }

        public void AsignaBase(string t_base)
        {
            Base = t_base;
        }

        public void AsignaTipoConsulta(bool t_tipo)
        {
            TipoQuery = t_tipo;
        }

        public DataTable ObtieneTabla()
        {
            return Tabla;
        }

        public int ObtieneRegistros()
        {
            return Registros;
        }

        public string ObtieneSQL()
        {
            return SQL;
        }

        public object ObtieneConsulta()
        {
            if (Consulta == null)
            {
                return "";
            }
            else
            {
                return Consulta;
            }
        }

        public void Execute_DT()
        {
            // Procedimiento de consulta para SQL Server
            // Ejecuta una consulta y guarda el resultado en un DT y el número de registros encontrados
            SqlCommand com;
            clss_BD db = new clss_BD();

            com = new SqlCommand();
            com.Connection = db.GetConection_SQL(Base);
            if (TipoQuery)
            {
               com.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                com.CommandType = CommandType.Text;
            }
            com.CommandText = SQL;
            com.CommandTimeout = 0;

            try 
            {
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                Tabla = dt;
                Registros = dt.Rows.Count;
            }
            catch (Exception e)
            {
                Registros = 0;
                Consulta = "";
                MessageBox.Show("Ejecución fallida (DT): " + SQL + ". " + e.Message, "Advertencia", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            finally
            {
                db.CloseConection_SQL(com);
            }
        }

        public void Execute_SC()
        {
            // Procedimiento de consulta para SQL Server
            // Ejecuta una transacción y guarda el primer valor obtenido de la consulta
            SqlCommand com;
            clss_BD db = new clss_BD();
            
            com = new SqlCommand();
            com.Connection = db.GetConection_SQL(Base);
            if (TipoQuery)
            {
                com.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                com.CommandType = CommandType.Text;
            }
            com.CommandText = SQL;
            com.CommandTimeout = 0;

            try 
            {
                Consulta = com.ExecuteScalar();
            }
            catch (Exception e)
            {
                Registros = 0;
                Consulta = "";
                MessageBox.Show("Ejecución fallida (SC): " + SQL + ". " + e.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                db.CloseConection_SQL(com);
            }    
        }

        public void Execute_IDU()
        {
            // Procedimiento de transacciones para SQL Server
            // Ejecuta una transacción y guarda el número de registros afectados
            SqlCommand com;
            clss_BD db = new clss_BD();
            
            com = new SqlCommand();
            com.Connection = db.GetConection_SQL(Base);
            if (TipoQuery)
            {
                com.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                com.CommandType = CommandType.Text;
            }
            com.CommandText = SQL;
            com.CommandTimeout = 0;

            try 
            {
                Registros = com.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Registros = 0;
                Consulta = "";
                MessageBox.Show("Ejecución fallida (UID): " + SQL + ". " + e.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                db.CloseConection_SQL(com);
            }    
        }
    }
}