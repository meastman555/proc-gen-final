using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public functions are called from UserInput.cs
public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    
    private Transform childCamera;
    private Quaternion staticCameraRotation;

    void Start()
    {
        //0 can be hardcoded since camera is first child in the prefab
        childCamera = transform.GetChild(0);
        staticCameraRotation = childCamera.transform.rotation;
    }

    //code adapted from this Unity forum: https://answers.unity.com/questions/731922/rotate-object-to-face-mouse-2d-rotate-on-z-axis.html
    //all the 3d rotation stuff I was trying with Quarternions and LookAt (based on my knowledge of cameras from graphics) was wonky when trying to go back to Unity 2d
    //so I opted to use this solution that is all over the internet (the camera rotation save/restore is my own addition)
    public void AimAtMouse() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //we don't want the camera to rotate with the player, but it's a child, so just need to restore it's saved rotation (which is static) each time
        childCamera.transform.rotation = staticCameraRotation;
    }

    public void FireShot() {
        //TODO: rotation is 90 degrees off, but I don't think it's as simple as adding to the z in quarternion?
        GameObject instantiatedBullet = Instantiate(bulletPrefab, bulletOrigin.position, transform.rotation);
        instantiatedBullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
    }
}
