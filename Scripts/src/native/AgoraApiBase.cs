//  AgoraApiBase.cs
//
//  Created by YuGuo Chen on September 27, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

namespace agora.rtc
{
    internal enum ApiTypeEngine
    {
        kEngineRelease,
        kEngineInitialize,
        kEngineGetVersion,
        kEngineGetErrorDescription,
        kEngineJoinChannel,
        kEngineUpdateChannelMediaOptions,
        kEngineLeaveChannel,
        kEngineRenewToken,
        kEngineSetChannelProfile,
        kEngineSetClientRole,
        kEngineStartEchoTest,
        kEngineStopEchoTest,
        kEngineEnableVideo,
        kEngineDisableVideo,
        kEngineStartPreview,
        kEngineStopPreview,
        kEngineStartLastMileProbeTest,
        kEngineStopLastMileProbeTest,
        kEngineSetVideoEncoderConfiguration,
        kEngineSetBeautyEffectOptions,
        kEngineSetupRemoteVideo,
        kEngineSetupLocalVideo,
        kEngineEnableAudio,
        kEngineDisableAudio,
        kEngineSetAudioProfile,
        kEngineEnableLocalAudio,
        kEngineMuteLocalAudioStream,
        kEngineMuteAllRemoteAudioStreams,
        kEngineSetDefaultMuteAllRemoteAudioStreams,
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
        kEngineRegisterAudioEncodedFrameObserver,
        kEngineStopAudioRecording,
        kEngineStartAudioMixing,
        kEngineStopAudioMixing,
        kEnginePauseAudioMixing,
        kEngineResumeAudioMixing,
        kEngineAdjustAudioMixingVolume,
        kEngineAdjustAudioMixingPublishVolume,
        kEngineGetAudioMixingPublishVolume,
        kEngineAdjustAudioMixingPlayoutVolume,
        kEngineGetAudioMixingPlayoutVolume,
        kEngineGetAudioMixingDuration,
        kEngineGetAudioMixingCurrentPosition,
        kEngineSetAudioMixingPosition,
        kEngineGetEffectsVolume,
        kEngineSetEffectsVolume,
        kEnginePreloadEffect,
        kEnginePlayEffect,
        kEnginePlayAllEffects,
        kEngineGetVolumeOfEffect,
        kEngineSetVolumeOfEffect,
        kEnginePauseEffect,
        kEnginePauseAllEffects,
        kEngineResumeEffect,
        kEngineResumeAllEffects,
        kEngineStopEffect,
        kEngineStopAllEffects,
        kEngineUnloadEffect,
        kEngineUnloadAllEffects,
        kEngineEnableSoundPositionIndication,
        kEngineSetRemoteVoicePosition,
        kEngineSetRemoteVoice3DPosition,
        kEngineSetVoiceBeautifierPreset,
        kEngineSetAudioEffectPreset,
        kEngineSetVoiceConversionPreset,
        kEngineSetAudioEffectParameters,
        kEngineSetVoiceBeautifierParameters,
        kEngineSetVoiceConversionParameters,
        kEngineSetLocalVoicePitch,
        kEngineSetLocalVoiceEqualization,
        kEngineSetLocalVoiceReverb,
        kEngineSetLocalVoiceReverbPreset,
        kEngineSetLocalVoiceChanger,
        kEngineSetLogFile,
        kEngineSetLogFilter,
        kEngineSetLogLevel,
        kEngineSetLogFileSize,
        kEngineSetLocalRenderMode,
        kEngineSetRemoteRenderMode,
        kEngineSetLocalVideoMirrorMode,
        kEngineEnableDualStreamMode,
        kEngineSetExternalAudioSource,
        kEngineSetExternalAudioSink,
        kEnginePullAudioFrame,
        kEngineStartPrimaryCustomAudioTrack,
        kEngineStopPrimaryCustomAudioTrack,
        kEngineStartSecondaryCustomAudioTrack,
        kEngineStopSecondaryCustomAudioTrack,
        kEngineSetRecordingAudioFrameParameters,
        kEngineSetPlaybackAudioFrameParameters,
        kEngineSetMixedAudioFrameParameters,
        kEngineSetPlaybackAudioFrameBeforeMixingParameters,
        kEngineEnableAudioSpectrumMonitor,
        kEngineDisableAudioSpectrumMonitor,
        kEngineRegisterAudioSpectrumObserver,
        kEngineUnregisterAudioSpectrumObserver,
        kEngineAdjustRecordingSignalVolume,
        kEngineMuteRecordingSignal,
        kEngineAdjustPlaybackSignalVolume,
        kEngineAdjustUserPlaybackSignalVolume,
        kEngineEnableLoopBackRecording,
        kEngineAdjustLoopbackRecordingVolume,
        kEngineGetLoopbackRecordingVolume,
        kEngineEnableInEarMonitoring,
        kEngineSetInEarMonitoringVolume,
        kEngineLoadExtensionProvider,
        kEngineSetExtensionProviderProperty,
        kEngineEnableExtension,
        kEngineSetExtensionProperty,
        kEngineGetExtensionProperty,
        kEngineSetCameraCapturerConfiguration,
        kEngineSwitchCamera,
        kEngineIsCameraZoomSupported,
        kEngineIsCameraFaceDetectSupported,
        kEngineIsCameraTorchSupported,
        kEngineIsCameraFocusSupported,
        kEngineIsCameraAutoFocusFaceModeSupported,
        kEngineSetCameraZoomFactor,
        kEngineEnableFaceDetection,
        kEngineGetCameraMaxZoomFactor,
        kEngineSetCameraFocusPositionInPreview,
        kEngineSetCameraTorchOn,
        kEngineSetCameraAutoFocusFaceModeEnabled,
        kEngineIsCameraExposurePositionSupported,
        kEngineSetCameraExposurePosition,
        kEngineIsCameraAutoExposureFaceModeSupported,
        kEngineSetCameraAutoExposureFaceModeEnabled,
        kEngineSetDefaultAudioRouteToSpeakerphone,
        kEngineSetEnableSpeakerphone,
        kEngineIsSpeakerphoneEnabled,
        kEngineStartScreenCaptureByDisplayId,
        kEngineStartScreenCaptureByScreenRect,
        kEngineStartScreenCapture,
        kEngineStartScreenCaptureByWindowId,
        kEngineSetScreenCaptureContentHint,
        kEngineUpdateScreenCaptureRegion,
        kEngineUpdateScreenCaptureParameters,
        kEngineStopScreenCapture,
        kEngineGetCallId,
        kEngineRate,
        kEngineComplain,
        kEngineAddPublishStreamUrl,
        kEngineRemovePublishStreamUrl,
        kEngineSetLiveTranscoding,
        kEngineStartLocalVideoTranscoder,
        kEngineUpdateLocalTranscoderConfiguration,
        kEngineStopLocalVideoTranscoder,
        kEngineStartPrimaryCameraCapture,
        kEngineStartSecondaryCameraCapture,
        kEngineStopPrimaryCameraCapture,
        kEngineStopSecondaryCameraCapture,
        kEngineSetCameraDeviceOrientation,
        kEngineSetScreenCaptureOrientation,
        kEngineStartPrimaryScreenCapture,
        kEngineStartSecondaryScreenCapture,
        kEngineStopPrimaryScreenCapture,
        kEngineStopSecondaryScreenCapture,
        kEngineGetConnectionState,
        kEngineRegisterEventHandler,
        kEngineUnregisterEventHandler,
        kEngineSetRemoteUserPriority,
        kEngineRegisterPacketObserver,
        kEngineSetEncryptionMode,
        kEngineSetEncryptionSecret,
        kEngineEnableEncryption,
        kEngineCreateDataStream,
        kEngineSendStreamMessage,
        kEngineAddVideoWaterMark,
        kEngineClearVideoWatermark,
        kEngineClearVideoWaterMarks,
        kEngineAddInjectStreamUrl,
        kEngineRemoveInjectStreamUrl,
        kEnginePauseAudio,
        kEngineResumeAudio,
        kEngineEnableWebSdkInteroperability,
        kEngineSendCustomReportMessage,
        kEngineRegisterMediaMetadataObserver,
        kEngineUnRegisterMediaMetadataObserver,
        kEngineStartAudioFrameDump,
        kEngineStopAudioFrameDump,
        kEngineRegisterLocalUserAccount,
        kEngineJoinChannelWithUserAccount,
        kEngineJoinChannelWithUserAccountEx,
        kEngineGetUserInfoByUserAccount,
        kEngineGetUserInfoByUid,
        kEngineStartChannelMediaRelay,
        kEngineUpdateChannelMediaRelay,
        kEngineStopChannelMediaRelay,
        kEngineSetDirectCdnStreamingAudioConfiguration,
        kEngineSetDirectCdnStreamingVideoConfiguration,
        kEngineStartDirectCdnStreaming,
        kEngineStopDirectCdnStreaming,
        kEngineUpdateDirectCdnStreamingMediaOptions,
        kEnginePushDirectCdnStreamingCustomVideoFrame,

