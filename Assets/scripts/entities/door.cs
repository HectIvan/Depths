using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
  /**
  * Start the door
  **/
  void
  Start()
  {
    m_open = false;
    m_player = GameObject.Find("Player");
    m_originalPos = transform.position;
  }

  /**
  * Open the door
  **/
  void
  Update()
  {
    // if player is at distance of interaction
    if (distanceTo(ref m_player) < m_interactDist && !m_open)
    {
      m_open = true;
      SFXManager.m_instance.PlayParentedSFXClip(m_openClip, transform, 1.0f, transform);
      // parent == door frame, parent.parent == room
      m_player.GetComponent<playerMovement>().m_currentRoom = transform.parent.parent.GetComponent<Room>().m_number;
    }
    if (m_open)
    {
      open();
    }
  }

  /**
  * Get the distance between the player and the door.
  * 
  * @return
  * The final distance.
  **/
  float
  distanceTo(ref GameObject _target)
  {
    float dist = Vector3.Distance(_target.transform.position, transform.position);
    return dist;
  }

  /**
  * Open the door
  **/
  void
  open()
  {
    if (m_elapsedTime < m_openDuration)
    {
      m_elapsedTime += Time.deltaTime;
      float t = m_elapsedTime / m_openDuration;
      transform.position = Vector3.Lerp(m_originalPos, m_targetPos.transform.position, t);
    }
  }

  /**
  * Open the door
  **/
  void
  close()
  {
    if (m_elapsedTime < m_openDuration)
    {
      m_elapsedTime += Time.deltaTime;
      float t = m_elapsedTime / m_openDuration;
      transform.position = Vector3.Lerp(m_targetPos.transform.position, m_originalPos, t);
    }
  }

  [Header("Target")]
  public GameObject m_targetPos;

  [Header("Interaction distance")]
  public float m_interactDist;

  private GameObject m_player;
  private Vector3 m_originalPos;
  
  [Header("Open animation")]
  public float m_elapsedTime;
  public float m_openDuration;

  private bool m_open;
  private bool m_anglerOpen;

  [SerializeField] private AudioClip m_openClip;
}
