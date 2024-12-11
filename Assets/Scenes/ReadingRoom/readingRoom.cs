using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class readingRoom : MonoBehaviour
{
  // Start is called before the first frame update
  void
  Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
      SceneManager.LoadScene("mainMenu");
    }
  }
}
