using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;

public class mahbuttons : MonoBehaviour
{
    public GameObject menu;
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;

    public void exTask1()
    {
        menu.SetActive(false);
        t1.SetActive(true);
    }

    public void exTask2()
    {
        menu.SetActive(false);
        t2.SetActive(true);
    }

    public void exTask3()
    {
        menu.SetActive(false);
        t3.SetActive(true);
    }


    public void GoTutorial()
    {
        SceneManager.LoadScene (sceneName:"Scenes/tutorial");
    }

    public void GoTask1()
    {
        SceneManager.LoadScene (sceneName:"Scenes/task1");
    }

    public void GoTask2()
    {
        SceneManager.LoadScene (sceneName:"Scenes/task2");   
    }

    public void GoTask3()
    {
        SceneManager.LoadScene (sceneName:"Scenes/task3"); 
    }

    public void GoFreeRoam()
    {
        SceneManager.LoadScene (sceneName:"Scenes/freeroam");       
    }
}
