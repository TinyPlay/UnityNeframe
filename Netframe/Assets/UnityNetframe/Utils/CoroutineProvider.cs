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
namespace UnityNetframe.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    /// <summary>
    /// Coroutine Provider
    /// </summary>
    public class CoroutineProvider : MonoBehaviour
    {
        static CoroutineProvider _singleton;
        static Dictionary<string,IEnumerator> _routines = new Dictionary<string,IEnumerator>(100);

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
        static void InitializeType ()
        {
            _singleton = new GameObject($"#{nameof(CoroutineProvider)}").AddComponent<CoroutineProvider>();
            DontDestroyOnLoad( _singleton );
        }

        public static Coroutine Start ( IEnumerator routine ) => _singleton.StartCoroutine( routine );
        public static Coroutine Start ( IEnumerator routine , string id )
        {
            var coroutine = _singleton.StartCoroutine( routine );
            if( !_routines.ContainsKey(id) ) _routines.Add( id , routine );
            else
            {
                _singleton.StopCoroutine( _routines[id] );
                _routines[id] = routine;
            }
            return coroutine;
        }
        public static void Stop ( IEnumerator routine ) => _singleton.StopCoroutine( routine );
        public static void Stop ( string id )
        {
            if( _routines.TryGetValue(id,out var routine) )
            {
                _singleton.StopCoroutine( routine );
                _routines.Remove( id );
            }
            else Debug.LogWarning($"coroutine '{id}' not found");
        }
        public static void StopAll () => _singleton.StopAllCoroutines();
    }
}