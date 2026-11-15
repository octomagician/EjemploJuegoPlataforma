using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //para el manejo de escenas

public class MapaScript : MonoBehaviour
{
    public GameObject flagLevel1, flagLevel2, flagLevel3, flagLevel4; //declaramos las banderitas del mapa
    int current_progress; //guarda el avance el jugador
    int level_select; 
    Animator animator; //instancia
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();//declaración con el personaje
        current_progress = PlayerPrefs.GetInt("current_progress");
        //current_progress = 0;
        switch (current_progress)
        {
            case 0: //ninguna bandera activa
                flagLevel1.SetActive(false);
                flagLevel2.SetActive(false);
                flagLevel3.SetActive(false);
                flagLevel4.SetActive(false);
                level_select = 1;
                transform.position = new Vector2(-4.2f,1);
                break;
            case 1: //primera activa
                flagLevel1.SetActive(true);
                flagLevel2.SetActive(false);
                flagLevel3.SetActive(false);
                flagLevel4.SetActive(false);
                level_select = 2;
                transform.position = new Vector2(-1.3f,1);
                break;
            case 2: //dos primeras
                flagLevel1.SetActive(true);
                flagLevel2.SetActive(true);
                flagLevel3.SetActive(false);
                flagLevel4.SetActive(false);
                level_select = 3;
                transform.position = new Vector2(1.7f,1);
                break;
            case 3: //tres primeras
                flagLevel1.SetActive(true);
                flagLevel2.SetActive(true);
                flagLevel3.SetActive(true);
                flagLevel4.SetActive(false);
                level_select = 4;
                transform.position = new Vector2(4.7f,1);
                break;
            case 4: //todas activadas
                flagLevel1.SetActive(true);
                flagLevel2.SetActive(true);
                flagLevel3.SetActive(true);
                flagLevel4.SetActive(true);
                level_select = 4;
                transform.position = new Vector2(4.7f,1);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (level_select == 1)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
                return;
            if(Input.GetKeyDown(KeyCode.RightArrow) && current_progress>0){
                level_select=2;
                transform.position = new Vector2(-1.3f,1);
                }
            if(Input.GetKeyDown(KeyCode.Return))
                animator.SetTrigger("enter"); 
        } 
        else if (level_select == 2)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                level_select = 1;
                transform.position = new Vector2(-4.2f,1);
                }
            if(Input.GetKeyDown(KeyCode.RightArrow) && current_progress>1){
                level_select = 3;
                transform.position = new Vector2(1.7f,1);
                }
            if(Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene(3);
        }
        else if (level_select == 3)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                level_select=2;
                transform.position = new Vector2(-1.3f,1);
                }
            if(Input.GetKeyDown(KeyCode.RightArrow) && current_progress>2){
                level_select = 4;
                transform.position = new Vector2(4.7f,1);
                }
            if(Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene(4);
        }
        else if (level_select == 4)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                level_select = 4;
                transform.position = new Vector2(4.7f,1);
                }
            if(Input.GetKeyDown(KeyCode.RightArrow) && current_progress>3)
                return;
            if(Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene(5);
        }
    }

    public void enterLevel()
    {
        switch (level_select)
        {
            case 1:
            SceneManager.LoadScene(2);
                break;
        }
    }
}
