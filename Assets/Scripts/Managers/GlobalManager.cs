using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public struct CharacterData
{
    public int _livesRemaining;
    public int _kills;
    public int _deaths;
    public int _damageTaken;
    public int _damageDealt;
}

public class GlobalManager : MonoBehaviour {
    public static GlobalManager _instance;

    /* Instances (Need to be Reset) */
    public string _currentScene = "MainMenu";   //Scene that is currently active
    public GameObject _hud;                     //The HUD for the Game scenes
    public GameObject _camera;
    //Transition Flags
    public bool _mainToGame = false;            //Transition from Main Menu to the Game (Play from Menu)
    public bool _gameToPause = false;           //Transition from the Game to Pause (Press Pause Button)
    public bool _gameToResult = false;          //Transition from the Game to Result  (Game Ends via Game Over)
    public bool _pauseToGame = false;           //Transition from Pause to the Game (Press Unpause Button)
    public bool _pauseToResult = false;         //Transition from Pause to Result (In Pause, Press Quit)
    public bool _resultToMain = false;          //Transition from Result to Main (In Result, Press Quit)
    public bool _resultToPost = false;
    public bool _postToMain = false;
    //Game Settings
    public int _numOfPlayers = 0;               //Number of Players in Game (2 - 4)
    public int _numOfLives = 0;                 //Number of Lives for each Player (1 - 10)
    public float _gameTime = 0f;                //The Max Time for the Game (0 - 300 s/5 m)
    public int _gameStage;                      //Selected Level (0 - 3) //Index in the List
    //Log Info
    public CharacterData[] _characterLogs;      //Character Logs for each Player //Used for Result scene
    public float _timeElapsed;                  //Time the game took
    public int _winner;                         //The winner of the game //Index in the List or -1 for nobody won
    //Misc. Values
    public int[] _playerTypes;
    public GameObject[] _players;                //Players in the game
    public bool[] _isActive;                    //If a Player is Active (has Lives left)
    public List<GameObject> _activeItemList;    //Items that are spawned in the level, and not picked up
    public float _spawnTimer = 0;               //The current timer for when to next spawn an item
    public float _currentTime = 0;
    public bool _useTime = false;
    public bool _loadGame = false;
    public bool _paused = false;
    //public Vector2[] _originalVel;
    //public float _originalTime = Time.timeScale;


    /* References (Should not be Reset) */
    //Scene Names
    public string _mainScene = "MainMenu";      //Main Menu Scene
    public string _pauseScene = "PauseMenu";    //Pause Menu Scene
    public string _resultScene = "ResultMenu";  //Result Menu Scene
    public string _gameScene = "Game";          //Just indicates that the Scene is one of the Levels
    public string _postScene = "LastWinScreen"; //Change later
    //Lists
    public string[] _levelList = { "main", "main" , "main" , "main" };                 //List of all Levels
    public AudioClip[] _levelAudio;
    public GameObject[] _itemList;              //List of all Items
    public GameObject[] _characterList;
    public Image[] _characterImageList;
    public Image[] _characterDeadList;
    public GameObject _hudPrefab;
    public GameObject _cameraPrefab;
    //Values
    public float _timeBetweenSpawns;            //Time in between each item spawn
    //Input Axis            
    public string _pauseAxis;                   //Input for Pausing in Game
    public string _quitAxis;                    //Input for Quitting to Main Menu from Result or for Going back to Game from Pause

    public AudioSource _backGroundMusic;
    public AudioSource _menuSounds;
    public AudioClip _menuMusic;
    public AudioClip _resultMusic;
    public AudioClip _gameMusic;
    public AudioClip _selectSound;
    public AudioClip _backSound;
    public AudioClip _pauseSound;
    public AudioClip _unpauseSound;

