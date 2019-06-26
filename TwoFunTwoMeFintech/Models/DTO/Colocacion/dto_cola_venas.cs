namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class dto_cola_ventas
    {
        public string cod_agente { set; get; }

        public int IdCola { set; get; }

        public int IdSolicitud { set; get; }
        public string Tipo { set; get; }
        public string SourceCode { set; get; }
        public bool IS_NEXT { set; get; }
    }
}