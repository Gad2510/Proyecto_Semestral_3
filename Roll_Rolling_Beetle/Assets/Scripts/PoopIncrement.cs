using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoopIncrement : MonoBehaviour
{
    Text screenPoints;
    
    [SerializeField]
    float bonusPoints=5;
    [SerializeField]
    float pointsMul=100f;
    public static float score = 0;
    public bool IsScaling=false;
    [SerializeField]
    Slider CacaPorcentage=null; // Slider UI para medir porcentaje

    [SerializeField]
    float scaleIncrement=0.01f, maxScale=2f; //Valores para maxima escala y ratio de inclemento

    [SerializeField]
    Personaje playerForward=null;//Referencia al player
    public SpawnPopoManager index;

    Vector3 InitScale;//Para guardar la escala inicial de la caca

    Vector3 sumV;//Vector para escalar la caca
    float diferencial;// Float para calcular la diferencia en tre escala actual y final

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        screenPoints = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
        InitScale = transform.localScale; 
        sumV = Vector3.one;
        diferencial = maxScale - transform.localScale.y;
        screenPoints.text = "SCORE: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerForward.IsMoving && transform.localScale.y<maxScale) //Revisa si esta en movimietno la caca y si no ha llegado a su maxima escala
        {
            transform.localScale += sumV*scaleIncrement*Time.deltaTime;  //Aumenta el tamaño
            CacaPorcentage.value = (transform.localScale.y - InitScale.y) / diferencial; //Modifica el valor en porcentaje
            score += scaleIncrement * Time.deltaTime * pointsMul;
            float screen=Mathf.Round(score);
            screenPoints.text ="SCORE: "+screen.ToString();
        }
    }

    public void RestartScale() //Reinicia los stats a los iniciales
    {
        transform.localScale = InitScale;
        CacaPorcentage.value = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bonus"))
        {
            Destroy(other.gameObject);
            index.poopNumber--;
            Corroboration();
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
