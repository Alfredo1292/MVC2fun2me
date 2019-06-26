namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class promesaPago
    {
        public int IdSolicitud { set; get; }
        public string FechaPago { set; get; }
        public int Cuota { set; get; }
        public string CalcularFechaSinFeriado { set; get; }
        public string MontoCuota { set; get; }
    }
}