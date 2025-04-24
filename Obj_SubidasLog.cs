using System;
using System.Data;

public class Obj_SubidasLog : ObjPadre
{
    private cls_sp_SubidasLog da;

    #region "Atributos"

    private int _SubidaLogId;
    private int _SubidaId;
    private string _Tabla;
    private int _SubidaExitosa;
    private string _Contenido;
    private string _Error;
    private DataSet _DsMaestros;

    #endregion

    #region "Propiedades"

    public virtual DataTable prop_Dt
    {
        get { return this._Dt_; }
        set { this._Dt_ = value; }
    }

    public virtual DataSet prop_DsMaestros
    {
        get { return this._DsMaestros; }
        set { this._DsMaestros = value; }
    }
    public virtual int prop_SubidaLogId
    {
        get { return this._SubidaLogId; }
        set { this._SubidaLogId = value; }
    }
    public virtual int prop_SubidaId
    {
        get { return this._SubidaId; }
        set { this._SubidaId = value; }
    }
    public virtual string prop_Tabla
    {
        get { return this._Tabla; }
        set { this._Tabla = value; }
    }
    public virtual int prop_SubidaExitosa
    {
        get { return this._SubidaExitosa; }
        set { this._SubidaExitosa = value; }
    }
    public virtual string prop_Contenido
    {
        get { return this._Contenido; }
        set { this._Contenido = value; }
    }
    public virtual string prop_Error
    {
        get { return this._Error; }
        set { this._Error = value; }
    }
    #endregion
    #region "Metodos"

    public new bool ValidarDatos(TipoAccionMensaje v)
    {
        bool functionReturnValue = false;
        switch (v)
        {
            case TipoAccionMensaje.AccionNuevo:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.AccionModificar:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.AccionEliminar:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.Buscar:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.GrabarEliminar:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.GrabarModificar:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.GrabarNuevo:
                functionReturnValue = true;
                break;
            case TipoAccionMensaje.Llenar:
                functionReturnValue = true;
                break;
        }
        return functionReturnValue;
    }

    public void Llenar()
    {
        ClsResultadoBD _Prop_clsResultadoBd = new ObjPadre.ClsResultadoBD();
        if (_SubidaLogId == 0)
        {
            InicializarObjeto();
        }
        else
        {
            _Prop_clsResultadoBd.Llenar(false, "", null);
            try
            {
                DataRow[] dr = _Dt_.Select("SubidaLogId='" + Convert.ToString(_SubidaLogId) + "'");
                if (dr != null)
                {
                    _SubidaLogId = DBNull.Value.Equals(dr[0]["SubidaLogId"]) ? 0 : Convert.ToInt32(dr[0]["SubidaLogId"]);
                    _SubidaId = DBNull.Value.Equals(dr[0]["SubidaId"]) ? 0 : Convert.ToInt32(dr[0]["SubidaId"]);
                    _Tabla = DBNull.Value.Equals(dr[0]["Tabla"]) ? "" : Convert.ToString(dr[0]["Tabla"]);
                    _SubidaExitosa = DBNull.Value.Equals(dr[0]["SubidaExitosa"]) ? 0 : Convert.ToInt16(dr[0]["SubidaExitosa"]);
                    _Contenido = DBNull.Value.Equals(dr[0]["Contenido"]) ? "" : Convert.ToString(dr[0]["Contenido"]);
                    _Error = DBNull.Value.Equals(dr[0]["Error"]) ? "" : Convert.ToString(dr[0]["Error"]);
                }
                else
                {
                    _Prop_clsResultadoBd.Llenar(true, "No se encontró el item solicitado. ", null);
                }
            }
            catch (Exception ex)
            {
                _Prop_clsResultadoBd.Llenar(true, "Error. No se encontró el item solicitado. " + ex.Message, null);
            }
        }
    }

    public void Main()
    {
        InicializarObjeto();
        //da = new cls_sp_SubidasLog();
    }
    public ClsResultadoBD BuscarObjyLlenar()
    {
        Sel_();
        Llenar();
        return Prop_clsResultadoBd;
    }

    public void InicializarObjeto()
    {
        _SubidaLogId = 0;
        _SubidaId = 0;
        _Tabla = "";
        _SubidaExitosa = 0;
        _Contenido = "";
        _Error = "";
    }

