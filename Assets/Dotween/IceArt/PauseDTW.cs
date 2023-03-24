using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseDTW : MonoBehaviour
{
    public RectTransform pausePanel;
    // Start is called before the first frame update
    void Start()
    {
       pausePanel.DOAnchorPos(Vector2.zero, 1f);  
    }

    
}
