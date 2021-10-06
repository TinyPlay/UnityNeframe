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
    using System.Text;
    
    /// <summary>
    /// Base64 Helper
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Encode to Base64
        /// </summary>
        /// <param name="decodedText"></param>
        /// <returns></returns>
        public static string Encode(string decodedText)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes (decodedText);
            string encodedText = Convert.ToBase64String (bytesToEncode);
            return encodedText;
        }

        /// <summary>
        /// Decode from Base64
        /// </summary>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        public static string Decode(string encodedText)
        {
            byte[] decodedBytes = Convert.FromBase64String (encodedText);
            string decodedText = Encoding.UTF8.GetString (decodedBytes);
            return decodedText;
        }
    }
}