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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityNetframe.Core.Enums;
    using UnityEngine;
    using UnityEngine.Networking;
    
    /// <summary>
    /// Request Data Class
    /// </summary>
    public class RequestData
    {
        // Base Params
        public string url = "";                                             // Request URL
        public RequestTypeEnum type = RequestTypeEnum.GET;                  // Request Type
        
        // Request Params
        public Dictionary<string, string> headers = new Dictionary<string, string>();   // Request Headers
        public Dictionary<string, string> postData = new Dictionary<string, string>();  // Form Data

        // Callbacks
        public Action<string> onError = null;                               // On Request Error
        public Action<string> onComplete = null;                            // On Request Complete
        public Action<float, ulong, DownloadHandler> onProgress = null;     // On Request Progress
        
        // Cache Data
        public bool cacheRequest = false;                                   // Cache Request
    }
}