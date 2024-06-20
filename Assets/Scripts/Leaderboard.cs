using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<Text> wallets;
    [SerializeField]
    private List<Text> scores;

    private string publicKey = "5d4f5f838d45508e4c9922994833d092e0ea54fb1e8c844b408f2c6c0e68c6b4";

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            for (int i = 0; i < wallets.Count; ++i)
            {
                wallets[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboard(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

}
