using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;

namespace MDS.Inventario.Api.Application.Mappers.Certificado
{
    public static class SolicitudCertificadoMapper
    {
        public static SolicitudExtend Map(SolicitudCertificadoModel dto)
        {
            return new SolicitudExtend()
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
                FECHA_ACTUALIZACION = dto.fechaActualizacion,
                MOTIVO_OTROS = dto.motivoOtros,
                CODIGO_MODULAR = dto.codigoModular,
                ANEXO = dto.anexo,
                ANIO_CULMINACION = dto.anioCulminacion,
                CICLO = dto.ciclo,
                ESTADO_ESTUDIANTE = dto.estadoEstudiante,
                CORREO_ELECTRONICO = dto.correoElectronico,
                APELLIDO_PATERNO = dto.apellidoPaterno,
                APELLIDO_MATERNO = dto.apellidoMaterno,
                NOMBRE = dto.nombre,
                DIRECTOR = dto.director,
                DSC_MOTIVO = dto.descripcionMotivo
            };
        }

        public static SolicitudCertificadoModel Map(SolicitudExtend entity)
        {
            return new SolicitudCertificadoModel()
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
                fechaActualizacion = entity.FECHA_ACTUALIZACION,
                motivoOtros = entity.MOTIVO_OTROS,
                codigoModular = entity.CODIGO_MODULAR,
                anexo = entity.ANEXO,
                anioCulminacion = entity.ANIO_CULMINACION,
                ciclo = entity.CICLO,
                estadoEstudiante = entity.ESTADO_ESTUDIANTE,
                correoElectronico = entity.CORREO_ELECTRONICO,
                apellidoPaterno = entity.APELLIDO_PATERNO,
                apellidoMaterno = entity.APELLIDO_MATERNO,
                nombre = entity.NOMBRE,
                director = entity.DIRECTOR,
                descripcionMotivo = entity.DSC_MOTIVO
            };
        }
    }
}
