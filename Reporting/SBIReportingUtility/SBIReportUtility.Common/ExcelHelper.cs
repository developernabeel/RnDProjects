using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// Converts Datatable to Excel file.
        /// </summary>
        /// <param name="dataTable">System.DataTable object</param>
        /// <returns>Excel file in bytes</returns>
        public static byte[] GetExcelFile(DataTable dataTable)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(memory, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                AddStyleSheet(spreadsheetDocument);

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>("sheet1");
                worksheetPart.Worksheet = new Worksheet();

                Columns columns = new Columns();
                columns.Append(CreateColumnData(1, (UInt32)dataTable.Columns.Count, 20));
                worksheetPart.Worksheet.Append(columns);

                // Append a new worksheet and associate it with the workbook.
                var sheet = new Sheet()
                {
                    Id = "sheet1",
                    SheetId = 1,
                    Name = "Sheet 1"
                };
                sheets.Append(sheet);

                SheetData sheetData = new SheetData();
                Row headerRow = new Row();

                if (dataTable.Rows.Count == 0)
                {
                    headerRow.AppendChild(ConstructCell("No Data", CellValues.String));
                    sheetData.AppendChild(headerRow);
                }
                else
                {
                    foreach (DataColumn dtColumn in dataTable.Columns)
                    {
                        headerRow.AppendChild(ConstructCell(dtColumn.ColumnName, CellValues.String));
                    }
                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dtRow in dataTable.Rows)
                    {
                        Row dataRow = new Row();
                        foreach (DataColumn dtColumn in dataTable.Columns)
                        {
                            string cellData = dtRow[dtColumn].ToString();
                            dataRow.AppendChild(ConstructCell(cellData, CellValues.String));
                        }
                        sheetData.AppendChild(dataRow);
                    }
                }

                worksheetPart.Worksheet.Append(sheetData);

                workbookpart.Workbook.Save();
                spreadsheetDocument.Close();

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Creates WorkbookStylesPart with 3 different style indexes.
        /// </summary>
        /// <param name="spreadsheet">SpreadsheetDocument</param>
        /// <returns>WorkbookStylesPart object</returns>
        public static WorkbookStylesPart AddStyleSheet(SpreadsheetDocument spreadsheet)
        {
            WorkbookStylesPart stylesheet = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();

            Stylesheet workbookstylesheet = new Stylesheet();
            Font font0 = new Font();         // Default font
            Font font1 = new Font();         // Bold font
            Bold bold = new Bold();
            font1.Append(bold);

            Fonts fonts = new Fonts();      // <APENDING Fonts>
            fonts.Append(font0);
            fonts.Append(font1);

            // <Fills>
            Fill fill0 = new Fill();        // Default fill
            Fills fills = new Fills();      // <APENDING Fills>
            fills.Append(fill0);

            // <Borders>
            Border border0 = new Border();     // Defualt border
            Borders borders = new Borders();    // <APENDING Borders>
            borders.Append(border0);

            // <CellFormats>
            CellFormat cellformat0 = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }; // Default style : Mandatory | Style ID =0
            CellFormat cellformat1 = new CellFormat() { FontId = 1 };  // Style with Bold text ; Style ID = 1
            CellFormat cellformat2 = new CellFormat() { FontId = 0 };
            cellformat2.ApplyAlignment = true;
            if (cellformat2.Alignment == null)
                cellformat2.Alignment = new Alignment() { WrapText = true };
            Alignment a1 = cellformat2.Alignment;
            if (a1.WrapText == null || a1.WrapText.Value == false)
                a1.WrapText = new BooleanValue(true);

            // <APENDING CellFormats>
            CellFormats cellformats = new CellFormats();
            cellformats.Append(cellformat0);
            cellformats.Append(cellformat1);
            cellformats.Append(cellformat2);

            // Append FONTS, FILLS , BORDERS & CellFormats to stylesheet
            workbookstylesheet.Append(fonts);
            workbookstylesheet.Append(fills);
            workbookstylesheet.Append(borders);
            workbookstylesheet.Append(cellformats);

            // Finalize
            stylesheet.Stylesheet = workbookstylesheet;
            stylesheet.Stylesheet.Save();

            return stylesheet;
        }

        /// <summary>
        /// Creates column with specified width for given range.
        /// </summary>
        /// <param name="startColIndex">Start column index</param>
        /// <param name="endColIndex">End column index</param>
        /// <param name="colWidth">Column width</param>
        /// <returns>Column object</returns>
        public static Column CreateColumnData(UInt32 startColIndex, UInt32 endColIndex, double colWidth)
        {
            Column col = new Column();
            col.Min = startColIndex;
            col.Max = endColIndex;
            col.Width = colWidth;
            col.CustomWidth = true;
            return col;
        }

        /// <summary>
        /// Returns a new Cell object.
        /// </summary>
        /// <param name="value">Cell value</param>
        /// <param name="dataType">Cell data type</param>
        /// <param name="styleIndex">Style Index of added stylesheet</param>
        /// <returns>Cell object</returns>
        public static Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
    }
}
