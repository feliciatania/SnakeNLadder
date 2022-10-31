using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KotakUlarTangga;
using static DosenMovement;
using Unity.Mathematics;

public class MahasiswaMovement : MonoBehaviour
{
    public int curPosMhs;
    public GameObject go_mhs;
    public Image MahasiswaImg;
    //public Sprite[] MahasiswaSprites;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;
    int xPos = 35;
    int yPos = 50;
    int curPosDos;
    float x;
    float y;
    public GameObject go_dosen;

    private Transform[] routes;
    private int routeToGo;
    private float tParam;
    private Vector2 MahasiswaPosition;
    private float speedModifier;

    void Awake()
    {
        KUT = GameObject.Find("Panel Papan").GetComponent<KotakUlarTangga>();
        kotak = KUT.kotak;
        GameInstance.onGameStart += onStart;
    }

    void Start()
    {
       
        GameInstance.onMahasiswaMove += onMahasiswaMove;
        GameInstance.onMahasiswaMoveOnKartu += onMahasiswaMoveOnKartu;
        
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

    public int getCurPosMhs()
    {
        return curPosMhs;
    }

    public void onGameOver(bool b)
    {
        gameover = true;
    }

    public void onGameStart()
    {
        Debug.Log("masuk mhs");
        curPosMhs = 0;
        go_mhs.transform.position = new Vector2(kotak[curPosMhs].transform.position.x + xPos, kotak[curPosMhs].transform.position.y + yPos);
    }
    public void onStart()
    {
        Debug.Log("masuk mhs");
        curPosMhs = 0;
        go_mhs.transform.position = new Vector2(kotak[curPosMhs].transform.position.x + xPos, kotak[curPosMhs].transform.position.y + yPos);

        //if(curPosMhs == 0)
        //{
        //    MahasiswaImg.rectTransform.localScale = new Vector3(1, 1, 1);
        //}
    }

    void onMahasiswaMove(int n)
    {
        if(!gameover)
        {
            this.Wait(0.5f, () =>
            {
                StartCoroutine(JumpMahasiswa(n));
            });
        }
    }

    void onMahasiswaMoveOnKartu(int n)
    {
        this.Wait(0.5f, () =>
        {
            StartCoroutine(JumpMahasiswaKartuOnPoisitifNegatif(n));
        });

    }

    IEnumerator MoveMahasiswa(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            curPosMhs++;

            Vector2 StartPosMhs = go_mhs.transform.position;
            float x = kotak[curPosMhs].transform.position.x + 30;
            float y = kotak[curPosMhs].transform.position.y + 10;
            Vector2 EndPosMhs = new Vector2(x, y);
            float time = 0;
            float duration = 1.5f;

            StartPosMhs = go_mhs.transform.position; //diganti jadi position karena masalah sort rendering
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            while (time < duration)
            {
                if (curPosMhs >= 1 && curPosMhs <= 3)
                {
                    x = xPos;
                    y = yPos;
                }
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    x = -xPos + 5;
                    y = yPos - 10;
                }  
                else if (curPosMhs >= 8)
                {
                    x = xPos - 10;
                    //y = yPos - 20;
                    y = yPos;
                }   
                x *= Screen.width / 1024f;
                y *= Screen.height / 576f;
                EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); // ditambahin posisi di tile target
                go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration); //diganti jadi position karena masalah sort rendering
                time += Time.deltaTime;
                //if (curPosMhs == 4 && go_mhs.transform.localScale.x >= 0.75f)
                //{
                //    go_mhs.transform.localScale += new Vector3(-0.0001f, -0.0001f, 1);
                //}
                //else if (curPosMhs == 8 && go_mhs.transform.localScale.x > 0.5f)
                //{
                //    go_mhs.transform.localScale += new Vector3(-0.0001f, -0.0001f, 1);
                //}
                yield return null;
            }

            if (curPosMhs == 11)
            {
                break;
            }
        }
        //if (curPosMhs == 4)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.75f, 0.75f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        //else if (curPosMhs == 8)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.5f, 0.5f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        if (curPosMhs == 11)
        {
            this.Wait(0.5f, () =>
            {
                GameInstance.onGameOver?.Invoke(true);
            });
            
        }
        else
        {
            checkPositifOrNegatif();
        }
         

    }

    void checkPositifOrNegatif()
    {
        
        if (curPosMhs == 2 | curPosMhs == 7)
        {
            GameInstance.onKartuPositif?.Invoke(curPosMhs);
        }
        else if (curPosMhs == 4 | curPosMhs == 9)
        {
           GameInstance.onKartuNegatif?.Invoke(curPosMhs);
        }
        else
        {
            curPosDos = go_dosen.GetComponent<DosenMovement>().getCurPosDosen();
            if (curPosDos == curPosMhs)
            {
                GameInstance.onDosenMarah?.Invoke(true);
                this.Wait(1.5f, () =>
                {
                    GameInstance.onDosenMarah?.Invoke(false);
                    this.Wait(0.5f, () =>
                    {
                        GameInstance.onGiliranDosen?.Invoke();
                    });
                });
            }
            else
            {
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranDosen?.Invoke();
                });
            }
        }
    }

    IEnumerator MoveMahasiswa2(int n)
    {
        if (n > 0)
        {
            for (int i = 1; i <= n; i++)
            {
                curPosMhs++;

                Vector2 StartPosMhs = go_mhs.transform.position;
                float x = kotak[curPosMhs].transform.position.x + 30;
                float y = kotak[curPosMhs].transform.position.y + 10;
                Vector2 EndPosMhs = new Vector2(x, y);
                float time = 0;
                float duration = 1.5f;

                StartPosMhs = go_mhs.transform.position; //ganti pakai position aja karena masalah sort rendering
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                while (time < duration)
                {
                    if (curPosMhs >= 1 && curPosMhs <= 3)
                    {
                        x = xPos;
                        y = yPos;
                    }
                    if (curPosMhs >= 4 && curPosMhs <= 7)
                    {
                        x = -xPos + 5;
                        y = yPos - 10;
                    }
                    else if (curPosMhs >= 8)
                    {
                        x = xPos - 10;
                        //y = yPos - 20;
                        y = yPos;
                    }
                    x *= Screen.width / 1024f;
                    y *= Screen.height / 576f;
                    EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
                    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration); //ganti pakai position aja karena masalah sort rendering
                    time += Time.deltaTime;

                    //if (curPosMhs == 4 && go_mhs.transform.localScale.x >= 0.75f)
                    //{
                    //    go_mhs.transform.localScale += new Vector3(-0.0001f, -0.0001f, 1);
                    //}
                    //else if (curPosMhs == 8 && go_mhs.transform.localScale.x >= 0.5f)
                    //{
                    //    go_mhs.transform.localScale += new Vector3(-0.0001f, -0.0001f, 1);
                    //}

                    yield return null;
                }
            }
        }
        else if (n < 0)
        {
            for (int i = n; i < 0; i++)
            {
                curPosMhs--;

                Vector2 StartPosMhs = go_mhs.transform.position;
                float x = kotak[curPosMhs].transform.position.x + 30;
                float y = kotak[curPosMhs].transform.position.y + 10;
                Vector2 EndPosMhs = new Vector2(x, y);
                float time = 0;
                float duration = 1.5f;

                StartPosMhs = go_mhs.transform.position;
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
                while (time < duration)
                {
                    if (curPosMhs >= 1 && curPosMhs <= 3)
                    {
                        x = xPos;
                        y = yPos;
                    }
                    if (curPosMhs >= 4 && curPosMhs <= 7)
                    {
                        x = -xPos + 5;
                        y = yPos - 10;
                    }
                    else if (curPosMhs >= 8)
                    {
                        x = xPos - 10;
                        //y = yPos - 20;
                        y = yPos;
                    }
                    x *= Screen.width / 1024f;
                    y *= Screen.height / 576f;
                    EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); //ditambah posisi tile target
                    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration); //ganti jadi position karena masalah sort rendering
                    time += Time.deltaTime;

                    //if (curPosMhs == 3 && go_mhs.transform.localScale.x <= 1f)
                    //{
                    //    go_mhs.transform.localScale += new Vector3(0.0001f, 0.0001f, 1);
                    //}
                    //else if (curPosMhs == 7 && go_mhs.transform.localScale.x <= 0.75f)
                    //{
                    //    go_mhs.transform.localScale += new Vector3(0.0001f, 0.0001f, 1);
                    //}

                    yield return null;
                }
            }
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

        }
        //if (curPosMhs == 3)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(1f, 1f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        //else if (curPosMhs == 4 || curPosMhs == 7)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.75f, 0.75f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        //else if (curPosMhs == 8)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.5f, 0.5f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        
        curPosDos = go_dosen.GetComponent<DosenMovement>().getCurPosDosen();
        if (curPosDos == curPosMhs)
        {
            GameInstance.onDosenMarah?.Invoke(true);
            this.Wait(1.5f, () =>
            {
                GameInstance.onDosenMarah?.Invoke(false);
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranDosen?.Invoke();
                });
            });
        }
        else
        {
            this.Wait(0.5f, () =>
            {
                GameInstance.onGiliranDosen?.Invoke();
            });
        }
    }

    IEnumerator JumpMahasiswa(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            curPosMhs++;
           
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (curPosMhs >= 1 && curPosMhs <= 3)
            {
                x = xPos;
                y = yPos;
            }
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                x = -xPos + 5;
                y = yPos - 10;
            }
            else if (curPosMhs >= 8)
            {
                x = xPos - 10;
                //y = yPos - 20;
                y = yPos;
            }

            Vector2 p0 = go_mhs.transform.position;
            Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y);
            if (curPosMhs == 4 || curPosMhs == 8)
            {
                p1 = new Vector2(go_mhs.transform.position.x, kotak[curPosMhs].transform.position.y + y * 2);
            }
            Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

            while (tParam < 1)
            {
                tParam += Time.deltaTime * speedModifier;

                MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                    2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                transform.position = MahasiswaPosition;
                yield return new WaitForEndOfFrame();

            }

            yield return new WaitForSeconds(0.25f);

            tParam = 0;

            if (curPosMhs == 11)
            {
                break;
            }
        }
        //if (curPosMhs == 4)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.75f, 0.75f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        //else if (curPosMhs == 8)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.5f, 0.5f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        if (curPosMhs == 11)
        {
            this.Wait(0.5f, () =>
            {
                GameInstance.onGameOver?.Invoke(true);
            });

        }
        else
        {
            checkPositifOrNegatif();
        }

    }

    IEnumerator JumpMahasiswaKartuOnPoisitifNegatif(int n)
    {
        int counter = math.abs(n);
        for (int i = 1; i <= counter; i++)
        {

            if (n > 0)
            {
                curPosMhs++;
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (n < 0)
            {
                curPosMhs--;
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0f, 0);
                }
                else
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
            }
            

            if (curPosMhs >= 1 && curPosMhs <= 3)
            {
                x = xPos;
                y = yPos;
            }
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                x = -xPos + 5;
                y = yPos - 10;
            }
            else if (curPosMhs >= 8)
            {
                x = xPos - 10;
                //y = yPos - 20;
                y = yPos;
            }

            Vector2 p0 = go_mhs.transform.position;
            Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y);
            if (curPosMhs == 4 || curPosMhs == 8)
            {
                p1 = new Vector2(go_mhs.transform.position.x, kotak[curPosMhs].transform.position.y + y * 2);
            }
            Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

            while (tParam < 1)
            {
                tParam += Time.deltaTime * speedModifier;

                MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                    2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                transform.position = MahasiswaPosition;
                yield return new WaitForEndOfFrame();

            }

            yield return new WaitForSeconds(0.25f);

            tParam = 0;
        }
        //if (curPosMhs == 4)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.75f, 0.75f, 0);

        //    go_mhs.transform.localScale = scale;
        //}
        //else if (curPosMhs == 8)
        //{
        //    Vector3 scale = go_mhs.transform.localScale;

        //    scale.Set(0.5f, 0.5f, 0);
        //    go_mhs.transform.localScale = scale;
        //}
        if (n < 0)
        {
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        curPosDos = go_dosen.GetComponent<DosenMovement>().getCurPosDosen();
        if (curPosDos == curPosMhs)
        {
            GameInstance.onDosenMarah?.Invoke(true);
            this.Wait(1.5f, () =>
            {
                GameInstance.onDosenMarah?.Invoke(false);
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranDosen?.Invoke();
                });
            });
        }
        else
        {
            this.Wait(0.5f, () =>
            {
                GameInstance.onGiliranDosen?.Invoke();
            });
        }
    }
}
