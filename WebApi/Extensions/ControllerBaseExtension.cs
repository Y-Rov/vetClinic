using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public static class ControllerBaseExtension
    {
        public static FileStreamResult File(this ControllerBase controllerBase, PdfFileModel pdfFileModel)
        {
            var result = controllerBase.File(pdfFileModel.FileStream, pdfFileModel.ContentType, pdfFileModel.DefaultFileName);
            return result;
        }
    }
}
