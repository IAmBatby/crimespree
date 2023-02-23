using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetectable : Detectable
{
    public Image radialBarLeft;
    public Image radialBarRight;
    public Image icon;
    public Sprite questionMark;
    public Sprite exclamationMark;
    public Color questionMarkColor;
    public Color exclamationMarkColor;
    public bool isDetected;

    void Start()
    {
        icon.sprite = questionMark;
        icon.color = questionMarkColor;
        icon.gameObject.SetActive(false);
    }

    void Update()
    {
        if(detectionRate <= 0 )
        {
            radialBarLeft.gameObject.SetActive(false);
            radialBarRight.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
        }
        else
        {
            radialBarLeft.gameObject.SetActive(true);
            radialBarRight.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
        }

        if(detectionRate >= 100)
        {
            radialBarLeft.gameObject.SetActive(false);
            radialBarRight.gameObject.SetActive(false);
            Detected();
        }

        radialBarLeft.fillAmount = detectionRate / 100;
        radialBarRight.fillAmount = detectionRate / 100;
    }


    void Detected()
    {
        icon.sprite = exclamationMark;
        icon.color = exclamationMarkColor;
    }
}
