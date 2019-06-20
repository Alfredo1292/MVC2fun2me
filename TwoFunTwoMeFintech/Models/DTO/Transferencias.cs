namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Transferencias
    {
        public string Nombre { get; set; }
        public string Producto { get; set; }
        public decimal Monto_transferencia { get; set; }
        public string Descripcion { get; set; }
        public string Cuenta { get; set; }
        public string CuentaSinpe { get; set; }
        public string UsrModifica { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string FechaTransferencia { get; set; }
        public int IdBanco { get; set; }
        public int Cantidad { get; set; }
        public decimal TotalxBanco { get; set; }
    }
}