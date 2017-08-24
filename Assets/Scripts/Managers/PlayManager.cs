//1: Scene Manager - Set current scene, and transition from scene to scene based on conditions
//2: Input Manager - Check for global inputs from user
//3: Gameplay Manager - Set the level, HUD, spawn players, and check if the game is over
//4: Logger - Log information for each player, and for the game for use in Results screen

/* Scene Manager */ /* (Global)
Scenes: Main, Game, Pause, Result 
Transitions: Main -> Game, Game -> Pause, Game -> Result, Pause -> Game, Pause -> Result, Result -> Main
Data: Current Scene, Flags for Transitions
References: Scenes*/

/* Input Manager */ /* (Only works for Pause Scene and Result Scene) (Does not manages Buttons (scenes do that))
Inputs: Quit, Pause
Data: Current Scene, Flags for State
References: Axis */

/* Gameplay Manager */ /* (Only works for Game)
Data: Current Level, Players, Game Settings, Spawn Points, Active Items
References: Level Spawn Points, Item List, Character List
*/

/* Logger */ /* (Only works for Game) Result uses this output
Data: Player Logging Info, Game Logging Info
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    //Global Stuff
    public string _currentScene = "MainMenu";
    public int _numOfPlayers = 0;
    public int _numOfLives = 0;
    public float _gameTime = 0f;
    public int _gameStage;
    public string[] _levelList;

    //Instance
    public bool _mainToGame = false;
    public bool _gameToPause = false;
    public bool _gameToResult = false;
    public bool _pauseToGame = false;
    public bool _pauseToResult = false;
    public bool _resultToMain = false; 

    //Reference
    public string _mainScene = "MainMenu";
    public string _pauseScene = "PauseMenu";
    public string _resultScene = "ResultMenu";
    public string _gameScene = "Game";
    
    void Update()
    {
        //Do and check certain things depending on the current scene (This will likely be true for all manager parts)
        if(_currentScene == _mainScene) //@ Main Menu
        {
            if(_mainToGame) //Players seleced Play, choose the settings, levels, and characters.
            {
                SceneManager.LoadScene(_levelList[_gameStage]);
            }
        }
        else if(_currentScene == _pauseScene) //@ Pause Menu 
        {
            if (_pauseToGame) //Player clicked on Back in Pause Menu
            {
                SceneManager.LoadScene(_levelList[_gameStage]);
            }
            else if (_pauseToResult)
            {
                SceneManager.LoadScene(_resultScene);
            }
        }
        else if(_currentScene == _resultScene) //@ Result Screen
        {
            if (_resultToMain)
            {
                SceneManager.LoadScene(_mainScene);
            }
        }
        else if(_currentScene == _gameScene) //@ Game Level
        {
            if (_gameToPause)
            {
                SceneManager.LoadScene(_pauseScene);
            }
            else if (_gameToResult)
            {
                SceneManager.LoadScene(_resultScene);
            }
        }
        else //String mismatch
        {
            Debug.Log("Bad Current Scene String: " + _currentScene);
        }
    }
}
