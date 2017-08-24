using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;







public struct GameSettings
{
    public int _numOfPlayers;
    public int _numOfLives;
    public float _time;
}

public struct CharacterResultLog
{
    public int livesLeft;
    public int damageDealt;
    public int damageTaken;
    public int kills;
}

public struct GameResultLog
{
    public int winner;
    public float totalTime;
    public GameSettings settings;
}

public class GameManager : MonoBehaviour {
    /* Core */
    public static GameManager _instance;

    /* Instance */
    public string _currentScene = "MainMenu";
    public GameObject _currentLevel;
    public GameObject _hud;
    public GameSettings _settings;
    public Character[] _players;
    public bool[] _playerIsActive;
    public List<GameObject> _activeItems;

    public bool _gameIsGoingOn = false;
    public bool _mainToGame = false;
    public bool _gameToPause = false;
    public bool _pauseToResult = false;
    public bool _gameToResult = false;
    public bool _resultToMain = false;
    public float _spawnTimer = 0f;


    /* Reference */
    public string _mainMenuScene = "MainMenu";
    public string _pauseScene = "PauseMenu";
    public string _resultScene = "ResultMenu";
    public string[] _sceneLevelList;
    public GameObject[] _levelList;
    public GameObject[] _itemList;
    public string[] _characterList = { "Character_Angry" };

    public float _timeBetweenSpawns = 5f;

    
  
	// Use this for initialization
	void Start () {
	    
	}

    void Awake()
    {
        if(_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_gameIsGoingOn) //In the game
        {
            if (Input.GetButtonDown("PauseAxis")) //Pause game
            {
                _gameToPause = true;
                _currentScene = _pauseScene;
                loadLevel(_pauseScene);
            }
            //Spawn item
            if (_spawnTimer <= 0)
            {
                spawnItem();
                _spawnTimer = _timeBetweenSpawns;
            }
            else
            {
                _spawnTimer -= Time.deltaTime;
            }

            updateHUD();

            sweepForDeath();
            sweepForRespawn();

            int winner = whoWon();
            if(winner != -1) //If the game is over: Game -> Result
            {
                //Set up CHaracter Logs
                //Set up Game Result
                _currentScene = _resultScene;
                loadLevel("ResultMenu");
            }
        }
        else if(_currentScene == _pauseScene)
        {
            //Check for unpause
            //Do pause menu buttons
        }
        else if(_currentScene == _resultScene) //In the result
        {
            //Destroy the Game info other than CharacterResultLog
            //Check for Quit Button Input: IF yes, then transition to Main Menu
            //Destroy Character Results, and the Result scene
        }
        else if(_currentScene == _mainMenuScene) //In main menu
        {
            //Destroy info from Game (ALL)
            //If wish to start game, then startgame
        }

        //Scene Transitions
        //Main to Game: Press Play -> Set Settings -> CHoose Level and Characters -> Press Play
        //Game to Pause: Press Pause
        //Pause to Result: Press Quit on Pause
        //Game to Result: Game Ends
        //Result to Main: Players choose Quit
	}

    public void loadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public int whoWon()
    {
        //Won is equal to -1, if more than one index is true in activeplayers, else is equal to the index for the one true
        int actives = 0;
        int winner = -1;
        for(int i = 0; i < _settings._numOfPlayers; i++)
        {
            if (_playerIsActive[i])
            {
                actives++;
                winner = i;
            }
        }
        if (actives == 0) return -2;//If for some reason, there are NO active players
        else if (actives != 1) return -1;
        else return winner;
    }
    public void sweepForDeath()
    {
        for(int i = 0; i < _settings._numOfPlayers; i++)
        {
            if (_players[i].isDead())
            {
                killCharacter(i);
            }
        }
    }
    public void sweepForRespawn()
    {
        for (int i = 0; i < _settings._numOfPlayers; i++)
        {
            if (_players[i].canRespawn() && _playerIsActive[i])
            {
                respawnCharacter(i);
            }
        }
    }
    public void initialSpawn()
    {
        List<Transform> spawns = new List<Transform>(0); //= _currentLevel.spawnpoints(); (something like that)
        int whichSpawn;
        for(int i = 0; i < _settings._numOfPlayers; i++)
        {
            whichSpawn = Random.Range(0, spawns.Count);
            _players[i].transform.position = spawns[whichSpawn].position;
            _players[i].respawn();
            setCharacter(i, true);
            spawns.Remove(spawns[whichSpawn]);
        }
    }

