using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour
{
  /**
  * Initialize interactable
  **/
  void
  Start()
  {
    m_interacted = false;
    m_showBillboard = false;
    m_player = GameObject.Find("Player");
  }

  /**
  * Update interactable
  **/
  void
  Update()
  {
    if (m_interactIcon) {
      m_interactIcon.SetActive(m_showBillboard);
      m_interactIcon.transform.LookAt(m_player.transform.GetChild(0).transform.position);
    }
    m_showBillboard = false;
  }

  public bool m_interacted;
  private GameObject m_player;
  
  [Header("Interact icon")]
  public GameObject m_interactIcon;
  public bool m_showBillboard;
}
