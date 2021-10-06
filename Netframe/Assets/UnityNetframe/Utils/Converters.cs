/*
 *  UnityNetframe Library
 *  A library for unity that allows you to work conveniently with Web APIs.
 *  Easy handling of batch queries, resource loading, and other components.
 *
 *  Copyright (C) 2021 TinyPlay, Inc. All Rights Reserved.
 *  Developer:  Ilya Rastorguev
 *  Version:    0.2.4
 *  URL:        https://github.com/TinyPlay/UnityNeframe/
 *  License:    MIT (https://github.com/TinyPlay/UnityNeframe/blob/main/LICENSE)
 */
namespace UnityNetframe.Utils
{
    using System;
    using UnityNetframe.Core.Enums;
    
    /// <summary>
    /// Converters Class
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Get Request method name from Enum
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetRequestMethod(RequestTypeEnum method)
        {
            switch (method)
            {
                case RequestTypeEnum.POST:
                    return "POST";
                case RequestTypeEnum.PUT:
                    return "PUT";
                case RequestTypeEnum.DELETE:
                    return "DELETE";
                case RequestTypeEnum.HEAD:
                    return "HEAD";
                default:
                    return "GET";
            }
        }
        
        /// <summary>
        /// Convert Byte to Float
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float[] ConvertByteToFloat(byte[] array)
        {
            float[] floatArr = new float[array.Length / 4];
            for (int i = 0; i < floatArr.Length; i++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(array, i * 4, 4);
                }
                floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
            }
            return floatArr;
        }
    }
}