using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button levelButton;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject minigameLevelsButton;
    [Header("Story Mode")]
    [SerializeField] int storyModeLevels;
    [SerializeField] int storyModeLevelsPerPage = 6;
    [SerializeField] Transform storyModeMenu;
    [SerializeField] GameObject previousSMPageButton;
    [SerializeField] GameObject nextSMPageButton;
    [Header("Minigame Mod")]
    [SerializeField] int minigameLevels;
    [SerializeField] int minigameLevelsPerPage = 8;
    [SerializeField] Transform minigameMenu;
    [SerializeField] GameObject previousMGPageButton;
    [SerializeField] GameObject nextMGPageButton;

    private int currentStoryPage;
    private int currentMinigamePage;
    private int totalStoryPages;
    private int totalMinigamePages;

    private void Start()
    {
        totalStoryPages = storyModeLevels / storyModeLevelsPerPage;
        totalMinigamePages = minigameLevels / minigameLevelsPerPage;

        if (storyModeLevels % storyModeLevelsPerPage != 0)
            totalStoryPages++;

        if (minigameLevels % minigameLevelsPerPage != 0)
            totalMinigamePages++;

        if(PlayerPrefs.GetInt("Level " + storyModeLevels) == 1)
        {
            minigameLevelsButton.SetActive(true);
        }
        else
        {
            minigameLevelsButton.SetActive(false);
        }
    }

    public void OpenStoryLevelsMenu()
    {
        if(currentStoryPage < totalStoryPages)
        {
            foreach(Transform child in storyModeMenu)
            {
                Destroy(child.gameObject);
            }

            previousSMPageButton.SetActive(currentStoryPage != 0);
            nextSMPageButton.SetActive(currentStoryPage != totalStoryPages - 1);

            int firstLevelFromCurrentPage = storyModeLevelsPerPage * currentStoryPage;
            int lastLevelOfCurrentPage = firstLevelFromCurrentPage + storyModeLevelsPerPage;

            if (lastLevelOfCurrentPage > storyModeLevels)
                lastLevelOfCurrentPage = storyModeLevels;

            for (int i = firstLevelFromCurrentPage; i < lastLevelOfCurrentPage; i++)
            {
                Button button = Instantiate(levelButton, storyModeMenu);
                int levelNumber = i + 1;
                button.GetComponentInChildren<TMP_Text>().SetText("Level " + levelNumber);
                button.onClick.AddListener(() => levelManager.StartLevel(levelNumber));
                
                if (PlayerPrefs.GetInt("Level " + i) == 1 || i == 0)
                {
                    button.interactable = true;
                }
            }
        }
    }

    public void OpenMinigameLevelsMenu()
    {
        if (currentMinigamePage < totalMinigamePages)
        {
            foreach (Transform child in minigameMenu)
            {
                Destroy(child.gameObject);
            }

            previousMGPageButton.SetActive(currentMinigamePage != 0);
            nextMGPageButton.SetActive(currentMinigamePage != totalMinigamePages - 1);

            int firstLevelFromCurrentPage = minigameLevelsPerPage * currentMinigamePage;
            int lastLevelOfCurrentPage = firstLevelFromCurrentPage + minigameLevelsPerPage;

            if (lastLevelOfCurrentPage > minigameLevels)
                lastLevelOfCurrentPage = minigameLevels;

            for (int i = firstLevelFromCurrentPage; i < lastLevelOfCurrentPage; i++)
            {
                Button button = Instantiate(levelButton, minigameMenu);
                int levelNumber = i + 1;
                button.GetComponentInChildren<TMP_Text>().SetText("Level " + levelNumber);
                button.onClick.AddListener(() => levelManager.StartLevel(levelNumber + storyModeLevels));
                button.interactable = true;
            }
        }
    }

    public void NextPage(bool isStoryMode)
    {
        if(isStoryMode && currentStoryPage + 1 < totalStoryPages)
        {
            currentStoryPage++;
            OpenStoryLevelsMenu();
        }
        else if(currentMinigamePage + 1 < totalMinigamePages)
        {
            currentMinigamePage++;
            OpenMinigameLevelsMenu();
        }
    }

    public void PreviousPage(bool isStoryMode)
    {
        if (isStoryMode && currentStoryPage - 1 >= 0)
        {
            currentStoryPage--;
            OpenStoryLevelsMenu();
        }
        else if(currentMinigamePage - 1 >= 0)
        {
            currentMinigamePage--;
            OpenMinigameLevelsMenu();
        }
    }

    public void CloseMenu(bool isStoryMode)
    {
        if (isStoryMode)
            currentStoryPage = 0;
        else
            currentMinigamePage = 0;
    }


#if UNITY_EDITOR
    [ContextMenu("ResetPlayerPrefs")]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}
