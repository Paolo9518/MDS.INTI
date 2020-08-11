using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System;

namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
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
