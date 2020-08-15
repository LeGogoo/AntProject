using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public float progress;
    public RectTransform trans0;
    public RectTransform trans1;
    public bool workOut = false;
    public bool m_consuming = false;
    private float m_consumeTime;
    private void Update()
    {
        if (m_consuming)
        {
            progress += 1 / m_consumeTime * Time.deltaTime;
            if(progress >= 1)
            {
                m_consuming = false;
                workOut = true;
            }
        }
        trans1.sizeDelta = new Vector2((trans0.sizeDelta.x - 6) * progress, trans1.sizeDelta.y);
    }
    public void StartConsume(float consumeTime)
    {
        this.m_consumeTime = consumeTime;
        m_consuming = true;
    }

}
