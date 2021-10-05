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
namespace UnityNetframe.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using UnityNetframe.Core;
    
    /// <summary>
    /// Netframe Setup Window
    /// Use this window to configure UnityNetframe
    /// basic SDK
    /// </summary>
    public class NetframeSetup : EditorWindow
    {
        // Public Params
        public static NetframeSetup setupWindow;
        public NetframeConfig Configuration = new NetframeConfig();

        // Private Params
        private static string configsPath = "Netframe/Config";
        
        /// <summary>
        /// Setup Interface
        /// </summary>
        private void OnGUI ()
        {
            // Welcome UI
            Rect welcome = (Rect) EditorGUILayout.BeginVertical();
            GUILayout.Label ("Welcome to Unity Netframe!", EditorStyles.largeLabel);
            GUILayout.Label("This library makes it easier for you to work with the network, including uploading and downloading content, sending requests, and managing queues.", EditorStyles.helpBox);
            EditorGUILayout.Space();
            if (GUILayout.Button("Read Documentation"))
            {
                Application.OpenURL("https://github.com/TinyPlay/UnityNeframe/");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            // Configuration Editor
            Rect configs = (Rect) EditorGUILayout.BeginVertical();
            GUILayout.Label ("NetframeConfiguration", EditorStyles.largeLabel);
            GUILayout.Label("Take note. The configuration from \"Assets/Resources/Netframe/Config.json\" will be loaded in runtime. If the configuration is not found, the default settings from the \"NetframeConfigs.cs\" class will be used.", EditorStyles.helpBox);
            EditorGUILayout.Space();
            
            // Queue Settings
            GUILayout.Label ("General Settings:", EditorStyles.boldLabel);
            Configuration.debugMode = EditorGUILayout.Toggle(
                new GUIContent("Enable Network Debug?", "Enable Network Requests/Response Logging"),
                Configuration.debugMode);
            Configuration.cacheLifetime = EditorGUILayout.IntField(new GUIContent("Cache Lifetime", "Request Cache Lifetime in Seconds"),
                Configuration.cacheLifetime);
            EditorGUILayout.Space();
            
            GUILayout.Label ("Queue Settings:", EditorStyles.boldLabel);
            Configuration.saveQueueBetweenSessions = EditorGUILayout.Toggle(
                new GUIContent("Save Between Sessions?", "Save or reset the queue at the end of the session (for example, when quitting the game)?"),
                Configuration.saveQueueBetweenSessions);
            Configuration.maxRequestQueue = EditorGUILayout.IntSlider(new GUIContent("Max Request Queue", "Maximum number of requests per sending from the queue"),
                Configuration.maxRequestQueue, 1, 200);
            Configuration.queueRequestsInterval = EditorGUILayout.Slider(new GUIContent("Queue Request Interval", "Interval between requests to send from the queue"),
                Configuration.queueRequestsInterval, 0.1f, 50f);
            Configuration.queueMaxAttempts = EditorGUILayout.IntSlider(new GUIContent("Max Queue Request Attempts", "Maximum number of attempts when sending a request from the queue"),
                Configuration.maxRequestQueue, 1, 200);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            // Save Configuration
            if (GUILayout.Button("Save Configuration"))
            {
                SaveSettings();
                Debug.Log($"<b>Unity Netframe Settings saved to:</b> \"Assets/Resources/{configsPath}.json\"");
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Load Configuration Settings
        /// </summary>
        private void LoadSettings()
        {
            TextAsset configAsset = Resources.Load<TextAsset>(configsPath);
            if (configAsset == null)
            {
                Configuration = new NetframeConfig();
                SaveSettings();
            }
            else
            {
                Configuration = JsonUtility.FromJson<NetframeConfig>(configAsset.text);
            }
        }

        /// <summary>
        /// Save Settings to Resources
        /// </summary>
        private void SaveSettings()
        {
            string configToSave = JsonUtility.ToJson(Configuration);
            using (FileStream fs = new FileStream("Assets/Resources/" + configsPath + ".json", FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(fs)){
                    writer.Write(configToSave);
                }
            }
            UnityEditor.AssetDatabase.Refresh ();
        }

        #region Window Events
        /// <summary>
        /// On Selection Changed
        /// </summary>
        private void OnSelectionChange() { LoadSettings(); Repaint(); }
        
        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable() { LoadSettings(); }
        
        /// <summary>
        /// On Focus
        /// </summary>
        private void OnFocus() { LoadSettings(); }
        #endregion

        #region Menu Items
        /// <summary>
        /// Show Window
        /// </summary>
        [MenuItem ("UnityNetframe/Setup")]
        public static void ShowWindow() {
            setupWindow = GetWindow<NetframeSetup>(false, "Unity Neframe Setup", true);
            setupWindow.Show();
            setupWindow.LoadSettings();
        }
        #endregion
    }

    /// <summary>
    /// Netframe Setup Initializer
    /// </summary>
    [InitializeOnLoad]
    public class NetframeSetupIntializer
    {
        // Private Params
        private static NetframeSetup SetupWindow;
        private static string configsPath = "Netframe/Config";
        
        /// <summary>
        /// Initialize Setup Handler
        /// </summary>
        static NetframeSetupIntializer()
        {
            EditorApplication.update += Startup;
        }
        
        /// <summary>
        /// Check Configurations and Launch Window
        /// </summary>
        static void Startup()
        {
            EditorApplication.update -= Startup;
         
            // Check Resources Exists
            TextAsset configAsset = Resources.Load<TextAsset>(configsPath);
            if (configAsset == null)
            {
                NetframeSetup.ShowWindow();
            }
        }
    }
}