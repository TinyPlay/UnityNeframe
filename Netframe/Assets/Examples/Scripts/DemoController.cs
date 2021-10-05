using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityNetframe;
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
    
    
    // Private Params
    private UNetframe UNet;
    private float timeToUpdateStatus = 0f;

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
    void Start()
    {
        
    }

    /// <summary>
    /// Update Every Frame
    /// </summary>
    void Update()
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
            UNet.GetNetworkType(status =>
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
}
