using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
  private void Awake()
  {
    if (m_instance == null)
    {
      m_instance = this;
    }
  }

  /// <summary>
  /// Creates a GameObject to play the random clip and destroys it once finished
  /// </summary>
  /// <param name="_clips">Array of audio clips.</param>
  /// <param name="_spawnPoint">Point where the audio will be played.</param>
  /// <param name="_volume">Volume of the clip.</param>
  public void
  PlayRandomSFXClips(AudioClip[] _clips, Transform _spawnPoint, float _volume)
  {
    // random index
    int rand = Random.Range(0, _clips.Length);
    // Spawn game object
    AudioSource audioSource = Instantiate(m_soundFXObject, _spawnPoint.transform.position, Quaternion.identity);

    // Assign audio clip
    audioSource.clip = _clips[rand];

    // Assign volume
    audioSource.volume = _volume;

    // Play sound
    audioSource.Play();

    // Get audio length
    float clipLength = audioSource.clip.length;

    // Destroy clip
    Destroy(audioSource.gameObject, clipLength);
  }

  /// <summary>
  /// Creates a GameObject to play the random clip following a transform and destroys it once finished
  /// </summary>
  /// <param name="_clips">Array of audio clips.</param>
  /// <param name="_spawnPoint">Point where the audio will be played.</param>
  /// <param name="_volume">Volume of the clip.</param>
  /// <param name="_parent">Parent of the audio clip.</param>
  public void
  PlayRandomParentedSFXClips(AudioClip[] _clips, Transform _spawnPoint, float _volume, Transform _parent)
  {
    // random index
    int rand = Random.Range(0, _clips.Length);
    // Spawn game object
    AudioSource audioSource = Instantiate(m_soundFXObject, _spawnPoint.transform.position, Quaternion.identity, _parent);

    // Assign audio clip
    audioSource.clip = _clips[rand];

    // Assign volume
    audioSource.volume = _volume;

    // Play sound
    audioSource.Play();

    // Get audio length
    float clipLength = audioSource.clip.length;

    // Destroy clip
    Destroy(audioSource.gameObject, clipLength);
  }

  /// <summary>
  /// Creates a GameObject to play the clip following a transform and destroys it once finished
  /// </summary>
  /// <param name="_clip">Audio clip.</param>
  /// <param name="_spawnPoint">Point where the audio will be played.</param>
  /// <param name="_volume">Volume of the clip.</param>
  /// <param name="_parent">Parent of the audio clip.</param>
  public void
  PlayParentedSFXClip(AudioClip _clip, Transform _spawnPoint, float _volume, Transform _parent)
  {
    // Spawn game object
    AudioSource audioSource = Instantiate(m_soundFXObject, _spawnPoint.transform.position, Quaternion.identity, _parent);

    // Assign audio clip
    audioSource.clip = _clip;

    // Assign volume
    audioSource.volume = _volume;

    // Play sound
    audioSource.Play();

    // Get audio length
    float clipLength = audioSource.clip.length;

    // Destroy clip
    Destroy(audioSource.gameObject, clipLength);
  }

  /// <summary>
  /// Creates a GameObject to play the clip on loop.
  /// </summary>
  /// <param name="_clip">Audio clip.</param>
  /// <param name="_spawnPoint">Point where the audio will be played.</param>
  /// <param name="_volume">Volume of the clip.</param>
  public void
  PlayLoopSFXClip(AudioClip _clip, Transform _spawnPoint, float _volume)
  {
    // Spawn game object
    AudioSource audioSource = Instantiate(m_soundFXObject, _spawnPoint.transform.position, Quaternion.identity);

    // Assign audio clip
    audioSource.clip = _clip;

    // Assign volume
    audioSource.volume = _volume;

    // Play sound
    audioSource.Play();

    // set loop
    audioSource.loop = true;
  }

  /// <summary>
  /// Creates a GameObject to play the clip following a transform on loop.
  /// </summary>
  /// <param name="_clip">Audio clip.</param>
  /// <param name="_spawnPoint">Point where the audio will be played.</param>
  /// <param name="_volume">Volume of the clip.</param>
  /// <param name="_parent">Parent of the audio clip.</param>
  public void
  PlayLoopParentedSFXClip(AudioClip _clip, Transform _spawnPoint, float _volume, Transform _parent)
  {
    // Spawn game object
    AudioSource audioSource = Instantiate(m_soundFXObject, _spawnPoint.transform.position, Quaternion.identity, _parent);

    // Assign audio clip
    audioSource.clip = _clip;

    // Assign volume
    audioSource.volume = _volume;

    // Play sound
    audioSource.Play();

    // set loop
    audioSource.loop = true;
  }

  /// <summary>
  /// Creates a GameObject to play the clip and destroys it once finished
  /// </summary>
  /// <param name="_clip">Audio clip.</param>
  /// <param name="_spawnPoint">Point where the audio will be played.</param>
  /// <param name="_volume">Volume of the clip.</param>
  public void
  PlaySFXClip(AudioClip _clip, Transform _spawnPoint, float _volume)
  {
    // Spawn game object
    AudioSource audioSource = Instantiate(m_soundFXObject, _spawnPoint.transform.position, Quaternion.identity);

    // Assign audio clip
    audioSource.clip = _clip;

    // Assign volume
    audioSource.volume = _volume;

    // Play sound
    audioSource.Play();

    // Get audio length
    float clipLength = audioSource.clip.length;

    // Destroy clip
    Destroy(audioSource.gameObject, clipLength);
  }

  /// <summary>
  /// Stops playing the audio.
  /// </summary>
  /// <param name="_audio">Audio Clip to stop.</param>
  //public void
  //stopSFXClip(AudioClip _audio)
  //{
  //  _audio.Stop();
  //}

  public static SFXManager m_instance;
  [SerializeField] private AudioSource m_soundFXObject;
}
