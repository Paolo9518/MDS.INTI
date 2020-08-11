using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;

namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public class NotasMapper
    {
        public static NotaCertificadoEntity Map(NotasRequest dto)
        {
            return new NotaCertificadoEntity()
            {
                ID_CONSTANCIA_NOTA = dto.IdConstanciaNota,
                ID_SOLICITUD = dto.IdSolicitud,
                ID_ANIO = dto.IdAnio,
                COD_MOD = dto.CodigoModular,
                ANEXO = dto.Anexo,
                ID_NIVEL = dto.IdNivel,
                DSC_NIVEL = dto.DscNivel,
                ID_GRADO = dto.IdGrado,
                DSC_GRADO = dto.DscGrado,
                ID_TIPO_AREA = dto.IdTipoArea,
                DSC_TIPO_AREA = dto.DscTipoArea,
                ID_AREA = dto.IdArea,
                DSC_AREA = dto.DscArea,
                ESCONDUCTA = dto.EsConducta,
                NOTA_FINAL_AREA = dto.NotaFinal,
                ESTADO = dto.Estado,
                CICLO = dto.Ciclo,
                ES_AREA_SIAGIE = dto.EsAreaSiagie,
                ACTIVO = dto.Activo,
                USUARIO= dto.usuario
            };
        }

        public static NotasRequest Map(NotaCertificadoEntity entity)
        {
            return new NotasRequest()
            {
                IdConstanciaNota = entity.ID_CONSTANCIA_NOTA,
                IdSolicitud = entity.ID_SOLICITUD,
                IdAnio = entity.ID_ANIO,
                CodigoModular = entity.COD_MOD,
                Anexo = entity.ANEXO,
                IdNivel = entity.ID_NIVEL,
                DscNivel = entity.DSC_NIVEL,
                IdGrado = entity.ID_GRADO,
                DscGrado = entity.DSC_GRADO,
                IdTipoArea = entity.ID_TIPO_AREA,
                DscTipoArea = entity.DSC_TIPO_AREA,
                IdArea = entity.ID_AREA,
                DscArea = entity.DSC_AREA,
                EsConducta = entity.ESCONDUCTA,
                NotaFinal = entity.NOTA_FINAL_AREA,
                Estado = entity.ESTADO,
                Ciclo = entity.CICLO,
                EsAreaSiagie = entity.ES_AREA_SIAGIE,
                Activo = entity.ACTIVO,
                usuario = entity.USUARIO
            };
        }
    }
}
