using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.Application.Mappers.Constancia
{
    public static class DeclaracionJuradaMapper
    {
        public static DeclaracionJuradaEntity Map(DeclaracionJuradaModel dto, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new DeclaracionJuradaEntity()
            {
                ID_DECLARACION = encryptionServerSecurity.Decrypt<int>(dto.idDeclaracion, 0),
                DESCRIPCION = dto.descripcion
            };
        }

        public static DeclaracionJuradaModel Map(DeclaracionJuradaEntity entity, IEncryptionServerSecurity encryptionServerSecurity)
        {
            return new DeclaracionJuradaModel()
            {
                idDeclaracion = encryptionServerSecurity.Encrypt(entity.ID_DECLARACION.ToString()),
                descripcion = entity.DESCRIPCION
            };
        }
    }
}
