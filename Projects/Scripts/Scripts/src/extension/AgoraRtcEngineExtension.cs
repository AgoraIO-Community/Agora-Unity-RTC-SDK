//  AgoraRtcEngineExtension.cs
//
//  Created by Yiqing Huang on July 1, 2021.
//  Modified by Yiqing Huang on July 1, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    public static class AgoraRtcEngineExtension
    {
        public static AgoraDisplayInfo[] GetDisplayInfos(this IAgoraRtcEngine agoraRtcEngine)
        {
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            var displayCollectionPtr = AgoraRtcNative.EnumerateDisplays();
            var displayCollection =
                (IrisDisplayCollection) (Marshal.PtrToStructure(displayCollectionPtr, typeof(IrisDisplayCollection)) ??
                                         new IrisDisplayCollection());
            var displayInfos = new AgoraDisplayInfo[displayCollection.length];
            for (var i = 0; i < displayCollection.length; i++)
            {
                var display =
                    (IrisDisplay) (Marshal.PtrToStructure(
                        displayCollection.displays + Marshal.SizeOf(typeof(IrisDisplay)) * i,
                        typeof(IrisDisplay)) ?? new IrisDisplay());
                displayInfos[i] = new AgoraDisplayInfo(display.id, display.bounds, display.work_area);
            }

            AgoraRtcNative.FreeIrisDisplayCollection(displayCollectionPtr);

            return displayInfos;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static AgoraWindowInfo[] GetWindowInfos(this IAgoraRtcEngine agoraRtcEngine)
        {
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            var windowCollectionPtr = AgoraRtcNative.EnumerateWindows();
            var windowCollection =
                (IrisWindowCollection) (Marshal.PtrToStructure(windowCollectionPtr, typeof(IrisWindowCollection)));
            var windowInfos = new AgoraWindowInfo[windowCollection.length];
            for (var i = 0; i < windowCollection.length; i++)
            {
                var window =
                    (IrisWindow) (Marshal.PtrToStructure(
                                      windowCollection.windows + Marshal.SizeOf(typeof(IrisWindow)) * i,
                                      typeof(IrisWindow)) ??
                                  new IrisWindow());
                windowInfos[i] = new AgoraWindowInfo(window.id, window.name, window.owner_name, window.bounds,
                    window.work_area);
            }

            AgoraRtcNative.FreeIrisWindowCollection(windowCollectionPtr);

            return windowInfos;
#else
            throw new PlatformNotSupportedException();
#endif
        }
    }

    public class AgoraWindowInfo
    {
        internal AgoraWindowInfo(ulong id, string name, string ownerName, IrisRect bounds,
            IrisRect workArea)
        {
            WindowId = id;
            WindowName = name;
            AppName = ownerName;
            Bounds = new Rectangle(Convert.ToInt32(bounds.x), Convert.ToInt32(bounds.y),
                Convert.ToInt32(bounds.width), Convert.ToInt32(bounds.height));
            WorkArea = new Rectangle(Convert.ToInt32(workArea.x), Convert.ToInt32(workArea.y),
                Convert.ToInt32(workArea.width), Convert.ToInt32(workArea.height));
        }

        public ulong WindowId { get; }
        public string WindowName { get; }
        public string AppName { get; }
        public Rectangle Bounds { get; }
        public Rectangle WorkArea { get; }
    }

    public class AgoraDisplayInfo
    {
        internal AgoraDisplayInfo(uint id, IrisRect bounds, IrisRect workArea)
        {
            DisplayId = id;
            Bounds = new Rectangle(Convert.ToInt32(bounds.x), Convert.ToInt32(bounds.y),
                Convert.ToInt32(bounds.width), Convert.ToInt32(bounds.height));
            WorkArea = new Rectangle(Convert.ToInt32(workArea.x), Convert.ToInt32(workArea.y),
                Convert.ToInt32(workArea.width), Convert.ToInt32(workArea.height));
        }

        public uint DisplayId { get; }
        public Rectangle Bounds { get; }
        public Rectangle WorkArea { get; }
    }
}