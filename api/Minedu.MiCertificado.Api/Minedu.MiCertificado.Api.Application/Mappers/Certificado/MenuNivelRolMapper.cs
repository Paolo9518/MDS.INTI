using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System;

namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public static class MenuNivelRolMapper
    {
        public static MenuNivelRolEntity Map(MenuNivelRolModel dto)
        {
            return new MenuNivelRolEntity()
            {
                ID_ROL = dto.idRol,
                ID_MENU_NIVEL = dto.idMenuNivel,
                DSC_MENU_NIVEL = dto.dscMenuNivel,
                DSC_ROL = dto.dscRol
            };
        }

        public static MenuNivelRolModel Map(MenuNivelRolEntity entity)
        {
            return new MenuNivelRolModel()
            {
                idRol = entity.ID_ROL,
                idMenuNivel = entity.ID_MENU_NIVEL,
                dscMenuNivel = entity.DSC_MENU_NIVEL,
                dscRol = entity.DSC_ROL
            };
        }
    }
}
