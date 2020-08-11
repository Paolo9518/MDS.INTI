using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;
using System;

namespace Minedu.MiCertificado.Api.Application.Mappers.Constancia
{
    public static class MotivoMapper
    {
        public static MotivoEntity Map(MotivoModel dto, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new MotivoEntity()
            {
                ID_MOTIVO = encryptionServerSecurity.Decrypt<int>(dto.idMotivo, 0),
                DESCRIPCION = dto.descripcion,
                REQUIERE_DETALLE = dto.requiereDetalle
            };
        }

        public static MotivoModel Map(MotivoEntity entity, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new MotivoModel()
            {
                idMotivo = encryptionServerSecurity.Encrypt(entity.ID_MOTIVO.ToString()),
                descripcion = entity.DESCRIPCION,
                requiereDetalle = entity.REQUIERE_DETALLE
            };
        }
    }
}
