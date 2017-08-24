using UnityEngine;
using System.Collections;
using System;

/* The character class */
/* Compilation of Movement, Health, Attack, Pickup, and Manager */
/* Everything will be public (unless its a Unity function) to make it easier for now */

//Have an Audio Source for each Audio Clip, preloaded with the Audio Clip
//When an action occurs such that a clip must be played, play the audio source
public class Character : MonoBehaviour
{
    /* Core References */
    [HideInInspector]
    public Animator _anim;
    [HideInInspector]
    public Rigidbody2D _rb;
    [HideInInspector]
    public BoxCollider2D _collider;
    [HideInInspector]
    public SpriteRenderer _sr;

    /* Animation Transition Names */
    public string _moveTransition = Constants.CHAR_MOVE_TRANS_NAME;
    public string _jumpTransition = Constants.CHAR_JUMP_TRANS_NAME;
    public string _hitTransition = Constants.CHAR_HIT_TRANS_NAME;
    public string _dieTransition = Constants.CHAR_DIE_TRANS_NAME;
    public string _pickupTransition = Constants.CHAR_PICKUP_TRANS_NAME;
    public string _equipTransition = Constants.CHAR_EQUIP_TRANS_NAME;
    public string _dropTransition = Constants.CHAR_DROP_TRANS_NAME;
    public string _attackTransition = Constants.CHAR_ATTACK_TRANS_NAME;

    /* Audio Engine References */
    public AudioSource _moveAudio;
    public AudioSource _jumpAudio;
    public AudioSource _equipAudio;
    public AudioSource _takeHitAudio;
    public AudioSource _dieAudio;
    public AudioSource _spawnAudio;
    //Add any AudioSources for things to relating to above


    /* Input Axes */
    /* Movement */
    public string _moveAxis = Constants.DEF_MOVE_HORIZONTAL_AXIS;
    public string _jumpAxis = Constants.DEF_JUMP_AXIS;
    /* Pickup */
    public string _equipAxis = Constants.DEF_EQUIP_AXIS;
    /* Attack */
    public string _attackAxis = Constants.DEF_ATTACK_AXIS;
    public string _attackSecondaryAxis = Constants.DEF_ATTACK_SECONDARY_AXIS;

    /* Values */
    /* Core */
    public string _playerNumber = Constants.PLAYER_1;
    /* Movement */
    public float _moveSpeed = Constants.DEF_CHAR_MOVE_SPEED;
    public float _jumpForce = Constants.DEF_CHAR_JUMP_FORCE;
    public float _maxSpeed = Constants.DEF_CHAR_MOVE_SPEED_MAX;
    public float _StartmoveSpeed;
    public float _StartjumpForce;
    public float _StartmaxSpeed;
    /* Health */
    public int _startHealth = Constants.DEF_START_HEALTH;
    public int _startLives = Constants.DEF_START_LIVES;
    public float _protectDuration = Constants.PROTECT_TIMER;
    public float _deathDuration = Constants.DEATH_TIMER;
    public float _knockBackDuration = 0.15f;
    public int _health = Constants.DEF_START_HEALTH, _lives = Constants.DEF_START_LIVES;
    public float _protectTimer = 0, _deathTimer = 0, _knockBackTimer = 0;
    /* Pickup */
    public float _scale = Constants.DEF_SCALE;
    public float _startScale = Constants.DEF_SCALE;
    public float _scaleDuration = Constants.SCALE_TIMER;
    public float _scaleTimer = 0;

    [HideInInspector]
    public float _itemEffectDuration = 0;


    /* Check Flags */
    /* Movement */
    [HideInInspector]
    public bool _isJumping = false, _isMoving = false, _isStuck = false, _isFacingRight = false;
    /* Health */
    [HideInInspector]
    public bool _isDead = false, _isOut = false;
    /* Pickup */
    [HideInInspector]
    public bool _isPickingUp = false, _hasPickUp = false, _isScaling = false, _isReverting = false, _isWearingArmor = false;
    /* Attack */
    [HideInInspector]
    public bool _isAttacking = false, _isSecondaryAttacking = false, _isStopping = false;

