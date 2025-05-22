using System;
using System.Data;
namespace proper_ws.Model
{
    public class UploadRequest
    {
        public int SubidaId { get; set; }
        public string UsuarioSubidaId { get; set; }
        public string UGUID { get; set; }
        public int DispositivoId { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public int SubidaExitosa { get; set; }
        public DataSet DsMaestros { get; set; }
    }
}
