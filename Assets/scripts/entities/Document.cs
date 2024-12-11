using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Document : MonoBehaviour
{
  /**
  * Start the document item
  **/
  void
  Start()
  {
    m_originalMesh = GetComponent<MeshFilter>();
    m_player = GameObject.Find("Player");
  }

  /**
  * Update the document item
  **/
  void
  Update()
  {
    transform.position = m_spawnPoint.transform.position;
    m_billBoard.SetActive(m_show);
    m_billBoard.transform.LookAt(m_player.transform.GetChild(0).transform.position);
    m_show = false;
  }

  /**
  * Instantiate the document
  **/
  public void
  spawn(GameObject _spawnPoint, bool _canInteract)
  {
    Start();
    transform.position = _spawnPoint.transform.position;
    transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f), Space.Self); // Random.Range(0.0f, 360.0f) in the Z axis
    m_spawnPoint = _spawnPoint;
    m_canInteract = _canInteract;

    // change type & point value
    int type = Random.Range(0, 4);
    int points = 0;
    if (type == 0)
    {
      swapMesh(5, m_small);
      points = 5;
    }
    if (type == 1)
    {
      swapMesh(10, m_medium);
      points = 10;
    }
    if (type == 2)
    {
      swapMesh(20, m_large);
      points = 20;
    }
    if (type == 3)
    {
      swapMesh(100, m_massive);
      points = 100;
    }

    m_display.GetComponent<TextMeshPro>().text = "$" + points.ToString();
  }

  /**
  * Pick up item.
  * 
  * Deactivates the item and gives the player the ammount of points the item was valued at.
  **/
  public void
  pickUp()
  {
    if (m_points > 99) { SFXManager.m_instance.PlaySFXClip(m_largeItemAudioClip, transform, 1.0f); }
    else { SFXManager.m_instance.PlaySFXClip(m_smallItemAudioClip, transform, 1.0f); }
    this.gameObject.SetActive(false);
    m_player.GetComponent<playerMovement>().m_points += m_points;
  }

  /**
  * Get the distance between the player and the item.
  * 
  * @return
  * The final distance.
  **/
  float
  distanceToPlayer()
  {
    float dist = Vector3.Distance(m_player.transform.position, transform.position);
    return dist;
  }

  /**
  * Swap Mesh
  * 
  * Changes the mesh of the current item.
  * 
  * @param _points.
  * How much will the item be valued at.
  * 
  * @param _mesh
  * The new mesh of the item.
  **/
  void
  swapMesh(float _points, Mesh _mesh)
  {
    m_points = _points;
    m_originalMesh.mesh = _mesh;
  }

  private GameObject m_player;
  public GameObject m_spawnPoint;
  public float m_interactDistance;
  public float m_points;
  public bool m_canInteract;

  [Header("Meshes")]
  public Mesh m_small;
  public Mesh m_medium;
  public Mesh m_large;
  public Mesh m_massive;
  private MeshFilter m_originalMesh;

  [Header("Audio clips")]
  public AudioClip m_largeItemAudioClip;
  public AudioClip m_smallItemAudioClip;

  [Header("interactable")]
  public bool m_show;
  public GameObject m_billBoard;
  public GameObject m_display;
}
