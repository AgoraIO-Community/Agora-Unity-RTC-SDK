//  AgoraRtcVideoFrameObserver.cs
//
//  Created by Yiqing Huang on June 9, 2021.
//  Modified by Yiqing Huang on June 24, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    internal sealed class RtcVideoFrameObserverNative
    {
        private IAgoraRtcVideoFrameObserver _videoFrameObserver;
        private LocalVideoFrames _localVideoFrames = new LocalVideoFrames();

        private class LocalVideoFrames
        {
            internal readonly VideoFrame CaptureVideoFrame = new VideoFrame();
            internal readonly VideoFrame PreEncodeVideoFrame = new VideoFrame();

            internal readonly Dictionary<string, Dictionary<uint, VideoFrame>> RenderVideoFrameEx =
                new Dictionary<string, Dictionary<uint, VideoFrame>>();
        }

        internal void SetVideoFrameObserver(IAgoraRtcVideoFrameObserver videoFrameObserver)
        {
            _videoFrameObserver = videoFrameObserver;
        }

        private VideoFrame ProcessVideoFrameReceived(ref IrisRtcVideoFrame videoFrame, string channelId, uint uid)
        {
            var localVideoFrame = new VideoFrame();

            var ifConverted = _videoFrameObserver.GetVideoFormatPreference() != VIDEO_FRAME_TYPE.FRAME_TYPE_YUV420;
            var videoFrameConverted = ifConverted
                ? AgoraRtcNative.ConvertVideoFrame(ref videoFrame, _videoFrameObserver.GetVideoFormatPreference())
                : videoFrame;

            if (channelId == "")
            {
                switch (uid)
                {
                    case 0:
                        localVideoFrame = _localVideoFrames.CaptureVideoFrame;
                        break;
                    case 1:
                        localVideoFrame = _localVideoFrames.PreEncodeVideoFrame;
                        break;
                }
            }
            else
            {
                if (_localVideoFrames.RenderVideoFrameEx[channelId] == null)
                {
                    _localVideoFrames.RenderVideoFrameEx[channelId] = new Dictionary<uint, VideoFrame>
                        {[uid] = new VideoFrame()};
                }
                else if (_localVideoFrames.RenderVideoFrameEx[channelId][uid] == null)
                {
                    _localVideoFrames.RenderVideoFrameEx[channelId][uid] = new VideoFrame();
                }

                localVideoFrame = _localVideoFrames.RenderVideoFrameEx[channelId][uid];
            }

            if (localVideoFrame.height != videoFrameConverted.height ||
                localVideoFrame.yStride != videoFrameConverted.y_stride ||
                localVideoFrame.uStride != videoFrameConverted.u_stride ||
                localVideoFrame.vStride != videoFrameConverted.v_stride)
            {
                localVideoFrame.yBuffer = new byte[videoFrameConverted.y_buffer_length];
                localVideoFrame.uBuffer = new byte[videoFrameConverted.u_buffer_length];
                localVideoFrame.vBuffer = new byte[videoFrameConverted.v_buffer_length];
            }

            if (videoFrameConverted.y_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.y_buffer, localVideoFrame.yBuffer, 0,
                    (int) videoFrameConverted.y_buffer_length);
            if (videoFrameConverted.u_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.u_buffer, localVideoFrame.uBuffer, 0,
                    (int) videoFrameConverted.u_buffer_length);
            if (videoFrameConverted.v_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.v_buffer, localVideoFrame.vBuffer, 0,
                    (int) videoFrameConverted.v_buffer_length);
            localVideoFrame.width = videoFrameConverted.width;
            localVideoFrame.height = videoFrameConverted.height;
            localVideoFrame.yStride = videoFrameConverted.y_stride;
            localVideoFrame.uStride = videoFrameConverted.u_stride;
            localVideoFrame.vStride = videoFrameConverted.v_stride;
            localVideoFrame.rotation = videoFrameConverted.rotation;
            localVideoFrame.renderTimeMs = videoFrameConverted.render_time_ms;
            localVideoFrame.avsync_type = videoFrameConverted.av_sync_type;

            if (ifConverted) AgoraRtcNative.ClearVideoFrame(ref videoFrameConverted);

            return localVideoFrame;
        }

        internal bool OnCaptureVideoFrame(ref IrisRtcVideoFrame videoFrame)
        {
            return _videoFrameObserver == null ||
                   _videoFrameObserver.OnCaptureVideoFrame(ProcessVideoFrameReceived(ref videoFrame, "", 0));
        }

        internal bool OnPreEncodeVideoFrame(ref IrisRtcVideoFrame videoFrame)
        {
            return _videoFrameObserver == null ||
                   _videoFrameObserver.OnPreEncodeVideoFrame(ProcessVideoFrameReceived(ref videoFrame, "", 1));
        }

        internal bool OnRenderVideoFrame(uint uid, ref IrisRtcVideoFrame videoFrame)
        {
            return true;
        }

        internal uint GetObservedFramePosition()
        {
            if (_videoFrameObserver == null)
                return (uint) (VIDEO_OBSERVER_POSITION.POSITION_POST_CAPTURER |
                               VIDEO_OBSERVER_POSITION.POSITION_PRE_RENDERER);

            return (uint) _videoFrameObserver.GetObservedFramePosition();
        }

        internal bool IsMultipleChannelFrameWanted()
        {
            return _videoFrameObserver?.IsMultipleChannelFrameWanted() ?? true;
        }

        internal bool OnRenderVideoFrameEx(string channelId, uint uid, ref IrisRtcVideoFrame videoFrame)
        {
            if (_videoFrameObserver == null) return true;

            return _videoFrameObserver.OnRenderVideoFrameEx(channelId, uid,
                ProcessVideoFrameReceived(ref videoFrame, channelId, uid));
        }

        internal void Dispose()
        {
            _videoFrameObserver = null;
            _localVideoFrames = null;
        }
    }
}