using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct RoomData
{
  public int m_id;
  public List<GameObject> m_navNodes;
  public Transform m_spawnPoint;
}

public class NodeMonster : MonoBehaviour
{
  enum STATE
  {
    kSearching = 0,
    kAgro,
    kNone
  }
  /// <summary>
  /// Start angler
  /// </summary>
  void
  Start()
  {
    // Clear();
    m_jumpscareTimer = 0.0f;
    gameObject.SetActive(false);
  }

  /// <summary>
  /// Update angler
  /// </summary>
  void
  Update()
  {
    //if (!m_started)
    //{
    //  transform.position = m_navRooms[m_currentRoom].m_spawnPoint.transform.position;
    //  transform.LookAt(GetComponent<angler>().getClosestNode().transform);
    //  m_started = true;
    //}
    //m_sprite.transform.LookAt(m_player.transform);
    //m_sprite.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, m_sprite.transform.eulerAngles.y, m_sprite.transform.eulerAngles.z);

    //// transform.LookAt(getClosestNode().transform);
    //transform.position += transform.forward * Time.deltaTime * m_speed;

    //if (m_currentRoom < 1) { gameObject.SetActive(false); }
    // Init();
    if (!m_jumpscaring)
    {
      Move();
    }
    else
    {
      Jumpscare();
    }
  }

  public void
  Clear()
  {
    m_jumpscareTimer = 0.0f;
    m_player = GameObject.Find("Player");
    m_navRooms = new List<RoomData>();
    m_path = new List<RoomData>();
    m_playedAttack = false;
    m_state = STATE.kNone;
    m_jumpscaring = false;
    m_didJumpscare = false;
  }

  public void
  Empty()
  {
    Clear();
    DeleteChildren();
  }

  public void
  DeleteChildren()
  {
    foreach (Transform t in transform)
    {
      if (t.tag != "NodeMonsterTag")
      {
        Destroy(t.gameObject);
      }
    }
  }

  public void
  Init()
  {
    if (m_state == STATE.kNone)
    {
      m_jumpscareTimer = 0.0f;
      m_jumpscaring = false;
      m_didJumpscare = false;
      m_currentRoom = m_roomSpawn;
      m_playedAttack = false;
      SFXManager.m_instance.PlayLoopParentedSFXClip(m_movingClip, transform, 1.0f, transform);
      transform.position = m_path[m_currentRoom].m_navNodes[0].transform.position;
      m_state = STATE.kSearching;
    }
  }

  void
  Move()
  {
    // if node monster has not been initialized or has otherwise been neutralized
    if (m_state == STATE.kNone) { return; }

    // check if monster is in agro
    if (m_state != STATE.kAgro) { CheckAgro(); }

    // if in agro and not in attack state
    if (m_state == STATE.kAgro && !m_playedAttack)
    {
      SFXManager.m_instance.PlayLoopParentedSFXClip(m_attackingClip, transform, 1.0f, this.transform);
      m_playedAttack = true;
    }
    FollowPath();
    Attack();

    // sprite look at player X axis locked
    m_sprite.transform.LookAt(m_player.transform);
    Vector3 spriteRot = m_sprite.transform.eulerAngles;
    m_sprite.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, spriteRot.y, spriteRot.z);

