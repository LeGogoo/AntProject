using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool randomCreatBox = true;
    public int lengthSize = 9;
    public int widthSize = 5;
    public float consumeTime = 2.0f;

    public RectTransform rectTransform;
    public GameObject box;

    public List<List<GameObject>> boxList0;
    public List<GameObject> boxList1;


    private float m_boxLength;
    private float m_boxHeight;

    private void Start()
    {
        boxList0 = new List<List<GameObject>>();
        boxList1 = new List<GameObject>();
        m_boxLength = rectTransform.sizeDelta.x / lengthSize;
        m_boxHeight = rectTransform.sizeDelta.y / widthSize;

        Vector2 startPosition = new Vector2(-m_boxLength * (lengthSize / 2), -m_boxHeight * (widthSize / 2));
        if(lengthSize % 2 == 0)
        {
            startPosition.x += m_boxLength / 2;
        }
        if (widthSize % 2 == 0)
        {
            startPosition.y += m_boxHeight / 2;
        }

        int iLayer, jLayer, lastLayer;
        for (int j = 0; j < widthSize; j++)
            for (int i = 0; i < lengthSize; i++)
            {
                var newGameObject = Instantiate(box);
                newGameObject.transform.SetParent(transform);
                newGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_boxLength, m_boxHeight);
                newGameObject.GetComponent<RectTransform>().localPosition = startPosition + new Vector2(i * m_boxLength, j * m_boxHeight);
                newGameObject.SetActive(false);

                iLayer = i < lengthSize / 2 ? i : lengthSize - i - 1;
                jLayer = j < widthSize / 2 ? j : widthSize - j - 1;
                lastLayer = iLayer < jLayer ? iLayer : jLayer;
                if (boxList0.Count <= lastLayer)
                {
                    boxList0.Add(new List<GameObject>());
                }
                boxList0[lastLayer].Add(newGameObject);
                boxList1.Add(newGameObject);
            }
    }

    private BoxManager m_currentConsume = null;

    private void Update()
    {
        if (m_currentConsume != null && !m_currentConsume.workOut)
        {
            return;
        }
        foreach (var box in boxList1)
        {
            if(box.active)
            {
                var boxManager = box.GetComponent<BoxManager>();
                if (!m_currentConsume)
                {
                    boxManager.StartConsume(consumeTime);
                    m_currentConsume = boxManager;
                    return;
                }
                else
                {
                    if (!boxManager.workOut)
                    {
                        boxManager.StartConsume(consumeTime);
                        m_currentConsume = boxManager;
                        return;
                    }
                }
            }
        }
    }

    public void ActiveOneBox()
    {
        if (randomCreatBox)
        {
            List<Vector2Int> positionPool = new List<Vector2Int>();
            for (int i = 0; i < boxList0.Count; i++)
            {
                for (int j = 0; j < boxList0[i].Count; j++)
                {
                    if (boxList0[i][j].active == false)
                    {
                        positionPool.Add(new Vector2Int(i, j));
                    }
                }
                if (positionPool.Count != 0)
                {
                    int index = Random.Range(0, positionPool.Count - 1);
                    boxList0[positionPool[index].x][positionPool[index].y].SetActive(true);
                    return;
                }
            }
        }

        else
        {
            for (int i = 0; i < boxList0.Count; i++)
            {
                for (int j = 0; j < boxList0[i].Count; j++)
                {
                    if (boxList0[i][j].active == false)
                    {
                        boxList0[i][j].SetActive(true);
                        return;
                    }
                }
            }
        }


    }
}
