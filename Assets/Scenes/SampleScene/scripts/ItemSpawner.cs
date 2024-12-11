using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
  /// <summary>
  /// Start is called before the first frame update
  /// </summary>
  void
  Start()
  {
    // clamp to 100 or 0
    if (spawnChance > 100) { spawnChance = 100; }
    if (spawnChance < 0) { spawnChance = 0; }

    if (Random.Range(0, 100) <= spawnChance)
    {
      int type = Random.Range(0, prefabs.Count - 1);
      GameObject instance = prefabs[type];
      Instantiate(instance, transform.position, transform.rotation, transform);
    }
  }

  /// <summary>
  /// Update is called once per frame
  /// </summary>
  void
  Update()
  {
    
  }

  public List<GameObject> prefabs;
  public float spawnChance;
}
