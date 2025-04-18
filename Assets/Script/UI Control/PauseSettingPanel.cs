using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettingPanel : BasePanelController
{
    [Header("Main Components")]
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SoundVolumeSlider;


    public override void Show()
    {
        Popup(AnimationTimeIn);
    
        MusicVolumeSlider.value = GameManager.Instance.PlayerData.MusicVolume;
        SoundVolumeSlider.value = GameManager.Instance.PlayerData.SFXVolume;
    }

    public override void Hide()
    {
        PopOut(AnimationTimeOut);
    }

    public void SaveSetting(){
        GameManager.Instance.SaveSettingsData(MusicVolumeSlider.value, SoundVolumeSlider.value);
    }

    
}
