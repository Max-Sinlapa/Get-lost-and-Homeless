using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV.Manager;
using UnityEngine;

public class UI_HP : MonoBehaviour
{
    public RectTransform _hpListContent;
    public GameObject _HpPrefab;
    
    private Dictionary<string, int> cachedHp_List;
    private Dictionary<string, GameObject> hpListEntries;

    public List<GameObject> childObject;

    public bool multiplayer;
    
    private void Awake()
    {
        cachedHp_List = new Dictionary<string, int>();
        hpListEntries = new Dictionary<string, GameObject>();

        if (_hpListContent == null)
            Debug.LogError("Missing room list Content.");

        UpdateUIhp();
    }

    private void Update()
    {
        
    }

    private void ClearHP_ListView()
    {
        foreach (GameObject entry in hpListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
        
        hpListEntries.Clear();

        if (childObject.Count > 1 )
        {
            foreach (GameObject hpChild in childObject)
            {
                Destroy(hpChild);
            }
        }
        
        
    }
    
    
    private void UpdateHP_ListView()
    {
        //print(cachedRoomList.Count);
        for (int i = 0; i < m_GameManager._allPlayerCurrentHealth; i++)
        {
            GameObject entry = Instantiate(_HpPrefab);
            entry.transform.SetParent(_hpListContent.transform);
            entry.transform.localScale = Vector3.one;
            
            childObject.Add(entry);
        }
        
    }

    public void UpdateUIhp()
    {
        ClearHP_ListView();
        UpdateHP_ListView();
    }
}
