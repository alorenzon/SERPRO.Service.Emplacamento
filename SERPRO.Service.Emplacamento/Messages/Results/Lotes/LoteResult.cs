using System;

namespace SERPRO.Service.Emplacamento.Messages.Results.Lotes
{
    public class LoteResult
    {
        public string Numero { get; set; }
        public int QuantidadeDeBlanksSolicitados { get; set; }
        public DateTime DataHoraRegistro { get; set; }
        public string Estado { get; set; }
    }
}
