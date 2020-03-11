using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPopo : MonoBehaviour
{

    [SerializeField]
    int maxNUM;
    [SerializeField]
    Vector2 maxSpawnpoint;
    [SerializeField]
    Object prefaPopo;

    Vector3 origin;


    public int poopNumber;
    // Start is called before the first frame update
    void Start()
    {
        StartPoop();
    }

    // Update is called once per frame
    void Update()
    {
        if(poopNumber< maxNUM)
        {
            SpawnPoop();
            
        }
    }

    private void StartPoop()
    {
        for(byte i=0; i< maxNUM; i++){
            SpawnPoop();
        }
    }
    
    private void SpawnPoop()
    {
        Vector3 pos;
        pos.y = 0f;
        pos.x = Random.Range(-maxSpawnpoint.x, maxSpawnpoint.x);
        pos.z = Random.Range(-maxSpawnpoint.y, maxSpawnpoint.y);

        Instantiate(prefaPopo, pos, Quaternion.identity);
        poopNumber++;
    }

}
