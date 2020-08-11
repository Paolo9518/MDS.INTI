using Minedu.Comun.IData;
using Entities = Minedu.MiCertificado.Api.DataAccess.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        Task<IEnumerable<Entities.Certificado.MenuCertificadoEntity>> ObtenerCertificadoMenu(Entities.Certificado.MenuCertificadoEntity entity);
        Task<IEnumerable<Entities.Certificado.DeclaracionJuradaCertificadoEntity>> ObtenerCertificadoDeclaracionJurada();
        Task<IEnumerable<Entities.Certificado.MotivoCertificadoEntity>> ObtenerCertificadoMotivo(Entities.Certificado.MotivoCertificadoEntity entity);
        Task<bool> EnviarCorreoCertificado(Entities.CorreoEntity entity);
        Task<IEnumerable<AreaCertificadoEntity>> ObtenerArea(string codigoTipoArea, string nivel);
        Task<string> InsertArea(string codigoTipoArea, string nivel, string descripcionArea, string usuario, string modalidad);
        Task<IEnumerable<SolicitudExtend>> ObtenerAniosSolicitud(string IdNivel, string CodigoModular, string Anexo, string EstadoSolicitud);
    }
}
