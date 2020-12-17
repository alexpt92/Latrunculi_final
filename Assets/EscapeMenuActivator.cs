using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenuActivator : MonoBehaviour
{

    public Button quit;
    public Button restart;
    public bool EscapeMenuOpen;
    public Slider attackSlider;
    public Slider threatSlider;
    public Slider hideSlider;
    public Slider attackSlider2;
    public Slider threatSlider2;
    public Slider hideSlider2;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = quit.GetComponent<RectTransform>();
        RectTransform rectTransformRestart = restart.GetComponent<RectTransform>();
        //GameObject.FindGameObjectWithTag("menuCanvas").GetComponent<RectTransform>().anchoredPosition = new Vector2((float)1,(float)1);
        rectTransform.anchoredPosition = new Vector2((float)0.5, (float)0.5);
        // rectTransformRestart.position = new Vector2((float)0.5, (float)0.25);
        quit.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeMenuOpen == false)
            {
                //SceneManager.LoadScene(1);
                EscapeMenuOpen = true;
                // GameObject.Find("escape").SetActive(true);
                // GameObject.Find("restart").SetActive(true);

                quit.gameObject.SetActive(true);
                restart.gameObject.SetActive(true);
                setSliderValues();
            }
            else
            {
                //SceneManager.LoadScene(0);
                EscapeMenuOpen = false;
                // GameObject.Find("Quit").SetActive(false);
                //GameObject.Find("Restart").SetActive(false);
                quit.gameObject.SetActive(false);
                restart.gameObject.SetActive(false);
            }
        }
    }

    public void resizeUI()
    {
       
    }

    public void CloseEscapeMenuOpen()
    {
        EscapeMenuOpen = false;
        quit.gameObject.SetActive(false);
        setSliderValues();
    }

    public void DisableSecondAIOptions()
    {
        if (attackSlider2.IsActive() == true)
        {
            attackSlider2.gameObject.SetActive(false);
            threatSlider2.gameObject.SetActive(false);
            hideSlider2.gameObject.SetActive(false);
        }
        else if (attackSlider2.IsActive() == false)
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
                restart.gameObject.SetActive(true);
                setSliderValues();
            }
            else
            {
                //SceneManager.LoadScene(0);
                EscapeMenuOpen = false;
                // GameObject.Find("Quit").SetActive(false);
                //GameObject.Find("Restart").SetActive(false);
                quit.gameObject.SetActive(false);
                restart.gameObject.SetActive(false);
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