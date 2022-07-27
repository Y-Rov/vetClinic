using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf.Graphics;

namespace Application.Services.GeneratePDF
{
    public static class PdfParams
    {
        public readonly static PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);

        public readonly static PdfBrush color = PdfBrushes.Black;

    }
}
