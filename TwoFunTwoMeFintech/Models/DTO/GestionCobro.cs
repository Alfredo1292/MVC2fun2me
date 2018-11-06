namespace TwoFunTwoMeFintech.Models.DTO
{
    public class GestionCobro
    {
        public int IdCredito { get; set; }

        public string FechaPP { get; set; }
        public string MontoPP { get; set; }
        public string Detalle { get; set; }
        public string AccionLlamada { get; set; }
        public string RespuestaGestion { get; set; }
        public string UsrModifica { get; set; }
    }
}