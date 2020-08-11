using Microsoft.Extensions.Configuration;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Minedu.MiCertificado.Api.BusinessLogic.Models;
using ReniecWSService;
using System;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.Application.Services
{
    public class ReniecService : IReniecService
    {
        private readonly IConfiguration _configuration;

        public ReniecService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private System.ServiceModel.ChannelFactory<T> init<T>()
        {
            var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.None);
            //binding.MaxReceivedMessageSize = Int32.MaxValue;
            //binding.MaxBufferSize = Int32.MaxValue;

            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxBufferSize = Int32.MaxValue;

            string url = _configuration.GetSection("ReniecService:BaseUrl").Value;

            var endpoint = new System.ServiceModel.EndpointAddress(url);
            var channelFactory = new System.ServiceModel.ChannelFactory<T>(binding, endpoint);
            return channelFactory;
        }

        public async Task<ReniecPersona> ReniecConsultarPersona(string nroDocumento)
        {
            ReniecPersona reniecPersona = null;

            try
            {
                using (var clientService = init<ReniecWSChannel>().CreateChannel())
                {
                    buscarDNICascada request = new buscarDNICascada()
                    {
                        usuario = _configuration.GetSection("ReniecService:UserName").Value,
                        clave = _configuration.GetSection("ReniecService:Password").Value,
                        ipsistema = _configuration.GetSection("ReniecService:RequestIP").Value,
                        dni = nroDocumento
                    };

                    var response = await clientService.buscarDNICascadaAsync(request);

                    if (response.@return.codigo.Equals("00"))
                    {
                        reniecPersona = new ReniecPersona();
                        var result = response.@return.persona;

                        reniecPersona.numDoc = result.numDoc;

                        reniecPersona.apellidoMaterno = result.apellidoMaterno;
                        reniecPersona.apellidoPaterno = result.apellidoPaterno;
                        reniecPersona.nombres = result.nombres;

                        reniecPersona.fecNacimiento = result.fecNacimiento;

                        reniecPersona.nombrePadre = result.nombrePadre;
                        reniecPersona.nroDocPadre = result.nroDocPadre;

                        reniecPersona.nombreMadre = result.nombreMadre;
                        reniecPersona.nroDocMadre = result.nroDocMadre;

                        reniecPersona.ubigeoDomicilio = result.dptoDomicilio + result.provDomicilio + result.distDomicilio;

                        reniecPersona.dptoDomicilio = result.dptoDomicilio;
                        reniecPersona.provDomicilio = result.provDomicilio;
                        reniecPersona.distDomicilio = result.distDomicilio;

                        reniecPersona.fecFallecimientoSpecified = result.fecFallecimientoSpecified;
                    }
                    else if(response.@return.codigo.Equals("01"))
                    {
                        reniecPersona = null;
                    }
                    else
                    { 
                        throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(ArgumentException)))
                {
                    throw;
                }
                else
                {
                    reniecPersona = null;
                }
            }

            return reniecPersona;
        }
    }
}
