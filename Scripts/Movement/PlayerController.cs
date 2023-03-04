using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //origional
    [Header("Assighnables")]
    public Rigidbody rb;

    [SerializeField] private Transform Camera;
    public static Transform cameraLook;
    [SerializeField] private Transform head;
    [SerializeField] private Transform orientation;
    [SerializeField] public AudioManager audioManager;

    [Header("Mouse Settings")]
    [SerializeField] private Vector2 sensitivity = new Vector2(5, 5);
    [SerializeField] private float sensitvityMultiplier = 0.01f;

    [Header("Movement Settings")]
    [SerializeField] private float gravity;
    [SerializeField] private float speed;
    [SerializeField] private float drag_magnitude;
    [SerializeField] private float grounddrag;
    [SerializeField] private float airdrag;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float aimMultiplier = 0.4f;
    [SerializeField] private float airMultiplier = 0.2f;

    [Header("Sliding and Crouching")]
    [SerializeField] private float slideCounterMovement = 0.2f;
    [SerializeField] private float slideStartThreshold = 5f;
    [SerializeField] private float slideSpeed = 4f;
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private float crouchMovementMultiplier = 0.5f;
    [Header("Crouching Animation")]
    [SerializeField] private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Vector3 playerScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private float crouchSpeed = 10f;

    [Header("JumpSettings")]
    [SerializeField] private float groundCheckRadious;
    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private int maxNumJump = 2;

    [Header("Controls")]
    [SerializeField] private KeyCode CrouchKey = KeyCode.C;

    [Header("Particles")]
    [SerializeField] private ParticleSystem speedEffect;
    [SerializeField] private Transform speedEffectTransform;
    [SerializeField] private float speedEffectThreshold = 10f;
    [SerializeField] private AnimationCurve particleSpeed = new AnimationCurve();

    [Header("Sounds")]

    [Header("FootSteps")]
    [SerializeField] public bool allowFootstep = true;
    [SerializeField] private float walkStepFrequency = 0.05f;
    [SerializeField] private float aimStepFrequency = 0.05f;
    [SerializeField] private float sprintStepFrequency = 0.05f;
    [SerializeField] private float crouchStepFrequency = 0.05f;
    [SerializeField] private AnimationCurve volumeOverSpeed = new AnimationCurve();


    [Header("Debug Bools")]
    [Space(5)]

    public Vector3 velocityOnHorizontalPlane;
    public float speedOnHorizontalPlane;

    // debug in inspecter
    public bool _grounded, _readytojump, _airborne, _aimWalking, _crouchWalking, _walking, _sprinting, _Sprinting, _aiming, _crouching, _sliding, _grappling;

    // for access by other scripts
    public static bool grounded, readytojump, airborne, aimWalking, crouchWalking, walking, sprinting, Sprinting, aiming, crouching, sliding, grappling;

    bool isSliding;
    public static float grapplemultiplier = 0.05f;
    public static float grappleDrag = 0.05f;

    public static float X;
    public static float Y;
    public static Vector3 moveDir;
    float _multiplier;
    float _drag;
    int _numJumps;
    float _stepFrequency;

    float timeToStep;



    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _numJumps = maxNumJump;
        PlayerController.readytojump = true;
        timeToStep = walkStepFrequency;
    }

    void UpdateCamPos()
    {
        Camera.position = head.position;
    }

    float xRotation;
    float yRotation;
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        yRotation += mouseX * sensitivity.x * sensitvityMultiplier;
        xRotation -= mouseY * sensitivity.y * sensitvityMultiplier;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        Camera.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0f);
    }

    void PlayerState()
    {
        aiming = ProjectileGun.aiming;

        walking = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && aiming == false && Sprinting == false && crouching == false && grounded == true;
        airborne = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && aiming == false && Sprinting == false && crouching == false && grounded == false;

        aimWalking = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && aiming == true && Sprinting == false && crouching == false && grounded == true;
        crouchWalking = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && aiming == false && Sprinting == false && crouching == true && grounded == true;

        if (ProjectileGun.aiming == true)
        {
            walking = false;
            //Sprinting = false;
            sprinting = false;
        }

        // if (walking == true)
        // {
        //     //Sprinting = false;
        //     sprinting = false;
        // }

        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprinting = !sprinting;
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            sprinting = false;
        }

        //sprinting = Input.GetKey(KeyCode.LeftShift);

        Sprinting = sprinting && Input.GetKey(KeyCode.W) && 
            crouching == false && aiming == false && grounded == true &&
            ProjectileGun.shooting == false && ProjectileGun.reloading == false;

        if (Input.GetKeyDown(CrouchKey))
        {
            crouching = !crouching;
            sprinting = false; // if you are trying to crouch but can't because sprinting, then sprinting becomes false
        }

        if (sprinting == true)
            crouching = false; // trying to sprint while crouching cancels the crouch
    }

    void SetSpeed()
    {
        if (PlayerController.grounded == false)
        {
            if (PlayerController.grappling == true)
            {
                _multiplier = grapplemultiplier;
                _drag = grappleDrag;
            }
            else
            {
                _multiplier = airMultiplier;
                _drag = airdrag;
            }

            return;
        }

        if (PlayerController.walking)
        {
            _multiplier = speedMultiplier;
            _drag = grounddrag;
        }
        else if (PlayerController.Sprinting)
        {
            _multiplier = sprintMultiplier;
            _drag = grounddrag;
        }
        else if (PlayerController.aimWalking)
        {
            _multiplier = aimMultiplier;
            _drag = grounddrag;
        }
        else if (PlayerController.crouching == true)
        {
            _multiplier = crouchMovementMultiplier;
            _drag = grounddrag;
            if (PlayerController.sliding == true)
            {
                _drag = slideCounterMovement;
            }
        }
        else
        {
            _multiplier = speedMultiplier;
            _drag = grounddrag;
        }
    }

    void StartCrouch()
    {
        if (PlayerController.crouching == true && speedOnHorizontalPlane > slideStartThreshold && isSliding == true  && PlayerController.grounded)
        {
            Slide();
            isSliding = false;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, crouchScale, crouchSpeed * Time.deltaTime);
    }
    void StopCrouch()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, playerScale, crouchSpeed * Time.deltaTime);

        PlayerController.crouching = false;
        PlayerController.sliding = false;
        isSliding = true;
    }

    void Slide()
    {
        PlayerController.sliding = true;
        rb.AddForce(orientation.transform.forward * slideSpeed, ForceMode.Impulse);
        print("Sliding");

        StartCoroutine(StopProjectedSlide(slideDuration));
        //StopProjectedSlide(velocityOnHorizontalPlane.magnitude);
    }

    private IEnumerator StopProjectedSlide(float duration)
    {
        yield return new WaitForSeconds(duration);

        PlayerController.sliding = false;
        isSliding = false;
    }

    void Jump(float jumpForce)
    {
        // PlayerController.crouching = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Movement()
    {
        //basic movement
        X = Input.GetAxis("Horizontal");
        Y = Input.GetAxis("Vertical");

        moveDir = orientation.right * X + orientation.forward * Y;


        //friction
        Drag();

        //jumping
        if (GroundCheck() == true)
        {
            _numJumps = maxNumJump;
            // PlayerController.readytojump = true;
        }

        if (PlayerController.grounded == true)
        {
            if (Input.GetKey(KeyCode.Space) && PlayerController.readytojump == true)
            {
                PlayerController.readytojump = false;
                Jump(jumpForce);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space) && _numJumps > 0 && PlayerController.readytojump == true)
            {
                PlayerController.readytojump = false;
                Jump(doubleJumpForce);
                _numJumps--;
            }
        }
        if (!Input.GetKey(KeyCode.Space))
            PlayerController.readytojump = true;

        //crouching and sliding
        if (PlayerController.crouching)
            StartCrouch();
        else
            StopCrouch();

        if (PlayerController.sliding)
            return;

        //adding force
        rb.AddForce(moveDir * speed * _multiplier);

        // footsteps
        HandleFootSteps();
    }

    void HandleFootSteps()
    {
        if (!allowFootstep)
            return;
        
        if (_stepFrequency != 0f)
        {
            if (timeToStep > 0f) timeToStep -= Time.deltaTime;
            if (timeToStep <= 0f) PlayFootSteps();
        }
    }

    void PlayFootSteps()
    {
        if (grounded == true && moveDir.magnitude > 0f)
        {
            audioManager.PlaySound("FootSteps", volumeOverSpeed.Evaluate(velocityOnHorizontalPlane.magnitude));
        }

        timeToStep = _stepFrequency;
    }

    Vector3 _groundCheckPos;
    bool GroundCheck()
    {
        bool _grounded = Physics.CheckSphere(transform.position - _groundCheckPos, groundCheckRadious, groundMask);

        return _grounded;
    }


    Vector3 counterMovement = Vector3.zero;
    // void Drag(){
    //     rb.drag = _drag * drag_magnitude;
    // }
    void Drag()
    {
        velocityOnHorizontalPlane = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // speedOnHorizontalPlane = Mathf.Sqrt(Mathf.Pow(velocityOnHorizontalPlane.x, 2) + Mathf.Pow(velocityOnHorizontalPlane.z, 2));
        speedOnHorizontalPlane = velocityOnHorizontalPlane.magnitude;

        if (PlayerController.sliding == true)
        {
            counterMovement = -velocityOnHorizontalPlane * slideCounterMovement * slideSpeed;
        }
        else
        {
            counterMovement = -velocityOnHorizontalPlane * _drag * speed;
        }


        rb.AddForce(counterMovement);
    }


    void Update()
    {
        PlayerController.cameraLook = Camera.transform;

        //looking around
        Look();
        UpdateCamPos();

        //grounded bool
        PlayerController.grounded = GroundCheck();

        //movement State
        SetSpeed();
        PlayerState();

        SetBools();

        velocityOnHorizontalPlane = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        _groundCheckPos = groundCheckPos;

        // footsteps
        SetFootStepFrequency();

        HandleParticles();
    }

    void HandleParticles()
    {
        // speedEffectTransform.LookAt(speedEffectTransform.position + rb.velocity);

        if (X > 0f && Y == 0f) speedEffectTransform.forward = orientation.right;
        else if (X < 0f && Y == 0f) speedEffectTransform.forward = -orientation.right;
        else if (X == 0f && Y > 0f) speedEffectTransform.forward = orientation.forward;
        else if (X == 0f && Y < 0f) speedEffectTransform.forward = -orientation.forward;

        if (rb.velocity.magnitude >= speedEffectThreshold)
            speedEffectTransform.gameObject.SetActive(true);
        else
            speedEffectTransform.gameObject.SetActive(false);
    }

    void SetFootStepFrequency()
    {
        if (walking)
            _stepFrequency = walkStepFrequency;
        else if (aimWalking)
            _stepFrequency = aimStepFrequency;
        else if (Sprinting)
            _stepFrequency = sprintStepFrequency;
        else if (crouching)
            _stepFrequency = crouchStepFrequency;
        else
            _stepFrequency = 0f;
    }

    void SetBools()
    {
        _grounded = PlayerController.grounded;
        _readytojump = PlayerController.readytojump;
        _airborne = PlayerController.airborne;
        _aimWalking = PlayerController.aimWalking;
        _crouchWalking = PlayerController.crouchWalking;
        _walking = PlayerController.walking;
        _sprinting = PlayerController.sprinting;
        _Sprinting = PlayerController.Sprinting;
        _aiming = PlayerController.aiming;
        _crouching = PlayerController.crouching;
        _sliding = PlayerController.sliding;
        _grappling = PlayerController.grappling;
    }

    void LateUpdate()
    {
    }

    void FixedUpdate()
    {
        // Gravity
        rb.AddForce(Vector3.down * gravity);

        Movement();
    }


    //Debugging

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - _groundCheckPos, groundCheckRadious);
    }
}

