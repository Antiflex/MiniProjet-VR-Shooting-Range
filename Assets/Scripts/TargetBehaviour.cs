using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particulePC;
    public GameObject particuleMQ;
    private GameObject particule;

    void Start()
    {
#if UNITY_ANDROID
            particule = particuleMQ;
#else
        particule = particulePC;

#endif
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Bullet"))
        {
            GameObject explosion= Instantiate(particule);
            explosion.transform.position = transform.position;
            Destroy(explosion,0.75f);
            Destroy(gameObject);
            Destroy(other.gameObject);            
        }
    }
}
