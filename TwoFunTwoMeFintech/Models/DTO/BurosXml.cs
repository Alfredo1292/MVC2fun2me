using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class BurosXml
    {
        public string TUCA { set; get; }
        public string CREDDIT { set; get; }
        public string EQUIFAX { set; get; }
		public string CREDISERVER { set; get; }
		public string GINI { set; get; }
        public string MensajeError { set; get; }
        public string Mensaje { set; get; }

        public string DATATUCA { set; get; }
        public string DATACREDDIT { set; get; }
        public string DATAEQUIFAX { set; get; } 
        public string DATAGINI { set; get; }
        public string MIMETYPE { set; get; }
		public string DATACREDISERVER { set; get; }

		public string NOMBRETUCA { set; get; }
        public string NOMBRECREDDIT { set; get; }
        public string NOMBREEQUIFAX { set; get; }
        public string NOMBREGINI { set; get; }

		public string NOMBRECREDISERVER { set; get; }
	}
}