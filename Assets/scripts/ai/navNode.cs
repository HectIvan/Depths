using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navNode : MonoBehaviour
{
  void
  Start()
  {
    m_visited = false;
    m_angler = GameObject.Find("angler");
  }

  void
  Update()
  {
    if (m_angler != null) {
      // if the node is in the range of the angler distance check
      if (Vector3.Distance(m_angler.transform.position, transform.position) < m_angler.GetComponent<NodeMonster>().m_distanceCheck)
      {
        // node is visited
        // m_angler.transform.LookAt(m_angler.GetComponent<angler>().getClosestNode().transform);
        m_visited = true;
      }
    }
  }
  
  public bool m_visited;
  GameObject m_angler;
}
