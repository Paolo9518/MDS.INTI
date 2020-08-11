using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface ICertificadoService
    {
        Task<StatusResponse> ConsultarCertificado(CertificadoModel model);

        Task<int> UpdateCertificado(ParametroModel objetoEncriptado);

        Task<StatusResponse> ConsultarDatosAcademicos(ParametroModel objetoEncriptado);

        Task<StatusResponse> ObtenerUsuariosPorIE(ParametroModel objetoEncriptado);

        Task<StatusResponse> ActualizarUsuariosPorIE(ParametroModel objetoEncriptado);

        Task<StatusResponse> InsertRegistroSolicitud(ParametroModel objetoEncriptado);

        Task<StatusResponse> ValidarDatosReniec(string nroDocumento, string idNivel);

        Task<int> InsertUpdateInstitucion(ParametroModel objetoEncriptado);

        Task<StatusResponse> PostInstitucion(ParametroModel objetoEncriptado);

        Task<StatusResponse> ConsultarSolicitudRechazada(ParametroModel objetoEncriptado);

        Task<StatusResponse> ConsultarCertificadosEmitidos(ParametroModel objetoEncriptado);

        Task<StatusResponse> ConsultarSolicitudPendiente(ParametroModel objetoEncriptado);

        Task<StatusResponse> InsertDatosAcademicos(ParametroModel objetoEncriptado);

        Task<StatusResponse> ValdarNotasPendientes(ParametroModel objetoEncriptado);

        Task<byte[]> VistaPreviaPDFCertificado(ParametroModel objetoEncriptado);

        Task<StatusResponse> EstudiantesPorAnioGradoSeccion(ParametroModel objetoEncriptado);
                
        Task<StatusResponse> ValidarSolicitud(CertificadoRequest request);

        Task<StatusResponse> GenerarPDFCertificado(ParametroModel objetoEncriptado);

        Task<byte[]> ValidarPDFCertificado(ParametroModel objetoEncriptado);

        Task<StatusResponse> RegistroMasivoEstudiantes(ParametroModel objetoEncriptado);

        Task<StatusResponse> ValidarDatosReniecApoderado(string nroDocumento);

        Task<StatusResponse> ValidarArea(AreaRequest request);

        Task<StatusResponse> ObtenerAreasPorDisenio(AreaPorDisenioRequest request);

        Task<byte[]> VistaPreviaPDF(ParametroModel objetoEncriptado);

        Task<int> UpdateEstadoCertificado(ParametroModel objetoEncriptado);

        Task<StatusResponse> ObtenerDatosSolicitud(ParametroModel objetoEncriptado);

        Task<StatusResponse> ActualizarDatosAcademicos(ParametroModel objetoEncriptado);

        Task<StatusResponse> ObtenerGradosPorNivelPersona(ParametroModel objetoEncriptado);

        //Task<StatusResponse> ObtenerDatosEstudiante(EstudiantePersonaRequest2 request);
        Task<StatusResponse> ObtenerDatosEstudiante(ParametroModel objetoEncriptado);

        Task<StatusResponse> RegistrarSolicitudes(ParametroModel objetoEncriptado);

        Task<StatusResponse> ConsultaPorNumeroDocumento(ParametroModel objetoEncriptado);

        Task<StatusResponse> ValidarCertificadoEstudios(ParametroModel objetoEncriptado);
    }
}