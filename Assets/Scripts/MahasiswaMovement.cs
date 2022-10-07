using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KotakUlarTangga;

public class MahasiswaMovement : MonoBehaviour
{
    public int curPosMhs;
    public GameObject go_mhs;
    public Image MahasiswaImg;
    public Sprite[] MahasiswaSprites;
    public GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;

    void Start()
    {
        GameInstance.onMahasiswaMove += onMahasiswaMove;
        GameInstance.onMahasiswaMoveOnKartu += onMahasiswaMoveOnKartu;
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

    void onMahasiswaMove(int n)
    {
        if(!gameover)
        {
            this.Wait(0.5f, () =>
            {
                StartCoroutine(MoveMahasiswa(n));
            });
        }
    }

    void onMahasiswaMoveOnKartu(int n)
    {
        this.Wait(0.5f, () =>
        {
            StartCoroutine(MoveMahasiswa2(n));
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
                if (curPosMhs >= 4 && curPosMhs <= 7)
                    x = -30;
                else
                    x = 30;
                x *= Screen.width / 1024f;
                y = 5;
                EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); // ditambahin posisi di tile target
                go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration); //diganti jadi position karena masalah sort rendering
                time += Time.deltaTime;
                yield return null;
            }

            //if (curPosDos == curPosMhs)
            //    DosenImg.sprite = DosenSprites[1];
            //else
            //    DosenImg.sprite = DosenSprites[0];

            if (curPosMhs == 11)
            {
                break;
            }
        }
        if (curPosMhs == 11)
        {
            GameInstance.onGameOver?.Invoke(true);
        }
        else
        {
            checkPositifOrNegatif();
        }

    }

    void checkPositifOrNegatif()
    {
        int x;
        //int n  = 0;
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
            this.Wait(2f, () =>
            {
                GameInstance.onGiliranDosen?.Invoke();
            });

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
                    if (curPosMhs >= 4 && curPosMhs <= 7)
                        x = -30;
                    else
                        x = 30; ;
                    x *= Screen.width / 1024f;
                    y = 5;
                    EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
                    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration); //ganti pakai position aja karena masalah sort rendering
                    time += Time.deltaTime;
                    yield return null;
                }

                //if (curPosDos == curPosMhs)
                //    DosenImg.sprite = DosenSprites[1];
                //else
                //    DosenImg.sprite = DosenSprites[0];
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
                    if (curPosMhs >= 4 && curPosMhs <= 7)
                        x = -30;
                    else
                        x = 30;
                    y = 5;
                    EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); //ditambah posisi tile target
                    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration); //ganti jadi position karena masalah sort rendering
                    time += Time.deltaTime;
                    yield return null;
                }

                //if (curPosDos == curPosMhs)
                //    DosenImg.sprite = DosenSprites[1];
                //else
                //    DosenImg.sprite = DosenSprites[0];
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
        this.Wait(1f, () =>
        {
            GameInstance.onGiliranDosen?.Invoke();
        });

    }

    public int getCurPosMhs
    {
        get
        {
            return curPosMhs;
        }
    }
}
