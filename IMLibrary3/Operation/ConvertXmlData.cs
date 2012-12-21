using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Web;
using System.Net;

namespace IMLibrary3.Operation
{
    /// <summary>
    /// 把DataSet、DataTable、DataView格式转换成XML字符串、XML文件
    /// </summary>
    public class DataToXml
    {
        /// <summary>
        /// 将DataTable对象转换成XML字符串
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataTable dt)
        {
            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    UnicodeEncoding ucode = new UnicodeEncoding();
                    string returnValue = ucode.GetString(temp).Trim();
                    return returnValue;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 将DataSet对象中指定的Table转换成XML字符串
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableIndex">DataSet对象中的Table索引</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataSet ds, int tableIndex)
        {
            if (tableIndex != -1)
            {
                return CDataToXml(ds.Tables[tableIndex]);
            }
            else
            {
                return CDataToXml(ds.Tables[0]);
            }
        }
        
        /// <summary>
        /// 将DataSet对象转换成XML字符串
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataSet ds)
        {
            return CDataToXml(ds, -1);
        }
        
        /// <summary>
        /// 将DataView对象转换成XML字符串
        /// </summary>
        /// <param name="dv">DataView对象</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataView dv)
        {
            return CDataToXml(dv.Table);
        } 
        
        /// <summary>
        /// 将DataSet对象数据保存为XML文件
        /// </summary>
        /// <param name="dt">DataSet</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>bool值</returns>
        //public static bool CDataToXmlFile(DataTable dt, string xmlFilePath)
        //{
            //if ((dt != null) && (!string.IsNullOrEmpty(xmlFilePath)))
            //{
            //    string path = HttpContext.Current.Server.MapPath(xmlFilePath);
            //    MemoryStream ms = null;
            //    XmlTextWriter XmlWt = null;
            //    try
            //    {
            //        ms = new MemoryStream();
            //        //根据ms实例化XmlWt
            //        XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
            //        //获取ds中的数据
            //        dt.WriteXml(XmlWt);
            //        int count = (int)ms.Length;
            //        byte[] temp = new byte[count];
            //        ms.Seek(0, SeekOrigin.Begin);
            //        ms.Read(temp, 0, count);
            //        //返回Unicode编码的文本
            //        UnicodeEncoding ucode = new UnicodeEncoding();
            //        //写文件
            //        StreamWriter sw = new StreamWriter(path);
            //        sw.WriteLine("<?xml version='1.0' encoding='utf-8'?>");
            //        sw.WriteLine(ucode.GetString(temp).Trim());
            //        sw.Close();
            //        return true;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        throw ex;
            //    }
            //    finally
            //    {
            //        //释放资源
            //        if (XmlWt != null)
            //        {
            //            XmlWt.Close();
            //            ms.Close();
            //            ms.Dispose();
            //        }
            //    }
            //}
            //else
            //{
            //    return false;
            //}
        //}
       
        /// <summary>
        /// 将DataSet对象中指定的Table转换成XML文件
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableIndex">DataSet对象中的Table索引</param>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>bool]值</returns>
        //public static bool CDataToXmlFile(DataSet ds, int tableIndex, string xmlFilePath)
        //{
        //    if (tableIndex != -1)
        //    {
        //        return CDataToXmlFile(ds.Tables[tableIndex], xmlFilePath);
        //    }
        //    else
        //    {
        //        return CDataToXmlFile(ds.Tables[0], xmlFilePath);
        //    }
        //}
        
        /// <summary>
        /// 将DataSet对象转换成XML文件
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>bool]值</returns>
        //public static bool CDataToXmlFile(DataSet ds, string xmlFilePath)
        //{
        //    return CDataToXmlFile(ds, -1, xmlFilePath);
        //}
        
        /// <summary>
        /// 将DataView对象转换成XML文件
        /// </summary>
        /// <param name="dv">DataView对象</param>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>bool]值</returns>
        //public static bool CDataToXmlFile(DataView dv, string xmlFilePath)
        //{
        //    return CDataToXmlFile(dv.Table, xmlFilePath);
        //}
    }


    /// <summary>
    /// XML形式的字符串、XML文江转换成DataSet、DataTable格式
    /// </summary>
    public class XmlToData
    {
        /// <summary>
        /// 将Xml内容字符串转换成DataSet对象
        /// </summary>
        /// <param name="xmlStr">Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <param name="tableIndex">Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr, int tableIndex)
        {
            return CXmlToDataSet(xmlStr).Tables[tableIndex];
        }
       
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr)
        {
            return CXmlToDataSet(xmlStr).Tables[0];
        }
        
        /// <summary>
        /// 读取Xml文件信息,并转换成DataSet对象
        /// </summary>
        /// <remarks>
        /// DataSet ds = new DataSet();
        /// ds = CXmlFileToDataSet("/XML/upload.xml");
        /// </remarks>
        /// <param name="xmlFilePath">Xml文件地址</param>
        /// <returns>DataSet对象</returns>
        //public static DataSet CXmlFileToDataSet(string xmlFilePath)
        //{
        //    if (!string.IsNullOrEmpty(xmlFilePath))
        //    {
        //        string path = HttpContext.Current.Server.MapPath(xmlFilePath);
        //        StringReader StrStream = null;
        //        XmlTextReader Xmlrdr = null;
        //        try
        //        {
        //            XmlDocument xmldoc = new XmlDocument();
        //            //根据地址加载Xml文件
        //            xmldoc.Load(path);

        //            DataSet ds = new DataSet();
        //            //读取文件中的字符流
        //            StrStream = new StringReader(xmldoc.InnerXml);
        //            //获取StrStream中的数据
        //            Xmlrdr = new XmlTextReader(StrStream);
        //            //ds获取Xmlrdr中的数据
        //            ds.ReadXml(Xmlrdr);
        //            return ds;
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //        finally
        //        {
        //            //释放资源
        //            if (Xmlrdr != null)
        //            {
        //                Xmlrdr.Close();
        //                StrStream.Close();
        //                StrStream.Dispose();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <param name="tableIndex">Table索引</param>
        /// <returns>DataTable对象</returns>
        //public static DataTable CXmlToDataTable(string xmlFilePath, int tableIndex)
        //{
        //    return CXmlFileToDataSet(xmlFilePath).Tables[tableIndex];
        //}
        
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <returns>DataTable对象</returns>
        //public static DataTable CXmlToDataTable(string xmlFilePath)
        //{
        //    return CXmlFileToDataSet(xmlFilePath).Tables[0];
        //}
    }


}
