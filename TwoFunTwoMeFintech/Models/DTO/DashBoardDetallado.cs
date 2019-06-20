namespace TwoFunTwoMeFintech.Models.DTO
{
    public class DashBoardDetallado
    {
        public int OrdenAsignacion { set; get; }
        public int IdCredito { set; get; }
        public string Identificacion { set; get; }
        public int DiasMora { set; get; }
        public decimal TotalMora { set; get; }
        public string Procesado { set; get; }
        public string FechaUltimaPromesaPago { set; get; }
        public string FechaUltimaGestion { set; get; }
        public string Agenteasignado { set; get; }
    }
}