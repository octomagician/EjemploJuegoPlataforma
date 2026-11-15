using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleClass : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void botonNewGame()
    {
        PlayerPrefs.SetInt("current_progress", 0);//si no existe este archivo persistente, crealo, si sí, reinicia el progreso
        SceneManager.LoadScene(1);
    }

    public void botonContinue()
    {
        if (!PlayerPrefs.HasKey("current_progress"))
            PlayerPrefs.SetInt("current_progress", 0);
        SceneManager.LoadScene(1);
    }
}
