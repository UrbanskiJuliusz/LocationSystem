using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [Header("Player Stats")]
        [SerializeField] private float movementForce = 5f;

        private FirstPersonActions playerActions;
        private InputAction move;
        private Rigidbody rb;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            playerActions = new FirstPersonActions();
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            move = playerActions.Player.Move;
            playerActions.Player.Enable();
        }

        private void OnDisable()
        {
            playerActions.Player.Disable();
        }

        void Update()
        {
            Vector3 velocity = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);

            rb.transform.position += movementForce * Time.deltaTime * velocity;
        }

        #endregion
    }
}