    void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _backGroundMusic.clip = _menuMusic;
        _backGroundMusic.Play();
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    /* The Main Manager Loop will perform various things depending on the current scene */
    void Update()
    {
        // Core Manager Loop
        if(_currentScene.CompareTo(_mainScene) == 0) //In the Main Menu
        {
            //Check for input moving between buttons
            //-Add sound to the Main Menu script for on click button

            //Check for play button click to set mainToGame
            if (_loadGame)
            {
                initializeGame();
            }
            if (_mainToGame)
            {
                startGame();
            }
        }
        else if(_currentScene.CompareTo(_pauseScene) == 0) //In the Pause Menu
        {
            if (Input.GetButtonDown(_quitAxis))
            {
                _pauseToGame = true;
            }

            if (_pauseToGame)
            {
                unpause();
            }
            else if (_pauseToResult)
            {
                endGame();
                startResult();
            }
        }
        else if(_currentScene.CompareTo(_resultScene) == 0) //In the Result Screen
        {
            if (Input.GetButtonDown(_quitAxis))
            {
               // _resultToMain = true;
                _resultToPost = true;
            }
            /*
            if (_resultToMain) //Change later
            {
                endResult();
                beginMain();
            }*/
            
            if (_resultToPost)
            {
                endResult();
                startPost();
            }
            
        }
        else if(_currentScene.CompareTo(_postScene) == 0)
        {   
            if (Input.GetButtonDown(_quitAxis))
            {
                _postToMain = true;
            }
            if (_postToMain)
            {
                endPost();
                beginMain();
            }
            
        }
        else if(_currentScene.CompareTo(_gameScene) == 0) //In the Game
        {
            //Debug.Log("Begin update game scene");
            if (Input.GetButtonDown(_pauseAxis)) //If pause is pressed
            {
                //Debug.Log("Pause pressed");
                //Debug.Log("Pause: " + _paused);
                if (!_paused) _gameToPause = true; //If not paused, then pause
                else _pauseToGame = true; //If paused, then unpause
            }
            if (Input.GetButtonDown(_quitAxis)) //If quit is pressed
            {
                _pauseToResult = true;
            }
            //Debug.Log("In game scene update");
            sweepForDeath();
            sweepForRespawn();
            sweepForDespawn();
            sweepForDeathAnimate();
            updateHUD();

            int winner = whoWon();
            //Debug.Log("winner: " + winner);
            if(winner != -1)
            {
                //Debug.Log("WINNER ANNOUNCED");
                _winner = winner;
                _timeElapsed = _gameTime - _currentTime;
                _gameToResult = true;
            }
            else if(winner == -2)
            {
                Debug.Log("Nobody won?");
            }

            if(_useTime && _currentTime <= 0)
            {
                //Debug.Log("TIME OVER");
                _timeElapsed = _gameTime;
                _gameToResult = true;
            }
            else if (_useTime && !_paused)
            {
                _currentTime -= Time.deltaTime;
            }

            if(_spawnTimer <= 0 && !_paused)
            {
                spawnItem();
                _spawnTimer = _timeBetweenSpawns;
            }
            else if(!_paused)
            {
                _spawnTimer -= Time.deltaTime;
            }

            

            if (_gameToPause) //If pausing game
            {
                pause();
            }
            if (_pauseToGame) //If unpausing game
            {
               // Debug.Log("Unpause");
                unpause();
            }
            if (_gameToResult || _pauseToResult) //If game ends by win or by quit
            {
                //Debug.Log("End game");
                _winner = whoWon();
                endGame();
                startResult();
            }
        }
        else //String Mismatch
        {
            Debug.Log("ERROR (String Mismatch): " + _currentScene);
        }
    }

