using proper_ws.Data;
using proper_ws.Interfaces;
using proper_ws.Model;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace proper_ws.Services
{
    public class DownloadUserService: IDownloadUserService
    {
        private readonly DB _db;

        public DownloadUserService()
        {
            _db = new DB();
        }
        public DataSet GetMaestras(SimpleRequest request)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@PsearchOpt", SqlDbType.VarChar, 15) { Value = request.searchOpt },
                new SqlParameter("@PsearchValue", SqlDbType.VarChar, 15) { Value = request.searchValue1 },
                new SqlParameter("@PsearchValue2", SqlDbType.VarChar, 15) { Value = request.searchValue2 },
                new SqlParameter("@PsearchValue3", SqlDbType.VarChar, 15) { Value = request.searchValue3 },
            };
            return _db.RunSpReturnDataset("[planta].[sp_Maestras_Seguridad]", parametros);
        }
        /*public DataSet GetAccess(SimpleRequest request)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@PsearchOpt", SqlDbType.VarChar, 15) { Value = request.searchOpt },
                new SqlParameter("@PsearchValue", SqlDbType.VarChar, 15) { Value = request.searchValue1 },
                new SqlParameter("@PsearchValue2", SqlDbType.VarChar, 15) { Value = request.searchValue2 },
                new SqlParameter("@PsearchValue3", SqlDbType.VarChar, 15) { Value = request.searchValue3 },
            };
            return _db.RunSpReturnDataset("[planta].[sp_Access_List]", parametros);
        }*/
    }
}
