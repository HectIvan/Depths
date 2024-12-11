using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
  /**
  * Start score manager
  **/
  void
  Start()
  {
    m_player = GameObject.Find("Player");
  }

  /**
  * Update score manager
  **/
  void
  Update()
  {
    m_scoreText.text = m_player.GetComponent<playerMovement>().m_points.ToString();
  }

  public Text m_scoreText;
  private GameObject m_player;
}
