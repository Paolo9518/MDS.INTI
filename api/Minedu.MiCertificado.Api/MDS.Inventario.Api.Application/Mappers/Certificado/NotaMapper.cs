using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;

namespace MDS.Inventario.Api.Application.Mappers.Certificado
{
    public class NotaMapper
    {
        public static NotaCertificadoEntity Map(NotaCertificadoModel dto)
        {
            return new NotaCertificadoEntity()
            {
                ID_CONSTANCIA_NOTA = dto.idConstanciaNota,
                ID_SOLICITUD = dto.idSolicitud,
                ID_ANIO = dto.idAnio,
                COD_MOD = dto.codMod,
                ANEXO = dto.anexo,
                ID_NIVEL = dto.idNivel,
                DSC_NIVEL = dto.dscNivel,
                ID_GRADO = dto.idGrado,
                DSC_GRADO = dto.dscGrado,
                ID_TIPO_AREA = dto.idTipoArea,
                DSC_TIPO_AREA = dto.dscTipoArea,
                ID_AREA = dto.idArea,
                DSC_AREA = dto.dscArea,
                ESCONDUCTA = dto.esconducta,
                NOTA_FINAL_AREA = dto.notaFinalArea
            };
        }

        public static NotaCertificadoModel Map(NotaCertificadoEntity entity)
        {
            return new NotaCertificadoModel()
            {
                idConstanciaNota = entity.ID_CONSTANCIA_NOTA,
                idSolicitud = entity.ID_SOLICITUD,
                idAnio = entity.ID_ANIO,
                codMod = entity.COD_MOD,
                anexo = entity.ANEXO,
                idNivel = entity.ID_NIVEL,
                dscNivel = entity.DSC_NIVEL,
                idGrado = entity.ID_GRADO,
                dscGrado = entity.DSC_GRADO,
                idTipoArea = entity.ID_TIPO_AREA,
                dscTipoArea = entity.DSC_TIPO_AREA,
                idArea = entity.ID_AREA,
                dscArea = entity.DSC_AREA,
                esconducta = entity.ESCONDUCTA,
                notaFinalArea = entity.NOTA_FINAL_AREA
            };
        }
    }
}
