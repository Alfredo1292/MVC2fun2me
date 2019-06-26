namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class DetalleProducto
    {
        public int IdSolicitud { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IdMontoCredito { get; set; }
        public decimal MontoCredito { get; set; }
        public decimal Cuota { get; set; }
        public string DescripcionCredito { get; set; }
        public int IdPlazoCredito { get; set; }
        public int PlazoDias { get; set; }
        public string PlazoDescripcion { get; set; }
        public int IdFrecuencia { get; set; }
        public int FrecuenciaDias { get; set; }
        public string FrecuenciaDescripcion { get; set; }
    }
}