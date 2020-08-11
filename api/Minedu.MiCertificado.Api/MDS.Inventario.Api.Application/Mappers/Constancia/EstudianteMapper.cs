using MDS.Inventario.Api.Application.Entities.Models.Constancia;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Mappers.Constancia
{
    public static class EstudianteMapper
    {
        public static EstudianteEntity Map(EstudianteModel dto)
        {
            return new EstudianteEntity()
            {
                ID_ESTUDIANTE = dto.idEstudiante,
                ID_PERSONA = dto.idPersona,
                ID_TIPO_DOCUMENTO = dto.idTipoDocumento,
                NUMERO_DOCUMENTO = dto.numeroDocumento,
                APELLIDO_PATERNO = dto.apellidoPaterno,
                APELLIDO_MATERNO = dto.apellidoMaterno,
                NOMBRES = dto.nombres,
                UBIGEO = dto.ubigeo,
                DEPARTAMENTO = dto.departamento,
                PROVINCIA = dto.provincia,
                DISTRITO = dto.distrito
            };
        }

        public static EstudianteModel Map(EstudianteEntity entity)
        {
            return new EstudianteModel()
            {
                idEstudiante = entity.ID_ESTUDIANTE,
                idPersona = entity.ID_PERSONA,
                idTipoDocumento = entity.ID_TIPO_DOCUMENTO,
                numeroDocumento = entity.NUMERO_DOCUMENTO,
                apellidoPaterno = entity.APELLIDO_PATERNO,
                apellidoMaterno = entity.APELLIDO_MATERNO,
                nombres = entity.NOMBRES,
                ubigeo = entity.UBIGEO,
                departamento = entity.DEPARTAMENTO,
                provincia = entity.PROVINCIA,
                distrito = entity.DISTRITO
            };
        }
    }
}
