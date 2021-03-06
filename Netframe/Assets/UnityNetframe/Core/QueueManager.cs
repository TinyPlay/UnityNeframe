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
    using System.Linq;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityNetframe.Core;
    using UnityNetframe.Utils;
    
    /// <summary>
    /// Queue Manager
    /// </summary>
    public class QueueManager
    {
        // Public Params
        public UnityEvent QueueSended = new UnityEvent();
        
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

            // Auto Restore Queue
            if (_config.saveQueueBetweenSessions)
            {
                LoadQueue();
                Start();
            }
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
        /// <summary>
        /// Start Queue
        /// </summary>
        /// <returns></returns>
        public QueueManager Start()
        {
            CoroutineProvider.Start(SendQueue());
            return this;
        }

        /// <summary>
        /// Send Queue
        /// </summary>
        /// <returns></returns>
        private IEnumerator SendQueue()
        {
            // Send Queue Limit
            while (_requestQueue.Count > 0)
            {
                foreach (object queueData in _requestQueue.Take(_config.maxRequestQueue))
                {
                    if (queueData is RequestData)
                    {
                        _requests.SendRequest((RequestData) queueData, () =>
                        {
                            Remove((RequestData) queueData);
                        });
                    }else if (queueData is Texture2DRequestData)
                    {
                        _requests.DownloadTexture2D((Texture2DRequestData) queueData, () =>
                        {
                            Remove((Texture2DRequestData) queueData);
                        });
                    }else if (queueData is AudioClipRequestData)
                    {
                        _requests.DownloadAudioClip((AudioClipRequestData) queueData, () =>
                        {
                            Remove((AudioClipRequestData) queueData);
                        });
                    }else if (queueData is AssetBundleRequestData)
                    {
                        _requests.DownloadAssetBundle((AssetBundleRequestData) queueData, () =>
                        {
                            Remove((AssetBundleRequestData) queueData);
                        });
                    }
                }
                
                // Save Queue
                if (_config.saveQueueBetweenSessions) SaveQueue();
                QueueSended.Invoke();
                yield return new WaitForSeconds(_config.queueRequestsInterval);
            }
        }

        /// <summary>
        /// Stop Queue
        /// </summary>
        /// <returns></returns>
        public QueueManager Stop()
        {
            CoroutineProvider.Stop(SendQueue());
            return this;
        }

        /// <summary>
        /// Get Queue Count
        /// </summary>
        /// <returns></returns>
        public int GetQueueCount()
        {
            return _requestQueue.Count;
        }

        /// <summary>
        /// Save Queue
        /// </summary>
        /// <returns></returns>
        public bool SaveQueue()
        {
            // Generate Save Data
            if (_requestQueue.Count > 0)
            {
                QueueSaveModel save = new QueueSaveModel();
                foreach (object requestData in _requestQueue)
                {
                    if (requestData is RequestData)
                        save.WebRequests.Add((RequestData) requestData);
                    else if(requestData is Texture2DRequestData)
                        save.TextureRequests.Add((Texture2DRequestData) requestData);
                    else if(requestData is AudioClipRequestData)
                        save.AudioClipRequests.Add((AudioClipRequestData) requestData);
                    else if(requestData is AssetBundleRequestData)
                        save.AssetBundleRequests.Add((AssetBundleRequestData) requestData);
                }
            
                // Save Queue to Binary
                BinaryFormatter converter = new BinaryFormatter();
                string saveFile = Application.persistentDataPath + "/queue.data";
                FileStream outputStream = new FileStream(saveFile, FileMode.Create);
                converter.Serialize(outputStream, save);
                outputStream.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Load Queue
        /// </summary>
        /// <returns></returns>
        public bool LoadQueue()
        {
            // Load Binary and Deserialize
            QueueSaveModel save = new QueueSaveModel();
            BinaryFormatter converter = new BinaryFormatter();

            string saveFile = Application.persistentDataPath + "/queue.data";
            if(File.Exists(saveFile)) {
                FileStream inputStream = new FileStream(saveFile, FileMode.Open);
                save = converter.Deserialize(inputStream) as QueueSaveModel;
                inputStream.Close();
            }
            else
            {
                return false;
            }
            
            // Add to Queue
            if (save.TextureRequests.Count > 0 || save.WebRequests.Count > 0 || save.AssetBundleRequests.Count > 0 ||
                save.AudioClipRequests.Count > 0)
            {
                foreach (Texture2DRequestData requestData in save.TextureRequests)
                {
                    Add(requestData);
                }
                foreach (RequestData requestData in save.WebRequests)
                {
                    Add(requestData);
                }
                foreach (AssetBundleRequestData requestData in save.AssetBundleRequests)
                {
                    Add(requestData);
                }
                foreach (AudioClipRequestData requestData in save.AudioClipRequests)
                {
                    Add(requestData);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}