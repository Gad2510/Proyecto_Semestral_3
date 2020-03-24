using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage _instance;
    // Start is called before the first frame update
    void Start()
    {
        if(_instance!= this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ChangeLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    //Funcionpara cambiar a pel juego
     /* public  void Chargelevel(string level)
    {
        SceneManager.LoadScene(1);
    }


      public void ChargeOptions(string options)
      {
        SceneManager.LoadScene(2);
      }

    public void ChargeMenu(string Menu)
    {
        SceneManager.LoadScene(0);
    }

    public void ChargeOver(string GameOver)
    {
        SceneManager.LoadScene(3);
    }*/


}
