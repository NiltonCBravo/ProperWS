using System;
namespace proper_ws.Model
{
    public class ContenedorHora
    {
        public string FechaIngreso { get; set; }
        public string HoraIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string HoraSalida { get; set; }
        public int RefferID { get; set; }
        public int UsuarioAutorizadoID { get; set; }
        public string PrecintoIngreso { get; set; }
        public string Comentarios { get; set; }
        public int EstadoID { get; set; }
        public int CreadoPor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? ActualizadoPor { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public int? AprobadoPor { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string ComentarioSalida { get; set; }
        public string TablaOrigen { get; set; }
    }
}
