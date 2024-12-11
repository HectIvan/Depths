using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
  /// <summary>
  /// Start pause menu
  /// </summary>
  void
  Start()
  {
    m_active = false;
    m_pauseMenu.SetActive(m_active);
    SetActiveOptions(m_active);
  }

  /// <summary>
  /// Update pause menu
  /// </summary>
  void
  Update()
  {
    
  }

  /// <summary>
  /// Resume the game
  /// </summary>
  public void
  ChangeState()
  {
    m_active = !m_active;
    m_pauseMenu.SetActive(m_active);
    Cursor.visible = m_active;
    if (m_active) 
    {
      SetActiveOptions(false);
      Cursor.lockState = CursorLockMode.None;
      Time.timeScale = 0.0f;
    }
    else 
    {
      SetActiveOptions(false);
      Cursor.lockState = CursorLockMode.Locked;
      Time.timeScale = 1.0f;
    }
  }

  /// <summary>
  /// Loads back to the main menu
  /// </summary>
  public void
  GoToMainMenu()
  {
    Time.timeScale = 1.0f;
    Cursor.lockState = CursorLockMode.None;
    LoadScene("mainMenu");
  }

  /// <summary>
  /// Quit application
  /// </summary>
  public void
  Quit()
  {
    Application.Quit();
  }

  public void
  onOptionsMenu()
  {
    m_optionsActive = !m_optionsActive;
    m_optionsMenu.SetActive(m_optionsActive);
  }

  void
  SetActiveOptions(bool _active)
  {
    m_optionsMenu.SetActive(_active);
    m_optionsActive = _active;
  }

  /// <summary>
  /// Load scene
  /// </summary>
  /// <param name="_sceneName">Scene directory</param>
  void
  LoadScene(string _sceneName)
  {
    SceneManager.LoadScene(_sceneName);
  }

  public GameObject m_pauseMenu;
  public GameObject m_optionsMenu;
  public bool m_active;
  public bool m_optionsActive;
}
