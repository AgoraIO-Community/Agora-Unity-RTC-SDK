//  AgoraRtcAudioFrameObserver.cs
//
//  Created by Yiqing Huang on June 9, 2021.
//  Modified by Yiqing Huang on June 9, 2021.
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

        private Dictionary<string, Dictionary<uint, AudioFrame>> _audioFrameChannelUidDict =
            new Dictionary<string, Dictionary<uint, AudioFrame>>();

        internal void SetAudioFrameObserver(IAgoraRtcAudioFrameObserver audioFrameObserver)
        {
            _audioFrameObserver = audioFrameObserver;
        }

        internal bool OnRecordAudioFrame(ref IrisRtcAudioFrame audioFrame)
        {
            if (_audioFrameObserver == null) return true;

            if (_audioFrameChannelUidDict[""] == null)
            {
                _audioFrameChannelUidDict[""] = new Dictionary<uint, AudioFrame> {[0] = new AudioFrame()};
            }
            else if (_audioFrameChannelUidDict[""][0] == null)
            {
                _audioFrameChannelUidDict[""][0] = new AudioFrame();
            }

            if (_audioFrameChannelUidDict[""][0].channels != audioFrame.channels ||
                _audioFrameChannelUidDict[""][0].samples != audioFrame.samples ||
                _audioFrameChannelUidDict[""][0].bytesPerSample != audioFrame.bytes_per_sample)
            {
                _audioFrameChannelUidDict[""][0].buffer = new byte[audioFrame.buffer_length];
            }

            if (audioFrame.buffer != IntPtr.Zero)
                Marshal.Copy(audioFrame.buffer, _audioFrameChannelUidDict[""][0].buffer, 0,
                    (int) audioFrame.buffer_length);
            _audioFrameChannelUidDict[""][0].type = audioFrame.type;
            _audioFrameChannelUidDict[""][0].samples = audioFrame.samples;
            _audioFrameChannelUidDict[""][0].bytesPerSample = audioFrame.bytes_per_sample;
            _audioFrameChannelUidDict[""][0].channels = audioFrame.channels;
            _audioFrameChannelUidDict[""][0].samplesPerSec = audioFrame.samples_per_sec;
            _audioFrameChannelUidDict[""][0].renderTimeMs = audioFrame.render_time_ms;
            _audioFrameChannelUidDict[""][0].avsync_type = audioFrame.av_sync_type;

            return _audioFrameObserver.OnRecordAudioFrame(_audioFrameChannelUidDict[""][0]);
        }

        internal bool OnPlaybackAudioFrame(ref IrisRtcAudioFrame audioFrame)
        {
            if (_audioFrameObserver == null) return true;

            if (_audioFrameChannelUidDict[""] == null)
            {
                _audioFrameChannelUidDict[""] = new Dictionary<uint, AudioFrame> {[1] = new AudioFrame()};
            }
            else if (_audioFrameChannelUidDict[""][1] == null)
            {
                _audioFrameChannelUidDict[""][1] = new AudioFrame();
            }

            if (_audioFrameChannelUidDict[""][1].channels != audioFrame.channels ||
                _audioFrameChannelUidDict[""][1].samples != audioFrame.samples ||
                _audioFrameChannelUidDict[""][1].bytesPerSample != audioFrame.bytes_per_sample)
            {
                _audioFrameChannelUidDict[""][1].buffer = new byte[audioFrame.buffer_length];
            }

            if (audioFrame.buffer != IntPtr.Zero)
                Marshal.Copy(audioFrame.buffer, _audioFrameChannelUidDict[""][1].buffer, 0,
                    (int) audioFrame.buffer_length);
            _audioFrameChannelUidDict[""][1].type = audioFrame.type;
            _audioFrameChannelUidDict[""][1].samples = audioFrame.samples;
            _audioFrameChannelUidDict[""][1].bytesPerSample = audioFrame.bytes_per_sample;
            _audioFrameChannelUidDict[""][1].channels = audioFrame.channels;
            _audioFrameChannelUidDict[""][1].samplesPerSec = audioFrame.samples_per_sec;
            _audioFrameChannelUidDict[""][1].renderTimeMs = audioFrame.render_time_ms;
            _audioFrameChannelUidDict[""][1].avsync_type = audioFrame.av_sync_type;

            return _audioFrameObserver.OnPlaybackAudioFrame(_audioFrameChannelUidDict[""][1]);
        }

        internal bool OnMixedAudioFrame(ref IrisRtcAudioFrame audioFrame)
        {
            if (_audioFrameObserver == null) return true;

            if (_audioFrameChannelUidDict[""] == null)
            {
                _audioFrameChannelUidDict[""] = new Dictionary<uint, AudioFrame> {[2] = new AudioFrame()};
            }
            else if (_audioFrameChannelUidDict[""][2] == null)
            {
                _audioFrameChannelUidDict[""][2] = new AudioFrame();
            }

            if (_audioFrameChannelUidDict[""][2].channels != audioFrame.channels ||
                _audioFrameChannelUidDict[""][2].samples != audioFrame.samples ||
                _audioFrameChannelUidDict[""][2].bytesPerSample != audioFrame.bytes_per_sample)
            {
                _audioFrameChannelUidDict[""][2].buffer = new byte[audioFrame.buffer_length];
            }

            if (audioFrame.buffer != IntPtr.Zero)
                Marshal.Copy(audioFrame.buffer, _audioFrameChannelUidDict[""][2].buffer, 0,
                    (int) audioFrame.buffer_length);
            _audioFrameChannelUidDict[""][2].type = audioFrame.type;
            _audioFrameChannelUidDict[""][2].samples = audioFrame.samples;
            _audioFrameChannelUidDict[""][2].bytesPerSample = audioFrame.bytes_per_sample;
            _audioFrameChannelUidDict[""][2].channels = audioFrame.channels;
            _audioFrameChannelUidDict[""][2].samplesPerSec = audioFrame.samples_per_sec;
            _audioFrameChannelUidDict[""][2].renderTimeMs = audioFrame.render_time_ms;
            _audioFrameChannelUidDict[""][2].avsync_type = audioFrame.av_sync_type;

            return _audioFrameObserver.OnMixedAudioFrame(_audioFrameChannelUidDict[""][2]);
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
            if (_audioFrameObserver == null) return true;

            if (_audioFrameChannelUidDict[channelId] == null)
            {
                _audioFrameChannelUidDict[channelId] = new Dictionary<uint, AudioFrame> {[uid] = new AudioFrame()};
            }
            else if (_audioFrameChannelUidDict[channelId][uid] == null)
            {
                _audioFrameChannelUidDict[channelId][uid] = new AudioFrame();
            }

            if (_audioFrameChannelUidDict[channelId][uid].channels != audioFrame.channels ||
                _audioFrameChannelUidDict[channelId][uid].samples != audioFrame.samples ||
                _audioFrameChannelUidDict[channelId][uid].bytesPerSample != audioFrame.bytes_per_sample)
            {
                _audioFrameChannelUidDict[channelId][uid].buffer = new byte[audioFrame.buffer_length];
            }

            if (audioFrame.buffer != IntPtr.Zero)
                Marshal.Copy(audioFrame.buffer, _audioFrameChannelUidDict[channelId][uid].buffer, 0,
                    (int) audioFrame.buffer_length);
            _audioFrameChannelUidDict[channelId][uid].type = audioFrame.type;
            _audioFrameChannelUidDict[channelId][uid].samples = audioFrame.samples;
            _audioFrameChannelUidDict[channelId][uid].bytesPerSample = audioFrame.bytes_per_sample;
            _audioFrameChannelUidDict[channelId][uid].channels = audioFrame.channels;
            _audioFrameChannelUidDict[channelId][uid].samplesPerSec = audioFrame.samples_per_sec;
            _audioFrameChannelUidDict[channelId][uid].renderTimeMs = audioFrame.render_time_ms;
            _audioFrameChannelUidDict[channelId][uid].avsync_type = audioFrame.av_sync_type;

            return _audioFrameObserver.OnPlaybackAudioFrameBeforeMixingEx(channelId, uid,
                _audioFrameChannelUidDict[channelId][uid]);
        }

        internal void Dispose()
        {
            _audioFrameObserver = null;
            _audioFrameChannelUidDict = null;
        }
    }
}