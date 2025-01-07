using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  // Start is called before the first frame update
  void
  Start()
  {
    slots = new List<GameObject>();
  }

  // Update is called once per frame
  void
  Update()
  {
    UserInput();
  }

  void
  UserInput()
  {
    // Check for number keys (0-9)
    for (Int16 i = 0; i <= 9; i++)
    {
      if (Input.GetKeyDown(i.ToString()))
      {
        selected = i;
        print(selected);
      }
    }
  }

  /// <summary>
  /// Insert a new slot to the inventory and reorganize the entire inventory.
  /// </summary>
  public void
  InsertSlot()
  {
    // instantiate a slot prefab
    var slot = Instantiate(slotPrefab,
                           inventoryPoint.transform.position,
                           inventoryPoint.transform.rotation,
                           inventoryPoint.transform);
    // add to the list
    slots.Add(slot);
    // reorganize the inventory
    ReorganizeInventorySlots();
  }

  /// <summary>
  /// Pop an inventory slot in the desired index.
  /// </summary>
  /// <param name="_index">Index that will be popped.</param>
  void
  PopSlot(Int32 _index)
  {
    slots.RemoveAt(_index);
  }

  /// <summary>
  /// Reorganize the inventory slots.
  /// </summary>
  void
  ReorganizeInventorySlots()
  {
    // get the width and height of the slot icon
    float width = slotPrefab.GetComponent<RectTransform>().rect.width;
    float height = slotPrefab.GetComponent<RectTransform>().rect.height;

    // inventory size will be the width of the slot multiplied by the ammount, summed by the
    // total spacing count between each slot
    float invSize = (width * slots.Count) + (slotSpacing * slots.Count - 1);

    float newX = -invSize * 0.5f + width * 0.5f;

    // for each slot in the inventory
    foreach (GameObject slot in slots) {
      slot.transform.localPosition = new Vector3(newX, slot.transform.localPosition.y, 0.0f);
      newX += width + slotSpacing;
    }
  }

  public GameObject slotPrefab;
  public GameObject inventoryPoint;
  public float slotSpacing;
  private List<GameObject> slots;
  private Int16 selected;
}
