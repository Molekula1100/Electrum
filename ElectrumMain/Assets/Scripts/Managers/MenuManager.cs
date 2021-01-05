using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]GameObject optionsPanel;
    [SerializeField]GameObject menuPanel;
    [SerializeField]GameObject heroChoosingPanel;

    public static HeroClasses selectedHeroClass;

    private void Start()
    {
        menuPanel.SetActive(true);
        heroChoosingPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void OnPlayClick()
    {
        heroChoosingPanel.SetActive(true);
        menuPanel.SetActive(false);   
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnKnightClick()
    {
       selectedHeroClass = HeroClasses.Knight;
    }
    public void OnThiefClick()
    {
        selectedHeroClass = HeroClasses.Thief;
    }
    public void OnWizardClick()
    {
       selectedHeroClass = HeroClasses.Wizard;
    }

    private void OnHeroSelectedPlayClick()
    {
        SceneManager.LoadScene(1); //PlayerScene
    }

    
    public void OnOptionsClick()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
}
