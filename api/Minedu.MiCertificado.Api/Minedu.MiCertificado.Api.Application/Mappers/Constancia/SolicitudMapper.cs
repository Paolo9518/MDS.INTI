using Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;

namespace Minedu.MiCertificado.Api.Application.Mappers.Constancia
{
    public static class SolicitudMapper
    {
        public static SolicitudEntity Map(SolicitudModel dto)
        {
            return new SolicitudEntity()
            {
                ID_SOLICITUD = dto.idSolicitud,
                ID_ESTUDIANTE = dto.idEstudiante,
                ID_SOLICITANTE = dto.idSolicitante,
                ID_MOTIVO = dto.idMotivo,
                ID_MODALIDAD = dto.idModalidad,
                ABR_MODALIDAD = dto.abrModalidad,
                DSC_MODALIDAD = dto.dscModalidad,
                ID_NIVEL = dto.idNivel,
                DSC_NIVEL = dto.dscNivel,
                ID_GRADO = dto.idGrado,
                DSC_GRADO = dto.dscGrado,
                ESTADO_SOLICITUD = dto.estadoSolicitud,
                CODIGO_VIRTUAL = dto.codigoVirtual,
                FECHA_CREACION = dto.fechaCreacion,
                MOTIVO_OTROS = dto.motivoOtros
            };
        }

        public static SolicitudModel Map(SolicitudEntity entity)
        {
            return new SolicitudModel()
            {
                idSolicitud = entity.ID_SOLICITUD,
                idSolicitante = entity.ID_SOLICITANTE,
                idEstudiante = entity.ID_ESTUDIANTE,
                idMotivo = entity.ID_MOTIVO,
                idModalidad = entity.ID_MODALIDAD,
                abrModalidad = entity.ABR_MODALIDAD,
                dscModalidad = entity.DSC_MODALIDAD,
                idNivel = entity.ID_NIVEL,
                dscNivel = entity.DSC_NIVEL,
                idGrado = entity.ID_GRADO,
                dscGrado = entity.DSC_GRADO,
                estadoSolicitud = entity.ESTADO_SOLICITUD,
                codigoVirtual = entity.CODIGO_VIRTUAL,
                fechaCreacion = entity.FECHA_CREACION,
                motivoOtros = entity.MOTIVO_OTROS
            };
        }
    }
}
