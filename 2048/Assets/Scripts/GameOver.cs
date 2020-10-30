using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    int you, best, point;
    public Text yourScore;
    public Text bestScore;
    public Text pointText;

    private void OnEnable()
    {
        you = Game.instance.score;
        point = Game.instance.point;

        yourScore.text = you.ToString();
        pointText.text = point.ToString();

        if (PlayerPrefs.HasKey("bestScore"))
        {
            best = PlayerPrefs.GetInt("bestScore");

            if(best > you)
            {
                bestScore.text = best.ToString();
            }
            else
            {
                PlayerPrefs.SetInt("bestScore", you);
                best = PlayerPrefs.GetInt("bestScore");
                bestScore.text = best.ToString();
            }
        }
        else
        {
            PlayerPrefs.SetInt("bestScore", you);
            best = PlayerPrefs.GetInt("bestScore");
            bestScore.text = best.ToString();
        }

    }

    public void OnClickOK()
    {
        Debug.Log("=====================Game Over===================== \n Your Score : " + Game.instance.score);
        SceneManager.LoadScene("MainMenu");
    }
}
