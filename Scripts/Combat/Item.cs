using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public List<GunSettings> items;
    private GunSettings item;

    public int maxInstantiations;

    public Vector3 effectScale;
    public float effectDuration;

    bool isdestroying;

    public int remainingInstantiations;

    void Awake()
    {
        isdestroying = false;
        remainingInstantiations = maxInstantiations;
    }

    void destroyItemEffect()
    {
        isdestroying = true;
        transform.localScale = Vector3.Lerp(transform.localScale, effectScale, effectDuration * Time.deltaTime);
        Destroy(this, (1 / effectDuration) + effectDuration);
    }
    
    public GunSettings chooseItem()
    {
        if (!isdestroying) {
            int randItem = Random.Range(0, items.Count-1);
            item = items[randItem];
            return item;
        }
        return null;
    }

    void Update()
    {
        if (remainingInstantiations <= 0)
            destroyItemEffect();
    }

}
