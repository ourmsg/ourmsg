using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IMLibrary3.Operation
{
    /// <summary>
    /// 序列化类
    /// </summary>
    public sealed  class  Serializers
    {
        
        #region Binary Serializers
        /// <summary>
        /// 将对像序列化为字节数组
        /// </summary>
        /// <param name="request">对像</param>
        /// <returns></returns>
        public static  byte[] SerializeBinary(object request)
        {
            return SerializeMemoryStream(request).ToArray();
        }
        /// <summary>
        /// 将对像序列化为内存流
        /// </summary>
        /// <param name="request">对像</param>
        /// <returns></returns>
        public static System.IO.MemoryStream SerializeMemoryStream(object request)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream;
        }


        /// <summary>
        /// 将字节数组反序列为对像
        /// </summary>
        /// <param name="Data">字节数组</param>
        /// <returns></returns>
        public static object DeSerializeBinary(byte[] Data)
        {
            return DeSerializeMemoryStream(new System.IO.MemoryStream(Data));
        }

        /// <summary>
        /// 将内存流反序列为对像
        /// </summary>
        /// <param name="memStream">内存流</param>
        /// <returns></returns>
        public static object DeSerializeMemoryStream(System.IO.MemoryStream memStream)
        {
            memStream.Position = 0;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            object newobj = deserializer.Deserialize(memStream);
            memStream.Close();
            return newobj;
        }
        #endregion

        #region XML Serializers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static byte[] SerializeBinarySOAP(object request)
        {
           return  SerializeMemoryStreamSOAP(request).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static System.IO.MemoryStream SerializeMemoryStreamSOAP(object request)
        {

            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static object DeSerializeBinarySOAP(byte[] Data)
        {
            return DeSerializeMemoryStreamSOAP(new System.IO.MemoryStream(Data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memStream"></param>
        /// <returns></returns>
        public static object DeSerializeMemoryStreamSOAP(System.IO.MemoryStream memStream)
        {
            object sr;
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter deserializer = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            memStream.Position = 0;
            sr = deserializer.Deserialize(memStream);
            memStream.Close();
            return sr;
        }

        #endregion

    }
}
