//  VideoRender.cs
//
//  Created by YuGuo Chen on October 9, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

#define __UNITY__
#if __UNITY__

using System;

namespace agora.rtc
{
    using IrisVideoFrameBufferHandle = IntPtr;
    
    internal abstract class IVideoStreamManager
    {
        internal abstract int EnableVideoFrameBuffer(int width, int height, uint uid, string channel_id = "");

        internal abstract void DisableVideoFrameBuffer(uint uid = 0, string channel_id = "");

        internal abstract bool GetVideoFrame(ref IrisVideoFrame video_frame,
            ref bool is_new_frame, uint uid, string channel_id = "");
    }

    internal class VideoStreamManager : IVideoStreamManager, IDisposable
    {
        private IAgoraRtcEngine _agoraRtcEngine;
        private IrisCVideoFrameBufferNative _videoFrameBuffer;
        private IrisVideoFrameBufferHandle _irisVideoFrameBufferHandle;

        private IntPtr videoFrameBufferManagerPtr;

        private bool _disposed;

        public VideoStreamManager(IAgoraRtcEngine agoraRtcEngine)
        {
            _agoraRtcEngine = agoraRtcEngine;
            videoFrameBufferManagerPtr = AgoraRtcNative.CreateIrisVideoFrameBufferManager();
        }

        ~VideoStreamManager()
        {
            AgoraRtcNative.FreeIrisVideoFrameBufferManager(videoFrameBufferManagerPtr);
            Dispose();
        }

        internal override int EnableVideoFrameBuffer(int width, int height, uint uid, string channel_id = "")
        {
            if (_agoraRtcEngine == null)
            {
                AgoraLog.LogError(string.Format("EnableVideoFrameCache ret: ${0}", ERROR_CODE_TYPE.ERR_NOT_INITIALIZED));
                return (int)ERROR_CODE_TYPE.ERR_NOT_INITIALIZED;
            }

            IntPtr irisEngine = (_agoraRtcEngine as AgoraRtcEngine).GetNativeHandler();
            //IntPtr videoFrameBufferManagerPtr = (_agoraRtcEngine as AgoraRtcEngine).Getppppp();

            if (irisEngine != IntPtr.Zero)
            {
                var rawDataPtr = AgoraRtcNative.GetIrisRtcRawData(irisEngine);
                //var videoFrameBufferManagerPtr = AgoraRtcNative.CreateIrisVideoFrameBufferManager();
                AgoraRtcNative.Attach(rawDataPtr, videoFrameBufferManagerPtr);
                _videoFrameBuffer = new IrisCVideoFrameBufferNative {
                    type = (int)VIDEO_FRAME_TYPE.FRAME_TYPE_RGBA,
                    OnVideoFrameReceived = IntPtr.Zero,
                    resize_width = width,
                    resize_height = height
                };
                _irisVideoFrameBufferHandle = AgoraRtcNative.EnableVideoFrameBuffer(videoFrameBufferManagerPtr, ref _videoFrameBuffer, uid, channel_id);
                //AgoraRtcNative.FreeIrisVideoFrameBufferManager(videoFrameBufferManagerPtr);
                return (int)ERROR_CODE_TYPE.ERR_OK;
            }
            return (int)ERROR_CODE_TYPE.ERR_NOT_INITIALIZED;
        }

        internal override void DisableVideoFrameBuffer(uint uid = 0, string channel_id = "")
        {
            if (_agoraRtcEngine == null)
            {
                AgoraLog.LogError(string.Format("EnableVideoFrameCache ret: ${0}", ERROR_CODE_TYPE.ERR_NOT_INITIALIZED));
                return;
            }

            IntPtr irisEngine = (_agoraRtcEngine as AgoraRtcEngine).GetNativeHandler();
            //IntPtr videoFrameBufferManagerPtr = (_agoraRtcEngine as AgoraRtcEngine).Getppppp();

            if (irisEngine != IntPtr.Zero)
            {
                var rawDataPtr = AgoraRtcNative.GetIrisRtcRawData(irisEngine);
                //var videoFrameBufferManagerPtr = AgoraRtcNative.CreateIrisVideoFrameBufferManager();
                AgoraRtcNative.DisableVideoFrameBufferByUid(videoFrameBufferManagerPtr, uid, channel_id);
                AgoraRtcNative.Detach(rawDataPtr, videoFrameBufferManagerPtr);
                //AgoraRtcNative.FreeIrisVideoFrameBufferManager(videoFrameBufferManagerPtr);
            }
        }

        internal override bool GetVideoFrame(ref IrisVideoFrame video_frame, ref bool is_new_frame, uint uid, string channel_id = "")
        {
            if (_agoraRtcEngine == null)
            {
                AgoraLog.LogError(string.Format("EnableVideoFrameCache ret: ${0}", ERROR_CODE_TYPE.ERR_NOT_INITIALIZED));
                return false;
            }

            IntPtr irisEngine = (_agoraRtcEngine as AgoraRtcEngine).GetNativeHandler();
            //IntPtr videoFrameBufferManagerPtr = (_agoraRtcEngine as AgoraRtcEngine).Getppppp();

            if (irisEngine != IntPtr.Zero)
            {
                //var rawDataPtr = AgoraRtcNative.GetIrisRtcRawData(irisEngine);
                //var videoFrameBufferManagerPtr = AgoraRtcNative.CreateIrisVideoFrameBufferManager();
                //AgoraRtcNative.Attach(rawDataPtr, videoFrameBufferManagerPtr);
                return AgoraRtcNative.GetVideoFrame(videoFrameBufferManagerPtr, ref video_frame, out is_new_frame, uid, channel_id);
                //AgoraRtcNative.FreeIrisVideoFrameBufferManager(videoFrameBufferManagerPtr);
            }
            return false;
        }

        internal void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _agoraRtcEngine = null;
                _disposed = true;
            } 
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

#endif