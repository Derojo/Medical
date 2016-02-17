using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClampRectTransform : MonoBehaviour {

    public float padding = 100.0f;
    public float elementSize = 72.0f;
    public float viewSize = 800.0f;

    private RectTransform rt;
    private int amountElements;
    private float contentSize;
    
    private void Start()
    {
        rt = GetComponent<RectTransform>();
     
    }

    private void Update()
    {
        //Clamp out rect transform
        amountElements = rt.childCount;
        contentSize = ((amountElements * (elementSize + padding )) - viewSize) * rt.localScale.y;

        //Checking for scroll enable after 4 or more items
        if (rt.childCount > 4)
        {
            GameObject.Find("ButtonContainer").GetComponent<ScrollRect>().enabled = true;

            //Clamping top
            if (rt.localPosition.y < padding)
            {
                rt.localPosition = new Vector3(rt.localPosition.x, padding, rt.localPosition.z);
            }
            //clamping bottom
            if (rt.localPosition.y > contentSize)
            {
                rt.localPosition = new Vector3(rt.localPosition.x, contentSize, rt.localPosition.z);
            }
        }//end if childcount statement

      
        
    }
}
