using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Room : MonoBehaviour
{
  /// <summary>
  /// Start the room
  /// </summary>
  void
  Start()
  {
    // initialize all lists
    m_children = new List<Transform>();

    m_hugePrefabs = new List<GameObject>();
    m_largePrefabs = new List<GameObject>();
    m_mediumPrefabs = new List<GameObject>();
    m_smallPrefabs = new List<GameObject>();
    m_lights = new List<GameObject>();

    // get item spawn points
    getChildrenOfTag("LargeSpawn");
    getChildrenOfTag("HugeSpawn");
    getChildrenOfTag("SmallSpawn");

    StoreChildrenOfTag(ref m_lights, "LightSource");

    // load all furniture
    loadFurniture(ref m_hugePrefabs, "hugeItems");
    loadFurniture(ref m_largePrefabs, "largeItems");
    loadFurniture(ref m_mediumPrefabs, "mediumItems");
    loadFurniture(ref m_smallPrefabs, "smallItems");

    // spawn furniture
    spawnEntities();
  }

  /// <summary>
  /// Update the room.
  /// </summary>
  void
  Update()
  {
    
  }

  public void
  BreakLights()
  {
    // foreach (GameObject _light in m_lights)
    // {
    //   _light.GetComponent<LightSource>().Break();
    // }
  }

  /// <summary>
  /// Stores the children of this transform of a certain tag in a specific container.
  /// </summary>
  /// <param name="_list">Object container.</param>
  /// <param name="_tag">Tag to search for.</param>
  void
  StoreChildrenOfTag(ref List<GameObject> _list, string _tag)
  {
    foreach (Transform _child in transform)
    {
      if (_child.tag == _tag)
      {
        _list.Add(_child.gameObject);
      }
    }
  }

  /// <summary>
  /// This function gets all the children of the current object that have the designated tag
  /// </summary>
  /// <param name="tag">Tag to search for.</param>
  void
  getChildrenOfTag(string _tag)
  {
    foreach (Transform t in transform)
    {
      if (t.tag == _tag)
      {
        m_children.Add(t);
      }
    }
  }

  /// <summary>
  /// This function checks for each item in the children list, 
  /// checking if the item is large, medium or small, then choosing a random prefab
  /// and instantiating it, removing the begining of the list, all until the list
  /// is empty.
  /// </summary>
  void
  spawnEntities()
  {
    while (m_children.Count > 0)
    {
      // if we have to spawn a huge item.
      if (m_children[0].tag == "HugeSpawn")
      {
        InstantiateItem(ref m_hugePrefabs);
      }
      // if we have to spawn a large item.
      if (m_children[0].tag == "LargeSpawn")
      {
        InstantiateItem(ref m_largePrefabs);
      }
      // if we have to spawn a small item
      else if (m_children[0].tag == "SmallSpawn")
      {
        InstantiateItem(ref m_smallPrefabs);
      }
    }
  }
  
  /// <summary>
  /// Load furniture prefabs from a directory
  /// </summary>
  /// <param name="_prefabs">Reference of a list where the prefabs will be stored.</param>
  /// <param name="_directory">Directory where the prefabs are stored.</param>
  void
  loadFurniture(ref List<GameObject> _prefabs, string _directory)
  {
    GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(_directory);

    _prefabs = new List<GameObject>();

    foreach (GameObject prefab in loadedPrefabs)
    {
      _prefabs.Add(prefab);
    }
  }

  /// <summary>
  /// Instantiate furniture
  /// </summary>
  /// <param name="_prefabs">Reference of a list where the prefabs are stored.</param>
  void
  InstantiateItem(ref List<GameObject> _prefabs)
  {
    // check if the item will be instanced
    if (Random.Range(0, 1) == 0)
    {
      int type = Random.Range(0, _prefabs.Count);
      GameObject instance = _prefabs[type]; // large crates will be default
      instance.transform.localScale = new UnityEngine.Vector3(0.01f, 0.01f, 0.01f);
      Instantiate(instance, m_children[0].transform.position, m_children[0].transform.rotation, this.transform);
    }
    m_children.Remove(m_children[0]);
  }

  // transforms of spawnable furniture
  private List<Transform> m_children;

  // List of all the lights in the room
  public List<GameObject> m_lights;

  // list of prefabs from each category
  private List<GameObject> m_hugePrefabs;
  private List<GameObject> m_largePrefabs;
  private List<GameObject> m_mediumPrefabs;
  private List<GameObject> m_smallPrefabs;

  [Header("Next Spawn Point")]
  public GameObject m_nextSpawnPoint;

  [Header("Room Number")]
  public int m_number;
}
