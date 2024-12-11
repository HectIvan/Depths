using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public class PlayerCamera : MonoBehaviour
{
  /**
  * Start the player camera.
  **/
  void
  Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  /**
  * Update the player camera.
  **/
  void
  Update()
  {
    if (!m_pauseMenu.GetComponent<pauseMenu>().m_active)
    {
      CameraInput();
    }
  }

  /// <summary>
  /// Input of camera
  /// </summary>
  void
  CameraInput()
  {
    float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_sensX;
    float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * m_sensY;

    m_yRotation += mouseX;
    m_xRotation -= mouseY;
    m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

    transform.rotation = Quaternion.Euler(m_xRotation, m_yRotation, 0);
    m_orientation.rotation = Quaternion.Euler(0, m_yRotation, 0);
  }

  public void
  SetColorblind(int _type)
  {
    transform.GetComponent<Colorblind>().Type = _type;
  }

  [Header("Camera Properties")]
  public float m_sensX;
  public float m_sensY;

  public Transform m_orientation;

  float m_xRotation;
  float m_yRotation;

  public GameObject m_pauseMenu;
}
