using proper_ws.Model;
using proper_ws.Services;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Services;
namespace proper_ws.Ws
{
    /// <summary>
    /// Summary description for wscal
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WscalUsers : System.Web.Services.WebService
    {

        private readonly DownloadUserService _downloadUserService;
        
        public WscalUsers()
        {
            _downloadUserService = new DownloadUserService();
        }
        
        [WebMethod]
        public DataSet DameMS_Maestras(SimpleRequest request)
        {
            return _downloadUserService.GetMaestras(request);
        }
        
        /*[WebMethod]
        public DataSet DameMS_Access(SimpleRequest request)
        {
            return _downloadUserService.GetAccess(request);
        }*/
    }
}
