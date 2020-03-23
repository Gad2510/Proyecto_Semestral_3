using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

    }
    //Funcionpara cambiar a pel juego
      public  void Chargelevel(string level)
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
    }


}
