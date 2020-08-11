using MDS.Inventario.Api.Application.Entities.Models;
using MDS.Inventario.Api.DataAccess.Contracts.Entities;

namespace MDS.Inventario.Api.Application.Mappers
{
    public static class CorreoMapper
    {
        public static CorreoEntity Map(CorreoModel dto)
        {
            return new CorreoEntity()
            {
                PARA = dto.para,
                CC = dto.cc,
                CCO = dto.cco,
                ASUNTO = dto.asunto,
                MENSAJE = dto.mensaje
            };
        }

        public static CorreoModel Map(CorreoEntity entity)
        {
            return new CorreoModel()
            {
                para = entity.PARA,
                cc = entity.CC,
                cco = entity.CCO,
                asunto = entity.ASUNTO,
                mensaje = entity.MENSAJE
            };
        }
    }
}
