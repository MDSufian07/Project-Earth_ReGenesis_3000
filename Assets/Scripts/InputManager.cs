using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool FireHeld { get; private set; }
    public bool FirePressed { get; private set; }
    public bool FireReleased { get; private set; }

    public bool RepairHeld { get; private set; }
    public bool RepairPressed { get; private set; }
    public bool RepairReleased { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool SprintHeld { get; private set; }

    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        FirePressed = Input.GetMouseButtonDown(0);
        FireHeld = Input.GetMouseButton(0);
        FireReleased = Input.GetMouseButtonUp(0);

        RepairPressed = Input.GetMouseButtonDown(1);
        RepairHeld = Input.GetMouseButton(1);
        RepairReleased = Input.GetMouseButtonUp(1);

        JumpPressed = Input.GetKeyDown(KeyCode.Space);
        SprintHeld = Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        MoveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
    }
}