    /* Misc. */
    /* Movement */
    public LayerMask _groundLayer;
    public LayerMask _playerLayer;
    public float _groundCheckDistance = Constants.MIN_GROUND_CHECK;
    public GameObject _leftCast;
    public GameObject _rightCast;
    /* Pickup */
    [HideInInspector]
    public GameObject _itemToPickUp;
    /* Attack */
    [HideInInspector]
    public GameObject _equippedWeapon;
    public Transform _hand;

    public int _damageTaken = 0;
    public int _damageDealt = 0;
    public int _kills = 0;

    public int _killer = -1;


    /* Core Methods */
    //Upon awake (before Start)
    void Awake()
    {
        //Initialise Core Components
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        _moveAxis += _playerNumber;
        _jumpAxis += _playerNumber;
        _attackAxis += _playerNumber;
        _attackSecondaryAxis += _playerNumber;
        _equipAxis += _playerNumber;
        _StartmoveSpeed = _moveSpeed;
        _StartjumpForce = _jumpForce;
        _StartmaxSpeed = _maxSpeed;

        //start without protection
        _protectTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Can only perform if the player is still able to play the game
        if (isOutOfLives()) return;

        //Check if character is dead
        if (isDead() && _deathTimer > 0)
        {
            //Debug.Log(_playerNumber + "Is dead");
            _deathTimer -= Time.deltaTime;
        }
        //Check if character has protection
        if (isProtected())
        {
            if (GetComponent<SpriteRenderer>().color.a == 1f)
            {
                Color tmp = GetComponent<SpriteRenderer>().color;
                tmp.a = 0.5f;
                GetComponent<SpriteRenderer>().color = tmp;
                //Debug.Log("Protected! :" + _protectTimer);
            }
            _protectTimer -= Time.deltaTime;
        }
        else
        {
            if (GetComponent<SpriteRenderer>().color.a == 0.5f)
            {
                Color tmp = GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                GetComponent<SpriteRenderer>().color = tmp;
            }
        }

        //scale character timer
        if (_scaleTimer > 0)
        {
            _scaleTimer -= Time.deltaTime;
        }
        else if (_isScaling)
        {
            //Debug.Log("SCALE: " + _scale);
            _isScaling = false;
            _isReverting = true;
        }

        //item effect duration timer
        if (_itemEffectDuration > 0)
        {
            _itemEffectDuration -= Time.deltaTime;
        }
        else if (_isWearingArmor)
        {
            _isWearingArmor = false;
            _isReverting = true;
        }

        if(_knockBackTimer > 0)
        {
            _knockBackTimer -= Time.deltaTime;
            //_isStopping = true;
        }
        else
        {
            _isStopping = false;
        }

        //Check inputs here
        //Debug.Log(_moveAxis);
        if ((Input.GetButton(_moveAxis) || Input.GetAxis(_moveAxis) != 0 ) && !GlobalManager._instance._paused)
        {
            //Debug.Log("Move!");
            _isMoving = canMove();
        }
        if (Input.GetButton(_jumpAxis) && !GlobalManager._instance._paused)
        {
            _isJumping = canJump();
        }
        if (Input.GetButton(_equipAxis) && !GlobalManager._instance._paused)
        {
            _isPickingUp = canPickUp();
        }
        if (Input.GetButton(_attackAxis) && !GlobalManager._instance._paused)
        {
            _isAttacking = canAttack();
        }
        //if (Input.GetButton(_attackSecondaryAxis))
        {
            //_isSecondaryAttacking = canSecondaryAttack();
        }
    }

