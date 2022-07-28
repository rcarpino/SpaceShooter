using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioClip _explosionSoundClip;

    private AudioSource _audioSource;
    

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is NULL");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }
        
        
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }

        _anim = GetComponent<Animator>();
        if( _anim == null)
        {
            Debug.LogError("Animator is null");
        }
    }
    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
       
        if(transform.position.y < -5.4f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 7.3f, 0); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }

        if(other.tag == "Laser")
        {
            
            if(_player != null)
            {
                _player.AddtoScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            Destroy(other.gameObject);
            
        }
    }
}
