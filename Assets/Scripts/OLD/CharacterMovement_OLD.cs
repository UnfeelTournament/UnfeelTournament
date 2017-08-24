
using UnityEngine;
using System.Collections;
using Enums;

//Script for controlling character movement of any kind 

/*TODO:
-Determine Addforce or Velocity for move
-Animations and Sounds
*/

//Bug: When standing on the edge of a platform, cannot jump. Possibly due to the raycast being a line straight down from the center, thus it does not detect the platform if the character is on the edge.

/* Note: Possibly Better Jump Check 
-Create transform circle as a child of character
-Place the circle on the very bottom of the character, and makes its radius very small
-Check using an Overlap circle of that transform circle and a ground layer mask
*/

//Maybe have rebindable controls, so have to put in checks to see which key is being pressed

/* Behavior for only Character Movement */
/*
public class CharacterMovement : MonoBehaviour
{
    //Movement Values
    public float _moveForce = Constants.DEF_CHAR_MOVE_FORCE;
    public float _jumpForce = Constants.DEF_CHAR_JUMP_FORCE;
    public float _maxSpeed = Constants.DEF_CHAR_MOVE_SPEED;

    public LayerMask _groundLayer;

    private bool _isFacingRight = false;

    private bool _isJumping = false;
    private bool _isGrounded = false;       //If the character is not in the air nor jumping; is on the ground
    private bool _canMove = true;

    public float _groundCheck = Constants.MIN_GROUND_CHECK;

    //Movement Inputs
    private string _movementAxisName;   //Name of the input axis for moving forward and back
    private float _movementInputValue;  //Input value of movement

    //Movement Audio
    public AudioSource _movementAudio;  //Audio Source reference for character movement
    public AudioClip _movingClip;       //Audio Clip for moving
    public AudioClip _jumpingClip;      //Audio Clip for jumping

    private Animator _anim;             //Animator component. Actual animations are not stored here
    private Rigidbody2D _rb;            //Rigidbody component. For dynamic physics
    private CircleCollider2D _collider; //Collider component. Change to other if necessary

    //The order of functions is: Awake -> Start -> Update
    //Initialize function; called when an instance of this script is loaded
    private void Awake()
    {
        //Instantiate and initialize things here

        //Set up references
        //_anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    //Called when the script is enabled just before any Updates
    private void Start()
    {
        _movementAxisName = "Horizontal"; //+ player_number
    }

    private void Update()
    {
        _isGrounded = isGrounded();

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _isJumping = true;
        }
    }

    //Physics code in Fixed Update
    private void FixedUpdate()
    {
        //GetAxis gives smoothed input, while GetAxisRaw does not
        float h = Input.GetAxis(_movementAxisName);

        //_anim.SetFloat("Speed", Mathf.Abs(h));

        move(h);
        if (_isJumping)
        {
            jump();
        }
;
    }

    //Called when this character is enabled
    private void OnEnable()
    {
        //Do we want the character to be kinematic?
        //_rb.isKinematic = false; //If the physics is causing the character to move unecessarily

        //reset values
        _movementInputValue = 0f;
    }

    //Called when this character is disabled
    private void OnDisable()
    {

    }

    //Move the character
    public void move(float h)
    {

        //Check if can move, it can't then do nothing
        if (!_canMove) return;
        //Set animator moving trigger


        //Accelerate
        if (h * _rb.velocity.x < _maxSpeed)
        {
            //Velocity v AddForce: AddForce applies a force with acceleration, while velocity simply changes it

            //One or the other
            //_rb.AddForce(Vector2.right * h * _moveForce);   
            _rb.velocity = new Vector2(h * _moveForce, _rb.velocity.y);
        }

        //Streamline if at max speed
        if (Mathf.Abs(_rb.velocity.x) > _maxSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxSpeed, _rb.velocity.y); //Velocity is the max speed in whatever direction
        }

        //Flip the character if need be
        if (h > 0 && !_isFacingRight) //If moving right and is not facing right
        {
            flip();
        }
        else if (h < 0 && _isFacingRight) //If moving left and is not facing left
        {
            flip();
        }
    }

    private bool isGrounded()
    {
        //better than Linecast as this will check for any ground collider in range, instead of specifying a ground collider to check for
        return Physics2D.Raycast(transform.position, Vector2.down, _groundCheck, _groundLayer);
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
        if (_isJumping)
        {
            //Set Jump Animation Trigger

            _rb.AddForce(new Vector2(0f, _jumpForce));
            _isJumping = false; // set this flag to false elsewhere to prevent multi jumps
        }
    }
}
*/