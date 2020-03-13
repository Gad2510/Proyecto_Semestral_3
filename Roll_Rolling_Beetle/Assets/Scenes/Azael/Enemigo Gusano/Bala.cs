using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float vel;
    public Transform rastro;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destructor", 10);
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * vel * Time.deltaTime);
        rastro.localScale = new Vector3(rastro.localScale.x, rastro.localScale.y, rastro.localScale.z + Time.deltaTime * (vel * 3.5f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void Destructor()
    {
        Destroy(gameObject);
    }
}
