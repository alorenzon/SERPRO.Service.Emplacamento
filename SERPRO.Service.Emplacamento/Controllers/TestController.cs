using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SERPRO.Service.Emplacamento.Clients;
using SERPRO.Service.Emplacamento.Enums;
using SERPRO.Service.Emplacamento.Messages.Commands.Lotes;
using SERPRO.Service.Emplacamento.Messages.Results;
using SERPRO.Service.Emplacamento.Messages.Results.Lotes;

namespace SERPRO.Service.Emplacamento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ISerproClient _client;
        public TestController(ISerproClient client)
            => _client = client;

        [HttpGet, Route("v1/GetAllAsync")]
        public async Task<ActionResult<IEnumerable<LoteResult>>> GetAllAsync()
        {
            var resultSerpro = await _client.GetAllAsync<BaseResult>(TipoIntegracaoFabricante.Lotes);

            var result = new List<LoteResult>();

            foreach (var lote in resultSerpro._embedded.Lotes)
            {
                var newLote = new LoteResult()
                {
                    Numero = lote.Numero,
                    QuantidadeDeBlanksSolicitados = lote.QuantidadeDeBlanksSolicitados,
                    DataHoraRegistro = lote.DataHoraRegistro,
                    Estado = lote.Estado
                };

                result.Add(newLote);
            }

            return Ok(result);
        }

        [HttpGet, Route("v1/GetAsync/{id}")]
        public async Task<ActionResult<LoteResult>> GetAsync([FromRoute] string id)
        {
            var resultSerpro = await _client.GetAsync(TipoIntegracaoFabricante.Lotes, id);

            var result = JsonConvert.DeserializeObject<LoteResult>(resultSerpro);

            return Ok(result);
        }

        [HttpGet, Route("v1/PostAsync")]
        public async Task<ActionResult<LoteResult>> PostAsync([FromBody] InsereLote command)
        {
            var resultSerpro = await _client.PostAsync(TipoIntegracaoFabricante.Lotes, command);

            var result = JsonConvert.DeserializeObject<LoteResult>(resultSerpro);

            return Ok(result);

        }
    }
}
