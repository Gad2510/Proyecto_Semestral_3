using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPopoManager : MonoBehaviour
{

    [SerializeField]
    int maxNUM=30;

    SpawnCounter[] spawnersInScene;

    private void Start()
    {

    }
    public void Active()
    {
        SpawnCounter.maxNumPerArea = (maxNUM / 9) + 1;
        spawnersInScene = GameObject.FindObjectsOfType<SpawnCounter>(); // Obtiene la referencia de todos los spawners 
        StartPoop();

    }

    public void UpdateSpawnersScene() //En caso de volver a empezar la escena
    {
        spawnersInScene = GameObject.FindObjectsOfType<SpawnCounter>(); // Obtiene la referencia de todos los spawners 
    }

    private void StartPoop()
    {
        for(int i=0; i< maxNUM; i++){//Crea todos los bonus 
            SpawnPoop();
        }
    }
    
    public void SpawnPoop(List<int> areasCheck =null)
    {
        if (areasCheck == null)
        {
            areasCheck = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }
        else if (areasCheck.Count == 0)
        {
            return;
        }

        int randomChoice = UnityEngine.Random.Range(0, areasCheck.Count);//Escoge un spawner al azar para crear un objeto
        if(!spawnersInScene[areasCheck[randomChoice]].spawnInArea())//Activa la funcion de spawn dentro del spawner
        {
            areasCheck.Remove(areasCheck[randomChoice]);
            SpawnPoop(areasCheck);
        }
        else
        {
            return;
        }
    }

}
