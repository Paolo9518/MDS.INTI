using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;


namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public class GradoCertificadoMapper
    {
        public static GradoCertificadoEntity Map(GradoCertificadoModel dto)
        {
            return new GradoCertificadoEntity()
            {
                ID_CONSTANCIA_GRADO = dto.idConstanciaGrado,
                ID_SOLICITUD = dto.idSolicitud,
                ID_GRADO = dto.idGrado,
                DSC_GRADO = dto.dscGrado,
                CORR_ESTADISTICA = dto.corrEstadistica,
                ID_ANIO = dto.idAnio,
                COD_MOD = dto.codMod,
                ANEXO = dto.anexo,
                SITUACION_FINAL = dto.situacionFinal,
                CICLO = dto.ciclo,
                USUARIO=dto.usuario
            };
        }

        public static GradoCertificadoModel Map(GradoCertificadoEntity entity)
        {
            return new GradoCertificadoModel()
            {
                idConstanciaGrado = entity.ID_CONSTANCIA_GRADO,
                idSolicitud = entity.ID_SOLICITUD,
                idGrado = entity.ID_GRADO,
                dscGrado = entity.DSC_GRADO,
                corrEstadistica = entity.CORR_ESTADISTICA,
                idAnio = entity.ID_ANIO,
                codMod = entity.COD_MOD,
                anexo = entity.ANEXO,
                situacionFinal = entity.SITUACION_FINAL,
                ciclo = entity.CICLO,
                usuario=entity.USUARIO
            };
        }
    }
}
