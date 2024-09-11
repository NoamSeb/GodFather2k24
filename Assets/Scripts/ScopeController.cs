using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeController : MonoBehaviour
{
    [SerializeField] private Transform _thoughtScreen;
    [SerializeField] private LayerMask _ignoredLayer;
    [SerializeField] private float _speed = 3.0f; 
    [SerializeField] private float _Delay = 1;
    [SerializeField] private float _ReloadDelay = 2;
    [SerializeField] private float _MagCapacity = 6;
    
    [SerializeField] private float _gunRange = 0.5f;
    [SerializeField] private bool _canCaptureMultiple = true;

    private Vector2 _MoveDirection;
    private bool _CanShoot = true;
    private CharacterController _Character;
    private float _bullet1Remaining;

    private List<JokeThemeSO> _capturedThemes;
    public List<JokeThemeSO> GetCapturedJokes() => _capturedThemes;
    
    [SerializeField] private int _playerIndex;
    public int GetPlayerIndex() => _playerIndex;
   
    private void Start()
    {
        _bullet1Remaining = _MagCapacity;
        Debug.Log(_bullet1Remaining);
        _capturedThemes = new List<JokeThemeSO>();
    }

    #region Initialization
    private void OnEnable()
    {
        _Character = gameObject.AddComponent<CharacterController>();
        _Character.radius = 0.4f;
        _Character.detectCollisions = true;
        _Character.excludeLayers = _ignoredLayer;

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
            //float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            // Use normalized vector to move the character
            Vector2 moveDirection = direction;

            Vector2 motion = moveDirection.normalized * _speed * Time.deltaTime;
            (bool canMoveX, bool canMoveY) = CanMove(motion);
            if (!canMoveX && !canMoveY) return;
            else
            {
                motion = new Vector2(canMoveX ? motion.x : 0, canMoveY ? motion.y : 0);
            }
            
            // Apply the movement
            _Character.Move(motion);
        }
    }

    private (bool,bool) CanMove(Vector2 motion)
    {
        Vector2 nextPos = (Vector2)transform.position + motion;
        Vector3 borderPos = _thoughtScreen.position;
        Vector3 borderScale = _thoughtScreen.localScale;
        return (!(nextPos.x < borderPos.x - borderScale.x / 2 || nextPos.x > borderPos.x + borderScale.x / 2), !(nextPos.y < borderPos.y - borderScale.y / 2) && !(nextPos.y > borderPos.y + borderScale.y / 2) );
    }

    void OnShoot()
    {
        _CanShoot = false;
        _bullet1Remaining -= 1;
        
        //Check Collision with Theme Bubbles
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _gunRange);
        foreach (Collider2D col in cols)
        {
            if (col.gameObject.CompareTag("ThoughtBubble"))
            {
                ThemeBubbleBehaviour themeBubble = col.GetComponent<ThemeBubbleBehaviour>();
                _capturedThemes.Add(themeBubble.GetThemeSo());
                themeBubble.Capture();
                if (!_canCaptureMultiple) break;
            }
        }
        
        StartCoroutine(WaitForShoot());
        Debug.Log("Bullet remaining : " + _bullet1Remaining);

        //Print CapturedThemes
        /*foreach (JokeThemeSO capturedTheme in _capturedThemes)
        {
            Debug.Log("One joke of theme : " + capturedTheme.Theme);
        }*/
    }
    private IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(_bullet1Remaining == 0 ? _ReloadDelay : _Delay);
        if (_bullet1Remaining == 0) { 
            _bullet1Remaining = _MagCapacity;
        }
        _CanShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _gunRange);
    }
}
