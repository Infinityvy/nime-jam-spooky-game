using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer audioMixer;

    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


    [System.Serializable]
    private struct LoadCategory
    {
        public LoadCategory(string path, bool loadInScene)
        {
            this.path = path;
            this.loadInScene = loadInScene;
        }

        public string path;
        public bool loadInScene;
    }

    [SerializeField]
    private LoadCategory[] categoriesToLoad = new LoadCategory[]
        {
            new LoadCategory("SFX/UI", false),
            new LoadCategory("SFX/Player", false),
        };


    private void Awake()
    { 
        instance = this;
    }

    private void Start()
    {
        List<AudioClip> clips = new List<AudioClip>();

        foreach(LoadCategory lc in categoriesToLoad)
        {
            if(lc.loadInScene)
            {
                clips.AddRange(Resources.LoadAll<AudioClip>(lc.path));
            }
        }

        foreach(AudioClip cl in clips)
        {
            audioClips.Add(cl.name, cl);
        }
    }

    public void setGroupVolume(AudioGroup group, float volume)
    {
        audioMixer.SetFloat(group.ToString() + "Volume", Mathf.Log10(volume * 0.02f) * 20);
    }
    public void setMasterVolume(Slider slider) { setGroupVolume(AudioGroup.Master, slider.value); }
    public void setSFXVolume(Slider slider) { setGroupVolume(AudioGroup.SFX, slider.value); }
    public void setMusicVolume(Slider slider) { setGroupVolume(AudioGroup.Music, slider.value); }

    public float getGroupVolume(AudioGroup group)
    {
        float volume;

        audioMixer.GetFloat(group.ToString() + "Volume", out volume);

        return volume;
    }

    public static AudioClip getAudioClip(string key)
    {
        return instance.audioClips[key];
    }
}
