using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Дипломчик
{
    public static class Reports
    {
        private static Paragraph GetParagraph(
            string text, 
            int textSize = 28, 
            string textFamily = "Times New Roman", 
            bool isBold = false,
            bool justifyCenter = false
        )
        {
            RunProperties prop = new RunProperties();
            prop.AppendChild(new RunFonts() { Ascii = textFamily, HighAnsi = textFamily, EastAsia = textFamily, ComplexScript = textFamily });
            prop.AppendChild(new FontSize() { Val = textSize.ToString() });
            if (isBold)
            {
                prop.AppendChild(new Bold());
            }

            ParagraphProperties paragraphProperties = new ParagraphProperties();

            if (justifyCenter)
            {
                paragraphProperties.AppendChild(new Justification() { Val = JustificationValues.Center });
            }
            return new Paragraph(
                new Run(
                    prop,
                    new Text(text)
                ),
                paragraphProperties
            );
        }

        private static Table CreateTable(params string[] cols)
        {
            Table table = new Table(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 }
                ),
                new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
            );
            


            TableRow titleRow = table.AppendChild(new TableRow());

            foreach (string col in cols)
            {
                titleRow.AppendChild(new TableCell(GetParagraph(col)));
            }
            return table;
        }

        public static void CreateUniformReport(string filename, DateTime startDate, DateTime endDate)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filename, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                body.AppendChild(GetParagraph(
                    "Отчет обмундирования",
                    34,
                    isBold: true,
                    justifyCenter: true
                ));
                body.AppendChild(GetParagraph($"С {startDate.ToString("dd.MM.yyyy")} по {endDate.ToString("dd.MM.yyyy")}"));

                Table table = CreateTable(" ", "Номер", "Название", "Срок ношения (мес.)", "Выдано");

                List<Uniform> uniforms = DBAdapter.GetAll(Uniform.GetNewFunc(), "Uniforms");

                foreach (Uniform u in uniforms)
                {
                    table.AppendChild(new TableRow(
                        new TableCell(GetParagraph(u.Id.ToString())),
                        new TableCell(GetParagraph(u.Number.ToString())),
                        new TableCell(GetParagraph(u.Name)),
                        new TableCell(GetParagraph(u.Period.ToString())),
                        new TableCell(GetParagraph(
                            DBAdapter.GetAll(Issued.GetNewFunc(),
                                "Issueds", $" WHERE [Uniform] = {u.Id} AND [Returned] = 0")
                            .FindAll(x => x.IssuedDate >= startDate && x.IssuedDate <= endDate).Count.ToString()
                        ))
                    ));
                }
                body.AppendChild(table);

                body.AppendChild(GetParagraph($"Отчет от {DateTime.Now.ToString("dd.MM.yyyy")}"));
            }
        }

        private static Table CreateIssuedTable(List<Issued> issueds, DateTime startDate, DateTime endDate)
        {
            Table table = CreateTable(" ", "Форма", "Сотрудник", "Дата выдачи", "Дата сдачи");
            foreach (Issued issued in issueds)
            {
                if (issued.IssuedDate >= startDate && issued.IssuedDate <= endDate)
                {
                    table.AppendChild(new TableRow(
                        new TableCell(GetParagraph(issued.Id.ToString())),
                        new TableCell(GetParagraph(issued.Uniform.ToString())),
                        new TableCell(GetParagraph(issued.Employee.ToString())),
                        new TableCell(GetParagraph(issued.IssuedDateStr)),
                        new TableCell(GetParagraph(issued.ToDateStr))
                    ));
                }
            }

            return table;
        }

        public static void CreateIssuedReport(string filename, DateTime startDate, DateTime endDate, bool returned)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filename, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                body.AppendChild(GetParagraph(
                    returned ? "Отчет сданного обмундирования" : "Отчет не сданного обмундирования",
                    34,
                    isBold: true,
                    justifyCenter: true
                ));

                body.AppendChild(GetParagraph($"С {startDate.ToString("dd.MM.yyyy")} по {endDate.ToString("dd.MM.yyyy")}"));

                body.AppendChild(CreateIssuedTable(
                    DBAdapter.GetAll(Issued.GetNewFunc(), "Issueds", $" WHERE [Returned] = {(returned ? 1 : 0)}"),
                    startDate,
                    endDate
                ));

                body.AppendChild(GetParagraph($"Отчет от {DateTime.Now.ToString("dd.MM.yyyy")}"));
            }
        }

        public static void CreateNeedReturnUniformReport(string filename)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filename, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                body.AppendChild(GetParagraph(
                    "Отчет о сдаче обмундирования в текущем месяце",
                    34,
                    isBold: true,
                    justifyCenter: true
                ));

                int curMonth = DateTime.Now.Month, curYear = DateTime.Now.Year;
                body.AppendChild(GetParagraph($"В месяце {curMonth} года {curYear} требуется сдать обмундирование следующим сотрудникам:"));

                foreach (Issued issued in DBAdapter.GetAll(Issued.GetNewFunc(), "Issueds", " WHERE [Returned] = 0"))
                {
                    if (((DateTime)issued.ToDate).Month == curMonth && ((DateTime)issued.ToDate).Year == curYear)
                    {
                        body.AppendChild(GetParagraph($"{issued.Employee} - {issued.Uniform} - {((DateTime)issued.ToDate).ToString("dd.MM.yyyy")}"));
                    }
                }

                body.AppendChild(GetParagraph($"Отчет от {DateTime.Now.ToString("dd.MM.yyyy")}"));
            }
        }
    }
}
