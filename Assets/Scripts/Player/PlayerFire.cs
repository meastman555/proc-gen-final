using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    public void FireShot() {
        //TODO: rotation is 90 degrees off, but I don't think it's as simple as adding to the z in quarternion
        GameObject instantiatedBullet = Instantiate(bulletPrefab, muzzle.position, transform.rotation);
        instantiatedBullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
    }
}
