using UnityEngine;

public class GunRecoil : MonoBehaviour
{

    [Header("Rotations")]
    Vector3 currentRotation;
    Vector3 targetRotation;

    [Header("Hipfire Recoil")]
    [SerializeField] float recoilX = -2f;
    [SerializeField] float recoilY = 2f;
    [SerializeField] float recoilZ = 0.35f;

    [Header("Settings")]
    [SerializeField] float snappieness = 6f;
    [SerializeField] float returnSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappieness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
