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

    /// <summary>
    /// Queue Saving Model
    /// </summary>
    [System.Serializable]
    public class QueueSaveModel
    {
        public List<RequestData> WebRequests = new List<RequestData>();
        public List<Texture2DRequestData> TextureRequests = new List<Texture2DRequestData>();
        public List<AudioClipRequestData> AudioClipRequests = new List<AudioClipRequestData>();
        public List<AssetBundleRequestData> AssetBundleRequests = new List<AssetBundleRequestData>();
    }
}