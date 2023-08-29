using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelHelp;
    public GameObject panelLevels;
    public GameObject buttonMain;
    public GameObject buttonLevels;

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Main()
    {
        panelMain.SetActive(true);
        panelHelp.SetActive(false);
        panelLevels.SetActive(false);
    }

    public void HelpMain()
    {
        panelMain.SetActive(false);
        panelHelp.SetActive(true);
        panelLevels.SetActive(false);
        buttonMain.SetActive(true);
        buttonLevels.SetActive(false);
    }

    public void Levels()
    {
        panelMain.SetActive(false);
        panelHelp.SetActive(false);
        panelLevels.SetActive(true);
    }

    public void HelpLevels()
    {
        panelMain.SetActive(false);
        panelHelp.SetActive(true);
        panelLevels.SetActive(false);
        buttonMain.SetActive(false);
        buttonLevels.SetActive(true);
    }

    public void Level(int index)
    {
        SceneManager.LoadScene(index);
    }
}
