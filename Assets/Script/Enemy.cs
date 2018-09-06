using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    // Use this for initialization
    [SerializeField] int Health=100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float ProjectileSpeed = 7.0f;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject dealthVfx;
    [SerializeField] float Timeduration=1f;
    [SerializeField] AudioClip deathSFx;
    [SerializeField] AudioClip ShootSFX;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
    [SerializeField] [Range(0, 1)]  float SoundVolume = 0.5f;

    void Start () {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update () {
        CountDownShoot();
	}

    private void CountDownShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
        }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -ProjectileSpeed);
        AudioSource.PlayClipAtPoint(ShootSFX, transform.position, SoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        damageDealer.Hit();
        Health -= damageDealer.getDamage();
        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        
        Destroy(gameObject);
        GameObject explosion = Instantiate(dealthVfx, transform.position, transform.rotation);
        Destroy(explosion, Timeduration);
        AudioSource.PlayClipAtPoint(deathSFx, Camera.main.transform.position, deathSoundVolume);
    }
}
