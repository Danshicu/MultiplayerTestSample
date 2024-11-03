using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;

    [Header("Settings")] 
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;

    private float _previousFireTime;
    private float _muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        inputReader.PrimaryFireEvent += Fire;
    }

    private void Fire(bool shouldFire)
    {
        if (!IsOwner) return;
        
        if(Time.time < 1/fireRate + _previousFireTime) return;
        
        SpawnProjectileServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        
        _previousFireTime = Time.time;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        inputReader.PrimaryFireEvent -= Fire;
    }

    private void SpawnProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        _muzzleFlashTimer = muzzleFlashDuration;
        GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }

    [ServerRpc]
    private void SpawnProjectileServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
        SpawnProjectileClientRpc(spawnPos, direction);
    }
    
    [ClientRpc]
    private void SpawnProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) return;
        SpawnProjectile(spawnPos, direction);
    }

   
    void Start()
    {
        
    }

   
    void Update()
    {
        if (_muzzleFlashTimer > 0f)
        {
            _muzzleFlashTimer-=Time.deltaTime;
            if (_muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }
    }
}
