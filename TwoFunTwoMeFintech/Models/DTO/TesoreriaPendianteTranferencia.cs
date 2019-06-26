namespace TwoFunTwoMeFintech.Models.DTO
{
    public class TesoreriaPendianteTranferencia
    {
        public int Status { get; set; }
        public string tipo { get; set; }
        public string Filtro { get; set; }
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Producto { get; set; }
        public decimal Monto { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string Sinpe { get; set; }
        public string Estado { get; set; }
    }
}