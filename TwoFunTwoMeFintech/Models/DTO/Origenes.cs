namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Origenes
    {
        public int Id { get; set; }

        public int? Origen { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool? Estado { get; set; }

        public string Aplicativo { get; set; }
        public string traceId { get; set; }

        public string Paso { get; set; }
    }
}