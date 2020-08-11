using MDS.Inventario.Api.Application.Contracts.Security;
using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;
using System;

namespace MDS.Inventario.Api.Application.Mappers.Certificado
{
    public static class AnioMapper
    {
        public static SolicitudExtend Map(AnioPorSolicitudResponse dto)
        {
            return new SolicitudExtend()
            {
                ANIO_CULMINACION = dto.IdAnio
            };
        }
 
        public static AnioPorSolicitudResponse Map(SolicitudExtend entity)
        {
            return new AnioPorSolicitudResponse()
            {
                IdAnio = entity.ANIO_CULMINACION

            };
        }
    }
}
