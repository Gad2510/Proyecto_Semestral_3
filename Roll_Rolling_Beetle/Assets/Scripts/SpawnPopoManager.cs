using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPopoManager : MonoBehaviour
{

    [SerializeField]
    int maxNUM=20;

    SpawnBonus [] spawnersInScene;

    public void Active()
    {
        spawnersInScene = GameObject.FindObjectsOfType<SpawnBonus>(); // Obtiene la referencia de todos los spawners 
        StartPoop();

    }
    // Update is called once per frame
    void Update()
    {
        /*if(active && poopNumber< maxNUM)//Verifica que este el numero maximo de bonus en el juego siempre
        {
            SpawnPoop();
            
        }*/
    }

    public void UpdateSpawnersScene() //En caso de volver a empezar la escena
    {
        spawnersInScene = GameObject.FindObjectsOfType<SpawnBonus>(); // Obtiene la referencia de todos los spawners 
    }

    private void StartPoop()
    {
        for(int i=0; i< maxNUM; i++){//Crea todos los bonus 
            SpawnPoop();
        }
    }
    
    public void SpawnPoop()
    {
        int randomChoice = Random.Range(0, spawnersInScene.Length);//Escoge un spawner al azar para crear un objeto

        spawnersInScene[randomChoice].generateBonus();//Activa la funcion de spawn dentro del spawner
    }

}
