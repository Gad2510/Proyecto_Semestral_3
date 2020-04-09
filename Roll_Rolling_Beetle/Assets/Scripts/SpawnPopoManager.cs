using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPopoManager : MonoBehaviour
{

    [SerializeField]
    int maxNUM=20;

    SpawnBonus [] spawnersInScene;

    public int poopNumber;
    // Start is called before the first frame update
    void Start()
    {
        spawnersInScene = GameObject.FindObjectsOfType<SpawnBonus>(); // Obtiene la referencia de todos los spawners 
        StartPoop();
    }

    // Update is called once per frame
    void Update()
    {
        if(poopNumber< maxNUM)//Verifica que este el numero maximo de bonus en el juego siempre
        {
            SpawnPoop();
            
        }
    }

    private void StartPoop()
    {
        for(byte i=0; i< maxNUM; i++){//Crea todos los bonus 
            SpawnPoop();
        }
    }
    
    private void SpawnPoop()
    {
        int randomChoice = Random.Range(0, spawnersInScene.Length);//Escoge un spawner al azar para crear un objeto

        spawnersInScene[randomChoice].generateBonus();//Activa la funcion de spawn dentro del spawner

        poopNumber++;
    }

}
