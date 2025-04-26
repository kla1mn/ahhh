using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollMod : MonoBehaviour
{
    
    private List<RectTransform> contentObjts;
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scroll;

    public void SetScrollToDefaultValue()
    {
        SetContentObjcts();

        var size = content.sizeDelta;
        size.y = SizeY();
        content.sizeDelta = size;
        scroll.verticalNormalizedPosition = 1f;
    }
    public void PrepareToStart()
    {
        var vertival = content.GetComponent<VerticalLayoutGroup>();
        vertival.enabled = false;
    }

    private void SetContentObjcts()
    {
        contentObjts = new List<RectTransform>();
        contentObjts?.Clear();

        foreach (RectTransform child in content.GetComponentInChildren<RectTransform>())
            contentObjts.Add(child);
    }

    private float SizeY()
    {
        var vertival = content.GetComponent<VerticalLayoutGroup>();
        vertival.enabled = true;
        var spacing = vertival != null ? vertival.spacing : 0;
        var ySize = 0f;
        for (int i = 0; i < contentObjts.Count; i++)
        {
            contentObjts[i].gameObject.SetActive(false);
            contentObjts[i].gameObject.SetActive(true);

            ySize += contentObjts[i].sizeDelta.y + spacing;

            //Debug.Log(contentObjts[i].gameObject.name + " "+ contentObjts[i].sizeDelta.y + " " + spacing);
        }
        return ySize;
    }
}

