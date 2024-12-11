using System.Collections;
using System.Collections.Generic;
// using TMPro.EditorUtilities;
using UnityEngine;

public class HeavyDoor : MonoBehaviour
{
  /// <summary>
  /// Start is called before the first frame update
  /// </summary>
  void
  Start()
  {
    m_leftLocal = m_leftDoor.transform.localPosition;
    m_rightLocal = m_rightDoor.transform.localPosition;

    m_leftTarget = new Vector3(m_leftLocal.x + m_doorWidth, m_leftLocal.y, m_leftLocal.z);
    m_rightTarget = new Vector3(m_rightLocal.x - m_doorWidth, m_rightLocal.y, m_rightLocal.z);

    if (!m_interactable)
    {
      m_player = GameObject.Find("Player");
    }
  }

  /// <summary>
  /// Update is called once per frame
  /// </summary>
  void
  Update()
  {
    // set if open
    if (!m_interactable)
    {
      // if at interact distance
      if (Vector3.Distance(m_player.transform.position, transform.position) < m_interactDistance)
      {
        m_open = true;
      }
    }
    else
    {
      m_open = m_interactable.GetComponent<interactable>().m_interacted;
    }
    
    // movement animation
    if (m_open)
    {
      open();
    }
    else
    {
      close();
    }
  }

  /// <summary>
  /// Open door animation
  /// </summary>
  void
  open()
  {
    /*****************************/
    /**
    * Move the left door
    **/
    /*****************************/
    if (m_leftDoor.transform.localPosition.x < m_leftTarget.x)
    {
      // newX is the current local x cordinate added with the sum of the speed in the deltatime multiplied by the play speed
      float newX = m_leftDoor.transform.localPosition.x + m_speed * Time.deltaTime * m_playSpeed;
      // set the new X coordinate to the local position of the door
      m_leftDoor.transform.localPosition = new Vector3(newX,
                                                       m_leftDoor.transform.localPosition.y,
                                                       m_leftDoor.transform.localPosition.z);
      // rotateValve(ref m_valveLeft, 1.0f);
    }   
    /*****************************/
    /**
    * Move the right door
    **/
    /*****************************/
    if (m_rightDoor.transform.localPosition.x > m_rightTarget.x)
    {
      // newX is the current local x cordinate added with the sum of the speed in the deltatime multiplied by the play speed
      float newX = m_rightDoor.transform.localPosition.x - m_speed * Time.deltaTime * m_playSpeed;
      // set the new X coordinate to the local position of the door
      m_rightDoor.transform.localPosition = new Vector3(newX,
                                                       m_rightDoor.transform.localPosition.y,
                                                       m_rightDoor.transform.localPosition.z);
      // rotateValve(ref m_valveRight, -1.0f);
    }
  }

  /// <summary>
  /// Close door animation
  /// </summary>
  void
  close()
  {
    /*****************************/
    /**
    * Move the left door
    **/
    /*****************************/
    if (m_leftDoor.transform.localPosition.x >= m_leftLocal.x)
    {
      // newX is the current local x cordinate added with the sum of the speed in the deltatime multiplied by the play speed
      float newX = m_leftDoor.transform.localPosition.x - m_speed * Time.deltaTime * m_playSpeed;
      // set the new X coordinate to the local position of the door
      m_leftDoor.transform.localPosition = new Vector3(newX,
                                                       m_leftDoor.transform.localPosition.y,
                                                       m_leftDoor.transform.localPosition.z);
      // rotateValve(ref m_valveLeft, -1.0f);
    }   
    /*****************************/
    /**
    * Move the right door
    **/
    /*****************************/
    if (m_rightDoor.transform.localPosition.x <= m_rightLocal.x)
    {
      // newX is the current local x cordinate added with the sum of the speed in the deltatime multiplied by the play speed
      float newX = m_rightDoor.transform.localPosition.x + m_speed * Time.deltaTime * m_playSpeed;
      // set the new X coordinate to the local position of the door
      m_rightDoor.transform.localPosition = new Vector3(newX,
                                                       m_rightDoor.transform.localPosition.y,
                                                       m_rightDoor.transform.localPosition.z);
      // rotateValve(ref m_valveRight, 1.0f);
    }
  }

  void
  rotateValve(ref GameObject _valve, float _rate)
  {
    // clamp _rate
    if (_rate < 0) { _rate = -1.0f; }
    if (_rate >= 0) { _rate = 1.0f; }
    // rotate the valve
    Quaternion rot = _valve.transform.localRotation;
    print(rot);
    float newY = rot.y + m_rotSpeed * m_playSpeed * Time.deltaTime * m_playSpeed * _rate;
    _valve.transform.localRotation = Quaternion.Euler(rot.x, newY, rot.y);
  }
  // to rotate valves, change the Z axis

  [Header("Door composition")]
  // Left door
  public GameObject m_leftDoor;
  private Vector3 m_leftLocal;
  private Vector3 m_leftTarget;
  
  // right door
  public GameObject m_rightDoor;
  private Vector3 m_rightLocal;
  private Vector3 m_rightTarget;
  
  // valves
  public GameObject m_valveLeft;
  public GameObject m_valveRight;

  // door width
  public float m_doorWidth;

  [Header("Interactable?")]
  // interactable
  public GameObject m_interactable;
  [Header("If no interactable, this will be taken")]
  public float m_interactDistance;
  private GameObject m_player;

  [Header("Parameters")]
  // animation
  public float m_playSpeed;
  public float m_speed;
  public float m_rotSpeed;
  // open/close
  public bool m_open;
}
