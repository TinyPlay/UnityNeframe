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
        }

        #region Base Requests
        /// <summary>
        /// Send Web Request
        /// </summary>
        /// <param name="requestConfiguration"></param>
        public void SendRequest(RequestData requestConfiguration)
        {
            CoroutineProvider.Start(SendWebRequest(requestConfiguration));
        }
        
        /// <summary>
        /// Send GET Request
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <returns></returns>
        private IEnumerator SendWebRequest(RequestData requestConfig)
        {
            // Load Request Cache
            if (requestConfig.cacheRequest)
            {
                string requestCache = GetRequestCache(requestConfig.url);
                if (requestCache != null)
                {
                    if (requestConfig.onComplete != null)
                    {
                        requestConfig.onComplete(requestCache);
                    }
                }
            }

            // Detect Method
            string requestMethod = Converters.GetRequestMethod(requestConfig.type);
            if(_config.debugMode) Debug.Log($"Sending {requestMethod} Request to: {requestConfig.url}");

            // Prepare Request
            UnityWebRequest webRequest = new UnityWebRequest(requestConfig.url, requestMethod);
            foreach (KeyValuePair<string, string> header in requestConfig.headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }

            // Prepare Params for POST
            if (requestConfig.type == RequestTypeEnum.POST)
            {
                foreach (KeyValuePair<string, string> formParameter in requestConfig.postData)
                {
                    webRequest.SetRequestHeader(formParameter.Key, formParameter.Value);
                }
            }
            

            // Send Request
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    if(_config.debugMode) Debug.LogError($"(<b>Connection Error!</b>) Failed to send {requestMethod} Request to: {requestConfig.url}\nError:{webRequest.error}");
                    if (requestConfig.onError != null) requestConfig.onError(webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    if(_config.debugMode) Debug.LogError($"(<b>Data Processing Error!</b>) Failed to send {requestMethod} Request to: {requestConfig.url}\nError:{webRequest.error}");
                    if (requestConfig.onError != null) requestConfig.onError(webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    if(_config.debugMode) Debug.LogError($"(<b>Protocol Error!</b>) Failed to send {requestMethod} Request to: {requestConfig.url}\nError:{webRequest.error}");
                    if (requestConfig.onError != null) requestConfig.onError(webRequest.error);
                    break;
                case UnityWebRequest.Result.InProgress:
                    if (requestConfig.onProgress != null)
                        requestConfig.onProgress(webRequest.downloadProgress, webRequest.downloadedBytes,
                            webRequest.downloadHandler);
                    break;
                case UnityWebRequest.Result.Success:
                    if(_config.debugMode) Debug.Log($"(<b>Success!</b>) {requestMethod} Request to: {requestConfig.url} successfully sended.\nResponse Data:{webRequest.downloadHandler.text}");
                    if (requestConfig.onComplete != null) requestConfig.onComplete(webRequest.downloadHandler.text);
                    if (requestConfig.cacheRequest) SaveRequestCache(requestConfig.url, webRequest.downloadHandler.text);
                    break;
            }
        }

        /// <summary>
        /// Get Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetRequestCache(string url)
        {
            string cachePath = Application.dataPath + Base64.Encode(url) + "_.cache";
            string cacheStamp = Application.dataPath + Base64.Encode(url) + ".cachestamp";
            if (File.Exists(cachePath) && File.Exists(cacheStamp))
            {
                int cacheCreationTime = Int32.Parse(File.ReadAllText(cacheStamp));
                if (UnixTime.SecondsElapsed(cacheCreationTime) > _config.cacheLifetime)
                {
                    File.Delete(cachePath);
                    File.Delete(cacheStamp);
                    return null;
                }

                return File.ReadAllText(cachePath);
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Save Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <param name="response"></param>
        private void SaveRequestCache(string url, string responseData)
        {
            string cachePath = Application.dataPath + Base64.Encode(url) + ".cache";
            string cacheStamp = Application.dataPath + Base64.Encode(url) + ".cachestamp";
            File.WriteAllText(cachePath, responseData);
            File.WriteAllText(cacheStamp, UnixTime.Current().ToString());
        }
        #endregion

        #region Content Management
        
        

        #endregion

        #region Queue Management

        

        #endregion

        #region Utils Methods
        /// <summary>
        /// Get Current Network Type
        /// </summary>
        /// <returns></returns>
        public void GetNetworkType(Action<NetworkType> onComplete)
        {
            // Check Reachability General
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                onComplete(NetworkType.None);
            }
            
            // Send Request to Google
            SendRequest(new RequestData()
            {
                url = "https://google.com/",
                type = RequestTypeEnum.GET,
                onComplete = handler =>
                {
                    // Check Network Mode
                    if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                    {
                        onComplete(NetworkType.WIFI);
                    }else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                    {
                        onComplete(NetworkType.Mobile);
                    }
                },
                onError = error =>
                {
                    onComplete(NetworkType.None);
                }
            });
        }
        
        
        #endregion
    }
}