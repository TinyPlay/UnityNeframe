using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityNetframe;
using UnityNetframe.Core;
using UnityNetframe.Core.Enums;

/// <summary>
/// Demo Controller
/// </summary>
public class DemoController : MonoBehaviour
{
    // Public Params
    [Header("Network Status")] 
    [SerializeField]
    private Text NetworkStatus;

    [SerializeField] 
    private float NetworkStatusUpdateInterval = 3f;

    [Header("Request View")] 
    [SerializeField] private InputField RequestUrlField;
    [SerializeField] private Toggle RequestCacheToggle;
    [SerializeField] private Dropdown RequestTypeField;
    [SerializeField] private Button AddToQueueButton;
    [SerializeField] private Button SendRequestButton;
    
    [Header("Queue View")] 
    [SerializeField] private GameObject QueueItemTemplate;
    [SerializeField] private Transform QueueView;
    [SerializeField] private Button QueueSendButton;


    // Private Params
    private UNetframe UNet;
    private float timeToUpdateStatus = 0f;
    private List<GameObject> QueueUIObjects = new List<GameObject>();

    /// <summary>
    /// On Before Scene Initialized
    /// </summary>
    private void Awake()
    {
        UNet = new UNetframe();
    }
    
    /// <summary>
    /// On Scene Started
    /// </summary>
    private void Start()
    {
        SendRequestButton.onClick.AddListener(SendRequest);
        AddToQueueButton.onClick.AddListener(AddToQueue);
        QueueSendButton.onClick.AddListener(SendQueue);
        UNet.Queue.QueueSended.AddListener(OnQueueSended);
    }

    /// <summary>
    /// Send Request
    /// </summary>
    private void SendRequest()
    {
        // Check URL
        if (RequestUrlField.text.Length < 1)
            return;

        // Send Request
        if (RequestTypeField.value == 0 || RequestTypeField.value == 1 || RequestTypeField.value == 2 || RequestTypeField.value == 3 || RequestTypeField.value == 4)
        {
            RequestData requestConfig = new RequestData();
            requestConfig.url = RequestUrlField.text;
            requestConfig.type = GetRequestType(RequestTypeField.value);
            requestConfig.cacheRequest = RequestCacheToggle.IsActive();
            UNet.Network.SendRequest(requestConfig);
        }else if (RequestTypeField.value == 5)
        {
            Texture2DRequestData requestConfig = new Texture2DRequestData();
            requestConfig.url = RequestUrlField.text;
            requestConfig.cacheTexture = RequestCacheToggle.IsActive();
            UNet.Network.DownloadTexture2D(requestConfig);
        }else if (RequestTypeField.value == 6)
        {
            AudioClipRequestData requestConfig = new AudioClipRequestData();
            requestConfig.url = RequestUrlField.text;
            requestConfig.cacheAudioClip = RequestCacheToggle.IsActive();
            UNet.Network.DownloadAudioClip(requestConfig);
        }else if (RequestTypeField.value == 7)
        {
            AssetBundleRequestData requestConfig = new AssetBundleRequestData();
            requestConfig.bundleUrl = RequestUrlField.text;
            requestConfig.mainfestUrl = RequestUrlField.text + ".manifest";
            requestConfig.cacheBundle = RequestCacheToggle.IsActive();
            UNet.Network.DownloadAssetBundle(requestConfig);
        }
        
        // Clear URL
        RequestUrlField.text = "";
    }

