using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ScopeController : MonoBehaviour
{
    [SerializeField, Foldout("Reference")] private Transform _thoughtScreen;
    [SerializeField, Foldout("Reference")] private GameUI _gameUI;
    [SerializeField, Foldout("Reference")] private LayerMask _ignoredLayer;
    [SerializeField, Foldout("Reference")] private GameObject _iconPrefab;
    [SerializeField] private float _speed = 3.0f; 
    [SerializeField] private float _Delay = 1;
    [SerializeField] private float _ReloadDelay = 2;
    [SerializeField, Foldout("Sound")] private AudioClip _reloadSound = null;
    [SerializeField, Foldout("Sound")] private AudioClip[] _shootSound = null;
    [SerializeField] private float _MagCapacity = 6;
    
    [SerializeField] private float _gunRange = 0.5f;
    [SerializeField] private bool _canCaptureMultiple = true;
    [SerializeField] private float _EffectVolume = 1.0f;

    [SerializeField, Foldout("Art")] private Animator _playerCharacter;
    [SerializeField, Foldout("Art")] private Transform _animTarget;

    private Vector2 _MoveDirection;
    private bool _CanShoot = true;
    private CharacterController _Character;
    private float _bullet1Remaining;

    private List<JokeThemeSO> _capturedThemes;
    private List<BonusType> _capturedBonus;
    public List<JokeThemeSO> GetCapturedJokes() => _capturedThemes;
    public List<BonusType> GetCapturedBonus() => _capturedBonus;
    
    [SerializeField] private int _playerIndex;
    public int GetPlayerIndex() => _playerIndex;
    public int GetPlayerScore() => _capturedBonus.Count + _capturedThemes.Count;
   

    private AudioSource audioSource;
    private GameManager _gameManager;

    private void Start()
    {
        _bullet1Remaining = _MagCapacity;
        Debug.Log(_bullet1Remaining);
        _capturedThemes = new List<JokeThemeSO>();
        _capturedBonus = new List<BonusType>();
        audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _gameManager.SetPlayer(this,_playerIndex);
        _animTarget.parent = transform;
        _animTarget.localPosition = Vector3.zero;
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
        if (_gameManager.GamePhase != 1) return;
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
        _gameUI.Shooting(_playerIndex);
        audioSource.PlayOneShot(_shootSound[Random.Range(0,_shootSound.Length)], _EffectVolume);
        _playerCharacter.SetTrigger("Shoot");
        
        //Check Collision with Theme Bubbles
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _gunRange);
        foreach (Collider2D col in cols)
        {
            if (col.gameObject.CompareTag("ThoughtBubble"))
            {
                ThemeBubbleBehaviour themeBubble = col.GetComponent<ThemeBubbleBehaviour>();
                _capturedThemes.Add(themeBubble.GetThemeSo());
                CollectedIcon iconPrefab = Instantiate(_iconPrefab, themeBubble.gameObject.transform.position, Quaternion.identity).GetComponent<CollectedIcon>();
                iconPrefab.Setup(this,themeBubble.GetSprite());
                themeBubble.Capture();
                if (!_canCaptureMultiple) break;
            }
            else if (col.gameObject.CompareTag("BonusBubble"))
            {
                BonusBubbleBehaviour bonusBubble = col.GetComponent<BonusBubbleBehaviour>();
                _capturedBonus.Add(bonusBubble.GetBonusType());
                CollectedIcon iconPrefab = Instantiate(_iconPrefab, bonusBubble.gameObject.transform.position, Quaternion.identity).GetComponent<CollectedIcon>();
                iconPrefab.Setup(this,bonusBubble.GetSprite());
                bonusBubble.Capture();
                if (!_canCaptureMultiple) break;
            }
        }

        if (GetPlayerScore() == 1) _gameManager.FirstCloudCaptured(_playerIndex);
        
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
        if (_bullet1Remaining == 0) {
            audioSource.PlayOneShot(_reloadSound, _EffectVolume);
        }
        yield return new WaitForSeconds(_bullet1Remaining == 0 ? _ReloadDelay : _Delay);
        if (_bullet1Remaining == 0)
        {
            _bullet1Remaining = _MagCapacity;
            _gameUI.Reload(_playerIndex);
        }
        _CanShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _gunRange);
    }

    public void ReadOKInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gameManager.PassToNextJoke(_playerIndex);
        }
    }

    public void UpdateScore()
    {
        _gameUI.UpdateScoreUI(_playerIndex, GetPlayerScore());

    }
}
