using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shelf : MonoBehaviour
{
  /**
  * Start the shelf
  **/
  void
  Start()
  {
    // check if item will be spawned
    if (Random.Range(0.0f, 11.0f) < 2.0f)
    {
      m_itemToSpawn = Instantiate(m_itemPrefab);
      m_itemToSpawn.GetComponent<Document>().spawn(m_itemSpawnPoint, true);
    }
  }

  /**
  * Update the shelf
  **/
  void
  Update()
  {
    
  }

  [Header("Item spawning")]
  public GameObject m_itemSpawnPoint;
  public GameObject m_itemPrefab;
  GameObject m_itemToSpawn;
}
