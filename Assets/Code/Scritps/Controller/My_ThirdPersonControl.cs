using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using StarterAssets;
using Max_DEV.Interac;

#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace Max_DEV.MoveMent
{
    //Use Class "my_AssetInput"
    [RequireComponent(typeof(My_AssetInput))]
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class My_ThirdPersonControl : MonoBehaviourPun 
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        // Particle
        public ParticleControl particle;

        #region MyOption

        [Header("My Option")]
        [SerializeField] protected ActorTriggerHandler m_ActorTriggerHandler;
        
        [Header("My Jump Option")]
        [SerializeField] private bool MultipleJump;
        public float JumpPower = 5;
        public int JumpLimit = 1;
        public int JumpCount = 0;
        
        [Header("My Climb Option")]
        [SerializeField] private bool Climb;
        private bool InClimbArea = false;
        public float ClimbAreaGravity = -5f;
        public int ClimbSpeed = 10;

        [Header("My Swim Option")] 
        [SerializeField] private bool SwimWater;
        private bool InWater = false;
        public float WaterGravity = -7f;
        public int SwimSpeed = 10;
        public float OutOfWaterJumpPower = 10f;

        [Header("My Attack Option")] 
        [SerializeField] private bool CanAttack;
        public AttackController _AttackController;
        
        private bool _canClimb = false;
        private bool _canSwim = false;
        
        private ObjectType _thisObjectType;
        private PhotonView _photonView;

        #endregion
        

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private My_AssetInput _input;  //config to use my_assetinput
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<My_AssetInput>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            #region My-Start-Region

            _thisObjectType = GetComponent<ObjectType_Identities>().Type;
            Debug.Log(""+this.gameObject + "ObjectType = " + _thisObjectType);
            _photonView = GetComponent<PhotonView>();

            #endregion
        }

        public void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            if (_photonView != null)
            {
                if(!photonView.IsMine)
                    return;
                
                Jump_ClimbAndGravity();
                GroundedCheck();
                Move();

                if (_input.interection == true)
                {
                    if (m_ActorTriggerHandler != null)
                    {
                        PerformInteraction();
                        _input.interection = false;
                    }
                    else
                    {
                        Debug.Log("No Interaction Assign");
                    }
                }
            
                /////// Attack
                if (_AttackController != null && _input.attack && CanAttack)
                {
                    Debug.Log("player PerformAttackRPC");
                    _AttackController.PerformAttack();
                    _AttackController.PerformAttackRPC(_photonView.ViewID);
                    _input.attack = false;
                }
            }
           
            //////////////////////////////////////////////////////////////////////////////////////////
            
            if (_photonView == null)
            {
                Jump_ClimbAndGravity();
                GroundedCheck();
                Move();
                
                if (_input.interection == true)
                {
                    if (m_ActorTriggerHandler != null)
                    {
                        PerformInteraction(); 
                        _input.interection = false;
                    }
                    else
                    {
                        Debug.Log("No Interaction Assign");
                    }
                }
                            
                /////// Attack
                if (_AttackController != null && _input.attack && CanAttack)
                {
                    //Debug.Log("PerformAttack Nomal");
                    _AttackController.PerformAttack();
                    _input.attack = false;
                }
            }
            
        }

        private void LateUpdate()
        {
            //CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
            
            // reset jump count when on ground
            if(Grounded || _canClimb)
                ResetJump();
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        public void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                
                //Check If Have Partical
                if (particle != null)
                {
                    particle.particIsPlay = false;
                }
                    
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
                
                //Check If Have Partical
                if (particle != null)
                {
                    particle.particIsPlay = true;
                }

            }
            else
            {
                _speed = targetSpeed;

                
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                
            }
           

        }

        public void Jump_ClimbAndGravity()
        {
            if (_canSwim && SwimWater)
            {
                if (_input.climb && SwimWater)
                    Swim(SwimSpeed);

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }
            
            if (Grounded || _canClimb || JumpCount < JumpLimit)
            {


                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                    
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    if (InWater)
                    {
                        //Debug.Log("Water Gravity");
                        _verticalVelocity += WaterGravity * Time.deltaTime;
                    }

                    if (InClimbArea)
                    {
                        //Debug.Log("ClimbArea Gravity");
                        _verticalVelocity += ClimbAreaGravity * Time.deltaTime;
                    }
                    if (!InWater && !InClimbArea)
                    {
                        //Debug.Log("ground Gravity");
                        _verticalVelocity += Gravity * Time.deltaTime;
                    }
                    //_verticalVelocity = -2f;
                }
                
                
                // Climb
                if (_input.climb && _canClimb && Climb)
                {
                    if (_canClimb == true)
                        Climb_Up(ClimbSpeed);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }
                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && JumpCount < JumpLimit && !_canClimb)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    //_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _verticalVelocity = Mathf.Sqrt(JumpPower);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                    

                    // incress JumpCount When jump
                    if(MultipleJump)
                        JumpCount += 1;
                    else
                        JumpCount += 999999;
                    
                    //Debug.Log("jump = "  + JumpCount);

                    _input.jump = false;
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                if (InWater)
                {
                    //Debug.Log("Water Gravity");
                    _verticalVelocity += WaterGravity * Time.deltaTime;
                }

                if (InClimbArea)
                {
                    //Debug.Log("ClimbArea Gravity");
                    _verticalVelocity += ClimbAreaGravity * Time.deltaTime;
                }
                if (!InWater && !InClimbArea)
                {
                    //Debug.Log("ground Gravity");
                    _verticalVelocity += Gravity * Time.deltaTime;
                }
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (LandingAudioClip != null)
                {
                    AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);

                }
            }
        }

        #region MyRegion

        private void OnTriggerEnter(Collider other)
                {
                    ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
                    if (OtherType != null)
                    {
                        switch (OtherType.Type)
                        {
                            case ObjectType.ClimbArea:
                                if (!Climb)
                                    return;
                                _canClimb = true;
                                InClimbArea = true;
                                ResetJump();
                                //print("can Climb");
                                break;
                            case ObjectType.Floor:
                                ResetJump();
                                Grounded = true;
                                //print("On Ground");
                                break;
                            case ObjectType.Water:
                                InWater = true;
                                if (!SwimWater)
                                {
                                    var hp = this.GetComponent<HealthPoint>();
                                    hp.DecreaseHp(1);
                                    hp.Respawn();
                                    Debug.Log("InWater");
                                    return;
                                }
                                _canSwim = true;
                                break;
                        }
                    }
                }
                
        private void OnTriggerExit(Collider other)
                {
                    ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
                    if (OtherType != null)
                    {
                        switch (OtherType.Type)
                        {
                            case ObjectType.ClimbArea:
                                InClimbArea = false;
                                _canClimb = false;
                                break;
                            case ObjectType.Water:
                                InWater = false;
                                _canSwim = false;
                                _verticalVelocity = Mathf.Sqrt(OutOfWaterJumpPower);
                                break;
                        }
                    }
                }
        
        public void ResetJump()
        {
            JumpCount = 0;
        }
        
        public void Climb_Up(float _climbSpeed)
        {
            _verticalVelocity = Mathf.Sqrt(_climbSpeed);
            //print("Climbing");
        }

        public void Swim(float _swimSpeed)
        {
            _verticalVelocity = Mathf.Sqrt(_swimSpeed);
            ///print("Swim");
        }

        protected virtual void PerformInteraction()
        {
            var interactable = m_ActorTriggerHandler.GetInteractable();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }

        private void Attack()
        {
            
        }

        public void movement_m(Vector2 direction)
        {
            Debug.Log("Direction = " + direction);
        }

        #endregion

        #region MonoPUN

        

        #endregion
    }
}