using GameFramework.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CC
{
    /// <summary>
    /// 设置界面
    /// </summary>
    public class SettingForm : UGuiForm
    {
        [SerializeField]
        private Toggle m_MusicMuteToggle = null;

        [SerializeField]
        private Slider m_MusicVolumeSlider = null;

        [SerializeField]
        private Toggle m_SoundMuteToggle = null;

        [SerializeField]
        private Slider m_SoundVolumeSlider = null;

        Language m_SelectLanguage = Language.Unspecified;

        [SerializeField]
        private CanvasGroup m_LanguageTipsCanvasGroup = null;

        [SerializeField]
        private Toggle m_EnglishToggle = null;

        [SerializeField]
        private Toggle m_ChineseSimplifiedToggle = null;
        //确认按钮
        public void OnEnterBntClick()
        {
            if (m_SelectLanguage == GameEntry.Localization.Language)
            {
                Close();
                return;
            }

            GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectLanguage.ToString());
            GameEntry.Setting.Save();

            GameEntry.Sound.StopMusic();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        //音乐开关
        public void OnMusicToggle(Toggle toggle, bool isOn)
        {
            GameEntry.Sound.Mute("Music", !isOn);
            m_MusicVolumeSlider.gameObject.SetActive(isOn);
        }

        //音乐声音大小
        public void OnMusicVolumeChanged(Slider slider, float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
        }

        //音效开关
        public void OnSoundToggle(Toggle toggle, bool isOn)
        {
            GameEntry.Sound.Mute("Sound", !isOn);
            GameEntry.Sound.Mute("UISound", !isOn);
            m_SoundVolumeSlider.gameObject.SetActive(isOn);
        }

        //音效声音大小
        public void OnSoundVolumeChanged(Slider slider, float volume)
        {
            GameEntry.Sound.SetVolume("Sound", volume);
            GameEntry.Sound.SetVolume("UISound", volume);
        }

        //语言选择英语
        public void OnEnglishSelected(Toggle toggle, bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectLanguage = Language.English;
            RefreshLanguageTips();
        }

        //语言选择简体中文
        public void OnChineseSimplifiedSelected(Toggle toggle, bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectLanguage = Language.ChineseSimplified;
            RefreshLanguageTips();
        }

        private void RefreshLanguageTips()
        {
            m_LanguageTipsCanvasGroup.gameObject.SetActive(m_SelectLanguage != GameEntry.Localization.Language);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //绑定事件
            m_MusicMuteToggle.onValueChanged.AddListener((bool isOn) => { OnMusicToggle(m_MusicMuteToggle, isOn); });
            m_MusicVolumeSlider.onValueChanged.AddListener((float value) => { OnMusicVolumeChanged(m_MusicVolumeSlider, value); });

            m_SoundMuteToggle.onValueChanged.AddListener((bool isOn) => { OnSoundToggle(m_SoundMuteToggle, isOn); });
            m_SoundVolumeSlider.onValueChanged.AddListener((float value) => { OnSoundVolumeChanged(m_SoundVolumeSlider, value); });

            m_EnglishToggle.onValueChanged.AddListener((bool isOn) => { OnEnglishSelected(m_EnglishToggle, isOn); });
            m_ChineseSimplifiedToggle.onValueChanged.AddListener((bool isOn) => { OnChineseSimplifiedSelected(m_ChineseSimplifiedToggle, isOn); });

            m_MusicMuteToggle.isOn = !GameEntry.Sound.IsMuted("Music");
            m_MusicVolumeSlider.value = GameEntry.Sound.GetVolume("Music");

            m_SoundMuteToggle.isOn = !GameEntry.Sound.IsMuted("Sound");
            m_SoundVolumeSlider.value = GameEntry.Sound.GetVolume("Sound");
            m_SelectLanguage = GameEntry.Localization.Language;
            switch (m_SelectLanguage)
            {
                case Language.English:
                    m_EnglishToggle.isOn = true;
                    break;
                case Language.ChineseSimplified:
                    m_ChineseSimplifiedToggle.isOn = true;
                    break;
                default:
                    break;
            }
        }

        private void Start()
        {
            Log.Info("Setting Start");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_LanguageTipsCanvasGroup.gameObject.activeSelf)
            {
                m_LanguageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }
    }
}
