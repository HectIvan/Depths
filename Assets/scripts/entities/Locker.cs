using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
  /**
  * Start the locker
  **/
  void
  Start()
  {
    m_leftRotIni = m_left.transform.rotation;
    m_rightRotIni = m_right.transform.rotation;

    m_leftRotEnd = m_leftRotIni;
    m_leftRotEnd.y = -90.0f;

    m_rightRotEnd = m_rightRotIni;
    m_rightRotEnd.y = 90.0f;
  }

  /**
  * Update the locker
  **/
  void
  Update()
  {
    if (m_leftHandle.GetComponent<interactable>().m_interacted ||
        m_rightHandle.GetComponent<interactable>().m_interacted)
    {
       m_leftHandle.GetComponent<interactable>().m_interacted = true;
       m_rightHandle.GetComponent<interactable>().m_interacted = true;
       open();
    }
    else
    {
       m_leftHandle.GetComponent<interactable>().m_interacted = false;
       m_rightHandle.GetComponent<interactable>().m_interacted = false;
       close();
    }
  }

  /**
  * Open the locker
  **/
  void
  open()
  {
    if (m_elapsedTime < m_openDuration)
    {
        m_elapsedTime += Time.deltaTime;
        float t = m_elapsedTime / m_openDuration;
        m_left.transform.localRotation = Quaternion.Lerp(m_leftRotIni, m_leftRotEnd, t);
        m_right.transform.localRotation = Quaternion.Lerp(m_rightRotIni, m_rightRotEnd, t);
    }
  }
  
  /**
  * Open the locker
  **/
  void
  close()
  {
    if (m_elapsedTime < m_openDuration)
    {
        m_elapsedTime += Time.deltaTime;
        float t = m_elapsedTime / m_openDuration;
        m_left.transform.localRotation = Quaternion.Lerp(m_leftRotEnd, m_leftRotIni, t);
        m_right.transform.localRotation = Quaternion.Lerp(m_rightRotEnd, m_rightRotIni, t);
    }
  }

  [Header("Left Door")]
  public GameObject m_left; // negative z
  public GameObject m_leftHandle;
  private Quaternion m_leftRotIni;
  private Quaternion m_leftRotEnd;

  [Header("Right Door")]
  public GameObject m_right; // positive z
  public GameObject m_rightHandle;
  private Quaternion m_rightRotIni;
  private Quaternion m_rightRotEnd;

  public float m_elapsedTime;
  public float m_openDuration;
}
