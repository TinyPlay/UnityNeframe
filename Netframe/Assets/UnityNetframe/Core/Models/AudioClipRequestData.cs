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
    /// AudioClip Request Data Class
    /// </summary>
    public class AudioClipRequestData
    {
        // Base Params
        public string url = "";                                             // Request URL
        public AudioType audioType = AudioType.OGGVORBIS;                   // Audio Type
        
        // Request Callbacks
        public Action<string> onError = null;                               // On Request Error
        public Action<AudioClip> onComplete = null;                         // Complete Callback
        public Action<float, ulong, DownloadHandler> onProgress = null;     // On Request Progress
        
        // Cache Data
        public bool cacheAudioClip = true;                                  // Save AudioClip to Cache
    }
}