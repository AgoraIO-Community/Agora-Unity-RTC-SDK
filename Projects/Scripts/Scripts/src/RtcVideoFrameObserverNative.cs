//  AgoraRtcVideoFrameObserver.cs
//
//  Created by Yiqing Huang on June 9, 2021.
//  Modified by Yiqing Huang on June 10, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    internal sealed class RtcVideoFrameObserverNative
    {
        private IAgoraRtcVideoFrameObserver _videoFrameObserver;

        private Dictionary<string, Dictionary<uint, VideoFrame>> _videoFrameChannelUidDict =
            new Dictionary<string, Dictionary<uint, VideoFrame>>();

        internal void SetVideoFrameObserver(IAgoraRtcVideoFrameObserver videoFrameObserver)
        {
            _videoFrameObserver = videoFrameObserver;
        }

        internal bool OnCaptureVideoFrame(ref IrisRtcVideoFrame videoFrame)
        {
            if (_videoFrameObserver == null) return true;

            var videoFrameConverted =
                _videoFrameObserver.GetVideoFormatPreference() == VIDEO_FRAME_TYPE.FRAME_TYPE_YUV420
                    ? videoFrame
                    : AgoraRtcNative.ConvertVideoFrame(ref videoFrame, _videoFrameObserver.GetVideoFormatPreference());

            if (_videoFrameChannelUidDict[""] == null)
            {
                _videoFrameChannelUidDict[""] = new Dictionary<uint, VideoFrame> {[0] = new VideoFrame()};
            }
            else if (_videoFrameChannelUidDict[""][0] == null)
            {
                _videoFrameChannelUidDict[""][0] = new VideoFrame();
            }

            if (_videoFrameChannelUidDict[""][0].height != videoFrameConverted.height ||
                _videoFrameChannelUidDict[""][0].yStride != videoFrameConverted.y_stride ||
                _videoFrameChannelUidDict[""][0].uStride != videoFrameConverted.u_stride ||
                _videoFrameChannelUidDict[""][0].vStride != videoFrameConverted.v_stride)
            {
                _videoFrameChannelUidDict[""][0].yBuffer = new byte[videoFrameConverted.y_buffer_length];
                _videoFrameChannelUidDict[""][0].uBuffer = new byte[videoFrameConverted.u_buffer_length];
                _videoFrameChannelUidDict[""][0].vBuffer = new byte[videoFrameConverted.v_buffer_length];
            }

            if (videoFrameConverted.y_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.y_buffer, _videoFrameChannelUidDict[""][0].yBuffer, 0,
                    (int) videoFrameConverted.y_buffer_length);
            if (videoFrameConverted.u_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.u_buffer, _videoFrameChannelUidDict[""][0].uBuffer, 0,
                    (int) videoFrameConverted.u_buffer_length);
            if (videoFrameConverted.v_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.v_buffer, _videoFrameChannelUidDict[""][0].vBuffer, 0,
                    (int) videoFrameConverted.v_buffer_length);
            _videoFrameChannelUidDict[""][0].width = videoFrameConverted.width;
            _videoFrameChannelUidDict[""][0].height = videoFrameConverted.height;
            _videoFrameChannelUidDict[""][0].yStride = videoFrameConverted.y_stride;
            _videoFrameChannelUidDict[""][0].uStride = videoFrameConverted.u_stride;
            _videoFrameChannelUidDict[""][0].vStride = videoFrameConverted.v_stride;
            _videoFrameChannelUidDict[""][0].rotation = videoFrameConverted.rotation;
            _videoFrameChannelUidDict[""][0].renderTimeMs = videoFrameConverted.render_time_ms;
            _videoFrameChannelUidDict[""][0].avsync_type = videoFrameConverted.av_sync_type;

            AgoraRtcNative.ClearVideoFrame(ref videoFrameConverted);

            return _videoFrameObserver.OnCaptureVideoFrame(_videoFrameChannelUidDict[""][0]);
        }

        internal bool OnPreEncodeVideoFrame(ref IrisRtcVideoFrame videoFrame)
        {
            if (_videoFrameObserver == null) return true;

            var videoFrameConverted =
                _videoFrameObserver.GetVideoFormatPreference() == VIDEO_FRAME_TYPE.FRAME_TYPE_YUV420
                    ? videoFrame
                    : AgoraRtcNative.ConvertVideoFrame(ref videoFrame, _videoFrameObserver.GetVideoFormatPreference());

            if (_videoFrameChannelUidDict[""] == null)
            {
                _videoFrameChannelUidDict[""] = new Dictionary<uint, VideoFrame> {[1] = new VideoFrame()};
            }
            else if (_videoFrameChannelUidDict[""][1] == null)
            {
                _videoFrameChannelUidDict[""][1] = new VideoFrame();
            }

            if (_videoFrameChannelUidDict[""][1].height != videoFrameConverted.height ||
                _videoFrameChannelUidDict[""][1].yStride != videoFrameConverted.y_stride ||
                _videoFrameChannelUidDict[""][1].uStride != videoFrameConverted.u_stride ||
                _videoFrameChannelUidDict[""][1].vStride != videoFrameConverted.v_stride)
            {
                _videoFrameChannelUidDict[""][1].yBuffer = new byte[videoFrameConverted.y_buffer_length];
                _videoFrameChannelUidDict[""][1].uBuffer = new byte[videoFrameConverted.u_buffer_length];
                _videoFrameChannelUidDict[""][1].vBuffer = new byte[videoFrameConverted.v_buffer_length];
            }

            if (videoFrameConverted.y_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.y_buffer, _videoFrameChannelUidDict[""][1].yBuffer, 0,
                    (int) videoFrameConverted.y_buffer_length);
            if (videoFrameConverted.u_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.u_buffer, _videoFrameChannelUidDict[""][1].uBuffer, 0,
                    (int) videoFrameConverted.u_buffer_length);
            if (videoFrameConverted.v_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.v_buffer, _videoFrameChannelUidDict[""][1].vBuffer, 0,
                    (int) videoFrameConverted.v_buffer_length);
            _videoFrameChannelUidDict[""][1].width = videoFrameConverted.width;
            _videoFrameChannelUidDict[""][1].height = videoFrameConverted.height;
            _videoFrameChannelUidDict[""][1].yStride = videoFrameConverted.y_stride;
            _videoFrameChannelUidDict[""][1].uStride = videoFrameConverted.u_stride;
            _videoFrameChannelUidDict[""][1].vStride = videoFrameConverted.v_stride;
            _videoFrameChannelUidDict[""][1].rotation = videoFrameConverted.rotation;
            _videoFrameChannelUidDict[""][1].renderTimeMs = videoFrameConverted.render_time_ms;
            _videoFrameChannelUidDict[""][1].avsync_type = videoFrameConverted.av_sync_type;

            AgoraRtcNative.ClearVideoFrame(ref videoFrameConverted);

            return _videoFrameObserver.OnPreEncodeVideoFrame(_videoFrameChannelUidDict[""][1]);
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

            var videoFrameConverted =
                _videoFrameObserver.GetVideoFormatPreference() == VIDEO_FRAME_TYPE.FRAME_TYPE_YUV420
                    ? videoFrame
                    : AgoraRtcNative.ConvertVideoFrame(ref videoFrame, _videoFrameObserver.GetVideoFormatPreference());

            if (_videoFrameChannelUidDict[channelId] == null)
            {
                _videoFrameChannelUidDict[channelId] = new Dictionary<uint, VideoFrame> {[uid] = new VideoFrame()};
            }
            else if (_videoFrameChannelUidDict[channelId][uid] == null)
            {
                _videoFrameChannelUidDict[channelId][uid] = new VideoFrame();
            }

            if (_videoFrameChannelUidDict[channelId][uid].height != videoFrameConverted.height ||
                _videoFrameChannelUidDict[channelId][uid].yStride != videoFrameConverted.y_stride ||
                _videoFrameChannelUidDict[channelId][uid].uStride != videoFrameConverted.u_stride ||
                _videoFrameChannelUidDict[channelId][uid].vStride != videoFrameConverted.v_stride)
            {
                _videoFrameChannelUidDict[channelId][uid].yBuffer = new byte[videoFrameConverted.y_buffer_length];
                _videoFrameChannelUidDict[channelId][uid].uBuffer = new byte[videoFrameConverted.u_buffer_length];
                _videoFrameChannelUidDict[channelId][uid].vBuffer = new byte[videoFrameConverted.v_buffer_length];
            }

            if (videoFrameConverted.y_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.y_buffer, _videoFrameChannelUidDict[channelId][uid].yBuffer, 0,
                    (int) videoFrameConverted.y_buffer_length);
            if (videoFrameConverted.u_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.u_buffer, _videoFrameChannelUidDict[channelId][uid].uBuffer, 0,
                    (int) videoFrameConverted.u_buffer_length);
            if (videoFrameConverted.v_buffer != IntPtr.Zero)
                Marshal.Copy(videoFrameConverted.v_buffer, _videoFrameChannelUidDict[channelId][uid].vBuffer, 0,
                    (int) videoFrameConverted.v_buffer_length);
            _videoFrameChannelUidDict[channelId][uid].width = videoFrameConverted.width;
            _videoFrameChannelUidDict[channelId][uid].height = videoFrameConverted.height;
            _videoFrameChannelUidDict[channelId][uid].yStride = videoFrameConverted.y_stride;
            _videoFrameChannelUidDict[channelId][uid].uStride = videoFrameConverted.u_stride;
            _videoFrameChannelUidDict[channelId][uid].vStride = videoFrameConverted.v_stride;
            _videoFrameChannelUidDict[channelId][uid].rotation = videoFrameConverted.rotation;
            _videoFrameChannelUidDict[channelId][uid].renderTimeMs = videoFrameConverted.render_time_ms;
            _videoFrameChannelUidDict[channelId][uid].avsync_type = videoFrameConverted.av_sync_type;

            AgoraRtcNative.ClearVideoFrame(ref videoFrameConverted);

            return _videoFrameObserver.OnPreEncodeVideoFrame(_videoFrameChannelUidDict[channelId][uid]);
        }

        internal void Dispose()
        {
            _videoFrameObserver = null;
            _videoFrameChannelUidDict = null;
        }
    }
}