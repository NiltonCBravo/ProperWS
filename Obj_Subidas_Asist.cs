using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static ObjPadre;

namespace proper_ws
{
    public class Obj_Subidas_Asist : ObjPadre
    {
        private cls_sp_Subidas_Asist da;

        #region "Atributos"

        private int _SubidaId;
        private string _UsuarioSubidaId;
        private string _UGUID;
        private int _DispositivoId;
        private DateTime _FechaHoraRegistro;
        private int _SubidaExitosa;
        private DataSet _DsMaestros;

        #endregion

        #region "Propiedades"

        public virtual DataTable prop_Dt
        {
            get { return this._Dt_; }
            set { this._Dt_ = value; }
        }
        public virtual string prop_UGUID
        {
            get { return this._UGUID; }
            set { this._UGUID = value; }
        }
        public virtual DataSet prop_DsMaestros
        {
            get { return this._DsMaestros; }
            set { this._DsMaestros = value; }
        }
        public virtual int prop_SubidaId
        {
            get { return this._SubidaId; }
            set { this._SubidaId = value; }
        }
        public virtual string prop_UsuarioSubidaId
        {
            get { return this._UsuarioSubidaId; }
            set { this._UsuarioSubidaId = value; }
        }
        public virtual int prop_DispositivoId
        {
            get { return this._DispositivoId; }
            set { this._DispositivoId = value; }
        }
        public virtual DateTime prop_FechaHoraRegistro
        {
            get { return this._FechaHoraRegistro; }
            set { this._FechaHoraRegistro = value; }
        }
        public virtual int prop_SubidaExitosa
        {
            get { return this._SubidaExitosa; }
            set { this._SubidaExitosa = value; }
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
            if (_SubidaId == 0)
            {
                InicializarObjeto();
            }
            else
            {
                _Prop_clsResultadoBd.Llenar(false, "", null);
                try
                {
                    DataRow[] dr = _Dt_.Select("SubidaId='" + Convert.ToString(_SubidaId) + "'");
                    if (dr != null)
                    {
                        _SubidaId = DBNull.Value.Equals(dr[0]["SubidaId"]) ? 0 : Convert.ToInt32(dr[0]["SubidaId"]);
                        _UsuarioSubidaId = DBNull.Value.Equals(dr[0]["UsuarioSubidaId"]) ? "" : Convert.ToString(dr[0]["UsuarioSubidaId"]);
                        _DispositivoId = DBNull.Value.Equals(dr[0]["DispositivoId"]) ? 0 : Convert.ToInt32(dr[0]["DispositivoId"]);
                        _FechaHoraRegistro = DBNull.Value.Equals(dr[0]["FechaHoraRegistro"]) ? FechaNula : Convert.ToDateTime(dr[0]["FechaHoraRegistro"]);
                        _SubidaExitosa = DBNull.Value.Equals(dr[0]["SubidaExitosa"]) ? 0 : Convert.ToInt16(dr[0]["SubidaExitosa"]);
                        _UGUID = DBNull.Value.Equals(dr[0]["UGUID"]) ? "" : Convert.ToString(dr[0]["UGUID"]);
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
            //da = new cls_sp_Subidas();
        }
        public ClsResultadoBD BuscarObjyLlenar()
        {
            Sel_();
            Llenar();
            return Prop_clsResultadoBd;
        }

        public void InicializarObjeto()
        {
            _SubidaId = 0;
            _UsuarioSubidaId = "";
            _UGUID = "";
            _DispositivoId = 0;
            _FechaHoraRegistro = FechaNula;
            _SubidaExitosa = 0;
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
                da = new cls_sp_Subidas_Asist ();
                DataTable dt = da.cls_sp_Subidas_INS(_SubidaId, _UsuarioSubidaId, _DispositivoId, _FechaHoraRegistro, _SubidaExitosa, _UGUID);
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
                da = new cls_sp_Subidas_Asist();
                DataTable dt = da.cls_sp_Subidas_UPD(_SubidaId, _UsuarioSubidaId, _DispositivoId, _FechaHoraRegistro, _SubidaExitosa, _UGUID);
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
                da = new cls_sp_Subidas_Asist();
                int n = da.cls_sp_Subidas_DEL(_SubidaId);
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
                da = new cls_sp_Subidas_Asist();
                int n = da.cls_sp_Subidas_INSXML(S_XML);
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
            da = new cls_sp_Subidas_Asist();
            _Dt_ = da.cls_sp_Subidas_SEL(_SubidaId);
        }
        public DataTable Sel_Vista()
        {
            da = new cls_sp_Subidas_Asist();
            return da.cls_sp_Subidas_SEL_VISTA(_SubidaId);
        }
        public void CargarMaestros()
        {
            da = new cls_sp_Subidas_Asist();
            _DsMaestros = da.cls_sp_Subidas_CARGARMAESTROS(_SubidaId);
        }

        #endregion

        public DataTable ds_INSXML_SQL_SQLLITE(int SUBIDA_ID)
        {
            da = new cls_sp_Subidas_Asist();
            return da.ds_INSXML_SQL_SQLLITE(SUBIDA_ID);
        }

        public DataTable ds_INSXML_SQL_SQLLITE_Calidad(int SUBIDA_ID)
        {
            da = new cls_sp_Subidas_Asist();
            return da.ds_INSXML_SQL_SQLLITE_Calidad(SUBIDA_ID);
        }

    }
}