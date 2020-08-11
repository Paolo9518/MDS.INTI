using MDS.Inventario.Api.Application.Contracts.Security;
using MDS.Inventario.Api.Application.Entities.Models.Constancia;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Mappers.Constancia
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
