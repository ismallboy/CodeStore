using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Amway.OA.MSET
{
    /// <summary>
    /// Execl文档帮助类
    /// 用于处理Execl文件读写等
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 读取Execl文件数据
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>Execl的数据表格</returns>
        public static DataTable ImportFromExcel(string path)
        {
            //获取文件的后缀名
            var ext = Path.GetExtension(path).ToLower();

            //通过后缀名判断Execl的读取版本
            switch (ext)
            {
                case ".xlsx":
                    return ImportFromExcel(path, ExcelVersion.Excel2007);
                default:
                    return ImportFromExcel(path, ExcelVersion.Excel2003);
            }
        }

        /// <summary>
        /// 读取Execl文件数据
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="excelVersion">Execl文件版本类型</param>
        /// <returns>Execl的数据表格</returns>
        public static DataTable ImportFromExcel(string path, ExcelVersion excelVersion)
        {
            string strConn = string.Empty;
            switch (excelVersion)
            {
                case ExcelVersion.Excel2003:
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";Extended Properties=\"Excel 8.0; IMEX=1\";";
                    break;
                case ExcelVersion.Excel2007:
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES\";";//打开2007
                    break;
                default:
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";Extended Properties=\"Excel 8.0; IMEX=1\";";
                    break;
            }

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                using (OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn))
                {
                    using (DataSet myDataSet = new DataSet())
                    {
                        myCommand.Fill(myDataSet);

                        if (myDataSet.Tables.Count > 0)
                        {
                            return myDataSet.Tables[0];
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 通过NPOI读取Execl文件内容.
        /// </summary>
        /// <param name="path">Execl文件地址</param>
        /// <param name="cellCount">列数</param>
        /// <returns>Execl数据集合</returns>
        public static List<List<string>> GetExcelData(string path, int cellCount = 5)
        {
            //打开文件流
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var list = new List<List<string>>();

                //读取文件
                var hssfworkbook = new HSSFWorkbook(stream);
                var sheet = hssfworkbook.GetSheetAt(0);

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null && sheet.GetRow(i).PhysicalNumberOfCells > 0)
                    {
                        var obj = new List<string>();
                        for (var n = 0; n < cellCount; n++)
                        {
                            //读取每一行每一个单元格的数据
                            if (sheet.GetRow(i).Cells.Count > n)
                            {
                                var cell = sheet.GetRow(i).Cells[n];
                                obj.Add(cell.StringCellValue);
                            }
                            else
                            {
                                obj.Add(null);
                            }
                        }
                        if (obj.Count > 0)
                        {
                            list.Add(obj);
                        }
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 通过NPOI读取Execl文件内容.
        /// </summary>
        /// <param name="path">Execl文件地址</param>
        /// <param name="cellCount">列数</param>
        /// <returns>Execl数据集合</returns>
        public static List<List<string>> GetExcelData(Stream stream, int cellCount = 5)
        {
            //打开文件流
            using (stream)
            {
                var list = new List<List<string>>();

                //读取文件
                var hssfworkbook = new HSSFWorkbook(stream);
                var sheet = hssfworkbook.GetSheetAt(0);

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null && sheet.GetRow(i).PhysicalNumberOfCells > 0)
                    {
                        var obj = new List<string>();
                        for (var n = 0; n < cellCount; n++)
                        {
                            //读取每一行每一个单元格的数据
                            if (sheet.GetRow(i).Cells.Count > n)
                            {
                                var cell = sheet.GetRow(i).Cells[n];
                                if (cell.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
                                {
                                    obj.Add(cell.NumericCellValue.ToNullString());
                                }
                                else
                                {
                                    obj.Add(cell.StringCellValue);
                                }
                            }
                            else
                            {
                                obj.Add(null);
                            }
                        }
                        if (obj.Count > 0)
                        {
                            list.Add(obj);
                        }
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 通过NPOI读取Execl文件内容.
        /// </summary>
        /// <param name="path">Execl文件地址</param>
        /// <param name="cellCount">列数</param>
        /// <returns>Execl数据集合</returns>
        public static List<List<string>> ReadExcelData(Stream stream, int cellCount = 4)
        {
            //打开文件流
            using (stream)
            {
                var list = new List<List<string>>();

                //读取文件
                var hssfworkbook = new HSSFWorkbook(stream);
                var sheet = hssfworkbook.GetSheetAt(0);

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null && sheet.GetRow(i).PhysicalNumberOfCells > 0)
                    {
                        var obj = new List<string>();
                        for (var n = 0; n < cellCount; n++)
                        {
                            //读取每一行每一个单元格的数据
                            if (sheet.GetRow(i).Cells.Count > n)
                            {
                                var cell = sheet.GetRow(i).Cells[n];
                                if (cell.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
                                {
                                    obj.Add(cell.NumericCellValue.ToNullString());
                                }
                                else
                                {
                                    obj.Add(cell.StringCellValue);
                                }
                            }
                            else
                            {
                                obj.Add(null);
                            }
                        }
                        if (obj.Count > 0)
                        {
                            list.Add(obj);
                        }
                    }
                }
                return list;
            }
        }
    }

    public enum ExcelVersion
    {
        Excel2003 = 1,
        Excel2007 = 2
    }
}