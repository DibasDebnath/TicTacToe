
using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioController Instance;
    // Use this for initialization

    public readonly String GameMusic = "GameMusic";
    public readonly String Tap = "Tap";



    public readonly String Music = "Music";
    


    public GameObject MusicOn;
    public GameObject MusicOff;

    void Awake()
    {
        Instance = this;
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
        }
    }

    

    public void Play(string name)
    {
        if (PlayerPrefs.GetInt(Music,1) == 1)
        {
            Sound s = Array.Find(sounds, sound => sound.audioClipName == name);
            s.audioSource.Play();
        }

    }

    public void Play(string name, bool play)
    {
        if (PlayerPrefs.GetInt(Music,1) == 1)
        {
            Sound s = Array.Find(sounds, sound => sound.audioClipName == name);
            s.audioSource.Play();
            s.audioSource.loop = play;
        }

    }
    public void PlayMenuBGMusic(string name)
    {

        if (PlayerPrefs.GetInt("musicoff") == 0)
        {
            Sound s = Array.Find(sounds, sound => sound.audioClipName == name);
            //Stop(GAME_PLAY);
            s.audioSource.Play();
            s.audioSource.loop = true;
        }

    }

    public void PlayGamePlayMusic(string name)
    {


        if (PlayerPrefs.GetInt(Music) == 0)
        {

            Sound s = Array.Find(sounds, sound => sound.audioClipName == name);
            //Stop(MENU_BG);
            s.audioSource.Play();
            s.audioSource.loop = true;
        }


    }


    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioClipName == name);
        s.audioSource.Stop();
    }




    public void OffSound()
    {
        if (PlayerPrefs.GetInt("soundoff") == 0)
        {
            PlayerPrefs.SetInt("soundoff", 1);

        }
        else if (PlayerPrefs.GetInt("soundoff") == 1)
        {

            PlayerPrefs.SetInt("soundoff", 0);

        }
    }

    public void offMusic()
    {
        if (PlayerPrefs.GetInt(Music,1) == 1)
        {
            PlayerPrefs.SetInt(Music, 0);
            Stop(GameMusic);
            MusicOn.SetActive(false);
            MusicOff.SetActive(true);
        }

        else if (PlayerPrefs.GetInt(Music,1) == 0)
        {
            PlayerPrefs.SetInt(Music, 1);
            Play(GameMusic,true);
            MusicOn.SetActive(true);
            MusicOff.SetActive(false);
        }
        
    }



    public void SetAtStart()
    {
        if (PlayerPrefs.GetInt(Music,1) == 0)
        {
            Stop(GameMusic);
            MusicOn.SetActive(false);
            MusicOff.SetActive(true);
        }

        else if (PlayerPrefs.GetInt(Music,1) == 1)
        {
           
            Play(GameMusic, true);
            MusicOn.SetActive(true);
            MusicOff.SetActive(false);
        }

    }


}
