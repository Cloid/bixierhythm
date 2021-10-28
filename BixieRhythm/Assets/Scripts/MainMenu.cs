using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitGame(){
        
        //Exits the game application
        Application.Quit();
        Debug.Log("The game has been closed.");
        
    }

    public void StartGame(){

        //Chanages from the Main menu to start the game
        SceneManager.LoadScene("Main Prototype");
        Debug.Log("Start has been selected.");

    }
}
