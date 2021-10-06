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
namespace UnityNetframe.Core
{
    /// <summary>
    /// Netframe Configuration Model
    /// </summary>
    [System.Serializable]
    public class NetframeConfig
    {
        // Queue Settings
        public bool debugMode = true;
        public int cacheLifetime = 600;
        public bool saveQueueBetweenSessions = true;
        public int maxRequestQueue = 10;
        public float queueRequestsInterval = 5f;
    }
}