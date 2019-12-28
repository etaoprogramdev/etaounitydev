using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesScript : MonoBehaviour
{
    public static PlayGamesScript Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success => {});
    }

    #region Leaderboard
    static public void AddScoreToLeaderBoard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }
    static public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion
}
