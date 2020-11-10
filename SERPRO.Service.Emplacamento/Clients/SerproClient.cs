using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SERPRO.Service.Emplacamento.Configs;
using SERPRO.Service.Emplacamento.Enums;
using SERPRO.Service.Emplacamento.Messages.Results;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SERPRO.Service.Emplacamento.Clients
{
    public class SerproClient : ISerproClient
    {
        private readonly SerproClientConfig _config = new SerproClientConfig();

        public SerproClient(IConfiguration configuration)
            => configuration.GetSection("SerproClient").Bind(_config);

        public async Task<T> GetAllAsync<T>(TipoIntegracaoFabricante tipoIntegracao, Dictionary<string, string> parameters = null) where T : BaseResult
        {
            var baseAddress = new Uri(_config.BaseUrl);

            #region Definição de controller

            //Controller que será chamada
            var controller = string.Empty;

            switch (tipoIntegracao)
            {
                case TipoIntegracaoFabricante.AceiteDevolucaoBlanks:
                    controller = "aceites-devolucao-blanks";
                    break;
                case TipoIntegracaoFabricante.Blanks:
                    controller = "blanks";
                    break;
                case TipoIntegracaoFabricante.BlanksDevolvidos:
                    controller = "blanks-devolvidos";
                    break;
                case TipoIntegracaoFabricante.BlanksEnviados:
                    controller = "blanks-enviados";
                    break;
                case TipoIntegracaoFabricante.CancelamentoEnvioBlanks:
                    controller = "cancelamentos-envios-blanks";
                    break;
                case TipoIntegracaoFabricante.ClienteAutenticado:
                    controller = "cliente-autenticado";
                    break;
                case TipoIntegracaoFabricante.EnvioBlanks:
                    controller = "envios-blanks";
                    break;
                case TipoIntegracaoFabricante.Estampador:
                    controller = "estampadores";
                    break;
                case TipoIntegracaoFabricante.InutilizacaoBlanks:
                    controller = "inutilizacoes-blanks";
                    break;
                case TipoIntegracaoFabricante.Lotes:
                    controller = "lotes";
                    break;
                case TipoIntegracaoFabricante.RecebimentoBlanks:
                    controller = "recebimentos-blanks";
                    break;
                default:
                    break;
            }

            #endregion

            //usa proxy, caso tenha
            var proxy = new WebProxy
            {
                UseDefaultCredentials = true,
            };

            using var httpHandler = new HttpClientHandler
            {
                //Ignora aviso de certificado inválido do Service Layer
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; },

                //Define uso do proxy
                Proxy = proxy
            };


            using var httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = baseAddress,
                Timeout = TimeSpan.FromMinutes(_config.Timeout)
            };

            string requestUri = "api-fabricantes/" + controller;

            if (parameters != null)
                requestUri = QueryHelpers.AddQueryString(requestUri, parameters);

            var response = await httpClient.GetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Retorno
            if (response.StatusCode == HttpStatusCode.OK)
                return (T)JsonConvert.DeserializeObject<BaseResult>(responseContent);
            else if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ArgumentException(responseContent);
            else
                throw new Exception(responseContent);
        }

        public async Task<dynamic> GetAsync(TipoIntegracaoFabricante tipoIntegracao, dynamic identifier)
        {
            var baseAddress = new Uri(_config.BaseUrl);

            #region Definição de controller

            //Controller que será chamada
            var controller = string.Empty;

            switch (tipoIntegracao)
            {
                case TipoIntegracaoFabricante.AceiteDevolucaoBlanks:
                    controller = "aceites-devolucao-blanks";
                    break;
                case TipoIntegracaoFabricante.Blanks:
                    controller = "blanks";
                    break;
                case TipoIntegracaoFabricante.BlanksDevolvidos:
                    controller = "blanks-devolvidos";
                    break;
                case TipoIntegracaoFabricante.BlanksEnviados:
                    controller = "blanks-enviados";
                    break;
                case TipoIntegracaoFabricante.CancelamentoEnvioBlanks:
                    controller = "cancelamentos-envios-blanks";
                    break;
                case TipoIntegracaoFabricante.ClienteAutenticado:
                    controller = "cliente-autenticado";
                    break;
                case TipoIntegracaoFabricante.EnvioBlanks:
                    controller = "envios-blanks";
                    break;
                case TipoIntegracaoFabricante.Estampador:
                    controller = "estampadores";
                    break;
                case TipoIntegracaoFabricante.InutilizacaoBlanks:
                    controller = "inutilizacoes-blanks";
                    break;
                case TipoIntegracaoFabricante.Lotes:
                    controller = "lotes";
                    break;
                case TipoIntegracaoFabricante.RecebimentoBlanks:
                    controller = "recebimentos-blanks";
                    break;
                default:
                    break;
            }

            #endregion

            //usa proxy, caso tenha
            var proxy = new WebProxy
            {
                UseDefaultCredentials = true,
            };

            using var httpHandler = new HttpClientHandler
            {
                //Define uso do proxy
                Proxy = proxy
            };

            using var httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = baseAddress,
                Timeout = TimeSpan.FromMinutes(_config.Timeout)
            };

            var requestUri = "api-fabricante/" + controller + "/" + identifier;

            var response = await httpClient.GetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Retorno
            if (response.StatusCode == HttpStatusCode.OK)
                return responseContent;
            else if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ArgumentException(responseContent);
            else
                throw new Exception(responseContent);
        }

        public async Task<string> PostAsync(TipoIntegracaoFabricante tipoIntegracao, dynamic command)
        {
            var jsonContent =
               JsonConvert.SerializeObject(command,
                   Formatting.None,
                   new JsonSerializerSettings
                   {
                       NullValueHandling = NullValueHandling.Ignore
                   });

            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var baseAddress = new Uri(_config.BaseUrl);

            #region Definição de controller

            //Controller que será chamada
            var controller = string.Empty;

            switch (tipoIntegracao)
            {
                case TipoIntegracaoFabricante.AceiteDevolucaoBlanks:
                    controller = "aceites-devolucao-blanks";
                    break;
                case TipoIntegracaoFabricante.Blanks:
                    controller = "blanks";
                    break;
                case TipoIntegracaoFabricante.BlanksDevolvidos:
                    controller = "blanks-devolvidos";
                    break;
                case TipoIntegracaoFabricante.BlanksEnviados:
                    controller = "blanks-enviados";
                    break;
                case TipoIntegracaoFabricante.CancelamentoEnvioBlanks:
                    controller = "cancelamentos-envios-blanks";
                    break;
                case TipoIntegracaoFabricante.ClienteAutenticado:
                    controller = "cliente-autenticado";
                    break;
                case TipoIntegracaoFabricante.EnvioBlanks:
                    controller = "envios-blanks";
                    break;
                case TipoIntegracaoFabricante.Estampador:
                    controller = "estampadores";
                    break;
                case TipoIntegracaoFabricante.InutilizacaoBlanks:
                    controller = "inutilizacoes-blanks";
                    break;
                case TipoIntegracaoFabricante.Lotes:
                    controller = "lotes";
                    break;
                case TipoIntegracaoFabricante.RecebimentoBlanks:
                    controller = "recebimentos-blanks";
                    break;
                default:
                    break;
            }

            #endregion

            //usa proxy, caso tenha
            var proxy = new WebProxy
            {
                UseDefaultCredentials = true,
            };

            using var httpHandler = new HttpClientHandler
            {
                //Define uso do proxy
                Proxy = proxy
            };

            using var httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = baseAddress,
                Timeout = TimeSpan.FromMinutes(_config.Timeout)
            };

            var response = await httpClient.PostAsync("api-fabricante/" + controller, contentString);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Retorno
            if (response.StatusCode == HttpStatusCode.Created)
                return responseContent;
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException(responseContent);
            else
                throw new Exception(responseContent);
        }
    }
}
