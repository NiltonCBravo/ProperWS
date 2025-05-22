using proper_ws.Model;
using System.Threading.Tasks;
namespace proper_ws.Interfaces
{
    public interface IUploadService
    {
        MessageResponse UpladContenedorHora(UploadRequest uploadRequest);
        MessageResponse UpladDataContainer(UploadRequest request);
        MessageResponse UpladDataDetail(UploadDetailRequest request);
        
        MessageResponse ProcessData(SimpleRequest request);
        MessageResponse ProcessDataUpdate(SimpleRequest request);
    }
}
