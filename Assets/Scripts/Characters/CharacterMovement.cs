using UnityEngine;
using System.Collections;
using Enums;

//Script for controlling character movement of any kind 

/*TODO:
-Add Jump Animation
-Add Move and Jump Sounds
-Implement unstuck
*/

/* Behavior for only Character Movement */
public class CharacterMovement : MonoBehaviour {
    //Movement Values
    public float _moveSpeed = Constants.DEF_CHAR_MOVE_SPEED;
    public float _jumpForce = Constants.DEF_CHAR_JUMP_FORCE;
    public float _maxSpeed = Constants.DEF_CHAR_MOVE_SPEED_MAX;

    //Movement Flags
    private bool _isJumping = false;
    private bool _isMoving = false;
    private bool _isStuck = false;

    //Movement Axes
    public string _moveAxis = Constants.DEF_MOVE_HORIZONTAL_AXIS;
    public string _jumpAxis = Constants.DEF_JUMP_AXIS;

    //Movement Direction Check
    private bool _isFacingRight = false;

    //Movement Grounded Check
    public LayerMask _groundLayer;
    public float _groundCheck = Constants.MIN_GROUND_CHECK;

    //Movement Audio
    public AudioSource _movementAudio;  //Audio Source reference for character movement
    public AudioClip _movingClip;       //Audio Clip for moving
    public AudioClip _jumpingClip;      //Audio Clip for jumping

    //Character references
    private Animator _anim;             //Animator component. Actual animations are not stored here
    private Rigidbody2D _rb;            //Rigidbody component. For dynamic physics
    private CircleCollider2D _collider; //Collider component. Change to other if necessary

    //Feet references to check if on ground
    public GameObject _leftFoot;
    public GameObject _rightFoot;

    //The order of functions is: Awake -> Start -> Update
    //Initialize function; called when an instance of this script is loaded

    //Initialization Functions
    private void Awake()
    {
        //Instantiate and initialize things here
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    //Called when the script is enabled just before any Updates
    private void Start()
    {
        
    }

    //Update Functions
    private void Update()
    {
        /*
        Debug.DrawRay(_groundCheckCircle.transform.position, _groundCheck * Vector2.down, Color.yellow);
        Debug.DrawRay(_groundCheckCircle.transform.position, _groundCheck * Vector2.left, Color.yellow);
        Debug.DrawRay(_groundCheckCircle.transform.position, _groundCheck * Vector2.right, Color.yellow);
        */
        //If Input is Move
        if (Input.GetButton(_moveAxis))
        {
            //Set move flag
            _isMoving = canMove() ? true : _isMoving;   //Thinking about whether flags should be set to false when can returns false (Currently only changes when can returns true)
        }
        //If Input is Jump
        if (Input.GetButton(_jumpAxis))
        {
            //Set jump flag
            _isJumping = canJump() ? true : _isJumping;
        }
    }

    //Physics code in Fixed Update
    private void FixedUpdate()
    {
        //If Move flag is on
        if (_isMoving)
        {
            //Get move value (negative is left, positive is right)
            float h = Input.GetAxis(_moveAxis); //GetAxis gives smoothed input, while GetAxisRaw does not
            move(h);
        }
        else
        {
            _anim.SetBool("CharacterMove", false);
        }
        //If Jump flag is on
        if (_isJumping)
        {
            jump();
        }
;    }

    //Called when this character is enabled
    private void OnEnable()
    {
        //Do we want the character to be kinematic?
        //_rb.isKinematic = false; //If the physics is causing the character to move unecessarily

    }

    //Called when this character is disabled
    private void OnDisable()
    {

    }
   
    void OnDrawGizmos()
    {

    }

    //Checker Functions
    //Check if one of the two feet is on the ground
    private bool isGrounded()
    {

       // return Physics2D.OverlapCircle(_groundCheckCircle.transform.position, _groundCheck, _groundLayer);
        bool leftCheck = Physics2D.Raycast(_leftFoot.transform.position, Vector2.down, _groundCheck, _groundLayer);
        bool rightCheck = Physics2D.Raycast(_rightFoot.transform.position, Vector2.down, _groundCheck, _groundLayer);
        return leftCheck || rightCheck;
    }

    //Check if can jump
    private bool canJump()
    {
        return isGrounded();
    }

    //Check if can move
    private bool canMove()
    {
        return true;
    }

    private bool isStuck()
    {
        //Check if h is non zero and position delta is zero:
        //If so then it is stuck
        //Unstuck by forcing fall
        return false;
    }

    //Check if moving at max speed
    private bool atMaxSpeed(float h)
    {
        return Mathf.Abs(_rb.velocity.x) > _maxSpeed;
    }

    //Check if needs to flip
    private bool needsToFlip(float h)
    {
        return (h > 0 && !_isFacingRight) || (h < 0 && _isFacingRight);
    }
    

    //Action Functions
    public void unstuck(float h)
    {

    }

    //Move the character
    public void move(float h)
    {
        //Set animation speed
        _anim.SetFloat("Speed", Mathf.Abs(h));

        //Set moving animator bool (Is Bool because it can be continuous)
        bool animate_move = h != 0f;
        _anim.SetBool("CharacterMove", animate_move);

        //Accelerate
        //h * _rb.velocity.x < _maxSpeed
        if (!atMaxSpeed(h))
        {
            _rb.velocity = new Vector2(h * _moveSpeed, _rb.velocity.y);
        }

        //Streamline if at max speed
        if (atMaxSpeed(h))
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxSpeed, _rb.velocity.y); //Velocity is the max speed in whatever direction
        }

        //Flip the character if need be
        if (needsToFlip(h))
        {
            flip();
        }
        _isMoving = false; //Reset flag
    }

    //Turns the character to face the opposite direction
    public void flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //Jump
    public void jump()
    {
        //Set Jump Animation Trigger (Is a Trigger because it happens once)
        _anim.SetTrigger("CharacterJump");

        _rb.AddForce(new Vector2(0f, _jumpForce));
        _isJumping = false; //Reset flag
    }

}
