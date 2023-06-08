using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelHelp;
    public GameObject panelLevelComplete;
    public GameObject levelManagerObject;
    private LevelManager levelManager;
    private bool levelComplete;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = levelManagerObject.GetComponent<LevelManager>();
        Main();
        panelLevelComplete.SetActive(false);
        levelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelComplete && levelManager.GetLevelComplete())
        {
            panelMain.SetActive(false);
            panelHelp.SetActive(false);
            panelLevelComplete.SetActive(true);
            levelManager.SetActive(false);
            levelComplete = true;
        }
    }

    public void Main()
    {
        panelMain.SetActive(true);
        panelHelp.SetActive(false);
        levelManager.SetActive(true);
    }

    public void Help()
    {
        panelMain.SetActive(false);
        panelHelp.SetActive(true);
        levelManager.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
