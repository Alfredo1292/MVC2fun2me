namespace TwoFunTwoMeFintech.Models.DTO
{
    public class SolicitudActual
    {
        public int Status { set; get; }
        public int IdSolicitud { set; get; }
        public string IdProducto { set; get; }
        public string Identificacion { set; get; }
        public string DetalleStatus { set; get; }
        public string MontoMaximo { set; get; }
        public string MontoProducto { set; get; }
    }
}