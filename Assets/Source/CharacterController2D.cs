using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float _crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool _airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask _whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform _groundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform _ceilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D _crouchDisableCollider;				// A collider that will be disabled when crouching

	public const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool _grounded;            // Whether or not the player is grounded.
	public bool Grounded => _grounded;
	public const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D _rigidbody2D;
	private bool _facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _velocity = Vector3.zero;

	public float Direction => _facingRight ? 1f : -1f;

	[FormerlySerializedAs("OnLandEvent")]
	[Header("Events")]
	[Space]

	[SerializeField] private UnityEvent _onLandEvent;

	[Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	[SerializeField] private BoolEvent _onCrouchEvent;
	private bool _wasCrouching = false;

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();

		if (_onLandEvent == null)
			_onLandEvent = new UnityEvent();

		if (_onCrouchEvent == null)
			_onCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		var wasGrounded = _grounded;
		_grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		var colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius, _whatIsGround);
		foreach (var c in colliders)
		{
			if (c.gameObject == gameObject)
				continue;

			_grounded = true;
			if (!wasGrounded)
				_onLandEvent.Invoke();
		}
	}

	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(_ceilingCheck.position, k_CeilingRadius, _whatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (_grounded || _airControl)
		{

			// If crouching
			if (crouch)
			{
				if (!_wasCrouching)
				{
					_wasCrouching = true;
					_onCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= _crouchSpeed;

				// Disable one of the colliders when crouching
				if (_crouchDisableCollider != null)
					_crouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (_crouchDisableCollider != null)
					_crouchDisableCollider.enabled = true;

				if (_wasCrouching)
				{
					_wasCrouching = false;
					_onCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, _rigidbody2D.linearVelocity.y);
			// And then smoothing it out and applying it to the character
			_rigidbody2D.linearVelocity = Vector3.SmoothDamp(_rigidbody2D.linearVelocity, targetVelocity, ref _velocity, _movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !_facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && _facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (_grounded && jump)
		{
			// Add a vertical force to the player.
			_grounded = false;
			_rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
