using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EscapeMenuActivator : MonoBehaviour
{

    public Button quit;
    public Button restart;
    public Button start;
    public Text quitText;
    public Text startText;
    public Text attackText;
    public Text attackText2;
    public Text hideText;
    public Text hideText2;
    public Text threatText;
    public Text threatText2;
    public TextMeshPro gameOverMessage;


    public bool EscapeMenuOpen;
    public Slider attackSlider;
    public Slider threatSlider;
    public Slider hideSlider;
    public Slider attackSlider2;
    public Slider threatSlider2;
    public Slider hideSlider2;
    public Toggle firstAiToggle;
    public Toggle secondAiToggle;
    public Dropdown DDX;
    public Dropdown DDY;

    public float uiBaseWidth;
    public float baseFontSize;

    Vector2 initialVectorBottomLeft;
    Vector2 initialVectorTopRight;

    Vector2 updatedVectorBottomLeft;
    Vector2 updatedVectorTopRight;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = quit.GetComponent<RectTransform>();
        RectTransform rectTransformRestart = restart.GetComponent<RectTransform>();
        //GameObject.FindGameObjectWithTag("menuCanvas").GetComponent<RectTransform>().anchoredPosition = new Vector2((float)1,(float)1);
        rectTransform.anchoredPosition = new Vector2((float)0.5, (float)0.5);
        // rectTransformRestart.position = new Vector2((float)0.5, (float)0.25);
        quit.gameObject.SetActive(true);
        initialVectorBottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        initialVectorTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        resizeUI();
    }

    // Update is called once per frame
    void Update()
    {

        updatedVectorBottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        updatedVectorTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeMenuOpen == false)
            {
                //SceneManager.LoadScene(1);
                EscapeMenuOpen = true;
                // GameObject.Find("escape").SetActive(true);
                // GameObject.Find("restart").SetActive(true);

               // quit.gameObject.SetActive(true);
               // restart.gameObject.SetActive(true);
               if (firstAiToggle) 
                setSliderValues();
            }
            else
            {
                //SceneManager.LoadScene(0);
                EscapeMenuOpen = false;
                // GameObject.Find("Quit").SetActive(false);
                //GameObject.Find("Restart").SetActive(false);
                //quit.gameObject.SetActive(false);
               // restart.gameObject.SetActive(false);
            }
        }
    }

    public void resizeUI()
    {

        float height = (Screen.height / 4) * 3;
        float width = Screen.width/ 6;

        attackSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        hideSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        threatSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        attackSlider2.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        threatSlider2.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        hideSlider2.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        quit.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(width*1.5));
        start.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(width * 1.5));
        float uiScale = Screen.width / uiBaseWidth;
        int buttonFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        int sliderFontSize = Mathf.RoundToInt((float)((baseFontSize-1) * uiScale*1.5));
        if (Screen.width < 1000)
            sliderFontSize = 14;
        if (Screen.width < 1500 && Screen.width > 1000)
            sliderFontSize = 16;
        if (Screen.width < 2000 && Screen.width > 1500)
            sliderFontSize = 18;
            quitText.fontSize = sliderFontSize;
        startText.fontSize = sliderFontSize;
        attackText.fontSize = sliderFontSize;
        attackText2.fontSize = sliderFontSize; 
        hideText.fontSize = sliderFontSize; 
        hideText2.fontSize = sliderFontSize; 
        threatText.fontSize = sliderFontSize; 
        threatText2.fontSize = sliderFontSize;
        GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().fontSize = buttonFontSize * 2;
}

    public void CloseEscapeMenuOpen()
    {
        EscapeMenuOpen = false;
        quit.gameObject.SetActive(false);
        attackSlider.gameObject.SetActive(false);
        threatSlider.gameObject.SetActive(false);
        hideSlider.gameObject.SetActive(false);
        attackSlider2.gameObject.SetActive(false);
        threatSlider2.gameObject.SetActive(false);
        hideSlider2.gameObject.SetActive(false);
        secondAiToggle.gameObject.SetActive(false);
        firstAiToggle.gameObject.SetActive(false);
        //DDX.gameObject.SetActive(false);
        setSliderValues();
    }

    public void DisableFirstAIOptions()
    {
        if (firstAiToggle.isOn == false)//(attackSlider.IsActive() == true)
        {
            attackSlider.gameObject.SetActive(false);
            threatSlider.gameObject.SetActive(false);
            hideSlider.gameObject.SetActive(false);
            secondAiToggle.isOn = false;
            attackSlider2.gameObject.SetActive(false);
            threatSlider2.gameObject.SetActive(false);
            hideSlider2.gameObject.SetActive(false);
            secondAiToggle.gameObject.SetActive(false);


        }
        else if (firstAiToggle.isOn)//(attackSlider.IsActive() == false)
        {
            attackSlider.gameObject.SetActive(true);
            threatSlider.gameObject.SetActive(true);
            hideSlider.gameObject.SetActive(true);

            secondAiToggle.gameObject.SetActive(true);
          //  secondAiToggle.isOn = true;

        }

    }

    public void DisableSecondAIOptions()
    {
        if (secondAiToggle.isOn == false)//attackSlider2.IsActive() == true)
        {
            attackSlider2.gameObject.SetActive(false);
            threatSlider2.gameObject.SetActive(false);
            hideSlider2.gameObject.SetActive(false);
        }
        else if (secondAiToggle.isOn)//attackSlider2.IsActive() == false)
        {
            attackSlider2.gameObject.SetActive(true);
            threatSlider2.gameObject.SetActive(true);
            hideSlider2.gameObject.SetActive(true);
        }
    }

    public void OpenMenu()
    {
       
            if (EscapeMenuOpen == false)
            {
                //SceneManager.LoadScene(1);
                EscapeMenuOpen = true;
                // GameObject.Find("escape").SetActive(true);
                // GameObject.Find("restart").SetActive(true);

                quit.gameObject.SetActive(true);
                //restart.gameObject.SetActive(true);
                setSliderValues();
            //attackSlider.gameObject.SetActive(true);
            //hideSlider.gameObject.SetActive(true);
           // threatSlider.gameObject.SetActive(true);
            firstAiToggle.gameObject.SetActive(true);
            secondAiToggle.gameObject.SetActive(true);
            if (secondAiToggle.isOn)
            {
                threatSlider2.gameObject.SetActive(true);
                hideSlider2.gameObject.SetActive(true);
                attackSlider2.gameObject.SetActive(true);
            }
            if (firstAiToggle.isOn)
            {
                threatSlider.gameObject.SetActive(true);
                hideSlider.gameObject.SetActive(true);
                attackSlider.gameObject.SetActive(true);
            }

        }
        else
            {
                //SceneManager.LoadScene(0);
                EscapeMenuOpen = false;
                // GameObject.Find("Quit").SetActive(false);
                //GameObject.Find("Restart").SetActive(false);
                quit.gameObject.SetActive(false);
              //  start.gameObject.SetActive(false);
            attackSlider.gameObject.SetActive(false);
            attackSlider2.gameObject.SetActive(false);
            hideSlider.gameObject.SetActive(false);
            hideSlider2.gameObject.SetActive(false);
            threatSlider.gameObject.SetActive(false);
            threatSlider2.gameObject.SetActive(false);
            firstAiToggle.gameObject.SetActive(false);
            secondAiToggle.gameObject.SetActive(false);
        }

    }

    private void setSliderValues()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        referenceScript = referenceObject.GetComponent<GameManager>();
        attackSlider.value = referenceScript.getPoints("attack");
        threatSlider.value = referenceScript.getPoints("threat");
        hideSlider.value = referenceScript.getPoints("hide");
    }

    public void Escape()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}