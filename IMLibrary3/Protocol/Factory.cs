using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using IMLibrary3.Protocol;

namespace IMLibrary3.Protocol
{
    /// <summary>
    /// 消息对像加工厂
    /// </summary>
    public sealed class Factory
    {

    
        /// <summary>
        /// 创建XML消息
        /// </summary>
        /// <param name="e">Element</param>
        /// <returns></returns>
        public static string CreateXMLMsg(object  e)
        {
            MemoryStream fileStream = new MemoryStream();
            XmlTextWriter textWriter = new XmlTextWriter(fileStream, Encoding.Default);
            textWriter.WriteStartDocument();

            try
            {

                textWriter.WriteStartElement(e.GetType().ToString());

                PropertyInfo[] propertys = e.GetType().GetProperties();
                foreach (PropertyInfo p in propertys)
                {
                    if (p.PropertyType == typeof(String)
                        || p.PropertyType == typeof(bool)
                        || p.PropertyType == typeof(int)
                        || p.PropertyType == typeof(Single)
                        || p.PropertyType == typeof(float)
                        || p.PropertyType == typeof(byte)
                        || p.PropertyType.IsEnum
                        || p.PropertyType == typeof(short)
                        || p.PropertyType == typeof(long)
                        || p.PropertyType == typeof(sbyte)
                        || p.PropertyType == typeof(ushort)
                        || p.PropertyType == typeof(uint)
                        || p.PropertyType == typeof(ulong)
                        || p.PropertyType == typeof(double)
                        || p.PropertyType == typeof(decimal)
                        || p.PropertyType == typeof(char)
                        || p.PropertyType == typeof(System.Net.IPAddress)
                        )
                    {
                        if (p.GetValue(e, null) != null
                            && (p.GetValue(e, null).ToString() != ""
                            || p.GetValue(e, null).ToString().ToLower() != "false"
                            || p.GetValue(e, null).ToString().ToLower() != "0"))
                            textWriter.WriteAttributeString(p.Name, p.GetValue(e, null).ToString());
                    }
                    else if (p.PropertyType == typeof(byte[]))
                    {
                        if (p.GetValue(e, null) != null)
                        {
                            textWriter.WriteAttributeString(p.Name, Convert.ToBase64String((byte[])p.GetValue(e, null)));
                        }
                    } 
                }

                if(e is Element )
                foreach (Object Obj in (e as Element).Data)
                {
                    textWriter.WriteStartElement(Obj.GetType().ToString());

                    propertys = Obj.GetType().GetProperties();
                    foreach (PropertyInfo p in propertys)
                    {
                        if (p.PropertyType == typeof(String)
                            || p.PropertyType == typeof(bool)
                            || p.PropertyType == typeof(int)
                            || p.PropertyType == typeof(Single)
                            || p.PropertyType == typeof(float)
                            || p.PropertyType == typeof(byte)
                            || p.PropertyType.IsEnum
                            || p.PropertyType == typeof(short)
                            || p.PropertyType == typeof(long)
                            || p.PropertyType == typeof(sbyte)
                            || p.PropertyType == typeof(ushort)
                            || p.PropertyType == typeof(uint)
                            || p.PropertyType == typeof(ulong)
                            || p.PropertyType == typeof(double)
                            || p.PropertyType == typeof(decimal)
                            || p.PropertyType == typeof(char)
                            || p.PropertyType == typeof(System.Net.IPAddress)
                            )
                        {
                            if (p.GetValue(Obj, null) != null
                                && (p.GetValue(Obj, null).ToString() != ""
                                || p.GetValue(Obj, null).ToString().ToLower() != "false"
                                || p.GetValue(Obj, null).ToString().ToLower() != "0"))
                                textWriter.WriteAttributeString(p.Name, p.GetValue(Obj, null).ToString());
                        }
                        else if (p.PropertyType == typeof(byte[]))
                        {
                            if (p.GetValue(Obj, null) != null)
                            {
                                textWriter.WriteAttributeString(p.Name, Convert.ToBase64String((byte[])p.GetValue(Obj, null)));
                            }
                        } 
                    }
                    textWriter.WriteEndElement();
                }
            }
            catch
            {

            }
            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();

            return IMLibrary3.Operation.TextEncoder.bytesToText(fileStream.ToArray()).Replace("<?xml version=\"1.0\" encoding=\"gb2312\"?>", "");
        }