    public void respawnCharacter(int character)
    {
        Transform[] spawns = { transform }; //= _currentLevel.spawnpoints(); (something like that)
        int whichSpawn = Random.Range(0, spawns.Length);
        _players[character].transform.position = spawns[whichSpawn].position;
        _players[character].respawn();
        setCharacter(character, true);
        //Get spawn points from current level
        //Choose a spawn point
        //Set the character transform position to be at that spawn point
        //Set the character to active
    }
    public void killCharacter(int character)
    {
        _players[character].die();
        if (_players[character].isOutOfLives())
        {
            removeCharacter(character);
        }
        setCharacter(character, false);
    }
    public void removeCharacter(int character)
    {
        _playerIsActive[character] = false;
    }
    public void spawnItem()
    {
        Transform[] spawns = { transform }; //= _currentLevel.itemSpawnPoints();
        int whichSpawn = (int)Random.Range(0, spawns.Length);
        int whichItem = (int)Random.Range(0, _itemList.Length);
        GameObject newItem = (GameObject)Instantiate(_itemList[whichItem], spawns[whichSpawn].position, new Quaternion(0, 0, 0, 0));
        _activeItems.Insert(_activeItems.Count, newItem);
    }
    public void reset()
    {
        //Set all of the variables to nothing (non-reference variables)
        _currentScene = "MainMenu";
        _currentLevel = null;
        _hud = null;
        _settings._numOfLives = 0;
        _settings._numOfPlayers = 0;
        _settings._time = 0f;
        _players = null;
        _playerIsActive = null;
        _activeItems = null;
        _gameIsGoingOn = false;
        _mainToGame = false;
        _gameToPause = false;
        _pauseToResult = false;
        _gameToResult = false;
        _resultToMain = false;
        _spawnTimer = 0f;
    }
    public void initializeHUD() //Initialize HUD to settings
    {

    }
    public void updateHUD() //Updates the HUD
    {

    }
    public void pause() //From Game to Pause
    {

    }
    public void unpause() //From Pause to Game
    {

    }
    public void startGame() //From Main to Game
    {
        //Set settings
        //Set level
        //Set HUD
        initialSpawn();
    }
    public void endGame() //From Game to Result
    {
        //Transitions from Game to Resut
        //Create Result with Character Logs
        //Reset the variables
        reset();
    }
    public void quitGame() //From Pause to Result
    {

    }
    public void quitResult() //From Result to Main
    {

    }
    public void doResult() //Creates the Result
    {

    }
    public void setCharacter(int character, bool onoff)
    {
        if (onoff)
        {
            //Turn off the sprite renderer, box collider, rigidbody, animator, audio source x4
            _players[character].GetComponent<SpriteRenderer>().enabled = false;
            _players[character].GetComponent<BoxCollider2D>().enabled = false;
            _players[character].GetComponent<Rigidbody2D>().isKinematic = true;
            _players[character].GetComponent<Animator>().enabled = false;
            AudioSource[] audioList = _players[character].GetComponents<AudioSource>();
            for(int i = 0; i < audioList.Length; i++)
            {
                audioList[i].enabled = false;
            }
        }
        else
        {
            _players[character].GetComponent<SpriteRenderer>().enabled = true;
            _players[character].GetComponent<BoxCollider2D>().enabled = true;
            _players[character].GetComponent<Rigidbody2D>().isKinematic = false;
            _players[character].GetComponent<Animator>().enabled = true;
            AudioSource[] audioList = _players[character].GetComponents<AudioSource>();
            for (int i = 0; i < audioList.Length; i++)
            {
                audioList[i].enabled = true;
            }
        }
    }
}
