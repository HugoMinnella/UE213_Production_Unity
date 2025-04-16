using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text highscoreText;

    int score = 0;
    int highscore = 0;

    public void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " Stars";
        highscoreText.text = "Highscore : " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + " Stars";

        if (highscore < score)
            PlayerPrefs.SetInt("highscore", score);
    }
}
