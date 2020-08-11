using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System;

namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public static class MotivoMapper
    {
        public static MotivoCertificadoEntity Map(MotivoCertificadoModel dto, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new MotivoCertificadoEntity()
            {
                ID_MOTIVO = encryptionServerSecurity.Decrypt<int>(dto.idMotivo, 0),
                DESCRIPCION = dto.descripcion,
                REQUIERE_DETALLE = dto.requiereDetalle
            };
        }

        public static MotivoCertificadoModel Map(MotivoCertificadoEntity entity, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new MotivoCertificadoModel()
            {
                idMotivo = encryptionServerSecurity.Encrypt(entity.ID_MOTIVO.ToString()),
                descripcion = entity.DESCRIPCION,
                requiereDetalle = entity.REQUIERE_DETALLE
            };
        }
    }
}
