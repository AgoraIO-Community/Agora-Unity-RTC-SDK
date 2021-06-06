//  AgoraCallbackObject.cs
//
//  Created by Tao Zhang.
//  Modified by Yiqing Huang on June 5, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace agora_gaming_rtc
{
    internal sealed class AgoraCallbackObject
    {
        private GameObject _CallbackGameObject { get; set; }
        internal AgoraCallbackQueue _CallbackQueue { set; get; }
        internal string GameObjectName { set; get; }

        internal AgoraCallbackObject(string gameObjectName)
        {
            InitGameObject(gameObjectName);
        }

        internal void Release()
        {
            if (!ReferenceEquals(_CallbackGameObject, null))
            {
                if (!ReferenceEquals(_CallbackQueue, null))
                {
                    _CallbackQueue.ClearQueue();
                }

                Object.Destroy(_CallbackGameObject);
                _CallbackGameObject = null;
                _CallbackQueue = null;
            }
        }

        private void InitGameObject(string gameObjectName)
        {
            DeInitGameObject(gameObjectName);
            _CallbackGameObject = new GameObject(gameObjectName);
            _CallbackQueue = _CallbackGameObject.AddComponent<AgoraCallbackQueue>();
            Object.DontDestroyOnLoad(_CallbackGameObject);
            _CallbackGameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private void DeInitGameObject(string gameObjectName)
        {
            var gameObject = GameObject.Find(gameObjectName);
            if (!ReferenceEquals(gameObject, null))
            {
                AgoraCallbackQueue callbackQueue = gameObject.GetComponent<AgoraCallbackQueue>();
                if (!ReferenceEquals(callbackQueue, null))
                {
                    callbackQueue.ClearQueue();
                }

                Object.Destroy(gameObject);
            }
        }
    }
}