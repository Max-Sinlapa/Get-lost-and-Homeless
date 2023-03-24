using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidPageDTW : MonoBehaviour
{
    public RectTransform Panel;
    // Start is called before the first frame update
    void Start()
    {
       Panel.DOAnchorPos(Vector2.zero, 1f);  
    }

    
}
