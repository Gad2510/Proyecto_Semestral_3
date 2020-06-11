using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBonus : MonoBehaviour
{
    [SerializeField]
    float Radius = 1.0f, altura=0f;

    [SerializeField]
    Object bonus=null;

    public bool has_bonus=false;
    SpawnCounter father;

    public void Start()
    {
        father = GetComponentInParent<SpawnCounter>();
    }
    public bool HasBonus
    {
        get
        {
            return has_bonus;
        }
        set
        {
            if (value)
            {
                generateBonus();
            }
            else
            {
                father.RemoveBonus();
            }
            has_bonus = value;
        }
    }

    public void generateBonus()
    {
        float degrees=(Random.Range(0f,360f))*Mathf.Deg2Rad; // Escoge una direcccion en forma de grados
        float magnitud=Random.Range(0f,Radius); //Define una distancia del centro

        float x = Mathf.Cos(degrees)*magnitud;//calcula el valor de x 
        float z = Mathf.Sin(degrees)*magnitud;//calcula el valor en z

        Vector3 pos = new Vector3(x+transform.position.x, altura, z+transform.position.z);//Crea un vector 3 para posicionar el objeto con el offset del padre

        GameObject.Instantiate(bonus, pos, Quaternion.identity, this.transform);//Crea el objeto emparentado con su spawner para cuestiones de prueba


    }

    public void deletePoop()//SOLO FUNCIONA EN EDITOR - Borra todos los hijos del spawner
    {
        for(int i = transform.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void OnDrawGizmos()//Para visualizar en el editor el espacio en el que las esferas van a ser creadas
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}
