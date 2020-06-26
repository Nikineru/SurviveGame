using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    private GameObject ProgressBar;
    void Start()
    {
        ProgressBar = transform.Find("ProgressBar").gameObject;
    }
    public void SetProgressBar(float value) 
    {
        ProgressBar.GetComponent<Image>().fillAmount = value;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
