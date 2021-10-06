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
namespace UnityNetframe
{
    using System;
    using System.IO;
    using System.Collections;
    using UnityNetframe.Core;
    using UnityEngine;
    using UnityNetframe.Core;
    using UnityNetframe.Core.Enums;
    using UnityNetframe.Utils;
    using UnityEngine.Networking;
    using System.Collections.Generic;

    /// <summary>
    /// Unity Netframe General Class
    /// This is Endpoint for all Netframe Components
    /// </summary>
    public class UNetframe
    {
        // Public Params
        public QueueManager Queue;
        public NetworkManager Network;
        
        // Private Params
        private static string configsPath = "Netframe/Config";
        private NetframeConfig _config;

        /// <summary>
        /// Unity Netframe Constructor.
        /// </summary>
        /// <param name="config"></param>
        public UNetframe(NetframeConfig config = null)
        { 
            // Load Unity Netframe Configuration
            if (config == null)
            {
                TextAsset configAsset = Resources.Load<TextAsset>(configsPath);
                _config = (configAsset == null)
                    ? new NetframeConfig()
                    : JsonUtility.FromJson<NetframeConfig>(configAsset.text);
            }
            else
            {
                _config = config;
            }
            
            if(_config.debugMode) Debug.Log("<b>Unity Netframe Initialized</b>");
            Network = new NetworkManager(_config);
            Queue = new QueueManager(_config, Network);
        }
    }
}