using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detector : MonoBehaviour
{
    [Title("Values")]
    public float detectionRateMultiplier;
    public float detectionStagnateTimer;

    [Title("Debug")]
    [ProgressBar("detectionRate", 100, DrawValueLabel = false)] public float detectionRate;
    public bool isInRange;
    public bool isDecreasing;
    public Coroutine coroutine;
    public bool detected;
    public Detectable detectable;

    [FoldoutGroup("References")] public Collider detectorZone;
    [FoldoutGroup("References")] public Image radialBarLeft;
    [FoldoutGroup("References")] public Image radialBarRight;
    [FoldoutGroup("References")] public Image icon;
    [FoldoutGroup("References")] public Sprite questionMark;
    [FoldoutGroup("References")] public Sprite exclamationMark;
    [FoldoutGroup("References")] public Color questionMarkColour;
    [FoldoutGroup("References")] public Color exclamationMarkColour;

    void Start()
    {
        icon.gameObject.SetActive(false);
        icon.sprite = questionMark;
        icon.color = questionMarkColour;
    }

    void OnTriggerEnter(Collider other)
    {
        if (detected == false)
        {
            if (other.CompareTag("Detectable"))
            {
                detectable = other.GetComponent<Detectable>();
                isInRange = true;
                isDecreasing = false;
                if (coroutine != null)
                {
                    StopStagnate();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (detected == false)
        {
            if (other.CompareTag("Detectable"))
            {
                isInRange = false;
                if (detectionRate > 0)
                {
                    StartStagnate();
                }
            }
        }
    }

    void Update()
    {
        if (detected == false)
        {
            if (isInRange)
            {
                Debug.Log("Detecting");
                detectionRate = Mathf.Lerp(detectionRate, 200, detectionRateMultiplier * Time.deltaTime);
                icon.gameObject.SetActive(true);
                radialBarLeft.gameObject.SetActive(true);
                radialBarRight.gameObject.SetActive(true);
            }

            if (isDecreasing == true)
            {
                detectionRate = Mathf.Lerp(detectionRate, -1, (detectionRateMultiplier * 10) * Time.deltaTime);
            }

            if (detectionRate <= 0)
            {
                isDecreasing = false;
                icon.gameObject.SetActive(false);
                if (detectable != null)
                {
                    detectable.detectionRate = 0;
                    detectable = null;
                }
                detectionRate = 0;
            }

            if (detectionRate >= 100)
            {
                detectionRate = 100;
                isInRange = false;
                Detected();
            }

            if (detectable != null)
                detectable.detectionRate = detectionRate;
            radialBarLeft.fillAmount = detectionRate / 100;
            radialBarRight.fillAmount = detectionRate / 100;
        }
    }

    void Detected()
    {
        detected = true;
        radialBarLeft.gameObject.SetActive(false);
        radialBarRight.gameObject.SetActive(false);
        icon.sprite = exclamationMark;
        icon.color = exclamationMarkColour;
        Debug.Log("Detected!");
    }
    void StartStagnate()
    {
        coroutine = StartCoroutine(Stagnate());
    }

    void StopStagnate()
    {
        StopCoroutine(coroutine);
        coroutine = null;
        icon.gameObject.SetActive(false);
        radialBarLeft.gameObject.SetActive(false);
        radialBarRight.gameObject.SetActive(false);
    }

    IEnumerator Stagnate()
    {
        yield return new WaitForSeconds(detectionStagnateTimer);
        isDecreasing = true;
    }
}
