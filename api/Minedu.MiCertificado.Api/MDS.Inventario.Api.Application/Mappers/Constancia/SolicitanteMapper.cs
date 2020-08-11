using MDS.Inventario.Api.Application.Entities.Models.Constancia;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia;

namespace MDS.Inventario.Api.Application.Mappers.Constancia
{
    public class SolicitanteMapper
    {
        public static SolicitanteEntity Map(SolicitanteModel dto)
        {
            return new SolicitanteEntity()
            {
                ID_SOLICITANTE = dto.idSolicitante,
                ID_PERSONA = dto.idPersona,
                ID_TIPO_DOCUMENTO = dto.idTipoDocumento,
                NUMERO_DOCUMENTO = dto.numeroDocumento,
                APELLIDO_PATERNO = dto.apellidoPaterno,
                APELLIDO_MATERNO = dto.apellidoMaterno,
                NOMBRES = dto.nombres,
                TELEFONO_CELULAR = dto.telefonoCelular,
                CORREO_ELECTRONICO = dto.correoElectronico,
                UBIGEO = dto.ubigeo,
                DEPARTAMENTO = dto.departamento,
                PROVINCIA = dto.provincia,
                DISTRITO = dto.distrito
            };
        }

        public static SolicitanteModel Map(SolicitanteEntity entity)
        {
            return new SolicitanteModel()
            {
                idSolicitante = entity.ID_SOLICITANTE,
                idPersona = entity.ID_PERSONA,
                idTipoDocumento = entity.ID_TIPO_DOCUMENTO,
                numeroDocumento = entity.NUMERO_DOCUMENTO,
                apellidoPaterno = entity.APELLIDO_PATERNO,
                apellidoMaterno = entity.APELLIDO_MATERNO,
                nombres = entity.NOMBRES,
                telefonoCelular = entity.TELEFONO_CELULAR,
                correoElectronico = entity.CORREO_ELECTRONICO,
                ubigeo = entity.UBIGEO,
                departamento = entity.DEPARTAMENTO,
                provincia = entity.PROVINCIA,
                distrito = entity.DISTRITO
            };
        }
    }
}
