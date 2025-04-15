using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ERT_WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] float fireRate;
    [SerializeField] float shotSpeed;
    private float shotCounter;

    [Header("Weapon Related References")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Animator anim;


    private void Update()
    {
        if (playerInput.actions["Fire"].ReadValue<float>() > 0)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = fireRate;
                shoot();
            }
            anim.SetBool("isFiring", true);
        }
        else
        {
            shotCounter = 0;
            anim.SetBool("isFiring", false);
        }
    }

    private void shoot()
    {
        int playerDir()
        {
            if(gameObject.transform.localScale.x < 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        GameObject GO = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>();
        rb.AddForce(muzzlePosition.right * shotSpeed * playerDir(), ForceMode2D.Impulse);
        Destroy(GO, 2f);
    }

}
