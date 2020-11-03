using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    int you, best, point, totalPoint, lifeTime;
    public Text yourScore;
    public GameObject bestScore;
    public Text pointText;
    public Text lifeTimeText;

    private void OnEnable()
    {
        you = Game.instance.score;
        point = Game.instance.point;
        lifeTime = Convert.ToInt32(Game.instance.lifeTime);

        Game.instance.isOver = true;

        bestScore.SetActive(false);

        yourScore.text = you.ToString();
        pointText.text = point.ToString();
        lifeTimeText.text = lifeTime.ToString();

        if (PlayerPrefs.HasKey("bestScore"))
        {
            best = PlayerPrefs.GetInt("bestScore");

            if(best < you)
            {
                PlayerPrefs.SetInt("bestScore", you);
                bestScore.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("bestScore", you);
            bestScore.SetActive(true);
        }

        if(PlayerPrefs.HasKey("point"))
        {
            totalPoint = PlayerPrefs.GetInt("point") + point;
            PlayerPrefs.SetInt("point", totalPoint);
        }
        else
        {
            totalPoint = point;
            PlayerPrefs.SetInt("point", totalPoint);
        }
    }

    public void OnClickOK()
    {
        Debug.Log("=====================Game Over===================== \n Your Score : " + Game.instance.score);
        SceneManager.LoadScene("MainMenu");
    }
}
