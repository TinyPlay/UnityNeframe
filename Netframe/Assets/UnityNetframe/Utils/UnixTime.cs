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
    using UnityEngine;
    using System.Collections;
    using System;
 
    /// <summary>
    /// Unix Time Worker
    /// </summary>
    public static class UnixTime  {
        /// <summary>
        /// Get Current Unix Time
        /// </summary>
        /// <returns></returns>
        public static int Current()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            return currentEpochTime;
        }
 
        /// <summary>
        /// Get Difference between two Unix Timestamps (in Seconds)
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        public static int SecondsElapsed(int t1)
        {
            int difference = Current() - t1;
            return Mathf.Abs(difference);
        }
 
        /// <summary>
        /// Get Difference between two Unix Timestamps (in Seconds)
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static int SecondsElapsed(int t1, int t2)
        {
            int difference = t1 - t2;
            return Mathf.Abs(difference);
        }
    }
}