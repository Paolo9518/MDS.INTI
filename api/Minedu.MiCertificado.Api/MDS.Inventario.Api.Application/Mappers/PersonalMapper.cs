using MDS.Inventario.Api.Application.Entities.Models;
using MDS.Inventario.Api.DataAccess.Contracts.Entities;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;

namespace MDS.Inventario.Api.Application.Mappers
{
    public static class PersonalMapper
    {
        public static PersonalEntity Map(PersonalExtends dto)
        {
            return new PersonalEntity()
            {
                ID_PERSONAL = dto.IdPersonal,
                ID_ENTIDAD = dto.IdEntidad,
                ID_TIPO_DOCUMENTO = dto.IdTipoDocumento,
                NUMERO_DOCUMENTO = dto.NumeroDocumento,
                APELLIDO_PATERNO = dto.ApellidoPaterno,
                APELLIDO_MATERNO = dto.ApellidoMaterno,
                NOMBRES = dto.Nombres,
                NOMBRE_COMPLETO = dto.NombreCompleto,
                FECHA_NACIMIENTO = dto.FechaNacimiento,
                FECHA_INGRESO = dto.FechaIngreso,
                NUM_LICENCIA_CONDUCIR = dto.NumLicenciaConducir,
                SEXO = dto.Sexo,
                ID_CARGO = dto.IdCargo,
                ID_AREA = dto.IdArea,
                TELEFONO_PERSONAL = dto.TelefonoPersonal,
                TELEFONO_TRABAJO = dto.TelefonoTrabajo,
                ID_ROL = dto.IdRol,
                DSC_ROL = dto.DscRol
            };
        }

        public static PersonalExtends Map(PersonalEntity entity)
        {
            return new PersonalExtends()
            {
                IdPersonal = entity.ID_PERSONAL,
                IdEntidad = entity.ID_ENTIDAD,
                IdTipoDocumento = entity.ID_TIPO_DOCUMENTO,
                NumeroDocumento = entity.NUMERO_DOCUMENTO,
                ApellidoPaterno = entity.APELLIDO_PATERNO,
                ApellidoMaterno = entity.APELLIDO_MATERNO,
                Nombres = entity.NOMBRES,
                NombreCompleto = entity.NOMBRE_COMPLETO,
                FechaNacimiento = entity.FECHA_NACIMIENTO,
                FechaIngreso = entity.FECHA_INGRESO,
                NumLicenciaConducir = entity.NUM_LICENCIA_CONDUCIR,
                Sexo = entity.SEXO,
                IdCargo = entity.ID_CARGO,
                IdArea = entity.ID_AREA,
                TelefonoPersonal = entity.TELEFONO_PERSONAL,
                TelefonoTrabajo = entity.TELEFONO_TRABAJO,
                DscRol = entity.DSC_ROL,
                IdRol = entity.ID_ROL
            };
        }
    }
}
