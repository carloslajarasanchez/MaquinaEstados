using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    InputSystem_Actions _inputActions;

    private void Start()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();
    }
    private void Update()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        Vector2 input = _inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(input.x, 0f, input.y) * _speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}
