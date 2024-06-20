using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverGroup;

    public Text scoreText;
    public Image fadeImage;

    private Blade blade;
    private Spawner spawner;

    private int score;

    public Text croakPoolText;
    public Text depositingText;
    public Text submitScoreText;

    public static GameManager Instance { get; private set; } 

    public UnityEvent<string, int> submitScoreEvent;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }
    }

    public void Start()
    {
        blade.enabled = false;
    }

    public void StartGame()
    {
        NewGame();
        depositingText.text = $"";
    }

    private void NewGame()
    {
        Time.timeScale = 1f;

        score = 0;
        scoreText.text = score.ToString();

        blade.enabled = true;
        spawner.enabled = true;

        ClearScene();

    }

    public void ToggleGameOverVisibility()
    {
        gameOverGroup.SetActive(true);
    }

    public async void SubmitScore()
    {
        string Address = await BlockchainManagerScript.Instance.GetAddress();

        submitScoreEvent.Invoke(Address, score);
    }

    private void ClearScene()
    {
        Bugs[] bugs = FindObjectsOfType<Bugs>();

        foreach (Bugs bug in bugs)
        {
            Destroy(bug.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score+= amount;
        scoreText.text = score.ToString();
    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.05f;

        while (elapsed < duration)
        {

            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);


        elapsed = 0f;

        while (elapsed < duration)
        {

            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            Time.timeScale = 1f;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        ToggleGameOverVisibility();
    }

    public async void LoadPoolBalance()
    {
        croakPoolText.text = $"Updating";
        BigInteger pool = await BlockchainManagerScript.Instance.GetPrizePoolBalance();
        croakPoolText.text = $"{pool}";
    }

    public async void DepositCroak()
    {
      await BlockchainManagerScript.Instance.PayToPlay();

    }

    public void ChangeText()
    {
        depositingText.text = $"DEPOSITING...";
    }

    public void SubmittingScoreText()
    {
        submitScoreText.text = $"Submitting Score...";
    }

    public void ScoreSubmittedText()
    {
        submitScoreText.text = $"Score Submitted!";
    }
}
