namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class CambioProductoCredito
    {
        public string Identificacion { set; get; }
        public int? IdSolicitud { set; get; }
        public string Nombre { set; get; }
        public string NombreProducto { set; get; }
        public decimal? MontoProducto { set; get; }
        public int? IdProducto { set; get; }
        public decimal? Cuota { set; get; }
        public string Frecuencia { set; get; }
        public decimal? MontoMaximo { set; get; }
        public int? PlazoDias { set; get; }
        public int? IdCredito { set; get; }
    }
}