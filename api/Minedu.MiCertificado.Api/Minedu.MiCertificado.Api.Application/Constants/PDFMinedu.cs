using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Minedu.MiCertificado.Api.Application.Constants
{
    public static class PDFMinedu
    {
        public static Table getTable(float[] columns)
        {
            Table table = new Table(UnitValue.CreatePercentArray(columns));
            table.SetWidth(UnitValue.CreatePercentValue(100));
            return table;
        }

        public static Cell getCell(string texto,
            float sizeFuente,
            bool strong = false,
            int rowSpan = 1,
            int colSpan = 1,
            TextAlignment horizontal = TextAlignment.CENTER,
            PdfFont font = null,
            bool border = true,
            VerticalAlignment vertical = VerticalAlignment.MIDDLE)
        {
            if (font == null) font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Cell celda = new Cell(rowSpan, colSpan);
            celda.Add(getParagraph(texto,
                sizeFuente,
                strong,
                font,
                horizontal))
                .SetVerticalAlignment(vertical);

            if (!border) celda.SetBorder(Border.NO_BORDER);

            return celda;
        }

        public static Paragraph getParagraph(string texto,
            float sizeFuente,
            bool strong,
            PdfFont font = null,
            TextAlignment horizontal = TextAlignment.CENTER)
        {
            if (font == null) font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Paragraph paragraph = new Paragraph(texto)
                .SetFontSize(sizeFuente)
                .SetFont(font)
                .SetTextAlignment(horizontal);

            if (strong) paragraph.SetBold();

            return paragraph;
        }

        public static void addEspacios(Document element, float sizeFuente, int numberLines = 1)
        {
            for (int i = 1; i <= numberLines; i++)
                element.Add(getParagraph("", sizeFuente, false));
        }

        public static Color getColor(string _color)
        {
            return WebColors.GetRGBColor(_color);
        }
    }
}