    /// <summary>
    /// Add to Queue
    /// </summary>
    private void AddToQueue()
    {
        // Check URL
        if (RequestUrlField.text.Length < 1)
            return;
        
        // Add to Queue
        if (RequestTypeField.value == 0 || RequestTypeField.value == 1 || RequestTypeField.value == 2 || RequestTypeField.value == 3 || RequestTypeField.value == 4)
        {
            RequestData requestConfig = new RequestData();
            requestConfig.url = RequestUrlField.text;
            requestConfig.type = GetRequestType(RequestTypeField.value);
            requestConfig.cacheRequest = RequestCacheToggle.IsActive();
            UNet.Queue.Add(requestConfig);
        }else if (RequestTypeField.value == 5)
        {
            Texture2DRequestData requestConfig = new Texture2DRequestData();
            requestConfig.url = RequestUrlField.text;
            requestConfig.cacheTexture = RequestCacheToggle.IsActive();
            UNet.Queue.Add(requestConfig);
        }else if (RequestTypeField.value == 6)
        {
            AudioClipRequestData requestConfig = new AudioClipRequestData();
            requestConfig.url = RequestUrlField.text;
            requestConfig.cacheAudioClip = RequestCacheToggle.IsActive();
            UNet.Queue.Add(requestConfig);
        }else if (RequestTypeField.value == 7)
        {
            AssetBundleRequestData requestConfig = new AssetBundleRequestData();
            requestConfig.bundleUrl = RequestUrlField.text;
            requestConfig.mainfestUrl = RequestUrlField.text + ".manifest";
            requestConfig.cacheBundle = RequestCacheToggle.IsActive();
            UNet.Queue.Add(requestConfig);
        }
        
        // Add To Interface
        GameObject queueItem = GameObject.Instantiate(QueueItemTemplate, QueueView);
        queueItem.transform.SetAsLastSibling();
        queueItem.GetComponent<QueueItem>().SetCtx(new QueueItem.Context
        {
            url = RequestUrlField.text,
            type = RequestTypeField.options[RequestTypeField.value].text
        });
        QueueUIObjects.Add(queueItem);
        
        // Clear URL
        RequestUrlField.text = "";
    }

    /// <summary>
    /// Send Queue
    /// </summary>
    private void SendQueue()
    {
        if (UNet.Queue.GetQueueCount() > 0)
        {
            UNet.Queue.Start();
            QueueSendButton.interactable = false;
        }
    }

    /// <summary>
    /// On QueueSended
    /// </summary>
    private void OnQueueSended()
    {
        // Update List
        for (int i = 0; i < UNet.GetMaxQueueRequests(); i++)
        {
            Destroy(QueueUIObjects[i]);
            QueueUIObjects.RemoveAt(i);
        }
        
        // Queue Button Activate
        if (UNet.Queue.GetQueueCount() < 1)
        {
            QueueSendButton.interactable = true;
        }
    }

    /// <summary>
    /// On Destroy
    /// </summary>
    private void OnDestroy()
    {
        SendRequestButton.onClick.RemoveAllListeners();
        QueueSendButton.onClick.RemoveAllListeners();
        AddToQueueButton.onClick.RemoveAllListeners();
        UNet.Queue.QueueSended.RemoveAllListeners();
    }

    /// <summary>
    /// Update Every Frame
    /// </summary>
    private void Update()
    {
        UpdateNetStatus();
    }

    /// <summary>
    /// Update Network Status
    /// </summary>
    private void UpdateNetStatus()
    {
        if (timeToUpdateStatus <= 0f)
        {
            timeToUpdateStatus = NetworkStatusUpdateInterval;
            UNet.Network.GetNetworkType(status =>
            {
                string netState = "";
                switch (status)
                {
                    case NetworkType.None:
                        netState = "No Internet";
                        break;
                    case NetworkType.Mobile:
                        netState = "Mobile Internet";
                        break;
                    case NetworkType.WIFI:
                        netState = "WIFI";
                        break;
                }
                NetworkStatus.text = $"<b>Current Network:</b>\n {netState}";
            });
        }
        else
        {
            timeToUpdateStatus -= Time.deltaTime;
        }
    }

    #region Utils
    /// <summary>
    /// Get Request type by Index
    /// </summary>
    /// <param name="requestTypeIndex"></param>
    /// <returns></returns>
    private RequestTypeEnum GetRequestType(int requestTypeIndex)
    {
        switch (requestTypeIndex)
        {
            case 0:
                return RequestTypeEnum.GET;
            case 1:
                return RequestTypeEnum.POST;
            case 2:
                return RequestTypeEnum.PUT;
            case 3:
                return RequestTypeEnum.DELETE;
            case 4:
                return RequestTypeEnum.HEAD;
            default:
                return RequestTypeEnum.GET;
        }
    }
    #endregion
}
