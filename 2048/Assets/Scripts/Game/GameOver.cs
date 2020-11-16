using System;
using System.Collections;
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
        SoundManager.Singleton.SoundInit();

        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Game"));
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Popup"));

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
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);

        StartCoroutine(GoToMain());
    }

    IEnumerator GoToMain()
    {
        yield return new WaitForSeconds(3.0f);

        TimeManager.Singleton.time = TimeSpan.FromSeconds(Game.instance.lifeTime);
        SaveManager.Singleton.SaveUserJson();
        SceneManager.LoadScene("MainMenu");
    }
}