    // despwn (deactivate) node monster
    int despawnRoom = m_player.GetComponent<playerMovement>().m_currentRoom - m_roomSpawnOffset;
    if (despawnRoom < 0) { despawnRoom = 0; }
    if (m_currentRoom <= despawnRoom)
    {
      gameObject.SetActive(false);
      m_state = STATE.kNone;
      DeleteChildren();
    }
  }

  void
  CheckAgro()
  {
    RaycastHit hit;
    Vector3 rayDirection = m_player.transform.position - transform.position;
    if (Physics.Raycast(transform.position, rayDirection, out hit, 40.0f, m_layerPlayer))
    {
      m_state = STATE.kAgro;
    }
  }

  /// <summary>
  /// Check if player is in range of attack
  /// </summary>
  void
  Attack()
  {
    if (Vector3.Distance(transform.position, m_player.transform.position) < m_attackRange)
    {
      m_jumpscaring = true;
    }
  }

  /// <summary>
  /// Create the navigation map for the angler.
  /// 
  /// This function takes a room, to then store all of its nav Nodes into
  /// a list for later use
  /// </summary>
  /// <param name="_module">reference to the room to add</param>
  public void
  SetRoomNav(ref GameObject _module)
  {
    // all navigation nodes in the room
    List<GameObject> navNodes = new List<GameObject>();
    getChildrenOfTag(ref _module, ref navNodes, "navigationNode");

    // create a temporary roomData variable
    RoomData roomData = new RoomData();
    roomData.m_navNodes = new List<GameObject>();

    // assign to the roomData the current room number
    roomData.m_id = _module.transform.GetChild(0).GetComponent<Room>().m_number;

    // set the spawn point to the room data
    roomData.m_spawnPoint = _module.transform;

    // for each navigation node found
    foreach (GameObject node in navNodes)
    {
      // assign to the room data
      roomData.m_navNodes.Add(node);
    }

    // insert the roomData to the navigation rooms list
    m_navRooms.Add(roomData);
  }

  /// <summary>
  /// Create path of navigation nodes
  /// </summary>
  /// <param name="_module">reference of room</param>
  public void
  setNavRoomNodes(GameObject _module)
  {
    List<GameObject> nodes = new List<GameObject>();
    RoomData room = new RoomData();
    room.m_navNodes = new List<GameObject>();
    room.m_id = _module.transform.GetChild(0).GetComponent<Room>().m_number;
    getChildrenOfTag(ref _module, ref room.m_navNodes, "navigationNode");

    m_path.Add(room);
  }

  /// <summary>
  /// Create path of navigation nodes
  /// </summary>
  /// <param name="_node">node to insert</param>
  public void
  setNavNodes(GameObject _node)
  {
    GameObject newNode = new GameObject();
    newNode.AddComponent<navNode>();
    newNode.transform.position = _node.transform.position;
    // m_path.Add(newNode);
  }

  /// <summary>
  /// This function gets all the children of the current object that have the designated tag
  /// </summary>
  /// <param name="_object">reference to the object.</param>
  /// <param name="_children">reference to the list to instert the children.</param>
  /// <param name="tag">Tag to search for.</param>
  void
  getChildrenOfTag(ref GameObject _object, ref List<GameObject> _children, string tag)
  {
    foreach (Transform t in _object.transform)
    {
      if (t.tag == tag)
      {
        _children.Add(t.gameObject);
      }
    }
  }

  /// <summary>
  /// Gets the closest node in the navRoom object
  /// </summary>
  /// <returns>The closest node</returns>
  public GameObject
  getClosestNode()
  {
    GameObject node = new GameObject();
    GameObject newNode = new GameObject();
    for (int i = 0; i < m_navRooms[m_currentRoom].m_navNodes.Count; ++i)
    {
      // get the new node
      newNode = m_navRooms[m_currentRoom].m_navNodes[i];
      // if the distance of the new node is lower than the current node
      if ((GetDistanceTo(newNode) < GetDistanceTo(node) &&
          !newNode.GetComponent<navNode>().m_visited) || // or if the node is not set
          node == null)
      {
         node = newNode;
      }

      bool allChecked = true;
      // if node has not been visited
      for (int j = 0; j < m_navRooms[m_currentRoom].m_navNodes.Count; ++j) {
        if (!m_navRooms[m_currentRoom].m_navNodes[j].GetComponent<navNode>().m_visited)
        {
          // not all have been checked
          allChecked = false;
        }
      }
      // print(allChecked);
      // if all have been visited, jump to next room
      if (allChecked) { m_currentRoom -= 1; }
      Mathf.Clamp(m_currentRoom, 0, m_navRooms.Count);
    }
    return node;
  }

  /// <summary>
  /// Gets the distance between the angler and a game object.
  /// </summary>
  float
  GetDistanceTo(GameObject _t)
  {
    return Vector3.Distance(transform.position, _t.transform.position);
  }

  /// <summary>
  /// Follow path
  /// </summary>
  void
  FollowPath()
  {
    // look and move towards the current node in the current room
    transform.LookAt(m_path[m_currentRoom].m_navNodes[m_currentNode].transform);
    transform.position += transform.forward * Time.deltaTime * m_speed;
    
    // if the current node was already visited
    if (GetDistanceTo(m_path[m_currentRoom].m_navNodes[m_currentNode]) < m_distanceCheck)// (m_path[m_currentRoom].m_navNodes[m_currentNode].GetComponent<navNode>().m_visited)
    {
      // jump to next node
      ++m_currentNode;
      // if the current node location is greater than the count of ndoes in the current node
      if (m_currentNode >= m_path[m_currentRoom].m_navNodes.Count)
      {
        // move to next room and reset the current node back to 0
        --m_currentRoom;
        m_currentNode = 0;
      }
    }
  }

  /// <summary>
  /// Activate the entity jumpscare
  /// </summary>
  void
  Jumpscare()
  {
    // add up to jumpscare timer
    m_jumpscareTimer += Time.deltaTime;

    // while the jumpscare has not ended
    if (m_jumpscareTimer < m_jumpscareTime)
    {
      // get initial position
      Vector3 iniPos = m_startPosition.transform.position;
      if (!m_didJumpscare)
      {
        // set to initial position
        transform.position = new Vector3(iniPos.x, iniPos.y, iniPos.z - 2.0f);
        m_didJumpscare = true;
      }
      // new z position (move forward)
      float posZ = transform.position.z + m_forwardSpeed * Time.deltaTime;
      // get random position around entity
      Vector3 randomPos = m_startPosition.transform.position + Random.insideUnitSphere * m_shakeIntensity;
      // set position X and Y to the random pos, set Z to the forward pos
      m_sprite.transform.position = new Vector3(randomPos.x, randomPos.y , posZ);
      transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
      // make entity look at player
      transform.LookAt(iniPos);
      m_sprite.transform.LookAt(iniPos);
      
      // lock player in position
      m_player.transform.position = new Vector3(iniPos.x, iniPos.y, iniPos.z + 2.0f);
      // lock player camera
      m_player.transform.GetChild(0).GetChild(0).transform.LookAt(iniPos);

      // swap to munch sprite
      if (m_jumpscareTimer > m_jumpscareTime * 0.8f)
      {

      }
    }
    else // killed player
    {
      // reset jumpscare timer
      m_jumpscareTimer = 0.0f;

      // TP player to reading room
      SceneManager.LoadScene("ReadingRoom");
      m_jumpscaring = false;
    }
  }

  [Header("Player")]
  [SerializeField] private LayerMask m_layerPlayer;
  
  [Header("Sprite")]
  public GameObject m_sprite;
  List<RoomData> m_navRooms;
  GameObject m_player;

  [Header("Monster")]
  public string m_name = "";

  [Header("Initialization Data")]
  public int m_roomSpawn;
  private int m_currentRoom;
  public float m_speed;
  public float m_distanceCheck;
  
  public List<RoomData> m_path;
  int m_currentNode;
  
  [Header("Audio Clips")]
  [SerializeField] private AudioClip m_movingClip;
  [SerializeField] private AudioClip m_attackingClip;

  bool m_playedAttack = false;

  STATE m_state = STATE.kNone;

  [Header("Spawn parameters")]
  public int m_roomSpawnOffset;
  public int m_spawnChance;
  public float m_attackRange;
  
  [Header("Jumpscare")]
  public GameObject m_startPosition;
  public float m_forwardSpeed;
  public float m_shakeIntensity;
  public bool m_jumpscaring;
  public float m_jumpscareTimer = 0.0f;
  public float m_jumpscareTime = 1.0f;
  bool m_didJumpscare = false;
}
