using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isFacingRight;

    [Header("Movement Force")]
    [SerializeField] private float frictionAmt;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deccerlation;
    [SerializeField] private float velPower;

    [Header("Ground Check")]
    public bool isGrounded;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float checkRadius;


    private Rigidbody2D _rb;
    private PlayerControls.GameplayActions controls {
        get { return GameplayInputManager.Instance.playerControls.Gameplay; }
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        float horizontalInput = controls.Movement.ReadValue<Vector2>().x;
        isGrounded = Physics2D.OverlapCircle(checkPoint.position, checkRadius, groundMask);

        #region Movement
        // Target velocity to achieve
        float tagrteVel = horizontalInput * speed;
        // Calculate velocity difference
        float velDiff = tagrteVel - _rb.linearVelocityX;
        // Check acceleration rate to use
        float accelRate = Mathf.Abs(velDiff) > 0.001f ? acceleration : deccerlation;
        // calculate force
        Vector2 movementForce = Mathf.Pow(Mathf.Abs(velDiff) * accelRate, velPower) * Mathf.Sign(velDiff) * Vector2.right;
        _rb.AddForce(movementForce * _rb.mass, ForceMode2D.Force);
        #endregion

        #region Friction
        if(isGrounded)
        {
            // Get smallest of the two values to use as friction on x-axis
            Vector2 friction = Mathf.Min(Mathf.Abs(_rb.linearVelocityX), Mathf.Abs(frictionAmt)) * Vector2.right;
            // Get the direction of the velocity and invert it
            friction *= -Mathf.Sign(_rb.linearVelocityX);
            _rb.AddForce(friction * Time.deltaTime, ForceMode2D.Force);
        }
        #endregion

        #region Slope Handeling
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector3.down, 1, groundMask); // Cast a ray downward from the player's feet
        if (hit) 
        {
            Vector3 surfaceNormal = hit.normal;
            float slopeAngle = Vector3.Angle(surfaceNormal, Vector3.up);

            if(slopeAngle > 0 && isGrounded && horizontalInput == 0) _rb.gravityScale = 0;
            else _rb.gravityScale = 1;
        }

        if(_rb.gravityScale == 0 && _rb.linearVelocityY > 0)
            _rb.linearVelocityY = 0;
        #endregion

        if((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight)) Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if(_rb == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_rb.linearVelocity);   
    }
}
