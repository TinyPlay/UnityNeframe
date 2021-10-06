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
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using UnityNetframe.Core;
    using UnityNetframe.Utils;
    using UnityNetframe.Core.Enums;
    
    /// <summary>
    /// Network Manager
    /// </summary>
    public class NetworkManager
    {
        // Private Params
        private NetframeConfig _config;

        /// <summary>
        /// Network Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        public NetworkManager(NetframeConfig config)
        {
            _config = config;
        }
        
        #region Base Requests
        /// <summary>
        /// Send Web Request
        /// </summary>
        /// <param name="requestConfiguration"></param>
        /// <param name="requestComplete"></param>
        public void SendRequest(RequestData requestConfiguration, Action requestComplete = null)
        {
            CoroutineProvider.Start(SendWebRequest(requestConfiguration, requestComplete));
        }
        
        /// <summary>
        /// Send GET Request
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        /// <returns></returns>
        private IEnumerator SendWebRequest(RequestData requestConfig, Action requestComplete = null)
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
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            webRequest.downloadHandler = dH;
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

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if(_config.debugMode) Debug.Log($"(<b>Success!</b>) {requestMethod} Request to: {requestConfig.url} successfully sended.\nResponse Data:{webRequest.downloadHandler.text}");
                if (requestConfig.onComplete != null) requestConfig.onComplete(webRequest.downloadHandler.text);
                if (requestConfig.cacheRequest) SaveRequestCache(requestConfig.url, webRequest.downloadHandler.text);
            }
            else
            {
                if(_config.debugMode) Debug.LogError($"Failed to send {requestMethod} Request to: {requestConfig.url}\nError:{webRequest.error}");
                if (requestConfig.onError != null) requestConfig.onError(webRequest.error);
            }

            if(requestComplete!=null) requestComplete();
            webRequest.Dispose();
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
        /// <summary>
        /// Download AudioClip from Server
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        public void DownloadAudioClip(AudioClipRequestData requestConfig, Action requestComplete = null)
        {
            CoroutineProvider.Start(GetAudioClip(requestConfig, requestComplete));
        }
        /// <summary>
        /// Get Audio Clip from Server
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        /// <returns></returns>
        private IEnumerator GetAudioClip(AudioClipRequestData requestConfig, Action requestComplete = null)
        {
            // Get AudioClip Cache
            if (requestConfig.cacheAudioClip)
            {
                AudioClip cachedClip = GetAudioClipCache(requestConfig.url);
                if (requestConfig.onComplete != null) requestConfig.onComplete(cachedClip);
            }
            
            // Prepare Request
            if(_config.debugMode) Debug.Log($"Downloading AudioClip from: {requestConfig.url}");
            UnityWebRequest multimedia = UnityWebRequestMultimedia.GetAudioClip(requestConfig.url, requestConfig.audioType);
            yield return multimedia.SendWebRequest();

            if (multimedia.result == UnityWebRequest.Result.Success)
            {
                if(_config.debugMode) Debug.Log($"(<b>Success!</b>) Downloading AudioClip complete from {requestConfig.url}");
                if (requestConfig.onComplete != null)
                    requestConfig.onComplete(DownloadHandlerAudioClip.GetContent(multimedia));
                if(requestConfig.cacheAudioClip) SaveContentCache(requestConfig.url, multimedia.downloadHandler.data);
            }
            else
            {
                if(_config.debugMode) Debug.LogError($"Failed to download AudioClip from: {requestConfig.url}\nError:{multimedia.error}");
                if (requestConfig.onError != null) requestConfig.onError(multimedia.error);
            }
            
            if (requestComplete != null) requestComplete();
            multimedia.Dispose();
        }

        /// <summary>
        /// Download Texture2D
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        public void DownloadTexture2D(Texture2DRequestData requestConfig, Action requestComplete = null)
        {
            CoroutineProvider.Start(GetTexture2D(requestConfig, requestComplete));
        }

        /// <summary>
        /// Get Texture2D from URL
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        /// <returns></returns>
        private IEnumerator GetTexture2D(Texture2DRequestData requestConfig, Action requestComplete = null)
        {
            // Get Texure From Cache
            if (requestConfig.cacheTexture)
            {
                Texture2D cachedTexture = GetTextureCache(requestConfig.url);
                if (requestConfig.onComplete != null) requestConfig.onComplete(cachedTexture);
            }
            
            // Prepare Request for Texutre
            if(_config.debugMode) Debug.Log($"Downloading Texture2D from: {requestConfig.url}");
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(requestConfig.url);
            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.Success)
            {
                if(_config.debugMode) Debug.Log($"(<b>Success!</b>) Downloading Texture2D complete from {requestConfig.url}");
                if (requestConfig.onComplete != null)
                    requestConfig.onComplete(DownloadHandlerTexture.GetContent(textureRequest));
                if(requestConfig.cacheTexture) SaveContentCache(requestConfig.url, textureRequest.downloadHandler.data);
            }
            else
            {
                if(_config.debugMode) Debug.LogError($"Failed to download Texture2D from: {requestConfig.url}\nError:{textureRequest.error}");
                if (requestConfig.onError != null) requestConfig.onError(textureRequest.error);
            }

            if (requestComplete != null) requestComplete();
            textureRequest.Dispose();
        }

        /// <summary>
        /// Download Asset Bundle from Server
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        public void DownloadAssetBundle(AssetBundleRequestData requestConfig, Action requestComplete = null)
        {
            CoroutineProvider.Start(GetAssetBundle(requestConfig, requestComplete));
        }

        /// <summary>
        /// Get AssetBundle from Server
        /// </summary>
        /// <param name="requestConfig"></param>
        /// <param name="requestComplete"></param>
        /// <returns></returns>
        private IEnumerator GetAssetBundle(AssetBundleRequestData requestConfig, Action requestComplete = null)
        {
            // Get Bundle from Cache
            while (!Caching.ready) {
                yield return null;
            }
            
            // Prepare Request Asset Bundle
            if(_config.debugMode) Debug.Log($"Downloading AssetBundle Manifest from: {requestConfig.mainfestUrl}");
            UnityWebRequest manifestRequest = UnityWebRequest.Get(requestConfig.mainfestUrl);
            yield return manifestRequest.SendWebRequest();
            if (manifestRequest.result == UnityWebRequest.Result.Success)
            {
                Hash128 hash = default;
                string hashRow = manifestRequest.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
                hash = Hash128.Parse(hashRow.Split(':')[1].Trim());
                if (hash.isValid == true)
                {
                    manifestRequest.Dispose();
                    UnityWebRequest bundleRequset = UnityWebRequestAssetBundle.GetAssetBundle(requestConfig.bundleUrl, hash, 0);
                    yield return bundleRequset.SendWebRequest();

                    if (manifestRequest.result == UnityWebRequest.Result.Success)
                    {
                        if(_config.debugMode) Debug.LogError($"Failed to download AssetBundle from: {requestConfig.bundleUrl}\nError:{bundleRequset.error}");
                        if (requestConfig.onError != null) requestConfig.onError(bundleRequset.error);
                    }
                    else
                    {
                        if(_config.debugMode) Debug.Log($"(<b>Success!</b>) Downloading AssetBundle complete from {requestConfig.bundleUrl}");
                        if (requestConfig.onComplete != null)
                            requestConfig.onComplete(DownloadHandlerAssetBundle.GetContent(bundleRequset));
                    }

                    bundleRequset.Dispose();
                }
                else
                {
                    if(_config.debugMode) Debug.Log($"(<b>Manifest Loading Error!</b>) Wrong AssetBundle Manifest.");
                    if (requestConfig.onError != null) requestConfig.onError($"Wrong AssetBundle Manifest Hash for: {requestConfig.mainfestUrl}");
                }
            }
            else
            {
                if(_config.debugMode) Debug.Log($"(<b>Mainfest Request Error!</b>) Failed to download AssetBundle Manifest from: {requestConfig.mainfestUrl}");
                if (requestConfig.onError != null) requestConfig.onError(manifestRequest.error);
            }

            if (requestComplete != null) requestComplete();
            manifestRequest.Dispose();
        }

        /// <summary>
        /// Save Content Cache
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentData"></param>
        private void SaveContentCache(string url, byte[] contentData)
        {
            string cachePath = Application.dataPath + Base64.Encode(url) + ".contentcache";
            File.WriteAllBytes(cachePath, contentData);
        }

        /// <summary>
        /// Get Texture2D Cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Texture2D GetTextureCache(string url)
        {
            Texture2D texture = null;
            string cachePath = Application.dataPath + Base64.Encode(url) + ".contentcache";
            
            if (File.Exists(cachePath))
            {
                byte[] data = File.ReadAllBytes(cachePath);
                texture = new Texture2D(1, 1);
                texture.LoadImage(data, true);
            }
            
            return texture;
        }

        /// <summary>
        /// Get AudioClip Cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private AudioClip GetAudioClipCache(string url)
        {
            AudioClip audioClip = null;
            string cachePath = Application.dataPath + Base64.Encode(url) + ".contentcache";
            
            if (File.Exists(cachePath))
            {
                byte[] data = File.ReadAllBytes(cachePath);
                using (Stream s = new MemoryStream(data))
                {
                    audioClip = AudioClip.Create(url, data.Length, 1, 48000, false);
                    float[] f = Converters.ConvertByteToFloat(data);
                    audioClip.SetData(f, 0);
                }
                audioClip.LoadAudioData();
            }
            
            return audioClip;
        }
        #endregion

        #region File Upload
        

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

        /// <summary>
        /// Get Content-Length in bytes by Response Header
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onComplete"></param>
        public void GetContentLength(string url, Action<int> onComplete)
        {
            CoroutineProvider.Start(GetHeaderLength(url, onComplete));
        }

        /// <summary>
        /// Get Content-Length Header
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator GetHeaderLength(string url, Action<int> onComplete, Action<string> onError = null)
        {
            // Prepare to Get Request Header
            if(_config.debugMode) Debug.Log($"Trying to get Content-Length for: {url}");
            UnityWebRequest webRequest = UnityWebRequest.Head(url);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var contentLength = webRequest.GetResponseHeader("Content-Length");
                if (int.TryParse(contentLength, out int returnValue))
                {
                    if(_config.debugMode) Debug.Log($"(<b>Success!</b>) Content-Length for {url} is {returnValue} bytes");
                    onComplete(returnValue);
                }
                else
                {
                    if (onError != null) onError($"Failed to parse Content-Length parameter for {url}");
                }
            }
            else
            {
                if(_config.debugMode) Debug.LogError($"Failed to get Content-Length for: {url}\nError:{webRequest.error}");
                if (onError != null) onError(webRequest.error);
            }

            webRequest.Dispose();
        }
        
        /// <summary>
        /// Clear Asset Bundle Cache by URL
        /// </summary>
        /// <param name="url"></param>
        public void ClearAssetBundleCache(string url)
        {
            string fileName = GetFileNameFromUrl(url);
            Caching.ClearAllCachedVersions(fileName);
        }

        /// <summary>
        /// Get Filename from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileNameWithoutExtension(uri.LocalPath);

            return fileName;
        }
        
        /// <summary>
        /// Get Hash From Manifest
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        private Hash128 GetHashFromManifest(string manifest)
        {
            string hashRow = manifest.Split("\n".ToCharArray())[5];
            var hash = Hash128.Parse(hashRow.Split(':')[1].Trim());

            return hash;
        }
        #endregion
    }
}