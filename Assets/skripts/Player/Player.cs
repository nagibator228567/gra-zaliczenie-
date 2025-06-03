using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{public static Player Instance { get; private set; }
    private float _movingSpeed = 10f;
    [SerializeField] private float _maxHealth;



    private Rigidbody2D rb;
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    private void FixedUpdate()
    {
        HandleMovement();

    }
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();

        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) < minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
                {
            isRunning = true;
        } else
        {
            isRunning = false;
        }
    }
    public bool IsRunning()
    {
        return isRunning;
    }
    public Vector3 GetMouseScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
    