    void FixedUpdate()
    {
        if (GlobalManager._instance._paused && isOutOfLives()) return;
        //Update character here
        if (_isMoving)
        {
            float inMove = Input.GetAxis(_moveAxis);
            //Debug.Log(inMove);
            move(inMove);
        }
        else
        {
            animateBool(_moveTransition, false);

            _moveAudio.Stop();
        }

        if (_isJumping)
        {
            jump();
        }
        else
        {
            //Nothing
        }

        if (_rb.velocity.y <= 0)
        {
            gameObject.layer = 9;
        }

        if (_isPickingUp)
        {
            if (_itemToPickUp.CompareTag("PickUp"))
                pickup(_itemToPickUp);
            else
                equip(_itemToPickUp);
        }
        else
        {

        }

        if (_isAttacking)
        {
            attack();
        }
        else
        {

        }

        if (_isSecondaryAttacking)
        {
            attackSecondary();
        }
        else
        {

        }

        //pickable item checks
        if(_isScaling)
        {
            scaleSprite(_scale);
        }
        else if (_isReverting)  //back to normal
        {
            scaleSprite(_startScale);
            _scale = _startScale;
            if(!_isWearingArmor)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    if (transform.GetChild(i).gameObject.name == "Star_Equipped(Clone)" || transform.GetChild(i).gameObject.name == "Star_Equipped")
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                }
                _maxSpeed = _StartmaxSpeed;
                _moveSpeed = _StartmoveSpeed;
                _jumpForce = _StartjumpForce;
            }
            //Debug.Log("NORMAL state!");
        }

        if (_isStopping)
        {
            //Vector2 og = new Vector2(0, 0);
            //knockBack(0, og);
        }
    }

    void OnEnable()
    {
        _rb.isKinematic = false;
    }

    void OnDisable()
    {
        _rb.isKinematic = true;
    }

    void OnDestroy()
    {
        //clean up stuff
    }
    //Character enters a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        //CHeck if other is a pick up item
        if (other.gameObject.CompareTag("PickUp") || other.gameObject.CompareTag("Weapon"))
        {
            _hasPickUp = true;
            _itemToPickUp = other.gameObject;
        }

       
        if (other.gameObject.CompareTag("Bullets") && !isDead() && !isProtected() && other.GetComponent<BulletDamage>().playerOrigin + 1 != Convert.ToInt32(_playerNumber))
        {
   
            int damage = other.GetComponent<BulletDamage>().getDamage();
            float force = (other.GetComponent<BulletDamage>().weaponOrigin.GetComponent<ItemEquipped>() ? other.GetComponent<BulletDamage>().weaponOrigin.GetComponent<ItemEquipped>().knockback : other.GetComponent<BulletDamage>().weaponOrigin.GetComponent<ItemMeele>().knockback);
            Vector3 odir = other.GetComponent<BulletMove>().origin;
            Vector3 pos = other.transform.position;
            Vector2 dir = new Vector2(pos.x - odir.x, pos.y - odir.y);
            //if(isGrounded() && dir.y < 0)
            {
                dir.y = 0;
            }
            if (odir.x == 0 && odir.y == 0) //Knock back bug
            {
                int pl = other.GetComponent<BulletDamage>().playerOrigin;
                bool fr = GlobalManager._instance._players[pl].GetComponent<Character>()._isFacingRight;
                dir.x = (fr ? 1 : -1);
            }
            //Debug.Log("Origin position: " + odir);
            //Debug.Log("Collide position: " + pos);
            //Debug.Log("DIRECITON OF KNOCKBACK: " + dir.normalized);
            //Debug.Log("Force: " + dir.normalized * force);
            
            takeDamage(damage);
            knockBack(force, dir.normalized);

            if(_health <= 0)
            {
                _killer = other.GetComponent<BulletDamage>().playerOrigin;
            }

            //added sound hit by baseball bat.
            if (other.name == "aoe(Clone)" || other.name == "aoe" && !isDead() && !isProtected())
            {
                //Debug.Log("hitted by baseballbat!");
                GameObject byBaseballBatAudio = Instantiate(Resources.Load("BaseballBatHitAudio", typeof(GameObject))) as GameObject;
                byBaseballBatAudio.GetComponent<AudioSource>().Play();
                Destroy(byBaseballBatAudio,0.3f);
            }

            //if (other.name == "Explosion(Clone)" || other.name == "Explosion")
            //{
            //    //player should only take damage from this collider once...
            //    return;
            //}
            if (other.name == "BazookaBullet(Clone)" || other.name == "BazookaBullet")
            {
                GameObject explosion = (GameObject)Instantiate(Resources.Load("Explosion", typeof(GameObject)), other.transform.position, other.transform.rotation);
                //explosion.GetComponent<BulletDamage>().weaponOrigin = other.gameObject.GetComponent<BulletDamage>().weaponOrigin;
                //explosion.GetComponent<BulletDamage>().playerOrigin = other.gameObject.GetComponent<BulletDamage>().playerOrigin;
                Destroy(explosion, 1f);

            }
            Destroy(other.gameObject);
        }

        //set itemtopickup to the other
    }
    //Character leaves a trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        //Check if other is a pick up item
        if (other.gameObject.CompareTag("PickUp") || other.gameObject.CompareTag("Weapon"))
        {
            _hasPickUp = false;
            _itemToPickUp = other.gameObject;
        }
    }
    //Character stays in a trigger collider
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp") || other.gameObject.CompareTag("Weapon"))
        {
            _hasPickUp = true;
            _itemToPickUp = other.gameObject;
        }
        if (other.gameObject.CompareTag("Bullets") && !isDead() && !isProtected() && other.GetComponent<BulletDamage>().playerOrigin  + 1 != Convert.ToInt32(_playerNumber))
        {
            int damage = other.GetComponent<BulletDamage>().getDamage();
            takeDamage(damage);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Boundary"))
        {
            _health = 0;
        }
    }

    /* Action methods */
    //Move the character
    public void move(float inMove)
    {
        if (isGrounded())
        {
            //Animate
            animateBool(_moveTransition, inMove != 0f);

            //Audio
            if(!_moveAudio.isPlaying)
                SetAudioEngine(_moveAudio);
        }
        

        if (!atMaxSpeed(inMove))    //Not at max speed, move at input speed
        {
            _rb.velocity = new Vector2(inMove * _moveSpeed, _rb.velocity.y);
        }
        if (atMaxSpeed(inMove)) // At max speed, move at max speed 
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxSpeed, _rb.velocity.y);
        }

        if (needsFlip(inMove))
        {
            flip();
        }

        _isMoving = false;

    }
    //Flips the sprite
    public void flip()
    {
        _isFacingRight = !_isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    //Apply vertical force to character
    public void jump()
    {
        animateTrigger(_jumpTransition);

        SetAudioEngine(_jumpAudio);

        _rb.AddForce(new Vector2(0f, _jumpForce * (_isScaling ? 1 +  _scale : 1)));

        _isJumping = false;

        gameObject.layer = 11;

        //_moveAudio.Stop();
    }
    //Respawn the character
    public void respawn()
    {
        _rb.isKinematic = false;
        _collider.isTrigger = false;

        SetAudioEngine(_spawnAudio);

        _health = _startHealth;

        _protectTimer = _protectDuration;

        _isDead = false;

    }

    public void knockBack(float force, Vector2 dir)
    {
        if (force == 0f) return;
        _rb.AddForce(dir * force);
        _knockBackTimer = _knockBackDuration;
        _isStopping = true;
    }
    //Character takes damage
    public void takeDamage(int dmg)
    {
        if (isProtected()) return;
        if (isDead()) return;

        animateTrigger(_hitTransition);

        SetAudioEngine(_takeHitAudio);

        if (dmg < 0)
        {
            _damageTaken += _health;
            _health = 0;
        }
        else
        {
            _health -= dmg;
            _damageTaken += dmg;
        }

    }
    //Character restores health
    public void restoreHealth(int health)
    {
        if (isDead() || isOutOfLives()) return;
        _health += health;
    }
    //Character dies
    public void die()
    {
        //Debug.Log("set death transition");
        animateTrigger(_dieTransition);

        _rb.isKinematic = true;
        _collider.isTrigger = true;
        SetAudioEngine(_dieAudio);

        if(_killer != -1)
        {
            GlobalManager._instance._players[_killer].GetComponent<Character>()._kills += 1;
            _killer = -1;
        }

        Destroy(_equippedWeapon);
        _equippedWeapon = null;
        _lives--;
        _deathTimer = _deathDuration;
        _isDead = true;     
    }
    //Character picks up item
    public void pickup(GameObject item) //Item parameter
    {
        animateTrigger(_pickupTransition);
        //Pick up effects
        //SetAudioEngine(_equipAudio);
        //Perform item effects
        //ItemEffect itemE = item.GetComponent<ItemEffect>();
        //Debug.Log("PICK UP: ");
        if (item.name == "cake(Clone)" || item.name == "cake")
        {
            if (!_isScaling)    //so doesnt effect the char by frame.
            {
                _isScaling = true;
                _scale *= item.GetComponent<ItemEffect>().scaleRate;
                _health += item.GetComponent<ItemEffect>().heal;
            }
            _scaleTimer = item.GetComponent<ItemEffect>().timeDuration;
            item.GetComponent<AudioSource>().Play();
        }
        else if (item.name == "star_idle(Clone)" || item.name == "star_idle")   //equip starArmor
        {
            //only equip the star once
            if (!transform.FindChild("Star_Equipped") && !transform.FindChild("Star_Equipped(Clone)"))
            {
                var temp_equippedArmor = (GameObject)Instantiate(item.GetComponent<ItemEffect>().itemPrefab, transform.position, transform.rotation);
                temp_equippedArmor.transform.parent = transform;
            }
            //refresh the star effect:
            _isWearingArmor = true;
            _maxSpeed = _StartmaxSpeed * item.GetComponent<ItemEffect>().moveSpeedScale;
            _moveSpeed = _StartmoveSpeed * item.GetComponent<ItemEffect>().moveSpeedScale;
            _jumpForce = _StartjumpForce * item.GetComponent<ItemEffect>().jumpForceScale;
            //_health = item.GetComponent<ItemEffect>().heal;             //restore health 
            //restoreHealth(item.GetComponent<ItemEffect>().heal);
            _itemEffectDuration = item.GetComponent<ItemEffect>().timeDuration;
            SetAudioEngine(_equipAudio);
        }

        item.GetComponent<SpriteRenderer>().enabled = false;    //hide the sprite from screen
        Destroy(item,0.5f);                                     //put delay 0.5f so, it can still play the audio clip
        _hasPickUp = false;
        _isPickingUp = false;

    }
    //Character equips item
    public void equip(GameObject item) //Weapon parameter
    {
        animateTrigger(_equipTransition);
        //Equip effects
        SetAudioEngine(_equipAudio);
        //Set equipped weapon to item
        //if item already equipped, drop that item first
        if (_equippedWeapon != null)
        {
            drop();
        }
        if (_isFacingRight)
        {
            _hand.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            _hand.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        _equippedWeapon = (GameObject)Instantiate(item.GetComponent<ItemEffect>().itemPrefab, _hand.transform.position, _hand.transform.rotation);
        int i = Convert.ToInt32(_playerNumber);
        //Debug.Log("Converted number: " + i);
        if (_equippedWeapon.GetComponent<ItemEquipped>())
            _equippedWeapon.GetComponent<ItemEquipped>().playerOrigin = i - 1;
        else if (_equippedWeapon.GetComponent<ItemMeele>())
            _equippedWeapon.GetComponent<ItemMeele>().playerOrigin = i - 1;
        if (_equippedWeapon.transform.GetChild(0).name == "Hilt")
        {
            int direction = _isFacingRight ? 1 : -1;
            _equippedWeapon.transform.localPosition = new Vector3(_equippedWeapon.transform.position.x + (direction*0.4f), _equippedWeapon.transform.position.y + 0.35f, _equippedWeapon.transform.position.z);
        }   
        _equippedWeapon.transform.parent = _hand.transform;

        Destroy(item);
        _hasPickUp = false;
        _isPickingUp = false;
    }
    //Character drops item
    public void drop()
    {
        animateTrigger(_dropTransition);
        //Drop effects
        //Set equipped weapon to empty
        GameObject temp = _equippedWeapon;
        _equippedWeapon = null;
        //Instantiate the idle version of the weapon in front of the character and have it fall to the ground

        //Destroy the current equipped weapon (temp)
        Destroy(temp);

    }
    //Character attacks (primary)
    public void attack()
    {
        //animateTrigger(_attackTransition);
        //attack effects
        //Call attack from equipped weapon
        //Debug.Log("In ATTACK");
        //Debug.Log("WEAPON NAME: " + _equippedWeapon.name);
        //if (_equippedWeapon.name.CompareTo("Gun_Ready(Clone)") == 0) _equippedWeapon.GetComponent<ItemEquipped>().Shoot(_isFacingRight);
        if (_equippedWeapon.name.CompareTo("BaseballBat_Equipped(Clone)") == 0) _equippedWeapon.GetComponent<ItemMeele>().swing(_isFacingRight);
        else _equippedWeapon.GetComponent<ItemEquipped>().Shoot(_isFacingRight);
        //Debug.Log(_equippedWeapon.gameObject.transform.rotation.y);

        _isAttacking = false;
    }
    //Character attacks (secondary)
    public void attackSecondary()
    {
        //Same as attack except with secondary attack
    }

    public void scaleSprite(float scale)
    {
        //Debug.Log("Scaling to :" + scale);
        _scale = scale;

        Vector3 sc = transform.localScale;
        sc.x = sc.y = sc.z = _scale;
        if (_isFacingRight)
            sc.x *= -1;
        transform.localScale = sc;
        _isReverting = false;
    }
    /* Flag and Check Methods */
    //Check if the character is on the ground
    public bool isGrounded()
    {
        bool leftGCheck = Physics2D.Raycast(_leftCast.transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        bool rightGCheck = Physics2D.Raycast(_rightCast.transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        bool leftPCheck = Physics2D.Raycast(_leftCast.transform.position, Vector2.down, _groundCheckDistance, _playerLayer);
        bool rightPCheck = Physics2D.Raycast(_rightCast.transform.position, Vector2.down, _groundCheckDistance, _playerLayer);
        return leftGCheck || rightGCheck || leftPCheck || rightPCheck;
    }
    //Check if the character is allowed to jump
    public bool canJump()
    {
        return (isStuck() || isGrounded()) && !isDead() && !_isStopping;
    }
    //Check if the character is allowed to move
    public bool canMove()
    {
        return !isDead() && !_isStopping;
    }
    //Check if the character is stuck (in the air, but not falling)
    public bool isStuck()
    {
        return (Constants.TOGGLE_WALL_HANG_JUMP ? _rb.velocity.x == 0 && _rb.velocity.y == 0 && !isGrounded() : false);
    }
    //Check if the character needs to flip the sprite
    public bool needsFlip(float inMove)
    {
        return (inMove > 0 && !_isFacingRight) || (inMove < 0 && _isFacingRight);
    }
    //Check if the character is moving at or above the max speed
    public bool atMaxSpeed(float inMove)
    {
        return Mathf.Abs(inMove * _rb.velocity.x) > _maxSpeed;
    }
    //Check if the character is dead
    public bool isDead()
    {
        return _health <= 0;
    }
    //Check if the character is out of lives
    public bool isOutOfLives()
    {
        return _lives <= 0;
    }
    //Check if the character has spawn protection
    public bool isProtected()
    {
        return _protectTimer > 0;
    }
    //Check if the character is allowed to respawn
    public bool canRespawn()
    {
        return _lives > 0 && _deathTimer <= 0 && _isDead;
    }
    //Check if the character can pick up 
    public bool canPickUp()
    {
        return _hasPickUp && !isDead();
    }
    //Check if the character can attack
    public bool canAttack()
    {
        return !isDead() && _equippedWeapon != null;
    }
    //Check if the character can make a secondary attack
    public bool canSecondaryAttack()
    {
        //only if equipped weapon has secondary 
        return !isDead();
    }


    /* Animator methods */
    //Set animation bool under condition
    public void animateBool(string transition, bool trigger)
    {
        _anim.SetBool(transition, trigger);
    }
    //Set animation trigger under condition
    public void animateTrigger(string transition)
    {
        _anim.SetTrigger(transition);
    }

    /* Audio methods */
    //Set the audio clip and play
    public void SetAudioEngine(AudioSource audio)
    {
        audio.Play();
    }
}