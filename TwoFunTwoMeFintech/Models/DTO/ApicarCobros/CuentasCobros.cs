using System;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class CuentasCobros
    {
        public string ReferenciaBancaria { get; set; }
        public int IdBanco { get; set; }
        public string BancoTransaccion { get; set; }
        public string FechaTransaccion { get; set; }
        public string DescripcionTrans { get; set; }
        public Decimal MontoCredito { get; set; }
    }
}