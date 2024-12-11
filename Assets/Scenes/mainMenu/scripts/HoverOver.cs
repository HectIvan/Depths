using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  void
  Start()
  {
    m_startPos = m_button.transform.localPosition;
    Image refImage = m_button.GetComponent<Image>();
    // m_offsetHover = refImage.rectTransform.rect.width * 0.15f;
  }

  void
  Update()
  {
    if (Time.deltaTime != 0)
    {
      delta = Time.deltaTime;
    }
        else
        {
            delta = 0.016f;
        }
    if (m_isHovering)
    {
      HoverEffect();
    }
    else
    {
      ReturnToPos();
    }
  }

  void
  HoverEffect()
  {
    if (transform.localPosition.x < m_startPos.x + m_offsetHover)
    {
      Vector3 pos = transform.localPosition;
      pos.x += m_speed * delta;
      pos.y += m_speed * transform.rotation.z * delta;
      transform.GetComponent<Image>().transform.localPosition = pos;
    }
  }

  void
  ReturnToPos()
  {
    if (transform.localPosition.x > m_startPos.x)
    {
      Vector3 pos = transform.localPosition;
      pos.x -= m_speed * delta;
      pos.y -= m_speed * transform.rotation.z * delta;
      transform.GetComponent<Image>().transform.localPosition = pos;
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    m_isHovering = true;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    m_isHovering = false;
  }

  public GameObject m_button;
  private float delta;
  private Vector3 m_startPos;
  public float m_offsetHover;
  public float m_speed;

  public bool m_isHovering;
}
