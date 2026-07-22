// Name: TitleScreenManager.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the function of the game's title screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    #region Public and Serialized Variables
    [Header("UI Screens")]
    [SerializeField] GameObject titleUI;        // Reference to the screen to play and quit the game
    [SerializeField] GameObject instructionsUI; // Reference to the screen which shows how to play the game
    [SerializeField] GameObject creditsUI;      // Reference to the screen which shows the credits

    [Header("Audio")]
    [SerializeField] AudioClip menuSelect;  // Sound that plays when a menu option is clicked

    // Hidden from inspector
    [HideInInspector] public enum UIScreens { TITLE, INSTRUCTIONS, CREDITS, NONE }; // Enum types for each type of screen
    [HideInInspector] public UIScreens currentScreen;                               // Reference to the currently active screen
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().clip = menuSelect;  // Load the audio clip for clicking a menu option
        UISwitch(UIScreens.TITLE);                      // Start on the title screen
    }

    // Play Game Button
    public void PlayGame()
    {
        GetComponent<AudioSource>().Play(); // Play the button click sound effect
        UISwitch(UIScreens.INSTRUCTIONS);   // Switch to the instructions screen
    }

    // Begin Game Button
    public void BeginGame()
    {
        GetComponent<AudioSource>().Play();     // Play the button click sound effect
        SceneManager.LoadScene("SceneName");    // Load the first level of the game (Replace "SceneName" with the name of the scene to load)
    }

    // Credits Button
    public void Credits()
    {
        GetComponent<AudioSource>().Play(); // Play the button click sound effect
        UISwitch(UIScreens.CREDITS);        // Switch to the credits screen
    }

    // Menu Button
    public void Menu()
    {
        GetComponent<AudioSource>().Play(); // Play the button click sound effect
        UISwitch(UIScreens.TITLE);          // Switch to the title screen
    }

    // Quit Button
    public void QuitGame()
    {
        GetComponent<AudioSource>().Play(); // Play the button click sound effect
        Application.Quit();                 // Quit the game
    }

    // Function for enabling and disabling UI screens
    public void UISwitch(UIScreens screen)
    {
        // If the title screen is selected...
        if (screen == UIScreens.TITLE)
        {
            // Activate selected screen
            titleUI.SetActive(true);

            // Disable all other screens in the scene
            instructionsUI.SetActive(false);
            creditsUI.SetActive(false);

            // Set currentScreen to the selected screen
            currentScreen = UIScreens.TITLE;
        }

        // If the instructions screen is selected...
        else if (screen == UIScreens.INSTRUCTIONS)
        {
            // Activate selected screen
            instructionsUI.SetActive(true);

            // Disable all other screens in the scene
            titleUI.SetActive(false);
            creditsUI.SetActive(false);

            // Set currentScreen to the selected screen
            currentScreen = UIScreens.INSTRUCTIONS;
        }

        // If the credits screen is selected...
        else if (screen == UIScreens.CREDITS)
        {
            // Activate selected screen
            creditsUI.SetActive(true);

            titleUI.SetActive(false);
            instructionsUI.SetActive(false);

            // Set currentScreen to the selected screen
            currentScreen = UIScreens.CREDITS;
        }
    }
    #endregion
}