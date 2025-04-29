using proper_ws.Model;
using System.Data;
using System.Threading.Tasks;
namespace proper_ws.Interfaces
{
    public interface IDownloadUserService
    {
        DataSet GetMaestras(SimpleRequest request);
        //DataSet GetAccess(SimpleRequest request);
    }
}
