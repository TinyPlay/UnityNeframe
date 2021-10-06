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
    /// Texture2D Request Data Class
    /// </summary>
    public class Texture2DRequestData
    {
        // Base Params
        public string url = "";                                             // Request URL

        // Request Callbacks
        public Action<string> onError = null;                               // On Request Error
        public Action<Texture2D> onComplete = null;                         // Complete Callback
        public Action<float, ulong, DownloadHandler> onProgress = null;     // On Request Progress
        
        // Cache Data
        public bool cacheTexture = true;                                    // Save Texture2D to Cache
    }
}