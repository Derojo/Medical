using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{

    public Color defaultColor;
    public Color selectedColor;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void OnTouchDown()
    {
        mat.color = selectedColor;

    }

    void OnTouchUp()
    {
        mat.color = defaultColor;
    }
    void OnTouchStay()
    {
        mat.color = selectedColor;
    }
    void OnTouchExit()
    {
        mat.color = defaultColor;
    }

    void update()
    {

    }


}//end class butoon
