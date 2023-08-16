using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button levelButton;
    [SerializeField] private int storyModeLevels;
    [SerializeField] private int minigameLevels;
    [SerializeField] private int storyModeLevelsPerPage = 6;
    [SerializeField] private int minigameLevelsPerPage = 8;
    [SerializeField] Transform storyModeMenu;
    [SerializeField] Transform minigameMenu;

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
    }

    public void OpenStoryLevelsMenu()
    {
        if(currentStoryPage < totalStoryPages)
        {
            foreach(Transform child in storyModeMenu)
            {
                Destroy(child.gameObject);
            }

            int firstLevelFromCurrentPage = storyModeLevelsPerPage * currentStoryPage;
            int lastLevelOfCurrentPage = firstLevelFromCurrentPage + storyModeLevelsPerPage;

            if (lastLevelOfCurrentPage > storyModeLevels)
                lastLevelOfCurrentPage = storyModeLevels;

            for (int i = firstLevelFromCurrentPage; i < lastLevelOfCurrentPage; i++)
            {
                Button button = Instantiate(levelButton, storyModeMenu);
                int levelNumber = i + 1;
                button.GetComponentInChildren<TMP_Text>().SetText("Level " + levelNumber);
                button.onClick.AddListener(() => LevelManager.Instance.StartLevel(i));
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

            int firstLevelFromCurrentPage = minigameLevelsPerPage * currentMinigamePage;
            int lastLevelOfCurrentPage = firstLevelFromCurrentPage + minigameLevelsPerPage;

            if (lastLevelOfCurrentPage > minigameLevels)
                lastLevelOfCurrentPage = minigameLevels;

            for (int i = firstLevelFromCurrentPage; i < lastLevelOfCurrentPage; i++)
            {
                Button button = Instantiate(levelButton, minigameMenu);
                int levelNumber = i + 1;
                button.GetComponentInChildren<TMP_Text>().SetText("Level " + levelNumber);
                button.onClick.AddListener(() => LevelManager.Instance.StartLevel(i + storyModeLevels));
                Debug.Log(i + storyModeLevels);
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
}