    public ClsResultadoBD Insert_()
    {
        ClsResultadoBD _Prop_clsResultadoBd = new ObjPadre.ClsResultadoBD();
        if (ValidarDatos(TipoAccionMensaje.GrabarNuevo) == false)
        {
            _Prop_clsResultadoBd.Llenar(false, "", null);
        }
        try
        {
            da = new cls_sp_SubidasLog();
            DataTable dt = da.cls_sp_SubidasLog_INS(_SubidaLogId, _SubidaId, _Tabla, _SubidaExitosa, _Contenido, _Error);
            _Prop_clsResultadoBd.Llenar(false, "Inserción exitosa", dt.Rows[0]["ID"].ToString());
        }
        catch (Exception ex)
        {
            _Prop_clsResultadoBd.Llenar(true, "Error en la inserción de datos. " + ex.Message, "0");
        }
        return _Prop_clsResultadoBd;
    }

    public ClsResultadoBD Update_()
    {
        ClsResultadoBD _Prop_clsResultadoBd = new ObjPadre.ClsResultadoBD();
        if (ValidarDatos(TipoAccionMensaje.GrabarNuevo) == false)
        {
            _Prop_clsResultadoBd.Llenar(false, "", null);
        }
        try
        {
            da = new cls_sp_SubidasLog();
            DataTable dt = da.cls_sp_SubidasLog_UPD(_SubidaLogId, _SubidaId, _Tabla, _SubidaExitosa, _Contenido, _Error);
            _Prop_clsResultadoBd.Llenar(false, "Actualización exitosa", dt.Rows[0]["ID"].ToString());
        }
        catch (Exception ex)
        {
            _Prop_clsResultadoBd.Llenar(true, "Error en la actualización de datos. " + ex.Message, "0");
        }
        return _Prop_clsResultadoBd;
    }

    public ClsResultadoBD Delete_()
    {
        ClsResultadoBD _Prop_clsResultadoBd = new ObjPadre.ClsResultadoBD();
        if (ValidarDatos(TipoAccionMensaje.GrabarEliminar) == false)
        {
            _Prop_clsResultadoBd.Llenar(false, "", null);
        }
        try
        {
            da = new cls_sp_SubidasLog();
            int n = da.cls_sp_SubidasLog_DEL(_SubidaLogId);
            if (n == 0)
            {
                _Prop_clsResultadoBd.Llenar(true, "Error en la eliminación de Datos", null);
            }
            else
            {
                _Prop_clsResultadoBd.Llenar(false, "Eliminación exitosa.", null);
            }
        }
        catch (Exception ex)
        {
            _Prop_clsResultadoBd.Llenar(true, "Error en la eliminación de Datos" + ex.Message, null);
        }
        return _Prop_clsResultadoBd;
    }

    public ClsResultadoBD INSXML(string S_XML)
    {
        ClsResultadoBD _Prop_clsResultadoBd = new ObjPadre.ClsResultadoBD();
        if (ValidarDatos(TipoAccionMensaje.GrabarEliminar) == false)
        {
            _Prop_clsResultadoBd.Llenar(false, "", null);
        }
        try
        {
            da = new cls_sp_SubidasLog();
            int n = da.cls_sp_SubidasLog_INSXML(S_XML);
            if (n == 0)
            {
                _Prop_clsResultadoBd.Llenar(true, "Error en la Inserción de Datos", null);
            }
            else
            {
                _Prop_clsResultadoBd.Llenar(false, "Insercion Exitosa", null);
            }
        }
        catch (Exception ex)
        {
            _Prop_clsResultadoBd.Llenar(true, "Error en la Inserción de Datos." + ex.Message, null);
        }
        return _Prop_clsResultadoBd;
    }

    public void Sel_()
    {
        da = new cls_sp_SubidasLog();
        _Dt_ = da.cls_sp_SubidasLog_SEL(_SubidaLogId);
    }
    public DataTable Sel_Vista()
    {
        da = new cls_sp_SubidasLog();
        return da.cls_sp_SubidasLog_SEL_VISTA(_SubidaLogId);
    }
    public void CargarMaestros()
    {
        da = new cls_sp_SubidasLog();
        _DsMaestros = da.cls_sp_SubidasLog_CARGARMAESTROS(_SubidaLogId);
    }

    #endregion

}