    //Scene functions
    //Transition function from Main to Game; Initializes the Game settings, player spawns, etc. 
    public void startGame()
    {
        
        _mainToGame = false;

        //_backGroundMusic.clip = _gameMusic;
        _backGroundMusic.clip = _levelAudio[_gameStage];
        _backGroundMusic.Play();

        //Debug.Log(_gameStage);
        SceneManager.LoadScene(_levelList[_gameStage]);
        _loadGame = true;

        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    public void initializeGame()
    {
        _currentScene = _gameScene;
        _loadGame = false;
        _currentTime = _gameTime;
        extractSettings();
        initializeArrays();
        
        spawnCharacterInitial();
        initializeCamera();
        initializeHUD();
        
        //Debug.Log("GAME SETTINGS:: Lives: " + _numOfLives + " ;Players: " + _numOfPlayers + " ;Time: " + _gameTime + " ;Stage: " + _levelList[_gameStage]);
    }

    //Transition function from Game to Result OR Pause to Result; Cleans up anything, sets up the Logs for Result
    public void endGame()
    {
        _gameToResult = false;
        _pauseToResult = false;
        _backGroundMusic.Stop();
        Destroy(_hud);
        Destroy(_camera);

        _currentScene = _resultScene;
    }

    //Transition function from Game to Result; load the logs for Result, Display Result
    public void startResult()
    {
        //Using the values of characterLogs, timeElapsed, and winner: set the values in the Results UI
        for(int i = 0; i < _numOfPlayers; i++)
        {
            _characterLogs[i]._deaths = _numOfLives - _players[i].GetComponent<Character>()._lives;
            _characterLogs[i]._damageTaken = _players[i].GetComponent<Character>()._damageTaken;
            _characterLogs[i]._damageDealt = _players[i].GetComponent<Character>()._damageDealt;
            _characterLogs[i]._kills = _players[i].GetComponent<Character>()._kills;
        }
        _backGroundMusic.clip = _resultMusic;
        _backGroundMusic.Play();
        SceneManager.LoadScene(_resultScene);
    }

    //Transition function from Result to Main; Clean up things from Result
    public void endResult()
    {
        //Set up for Post
        //_resultToMain = false;
        //_currentScene = _mainScene;
        //resetInstances();
    }

    //Get the win story of winner and set the text
    public void startPost()
    {
        _resultToPost = false;
        //Set text of winner
        _currentScene = _postScene;
        SceneManager.LoadScene(_postScene);
    }

    //Go to main
    public void endPost()
    {
        _postToMain = false;
        _currentScene = _mainScene;
        resetInstances();
    }
    //Transition function from Game to Pause
    public void pause()
    {
        _menuSounds.clip = _pauseSound;
        _menuSounds.Play();
        //Debug.Log("Pause");
        _gameToPause = false;
        //_currentScene = _pauseScene;
        _paused = true;
        /*
        for(int i = 0; i < _numOfPlayers; i++)
        {
            
            _originalVel[i] = _players[i].GetComponent<Character>()._rb.velocity;
            _players[i].GetComponent<Character>()._rb.velocity = new Vector2(0, 0);
            _players[i].GetComponent<Character>()._rb.isKinematic = true;
        }*/
        //Debug.Log("Set paused");
        _backGroundMusic.Pause();


        //SceneManager.LoadScene(_pauseScene);
    }

    //Transition function from Pause to Game
    public void unpause()
    {
        _menuSounds.clip = _unpauseSound;
        _menuSounds.Play();
        //Debug.Log("Unpause");
        _pauseToGame = false;
        //_currentScene = _gameScene;
        _paused = false;
        /*
        for (int i = 0; i < _numOfPlayers; i++)
        {
            _players[i].GetComponent<Character>()._rb.isKinematic = false;
            _players[i].GetComponent<Character>()._rb.velocity = _originalVel[i];
        }*/
        _backGroundMusic.UnPause();

        //SceneManager.LoadScene(_levelList[_gameStage]);
    }

    //Transition function from Result to Main OR initialize Main; Initializes Main Menu, resets everything
    public void beginMain()
    {
        resetInstances();
        _backGroundMusic.Stop();
        _backGroundMusic.clip = _menuMusic;
        _backGroundMusic.Play();

        SceneManager.LoadScene(_mainScene);
    }

    //Action functions
    //Kill the character; Set it to inactive
    public void killCharacter(int c)
    {
        _players[c].GetComponent<Character>().die();
        //Debug.Log("Now has Life: " + _players[c].GetComponent<Character>()._lives);
        if (isOutOfGame(c))
        {
            //Debug.Log("out of the game");
            kickCharacter(c);
        }     
    }
    
    //Respawn the character at a random Spawn Point; Set it to active
    public void respawnCharacter(int c)
    {
        Transform[] spawnPoints = getAllSpawnPointsAsArray(_gameStage);
        int spawn = (int) getRandomNumber(0, spawnPoints.Length);
        if(spawn == spawnPoints.Length)
        {
            Debug.Log("MAX IS GREATER THAN MIN");
        }

        _players[c].transform.position = spawnPoints[spawn].position;
        setActiveCharacter(c, true);
        _players[c].GetComponent<Character>().respawn();
    }

    //Set the character flag to false
    public void kickCharacter(int c)
    {
        _isActive[c] = false;
        setActiveCharacter(c, false);
    }

    //Turn on/off its compomenents 
    public void setActiveCharacter(int c, bool active)
    {
        if (active)
        {
            _players[c].GetComponent<SpriteRenderer>().enabled = true;
            _players[c].GetComponent<BoxCollider2D>().enabled = true;
            _players[c].GetComponent<Rigidbody2D>().isKinematic = false;
            _players[c].GetComponent<Animator>().enabled = true;
            AudioSource[] audioList = _players[c].GetComponents<AudioSource>();
            for (int i = 0; i < audioList.Length; i++)
            {
                audioList[i].enabled = true;
            }
        }
        else
        {
            _players[c].GetComponent<SpriteRenderer>().enabled = false;
            _players[c].GetComponent<BoxCollider2D>().enabled = false;
            _players[c].GetComponent<Rigidbody2D>().isKinematic = true;
            _players[c].GetComponent<Animator>().enabled = false;
            AudioSource[] audioList = _players[c].GetComponents<AudioSource>();
            for (int i = 0; i < audioList.Length; i++)
            {
                audioList[i].enabled = false;
            }
        }
        //_players[c].gameObject.SetActive(active);
    }

    //Initial spawn of each Player upon game start
    public void spawnCharacterInitial()
    {
        List<Transform> spawnPoints = getAllSpawnPointsAsList(_gameStage);
        int spawn;
        //Debug.Log("NUM OF PLAYERS: " + _numOfPlayers);
        for(int i = 0; i < _numOfPlayers; i++)
        {
            spawn = (int)getRandomNumber(0, spawnPoints.Count);
            //Debug.Log("I: " + i);
            //Debug.Log("SPAWN RANDOM: " + spawn);
            //Debug.Log("SPAWN POINT COUNT: " + spawnPoints.Count);
            //Debug.Log("PLAYER COUNT: " + _players.Length);

            //the 0 in characterList is the characterType
            _players[i] = (GameObject)Instantiate(_characterList[_playerTypes[i]], spawnPoints[spawn].position, new Quaternion(0, 0, 0, 0));
            _players[i].GetComponent<Character>()._playerNumber = "" + (i + 1);
            _players[i].GetComponent<Character>().respawn();
            _players[i].GetComponent<Character>()._startLives = _numOfLives;
            _players[i].GetComponent<Character>()._lives = _numOfLives;
            _isActive[i] = true;
            setActiveCharacter(i, true);
            spawnPoints.Remove(spawnPoints[spawn]);
        }
    }

    //Spawn a random item at a random spawn point
    public void spawnItem()
    {
        List<Transform> spawnPoints = getAllSpawnPointsAsList(_gameStage);
        int spawn = 0;
        while (spawnPoints.Count > 0)
        {
            spawn = (int)getRandomNumber(0, spawnPoints.Count);
            if (spawnPoints[spawn].childCount != 0)
            {
                spawnPoints.Remove(spawnPoints[spawn]);
            }
            else break;
        }
        if (spawnPoints.Count == 0) return;

        int item = (int)getRandomNumber(0, _itemList.Length);
        GameObject newItem = (GameObject)Instantiate(_itemList[item], spawnPoints[spawn].position, new Quaternion(0, 0, 0, 0));
        newItem.transform.parent = spawnPoints[spawn].transform;
        _activeItemList.Insert(_activeItemList.Count, newItem);
    }

    //Reset instance variable values 
    public void resetInstances()
    {
        _currentScene = _mainScene;
        _hud = null;
        _camera = null;

        _mainToGame = _gameToPause = _gameToResult = _pauseToGame = _pauseToResult = _resultToMain = false;

        _numOfPlayers = _numOfLives = 0;
        _gameTime = 0f;
        _gameStage = -1;

        _characterLogs = null;
        _timeElapsed = 0;
        _winner = -1;
        _players = null;
        _isActive = null;
        _activeItemList = null;
        _spawnTimer = 0;
    }

    public void initializeCamera()
    {
        _camera = (GameObject)Instantiate(_cameraPrefab, transform); //Find camera point of level
    }

    //Create and Initialize HUD component values and the positioning and look on the scene
    public void initializeHUD()
    {
        
        _hud = (GameObject)Instantiate(_hudPrefab, transform);
        //_hud.transform.parent = _camera.transform;
        Transform playerPanel = _hud.transform.Find("Player Panel");
        Transform controller = _hud.transform.Find("HUD Controller");
        HUDControl[] controls = controller.GetComponents<HUDControl>();
        for (int i = 0; i < _numOfPlayers; i++)
        {
            playerPanel.GetChild(i).gameObject.SetActive(true);
            controls[i].enabled = true;
            controls[i].setUI();
        }

        //Set the number of "Character Components" to equal the number of players
        //Set the number of Lives in each Health Character Component to equal the number of lives
        //Set the health of each Character to 100%
        //Set the timer to equal the game timer
    }

    //Update the HUD (if needed) by checking the values in Characters
    public void updateHUD()
    {
        Transform controller = _hud.transform.Find("HUD Controller");
        HUDControl[] controls = controller.GetComponents<HUDControl>();
        for (int i = 0; i < _numOfPlayers; i++)
        {
            controls[i].setUI();
        }

        controller.GetComponent<HUDTime>().togglePause(_paused);
        //Check the values in iniailizeHUD
        //Set the values to those values
        //Readjust the canvas if necessary
    }

    //Extract settings from the Main Menu Game Setting
    public void extractSettings()
    {
        //Extract the timer, numplayers, numlives, and gamestage from Main Menu scripts
        //Load them in 
    }

    //Sweep functions (Check all of some type)
    //Check each player in the game to see if they can die. If so, kill the character
    public void sweepForDeath()
    {
        for(int i = 0; i < _numOfPlayers; i++)
        {
            if (canDie(i))
            {
                //Debug.Log(i + " can die");
                killCharacter(i);
            }
        }
    }

    //Check each player in the game to see if they can respawn. If so, respawn the character
    public void sweepForRespawn()
    {
        for(int i = 0; i < _numOfPlayers; i++)
        {
            if (canRespawn(i))
            {
                //Debug.Log(i + " can respawn");
                respawnCharacter(i);
            }
        }
    }

    public void sweepForDespawn()
    {
        for(int i = 0; i < _activeItemList.Count; i++)
        {
            if(_activeItemList[i] == null)
            {
                _activeItemList.RemoveAt(i);
            }
        }
    }

    public void sweepForDeathAnimate()
    {
        for(int i = 0; i < _numOfPlayers; i++)
        {
            if (canDie(i) && !_players[i].GetComponent<Character>()._anim.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
            {
                //Debug.Log("Setting death");
                setActiveCharacter(i, false);
            }
                
        }

    }

    //Check functions
    //Checks which player won: -2 if nobody is alive (and this has yet to be called for some reason), -1 if there is no winner yet (more than 1 player still has lives), 0 - 3 if there is a winner (the only player that has lives left)
    public int whoWon()
    {
        //IF the game time is not yet over, or if not quit has been issued, return -1
        bool forceGameOver = _currentTime <= 0 || _pauseToResult || _gameToResult;
        int actives = 0;
        int winner = -1;
        for (int i = 0; i < _numOfPlayers; i++)
        {
            //Debug.Log("I: " + i);
            if (_isActive[i])
            {
                //Debug.Log("IS ACTIVE");
                actives++;
                winner = i;
            }
        }
        if (forceGameOver) //Game is forced over
        {
            if (actives == 1) return winner; //Again, if there is one person standing, that person is the winner
            else return breakTies();
        }
        else
        {
            if (actives == 1) return winner; //Again, if there is one person standing, that person is the winner
            else if (actives == 0) return breakTies();
            else return -1;
        }
    }

    public int breakTies()
    {
        List<Character> k = new List<Character>(_numOfPlayers);

        for (int i = 0; i < _numOfPlayers; i++)
        {
            k.Add(_players[i].GetComponent<Character>());
        }

        //Find max of kill
        int max = k[0]._kills;
        List<int> who = new List<int>(_numOfPlayers);
        for (int i = 1; i < _numOfPlayers; i++)
        {
            if (k[i]._kills > max)
                max = k[i]._kills;
        }
        //Debug.Log("Highest kills: " + max);
        //Find tie
        for (int i = 0; i < _numOfPlayers; i++)
        {
            if (k[i]._kills == max)
                who.Add(i);
        }
        if (who.Count == 1) return who[0];
        //Then sort by Damage dealt

        //Find max damage
        max = k[who[0]]._damageDealt;
        for (int i = 1; i < who.Count; i++)
        {
            if (k[who[i]]._damageDealt > max)
                max = k[who[i]]._damageDealt;
        }
        //Debug.Log("Max Damage: " + max);
        List<int> who2 = new List<int>(who.Count);
        for (int i = 0; i < who.Count; i++)
        {
            if (k[who[i]]._damageDealt == max)
                who2.Add(who[i]);
        }

        if (who2.Count == 1) return who2[0];
        //Then sort by Lives left
        max = k[who2[0]]._lives;
        for (int i = 1; i < who2.Count; i++)
        {
            if (k[who2[i]]._lives > max)
                max = k[who2[i]]._lives;
        }
        //Debug.Log("Lives left: " + max);
        List<int> who3 = new List<int>(who2.Count);
        for (int i = 0; i < who2.Count; i++)
        {
            if (k[who2[i]]._lives == max)
                who3.Add(who2[i]);
        }
        if (who3.Count == 1) return who3[0];
        return -2;
    }

    //Character @ c can respawn AND is currently active
    public bool canRespawn(int c)
    {
        return _isActive[c] && _players[c].GetComponent<Character>().canRespawn() && !_paused;
    }

    //Character @ c has no more health AND is currently active
    public bool canDie(int c)
    {
        //Debug.Log("C: " + c);
        //Debug.Log("Size: " + _isActive.Length + ", " + _players.Length);
        //Debug.Log("Player Character: " + _players[c]);
        return _isActive[c] && _players[c].GetComponent<Character>().isDead() && !_players[c].GetComponent<Character>()._isDead && !_paused;
    }

    //Character @ c has no more lives AND is currently active (If c is not active, then there is no reason to do this)
    public bool isOutOfGame(int c)
    {
        return _isActive[c] && _players[c].GetComponent<Character>().isOutOfLives() && !_paused;
    }

    //Misc./Helper Functions
    //Gets an array of all spawn points in the level
    public Transform[] getAllSpawnPointsAsArray(int l)
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Spawn"); //Change based on Level functions
        Transform[] spawns = new Transform[temp.Length];
        for(int i = 0; i < temp.Length; i++)
        {
            spawns[i] = temp[i].transform;
        }
        return spawns;
    }

    //Gets a list of all spawn points in the level
    public List<Transform> getAllSpawnPointsAsList(int l)
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Spawn"); //Change based on Level functions
        List<Transform> spawns = new List<Transform>(temp.Length);
        for (int i = 0; i < temp.Length; i++)
        {
            spawns.Insert(i, temp[i].transform);
        }
        return spawns;
        /*
        Debug.Log("Level: " + _levelList[l]);
        //Transform temp = SceneManager.GetSceneByName(_levelList[l]).GetRootGameObjects()[0].transform;
        Transform temp = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform;
        List<Transform> spawns = new List<Transform>(temp.childCount);
        Debug.Log("CHILD COUNT: " + temp.childCount);
        for (int i = 0; i < temp.childCount; i++)
        {
            spawns[i] = temp.GetChild(i);
        }
        Debug.Log("TEMP LENGTH: " + temp.childCount);
        
        return spawns;
        */
    }

    //Choose a Random in Rage
    public float getRandomNumber(int min, int max)
    {
        if (max < min) return max + 1f;
        return Random.Range(min, max);
    }

    public void initializePlayerArrays()
    {
        _characterLogs = new CharacterData[_numOfPlayers];
        _players = new GameObject[_numOfPlayers];
        _playerTypes = new int[_numOfPlayers];
        _isActive = new bool[_numOfPlayers];
        //_originalVel = new Vector2[_numOfPlayers];
    }

    //Initialize the instance arrays and lists
    public void initializeArrays()
    {


        _activeItemList = new List<GameObject>(3);
    }
}
