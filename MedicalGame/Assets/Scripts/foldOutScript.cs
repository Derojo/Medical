using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class foldOutScript : Toggle
    {
        public  float startHeight = 54f;
        private GameObject content;

        // Use this for initialization
        void Start()
        {
            startHeight = this.GetComponent<LayoutElement>().preferredHeight;
            content = this.transform.Find("Content").gameObject;

        }

        // Update is called once per frame
        void Update()
        {
            if (base.isOn)
            {
             
			this.GetComponent<LayoutElement> ().DOMinSize (new Vector2 (0, (LayoutUtility.GetPreferredHeight (content.GetComponent<RectTransform> ()) + startHeight)),0.5f, false).SetEase(Ease.OutExpo);

            }
            else
            {
			this.GetComponent<LayoutElement> ().DOMinSize (new Vector2 (0, startHeight), 0.5f, false).SetEase(Ease.OutExpo);
//     
            }

        }

    }

