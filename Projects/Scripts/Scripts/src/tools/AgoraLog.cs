//  AgoraLog.cs
//
//  Created by Yiqing Huang on June 2, 2021.
//  Modified by Yiqing Huang on June 2, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//


using UnityEngine;

namespace agora_gaming_rtc
{
    internal class AgoraLog
    {
        private const string AgoraMsgTag = "[Agora]: ";

        internal static void Log(string msg)
        {
            Debug.LogFormat("{0} {1}\n", AgoraMsgTag, msg);
        }

        internal static void LogWarning(string warningMsg)
        {
            Debug.LogWarningFormat("{0} {1}\n", AgoraMsgTag, warningMsg);
        }

        internal static void LogError(string errorMsg)
        {
            Debug.LogErrorFormat("{0} {1}\n", AgoraMsgTag, errorMsg);
        }
    }
}