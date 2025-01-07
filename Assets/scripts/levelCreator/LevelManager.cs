using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.UIElements;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  /// <summary>
  /// Start the level creator
  /// </summary>
  void
  Start()
  {
    // m_angler = GameObject.Find("angler");
    SFXManager.m_instance.PlayLoopParentedSFXClip(m_ambienceSound, m_player.transform, 3.0f, m_player.transform);
    m_attackTimer = m_attackTime;
    
    loadRooms(ref m_roomPrefabs, "rooms");
    loadRooms(ref m_rareRoomsPrefabs, "rareRooms");
    loadRooms(ref m_epicRoomsPrefabs, "epicRooms");
    instantiateRooms();
  }

  /// <summary>
  /// Update the level creator
  /// </summary>
  void
  Update()
  {
    if (m_optimizeRooms) {
      Optimization();
    }
    PlayAmbienceSounds();
    AttackTimer();
  }

  /// <summary>
  /// Optimizes room activation.
  /// 
  /// by setting a render distance from the current room to the desired length,
  /// every other room is deactivated, leading to an increase in performance.
  /// </summary>
  void
  Optimization()
  {
    int roomNumber;
    int currentPlayerRoom = m_player.GetComponent<playerMovement>().m_currentRoom;
    foreach (GameObject room in m_rooms) {
      roomNumber = room.transform.GetChild(0).GetComponent<Room>().m_number;
      // if the current room is 10 layers below or above the current player room
      if (roomNumber < currentPlayerRoom - m_renderTolerance ||
          roomNumber > currentPlayerRoom + 1) {
        SetRoomState(room, false);
      }
      else {
        SetRoomState(room, true);
      }
    }
    if (currentPlayerRoom > m_renderTolerance) {
      startRoom.SetActive(false);
    }
  }

  void
  SetRoomState(GameObject _room, bool _state)
  {
    _room.gameObject.SetActive(_state);
    for (int i = 0; i < _room.transform.childCount; ++i) {
      _room.transform.GetChild(i).gameObject.SetActive(_state);
    }
  }

  /// <summary>
  /// Load room prefabs from a directory
  /// </summary>
  /// <param name="_prefabs">Reference of a list where the prefabs will be stored.</param>
  /// <param name="_directory">Directory where the prefabs are stored.</param>
  void
  loadRooms(ref List<GameObject> _prefabs, string _directory)
  {
    GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(_directory);

    _prefabs = new List<GameObject>();

    foreach (GameObject prefab in loadedPrefabs) {
      _prefabs.Add(prefab);
    }
  }

  /// <summary>
  /// Instantiate all rooms.
  /// </summary>
  void
  instantiateRooms()
  {
    // all rooms instantiated
    m_rooms = new List<GameObject>();

    // instantiate initial room
    var module = Instantiate(m_roomPrefabs[0], transform.position, transform.rotation);
    m_rooms.Add(module);
    module.transform.GetChild(0).GetComponent<Room>().m_number = 0;
    // m_angler.GetComponent<angler>().setNavRoomNodes(module.transform.GetChild(0).gameObject);

    // for each instance in the remaining room count
    for (int i = 1; i < m_roomCount; ++i) {
      int type = UnityEngine.Random.Range(0, 100);
      
      // if the chance is that of an epic room
      if (type < m_epicRoomChance) {
        TypeRoomInstantiate(ref m_epicRoomsPrefabs, ref module, i);
      }
      // if the chance is that of a rare room
      else if (type < m_rareRoomChance) {
        TypeRoomInstantiate(ref m_rareRoomsPrefabs, ref module, i);
      }
      // if the chance is that of a normal room
      else if (type < m_normalRoomChance) {
        TypeRoomInstantiate(ref m_roomPrefabs, ref module, i);
      }
      // if for some reason it doesnt fit any chance, create a normal room
      else {
        TypeRoomInstantiate(ref m_roomPrefabs, ref module, i);
      }
    }
    for (int i = 0; i < m_nodeMonsters.Length; ++i) {
      m_nodeMonsters[i].GetComponent<NodeMonster>().Clear();
      SetNodeMonsterRooms(ref m_nodeMonsters[i]);
    }
  }

  /// <summary>
  /// Instantiate room of specific prefab list.
  /// </summary>
  /// <param name="_prefabs">List of prefabs</param>
  /// <param name="_module">Current room instance</param>
  /// <param name="_number">Room number</param>
  void
  TypeRoomInstantiate(ref List<GameObject> _prefabs, ref GameObject _module, int _number)
  {
    // if the prefab list is not empty
    if (_prefabs.Count > 0) {
      int randomIndex = UnityEngine.Random.Range(0, _prefabs.Count); // choose random room from prefabs
      GameObject nextSpawn = _module.transform.GetChild(0).GetComponent<Room>().m_nextSpawnPoint; // get spawn point
      _module = Instantiate(_prefabs[randomIndex], nextSpawn.transform.position, nextSpawn.transform.rotation); // instantiate the new room
      _module.transform.GetChild(0).GetComponent<Room>().m_number = _number; // assign number to new room
      m_rooms.Add(_module); // add room to the room list
      // m_angler.GetComponent<angler>().SetRoomNav(ref _module);
    }
  }

  public void
  SetNodeMonsterRooms(ref GameObject _monster)
  {
    _monster.GetComponent<NodeMonster>().Clear();
    foreach (GameObject room in m_rooms) {
      _monster.GetComponent<NodeMonster>().setNavRoomNodes(room);
    }
    // _monster.GetComponent<NodeMonster>().Init();
  }
  
  void
  PlayAmbienceSounds()
  {
    if (m_rSoundTimer == 0) {
      m_rSoundTimer = UnityEngine.Random.Range(1, m_maxSoundTimerPossible);
    }
    m_soundTime += Time.deltaTime;
    if (m_soundTime > m_rSoundTimer) {
      m_rSoundTimer = 0;
      m_soundTime = 0;
      SFXManager.m_instance.PlayRandomParentedSFXClips(m_randomSounds, m_player.transform, 1.0f, m_player.transform);
    }
  }

  void
  AttackTimer()
  {
    // check if attack Time has expired (an attack is initialized)
    if (m_player.GetComponent<playerMovement>().m_currentRoom > 0) {
      m_attackTime -= Time.deltaTime;
    }
    if (m_attackTime < 0) {
      m_attackTime = m_attackTimer;
      // select a random chance
      int chance = UnityEngine.Random.Range(0, m_totalChane);
      // for each entity in the list
      bool spawnedEntity = false;
      for (int i = 0; i < m_nodeMonsters.Length; ++i) {
        // get their spawn chance
        int monsterChance = m_nodeMonsters[i].GetComponent<NodeMonster>().m_spawnChance;
        // check if the spawn chance is equal or lower than the selected monster
        if (chance <= m_spawnChances[i]) {
          ActivateEntity(ref m_nodeMonsters[i]);
          FlickerRooms();
          spawnedEntity = true;
          break;
        }
      }
      // if no entity spawned spawn the most common
      if (!spawnedEntity) {
        ActivateEntity(ref m_nodeMonsters[m_nodeMonsters.Length - 1]);
        FlickerRooms();
      }
      
    }
  }

  public void
  SetEpileptic(bool _epileptic)
  {
    m_epileptic = _epileptic;
  }

  void
  FlickerRooms()
  {
    foreach (GameObject _room in m_rooms)
    {
      if (_room.gameObject.activeSelf) {
        SFXManager.m_instance.PlayParentedSFXClip(m_flickerSound, _room.transform, 1.0f, _room.transform);
      }
      if (_room.transform.GetChild(0).GetComponent<Room>().m_number <= m_player.GetComponent<playerMovement>().m_currentRoom) {
        for (int i = 0; i < _room.transform.GetChild(0).GetComponent<Room>().m_lights.Count; ++i) {
          _room.transform.GetChild(0).GetComponent<Room>().m_lights[i].GetComponent<LightSource>().m_epileptic = m_epileptic;
          _room.transform.GetChild(0).GetComponent<Room>().m_lights[i].GetComponent<LightSource>().m_flickering = true;
        }
      }
    }
  }

  void
  BreakRoomLights()
  {
    // foreach (GameObject _room in m_rooms)
    // {
    //   for (int i = 0; i < _room.transform.GetChild(0).GetComponent<Room>().m_lights.Count; ++i)
    //   {
    //     _room.transform.GetChild(0).GetComponent<Room>().m_lights[i].GetComponent<LightSource>().Break();
    //   }
    // }
  }

  void
  ActivateEntity(ref GameObject _entity) // current bug, entity does not send lists correctly
  {
    _entity.SetActive(true);
    int currPlayerRoom = m_player.GetComponent<playerMovement>().m_currentRoom;
    int spawnOffset = _entity.GetComponent<NodeMonster>().m_roomSpawnOffset;
    int spawnRoom = currPlayerRoom + spawnOffset;
    if (spawnRoom < 0) { spawnRoom = 0; }
    if (spawnRoom > m_roomCount) { return;}
    _entity.GetComponent<NodeMonster>().m_roomSpawn = spawnRoom;
    _entity.GetComponent<NodeMonster>().Init();
  }

  private List<GameObject> m_rooms;
  private List<GameObject> m_roomPrefabs;
  private List<GameObject> m_rareRoomsPrefabs;
  private List<GameObject> m_epicRoomsPrefabs;

  [Header("Room Chances")]
  public int m_normalRoomChance;
  public int m_rareRoomChance;
  public int m_epicRoomChance;

  [Header("Room Count")]
  public int m_roomCount;

  [Header("Node Monsters\n(Rarest to most common)\n(Add chances respective to the node monster)")]
  public GameObject[] m_nodeMonsters;
  public int[] m_spawnChances;
  public int m_totalChane;

  [Header("Player reference")]
  public GameObject m_player;
  
  [Header("Room optimization")]
  public int m_renderTolerance;
  public bool m_optimizeRooms;
  public GameObject startRoom;

  [Header("Sounds")]
  [SerializeField] private AudioClip m_ambienceSound;
  [SerializeField] private AudioClip m_flickerSound;
  [SerializeField] private AudioClip[] m_randomSounds;
  private float m_rSoundTimer = 0;
  private float m_soundTime = 0;
  public float m_maxSoundTimerPossible;

  [Header("Angler Attack")]
  public float m_attackTime;
  private float m_attackTimer;

  [Header("Accessibility")]
  public bool m_epileptic;
}
