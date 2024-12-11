using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
  public void
  SetMasterVolume(float _volume)
  {
    m_audioMixer.SetFloat("MasterVolume", Mathf.Log10(_volume) * 20.0f);
  }

  public void
  SetSFXVolume(float _volume)
  {
    m_audioMixer.SetFloat("SFXVolume", Mathf.Log10(_volume) * 20.0f);
  }

  public void
  SetMusicVolume(float _volume)
  {
    m_audioMixer.SetFloat("MusicVolume", Mathf.Log10(_volume) * 20.0f);
  }

  [SerializeField] private AudioMixer m_audioMixer;
}
