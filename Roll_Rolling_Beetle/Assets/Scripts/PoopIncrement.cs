using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoopIncrement : MonoBehaviour
{
    public float separacion;
    public Text screenPoints;

    [SerializeField]
    float bonusPoints = 20;
    [SerializeField]
    float pointsMul = 100f;
    public static float score = 0;
    public bool IsScaling = false;

    //UI
    public Slider CacaPorcentage; // Slider UI para medir porcentaje
    public Text gusanoK; //Texto de que puedes matar al gusano
    public Text mantisK;// Texto de matar mantis
    public Color color1;
    public Color color2;
    float alpha; //Transparencia
    float alpha2;
    bool desactivarG = false; //Booleano para desactivar los textos una vez se usen
    bool desactivarM = false;
    public float speed;
    


    [SerializeField]
    float scaleIncrement = 0.01f, maxScale = 2f; //Valores para maxima escala y ratio de inclemento

    Vector3 InitScale;//Para guardar la escala inicial de la caca

    Vector3 sumV;//Vector para escalar la caca
    float diferencial;// Float para calcular la diferencia en tre escala actual y final

    // Start is called before the first frame update
    void Start()
    {
        CacaPorcentage = GameObject.FindGameObjectWithTag("UI").transform.Find("Slider").GetComponent<Slider>();
        screenPoints = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
        float screen = Mathf.Round(score);
        screenPoints.text = "SCORE: " + screen.ToString();

        InitScale = this.transform.localScale;
        sumV = Vector3.one;
        diferencial = maxScale - transform.localScale.y;
        if (gusanoK != null)
            color1 = gusanoK.color;
        if(mantisK != null)
            color2 = mantisK.color;

        alpha = 0;
        alpha2 = 0;
    }

    private void Update()
    {
        showTextG();
         if(desactivarG == true)
        {
            gusanoK.color = new Color(0, 0, 0, 0);
        }
         showTextM();
        if (desactivarM == true)
        {
            mantisK.color = new Color(0, 0, 0, 0);
        }
    }


    public bool AddScore()
    {
        bool state = Personaje.IsMoving && transform.localScale.y < maxScale;
        if (state) //Revisa si esta en movimietno la caca y si no ha llegado a su maxima escala
        {
            transform.localScale += sumV * scaleIncrement * Time.deltaTime;  //Aumenta el tamaño
            //Mover un poco hacia el frente
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + separacion);

            score++;
            CacaPorcentage.value = (transform.localScale.y - InitScale.y) / diferencial; //Modifica el valor en porcentaje
            float screen = Mathf.Round(score);
            screenPoints.text = "SCORE: " + screen.ToString();
        }

        return state;
    }
    public void RestartScale() //Reinicia los stats a los iniciales
    {
        transform.localScale = InitScale;
        CacaPorcentage.value = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonus"))
        {
            Destroy(other.gameObject);
            CrearMapa.poopMnager.SpawnPoop();
            Corroboration();
        }
    }

    public void Decrement()//Cuando es golpeado por el gusano
    {
        float points = 20;

        float espontaniusDecrement = (points / pointsMul);

        if ((transform.localScale.y- espontaniusDecrement) < InitScale.y)
        {
            espontaniusDecrement = InitScale.y;
        }
        transform.localScale -=sumV* espontaniusDecrement;  //decremento tamaño el tamaño
        score -= points;

        if (score < 0)
        {
            score = 0f;
        }

        float screen = Mathf.Round(score);
        screenPoints.text = "SCORE: " + screen.ToString();
    }
    
    public  void showTextG() // Vamos a mostrar el texto del gusano
    {
        if (gusanoK == null)
            return;
        if (CacaPorcentage.value >=0.38f && CacaPorcentage.value <= 0.45f)
        {
            alpha += Time.deltaTime * speed; //Aparecemos en deltatime el texto
            if(alpha >= 1)
            {
                alpha = 1;
            }
            gusanoK.color = new Color(color1.r, color1.g, color1.b, alpha);
        }
        else if(CacaPorcentage.value >= 0.5f) // Despues de tanto porcentaje se devanece
        {
            alpha -= Time.deltaTime * speed;
            if (alpha <= 0)
            {
                alpha = 0;
            }
            desactivarG = true; //Una vez ya pase su tiempo se desactivara
            gusanoK.color = new Color(color1.r, color1.g, color1.b, alpha);

        }

    }
     public void showTextM()
    {
        if (mantisK == null)
            return;
        if (CacaPorcentage.value >= 0.68f && CacaPorcentage.value <= 0.78f)
        {
            alpha2 += Time.deltaTime * speed;
            if (alpha2 >= 1)
            {
                alpha2 = 1;
 
            }
            mantisK.color = new Color(color2.r, color2.g, color2.b, alpha2);
        }


        else if (CacaPorcentage.value >= 0.75) // Despues de tanto porcentaje se devanece
        {
            alpha2 -= Time.deltaTime * speed;
            if (alpha2 <= 0)
            {
                alpha2 = 0;
                
            }
            desactivarM = true; //Se hace true para que ya no vuelva a aparecer
            mantisK.color = new Color(color2.r, color2.g, color2.b, alpha2);
        }
    }

    private void Corroboration()
    {
        if (transform.localScale.y <= maxScale)
        {
            StopAllCoroutines();
        }

        float espontaniusIncrement = (bonusPoints / pointsMul);

        if ((espontaniusIncrement + transform.localScale.y) > maxScale)
            espontaniusIncrement -= (espontaniusIncrement + transform.localScale.y) - maxScale;

        if (IsScaling)
        {
            
            transform.localScale += sumV * espontaniusIncrement;
            score += espontaniusIncrement * pointsMul;
            float screen = Mathf.Round(score);
            screenPoints.text = "SCORE: " + screen.ToString();
            IsScaling = false;
            StopAllCoroutines();
        }

        IsScaling = true;
        StartCoroutine(ScalePoop(espontaniusIncrement));
    }

    private IEnumerator ScalePoop(float espontaniusIncrement)
    {
        float counter=0f;

        while (counter< espontaniusIncrement)
        {
            transform.localScale += sumV * scaleIncrement * Time.deltaTime*2;  //Aumenta el tamaño
            score += scaleIncrement * Time.deltaTime * pointsMul*2;
            float screen = Mathf.Round(score);
            screenPoints.text = "SCORE: " + screen.ToString();
            counter += Time.deltaTime*2;
            yield return null;
        }

        IsScaling = false;
    }
}
