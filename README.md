# Unity Netframe Library
The Unity Netframe library is designed to facilitate network requests and resource loading. It can be used to work with the API, or to get updates. Sending queries is done using Coroutine (there is a coroutine-simulating singleton class for this).

Library Developed by <a href="https://tinydev.ru/">TinyPlay, Inc.</a> (<a href="https://github.com/TinyPlay/UnityNeframe/blob/main/LICENSE">MIT License</a>).

## General Features:
* All kinds of queries (GET, POST, PUT, DELETE, HEAD);
* Request Queue Support;
* Caching requests and File Downloads;
* Flexible library customization;
* Texture2D, AudioClip, AssetBundle Downloads wih Caching;
* Progress Handling for Direct Requests and Queue;
* A true check of the Internet connection and determination of the type of connection;

## Get Started
After installation you can open the demo scene, or go to the library settings. The demo stage allows you to see how all aspects of the library work.

<b>Setup Library:</b><br/>
Open Setup window by <b>UnityNetframe -> Setup</b> and configure your Netframe Settings:<br/><br/>
<img src="https://github.com/TinyPlay/UnityNeframe/blob/develop/setup.png?raw=true" />

<b>Working with Library</b><br/>
To start working with the library, initialize the UNetframe object in your project:
```csharp
UNetframe UNet = new UNetframe();
```

Now you can work with Unity Netframe API. For example, check user internet connection:
```csharp
UNet.Network.GetNetworkType(status =>{
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
  
  Debug.Log($"<b>Current Network:</b> {netState}");
});
```

## API Reference


## Direct Web Requests (HTTP)

## Content Downloading

## Content Uploading

## Working with Queue

## Utils
