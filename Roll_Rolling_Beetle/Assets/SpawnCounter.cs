using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCounter : MonoBehaviour
{
    public int counter = 0; //Cuantas popos ahi en este spawner
    SpawnBonus[] bonusChilds;
    // Start is called before the first frame update
    void Start()
    {
        bonusChilds = GetComponentsInChildren<SpawnBonus>();
    }

    public bool spawnInArea()
    {
        bool condition = counter < 4;
        if (!condition)
        {
            return condition;
        }

        int choice = Random.Range(0, bonusChilds.Length);

        bonusChilds[choice].generateBonus();

        counter++;
        
        return condition;
    }

    public void RemoveBonus()
    {
        counter--;
        CrearMapa.poopMnager.SpawnPoop();
    }
}
