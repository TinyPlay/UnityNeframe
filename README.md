# Unity Netframe Library
The Unity Netframe library is designed to facilitate network requests and resource loading. It can be used to work with the API, or to get updates. Sending queries is done using Coroutine (there is a coroutine-simulating singleton class for this).

Library Developed by <a href="https://tinydev.ru/">TinyPlay, Inc.</a> (<a href="https://github.com/TinyPlay/UnityNeframe/blob/main/LICENSE">MIT License</a>).

<b>Warning! The library is still a work in progress, so we advise you to use it for evaluation purposes only. We will appreciate your help and feedback.</b>

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

## Direct Web Requests (HTTP)
You can send Web Requests immediately without using queue.<br/>
To do this, use the <b>"Network"</b> object in the <b>"UNetframe"</b> class:
```csharp
UNet.Network.SendRequest(new RequestData()
{
  url = "https://example.com",
  type = RequestTypeEnum.POST,
  cacheRequest = true,
  headers = new Dictionary<string, string>(),
  postData = new Dictionary<string, string>(),
  onComplete = data =>
  {
  },
  onError = error =>
  {
  }
});
```

<b>SendRequest method apply RequestData Object with Parameters:</b>
* <b>url</b> (string) - Request URL;
* <b>type</b> (RequestTypeEnum) - Request Type (GET, POST, PUT, DELETE, HEAD);
* <b>cacheRequest</b> (bool) - Request Cache. Lifetime can be founded in Netframe Configs;
* <b>headers</b> (Dictionary<string,string>) - Headers Dictionary for this request;
* <b>postData</b> (Dictionary<string,string>) - Additional Data for POST requests;
* <b>onComplete</b> (Action<string>) - Complete Callback with string response;
* <b>onError</b> (Action<string>) - Error Callback with description;


## Content Downloading
In the same way as with normal queries, you can upload specific content (textures, audio, asset bundles). To do this, use the following construction:
```csharp
UNet.Network.DownloadTexture2D(new Texture2DRequestData()
{
  url = "https://example.com/image.png",
  cacheTexture = true,
  onComplete = tex =>
  {
  },
  onError = tex =>
  {
  },
  onProgress = (progress, size, handler) =>
  {
  }
});
```
  
<b>DownloadTexure2D and similar methods apply objects created specifically for these types with approximately the same parameters, where</b>
* <b>url</b> (string) - Request URL;
* <b>cacheTexture</b> (bool) - Request Cache.
* <b>onComplete</b> (Action<object>) - Complete Callback with downloaded object response;
* <b>onError</b> (Action<string>) - Error Callback with description;
* <b>onProgress</b> (Action<float,ulong,DownloadHandler) - Progress Callback with DownloadHandler;

## Content Uploading
Work in Progress...

## Working with Queue
If you need to send batch requests, or download a lot of resources, we recommend that you use a queue. It divides requests into separate groups and sends them at specific intervals.
  
To use the queue, you can use the following construction:
```csharp
UNet.Queue.Add(new RequestData()
{
  url = "https://google.com",
  type = RequestTypeEnum.GET,
  cacheRequest = false
}).Add(new RequestData()
{
  url = "https://yandex.com",
  type = RequestTypeEnum.POST,
  cacheRequest = false
}).Add(new Texture2DRequestData()
{
  url = "https://example.com/image.png",
  cacheTexture = true
}).Start();
```

## Credits
Unity Netframe Library developed by Ilya Rastorguev.
Copyright (c) 2021. TinyPlay, Inc.
