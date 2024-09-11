using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeController : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f; 
    [SerializeField] private float _Delay = 1;
    [SerializeField] private float _ReloadDelay = 2;
    [SerializeField] private float _MagCapacity = 6;

    private Vector2 _MoveDirection;
    private bool _CanShoot = true;
    private CharacterController _Character;
    private float _bullet1Remaining;

    [SerializeField] private int _playerIndex;
    public int GetPlayerIndex() => _playerIndex;
   
    private void Start()
    {
        _bullet1Remaining = _MagCapacity;
        Debug.Log(_bullet1Remaining);
    }

    #region Initialization
    private void OnEnable()
    {
        _Character = gameObject.AddComponent<CharacterController>();
        _Character.radius = 0.4f;
        _Character.detectCollisions = true;

        // Find the character object
        Transform childObject = transform.Find("scope");
        
    }

    private void OnDisable()
    {
        Destroy(_Character);
    }
    #endregion

    void FixedUpdate()
    {
        OnMove();
    } 

    public void ReadMoveInput(InputAction.CallbackContext context)
    {
        _MoveDirection = context.ReadValue<Vector2>();
        
    }

    public void ReadShootInput (InputAction.CallbackContext context)
    {
        if (context.performed && _CanShoot)
        {
            OnShoot();
        }
        else if (context.performed && !_CanShoot)
        {
            Debug.Log("Cannot shoot yet !");
        }
    }


    void OnMove()
    {
        Vector2 direction = new Vector2(_MoveDirection.x, _MoveDirection.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Get direction angle from direction vector
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            // Use normalized vector to move the character
            Vector2 moveDirection = direction;

            // Apply the movement
            _Character.Move(moveDirection.normalized * _speed * Time.deltaTime);
        }
    }

    void OnShoot()
    {
        _CanShoot = false;
        _bullet1Remaining -= 1;
        StartCoroutine(WaitForShoot());
        Debug.Log("Bullet remaining : " + _bullet1Remaining);
    }
    private IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(_bullet1Remaining == 0 ? _ReloadDelay : _Delay);
        if (_bullet1Remaining == 0) { 
        _bullet1Remaining = _MagCapacity;
        }
        _CanShoot = true;   
    }
}
