using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    //[SerializeField]
    //private int _shieldStrength = 0;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private int _shieldStrength = 3;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;
    [SerializeField]
    private GameObject _rightShieldVisualizer, _leftShieldVisualizer;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _ammoCount = 15;

    private UIManager _uiManager;
    private bool _isThrusterActive = false;
    private float _thrusterSpeedMultiplier = 2;
    
   



    
    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponentInChildren<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount != 0)
        {
            FireLaser();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && _isThrusterActive == false)
        {
            EngageThrusters();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isThrusterActive = false;
            _speed /= _thrusterSpeedMultiplier;
        }
        

    


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed  * Time.deltaTime);
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0),0);

        if (transform.position.x > 11.4f)
        {
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.4f)
        {
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _ammoCount--;
        AmmoCount(_ammoCount);

        _canFire = Time.time + _fireRate;
        
        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
        _audioSource.Play();
        //play the laser audio clip
        
    }

    public void Damage()
    {
        
        if(_isShieldActive == true)
        {
            _shieldStrength--;

            switch (_shieldStrength)
            {
                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
                case 1:
                    _shieldRenderer.color = new Color(1f, 0f, 0f, .15f);
                    break;
                case 2:
                    _shieldRenderer.color = new Color(0f, 0f, 1f, .15f);
                    break;

            }
            return;
        }

        _lives--;
        
        if(_lives == 2)
        {
            _rightShieldVisualizer.SetActive(true);

        }
        else if(_lives == 1)
        {
            _leftShieldVisualizer.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);


        if(_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void EngageThrusters()
    {
        _isThrusterActive = true;
        _speed *= _thrusterSpeedMultiplier;

    }

    public void AmmoCount(int ammoCount)
    {
        _uiManager.UpdateAmmoCount(ammoCount);
    }


    public void AddtoScore(int points)
    {
    
        _score += points;
        _uiManager.UpdateScore(_score);
    
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        if (_isSpeedBoostActive == false)
        {
            _isSpeedBoostActive = true;
            _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldStrength = 3;
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    } 

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
}
