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
    using UnityEngine;
    using UnityNetframe.Core;
    using UnityNetframe.Utils;
    
    /// <summary>
    /// Queue Manager
    /// </summary>
    public class QueueManager
    {
        // Private Params
        private NetframeConfig _config;
        private NetworkManager _requests;
        private List<object> _requestQueue = new List<object>();

        /// <summary>
        /// Queue Manager Constructor
        /// </summary>
        /// <param name="Instance"></param>
        public QueueManager(NetframeConfig config, NetworkManager instance)
        {
            _config = config;
            _requests = instance;
        }

        #region Working with Queue
        /// <summary>
        /// Add WebRequest to Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Add(RequestData requestConfig)
        {
            _requestQueue.Add(requestConfig);
            return this;
        }

        /// <summary>
        /// Add AudioClip Request to Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Add(AudioClipRequestData requestConfig)
        {
            _requestQueue.Add(requestConfig);
            return this;
        }

        /// <summary>
        /// Add Texture2D Request to Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Add(Texture2DRequestData requestConfig)
        {
            _requestQueue.Add(requestConfig);
            return this;
        }

        /// <summary>
        /// Add AssetBundle Request to Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Add(AssetBundleRequestData requestConfig)
        {
            _requestQueue.Add(requestConfig);
            return this;
        }

        /// <summary>
        /// Remove WebRequest from Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        public QueueManager Remove(RequestData requestConfig)
        {
            _requestQueue.Remove(requestConfig);
            return this;
        }
        
        /// <summary>
        /// Remove AudioClip Request from Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Remove(AudioClipRequestData requestConfig)
        {
            _requestQueue.Remove(requestConfig);
            return this;
        }
        
        /// <summary>
        /// Remove Texture2D Request from Queue
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Remove(Texture2DRequestData requestConfig)
        {
            _requestQueue.Remove(requestConfig);
            return this;
        }
        
        /// <summary>
        /// Remove AssetBundle Request from URL
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        public QueueManager Remove(AssetBundleRequestData requestConfig)
        {
            _requestQueue.Remove(requestConfig);
            return this;
        }

        /// <summary>
        /// Clear Queue
        /// </summary>
        /// <returns></returns>
        public QueueManager ClearQueue()
        {
            _requestQueue.Clear();
            return this;
        }
        
        
        #endregion

        #region Queue Processing
        public QueueManager Start()
        {
            return this;
        }

        public QueueManager Stop()
        {
            return this;
        }
        
        #endregion
        
    }
}