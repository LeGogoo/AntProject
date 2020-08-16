using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardManager : MonoBehaviour
{
    [Tooltip("初始显示的个数，也是同时显示的个数")]
    public int startCount;

    public List<GameObject> awardList = new List<GameObject>();
    private HashSet<int> m_curShowIndex = new HashSet<int>();

    private void Awake()
    {
        if(startCount < m_curShowIndex.Count)
        {
            startCount = m_curShowIndex.Count;
        }

        foreach(var award in awardList)
        {
            award.SetActive(false);
        }
    }

    private void Start()
    {
        for(int i = 0; i < startCount; i++)
        {
            CreatAward();
        }
    }

    public void CreatAward(GameObject award)
    {
        CreatAward();
        for(int i = 0; i < awardList.Count; i++)
        {
            if(awardList[i] == award)
            {
                m_curShowIndex.Remove(i);
                award.SetActive(false);
                break;
            }
        }
    }

    private void CreatAward()
    {
        int newIndex;
        do
        {
            newIndex = Random.Range(0, awardList.Count);
        }
        while (m_curShowIndex.Contains(newIndex));

        m_curShowIndex.Add(newIndex);
        awardList[newIndex].SetActive(true);
    }
}
