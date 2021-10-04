using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FafaTools.Audio;

public class GameManager : Singleton<GameManager>
{
    public BoardManager boardManager;
    public GameObject victoryPanel;
    public GameObject losePanel;
    public GameObject reloadTimer;
    public bool IsGameFinished { get; set; } = false;
    public float timer = 0f;


    public void Start()
    {
        victoryPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    IEnumerator ReloadSceneCoroutine()
    {
        timer = 10f;
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            reloadTimer.GetComponent<UnityEngine.UI.Text>().text = ((int)timer).ToString();
        }
    }

    public void WinGame()
    {
        //victoryPanel.SetActive(true);
        //IsGameFinished = true;
        //StartCoroutine(ReloadSceneCoroutine());
    }

    public void LoseGame(ResourcesTypes type, bool isAtMax)
    {
        SoundManager.Instance.DoTransition(false);
        string loseText = "";
        losePanel.SetActive(true);
        IsGameFinished = true;
        switch (type)
        {
            case ResourcesTypes.POPULATION:
                if (isAtMax)
                {
                    loseText = "Too many wants to live here, and there is not enough housing to host everyone. Most people are homeless, and the rents are absurdly high.";
                }
                else
                {
                    loseText = "The city isn't attractive enough. Most of his inhabitants are going elsewhere, and soon this will be a ghost town.";
                }
                break;

            case ResourcesTypes.MONEY:
                if (isAtMax)
                {
                    loseText = "Public powers crawl under litteral piles of gold, and with great richness, comes great corruption rate. You can no longer count on firemen or officials to work for anything but themselves.";
                }
                else
                {
                    loseText = "No more funds ! With no money you can't build anything, and with no building you won't ever earn money. Kind of a vicious circle.";
                }
                break;

            case ResourcesTypes.INDUSTRY:
                if (isAtMax)
                {
                    loseText = "While you can most certainly find a job here, you won't ever breathe fresh air. The last people living here to see a bird are too old to tell a story about it.";
                }
                else
                {
                    loseText = " No industry in sight, this city can no longer be independent. For each and every material, it will need to import it, and also it doesn't have any personnality.";
                }
                break;

            case ResourcesTypes.ACTIVITY:
                if (isAtMax)
                {
                    loseText = "Bread and circuses. This town is a paradise. The kind of paradise where you stay two weeks at most, and then go back to your home.";
                }
                else
                {
                    loseText = "Commute, work, sleep, and repeat. Why would you need to laugh if it takes time over your working hours ?";
                }
                break;
        }
        losePanel.GetComponentInChildren<UnityEngine.UI.Text>().text = loseText;

        StartCoroutine(ReloadSceneCoroutine());

    }
}
