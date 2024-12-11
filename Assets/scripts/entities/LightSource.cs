using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class LightSource : MonoBehaviour
{
  // Start is called before the first frame update
  void
  Start()
  {
    m_flickering = false;
    m_broken = false;
    m_epileptic = false;
    m_originalIntensity = m_lightSource.intensity;
    m_switchDuration = 0.2f;
    m_flickerDuration = 1.5f;
    m_flickerTimer = 0.0f;
    m_timer = 0.0f;
  }

  // Update is called once per frame
  void
  Update()
  {
    if (m_flickering && !m_broken)
    {
      m_flickerTimer += Time.deltaTime;
      if (m_epileptic)
      {
        FlickerEpileptic();
      }
      else
      {
        FlickerNormal();
      }
      if (m_flickerTimer > m_flickerDuration)
      {
        m_flickering = false;
        m_flickerTimer = 0;
        m_lightSource.intensity = m_originalIntensity;
      }
    }
  }

  void
  FlickerNormal()
  {
    // if its in time to be on
    if (m_timer < m_switchDuration)
    {
      m_timer += Time.deltaTime;
      m_lightSource.intensity = m_originalIntensity;
    }
    else if (m_timer > 0)
    {
      m_timer -= Time.deltaTime;
      m_lightSource.intensity = 0;
    }
    else
    {
      m_lightSource.intensity = m_originalIntensity;
    }
  }

  void
  FlickerEpileptic()
  {
    if (m_flickerTimer < m_flickerDuration * 0.5f)
    {
      if (m_lightSource.intensity > 0)
      {
        m_lightSource.intensity -= m_originalIntensity * Time.deltaTime;
      }
    }
    else
    {
      if (m_lightSource.intensity < m_originalIntensity)
      {
        m_lightSource.intensity += m_originalIntensity * Time.deltaTime;
      }
    }
  }
  
  public void
  Break()
  {
    m_broken = true;
    m_lightSource.intensity = 0;
  }

  // Accessibility
  public bool m_epileptic;

  [Header("Light")]
  public Light m_lightSource;
  private float m_originalIntensity;

  [Header("Flickering")]
  public float m_switchDuration;
  public bool m_flickering;
  public float m_flickerDuration;
  private float m_timer;
  private float m_flickerTimer;
  private bool m_broken;
}
