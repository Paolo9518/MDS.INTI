using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public static class DeclaracionJuradaMapper
    {
        public static DeclaracionJuradaCertificadoEntity Map(DeclaracionJuradaCertificadoModel dto, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new DeclaracionJuradaCertificadoEntity()
            {
                ID_DECLARACION = encryptionServerSecurity.Decrypt<int>(dto.idDeclaracion, 0),
                DESCRIPCION = dto.descripcion
            };
        }

        public static DeclaracionJuradaCertificadoModel Map(DeclaracionJuradaCertificadoEntity entity, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new DeclaracionJuradaCertificadoModel()
            {
                idDeclaracion = encryptionServerSecurity.Encrypt(entity.ID_DECLARACION.ToString()),
                descripcion = entity.DESCRIPCION
            };
        }
    }
}
