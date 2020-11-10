using SERPRO.Service.Emplacamento.Enums;
using SERPRO.Service.Emplacamento.Messages.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERPRO.Service.Emplacamento.Clients
{
    public interface ISerproClient
    {
        Task<T> GetAllAsync<T>(TipoIntegracaoFabricante tipoIntegracao, Dictionary<string, string> parameters = null) where T : BaseResult;
        Task<dynamic> GetAsync(TipoIntegracaoFabricante tipoIntegracao, dynamic identfier );
        Task<string> PostAsync(TipoIntegracaoFabricante tipoIntegracao, dynamic command);
    }
}
