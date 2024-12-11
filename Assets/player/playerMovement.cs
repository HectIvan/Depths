using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class playerMovement : MonoBehaviour
{
  /**
  * Start the player movement script.
  **/
  void
  Start()
  {
    m_Rigidbody = GetComponent<Rigidbody>();
    m_Rigidbody.freezeRotation = true;
  }

  private void
  FixedUpdate()
  {
    move();
  }

  /**
  * Update the player movement.
  **/
  void
  Update()
  {
    m_grounded = Physics.Raycast(transform.position, Vector3.down, m_height * 0.5f + 0.2f, m_isGround);
    input();
    speedClamp();

    if (m_grounded)
    {
      m_Rigidbody.drag = m_drag;
    }
    else
    {
      m_Rigidbody.drag = 0.0f;
    }
  }

  /**
  * check interaction with entity of type interactable
  **/
  void
  checkInteraction()
  {
    RaycastHit hit;
    Debug.DrawRay(m_camera.transform.position, m_camera.transform.forward * m_interDistance, Color.red);
    // if the re was a collision and there was an input
    if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hit, m_interDistance, m_interactable))
    {
      var check = hit.collider.gameObject.GetComponent<interactable>();
      if (check != null)
      {
        check.m_showBillboard = true;
      }
      var doc = hit.collider.gameObject.GetComponent<Document>();
      if (doc != null)
      {
        doc.m_show = true;
      }
      if (UnityEngine.Input.GetKeyDown(m_interact))
      {
        // if the item is an interactable
        var interactable = hit.collider.gameObject.GetComponent<interactable>();
        if (interactable != null)
        {
          // if its a locker
          var locker = interactable.transform.parent.gameObject.transform.parent.GetComponent<Locker>();
          if (locker != null)
          {
            locker.GetComponent<Locker>().m_elapsedTime = 0;
          }
          // if its a drawer
          var drawer = interactable.transform.parent.gameObject.GetComponent<drawer>();
          if (drawer != null)
          {
            drawer.GetComponent<drawer>().m_elapsedTime = 0;
          }
         
          interactable.m_interacted = !hit.collider.gameObject.GetComponent<interactable>().m_interacted;
          return;
        }
        // if item is a document
        var document = hit.collider.gameObject.GetComponent<Document>();
        if (document != null)
        {
          document.pickUp();
        }

      }
    }
  }

  /**
  * Take player input.
  **/
  private void
  input()
  {
    // take input
    if (!m_pauseMenu.GetComponent<pauseMenu>().m_active)
    {
        m_horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        m_verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");
        checkInteraction();
    }
    if (UnityEngine.Input.GetKeyDown(m_pause))
    {
      m_pauseMenu.GetComponent<pauseMenu>().ChangeState();
    }
  }

  /**
  * Move the player.
  **/
  private void
  move()
  {
    // calculate direction
    m_moveDirection = m_orientation.forward * m_verticalInput +
                      m_orientation.right * m_horizontalInput; 
    m_Rigidbody.AddForce(m_moveDirection.normalized * m_speed * 10f, ForceMode.Force);
  }

  /**
  * Clamp player speed.
  **/
  private void
  speedClamp()
  {
    Vector3 flatVel = new Vector3(m_Rigidbody.velocity.x, 0.0f, m_Rigidbody.velocity.z);

    if (flatVel.magnitude > m_speed)
    {
      Vector3 limitVel = flatVel.normalized * m_speed;
      m_Rigidbody.velocity = new Vector3(limitVel.x, m_Rigidbody.velocity.y, limitVel.z);
    }
  }

  void
  FootStepsSounds()
  {
    
  }

  [Header("Movement")]
  public float m_speed;
  public float m_drag;

  public Transform m_orientation;
  public Camera m_camera;

  float m_horizontalInput;
  float m_verticalInput;

  Vector3 m_moveDirection;

  Rigidbody m_Rigidbody;

  [Header("Ground check")]
  public float m_height;
  public LayerMask m_isGround;
  bool m_grounded;

  [Header("Controls")]
  public KeyCode m_interact;
  public KeyCode m_pause;

  [Header("In-Game data")]
  public float m_points;
  public float m_interDistance;
  public LayerMask m_interactable;

  [Header("UI")]
  public GameObject m_pauseMenu;

  public int m_currentRoom = 0;
}
