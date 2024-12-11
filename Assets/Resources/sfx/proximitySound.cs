using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximitySound : MonoBehaviour
{
  // Start is called before the first frame update
  void
  Start()
  {
    m_player = GameObject.Find("Player");
    m_played = false;
  }

  // Update is called once per frame
  void
  Update()
  {
    if (m_player != null)
    {
      if (Vector3.Distance(m_player.transform.position, transform.position) < m_triggerDistance && !m_played)
      {
        MusicManager.m_instance.PlayRandomParentedMusicClips(m_audioClips, m_player.transform, 1.0f, m_player.transform);
        m_played = true;
      }
    }
  }

  private GameObject m_player;
  public float m_triggerDistance;
  public AudioClip[] m_audioClips;
  private bool m_played;
}
