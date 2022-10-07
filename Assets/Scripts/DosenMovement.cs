using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DosenMovement : MonoBehaviour
{
    int curPosDos;
    public GameObject go_dosen;
    public Image DosenImg;
    public Sprite[] DosenSprites;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;

    void Start()
    {
        GameInstance.onDosenMove += onDosenMove;
        KUT = GameObject.Find("Panel Papan").GetComponent<KotakUlarTangga>();
        kotak = KUT.kotak;
        GameInstance.onGameOver += onGameOver;
    }

    void Update()
    {
        
    }

    public void onGameOver(bool b)
    {
        gameover = true;
    }

    public void onDosenMove()
    {
        if(!gameover)
        {
            this.Wait(1f, () =>
            {
                StartCoroutine(MoveDosen());
            });
        }
    }

    IEnumerator MoveDosen()
    {
        curPosDos++;
        float x = go_dosen.transform.localPosition.x;
        float y = go_dosen.transform.localPosition.y;
        Vector2 StartPosDosen = new Vector2(x, y);
        Vector2 EndPosDosen;
        float time = 0;
        float duration = 1.5f;

        StartPosDosen = go_dosen.transform.position;
        if (curPosDos == 4)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (curPosDos == 8)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        StartPosDosen = go_dosen.transform.position;
        while (time < duration)
        {
            if (curPosDos >= 4 && curPosDos <= 7)
                x = 30;
            else
                x = -30;
            x *= Screen.width / 1024f;
            y = 15;
            EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y); //ditambah dengan posisi tile target
            go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, time / duration); //diganti jadi world position karena masalah sort rendering
            time += Time.deltaTime;
            yield return null;
        }

        if (curPosDos == 11)
        {
            GameInstance.onGameOver?.Invoke(false);
        }
        else
        {
            GameInstance.onGiliranMahasiswa?.Invoke();
        }

    }
}
