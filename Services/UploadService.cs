using proper_ws.Data;
using proper_ws.Interfaces;
using proper_ws.Model;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace proper_ws.Services
{
    public class UploadService: IUploadService
    {
        private readonly DB _db;

        public UploadService()
        {
            _db = new DB();
        }
        public MessageResponse UpladContenedorHora(UploadRequest uploadRequest)
        {
            return _db.Ejecutar_sp_Subidas_INS_Async("[comercial].[sp_Subidas_INS]", uploadRequest);
        }

        public MessageResponse UpladDataContainer(UploadRequest uploadRequest)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SubidaId", SqlDbType.Int)
                {
                    Value = uploadRequest.SubidaId
                },
                new SqlParameter("@UsuarioSubidaId", SqlDbType.VarChar, 50)
                {
                    Value = uploadRequest.UsuarioSubidaId
                },
                new SqlParameter("@DispositivoId", SqlDbType.SmallInt)
                {
                    Value = uploadRequest.DispositivoId
                },
                new SqlParameter("@FechaHoraRegistro", SqlDbType.SmallDateTime)
                {
                    Value = uploadRequest.FechaHoraRegistro
                },
                new SqlParameter("@SubidaExitosa", SqlDbType.Bit)
                {
                    Value = uploadRequest.SubidaExitosa
                },
                new SqlParameter("@UGUID", SqlDbType.VarChar, 100)
                {
                    Value = uploadRequest.UGUID
                }
            };

            return _db.ExecuteSpReturnMessageResponse("[comercial].[sp_Subidas_INS]", parameters);
        }
        public MessageResponse UpladDataDetail(UploadDetailRequest uploadRequest)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SubidaLogId", SqlDbType.Int)
                {
                    Value = uploadRequest.SubidaLogId
                },
                new SqlParameter("@SubidaId", SqlDbType.Int)
                {
                    Value = uploadRequest.SubidaId
                },
                new SqlParameter("@Tabla", SqlDbType.VarChar, 50)
                {
                    Value = uploadRequest.Tabla
                },
                new SqlParameter("@SubidaExitosa", SqlDbType.SmallInt)
                {
                    Value = uploadRequest.SubidaExitosa
                },
                new SqlParameter("@Error", SqlDbType.VarChar, 200)
                {
                    Value = uploadRequest.Error
                },
                new SqlParameter("@Contenido", SqlDbType.VarChar)
                {
                    Value = uploadRequest.Contenido
                }
            };
            return _db.ExecuteSpReturnMessageResponse("[comercial].[sp_SubidasDetail_INS]", parameters);
        }
        public MessageResponse ProcessData(SimpleRequest request)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SUBIDA_ID", SqlDbType.Int)
                {
                    Value = request.searchValue1
                }
            };
            return _db.ExecuteSpReturnMessageResponse("[comercial].[ds_INSXML_SQL_SQLLITE_CONTENEDOR]", parameters);
        }
        
        public MessageResponse ProcessDataUpdate(SimpleRequest request)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SUBIDA_ID", SqlDbType.Int)
                {
                    Value = request.searchValue1
                }
            };
            return _db.ExecuteSpReturnMessageResponse("[comercial].[ds_INSXML_SQL_SQLLITE_UPDATE_CONTENEDOR]", parameters);
        }
    }
}
