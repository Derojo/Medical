using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class foldOutScript : Toggle
    {
        public  float startHeight = 0f;
        private GameObject content;

        // Use this for initialization
        void Start()
        {
            startHeight = this.GetComponent<LayoutElement>().preferredHeight;
            content = this.transform.Find("content").gameObject;

        }

        // Update is called once per frame
        void Update()
        {
            if (base.isOn)
            {
                //Debug.Log(LayoutUtility.GetPreferredHeight(content.GetComponent<RectTransform>()));
                this.GetComponent<LayoutElement>().preferredHeight = LayoutUtility.GetPreferredHeight(content.GetComponent<RectTransform>()) + startHeight;
                //Debug.Log(wantedHeight);
            }
            else {
                this.GetComponent<LayoutElement>().preferredHeight = startHeight;
            }

        }

    }

