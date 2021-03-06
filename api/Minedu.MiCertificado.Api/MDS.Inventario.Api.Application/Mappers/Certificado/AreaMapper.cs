﻿using MDS.Inventario.Api.Application.Contracts.Security;
using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;
using System;

namespace MDS.Inventario.Api.Application.Mappers.Certificado
{
    public static class AreaMapper
    {
        public static AreaCertificadoEntity Map(AreaModel dto)
        {
            return new AreaCertificadoEntity()
            {
                ID_AREA = dto.idArea,
                DSC_AREA = dto.descripcionArea,
                ID_NIVEL = dto.nivel,
                ID_TIPO_AREA = dto.codigoTipoArea,
                ANIO_INICIO=dto.anioInicio,
                ANIO_FIN = dto.anioFin,
                ID_DISENIO=dto.idDisenio
            };
        }
 
        public static AreaModel Map(AreaCertificadoEntity entity)
        {
            return new AreaModel()
            {
                idArea = entity.ID_AREA,
                descripcionArea = entity.DSC_AREA.ToString(),
                nivel = entity.ID_NIVEL,
                codigoTipoArea = entity.ID_TIPO_AREA,
                anioInicio=entity.ANIO_INICIO,
                anioFin=entity.ANIO_FIN,
                idDisenio = entity.ID_DISENIO
            };
        }
    }
}
