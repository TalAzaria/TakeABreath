using System;
using System.Collections.Generic;
using System.Linq;
using Dan.Main;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    public bool DebugModeEnabled = true;
    public bool TestsEnabled = true;
    private bool areAllTestsPassed = true;

    [SerializeField] private int localScore;

    [SerializeField] private TMP_Text[] _entryTextObjects;
    [SerializeField] private TMP_Text[] _entryScoreObjects;

    [SerializeField] private TMP_InputField _usernameInputField;
    List<string> playerNames = new List<string>();

    [SerializeField] private TMP_Text alreadyExistsText;
    [SerializeField] private TMP_Text loadingText;

    private class Score
    {
        public string name;
        public int value;

        public Score(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return "name: " + name + ", value: " + value;
        }
    }

    private List<Score> scoresList = new List<Score>();
    private static GameObject scoresInstance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (scoresInstance == null)
        {
            scoresInstance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        if(alreadyExistsText != null)
        alreadyExistsText.enabled = false;

        Leaderboards.TakeABreath.ResetPlayer();

        LeaderboardCreator.LoggingEnabled = false;
        LoadEntries();

        if (TestsEnabled)
        {
            if (DebugModeEnabled)
                //print("Add one value");
            AddScore("a", 1);
            DisplayList();
            ValidateTest(new List<int> { 1 });
            scoresList.Clear();
            if (DebugModeEnabled)
                //print("Add two sorted value asc");
            AddScore("a", 1);
            AddScore("a", 2);
            DisplayList();
            ValidateTest(new List<int> { 1, 2 });
            scoresList.Clear();
            if (DebugModeEnabled)
              //  print("Add two sorted value dec");
            AddScore("a", 2);
            AddScore("a", 1);
            DisplayList();
            ValidateTest(new List<int> { 1, 2 });
            scoresList.Clear();
            if (DebugModeEnabled)
              //  print("Add three unsorted value");
            AddScore("a", 3);
            AddScore("a", 1);
            AddScore("a", 2);
            DisplayList();
            ValidateTest(new List<int> { 1, 2, 3 });
            scoresList.Clear();

            if (areAllTestsPassed)
                print("All tests passed!");
        }
    }

    void OnPlayerResetSuccess(bool success)
    {
        Debug.Log(success ? "Player entry was successfully reset." : "Failed to reset player entry.");
    }

    void OnPlayerResetError(string errorMessage)
    {
        Debug.LogError("Error resetting player entry: " + errorMessage);
    }
    public void AddScore(string name, int value)
    {
        Score score = new Score(name, value);
       // if (DebugModeEnabled)
            //Debug.Log("Add score: " + score.ToString());

        scoresList.Add(score);
        SortScoresList();
    }

    public void UpdateScore(string name, int value)
    {
        if (DebugModeEnabled)
            Debug.Log("Remove old score");

        scoresList.RemoveAll(x => x.name == name);
        AddScore(name, value);
    }

    private void SortScoresList()
    {
        scoresList = scoresList.OrderBy(x => x.value).ToList();
    }

    private void DisplayList()
    {
        if (DebugModeEnabled)
        {
            //Debug.Log("Display list");
            foreach (Score score in scoresList)
            {
            }
        }
    }

    private void ValidateTest(List<int> expected)
    {
        if (expected.Count == scoresList.Count)
        {
            for (int i = 0; i < expected.Count; i++)
            {
                if (scoresList[i].value != expected[i])
                {
                    areAllTestsPassed = false;
                   // Debug.Log("Test failed! expected " + expected[i] + ", got " + scoresList[i].value);
                    return;
                }
            }

           // if (DebugModeEnabled)
             //   Debug.Log("Test Passed!");
        }
        else
        {
           // Debug.Log("Test failed! the expected list and the list are not the same size");
            areAllTestsPassed = false;
        }
    }

    private void LoadEntries()
    {
       // loadingText.enabled = true;

        LeaderboardCreator.LoggingEnabled = false;

        Leaderboards.TakeABreath.GetEntries(entries =>
        {
            foreach (var t in _entryTextObjects)
                t.text = "";

            var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
            for (int i = 0; i < length; i++)
            {
                _entryTextObjects[i].text = $"{entries[i].Username}";
                _entryScoreObjects[i].text = $"{entries[i].Score}";

                playerNames.Add(entries[i].Username);
            }
        });
    }

    public void UploadEntry()
    {
        if (!playerNames.Contains(_usernameInputField.text))
        {
            alreadyExistsText.enabled = false;

            Leaderboards.TakeABreath.UploadNewEntry(_usernameInputField.text, localScore, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }
        else
        {
            Debug.Log("username already exist");
            alreadyExistsText.enabled = true;
        }

    }
}
