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
    public Button rulesBtn;

    public Text quitText;
    public Text startText;
    public Text restartText;
    public Text rulesText;
    public Text attackText;
    public Text attackText2;
    public Text hideText;
    public Text hideText2;
    public Text threatText;
    public Text threatText2;
    public Text diffText;
    public TextMeshPro gameOverMessage;
    public Dropdown dddiff;
    public Image rules;

    public bool EscapeMenuOpen;
    public Slider attackSlider;
    public Slider threatSlider;
    public Slider hideSlider;
    public Slider attackSlider2;
    public Slider threatSlider2;
    public Slider hideSlider2;
    public Toggle firstAiToggle;
    public Toggle secondAiToggle;
    public Toggle noAiToggle;
    public Dropdown DDX;
    public Dropdown DDY;

    public float uiBaseWidth;
    public float uiBaseHeight;
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

        quit.gameObject.SetActive(true);
        initialVectorBottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        initialVectorTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        uiBaseWidth = Screen.width;
        uiBaseHeight = Screen.height;
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
                EscapeMenuOpen = true;

               if (firstAiToggle) 
                setSliderValues();
            }
            else
            {
                EscapeMenuOpen = false;
            }
        }
    }

    public void openRules()
    {
        if (rules.gameObject.activeSelf)
        {
            rules.gameObject.SetActive(false);
        }
        else
        {
            rules.gameObject.SetActive(true);
        }
    }

    public void resizeUI()
    {

        float height = Screen.height / 10;
        float width = Screen.width/ 6;
        float uiScale = Screen.width / uiBaseWidth;
        int buttonFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        int sliderFontSize = Mathf.RoundToInt((float)((baseFontSize-1) * uiScale*1.5));
        if (Screen.width < 1000)
            sliderFontSize = 14;
        if (Screen.width < 1500 && Screen.width > 1000)
            sliderFontSize = 16;
        if (Screen.width < 2000 && Screen.width > 1500)
            sliderFontSize = 18;

        float changedRatioWidth = Screen.width / uiBaseWidth;
        float changedRatioHeight = Screen.height / uiBaseHeight;
        buttonFontSize = (int)(baseFontSize * changedRatioWidth);

        float ruleHeight = Screen.height - Screen.height / 25;
        GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().fontSize = buttonFontSize * 2;
}

    public void CloseEscapeMenuOpen()
    {
        EscapeMenuOpen = false;

        secondAiToggle.gameObject.SetActive(false);
        firstAiToggle.gameObject.SetActive(false);
        noAiToggle.gameObject.SetActive(false);

    }

    public void DisableFirstAIOptions()
    {
        if (noAiToggle.isOn)
        {
            secondAiToggle.isOn = false;
            firstAiToggle.isOn = false;

        }
        if (firstAiToggle.isOn == false)
        {
            secondAiToggle.isOn = false;
            noAiToggle.isOn = false;
        }
        else if (secondAiToggle.isOn)
        {
            firstAiToggle.isOn = false;
            noAiToggle.isOn = false;
        }

    }
    public void DisableFirstAndSecondAIOptions()
    {
        if (noAiToggle.isOn)
        {
            secondAiToggle.isOn = false;
            firstAiToggle.isOn = false;
        }

    }

    public void DisablePlayerAndSecondAIOptions()
    {
        if (firstAiToggle.isOn == true)
        {
            secondAiToggle.isOn = false;
            noAiToggle.isOn = false;
        }
    }

    public void DisablePlayerAndFirstAIOptions()
    {
        if (secondAiToggle.isOn == true)
        {
            firstAiToggle.isOn = false;
            noAiToggle.isOn = false;
        }
    }
    public void DisableSecondAIOptions()
    {
        if (secondAiToggle.isOn == false)
        {
            dddiff.gameObject.SetActive(true);

        }
        else if (secondAiToggle.isOn)
        {
            dddiff.gameObject.SetActive(false);
        }
    }

    public void OpenMenu()
    {
       
            if (EscapeMenuOpen == false)
            {
                EscapeMenuOpen = true;
            firstAiToggle.gameObject.SetActive(true);
            secondAiToggle.gameObject.SetActive(true);
            if (secondAiToggle.isOn)
            {

            }
            if (firstAiToggle.isOn)
            {

            }

        }
        else
            {
            EscapeMenuOpen = false;
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