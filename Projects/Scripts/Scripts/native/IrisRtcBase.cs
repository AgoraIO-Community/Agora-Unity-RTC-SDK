//  IrisRtcBase.cs
//
//  Created by Yiqing Huang on May 25, 2021.
//  Modified by Yiqing Huang on May 25, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    internal enum ApiTypeEngine
    {
        kEngineInitialize,
        kEngineRelease,
        kEngineSetChannelProfile,
        kEngineSetClientRole,
        kEngineJoinChannel,
        kEngineSwitchChannel,
        kEngineLeaveChannel,
        kEngineRenewToken,
        kEngineRegisterLocalUserAccount,
        kEngineJoinChannelWithUserAccount,
        kEngineGetUserInfoByUserAccount,
        kEngineGetUserInfoByUid,
        kEngineStartEchoTest,
        kEngineStopEchoTest,
        kEngineSetCloudProxy,
        kEngineEnableVideo,
        kEngineDisableVideo,
        kEngineSetVideoProfile,
        kEngineSetVideoEncoderConfiguration,
        kEngineSetCameraCapturerConfiguration,
        kEngineSetupLocalVideo,
        kEngineSetupRemoteVideo,
        kEngineStartPreview,
        kEngineSetRemoteUserPriority,
        kEngineStopPreview,
        kEngineEnableAudio,
        kEngineEnableLocalAudio,
        kEngineDisableAudio,
        kEngineSetAudioProfile,
        kEngineMuteLocalAudioStream,
        kEngineMuteAllRemoteAudioStreams,
        kEngineSetDefaultMuteAllRemoteAudioStreams,
        kEngineAdjustUserPlaybackSignalVolume,
        kEngineMuteRemoteAudioStream,
        kEngineMuteLocalVideoStream,
        kEngineEnableLocalVideo,
        kEngineMuteAllRemoteVideoStreams,
        kEngineSetDefaultMuteAllRemoteVideoStreams,
        kEngineMuteRemoteVideoStream,
        kEngineSetRemoteVideoStreamType,
        kEngineSetRemoteDefaultVideoStreamType,
        kEngineEnableAudioVolumeIndication,
        kEngineStartAudioRecording,
        kEngineStopAudioRecording,
        kEngineStartAudioMixing,
        kEngineStopAudioMixing,
        kEnginePauseAudioMixing,
        kEngineResumeAudioMixing,
        kEngineSetHighQualityAudioParameters,
        kEngineAdjustAudioMixingVolume,
        kEngineAdjustAudioMixingPlayoutVolume,
        kEngineGetAudioMixingPlayoutVolume,
        kEngineAdjustAudioMixingPublishVolume,
        kEngineGetAudioMixingPublishVolume,
        kEngineGetAudioMixingDuration,
        kEngineGetAudioMixingCurrentPosition,
        kEngineSetAudioMixingPosition,
        kEngineSetAudioMixingPitch,
        kEngineGetEffectsVolume,
        kEngineSetEffectsVolume,
        kEngineSetVolumeOfEffect,
        kEngineEnableFaceDetection,
        kEnginePlayEffect,
        kEngineStopEffect,
        kEngineStopAllEffects,
        kEnginePreloadEffect,
        kEngineUnloadEffect,
        kEnginePauseEffect,
        kEnginePauseAllEffects,
        kEngineResumeEffect,
        kEngineResumeAllEffects,
        kEngineEnableDeepLearningDenoise,
        kEngineEnableSoundPositionIndication,
        kEngineSetRemoteVoicePosition,
        kEngineSetLocalVoicePitch,
        kEngineSetLocalVoiceEqualization,
        kEngineSetLocalVoiceReverb,
        kEngineSetLocalVoiceChanger,
        kEngineSetLocalVoiceReverbPreset,
        kEngineSetVoiceBeautifierPreset,
        kEngineSetAudioEffectPreset,
        kEngineSetVoiceConversionPreset,
        kEngineSetAudioEffectParameters,
        kEngineSetVoiceBeautifierParameters,
        kEngineSetLogFile,
        kEngineSetLogFilter,
        kEngineSetLogFileSize,
        kEngineUploadLogFile,
        kEngineSetLocalRenderMode,
        kEngineSetRemoteRenderMode,
        kEngineSetLocalVideoMirrorMode,
        kEngineEnableDualStreamMode,
        kEngineSetExternalAudioSource,
        kEngineSetExternalAudioSink,
        kEngineSetRecordingAudioFrameParameters,
        kEngineSetPlaybackAudioFrameParameters,
        kEngineSetMixedAudioFrameParameters,
        kEngineAdjustRecordingSignalVolume,
        kEngineAdjustPlaybackSignalVolume,
        kEngineEnableWebSdkInteroperability,
        kEngineSetVideoQualityParameters,
        kEngineSetLocalPublishFallbackOption,
        kEngineSetRemoteSubscribeFallbackOption,
        kEngineSwitchCamera,
        kEngineSetDefaultAudioRouteToSpeakerPhone,
        kEngineSetEnableSpeakerPhone,
        kEngineEnableInEarMonitoring,
        kEngineSetInEarMonitoringVolume,
        kEngineIsSpeakerPhoneEnabled,
        kEngineSetAudioSessionOperationRestriction,
        kEngineEnableLoopBackRecording,
        kEngineStartScreenCaptureByDisplayId,
        kEngineStartScreenCaptureByScreenRect,
        kEngineStartScreenCaptureByWindowId,
        kEngineSetScreenCaptureContentHint,
        kEngineUpdateScreenCaptureParameters,
        kEngineUpdateScreenCaptureRegion,
        kEngineStopScreenCapture,
        kEngineStartScreenCapture,
        kEngineSetVideoSource,
        kEngineGetCallId,
        kEngineRate,
        kEngineComplain,
        kEngineGetVersion,
        kEngineEnableLastMileTest,
        kEngineDisableLastMileTest,
        kEngineStartLastMileProbeTest,
        kEngineStopLastMileProbeTest,
        kEngineGetErrorDescription,
        kEngineSetEncryptionSecret,
        kEngineSetEncryptionMode,
        kEngineEnableEncryption,
        kEngineRegisterPacketObserver,
        kEngineCreateDataStream,
        kEngineSendStreamMessage,
        kEngineAddPublishStreamUrl,
        kEngineRemovePublishStreamUrl,
        kEngineSetLiveTranscoding,
        kEngineAddVideoWaterMark,
        kEngineClearVideoWaterMarks,
        kEngineSetBeautyEffectOptions,
        kEngineAddInjectStreamUrl,
        kEngineStartChannelMediaRelay,
        kEngineUpdateChannelMediaRelay,
        kEngineStopChannelMediaRelay,
        kEngineRemoveInjectStreamUrl,
        kEngineSendCustomReportMessage,
        kEngineGetConnectionState,
        kEngineEnableRemoteSuperResolution,
        kEngineRegisterMediaMetadataObserver,
        kEngineSetParameters,

        kEngineUnRegisterMediaMetadataObserver,
        kEngineSetMaxMetadataSize,
        kEngineSendMetadata,
        kEngineSetAppType,

        kMediaPushAudioFrame,
        kMediaPullAudioFrame,
        kMediaSetExternalVideoSource,
        kMediaPushVideoFrame,
    }

    internal enum ApiTypeChannel
    {
        kChannelCreateChannel,
        kChannelRelease,
        kChannelJoinChannel,
        kChannelJoinChannelWithUserAccount,
        kChannelLeaveChannel,
        kChannelPublish,
        kChannelUnPublish,
        kChannelChannelId,
        kChannelGetCallId,
        kChannelRenewToken,
        kChannelSetEncryptionSecret,
        kChannelSetEncryptionMode,
        kChannelEnableEncryption,
        kChannelRegisterPacketObserver,
        kChannelRegisterMediaMetadataObserver,
        kChannelUnRegisterMediaMetadataObserver,
        kChannelSetMaxMetadataSize,
        kChannelSendMetadata,
        kChannelSetClientRole,
        kChannelSetRemoteUserPriority,
        kChannelSetRemoteVoicePosition,
        kChannelSetRemoteRenderMode,
        kChannelSetDefaultMuteAllRemoteAudioStreams,
        kChannelSetDefaultMuteAllRemoteVideoStreams,
        kChannelMuteAllRemoteAudioStreams,
        kChannelAdjustUserPlaybackSignalVolume,
        kChannelMuteRemoteAudioStream,
        kChannelMuteAllRemoteVideoStreams,
        kChannelMuteRemoteVideoStream,
        kChannelSetRemoteVideoStreamType,
        kChannelSetRemoteDefaultVideoStreamType,
        kChannelCreateDataStream,
        kChannelSendStreamMessage,
        kChannelAddPublishStreamUrl,
        kChannelRemovePublishStreamUrl,
        kChannelSetLiveTranscoding,
        kChannelAddInjectStreamUrl,
        kChannelRemoveInjectStreamUrl,
        kChannelStartChannelMediaRelay,
        kChannelUpdateChannelMediaRelay,
        kChannelStopChannelMediaRelay,
        kChannelGetConnectionState,
        kChannelEnableRemoteSuperResolution,
    }

    internal enum ApiTypeAudioDeviceManager
    {
        kGetAudioPlaybackDeviceCount,
        kGetAudioPlaybackDeviceInfoByIndex,
        kSetCurrentAudioPlaybackDeviceId,
        kGetCurrentAudioPlaybackDeviceId,
        kGetCurrentAudioPlaybackDeviceInfo,
        kSetAudioPlaybackDeviceVolume,
        kGetAudioPlaybackDeviceVolume,
        kSetAudioPlaybackDeviceMute,
        kGetAudioPlaybackDeviceMute,
        kStartAudioPlaybackDeviceTest,
        kStopAudioPlaybackDeviceTest,

        kGetAudioRecordingDeviceCount,
        kGetAudioRecordingDeviceInfoByIndex,
        kSetCurrentAudioRecordingDeviceId,
        kGetCurrentAudioRecordingDeviceId,
        kGetCurrentAudioRecordingDeviceInfo,
        kSetAudioRecordingDeviceVolume,
        kGetAudioRecordingDeviceVolume,
        kSetAudioRecordingDeviceMute,
        kGetAudioRecordingDeviceMute,
        kStartAudioRecordingDeviceTest,
        kStopAudioRecordingDeviceTest,

        kStartAudioDeviceLoopbackTest,
        kStopAudioDeviceLoopbackTest,
    }

    internal enum ApiTypeVideoDeviceManager
    {
        kGetVideoDeviceCount,
        kGetVideoDeviceInfoByIndex,
        kSetCurrentVideoDeviceId,
        kGetCurrentVideoDeviceId,
        kStartVideoDeviceTest,
        kStopVideoDeviceTest,
    }

    internal enum ApiTypeRawDataPlugin
    {
        kRegisterPlugin,
        kUnregisterPlugin,
        kHasPlugin,
        kEnablePlugin,
        kGetPlugins,
        kSetPluginParameter,
        kGetPluginParameter,
        kRelease
    }

    internal enum AudioFrameType
    {
        kFrameTypePCM16,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IrisRtcAudioFrame
    {
        internal AudioFrameType type;
        internal int samples;
        internal int bytes_per_sample;
        internal int channels;
        internal int samples_per_sec;
        internal byte[] buffer;
        internal uint buffer_length;
        internal long render_time_ms;
        internal int av_sync_type;
    }

    internal enum VideoFrameType
    {
        kFrameTypeYUV420,
        kFrameTypeYUV422,
        kFrameTypeRGBA,
    }

    internal enum VideoObserverPosition
    {
        kPositionPostCapturer = 1 << 0,
        kPositionPreRenderer = 1 << 1,
        kPositionPreEncoder = 1 << 2,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IrisRtcVideoFrame
    {
        VideoFrameType type;
        int width;
        int height;
        int y_stride;
        int u_stride;
        int v_stride;
        byte[] y_buffer;
        byte[] u_buffer;
        byte[] v_buffer;
        uint y_buffer_length;
        uint u_buffer_length;
        uint v_buffer_length;
        int rotation;
        long render_time_ms;
        int av_sync_type;
    }
}