        #region 从XML中获取一个对像
        /// <summary>
        /// 从XML中获取一个对像 
        /// </summary>
        public static object CreateInstanceObject(string xmlStr)
        {
            object obj = null;
            try
            {
                XmlReader xmlReader = XmlTextReader.Create(new StringReader(xmlStr));
                if (xmlReader.Read() && xmlReader.NodeType == XmlNodeType.Element)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Type type = assembly.GetType(xmlReader.Name);
                    obj = System.Activator.CreateInstance(type);
                    if (obj == null) return null;

                    PropertyInfo[] propertys = obj.GetType().GetProperties();
                    if (xmlReader.HasAttributes)
                        foreach (PropertyInfo p in propertys)
                        {
                            if (xmlReader.MoveToAttribute(p.Name))
                            {
                                if (p.PropertyType == typeof(String))
                                    p.SetValue(obj, xmlReader.ReadContentAsString(), null);
                                else if (p.PropertyType == typeof(bool))
                                    p.SetValue(obj, bool.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(int))
                                    p.SetValue(obj, int.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(Single))
                                    p.SetValue(obj, Single.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(float))
                                    p.SetValue(obj, float.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType.IsEnum)
                                    p.SetValue(obj, Enum.Parse(p.PropertyType, xmlReader.ReadContentAsString(), true), null);
                                else if (p.PropertyType == typeof(short))
                                    p.SetValue(obj, short.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(long))
                                    p.SetValue(obj, long.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(byte))
                                    p.SetValue(obj, byte.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(sbyte))
                                    p.SetValue(obj, sbyte.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(ushort))
                                    p.SetValue(obj, ushort.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(uint))
                                    p.SetValue(obj, uint.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(ulong))
                                    p.SetValue(obj, ulong.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(double))
                                    p.SetValue(obj, double.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(decimal))
                                    p.SetValue(obj, decimal.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(char))
                                    p.SetValue(obj, char.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(System.Net.IPAddress))
                                    p.SetValue(obj, System.Net.IPAddress.Parse(xmlReader.ReadContentAsString()), null);
                                else if (p.PropertyType == typeof(byte[]))
                                    p.SetValue(obj, Convert.FromBase64String(xmlReader.ReadContentAsString()), null);
                            }
                        }
                }
            }
            catch { }
            return obj;
         }
        #endregion

        #region 从XML中获取所需要的对像(对像在返回值集合中获取)
        /// <summary>
        /// 从XML中获取所需要的对像(对像在返回值集合中获取)
        /// </summary>
        public static List<Object> CreateInstanceObjects(string xmlStr)
        {
            List<Object> objList = null;
            try
            {
                XmlReader xmlReader = XmlTextReader.Create(new StringReader(xmlStr));               
                while (xmlReader.Read())
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        Type type = assembly.GetType(xmlReader.Name);
                        object obj = System.Activator.CreateInstance(type);
                        PropertyInfo[] propertys = obj.GetType().GetProperties();
                        if (xmlReader.HasAttributes)
                            foreach (PropertyInfo p in propertys)
                            {
                                if (xmlReader.MoveToAttribute(p.Name))
                                    try
                                    {
                                        if (p.PropertyType == typeof(String))
                                            p.SetValue(obj, xmlReader.ReadContentAsString(), null);
                                        else if (p.PropertyType == typeof(bool))
                                            p.SetValue(obj, bool.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(int))
                                            p.SetValue(obj, int.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(Single))
                                            p.SetValue(obj, Single.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(float))
                                            p.SetValue(obj, float.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType.IsEnum)
                                            p.SetValue(obj, Enum.Parse(p.PropertyType, xmlReader.ReadContentAsString(), true), null);
                                        else if (p.PropertyType == typeof(short))
                                            p.SetValue(obj, short.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(long))
                                            p.SetValue(obj, long.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(byte))
                                            p.SetValue(obj, byte.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(sbyte))
                                            p.SetValue(obj, sbyte.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(ushort))
                                            p.SetValue(obj, ushort.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(uint))
                                            p.SetValue(obj, uint.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(ulong))
                                            p.SetValue(obj, ulong.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(double))
                                            p.SetValue(obj, double.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(decimal))
                                            p.SetValue(obj, decimal.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(char))
                                            p.SetValue(obj, char.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(System.Net.IPAddress))
                                            p.SetValue(obj, System.Net.IPAddress.Parse(xmlReader.ReadContentAsString()), null);
                                        else if (p.PropertyType == typeof(byte[]))
                                            p.SetValue(obj, Convert.FromBase64String(xmlReader.ReadContentAsString()), null);
                                    }
                                    catch { }
                            }
                        if (objList == null)
                            objList = new List<object>();

                        objList.Add(obj);
                    }
            }
            catch { }

            return objList;
        }
        #endregion
    }
}
