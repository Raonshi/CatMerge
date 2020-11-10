using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    int you, point, lifeTime;

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

        if (you == GameManager.Singleton.best)
        {
            GameManager.Singleton.best = you;
            bestScore.SetActive(true);
        }

        GameManager.Singleton.totalPoint += point;
    }

    public void OnClickOK()
    {
        Debug.Log("=====================Game Over===================== \n Your Score : " + Game.instance.score);

        //현재 유저의 점수를 랭킹 점수와 비교
        GameManager.Singleton.CompareScore();


        SaveManager.Singleton.SaveUserJson();
        SceneManager.LoadScene("MainMenu");
    }
}
