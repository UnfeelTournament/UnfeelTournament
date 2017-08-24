using UnityEngine;
using System.Collections;

//Script for controlling character health, lives, taking damage, and healing
//Character Managers or Game Manager should take care of respawning, spawning, checking for game over, and handling UI for health.
//Upon game over for the character, the manager should disable the character
//Upon death for the character, the manager should disable the character's action scripts until the character respawns

public class CharacterHealth : MonoBehaviour {
    //Health values
    public int _defHealth = Constants.DEF_START_HEALTH;
    public int _defLives = Constants.DEF_START_LIVES;
    public float _defProtectTimer = Constants.PROTECT_TIMER;
    public float _defDeathTimer = Constants.DEATH_TIMER;

    //Health internal values
    public int _health = Constants.DEF_START_HEALTH;
    public int _lives = Constants.DEF_START_LIVES;
    public float _protectTimer = 0;
    public float _deathTimer = 0;

    //Health flags
    [HideInInspector] public bool _isDead = false;
    [HideInInspector] public bool _isOut = false; //If the player is out of lives
   
    //Health Audio
    public AudioSource _healthAudio;
    public AudioClip _takeDamageAudioClip;
    public AudioClip _dieAudioClip;
    public AudioClip _outAudioClip;
    //The below Audio Clips: Might be better to move to some manager that controls on pickup
    public AudioClip _restoreHealthAudioClip;
    public AudioClip _restoreLifeAudioClip;

    //Any Particles or Effects for taking damage, healing, and dying here

    private Animator _anim;

    void Awake()
    {
        //Setup anim here
        _anim = GetComponent<Animator>();
    }

	void Start () {
	    
	}

    //When game over
    void OnDisable()
    {

    }
	
	void Update () {

        //Death Timer + Respawn
        if (isDead())
        {
            _deathTimer -= Time.deltaTime;
            if (canRespawn())
            {
                respawn();
            }
        }
        //Protection Timer
        if (isProtected())
        {
            _protectTimer -= Time.deltaTime;
        }

        //debugTakeDamage();
	}

    public bool isDead()
    {
        return _health <= 0;
    }

    public bool isOut()
    {
        return _lives <= 0;
    }

    public bool isProtected()
    {
        return _protectTimer > 0;
    }

    public bool canRespawn()
    {
        return _lives > 0 && _deathTimer <= 0;
    }

    public void respawn()
    {
        _health = _defHealth;
        _protectTimer = _defProtectTimer;
        _isDead = false;
    }

    //This should be the only way to take damage; thus, there should be no reason to check for death outside of this function. 
    //If the character falls, just do 9999 damage to it
    public void takeDamage(int dmg)
    {
        //If character has Spawn Protection, no damage, no effects
        if (isProtected()) return;

        _anim.SetTrigger("CharacterHit");

        //dmg sfx

        _health -= dmg;

        if(isDead())
        {
            die();
        }
    }

    public void restoreHealth(int health)
    {
        if (isDead() || isOut()) return;

        //Heal particles and sound


        _health += health;
    }

    public void restoreLife(int life)
    {
        if (isOut()) return;
        _lives += life;
    }

    public void die()
    {
        _lives--;
        _deathTimer = _defDeathTimer;
        _isDead = true;
        //Any death particles and sfx and sound here

        //If the character is out of lives
        if(isOut())
        {
            over();
        }
    }

    //Game over for character
    public void over()
    {
        _isOut = true;
        //Any game over particles, sfx, and sound here
    }

    public void debugTakeDamage()
    {
        if(Input.GetButtonDown("Fire1"))
            takeDamage(10);

    }
}
