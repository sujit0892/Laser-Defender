using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float speed = 10f;

    [SerializeField] float padding = 1f;
  
    [SerializeField] float ProjectileSpeed =10f;

    [SerializeField] GameObject LaserPreFab;
    [SerializeField] float projectileFiringPeriod = 0.5f;
    [SerializeField] float Health = 100f;
    Coroutine FireCoroutine;

    float xMax;
    float xMin;
    float yMax;
    float yMin;

    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float SoundVolume = 0.5f;
    [SerializeField] AudioClip deathSFx;
    [SerializeField] AudioClip ShootSFX;
    // Use this for initialization
    void Start () {
        SetUpMoveBoundary();
        
	}

    private void SetUpMoveBoundary()
    {
        Camera gameCamera = Camera.main;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
    }

    // Update is called once per frame
    void Update () {
        Move();
        Fire();
	}

    private void Fire()
    {
       if(Input.GetButtonDown("Fire1"))
        {
            FireCoroutine = StartCoroutine(FireContinously());
        }
       if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(FireCoroutine);
        }
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal")*Time.deltaTime*speed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime*speed;

        var newPos = Mathf.Clamp(transform.position.x + deltaX,xMin,xMax);
        var newPosY = Mathf.Clamp(transform.position.y + deltaY,yMin,yMax);
        
        transform.position = new Vector2(newPos, newPosY);
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(LaserPreFab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed);
            AudioSource.PlayClipAtPoint(ShootSFX, transform.position, SoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        //if (!damageDealer) { return; }
        Health -= damageDealer.getDamage();
        damageDealer.Hit();
        if (Health <= 0)
            Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFx, Camera.main.transform.position, deathSoundVolume);
    }
} 
