using NPOI.SS.UserModel;

using System;

namespace CAF.Core.Extensions
{
    public static class ExcelExtensions
    {
        public static void SetCellValue(this IRow row, int column, string value)
        {
            var cell = row.GetCell(column);
            if (cell == null)
                cell = row.CreateCell(column);

            cell.SetCellValue(value);
        }
        public static T GetCellStringEnum<T>(this ICell cell) where T : Enum
        {
            if (cell != null)
            {
                return (cell.StringCellValue ?? string.Empty).ToEnumByDisplayName<T>();
            }
            else return default;
        }
        public static IRow GetOrCreate(this ISheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null)
                row = sheet.CreateRow(rowIndex);

            return row;
        }

        public static string GetFormattedCellValue(this ICell cell, IFormulaEvaluator eval = null)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue;

                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            DateTime date = cell.DateCellValue;
                            ICellStyle style = cell.CellStyle;
                            return date.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString();
                        }

                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "TRUE" : "FALSE";

                    case CellType.Formula:
                        if (eval != null)
                            return GetFormattedCellValue(eval.EvaluateInCell(cell));
                        else
                            return cell.CellFormula;

                    case CellType.Error:
                        return FormulaError.ForInt(cell.ErrorCellValue).String;
                }
            }
            // null or blank cell, or unknown cell type
            return string.Empty;
        }
    }
}
