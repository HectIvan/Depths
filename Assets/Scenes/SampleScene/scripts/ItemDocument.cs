using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDocument : MonoBehaviour
{
  /// <summary>
  /// Start is called before the first frame update
  /// </summary>
  void
  Start()
  {
    m_interactable = gameObject.GetComponent<interactable>();
    if (!m_interactable) { Debug.Log("Failed to find interactable"); }
    m_player = GameObject.Find("Player");
    textDisplay.GetComponent<TextMeshPro>().text = "$" + points.ToString();
  }

  /// <summary>
  /// Update is called once per frame
  /// </summary>
  void
  Update()
  {
    // if insteractable
    if (m_interactable != null)
    {
      if (m_interactable.m_interacted)
      {
        PickUp();
      }
    }
  }

  void
  PickUp()
  {
    SFXManager.m_instance.PlaySFXClip(audioClip, transform, 1.0f);
    m_player.GetComponent<playerMovement>().m_points += points;
    gameObject.SetActive(false);
  }

  public float points;
  public AudioClip audioClip;
  public GameObject textDisplay;
  private GameObject m_player;
  private interactable m_interactable;
}
