using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;

namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public static class IEMapper
    {
        public static IEEntity Map(IEModel dto)
        {
            return new IEEntity()
            {
                TIPO_DOCUMENTO = dto.tipoDocumento,
                NUMERO_DOCUMENTO = dto.numeroDocumento,
                CODIGO_MODULAR = dto.codigoModular,
                ANEXO = dto.anexo,
                CENTRO_EDUCATIVO = dto.centroEducativo,
                ID_ROL = dto.idRol,
                DESCRIPCION_ROL =dto.descripcionRol,
                NIVEL = dto.nivel,
                TIPO_SEDE = dto.tipoSede,
                POR_DEFECTO = dto.porDefecto,
                ESTADO = dto.estado, //HJSH
                USUARIO=dto.usuario
            };
        }

        public static IEModel Map(IEEntity entity)
        {
            return new IEModel()
            {
                tipoDocumento = entity.TIPO_DOCUMENTO,
                numeroDocumento = entity.NUMERO_DOCUMENTO,
                codigoModular = entity.CODIGO_MODULAR,
                anexo = entity.ANEXO,
                centroEducativo = entity.CENTRO_EDUCATIVO,
                idRol = entity.ID_ROL,
                descripcionRol = entity.DESCRIPCION_ROL,
                nivel = entity.NIVEL,
                tipoSede = entity.TIPO_SEDE,
                porDefecto = entity.POR_DEFECTO,
                estado = entity.ESTADO, //HJSH
                usuario = entity.USUARIO
            }; 
        }
    }
}
