using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] int damage;
    [SerializeField] float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    [SerializeField] int magazineSize, bulletsPerTap;
    [SerializeField] bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // Bools 
    bool shooting, readyToShoot, reloading;

    [Header("Aim Settings")]
    [SerializeField] Vector3 aimDownSight;
    [SerializeField] Vector3 hipFire;
    [SerializeField] float aimSpeed;

    [Header("Controls")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;
    [SerializeField] KeyCode aimKey = KeyCode.Mouse1;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    //public GameObject bulletHoleGraphic;
    public TMP_Text ammoText;

    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;  
    public GunRecoil recoilScript;

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //SetText
        ammoText.SetText(bulletsLeft + " / " + magazineSize);    
    }

    void MyInput() 
    {
        //Input for our Shoot Button
        if (allowButtonHold)
        {
            shooting = Input.GetKey(shootKey);
        }    
        else
        {
            shooting = Input.GetKeyDown(shootKey);
        }
        
        //Input when reloading
        if(Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        //When ready to shoot
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

        //When aiming
        AimDown();
            
    }

    void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if(Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);

            //Deals Damage to the Target
            rayHit.collider.GetComponent<Target>().TakeDamage(damage);
        }

        // Graphics
        //GameObject impactBulletHole = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        GameObject impactMuzzle = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Destroy(impactMuzzle, 0.1f);

        // Recoil
        recoilScript.RecoilFire();

        bulletsLeft--;
        bulletsShot--;

        if(!IsInvoking("ResetShot") && !readyToShoot)
        {
            Invoke("ResetShot", timeBetweenShooting);
        }

        

        //To shoot more bullets at once
        if(bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    void AimDown()
    {
        if(Input.GetKey(aimKey))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimDownSight, aimSpeed * Time.deltaTime);
        }

        if(Input.GetKeyUp(aimKey))
        {
            transform.localPosition = hipFire;
        }
    }

    // Aim down Notes:
    // MP5: Aim: 0, 0.463, 0.323 Hip: 0.64, 0.28, 1.0589
}
