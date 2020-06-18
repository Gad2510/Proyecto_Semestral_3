using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCounter : MonoBehaviour
{
    public static int maxNumPerArea;
    public int counter = 0; //Cuantas popos ahi en este spawner
    SpawnBonus[] bonusChilds;
    // Start is called before the first frame update
    void Start()
    {
        bonusChilds = GetComponentsInChildren<SpawnBonus>();//Obtiene todos los spawners en el
    }

    public bool spawnInArea()
    {
        bool condition = counter < maxNumPerArea;//Verifica si ya exedio e limite
        if (!condition)
        {
            return condition;
        }

        int choice = Random.Range(0, bonusChilds.Length); //Selecciona el hijo para spawnear
        bool fill= bonusChilds[choice].HasBonus;//Checa si tiene un espacio para el bonus
        for (int i = 0; i < bonusChilds.Length && fill; i++)
        {
            choice = (choice < bonusChilds.Length - 1) ? choice + 1 : 0;//Canbia el index del choice
            fill =bonusChilds[choice].HasBonus;
        }

        bonusChilds[choice].HasBonus=true;//Cambia su estado a que tiene un bonus y lo genera en el script

        counter++;
        
        return condition;
    }

    public void RemoveBonus()//Quita el bonus del contador
    {
        counter--;
        CrearMapa.poopMnager.SpawnPoop();
    }
}
