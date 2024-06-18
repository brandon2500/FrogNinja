using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverGroup;
    public Text rankText;

    public Text scoreText;
    public Image fadeImage;

    private Blade blade;
    private Spawner spawner;

    private int score;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
    }

    public void Start()
    {
        blade.enabled = false;
    }

    public void StartGame()
    {
        NewGame();
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

    public async void ToggleGameOverVisibility()
    {
        gameOverGroup.SetActive(true);
        int rank = await BlockchainManagerScript.Instance.GetRank();
        rankText.text = $"GLOBAL RANK: {rank}";
    }

    public async void SubmitScore()
    {
        rankText.text = $"GLOBAL RANK: ...";
        await BlockchainManagerScript.Instance.SubmitScore(score);
        int rank = await BlockchainManagerScript.Instance.GetRank();
        rankText.text = $"GLOBAL RANK: {rank}";
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



}
