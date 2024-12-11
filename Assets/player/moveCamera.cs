using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
  /**
  * Start the move camera script.
  **/
  void
  Start()
  {
    
  }

  /**
  *  Update the camera script.
  **/
  void
  Update()
  {
    transform.position = cameraPosition.position;
  }

  public Transform cameraPosition;
}