        kEngineJoinChannelEx,
        kEngineLeaveChannelEx,
        kEngineUpdateChannelMediaOptionsEx,
        kEngineSetVideoEncoderConfigurationEx,
        kEngineSetupRemoteVideoEx,
        kEngineMuteRemoteAudioStreamEx,
        kEngineMuteRemoteVideoStreamEx,
        kEngineSetRemoteVoicePositionEx,
        kEngineSetRemoteVoice3DPositionEx,
        kEngineSetRemoteRenderModeEx,
        kEngineEnableLoopBackRecordingEx,
        kEngineGetConnectionStateEx,
        kEngineEnableEncryptionEx,
        kEngineCreateDataStreamEx,
        kEngineSendStreamMessageEx,
        kEngineAddVideoWaterMarkEx,
        kEngineClearVideoWatermarkEx,
        kEngineSendCustomReportMessageEx,
        
        kEngineSetAppType
    }

    internal enum ApiTypeAudioDeviceManager
    {
        kADMEnumeratePlaybackDevices,
        kADMSetPlaybackDevice,
        kADMGetPlaybackDevice,
        kADMGetPlaybackDeviceInfo,
        kADMSetPlaybackDeviceVolume,
        kADMGetPlaybackDeviceVolume,
        kADMSetPlaybackDeviceMute,
        kADMGetPlaybackDeviceMute,
        kADMStartPlaybackDeviceTest,
        kADMStopPlaybackDeviceTest,

        kADMEnumerateRecordingDevices,
        kADMSetRecordingDevice,
        kADMGetRecordingDevice,
        kADMGetRecordingDeviceInfo,
        kADMSetRecordingDeviceVolume,
        kADMGetRecordingDeviceVolume,
        kADMSetRecordingDeviceMute,
        kADMGetRecordingDeviceMute,
        kADMStartRecordingDeviceTest,
        kADMStopRecordingDeviceTest,

        kADMStartAudioDeviceLoopbackTest,
        kADMStopAudioDeviceLoopbackTest,
    }

    internal enum ApiTypeVideoDeviceManager
    {
        kVDMEnumerateVideoDevices,
        kVDMSetDevice,
        kVDMGetDevice,
        kVDMStartDeviceTest,
        kVDMStopDeviceTest,
    }

    internal enum ApiTypeRawDataPluginManager 
    {
        kRDPMRegisterPlugin,
        kRDPMUnregisterPlugin,
        kRDPMHasPlugin,
        kRDPMEnablePlugin,
        kRDPMGetPlugins,
        kRDPMSetPluginParameter,
        kRDPMGetPluginParameter,
        kRDPMRelease
    }
} // namespace agora.rtc