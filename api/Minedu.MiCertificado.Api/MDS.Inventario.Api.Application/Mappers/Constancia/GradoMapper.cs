using MDS.Inventario.Api.Application.Entities.Models.Constancia;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Mappers.Constancia
{
    public class GradoMapper
    {
        public static GradoEntity Map(GradoModel dto)
        {
            return new GradoEntity()
            {
                ID_CONSTANCIA_GRADO = dto.idConstanciaGrado,
                ID_SOLICITUD = dto.idSolicitud,
                ID_GRADO = dto.idGrado,
                DSC_GRADO = dto.dscGrado,
                CORR_ESTADISTICA = dto.corrEstadistica,
                ID_ANIO = dto.idAnio,
                COD_MOD = dto.codMod,
                ANEXO = dto.anexo,
                SITUACION_FINAL = dto.situacionFinal
            };
        }

        public static GradoModel Map(GradoEntity entity)
        {
            return new GradoModel()
            {
                idConstanciaGrado = entity.ID_CONSTANCIA_GRADO,
                idSolicitud = entity.ID_SOLICITUD,
                idGrado = entity.ID_GRADO,
                dscGrado = entity.DSC_GRADO,
                corrEstadistica = entity.CORR_ESTADISTICA,
                idAnio = entity.ID_ANIO,
                codMod = entity.COD_MOD,
                anexo = entity.ANEXO,
                situacionFinal = entity.SITUACION_FINAL
            };
        }
    }
}
