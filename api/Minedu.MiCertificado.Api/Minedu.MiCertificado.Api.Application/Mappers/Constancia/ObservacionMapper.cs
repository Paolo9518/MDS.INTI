using Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;

namespace Minedu.MiCertificado.Api.Application.Mappers.Constancia
{
    public class ObservacionMapper
    {
        public static ObservacionEntity Map(ObservacionModel dto)
        {
            return new ObservacionEntity()
            {
                ID_CONSTANCIA_OBSERVACION = dto.idConstanciaObservacion,
                ID_SOLICITUD = dto.idSolicitud,
                ID_NIVEL = dto.idNivel,
                ID_ANIO = dto.idAnio,
                RESOLUCION = dto.resolucion,
                TIPO_SOLICITUD = dto.tipoSolicitud,
                MOTIVO = dto.motivo,
                ID_TIPO = dto.idTipo,
                DSC = dto.dsc,
                TIPO_OBS = dto.tipoObs
            };
        }

        public static ObservacionModel Map(ObservacionEntity entity)
        {
            return new ObservacionModel()
            {
                idConstanciaObservacion = entity.ID_CONSTANCIA_OBSERVACION,
                idSolicitud = entity.ID_SOLICITUD,
                idNivel = entity.ID_NIVEL,
                idAnio = entity.ID_ANIO,
                resolucion = entity.RESOLUCION,
                tipoSolicitud = entity.TIPO_SOLICITUD,
                motivo = entity.MOTIVO,
                idTipo = entity.ID_TIPO,
                dsc = entity.DSC,
                tipoObs = entity.TIPO_OBS
            };
        }
    }
}
