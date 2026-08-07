This file is a merged representation of a subset of the codebase, containing files not matching ignore patterns, combined into a single document by Repomix.

# File Summary

## Purpose
This file contains a packed representation of a subset of the repository's contents that is considered the most important context.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Repository files (if enabled)
5. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Files matching these patterns are excluded: bin/**, obj/**, *.user
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Files are sorted by Git change count (files with more changes are at the bottom)

# Directory Structure
```
Clases/
  clss_BD.cs
  clss_Funciones.cs
  clss_Query.cs
  clss_Static.cs
  DateTimeExtension.cs
  desktop.ini
Iconos/
  Busca.ico
  Cancela.ico
  desktop.ini
  Imprime.ico
  Ok.ico
  Principal.ico
  RS.ico
  Save.ico
Properties/
  AssemblyInfo.cs
  desktop.ini
  Resources.Designer.cs
  Resources.resx
  Settings.Designer.cs
  Settings.settings
app.config
desktop.ini
frmAutenticacion.cs
frmAutenticacion.Designer.cs
frmAutenticacion.resx
frmCantidad.cs
frmCantidad.Designer.cs
frmCantidad.resx
frmPrincipal.cs
frmPrincipal.Designer.cs
frmPrincipal.resx
Program.cs
Suministro.csproj
```

# Files

## File: Clases/clss_BD.cs
```csharp
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
```

## File: Clases/clss_Funciones.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;


    class clss_Funciones
    {
        public string CompletaCadena(string cadena, int tamaño, string caracter, char lado)
        {
            if (lado == 'D') // Complemento a la Derecha
            {
                if (cadena.Length < tamaño)
                {
                    return CompletaCadena(cadena + caracter, tamaño, caracter, lado);
                }
                else
                {
                    return cadena;
                }
            }
            else if (lado == 'I') // Complemento a la Izquierda
            {
                if (cadena.Length < tamaño)
                {
                    return CompletaCadena(caracter + cadena, tamaño, caracter, lado);
                }
                else
                {
                    return cadena;
                }
            }
            else
            {
                return cadena;
            }
        }       
    }
```

## File: Clases/clss_Query.cs
```csharp
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
```

## File: Clases/clss_Static.cs
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suministro
{
    class clss_Static
    {
        public static string TeclaPulsada;
        public static DateTime FechaTeclaPulsada;
        public static string TeclaFinal;

        public clss_Static() { }

        public string ObtieneTeclaPulsada()
        {
            return TeclaPulsada;
        }

        public void ColocaTeclaPulsada(string cad)
        {
            TeclaPulsada = cad;
        }

        public DateTime ObtieneFechaTeclaPulsada()
        {
            return FechaTeclaPulsada;
        }

        public void ColocaFechaTeclaPulsada(DateTime cad)
        {
            FechaTeclaPulsada = cad;
        }

        public string ObtieneTeclaFinal()
        {
            return TeclaFinal;
        }

        public void ColocaTeclaFinal(string cad)
        {
            TeclaFinal = cad;
        }
    }
}
```

## File: Clases/DateTimeExtension.cs
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Demo DateTime.DateDiff
/// </summary>
public class DateTimeExtension
{

    /// <summary>
    /// Devuelve un valor Long que especifica el número de
    /// intervalos de tiempo entre dos valores Date.
    /// </summary>
    /// <param name="interval">Obligatorio. Valor de enumeración
    /// DateInterval o expresión String que representa el intervalo
    /// de tiempo que se desea utilizar como unidad de diferencia
    /// entre Date1 y Date2.</param>
    /// <param name="date1">Obligatorio. Date. Primer valor de
    /// fecha u hora que se desea utilizar en el cálculo.</param>
    /// <param name="date2">Obligatorio. Date. Segundo valor de
    /// fecha u hora que se desea utilizar en el cálculo.</param>
    /// <returns></returns>
    public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
    {
        long rs = 0;
        TimeSpan diff = date2.Subtract(date1);
        switch (interval)
        {
            case DateInterval.Day:
            case DateInterval.DayOfYear:
                rs = (long)diff.TotalDays;
                break;
            case DateInterval.Hour:
                rs = (long)diff.TotalHours;
                break;
            case DateInterval.Minute:
                rs = (long)diff.TotalMinutes;
                break;
            case DateInterval.Month:
                rs = (date2.Month - date1.Month) + (12 * DateTimeExtension.DateDiff(DateInterval.Year, date1, date2));
                break;
            case DateInterval.Quarter:
                rs = (long)Math.Ceiling((double)(DateTimeExtension.DateDiff(DateInterval.Month, date1, date2) / 3.0));
                break;
            case DateInterval.Second:
                rs = (long)diff.TotalSeconds;
                break;
            case DateInterval.Milisecond:
                rs = (long)diff.TotalMilliseconds;
                break;
            case DateInterval.Weekday:
            case DateInterval.WeekOfYear:
                rs = (long)(diff.TotalDays / 7);
                break;
            case DateInterval.Year:
                rs = date2.Year - date1.Year;
                break;
        }//switch
        return rs;
    }//DateDiff
}

/// <summary>
/// Enumerados que definen los tipos de
/// intervalos de tiempo posibles.
/// </summary>
public enum DateInterval
{
    Day,
    DayOfYear,
    Hour,
    Minute,
    Month,
    Quarter,
    Second,
    Milisecond,
    Weekday,
    WeekOfYear,
    Year
}
```

## File: Clases/desktop.ini
```ini
[.ShellClassInfo]
InfoTip=Esta carpeta se ha compartido online.
IconFile=C:\Program Files (x86)\Google\Drive\googledrivesync.exe
IconIndex=16
```

## File: Iconos/desktop.ini
```ini
[.ShellClassInfo]
InfoTip=Esta carpeta se ha compartido online.
IconFile=C:\Program Files (x86)\Google\Drive\googledrivesync.exe
IconIndex=16
```

## File: Properties/AssemblyInfo.cs
```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// La información general sobre un ensamblado se controla mediante el siguiente 
// conjunto de atributos. Cambie estos atributos para modificar la información
// asociada con un ensamblado.
[assembly: AssemblyTitle("Suministro")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Suministro")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Si establece ComVisible como false, los tipos de este ensamblado no estarán visibles 
// para los componentes COM. Si necesita obtener acceso a un tipo de este ensamblado desde 
// COM, establezca el atributo ComVisible como true en este tipo.
[assembly: ComVisible(false)]

// El siguiente GUID sirve como identificador de typelib si este proyecto se expone a COM
[assembly: Guid("09c125a6-4b94-451b-92b0-d039065b9f33")]

// La información de versión de un ensamblado consta de los cuatro valores siguientes:
//
//      Versión principal
//      Versión secundaria 
//      Número de compilación
//      Revisión
//
// Puede especificar todos los valores o establecer como predeterminados los números de versión de compilación y de revisión 
// mediante el asterisco ('*'), como se muestra a continuación:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
```

## File: Properties/desktop.ini
```ini
[.ShellClassInfo]
InfoTip=Esta carpeta se ha compartido online.
IconFile=C:\Program Files (x86)\Google\Drive\googledrivesync.exe
IconIndex=16
```

## File: Properties/Resources.Designer.cs
```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.296
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Suministro.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso con establecimiento inflexible de tipos, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o vuelva a generar su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Suministro.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso con establecimiento inflexible de tipos.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Bitmap Busca {
            get {
                object obj = ResourceManager.GetObject("Busca", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap Cancela {
            get {
                object obj = ResourceManager.GetObject("Cancela", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap Imprime {
            get {
                object obj = ResourceManager.GetObject("Imprime", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap Ok {
            get {
                object obj = ResourceManager.GetObject("Ok", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap Save {
            get {
                object obj = ResourceManager.GetObject("Save", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
```

## File: Properties/Resources.resx
```
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <assembly alias="System.Windows.Forms" name="System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
  <data name="Busca" type="System.Resources.ResXFileRef, System.Windows.Forms">
    <value>..\Iconos\Busca.ico;System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</value>
  </data>
  <data name="Ok" type="System.Resources.ResXFileRef, System.Windows.Forms">
    <value>..\Iconos\Ok.ico;System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</value>
  </data>
  <data name="Imprime" type="System.Resources.ResXFileRef, System.Windows.Forms">
    <value>..\Iconos\Imprime.ico;System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</value>
  </data>
  <data name="Cancela" type="System.Resources.ResXFileRef, System.Windows.Forms">
    <value>..\Iconos\Cancela.ico;System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</value>
  </data>
  <data name="Save" type="System.Resources.ResXFileRef, System.Windows.Forms">
    <value>..\Iconos\Save.ico;System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</value>
  </data>
</root>
```

## File: Properties/Settings.Designer.cs
```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.18444
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Suministro.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.0.174")]
        public string SAP_Servidor {
            get {
                return ((string)(this["SAP_Servidor"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("sa")]
        public string SAP_Usuario {
            get {
                return ((string)(this["SAP_Usuario"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("P@ssw0rd")]
        public string SAP_Contrasena {
            get {
                return ((string)(this["SAP_Contrasena"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("VESER")]
        public string BaseSAP {
            get {
                return ((string)(this["BaseSAP"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OINV")]
        public string OINV {
            get {
                return ((string)(this["OINV"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OPOR")]
        public string OPOR {
            get {
                return ((string)(this["OPOR"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("INV1")]
        public string INV1 {
            get {
                return ((string)(this["INV1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("POR1")]
        public string POR1 {
            get {
                return ((string)(this["POR1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("BUFFER")]
        public string BaseRS {
            get {
                return ((string)(this["BaseRS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RS_RECEPCION_CAB")]
        public string RECEPCION_CAB {
            get {
                return ((string)(this["RECEPCION_CAB"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RS_SUMINISTRO_CAB")]
        public string SUMINISTRO_CAB {
            get {
                return ((string)(this["SUMINISTRO_CAB"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RS_CONFIRMACIONES")]
        public string CONFIRMACIONES {
            get {
                return ((string)(this["CONFIRMACIONES"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RS_RECEPCION_DET")]
        public string RECEPCION_DET {
            get {
                return ((string)(this["RECEPCION_DET"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RS_SUMINISTRO_DET")]
        public string SUMINISTRO_DET {
            get {
                return ((string)(this["SUMINISTRO_DET"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("T")]
        public string STS_TOTAL {
            get {
                return ((string)(this["STS_TOTAL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("P")]
        public string STS_PARCIAL {
            get {
                return ((string)(this["STS_PARCIAL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("R")]
        public string RECEPCION {
            get {
                return ((string)(this["RECEPCION"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("S")]
        public string SUMINISTRO {
            get {
                return ((string)(this["SUMINISTRO"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LDAP://grupodiniz.com.mx")]
        public string SERVER {
            get {
                return ((string)(this["SERVER"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("R")]
        public string STS_PRELI {
            get {
                return ((string)(this["STS_PRELI"]));
            }
        }
    }
}
```

## File: Properties/Settings.settings
```
<?xml version='1.0' encoding='utf-8'?>
<SettingsFile xmlns="http://schemas.microsoft.com/VisualStudio/2004/01/settings" CurrentProfile="(Default)" GeneratedClassNamespace="Suministro.Properties" GeneratedClassName="Settings">
  <Profiles />
  <Settings>
    <Setting Name="SAP_Servidor" Type="System.String" Scope="Application">
      <Value Profile="(Default)">192.168.0.174</Value>
    </Setting>
    <Setting Name="SAP_Usuario" Type="System.String" Scope="Application">
      <Value Profile="(Default)">sa</Value>
    </Setting>
    <Setting Name="SAP_Contrasena" Type="System.String" Scope="Application">
      <Value Profile="(Default)">P@ssw0rd</Value>
    </Setting>
    <Setting Name="BaseSAP" Type="System.String" Scope="Application">
      <Value Profile="(Default)">VESER</Value>
    </Setting>
    <Setting Name="OINV" Type="System.String" Scope="Application">
      <Value Profile="(Default)">OINV</Value>
    </Setting>
    <Setting Name="OPOR" Type="System.String" Scope="Application">
      <Value Profile="(Default)">OPOR</Value>
    </Setting>
    <Setting Name="INV1" Type="System.String" Scope="Application">
      <Value Profile="(Default)">INV1</Value>
    </Setting>
    <Setting Name="POR1" Type="System.String" Scope="Application">
      <Value Profile="(Default)">POR1</Value>
    </Setting>
    <Setting Name="BaseRS" Type="System.String" Scope="Application">
      <Value Profile="(Default)">BUFFER</Value>
    </Setting>
    <Setting Name="RECEPCION_CAB" Type="System.String" Scope="Application">
      <Value Profile="(Default)">RS_RECEPCION_CAB</Value>
    </Setting>
    <Setting Name="SUMINISTRO_CAB" Type="System.String" Scope="Application">
      <Value Profile="(Default)">RS_SUMINISTRO_CAB</Value>
    </Setting>
    <Setting Name="CONFIRMACIONES" Type="System.String" Scope="Application">
      <Value Profile="(Default)">RS_CONFIRMACIONES</Value>
    </Setting>
    <Setting Name="RECEPCION_DET" Type="System.String" Scope="Application">
      <Value Profile="(Default)">RS_RECEPCION_DET</Value>
    </Setting>
    <Setting Name="SUMINISTRO_DET" Type="System.String" Scope="Application">
      <Value Profile="(Default)">RS_SUMINISTRO_DET</Value>
    </Setting>
    <Setting Name="STS_TOTAL" Type="System.String" Scope="Application">
      <Value Profile="(Default)">T</Value>
    </Setting>
    <Setting Name="STS_PARCIAL" Type="System.String" Scope="Application">
      <Value Profile="(Default)">P</Value>
    </Setting>
    <Setting Name="RECEPCION" Type="System.String" Scope="Application">
      <Value Profile="(Default)">R</Value>
    </Setting>
    <Setting Name="SUMINISTRO" Type="System.String" Scope="Application">
      <Value Profile="(Default)">S</Value>
    </Setting>
    <Setting Name="SERVER" Type="System.String" Scope="Application">
      <Value Profile="(Default)">LDAP://grupodiniz.com.mx</Value>
    </Setting>
    <Setting Name="STS_PRELI" Type="System.String" Scope="Application">
      <Value Profile="(Default)">R</Value>
    </Setting>
  </Settings>
</SettingsFile>
```

## File: app.config
```
<?xml version="1.0"?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="Suministro.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false"/>
        </sectionGroup>
    </configSections>
    <applicationSettings>
      <Suministro.Properties.Settings>
        <setting name="SAP_Servidor" serializeAs="String">
          <value>192.168.0.174</value>
        </setting>
        <setting name="SAP_Usuario" serializeAs="String">
          <value>sa</value>
        </setting>
        <setting name="SAP_Contrasena" serializeAs="String">
          <value>P@ssw0rd</value>
        </setting>
        <setting name="BaseSAP" serializeAs="String">
          <value>VESER</value>
        </setting>
        <setting name="OINV" serializeAs="String">
          <value>OINV</value>
        </setting>
        <setting name="OPOR" serializeAs="String">
          <value>OPOR</value>
        </setting>
        <setting name="INV1" serializeAs="String">
          <value>INV1</value>
        </setting>
        <setting name="POR1" serializeAs="String">
          <value>POR1</value>
        </setting>
        <setting name="BaseRS" serializeAs="String">
          <value>BUFFER</value>
        </setting>
        <setting name="RECEPCION_CAB" serializeAs="String">
          <value>RS_RECEPCION_CAB</value>
        </setting>
        <setting name="SUMINISTRO_CAB" serializeAs="String">
          <value>RS_SUMINISTRO_CAB</value>
        </setting>
        <setting name="CONFIRMACIONES" serializeAs="String">
          <value>RS_CONFIRMACIONES</value>
        </setting>
        <setting name="RECEPCION_DET" serializeAs="String">
          <value>RS_RECEPCION_DET</value>
        </setting>
        <setting name="SUMINISTRO_DET" serializeAs="String">
          <value>RS_SUMINISTRO_DET</value>
        </setting>
        <setting name="STS_TOTAL" serializeAs="String">
          <value>T</value>
        </setting>
        <setting name="STS_PARCIAL" serializeAs="String">
          <value>P</value>
        </setting>
        <setting name="RECEPCION" serializeAs="String">
          <value>R</value>
        </setting>
        <setting name="SUMINISTRO" serializeAs="String">
          <value>S</value>
        </setting>
        <setting name="SERVER" serializeAs="String">
			<value>LDAP://192.168.0.199</value>
        </setting>
        <setting name="STS_PRELI" serializeAs="String">
          <value>R</value>
        </setting>
      </Suministro.Properties.Settings>
    </applicationSettings>
<startup><supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/></startup></configuration>
```

## File: desktop.ini
```ini
[.ShellClassInfo]
InfoTip=Esta carpeta se ha compartido online.
IconFile=C:\Program Files (x86)\Google\Drive\googledrivesync.exe
IconIndex=16
```

## File: frmAutenticacion.cs
```csharp
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
```

## File: frmAutenticacion.Designer.cs
```csharp
namespace Suministro
{
    partial class frmAutenticacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_si = new System.Windows.Forms.Button();
            this.btn_no = new System.Windows.Forms.Button();
            this.txt_usr = new System.Windows.Forms.TextBox();
            this.txt_pwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_si
            // 
            this.btn_si.Location = new System.Drawing.Point(33, 119);
            this.btn_si.Name = "btn_si";
            this.btn_si.Size = new System.Drawing.Size(75, 23);
            this.btn_si.TabIndex = 2;
            this.btn_si.Text = "Validar";
            this.btn_si.UseVisualStyleBackColor = true;
            this.btn_si.Click += new System.EventHandler(this.btn_si_Click);
            // 
            // btn_no
            // 
            this.btn_no.Location = new System.Drawing.Point(156, 121);
            this.btn_no.Name = "btn_no";
            this.btn_no.Size = new System.Drawing.Size(75, 23);
            this.btn_no.TabIndex = 3;
            this.btn_no.Text = "Cancelar";
            this.btn_no.UseVisualStyleBackColor = true;
            this.btn_no.Click += new System.EventHandler(this.btn_no_Click);
            // 
            // txt_usr
            // 
            this.txt_usr.Location = new System.Drawing.Point(33, 28);
            this.txt_usr.Name = "txt_usr";
            this.txt_usr.PasswordChar = '*';
            this.txt_usr.Size = new System.Drawing.Size(198, 20);
            this.txt_usr.TabIndex = 0;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Location = new System.Drawing.Point(33, 81);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.Size = new System.Drawing.Size(198, 20);
            this.txt_pwd.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Usuario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(30, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Contraseña";
            // 
            // frmAutenticacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(263, 156);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.txt_usr);
            this.Controls.Add(this.btn_no);
            this.Controls.Add(this.btn_si);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAutenticacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credenciales";
            this.Load += new System.EventHandler(this.frmAutenticacion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_si;
        private System.Windows.Forms.Button btn_no;
        private System.Windows.Forms.TextBox txt_usr;
        private System.Windows.Forms.TextBox txt_pwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
```

## File: frmAutenticacion.resx
```
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>
```

## File: frmCantidad.cs
```csharp
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
```

## File: frmCantidad.Designer.cs
```csharp
namespace Suministro
{
    partial class frmCantidad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.mxt_cant = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(22, 79);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(67, 25);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(130, 79);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(67, 25);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "Cancelar";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // mxt_cant
            // 
            this.mxt_cant.Location = new System.Drawing.Point(59, 26);
            this.mxt_cant.MaxLength = 10;
            this.mxt_cant.Name = "mxt_cant";
            this.mxt_cant.Size = new System.Drawing.Size(100, 20);
            this.mxt_cant.TabIndex = 0;
            this.mxt_cant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mxt_cant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mxt_cant_KeyPress);
            // 
            // frmCantidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(221, 110);
            this.Controls.Add(this.mxt_cant);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCantidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ingrese cantidad";
            this.Load += new System.EventHandler(this.frmCantidad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox mxt_cant;
    }
}
```

## File: frmCantidad.resx
```
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>
```

## File: frmPrincipal.cs
```csharp
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
        private int aYPos,R;
        private int margen = 186;
        private int ConteoParcial = 0;
        private int numReg = 57;
        private string KeysPressedFirst="";
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
                                      "FROM " + TablaH + " WHERE YEAR(DocDate)>=2017 AND U_SERIE = '" + NumFact.Replace("'","").Split(',')[0].Substring(0, 1) + "' " +
                                      "AND U_NUMDOC = '" + NumFact.Replace("'","").Split(',')[0].Substring(1) + "' ");
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

            for (int i = 0; i <= dtFact.Rows.Count-1; i++)
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
                    this.txt_prov.Text += dtFact.Rows[i][1].ToString() + "  -  " + dtFact.Rows[i][2].ToString().Replace(":","") + ":" + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
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
                this.txt_prov.Text += dtFact.Rows[i][1].ToString().Replace(char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10),"") + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);
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
            string Parcialidad="1";

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
                                         "','" + this.dgv_fact["Descripcion", row.Index].Value.ToString().Replace("'"," ") + "'," + this.dgv_fact["CantidadF", row.Index].Value +
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
                                  Func.CompletaCadena(string.Format("{0:f}",this.dgv_fact["CantidadF", x].Value), 12, " ", 'I') + "  " +
                                  Func.CompletaCadena(string.Format("{0:f}",this.dgv_fact["CantidadR", x].Value), 12, " ", 'I') + "  " +
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
            QryParcialidad.AsignaSQL("SELECT ISNULL(MAX(Parcialidad),0) FROM " + TablaCH + " WHERE NumFac='" + NumFact +"'");
            QryParcialidad.Execute_SC();

            if (QryParcialidad.ObtieneConsulta().ToString() == "")
            {
                valor = "0";
            }
            else
            {
                valor =  ((int)QryParcialidad.ObtieneConsulta() + 1).ToString();
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
                     cadena = cadena.Replace("'", "").Replace("#","");
                     
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
    }
}
```

## File: frmPrincipal.Designer.cs
```csharp
namespace Suministro
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txt_fact3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_fact2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbn_sal = new System.Windows.Forms.RadioButton();
            this.gpb_fact = new System.Windows.Forms.GroupBox();
            this.txt_fechafin = new System.Windows.Forms.TextBox();
            this.txt_fechaini = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_tot = new System.Windows.Forms.TextBox();
            this.txt_prov = new System.Windows.Forms.TextBox();
            this.txt_imp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_sub = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_fact = new System.Windows.Forms.Label();
            this.txt_fact1 = new System.Windows.Forms.TextBox();
            this.btn_busq = new System.Windows.Forms.Button();
            this.txtCodeBar = new System.Windows.Forms.TextBox();
            this.txt_temp = new System.Windows.Forms.TextBox();
            this.btn_imprimir = new System.Windows.Forms.Button();
            this.btn_cancelar = new System.Windows.Forms.Button();
            this.btn_confirmar = new System.Windows.Forms.Button();
            this.dgv_fact = new System.Windows.Forms.DataGridView();
            this.Factura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Documento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoPaq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoBar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoArt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Proyecto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnidadMed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsl_estatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tmr_tiempo = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gpb_fact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fact)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainer1.Panel1.Controls.Add(this.txt_fact3);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.txt_fact2);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.gpb_fact);
            this.splitContainer1.Panel1.Controls.Add(this.lbl_fact);
            this.splitContainer1.Panel1.Controls.Add(this.txt_fact1);
            this.splitContainer1.Panel1.Controls.Add(this.btn_busq);
            this.splitContainer1.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel1_MouseMove);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainer1.Panel2.Controls.Add(this.txtCodeBar);
            this.splitContainer1.Panel2.Controls.Add(this.txt_temp);
            this.splitContainer1.Panel2.Controls.Add(this.btn_imprimir);
            this.splitContainer1.Panel2.Controls.Add(this.btn_cancelar);
            this.splitContainer1.Panel2.Controls.Add(this.btn_confirmar);
            this.splitContainer1.Panel2.Controls.Add(this.dgv_fact);
            this.splitContainer1.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel2_MouseMove);
            this.splitContainer1.Size = new System.Drawing.Size(1188, 554);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 0;
            // 
            // txt_fact3
            // 
            this.txt_fact3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_fact3.BackColor = System.Drawing.Color.White;
            this.txt_fact3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_fact3.Location = new System.Drawing.Point(966, 16);
            this.txt_fact3.MaxLength = 10;
            this.txt_fact3.Name = "txt_fact3";
            this.txt_fact3.Size = new System.Drawing.Size(94, 20);
            this.txt_fact3.TabIndex = 2;
            this.txt_fact3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fact3.TextChanged += new System.EventHandler(this.txt_fact3_TextChanged);
            this.txt_fact3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fact3_KeyPress);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(950, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "-";
            // 
            // txt_fact2
            // 
            this.txt_fact2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_fact2.BackColor = System.Drawing.Color.White;
            this.txt_fact2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_fact2.Location = new System.Drawing.Point(850, 16);
            this.txt_fact2.MaxLength = 10;
            this.txt_fact2.Name = "txt_fact2";
            this.txt_fact2.Size = new System.Drawing.Size(94, 20);
            this.txt_fact2.TabIndex = 1;
            this.txt_fact2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fact2.TextChanged += new System.EventHandler(this.txt_fact2_TextChanged);
            this.txt_fact2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fact2_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(834, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "-";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.PowderBlue;
            this.groupBox1.Controls.Add(this.rbn_sal);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 46);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // rbn_sal
            // 
            this.rbn_sal.AutoSize = true;
            this.rbn_sal.Location = new System.Drawing.Point(9, 17);
            this.rbn_sal.Name = "rbn_sal";
            this.rbn_sal.Size = new System.Drawing.Size(142, 17);
            this.rbn_sal.TabIndex = 1;
            this.rbn_sal.TabStop = true;
            this.rbn_sal.Text = "Suministro de mercancía";
            this.rbn_sal.UseVisualStyleBackColor = true;
            this.rbn_sal.CheckedChanged += new System.EventHandler(this.rbn_sal_CheckedChanged);
            // 
            // gpb_fact
            // 
            this.gpb_fact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpb_fact.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gpb_fact.Controls.Add(this.txt_fechafin);
            this.gpb_fact.Controls.Add(this.txt_fechaini);
            this.gpb_fact.Controls.Add(this.label1);
            this.gpb_fact.Controls.Add(this.txt_tot);
            this.gpb_fact.Controls.Add(this.txt_prov);
            this.gpb_fact.Controls.Add(this.txt_imp);
            this.gpb_fact.Controls.Add(this.label2);
            this.gpb_fact.Controls.Add(this.txt_sub);
            this.gpb_fact.Controls.Add(this.label3);
            this.gpb_fact.Controls.Add(this.label4);
            this.gpb_fact.Location = new System.Drawing.Point(12, 55);
            this.gpb_fact.Name = "gpb_fact";
            this.gpb_fact.Size = new System.Drawing.Size(1168, 99);
            this.gpb_fact.TabIndex = 15;
            this.gpb_fact.TabStop = false;
            this.gpb_fact.Text = "Datos de factura";
            // 
            // txt_fechafin
            // 
            this.txt_fechafin.Location = new System.Drawing.Point(776, 71);
            this.txt_fechafin.Name = "txt_fechafin";
            this.txt_fechafin.Size = new System.Drawing.Size(94, 20);
            this.txt_fechafin.TabIndex = 20;
            this.txt_fechafin.Visible = false;
            // 
            // txt_fechaini
            // 
            this.txt_fechaini.Location = new System.Drawing.Point(978, 71);
            this.txt_fechaini.Name = "txt_fechaini";
            this.txt_fechaini.Size = new System.Drawing.Size(94, 20);
            this.txt_fechaini.TabIndex = 19;
            this.txt_fechaini.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Socio de negocios:";
            // 
            // txt_tot
            // 
            this.txt_tot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_tot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_tot.Enabled = false;
            this.txt_tot.Location = new System.Drawing.Point(1043, 19);
            this.txt_tot.MaxLength = 50;
            this.txt_tot.Multiline = true;
            this.txt_tot.Name = "txt_tot";
            this.txt_tot.ReadOnly = true;
            this.txt_tot.Size = new System.Drawing.Size(114, 62);
            this.txt_tot.TabIndex = 14;
            this.txt_tot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_prov
            // 
            this.txt_prov.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_prov.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_prov.Enabled = false;
            this.txt_prov.Location = new System.Drawing.Point(106, 21);
            this.txt_prov.MaxLength = 100;
            this.txt_prov.Multiline = true;
            this.txt_prov.Name = "txt_prov";
            this.txt_prov.ReadOnly = true;
            this.txt_prov.Size = new System.Drawing.Size(521, 62);
            this.txt_prov.TabIndex = 11;
            // 
            // txt_imp
            // 
            this.txt_imp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_imp.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_imp.Enabled = false;
            this.txt_imp.Location = new System.Drawing.Point(876, 19);
            this.txt_imp.MaxLength = 50;
            this.txt_imp.Multiline = true;
            this.txt_imp.Name = "txt_imp";
            this.txt_imp.ReadOnly = true;
            this.txt_imp.Size = new System.Drawing.Size(114, 62);
            this.txt_imp.TabIndex = 13;
            this.txt_imp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(631, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Subtotal:";
            // 
            // txt_sub
            // 
            this.txt_sub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_sub.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_sub.Enabled = false;
            this.txt_sub.Location = new System.Drawing.Point(686, 19);
            this.txt_sub.MaxLength = 50;
            this.txt_sub.Multiline = true;
            this.txt_sub.Name = "txt_sub";
            this.txt_sub.ReadOnly = true;
            this.txt_sub.Size = new System.Drawing.Size(114, 62);
            this.txt_sub.TabIndex = 12;
            this.txt_sub.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(817, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Impuesto:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1003, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Total:";
            // 
            // lbl_fact
            // 
            this.lbl_fact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_fact.AutoSize = true;
            this.lbl_fact.Location = new System.Drawing.Point(648, 19);
            this.lbl_fact.Name = "lbl_fact";
            this.lbl_fact.Size = new System.Drawing.Size(80, 13);
            this.lbl_fact.TabIndex = 4;
            this.lbl_fact.Text = "No. Factura(s): ";
            // 
            // txt_fact1
            // 
            this.txt_fact1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_fact1.BackColor = System.Drawing.Color.White;
            this.txt_fact1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_fact1.Location = new System.Drawing.Point(734, 16);
            this.txt_fact1.MaxLength = 10;
            this.txt_fact1.Name = "txt_fact1";
            this.txt_fact1.Size = new System.Drawing.Size(94, 20);
            this.txt_fact1.TabIndex = 0;
            this.txt_fact1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fact1.TextChanged += new System.EventHandler(this.txt_fact1_TextChanged);
            this.txt_fact1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fact1_KeyPress);
            // 
            // btn_busq
            // 
            this.btn_busq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_busq.Image = global::Suministro.Properties.Resources.Busca;
            this.btn_busq.Location = new System.Drawing.Point(1077, 9);
            this.btn_busq.Name = "btn_busq";
            this.btn_busq.Size = new System.Drawing.Size(103, 40);
            this.btn_busq.TabIndex = 3;
            this.btn_busq.Text = "Buscar";
            this.btn_busq.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_busq.UseVisualStyleBackColor = true;
            this.btn_busq.Click += new System.EventHandler(this.btn_busq_Click);
            // 
            // txtCodeBar
            // 
            this.txtCodeBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCodeBar.Location = new System.Drawing.Point(177, 319);
            this.txtCodeBar.Name = "txtCodeBar";
            this.txtCodeBar.Size = new System.Drawing.Size(100, 20);
            this.txtCodeBar.TabIndex = 5;
            this.txtCodeBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodeBar_KeyPress);
            // 
            // txt_temp
            // 
            this.txt_temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_temp.BackColor = System.Drawing.Color.White;
            this.txt_temp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_temp.Location = new System.Drawing.Point(816, 319);
            this.txt_temp.MaxLength = 35;
            this.txt_temp.Name = "txt_temp";
            this.txt_temp.Size = new System.Drawing.Size(47, 20);
            this.txt_temp.TabIndex = 4;
            this.txt_temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_temp.Visible = false;
            this.txt_temp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_temp_KeyPress);
            this.txt_temp.MouseLeave += new System.EventHandler(this.txt_temp_MouseLeave);
            // 
            // btn_imprimir
            // 
            this.btn_imprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_imprimir.Enabled = false;
            this.btn_imprimir.Image = global::Suministro.Properties.Resources.Imprime;
            this.btn_imprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_imprimir.Location = new System.Drawing.Point(536, 319);
            this.btn_imprimir.Name = "btn_imprimir";
            this.btn_imprimir.Size = new System.Drawing.Size(103, 40);
            this.btn_imprimir.TabIndex = 3;
            this.btn_imprimir.Text = "Imprimir";
            this.btn_imprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_imprimir.UseVisualStyleBackColor = true;
            this.btn_imprimir.Click += new System.EventHandler(this.btn_imprimir_Click);
            // 
            // btn_cancelar
            // 
            this.btn_cancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancelar.Image = global::Suministro.Properties.Resources.Cancela;
            this.btn_cancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancelar.Location = new System.Drawing.Point(1085, 319);
            this.btn_cancelar.Name = "btn_cancelar";
            this.btn_cancelar.Size = new System.Drawing.Size(103, 40);
            this.btn_cancelar.TabIndex = 2;
            this.btn_cancelar.Text = "Cancelar";
            this.btn_cancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_cancelar.UseVisualStyleBackColor = true;
            this.btn_cancelar.Click += new System.EventHandler(this.btn_cancelar_Click);
            // 
            // btn_confirmar
            // 
            this.btn_confirmar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_confirmar.Enabled = false;
            this.btn_confirmar.Image = global::Suministro.Properties.Resources.Ok;
            this.btn_confirmar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_confirmar.Location = new System.Drawing.Point(12, 319);
            this.btn_confirmar.Name = "btn_confirmar";
            this.btn_confirmar.Size = new System.Drawing.Size(103, 40);
            this.btn_confirmar.TabIndex = 1;
            this.btn_confirmar.Text = "Confirma";
            this.btn_confirmar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_confirmar.UseVisualStyleBackColor = true;
            this.btn_confirmar.Click += new System.EventHandler(this.btn_confirmar_Click);
            // 
            // dgv_fact
            // 
            this.dgv_fact.AllowUserToAddRows = false;
            this.dgv_fact.AllowUserToDeleteRows = false;
            this.dgv_fact.AllowUserToResizeColumns = false;
            this.dgv_fact.AllowUserToResizeRows = false;
            this.dgv_fact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_fact.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_fact.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_fact.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Factura,
            this.Documento,
            this.CodigoPaq,
            this.CodigoBar,
            this.CodigoArt,
            this.Descripcion,
            this.CantidadF,
            this.CantidadR,
            this.Subtotal,
            this.Cuenta,
            this.Proyecto,
            this.Caja,
            this.Linea,
            this.UnidadMed});
            this.dgv_fact.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgv_fact.Location = new System.Drawing.Point(2, 3);
            this.dgv_fact.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_fact.MultiSelect = false;
            this.dgv_fact.Name = "dgv_fact";
            this.dgv_fact.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_fact.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_fact.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgv_fact.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_fact.Size = new System.Drawing.Size(1188, 311);
            this.dgv_fact.TabIndex = 0;
            this.dgv_fact.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_fact_CellClick);
            this.dgv_fact.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_fact_CellDoubleClick);
            this.dgv_fact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgv_fact_KeyPress);
            // 
            // Factura
            // 
            this.Factura.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Factura.DataPropertyName = "Factura";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Factura.DefaultCellStyle = dataGridViewCellStyle1;
            this.Factura.DividerWidth = 1;
            this.Factura.Frozen = true;
            this.Factura.HeaderText = "Factura";
            this.Factura.Name = "Factura";
            this.Factura.ReadOnly = true;
            this.Factura.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Factura.Width = 50;
            // 
            // Documento
            // 
            this.Documento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Documento.DataPropertyName = "Documento";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Documento.DefaultCellStyle = dataGridViewCellStyle2;
            this.Documento.DividerWidth = 1;
            this.Documento.Frozen = true;
            this.Documento.HeaderText = "Documento";
            this.Documento.Name = "Documento";
            this.Documento.ReadOnly = true;
            this.Documento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Documento.Width = 69;
            // 
            // CodigoPaq
            // 
            this.CodigoPaq.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CodigoPaq.DataPropertyName = "CodigoPaq";
            this.CodigoPaq.DividerWidth = 1;
            this.CodigoPaq.Frozen = true;
            this.CodigoPaq.HeaderText = "Código Paq.";
            this.CodigoPaq.Name = "CodigoPaq";
            this.CodigoPaq.ReadOnly = true;
            this.CodigoPaq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodigoPaq.Visible = false;
            // 
            // CodigoBar
            // 
            this.CodigoBar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CodigoBar.DataPropertyName = "CodigoBar";
            this.CodigoBar.DividerWidth = 1;
            this.CodigoBar.Frozen = true;
            this.CodigoBar.HeaderText = "Código Bar.";
            this.CodigoBar.Name = "CodigoBar";
            this.CodigoBar.ReadOnly = true;
            this.CodigoBar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CodigoBar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodigoBar.Width = 62;
            // 
            // CodigoArt
            // 
            this.CodigoArt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CodigoArt.DataPropertyName = "CodigoArt";
            this.CodigoArt.DividerWidth = 1;
            this.CodigoArt.Frozen = true;
            this.CodigoArt.HeaderText = "Codigo Art.";
            this.CodigoArt.Name = "CodigoArt";
            this.CodigoArt.ReadOnly = true;
            this.CodigoArt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodigoArt.Width = 60;
            // 
            // Descripcion
            // 
            this.Descripcion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Descripcion.DataPropertyName = "Descripcion";
            this.Descripcion.DividerWidth = 1;
            this.Descripcion.Frozen = true;
            this.Descripcion.HeaderText = "Descripción";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.ReadOnly = true;
            this.Descripcion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Descripcion.Width = 70;
            // 
            // CantidadF
            // 
            this.CantidadF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CantidadF.DataPropertyName = "CantidadF";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            this.CantidadF.DefaultCellStyle = dataGridViewCellStyle3;
            this.CantidadF.DividerWidth = 1;
            this.CantidadF.Frozen = true;
            this.CantidadF.HeaderText = "Cantidad Fact.";
            this.CantidadF.Name = "CantidadF";
            this.CantidadF.ReadOnly = true;
            this.CantidadF.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CantidadF.Width = 75;
            // 
            // CantidadR
            // 
            this.CantidadR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CantidadR.DataPropertyName = "CantidadR";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.Format = "N6";
            dataGridViewCellStyle4.NullValue = "0.000000";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CantidadR.DefaultCellStyle = dataGridViewCellStyle4;
            this.CantidadR.DividerWidth = 1;
            this.CantidadR.Frozen = true;
            this.CantidadR.HeaderText = "Cantidad Rec.";
            this.CantidadR.Name = "CantidadR";
            this.CantidadR.ReadOnly = true;
            this.CantidadR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CantidadR.Width = 74;
            // 
            // Subtotal
            // 
            this.Subtotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Subtotal.DataPropertyName = "Subtotal";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle5.NullValue = null;
            this.Subtotal.DefaultCellStyle = dataGridViewCellStyle5;
            this.Subtotal.DividerWidth = 1;
            this.Subtotal.HeaderText = "Subtotal";
            this.Subtotal.Name = "Subtotal";
            this.Subtotal.ReadOnly = true;
            this.Subtotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Subtotal.Visible = false;
            // 
            // Cuenta
            // 
            this.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Cuenta.DataPropertyName = "Cuenta";
            this.Cuenta.DividerWidth = 1;
            this.Cuenta.HeaderText = "Cuenta";
            this.Cuenta.Name = "Cuenta";
            this.Cuenta.ReadOnly = true;
            this.Cuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cuenta.Visible = false;
            // 
            // Proyecto
            // 
            this.Proyecto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Proyecto.DataPropertyName = "Proyecto";
            this.Proyecto.DividerWidth = 1;
            this.Proyecto.HeaderText = "Proyecto";
            this.Proyecto.Name = "Proyecto";
            this.Proyecto.ReadOnly = true;
            this.Proyecto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Proyecto.Visible = false;
            // 
            // Caja
            // 
            this.Caja.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Caja.DataPropertyName = "Caja";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle6.NullValue = null;
            this.Caja.DefaultCellStyle = dataGridViewCellStyle6;
            this.Caja.DividerWidth = 1;
            this.Caja.Frozen = true;
            this.Caja.HeaderText = "No. Caja";
            this.Caja.MaxInputLength = 35;
            this.Caja.Name = "Caja";
            this.Caja.ReadOnly = true;
            this.Caja.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Caja.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Linea
            // 
            this.Linea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Linea.DataPropertyName = "Linea";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Linea.DefaultCellStyle = dataGridViewCellStyle7;
            this.Linea.DividerWidth = 1;
            this.Linea.HeaderText = "Linea";
            this.Linea.MaxInputLength = 5;
            this.Linea.Name = "Linea";
            this.Linea.ReadOnly = true;
            this.Linea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Linea.Visible = false;
            // 
            // UnidadMed
            // 
            this.UnidadMed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UnidadMed.DataPropertyName = "UnidadMed";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.UnidadMed.DefaultCellStyle = dataGridViewCellStyle8;
            this.UnidadMed.DividerWidth = 1;
            this.UnidadMed.Frozen = true;
            this.UnidadMed.HeaderText = "Unidad Med./Emp.";
            this.UnidadMed.MaxInputLength = 15;
            this.UnidadMed.Name = "UnidadMed";
            this.UnidadMed.ReadOnly = true;
            this.UnidadMed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.UnidadMed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UnidadMed.Width = 150;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_estatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 532);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1188, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsl_estatus
            // 
            this.tsl_estatus.Name = "tsl_estatus";
            this.tsl_estatus.Size = new System.Drawing.Size(0, 17);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // tmr_tiempo
            // 
            this.tmr_tiempo.Interval = 350;
            this.tmr_tiempo.Tick += new System.EventHandler(this.tmr_tiempo_Tick);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1188, 554);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Suministro Mercancías v 1.2.8.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpb_fact.ResumeLayout(false);
            this.gpb_fact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fact)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton rbn_sal;
        private System.Windows.Forms.DataGridView dgv_fact;
        private System.Windows.Forms.TextBox txt_fact1;
        private System.Windows.Forms.Button btn_busq;
        private System.Windows.Forms.Label lbl_fact;
        private System.Windows.Forms.TextBox txt_tot;
        private System.Windows.Forms.TextBox txt_imp;
        private System.Windows.Forms.TextBox txt_sub;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_prov;
        private System.Windows.Forms.GroupBox gpb_fact;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsl_estatus;
        private System.Windows.Forms.Button btn_confirmar;
        private System.Windows.Forms.Button btn_cancelar;
        private System.Windows.Forms.Button btn_imprimir;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TextBox txt_fechafin;
        private System.Windows.Forms.TextBox txt_fechaini;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_temp;
        private System.Windows.Forms.Timer tmr_tiempo;
        private System.Windows.Forms.TextBox txt_fact3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_fact2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCodeBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Factura;
        private System.Windows.Forms.DataGridViewTextBoxColumn Documento;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoPaq;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoArt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadF;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadR;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Proyecto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caja;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linea;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidadMed;
    }
}
```

## File: frmPrincipal.resx
```
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <metadata name="Factura.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Documento.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="CodigoPaq.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="CodigoBar.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="CodigoArt.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Descripcion.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="CantidadF.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="CantidadR.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Subtotal.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Cuenta.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Proyecto.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Caja.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="Linea.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="UnidadMed.UserAddedColumn" type="System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>True</value>
  </metadata>
  <metadata name="statusStrip1.TrayLocation" type="System.Drawing.Point, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
    <value>17, 17</value>
  </metadata>
  <metadata name="printDocument1.TrayLocation" type="System.Drawing.Point, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
    <value>133, 17</value>
  </metadata>
  <metadata name="tmr_tiempo.TrayLocation" type="System.Drawing.Point, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
    <value>274, 17</value>
  </metadata>
  <metadata name="$this.TrayHeight" type="System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
    <value>50</value>
  </metadata>
  <assembly alias="System.Drawing" name="System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
  <data name="$this.Icon" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
    <value>
        AAABAAEAICAAAAEAIACoEAAAFgAAACgAAAAgAAAAQAAAAAEAIAAAAAAAgBAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABCAAAAcwAA
        AHMAAABzAAAAcwAAAHMAAABzAAAAcwAAAHMAAABzAAAAcwAAAHMAAABzAAAAcwAAAHMAAABzAAAAcwAA
        AHMAAABzAAAAcwAAAHMAAABzAAAAcwAAAHMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKEd/wCh
        Hf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wCh
        Hf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AAAAcwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAoR3/MdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxAChHf8AAABzAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAChHf8x1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEAKEd/wAAAHMAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAKEd/zHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQAoR3/AAAAcwAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAoR3/MdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxACh
        Hf8AAABzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAChHf8x1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEAKEd/wAAAHMAAAAAAAAAAAAAAEIAAABzAAAAcwAAAHMAAABzAKEd/zHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQAoR3/AAAAcwAAAAC0NwD/tDcA/7Q3AP+0NwD/tDcA/7Q3AP8AoR3/MdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxAChHf8AAABzAAAAALQ3AP+0NwDEtDcAxLQ3AMS0NwDEtDcAxACh
        Hf9L2mzHMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEAKEd/wAAAHMAAAAAtDcA/7Q3AMS0NwDEtDcAxLQ3
        AMS0NwDEAKEd/6Pnstdk3oHLMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQAoR3/AAAAcwAAAAC0NwD/tDcAxLQ3
        AMS0NwDEtDcAxLQ3AMQAoR3/x+7O4LPqvtqC4pjQSNppxzHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxAChHf8AAABzAAAAALQ3
        AP+0NwDEtDcAxLQ3AMS0NwDEtDcAxAChHf/Q8Nbkxu7O4LrsxN2f56/WcuCLzULZY8Yx1lbEMdZWxDHW
        VsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEAKEd/wAA
        AHMAAAAAtDcA/7Q3AMS0NwDEtDcAxLQ3AMS0NwDEAKEd/9fy3OfR8dflx+7P4b3sx92z6r/bmeaq1HLg
        i81K2mrHMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQAoR3/AAAAcwAAAAC0NwD/tDcAxLQ3AMS0NwDEtDcAxLQ3AMQAoR3/3fTh6tnz3ujT8djly+/S4sLt
        y9+668Pdsuq92qLnsdaC4pjQZN6Ay0XaZ8cx1lbEMdZWxDHWVsQx1lbEMdZWxDHWVsQx1lbEMdZWxDHW
        VsQx1lbEMdZWxAChHf8AAABzAAAAALQ3AP+0NwDEtDcAxLQ3AMS0NwDEtDcAxAChHf/h9eXs3/Xi69v0
        4OnW8tznz/DV48fuz+G/7Mjet+vC3LDqvNqp6LbYn+ev1orjntF14I7OZN6Ay1PbcslF2mfHONhdxTXX
        WcQx1lbENddZxDjYXcVC2WPGAKEd/wAAAHMAAAAAtDcA/7tOHMe0NwDEtDcAxLQ3AMS0NwDEAKEd/+P2
        5u3j9ubt4fXl7N/14uvb89/p1fLa5szv0+PE7szgvOzG3bbrwduw6rzaqei22KPnstef56/Wmuar1Zbl
        qNSV5abTj+Si0ozkodKL5J/RhuKb0YTimdAAoR3/AAAAcwAAAAC0NwD/05d708JjN8q0NwDEtDcAxLQ3
        AMQAoR3/5vfo7uX36O7l9uju5Pbn7eL25eze9OLq2PPd6NHx2OXK79Hiw+3L4L3sx92268Hcsuq92qvp
        uNil6LPXoOew1prmq9WW5ajUlOWl047kodKL5J/Rh+Od0QChHf8AAABzAAAAALQ3AP/euKfb2auW2MuB
        Xc+7ThzHtDcAxAChHf/o9+rv6Pfq7+f36e/m9+nv5ffo7uT25+3h9eXs3fTh6tfy3OfR8dfky+/S4sPt
        y+C/7MjeuuvD3LPqv9ut6brZp+i12KLnsdab5qzVl+Wo1JHlpNOO5KHSAKEd/wAAAHMAAAAAtDcA/+LD
        tt/fu6vd3bel29WghtXJeFHNAKEd/+r47PHp+Ozx6fjs8Oj46/Do9+rv5/fp7+b36O7j9ubt3/Xj69v0
        4OnW8tvn0fHX5Mzv0+LG7s7gwO3J3rvsxt2268Hcseq82qvpuNil6LPXoOew1pnmqtQAoR3/AAAAcwAA
        AAC0NwD/59DG4+TJveHhwrPe37ur3N22pNsAoR3/7Pnu9Ov57vPr+e3y6vjs8en47PHp+Ovw6Pfq7+b3
        6e/l9uju4vbm7N/14uvb89/p1vLc59Hx1+XM79Pjx+7P4cPty+C/7MjeuuvD3LTqv9uw6rzaq+m42ACh
        Hf8AAABCAAAAALQ3AP/r2dLo6dXM5ufPxOPkx7rg4cGz3gChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wCh
        Hf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wChHf8AoR3/AKEd/wCh
        Hf8AoR3/AKEd/wAAAAAAAAAAtDcA/+7g2uvt3dfq69nS6OnTyuXmzMLj48a54OHAst7fu6vd3rin2920
        otrarpnZ05p/1M2GZNDHckrNwWE0yr1RIci4Qw/GtTwFxLQ3AMS1PAXEuEMPxrpLGMe0NwD/AAAAcwAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC0NwD/8OPe7fDi3ezv4Nvr7d3W6evY0efo0snl5szB4uTH
        uuDhwrPe37ys3d65qNzdtaPb3LOg2tuxndnarpnZ2KqU2NinkNfWpIzW1qOK1tWghtXUnYHU05l91LQ3
        AP8AAABzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALQ3AP/y5uLu8ebh7vHl4O7w497s7uDa6+3d
        1unr2NHn6NPJ5ebMwuPkyLzh4sS33+HAst7fu6vd3bel29yzodrbsZ3Z2q2Y2NiqlNfXpo/X1qSL1tWh
        h9XUnoLVtDcA/wAAAHMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAtDcA//Po5e/z6OXv8ufj7/Lm
        4u/x5eDu8OPe7O7g2uvt3dbp69jR5+nUy+Xn0Mbk5szB4uTIvOHiw7bf4b+x3t+7q9zeuKfb3LOg2tqv
        mtnarJfY2KeQ19akjNa0NwD/AAAAcwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC0NwD/9Orn8fPq
        5vHz6ubw8+nl8PLo5O/y5+Lv8eXh7vDj3uzu4Nrr7d7Y6uzb1Ojr2NHn6dXM5ujRx+TmzMLi5cm+4ePG
        ueDhwbPe4L2u3d65qNzdtaPb27Ge2rQ3AP8AAABzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALQ3
        AP/17Ony9ezp8vTr6PL06+fx8+rm8fPp5vDy6OTv8ufi7/Hl4e7w497s7+Hc7O7f2ers3NXp69rT6OrX
        z+fp1czm6NHH5ObNw+Plyr/i5Me64OLDtd/hv7HetDcA/wAAAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAtDcA/7Q3AP+0NwD/tDcA/7Q3AP+0NwD/tDcA/7Q3AP+0NwD/tDcA/7Q3AP+0NwD/tDcA/7Q3
        AP+0NwD/tDcA/7Q3AP+0NwD/tDcA/7Q3AP+0NwD/tDcA/7Q3AP+0NwD/AAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAA////////////AAAA/gAAAP4AAAD+AAAA/gAAAP4AAAD+AAAAwAAAAIAA
        AACAAAAAgAAAAIAAAACAAAAAgAAAAIAAAACAAAAAgAAAAIAAAACAAAAAgAAAAIAAAACAAAABgAAAP4AA
        AD+AAAA/gAAAP4AAAD+AAAA/gAAAf/////8=
</value>
  </data>
</root>
```

## File: Program.cs
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Suministro
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPrincipal());
        }
    }
}
```

## File: Suministro.csproj
```
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">x86</Platform>
    <ProductVersion>8.0.30703</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{63A8603C-80FF-4E68-A0D9-2C716F46D8B1}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>Suministro</RootNamespace>
    <AssemblyName>Suministro</AssemblyName>
	  <TargetFrameworkVersion>v4.5.2</TargetFrameworkVersion>

	  <TargetFrameworkProfile>
    </TargetFrameworkProfile>
    <FileAlignment>512</FileAlignment>
    <PublishUrl>publish\</PublishUrl>
    <Install>true</Install>
    <InstallFrom>Disk</InstallFrom>
    <UpdateEnabled>false</UpdateEnabled>
    <UpdateMode>Foreground</UpdateMode>
    <UpdateInterval>7</UpdateInterval>
    <UpdateIntervalUnits>Days</UpdateIntervalUnits>
    <UpdatePeriodically>false</UpdatePeriodically>
    <UpdateRequired>false</UpdateRequired>
    <MapFileExtensions>true</MapFileExtensions>
    <ApplicationRevision>0</ApplicationRevision>
    <ApplicationVersion>1.0.0.%2a</ApplicationVersion>
    <IsWebBootstrapper>false</IsWebBootstrapper>
    <UseApplicationTrust>false</UseApplicationTrust>
    <BootstrapperEnabled>true</BootstrapperEnabled>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|x86' ">
    <PlatformTarget>x86</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|x86' ">
    <PlatformTarget>x86</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup>
    <StartupObject>Suministro.Program</StartupObject>
  </PropertyGroup>
  <PropertyGroup>
    <ApplicationIcon>Iconos\RS.ico</ApplicationIcon>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.DirectoryServices" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Deployment" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Clases\clss_BD.cs" />
    <Compile Include="Clases\clss_Query.cs" />
    <Compile Include="Clases\clss_Static.cs" />
    <Compile Include="Clases\clss_Funciones.cs" />
    <Compile Include="Clases\DateTimeExtension.cs" />
    <Compile Include="frmAutenticacion.cs">
      <SubType>Form</SubType>
    </Compile>
    <Compile Include="frmAutenticacion.Designer.cs">
      <DependentUpon>frmAutenticacion.cs</DependentUpon>
    </Compile>
    <Compile Include="frmPrincipal.cs">
      <SubType>Form</SubType>
    </Compile>
    <Compile Include="frmPrincipal.Designer.cs">
      <DependentUpon>frmPrincipal.cs</DependentUpon>
    </Compile>
    <Compile Include="frmCantidad.cs">
      <SubType>Form</SubType>
    </Compile>
    <Compile Include="frmCantidad.Designer.cs">
      <DependentUpon>frmCantidad.cs</DependentUpon>
    </Compile>
    <Compile Include="Program.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
    <EmbeddedResource Include="frmAutenticacion.resx">
      <DependentUpon>frmAutenticacion.cs</DependentUpon>
    </EmbeddedResource>
    <EmbeddedResource Include="frmCantidad.resx">
      <DependentUpon>frmCantidad.cs</DependentUpon>
    </EmbeddedResource>
    <EmbeddedResource Include="frmPrincipal.resx">
      <DependentUpon>frmPrincipal.cs</DependentUpon>
      <SubType>Designer</SubType>
    </EmbeddedResource>
    <EmbeddedResource Include="Properties\Resources.resx">
      <Generator>ResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.cs</LastGenOutput>
      <SubType>Designer</SubType>
    </EmbeddedResource>
    <Compile Include="Properties\Resources.Designer.cs">
      <AutoGen>True</AutoGen>
      <DependentUpon>Resources.resx</DependentUpon>
      <DesignTime>True</DesignTime>
    </Compile>
    <None Include="app.config" />
    <None Include="Properties\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.cs</LastGenOutput>
    </None>
    <Compile Include="Properties\Settings.Designer.cs">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <DesignTimeSharedInput>True</DesignTimeSharedInput>
    </Compile>
  </ItemGroup>
  <ItemGroup>
    <Content Include="Iconos\Imprime.ico" />
    <Content Include="Iconos\Busca.ico" />
    <Content Include="Iconos\Cancela.ico" />
    <Content Include="Iconos\Ok.ico" />
    <Content Include="Iconos\Principal.ico" />
    <Content Include="Iconos\RS.ico" />
    <Content Include="Iconos\Save.ico" />
  </ItemGroup>
  <ItemGroup>
    <BootstrapperPackage Include=".NETFramework,Version=v4.0">
      <Visible>False</Visible>
      <ProductName>Microsoft .NET Framework 4 %28x86 y x64%29</ProductName>
      <Install>true</Install>
    </BootstrapperPackage>
    <BootstrapperPackage Include="Microsoft.Net.Client.3.5">
      <Visible>False</Visible>
      <ProductName>.NET Framework 3.5 SP1 Client Profile</ProductName>
      <Install>false</Install>
    </BootstrapperPackage>
    <BootstrapperPackage Include="Microsoft.Net.Framework.3.5.SP1">
      <Visible>False</Visible>
      <ProductName>.NET Framework 3.5 SP1</ProductName>
      <Install>false</Install>
    </BootstrapperPackage>
    <BootstrapperPackage Include="Microsoft.Windows.Installer.3.1">
      <Visible>False</Visible>
      <ProductName>Windows Installer 3.1</ProductName>
      <Install>true</Install>
    </BootstrapperPackage>
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name="BeforeBuild">
  </Target>
  <Target Name="AfterBuild">
  </Target>
  -->
</Project>
```
