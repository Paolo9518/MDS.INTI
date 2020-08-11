using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System;
using System.Collections.Generic;
using System.Text;


namespace Minedu.MiCertificado.Api.Application.Mappers.Certificado
{
    public static class EstudianteCertificadoMapper
    {
        public static EstudianteCertificadoEntity Map(EstudianteCertificadoModel dto)
        {
            return new EstudianteCertificadoEntity()
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

        public static EstudianteCertificadoModel Map(EstudianteCertificadoEntity entity)
        {
            return new EstudianteCertificadoModel()
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
