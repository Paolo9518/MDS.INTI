using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;

namespace MDS.Inventario.Api.Application.Mappers.Certificado
{
    public class ObservacionCertificadoMapper
    {
        public static ObservacionCertificadoEntity Map(ObservacionCertificadoModel dto)
        {
            return new ObservacionCertificadoEntity()
            {
                ID_CERTIFICADO_OBSERVACION = dto.idCertificadoObservacion,
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

        public static ObservacionCertificadoModel Map(ObservacionCertificadoEntity entity)
        {
            return new ObservacionCertificadoModel()
            {
                idCertificadoObservacion = entity.ID_CERTIFICADO_OBSERVACION,
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
