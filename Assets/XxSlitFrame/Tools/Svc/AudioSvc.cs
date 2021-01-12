using System.Collections.Generic;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 音乐服务
    /// </summary>
    public class AudioSvc : SvcBase
    {
        public static AudioSvc Instance;
        private AudioSource _backgroundAudioSource;
        private AudioSource _effectAudioSource;
        private AudioSource _tipAndDialogAudioSource;
        public AudioData audioData;
        private Dictionary<AudioData.AudioType, AudioClip> _audioDlc;

        public override void StartSvc()
        {
            Instance = GetComponent<AudioSvc>();
        }

        public override void InitSvc()
        {
            //创建音效组件
            if (_effectAudioSource == null)
            {
                _effectAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = false;
            }
            //创建提示与对话组件
            if (_tipAndDialogAudioSource == null)
            {
                _tipAndDialogAudioSource = gameObject.AddComponent<AudioSource>();
                _tipAndDialogAudioSource.playOnAwake = false;
            }
            //创建背景音乐组件
            if (_backgroundAudioSource == null)
            {
                _backgroundAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = true;
                _backgroundAudioSource.volume = 0.5f;
                _backgroundAudioSource.loop = true;
            }
            //音效初始化
            _audioDlc = new Dictionary<AudioData.AudioType, AudioClip>();
            foreach (AudioData.AudioDataInfo audioDataInfo in audioData.audioDataInfos)
            {
                if (!_audioDlc.ContainsKey(audioDataInfo.audioType) && audioDataInfo.dialogueAudioClip != null)
                {
                    _audioDlc.Add(audioDataInfo.audioType, audioDataInfo.dialogueAudioClip);
                }
            }
            //播放背景音乐
            PlayBackgroundAudio();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioType"></param>
        public void PlayEffectAudio(AudioData.AudioType audioType)
        {
            if (_audioDlc.ContainsKey(audioType))
            {
                _effectAudioSource.clip = _audioDlc[audioType];
                _effectAudioSource.Play();
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlayEffectAudio(AudioClip audioClip)
        {
            _effectAudioSource.clip = audioClip;
            _effectAudioSource.Play();
        }

        public void PlayTipAndDialogAudio(AudioClip audioClip)
        {
            _tipAndDialogAudioSource.Stop();
            _tipAndDialogAudioSource.clip = audioClip;
            _tipAndDialogAudioSource.Play();
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopEffectAudio()
        {
            _effectAudioSource.Stop();
            _effectAudioSource.clip = null;
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopTipAndDialogAudio()
        {
            _tipAndDialogAudioSource.Stop();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _effectAudioSource.Pause();
            _backgroundAudioSource.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            _effectAudioSource.UnPause();
            if (PersistentDataSvc.Instance.audioState)
            {
                _backgroundAudioSource.UnPause();
            }
        }


        /// <summary>
        /// 切换音乐状态
        /// </summary>
        public void SwitchBackgroundState()
        {
            if (_backgroundAudioSource.isPlaying)
            {
                PauseBackgroundAudio();
            }
            else
            {
                PlayBackgroundAudio();
            }
        }

        /// <summary>
        /// 暂停背景音乐播放
        /// </summary>
        public void PauseBackgroundAudio()
        {
            PlayEffectAudio(AudioData.AudioType.ECloseBackground);
            _backgroundAudioSource.Pause();
            PersistentDataSvc.Instance.audioState = false;
        }

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void PlayBackgroundAudio()
        {
            if (_audioDlc.ContainsKey(AudioData.AudioType.EBackground) && _audioDlc[AudioData.AudioType.EBackground] != null)
            {
                _backgroundAudioSource.clip = _audioDlc[AudioData.AudioType.EBackground];
                _backgroundAudioSource.Play();
                PersistentDataSvc.Instance.audioState = true;
            }
            else
            {
                // Debug.LogError("没有指定背景音乐");
            }
        }
    }
}