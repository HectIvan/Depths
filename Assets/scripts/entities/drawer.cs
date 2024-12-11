using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class drawer : MonoBehaviour
{
  /**
  * Start the drawer.
  **/
  void
  Start()
  {
    m_originalPosition = transform.position;
    m_targetPosition = m_originalPosition;
    m_targetPosition -= transform.up.normalized * 0.7f;
    m_elapsedTime = 0.0f;

    m_player = GameObject.Find("Player");
  }

  /**
  * Update of the drawer.
  **/
  void Update()
  {
    // if the drawer has been opened
    if (m_handle.GetComponent<interactable>().m_interacted && !m_open)
    {
      SFXManager.m_instance.PlayParentedSFXClip(m_openClip, transform, 1.0f, transform);
      m_elapsedTime = 0.0f;
      m_open = true;
    }
    else if (!m_handle.GetComponent<interactable>().m_interacted && m_open) // if the drawer is closed
    {
      SFXManager.m_instance.PlayParentedSFXClip(m_closeClip, transform, 1.0f, transform);
      m_elapsedTime = 0.0f;
      m_open = false;
    }
    if (m_open)
    {
      open();
    }
    else
    {
      close();
    }
  }

  /**
  * Open the drawer.
  **/
  void
  open()
  {
    if (m_elapsedTime < m_openDuration)
    {
        m_elapsedTime += Time.deltaTime;
        float t = m_elapsedTime / m_openDuration;
        transform.position = Vector3.Lerp(m_originalPosition, m_targetPosition, t);
    }
  }

  /**
  * Close the drawer.
  **/
  void
  close()
  {
    if (m_elapsedTime < m_openDuration)
    {
      m_elapsedTime += Time.deltaTime;
      float t = m_elapsedTime / m_openDuration;
      transform.position = Vector3.Lerp(m_targetPosition, m_originalPosition, t);
    }
  }
  
  /**
  * Get the distance between the player and the handle.
  * 
  * @return
  * The final distance.
  **/
  float
  distanceToPlayer()
  {
    float dist = Vector3.Distance(m_player.transform.position, m_handle.transform.position);
    return dist;
  }

  [Header("In-game entities")]
  public float spawnChance;
  private GameObject m_player;
  public GameObject m_handle;

  private Vector3 m_originalPosition;
  private Vector3 m_targetPosition;

  
  public float m_openDuration;
  public float m_elapsedTime;

  public float m_interactionDistance;

  private bool m_open;
  [SerializeField] private AudioClip m_openClip;
  [SerializeField] private AudioClip m_closeClip;
}
