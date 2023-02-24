using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject impact;
    [SerializeField] private float damage;
    [SerializeField] private bool useGravity;
    [SerializeField] private bool allowBounce;
    [SerializeField] private float bounciness;
    [SerializeField] private int maxCollisons;
    [SerializeField] private GameObject bulletGraphics;
    [SerializeField] private float graphicsFollowSpeed;


    private GameObject currentGraphics;
    public Transform muzzlePoint;

    int collisions;
    private PhysicMaterial material;

    void Start()
    {
        SetUp();
    }

    void FixedUpdate()
    {
        if (collisions >= maxCollisons) DestroyThis();
        // bulletGraphics.transform.localPosition
        SpawnGraphics();
    }

    public void SpawnGraphics()
    {
        currentGraphics.transform.localPosition = Vector3.Lerp(currentGraphics.transform.localPosition, transform.localPosition, graphicsFollowSpeed * Time.deltaTime);

    }

    void SetUp()
    {
        material = new PhysicMaterial();
        rb.useGravity = useGravity;

        material.dynamicFriction = 0;
        material.staticFriction = 0;
        material.frictionCombine = PhysicMaterialCombine.Minimum;

        material.bounceCombine = allowBounce ? PhysicMaterialCombine.Maximum : PhysicMaterialCombine.Minimum;
        material.bounciness = Mathf.Clamp01(bounciness);

        GetComponent<Collider>().material = material;

        // spawn the bullet graphics
        currentGraphics = Instantiate(bulletGraphics, muzzlePoint.position, Quaternion.identity);
        currentGraphics.transform.forward = transform.forward;
    }

    void OnCollisionEnter(Collision collider)
    {
        Hit(collider.transform);
        collisions++;
    }

    void Hit(Transform hitTransform)
    {
        if (collisions >= maxCollisons) DestroyThis();
        Damagable target = hitTransform.GetComponent<Damagable>();

        if (target != null)
        {
            target.TakeDamage(damage);
            DestroyThis();
        }
        else if (allowBounce == false)
        {
            DestroyThis();
        }
    }

    void DestroyThis()
    {
        Instantiate(impact, transform.position, Quaternion.identity);
        Destroy(currentGraphics);
        Destroy(gameObject);
        //print("Destroy position" + transform.position);
    }
}
