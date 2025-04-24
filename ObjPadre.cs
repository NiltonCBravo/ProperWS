using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;


public class ObjPadre
{
    private System.Data.DataTable _dt;

    // Public _FechaNula As DateTime = "01/01/1900"
    // Public _FechaMaxima As DateTime = "31/12/2049"

    public DateTime _FechaMinimabusqueda = new DateTime(2014, 1, 1);
    public DateTime _FechaNula = new DateTime(1900, 1, 1);
    public DateTime _FechaMaxima = new DateTime(2049, 12, 31);

    private ClsResultadoBD _ClsResultadoBd;
    public string Public_NombreAplicacionMsgbox = "Proper";

    public void New()
    {
        Prop_clsResultadoBd = new ClsResultadoBD();
    }

    public class Foo_string_string
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
    }

    public class Foo_int_string
    {
        public int Column1 { get; set; }
        public string Column2 { get; set; }
    }

    public List<Foo_int_string> Dame_Lista_int_string(DataTable dt, string nombre_columna1, string nombre_columna2)
    {
        List<Foo_int_string> _items = new List<Foo_int_string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int c1 = Convert.ToInt32(dt.Rows[i][nombre_columna1].ToString());
            string c2 = dt.Rows[i][nombre_columna2].ToString();
            _items.Add(new Foo_int_string { Column1 = c1, Column2 = c2 });
        }
        return _items;
    }

    public List<Foo_string_string> Dame_Lista_string_string(DataTable dt, string nombre_columna1, string nombre_columna2)
    {
        List<Foo_string_string> _items = new List<Foo_string_string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string c1 = dt.Rows[i][nombre_columna1].ToString();
            string c2 = dt.Rows[i][nombre_columna2].ToString();
            _items.Add(new Foo_string_string { Column1 = c1, Column2 = c2 });
        }
        return _items;
    }
    public bool ValidarDatos(TipoAccionMensaje v)
    {
        bool _ValidarDatos = false;
        switch (v)
        {
            case TipoAccionMensaje.AccionNuevo:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.AccionModificar:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.AccionEliminar:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.Buscar:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.GrabarEliminar:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.GrabarModificar:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.GrabarNuevo:
                {
                    _ValidarDatos = true;
                    break;
                }

            case TipoAccionMensaje.Llenar:
                {
                    _ValidarDatos = true;
                    break;
                }
        }

        return _ValidarDatos;
    }

    //public void MesgBox(string sMessage)
    //{
    //    string msg;
    //    msg = "<script language='javascript'>";
    //    msg += "alert('" + sMessage + "');";
    //    msg += "<" + "/script>";

    //    return msg;
    //}

    public static DataTable ListToDataTable<T>(IList<T> data)
    {
        //--Install - Package FastMember

        DataTable table = new DataTable();
        //special handling for value types and string
        if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
        {
            DataColumn dc = new DataColumn("Value");
            table.Columns.Add(dc);
            foreach (T item in data)
            {
                DataRow dr = table.NewRow();
                dr[0] = item;
                table.Rows.Add(dr);
            }
        }
        else
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    catch (Exception)
                    {
                        //ex.Message 
                        row[prop.Name] = DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
        }
        return table;
    }
    public class ClsResultadoBD
    {
        private bool _Fallo;
        private string _Mensaje;
        // Private _IconodelMsg As MessageBoxIcon
        private object _Datos;
        private string _Id;

        // Public Overridable Property IconodelMsg() As MessageBoxIcon
        // Get
        // Return Me._IconodelMsg
        // End Get
        // Set(ByVal value As MessageBoxIcon)
        // Me._IconodelMsg = value
        // End Set
        // End Property

        public virtual string Mensaje
        {
            get
            {
                return this._Mensaje;
            }
            set
            {
                this._Mensaje = value;
            }
        }
        public virtual string Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
            }
        }
        public virtual bool Fallo
        {
            get
            {
                return this._Fallo;
            }
            set
            {
                this._Fallo = value;
            }
        }

        public virtual object Datos
        {
            get
            {
                return this._Datos;
            }
            set
            {
                this._Datos = value;
            }
        }

        public void Llenar(bool p_fallo, string p_mensaje = "", string p_id = "0", object p_datos = null)
        {
            this._Fallo = p_fallo;
            this._Mensaje = p_mensaje;
            this._Datos = p_datos;
            this.Id = p_id;
        }
    }

    public virtual ClsResultadoBD Prop_clsResultadoBd
    {
        get
        {
            return this._ClsResultadoBd;
        }
        set
        {
            this._ClsResultadoBd = value;
        }

    }
    public virtual System.Data.DataTable _Dt_
    {
        get
        {
            return this._dt;
        }
        set
        {
            this._dt = value;
        }
    }
    public virtual DateTime FechaNula
    {
        get
        {
            return this._FechaNula;
        }
        set
        {
            this._FechaNula = value;
        }
    }

    public enum TipoAccionMensaje
    {
        AccionNuevo,
        AccionModificar,
        AccionEliminar,
        Buscar,
        GrabarEliminar,
        GrabarModificar,
        GrabarNuevo,
        Llenar
    }
    public enum Maestros
    {
        Tanque = 0,
        TanqueUsuario = 1,
        Vehiculo = 2,
        VehiculoUsuario = 3,
        Usuario = 4,
        UsuarioTipoRol = 5
    }

    // Public Function SelectDataTable(ByVal dt As DataTable, ByVal filter As String) As DataTable
    // SelectDataTable = Nothing
    // Try
    // Dim rows As Data.DataRow()
    // Dim dtNew As Data.DataTable
    // dtNew = dt.Clone()
    // rows = dt.Select(filter)
    // For Each dr As Data.DataRow In rows
    // dtNew.ImportRow(dr)
    // Next
    // Return dtNew
    // Catch ex As Exception
    // End Try
    // End Function

    public DataTable SelectDataTable(DataTable dt, string filter, string sort = "")
    {
        DataRow[] rows;
        DataTable dtNew;
        dtNew = dt.Clone();
        if (sort == "")
            rows = dt.Select(filter);
        else
            rows = dt.Select(filter, sort);
        foreach (DataRow dr in rows)
            dtNew.ImportRow(dr);
        return dtNew;
    }

    public string DameXMLdeDatatable(System.Data.DataTable dt)
    {
        string _DameXMLdeDatatable = "";
        DataSet ds;
        ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        _DameXMLdeDatatable = ds.GetXml();
        return _DameXMLdeDatatable;
    }

    //public string DameEspaciosDecimales(string X, int numDec)
    //{
    //    string _DameEspaciosDecimales = "0.00";
    //    if (Information.IsNumeric(X))
    //    {
    //        double n = Math.Round(System.Convert.ToDouble(X), numDec);
    //        _DameEspaciosDecimales = string.Format("{0:N" + numDec.ToString() + "}", n);
    //    }
    //    else
    //        _DameEspaciosDecimales = "0.00";

    //    return _DameEspaciosDecimales;
    //}
}


