using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;

namespace MDS.Inventario.Api.Application.Mappers.Certificado
{
    public static class CertificadoMapper
    {
        public static SolicitudExtend Map(CertificadoModel dto)
        {
            return new SolicitudExtend()
            {
                ID_SOLICITUD = dto.IdSolicitud,
                ID_ESTUDIANTE = dto.IdEstudiante,
                ID_SOLICITANTE = dto.IdSolicitante,
                ID_MOTIVO = dto.IdMotivo,
                ID_MODALIDAD = dto.IdModalidad,
                ID_PERSONA = dto.IdPersona,
                ID_NIVEL = dto.IdNivel,
                ID_GRADO = dto.IdGrado,

                ID_TIPO_DOCUMENTO = dto.IdTipoDocumento,
                TIPO_DOCUMENTO = dto.TipoDocumento,
                NUMERO_DOCUMENTO = dto.NumeroDocumento,                
                ESTADO_SOLICITUD = dto.EstadoSolicitud,
                DSC_ESTADO_SOLICITUD = dto.DescripcionEstadoSolicitud,
                FECHA_SOLICITUD = dto.FechaSolicitud,
                NOMBRE_ESTUDIANTE = dto.NombresEstudiante,
                APELLIDO_PATERNO = dto.ApellidoPaterno,
                APELLIDO_MATERNO = dto.ApellidoMaterno,
                NOMBRE=dto.Nombre,
                CORREO_ELECTRONICO=dto.CorreoElectronico,
                ULTIMO_ANIO=dto.UltimoAnio,

                ABR_MODALIDAD = dto.AbreviaturaModalidad,
                DSC_MODALIDAD = dto.DescripcionModalidad,                
                DSC_NIVEL = dto.DescripcionNivel,                
                DSC_GRADO = dto.DescripcionGrado,
                FECHA_PROCESO = dto.FechaProceso,
                CODIGO_VIRTUAL = dto.CodigoVirtual,
                DSC_MOTIVO = dto.DescripcionMotivo,
                FECHA_INICIO = dto.FechaInicio,
                FECHA_FIN = dto.FechaFin,

                pageNumber= dto.pageNumber,
                rowsPerPage = dto.rowsPerPage,
                TotalRegistros = dto.TotalRegistros,
                COD_SOLICITUD = dto.CodSolicitud,
                CODIGO_MODULAR = dto.CodigoModular,
                ANEXO = dto.Anexo,

                DSC_ESTADO_ESTUDIANTE = dto.DscEstadoEstudiante,
                ESTADO_ESTUDIANTE = dto.EstadoEstudiante,
                CICLO = dto.Ciclo
            };
        }

        public static CertificadoModel Map(SolicitudExtend dto)
        {
            return new CertificadoModel()
            {
                IdSolicitud = dto.ID_SOLICITUD,
                IdEstudiante = dto.ID_ESTUDIANTE,
                IdSolicitante = dto.ID_SOLICITANTE,
                IdMotivo = dto.ID_MOTIVO,
                IdModalidad = dto.ID_MODALIDAD,
                IdPersona = dto.ID_PERSONA,
                IdNivel = dto.ID_NIVEL,
                IdGrado = dto.ID_GRADO,

                IdTipoDocumento = dto.ID_TIPO_DOCUMENTO,
                TipoDocumento = dto.TIPO_DOCUMENTO,
                NumeroDocumento = dto.NUMERO_DOCUMENTO,                
                EstadoSolicitud = dto.ESTADO_SOLICITUD,
                DescripcionEstadoSolicitud = dto.DSC_ESTADO_SOLICITUD,
                FechaSolicitud = dto.FECHA_SOLICITUD,
                NombresEstudiante = dto.NOMBRE_ESTUDIANTE,
                ApellidoPaterno = dto.APELLIDO_PATERNO,
                ApellidoMaterno = dto.APELLIDO_MATERNO,
                Nombre = dto.NOMBRE,
                CorreoElectronico = dto.CORREO_ELECTRONICO,
                UltimoAnio = dto.ULTIMO_ANIO,

                AbreviaturaModalidad = dto.ABR_MODALIDAD,
                DescripcionModalidad = dto.DSC_MODALIDAD,                
                DescripcionNivel = dto.DSC_NIVEL,                
                DescripcionGrado = dto.DSC_GRADO,
                FechaProceso = dto.FECHA_PROCESO,
                CodigoVirtual = dto.CODIGO_VIRTUAL,
                DescripcionMotivo = dto.DSC_MOTIVO,
                FechaInicio = dto.FECHA_INICIO,
                FechaFin = dto.FECHA_FIN,

                pageNumber = dto.pageNumber,
                rowsPerPage = dto.rowsPerPage,
                TotalRegistros = dto.TotalRegistros,

                CodSolicitud = dto.COD_SOLICITUD,
                CodigoModular = dto.CODIGO_MODULAR,
                Anexo = dto.ANEXO,
                EstadoEstudiante =dto.ESTADO_ESTUDIANTE,
                DscEstadoEstudiante = dto.DSC_ESTADO_ESTUDIANTE,
                Ciclo = dto.CICLO
            };
        }
    }
}
