//  AgoraRtcAudioFrameObserver.cs
//
//  Created by Yiqing Huang on June 9, 2021.
//  Modified by Yiqing Huang on June 24, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    internal sealed class RtcAudioFrameObserverNative
    {
        private IAgoraRtcAudioFrameObserver _audioFrameObserver;
        private LocalAudioFrames _localAudioFrames = new LocalAudioFrames();

        private class LocalAudioFrames
        {
            internal readonly AudioFrame RecordAudioFrame = new AudioFrame();
            internal readonly AudioFrame PlaybackAudioFrame = new AudioFrame();
            internal readonly AudioFrame MixedAudioFrame = new AudioFrame();

            internal readonly Dictionary<string, Dictionary<uint, AudioFrame>> AudioFrameBeforeMixingEx =
                new Dictionary<string, Dictionary<uint, AudioFrame>>();
        }

        internal void SetAudioFrameObserver(IAgoraRtcAudioFrameObserver audioFrameObserver)
        {
            _audioFrameObserver = audioFrameObserver;
        }

        private AudioFrame ProcessAudioFrameReceived(ref IrisRtcAudioFrame audioFrame, string channelId, uint uid)
        {
            var localAudioFrame = new AudioFrame();

            if (channelId == "")
            {
                // Local Audio Frame
                switch (uid)
                {
                    case 0:
                        localAudioFrame = _localAudioFrames.RecordAudioFrame;
                        break;
                    case 1:
                        localAudioFrame = _localAudioFrames.PlaybackAudioFrame;
                        break;
                    case 2:
                        localAudioFrame = _localAudioFrames.MixedAudioFrame;
                        break;
                }
            }
            else
            {
                // Remote Audio Frame
                if (_localAudioFrames.AudioFrameBeforeMixingEx[channelId] == null)
                {
                    _localAudioFrames.AudioFrameBeforeMixingEx[channelId] = new Dictionary<uint, AudioFrame>
                        {[uid] = new AudioFrame()};
                }
                else if (_localAudioFrames.AudioFrameBeforeMixingEx[channelId][uid] == null)
                {
                    _localAudioFrames.AudioFrameBeforeMixingEx[channelId][uid] = new AudioFrame();
                }

                localAudioFrame = _localAudioFrames.AudioFrameBeforeMixingEx[channelId][uid];
            }

            if (localAudioFrame.channels != audioFrame.channels ||
                localAudioFrame.samples != audioFrame.samples ||
                localAudioFrame.bytesPerSample != audioFrame.bytes_per_sample)
            {
                localAudioFrame.buffer = new byte[audioFrame.buffer_length];
            }

            if (audioFrame.buffer != IntPtr.Zero)
                Marshal.Copy(audioFrame.buffer, localAudioFrame.buffer, 0, (int) audioFrame.buffer_length);
            localAudioFrame.type = audioFrame.type;
            localAudioFrame.samples = audioFrame.samples;
            localAudioFrame.bytesPerSample = audioFrame.bytes_per_sample;
            localAudioFrame.channels = audioFrame.channels;
            localAudioFrame.samplesPerSec = audioFrame.samples_per_sec;
            localAudioFrame.renderTimeMs = audioFrame.render_time_ms;
            localAudioFrame.avsync_type = audioFrame.av_sync_type;

            return localAudioFrame;
        }

        internal bool OnRecordAudioFrame(ref IrisRtcAudioFrame audioFrame)
        {
            return _audioFrameObserver == null ||
                   _audioFrameObserver.OnRecordAudioFrame(ProcessAudioFrameReceived(ref audioFrame, "", 0));
        }

        internal bool OnPlaybackAudioFrame(ref IrisRtcAudioFrame audioFrame)
        {
            return _audioFrameObserver == null ||
                   _audioFrameObserver.OnPlaybackAudioFrame(ProcessAudioFrameReceived(ref audioFrame, "", 1));
        }

        internal bool OnMixedAudioFrame(ref IrisRtcAudioFrame audioFrame)
        {
            return _audioFrameObserver == null ||
                   _audioFrameObserver.OnMixedAudioFrame(ProcessAudioFrameReceived(ref audioFrame, "", 2));
        }

        internal bool OnPlaybackAudioFrameBeforeMixing(uint uid, ref IrisRtcAudioFrame audioFrame)
        {
            return true;
        }

        internal bool IsMultipleChannelFrameWanted()
        {
            return _audioFrameObserver?.IsMultipleChannelFrameWanted() ?? true;
        }

        internal bool OnPlaybackAudioFrameBeforeMixingEx(string channelId, uint uid, ref IrisRtcAudioFrame audioFrame)
        {
            return _audioFrameObserver == null || _audioFrameObserver.OnPlaybackAudioFrameBeforeMixingEx(channelId, uid,
                ProcessAudioFrameReceived(ref audioFrame, channelId, uid));
        }

        internal void Dispose()
        {
            _audioFrameObserver = null;
            _localAudioFrames = null;
        }
    }
}