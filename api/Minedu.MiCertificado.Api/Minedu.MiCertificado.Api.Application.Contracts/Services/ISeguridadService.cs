using Minedu.MiCertificado.Api.BusinessLogic.Models.Central;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface ISeguridadService
    {
        //Task<UsuarioAutenticacion_IIResponse> SeguridadConsultarDatos(UsuarioAutenticacion_IIRequest request);
        Task<BEUsuarioResponse> SeguridadConsultarDatos(AuthModel request, string idSistema);

        Task<BEUsuarioPermisoResponse> UsuarioPermisoTraerPorDefecto(string usrLogUsr, string id_sistema_id);
        Task<BEUsuarioPermisoResponse> UsuarioPermisoLeerPorSistema(string usrLogUsr, string id_sistema_id);

        Task<List<BEUsuarioPermisoResponse>> UsuarioPermisoBuscar(string usrLogUsr, string id_sistema_id);
        Task<List<BEUsuarioPermisoResponse>> UsuarioPermisoListar(string usrLogUsr);

        Task<BEUsuarioPermisoResponse> UsuarioPermisosLlenar(string anexo, string idSede, string idSistema, string usrLogUsr);
        //Task<SeguridadWSService.UsuarioPermisoListarResponse> UsuarioPermisoListar(string nombreUsuario)
        Task<List<UsuarioPermiso>> UsuarioPermisoListarSede(string cCodModular, string cCodAnexo, string idSistema);
        Task<UsuarioPermiso> UsuarioPermisoLeerPorKey(UsuarioPermisoResponse request);
        void UsuarioPermisoActualizar(UsuarioPermisoRequest request);
        void UsuarioPermisoInsertar(UsuarioPermisoRequest request);
    }
}
