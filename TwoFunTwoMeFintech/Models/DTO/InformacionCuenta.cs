﻿using System;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class InformacionCuenta
    {
        public string IdCredito { get; set; }
        public string Producto { get; set; }
        public int? DiasVenc { get; set; }
        public string Cuota { get; set; }
        public string originacion { get; set; }
        public string SaldoVenc { get; set; }
        public string SaldoTotal { get; set; }
        public string CapitalPendiente { get; set; }
        public string IntCorriente { get; set; }
        public string IntMoraPendiente { get; set; }
<<<<<<< HEAD
		public string MontoCargoMora { get; set; }
=======
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
        public string MoraDesde { get; set; }
        public string UltimaFechaAlDia { get; set; }
        public string FechaUltPago { get; set; }
        public string MontoUltPago { get; set; }

        public string tipomovimiento { get; set; }
        public string Fechas { get; set; }
        public string OriginacionPendiente { get; set; }
        public string InteresPendiente { get; set; }
        public string MoraPendiente { get; set; }
        public string SaldoAlDia { get; set; }
        public string totalcobrado { get; set; }
        public string CapitalCobrado { get; set; }
        public string OriginacionCobrado { get; set; }
        public string InteresCobrado { get; set; }
        public string MoraCobrado { get; set; }
    }
}