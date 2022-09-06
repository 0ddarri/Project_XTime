using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundInfo
{
    public string Name;
    [SerializeField] AudioSource Audio;

    public void Play()
    {
        Audio.Play();
    }

    public void Stop()
    {
        Audio.Stop();
    }

    public void Mute(bool value)
    {
        Audio.mute = !value;
    }
}

public enum SOUND_TYPE
{
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<SoundInfo> BGMList = new List<SoundInfo>();
    [SerializeField] List<SoundInfo> SFXList = new List<SoundInfo>();


    public void PlaySound(SOUND_TYPE type, string name)
    {
        List<SoundInfo> soundlist = (type == SOUND_TYPE.BGM) ? BGMList : SFXList;

        for(int i = 0; i < soundlist.Count; i++)
        {
            if(soundlist[i].Name == name)
            {
                soundlist[i].Play();
                return;
            }
        }
    }

    public void StopSound(SOUND_TYPE type, string name)
    {
        List<SoundInfo> soundlist = (type == SOUND_TYPE.BGM) ? BGMList : SFXList;

        for (int i = 0; i < soundlist.Count; i++)
        {
            if (soundlist[i].Name == name)
            {
                soundlist[i].Stop();
                return;
            }
        }
    }

    public void StopAllSound()
    {
        for (int i = 0; i < BGMList.Count; i++)
            BGMList[i].Stop();

        for (int i = 0; i < SFXList.Count; i++)
            SFXList[i].Stop();
    }

    public void SetVolume(SOUND_TYPE type, bool value)
    {
        List<SoundInfo> soundlist = (type == SOUND_TYPE.BGM) ? BGMList : SFXList;

        for(int i = 0; i < soundlist.Count; i++)
        {
            soundlist[i].Mute(value);
        }
    }
}
