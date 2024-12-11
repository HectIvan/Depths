using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
  /// <summary>
  /// Start is called before the first frame update
  /// </summary>
  void
  Start()
  {
    if (m_room != null)
    {
        // get the next room number
        int roomNumber = ++m_room.GetComponent<Room>().m_number;

        // set the number as the text of the number display
        m_number.GetComponent<TextMeshPro>().text = NumberToString(roomNumber);
    }
  }

  /// <summary>
  /// Update is called once per frame
  /// </summary>
  void
  Update()
  {
    
  }

  string
  NumberToString(int _number)
  {
    ++_number;
    string finalText = "";
    int numZeros = 3 - _number.ToString().Length;
    if (numZeros < 0)
    {
      return "WTH";
    }
    for (int i = 0; i < numZeros; ++i)
    {
      finalText += "0";
    }
    finalText += _number.ToString();
    return finalText;
  }

  public GameObject m_room;
  public GameObject m_number;
}
