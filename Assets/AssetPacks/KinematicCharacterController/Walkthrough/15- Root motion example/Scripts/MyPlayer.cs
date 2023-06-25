using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System.Linq;
using UnityEngine.InputSystem;
using Triggers;

namespace KinematicCharacterController.Walkthrough.RootMotionExample
{
    public class MyPlayer : MonoBehaviour
    {
        public ExampleCharacterCamera OrbitCamera;
        public Transform CameraFollowPoint;
        public MonoBehaviour Character;
        public PlayerInput playerInput;

        public List<TriggerOnInputKeyPressed> inputKeyTriggers = new List<TriggerOnInputKeyPressed>();

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        protected bool isClimbing = false;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            OrbitCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            OrbitCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                //OrbitCamera.TargetDistance = (OrbitCamera.TargetDistance == 0f) ? OrbitCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            
            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            if (!((MyCharacterController)Character).useMouse)
            {
                characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            }

            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            ((MyCharacterController)Character).SetInputs(ref characterInputs);

        }


        public void ToggleClimbing(GameObject toClimb)
        {
            if (((Player.FinalCharacterController)Character).CurrentCharacterState == Player.CharacterState.Climbing)
            {
                StopClimbing();
            } else
            {
                StartClimbing(toClimb);
            }
        }

        public void StartClimbing(GameObject toClimb)
        { 
            isClimbing = true;
            ((Player.FinalCharacterController)Character).TransitionToState(Player.CharacterState.Climbing);

            Vector3 climbPosition = new Vector3();
            climbPosition.Set(toClimb.transform.position.x, transform.position.y, toClimb.transform.position.z);
            climbPosition = climbPosition - toClimb.transform.forward * (toClimb.GetComponent<BoxCollider>().size.z * 0.5f);
            Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPositionAndRotation(climbPosition, toClimb.transform.rotation);

            //playerInput.SwitchCurrentActionMap("KeyboardClimbingControls");

            Character.GetComponent<KinematicCharacterMotor>().HasPlanarConstraint = true;
            Character.GetComponent<Rigidbody>().useGravity = false;
        }

        public void StopClimbing()
        {
            isClimbing = false;
            ((Player.FinalCharacterController)Character).TransitionToState(Player.CharacterState.Default);
            //playerInput.SwitchCurrentActionMap("KeyboardDefaultControls");
            Character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Character.GetComponent<KinematicCharacterMotor>().HasPlanarConstraint = false;
            Character.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}