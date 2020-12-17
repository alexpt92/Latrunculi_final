using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    private Text textSliderValue;
    // Start is called before the first frame update
    void Start()
    {
        textSliderValue = GetComponent<Text>();
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        string sliderMessage = "";
        if (sliderUI.name.Contains("Attack"))
        {
            sliderMessage = "Angriff = " + sliderUI.value;
        }
        else if (sliderUI.name.Contains("Hide"))
        {
            sliderMessage = "Verteidigung = " + sliderUI.value;
        }
        else if (sliderUI.name.Contains("Threat"))
        {
            sliderMessage = "Bedrohung = " + sliderUI.value;
        }

        //string sliderMessage = sliderUI.name.Substring(0, sliderUI.name.Length-6) +  "Points = " + sliderUI.value;
        textSliderValue.text = sliderMessage;
    }
    public void ChangeAttackPoints()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        referenceScript = referenceObject.GetComponent<GameManager>();
        referenceScript.ChangeAttackPoints(sliderUI.value);

    }

    public void ChangeAttackPoints2()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        referenceScript = referenceObject.GetComponent<GameManager>();
        referenceScript.ChangeAttackPoints2(sliderUI.value);

    }


    public void ChangeHidePoints()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        // referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
        referenceScript = referenceObject.GetComponent<GameManager>();
        // referenceScript.pointHide = sliderUI.value;
        referenceScript.ChangeHidePoints(sliderUI.value);
    }
    public void ChangeHidePoints2()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        // referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
        referenceScript = referenceObject.GetComponent<GameManager>();
        // referenceScript.pointHide = sliderUI.value;
        referenceScript.ChangeHidePoints2(sliderUI.value);
    }

    public void ChangeThreatPoints()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        // referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
        referenceScript = referenceObject.GetComponent<GameManager>();
        // referenceScript.pointHide = sliderUI.value;
        referenceScript.ChangeThreatPoints(sliderUI.value);
    }
    public void ChangeThreatPoints2()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        // referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
        referenceScript = referenceObject.GetComponent<GameManager>();
        // referenceScript.pointHide = sliderUI.value;
        referenceScript.ChangeThreatPoints2(sliderUI.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
