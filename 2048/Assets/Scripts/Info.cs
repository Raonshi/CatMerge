using UnityEngine;
using UnityEngine.SceneManagement;


public class Info : MonoBehaviour
{
    public GameObject Yes;
    public GameObject No;

    private void OnEnable()
    {

    }

    public void OnClickYes()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
#if UNITY_ENGINE
                EditorApplication.isPlaying = false;           
#else
                Application.Quit();
#endif
                break;
            case "Game":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }
}
