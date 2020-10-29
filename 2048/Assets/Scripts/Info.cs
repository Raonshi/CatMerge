using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public GameObject Yes;
    public GameObject No;

    private void OnEnable()
    {

    }

    public void OnClickYes()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }
}
