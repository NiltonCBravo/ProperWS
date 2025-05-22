namespace proper_ws.Model
{
    public class UploadDetailRequest
    {
        public int SubidaLogId { get; set; }
        public int SubidaId { get; set; }
        public string Tabla { get; set; }
        public int SubidaExitosa { get; set; } = 0;
        public string Contenido { get; set; }
        public string Error { get; set; }
    }
}
