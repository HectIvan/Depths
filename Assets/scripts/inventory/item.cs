using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
  // Start is called before the first frame update
  void
  Start()
  {
    inter = GetComponent<interactable>();
  }

  // Update is called once per frame
  void
  Update()
  {
    if (inter) {
      if (inter.m_interacted) {
        PickUp();
      }
    }
  }

  void
  PickUp()
  {
    GameObject player = GameObject.Find("Player");
    if (player != null) {
      player.GetComponent<Inventory>().InsertSlot();
      gameObject.SetActive(false);
    }
  }

  private interactable inter;
}
