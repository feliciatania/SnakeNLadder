using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DosenMovement : MonoBehaviour
{
    [SerializeField]
    public int curPosDos;
    public GameObject go_dosen;
    public Image DosenImg;
    //public Sprite[] DosenSprites;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;
    int xPos = 35;
    int yPos = 50;
    int curPosMhs;
    public GameObject go_mhs;

    private Transform[] routes;
    private int routeToGo;
    private float tParam;
    private Vector2 dosenPosition;
    private float speedModifier;

    void Awake()
    {
        KUT = GameObject.Find("Panel Papan").GetComponent<KotakUlarTangga>();
        kotak = KUT.kotak;
        GameInstance.onGameStart += onStart;
    }
    void Start()
    {
        GameInstance.onDosenMove += onDosenMove;
        GameInstance.onGameOver += onGameOver;
        GameInstance.onGameStart += onStart;

        //LOMPAT
        routeToGo = 0;
        tParam = 0;
        speedModifier = 0.75f;

    }

    void Update()
    {
        
    }

    public void onGameOver(bool b)
    {
        gameover = true;
    }

    public void onStart()
    {
        Debug.Log("masuk dosen");
        curPosDos = 0;
        go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        go_dosen.transform.position = new Vector2(kotak[curPosDos].transform.position.x - xPos, kotak[curPosDos].transform.position.y + yPos);
        
    }

    public void onDosenMove()
    {
        if(!gameover)
        {
            this.Wait(1f, () =>
            {
                StartCoroutine(JumpDosen());
            });
            
        }
    }

    public int getCurPosDosen()
    {
        return curPosDos;
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
            go_dosen.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (curPosDos == 8)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }

        StartPosDosen = go_dosen.transform.position;
        while (time < duration)
        {
            if (curPosDos >= 1 && curPosDos <= 3)
            {
                x = - xPos;
                y = yPos;
            }
            if (curPosDos >= 4 && curPosDos <= 7)
            {
                x = xPos - 5;
                y = yPos - 10;
            }
            else if (curPosDos >= 8)
            {
                x = - xPos + 10;
                //y = 50 - 20;
                y = yPos;
            }
            x *= Screen.width / 1024f;
            y *= Screen.height / 576f;
            EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y * 2); //ditambah dengan posisi tile target
            go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, time / duration); //diganti jadi world position karena masalah sort rendering
            time += Time.deltaTime;
            //if (curPosDos == 4 && go_mhs.transform.localScale.x >= 0.75f)
            //{
            //    go_mhs.transform.localScale += new Vector3(-0.0001f, -0.0001f, 1);
            //}
            //else if (curPosDos == 8 && go_mhs.transform.localScale.x >= 0.5f)
            //{
            //    go_mhs.transform.localScale += new Vector3(-0.0001f, -0.0001f, 1);
            //}
            yield return null;
        }
        //if (curPosDos == 4)
        //{
        //    Vector3 scale = go_dosen.transform.localScale;

        //    scale.Set(0.75f, 0.75f, 0);

        //    go_dosen.transform.localScale = scale;
        //}
        //else if (curPosDos == 8)
        //{
        //    Vector3 scale = go_dosen.transform.localScale;

        //    scale.Set(0.5f, 0.5f, 0);

        //    go_dosen.transform.localScale = scale;
        //}
        curPosMhs = go_mhs.GetComponent<MahasiswaMovement>().getCurPosMhs();
        if (curPosDos == curPosMhs)
        {
            GameInstance.onDosenMarah?.Invoke(true);
            this.Wait(1.5f, () =>
            {
                GameInstance.onDosenMarah?.Invoke(false);
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranMahasiswa?.Invoke();
                });
            });
        }
        else
        {
            this.Wait(0.5f, () =>
            {
                if (curPosDos == 11)
                {
                    GameInstance.onGameOver?.Invoke(false);
                }
                else
                {
                    GameInstance.onGiliranMahasiswa?.Invoke();
                }
            });
        }
       

    }

    IEnumerator JumpDosen()
    {
        curPosDos++;
        float x = go_dosen.transform.localPosition.x;
        float y = go_dosen.transform.localPosition.y;
        if (curPosDos >= 1 && curPosDos <= 3)
        {
            x = -xPos;
            y = yPos;
        }
        if (curPosDos >= 4 && curPosDos <= 7)
        {
            x = xPos - 5;
            y = yPos - 10;
        }
        else if (curPosDos >= 8)
        {
            x = -xPos + 10;
            //y = 50 - 20;
            y = yPos;
        }
        x *= Screen.width / 1024f;
        y *= Screen.height / 576f;

        if (curPosDos == 4)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (curPosDos == 8)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }

        Vector2 p0 = go_dosen.transform.position;
        Vector2 p1 = new Vector2((go_dosen.transform.position.x + kotak[curPosDos].transform.position.x + x)/2, go_dosen.transform.position.y + y);
        if (curPosDos == 4 || curPosDos == 8)
        {
            p1 = new Vector2(go_dosen.transform.position.x, kotak[curPosDos].transform.position.y + y);
        }
        Vector2 p2 = new Vector2 (kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y);

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            dosenPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

            transform.position = dosenPosition;
            yield return new WaitForEndOfFrame();

        }

        tParam = 0f;

        curPosMhs = go_mhs.GetComponent<MahasiswaMovement>().getCurPosMhs();
        if (curPosDos == curPosMhs)
        {
            GameInstance.onDosenMarah?.Invoke(true);
            this.Wait(1.5f, () =>
            {
                GameInstance.onDosenMarah?.Invoke(false);
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranMahasiswa?.Invoke();
                });
            });
        }
        else
        {
            this.Wait(0.5f, () =>
            {
                if (curPosDos == 11)
                {
                    GameInstance.onGameOver?.Invoke(false);
                }
                else
                {
                    GameInstance.onGiliranMahasiswa?.Invoke();
                }
            });
        }

    }


}
