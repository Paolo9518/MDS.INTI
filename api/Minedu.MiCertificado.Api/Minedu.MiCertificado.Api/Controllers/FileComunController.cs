using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using Minedu.MiCertificado.Api.Utils;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;
using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.Application.Security;

namespace Minedu.MiCertificado.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileComunController : ControllerBase
    {
        private readonly ICertificadoService _certificadoService;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;

        public FileComunController(
            ICertificadoService certificadoService,  
            IHostingEnvironment env,
            IConfiguration configuration,
            IEncryptionServerSecurity encryptionServerSecurity)
        {
            _certificadoService = certificadoService;
            _env = env;
            _configuration = configuration;
            _encryptionServerSecurity=encryptionServerSecurity;
    }


        [HttpPost("uploadFile", Name = "UploadFile")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile body)
        {
            var response = new StatusResponse();






            /*List<string> lstError = new List<string>();
            string extension = null;
            string nombreOriginal = "";
            string nombreArchivo = "";
            long? size = 0;

            var file = HttpContext.Request.Form.Files[0];   
            
            nombreOriginal = UnquoteToken(file.FileName);
            extension = getExtensionArchivo(nombreOriginal);
            nombreArchivo = getFileNameTemp();
            if (extension != null)
                nombreArchivo = nombreArchivo + '.' + extension;
            size = file.Length;

            var folderName = nombreArchivo;

            if (file.Length > 0)
            {
                var fullPath = Path.Combine(getRutaArchivo(),folderName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);

                    //Archivo corrupto
                    if (!ValidarArchivo(extension, stream))
                    {
                        lstError.Add("Error, el archivo no corresponde al tipo especificado.");
                    }
                }
                               
                //validar tamaño
                if (!String.IsNullOrEmpty(size.ToString()) && Convert.ToInt64(size) * 1024 < size)
                {
                    lstError.Add("El tamaño del archivo no es válido");
                }              
            }

            if (lstError.Count == 0)
            {
                response.Success = true;
                response.Data = ReactEncryptationSecurity.Encrypt("1");
                response.Messages.Add("Carga de archivo exitoso.");
            }
            else
            {
                response.Success = false;
                response.Data = ReactEncryptationSecurity.Encrypt("0");
                response.Messages.Add(lstError[0].ToString());
            }*/
            return Ok(response);
        }

        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }

        /*METOO PARA EXTRAER UNA RUTA BASE POR DEFECTO*/
        private string getRutaServidorDefault()
        {
           return Path.Combine(_env.ContentRootPath, getRutaReferencialDefault());
        }

        /*METOO PARA EXTRAER UNA RUTA REFERENCIAL POR DEFECTO*/
        private string getRutaReferencialDefault()
        {
            return "carga/temp/";
        }


        private string getExtensionArchivo(string nombreArchivo)
        {
            if (nombreArchivo != null && nombreArchivo.LastIndexOf(".") > -1)
            {
                return nombreArchivo.Substring(nombreArchivo.LastIndexOf(".") + 1);
            }
            return null;
        }

        /*METOO PARA EXTRAER LA RUTA DEL ARCHIVO*/
        private string getRutaArchivo()
        {
            string path = getRutaServidorDefault();
            if (path != null && !path.Trim().Equals(string.Empty))
            {
                path = _configuration.GetSection("File:FileServer").Value.ToString();
            }
            return path;
        }

        /*METOO PARA GENERAR UN NOMBRE ALEATORIO*/
        private String getFileNameTemp()
        {
            Random rnd = new Random();
            String name = "file_" + rnd.Next(0, 1000000) + "_" + rnd.Next(0, 1000000) + "_" + rnd.Next(0, 1000000);// +".tmp";
            return name;
        }

        private bool ValidarPDF(Stream fileStream)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            using (var br = new BinaryReader(fileStream))
            {
                var buffer = br.ReadBytes(5);
                var enc = new ASCIIEncoding();
                var header = enc.GetString(buffer);
                if (buffer[0] == 0x25 && buffer[1] == 0x50
                    && buffer[2] == 0x44 && buffer[3] == 0x46)
                {
                    return header.StartsWith("%PDF-");
                }
            }
            return false;
        }

        protected bool ValidarArchivo(string extension, Stream fileStream)
        {
            bool flag = false;
            switch (extension)
            {                
                case "pdf":
                    {
                        flag = ValidarPDF(fileStream);
                    }
                    break;
            }

            return flag;
        }

    }
}