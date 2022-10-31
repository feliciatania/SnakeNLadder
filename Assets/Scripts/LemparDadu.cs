using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LemparDadu : MonoBehaviour
{
    public Image DiceImg;
    public Sprite[] diceSidesSprites;
    int angkaDadu;
    bool islemparDaduOnce;
    // Start is called before the first frame update
    void Start()
    {
        islemparDaduOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void onRolltheDice()
    {
        if (islemparDaduOnce == false)
        {
            islemparDaduOnce = true;
            StartCoroutine(RollTheDice());
            this.Wait(3f, () =>
            {
                GameInstance.onLemparDadu?.Invoke();
                GameInstance.onMahasiswaMove?.Invoke(angkaDadu);
                islemparDaduOnce = false;
                DiceImg.sprite = diceSidesSprites[0];
            });
        }
    }

    IEnumerator RollTheDice()
    {
        int randomDiceSide = 0;
        for (int i = 0; i <= 30; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(0, 3);

            DiceImg.sprite = diceSidesSprites[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }
        angkaDadu = randomDiceSide + 1;
        //angkaDadu = 4;
    }
}
