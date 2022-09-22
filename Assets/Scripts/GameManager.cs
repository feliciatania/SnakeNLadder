using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA = new List<QuestionAndAnswers>();
    public GameObject[] options = new GameObject[3];
    public int currentQuestion;

    public TextMeshProUGUI QuestionTxt;

    public GameObject go_versus;
    public GameObject go_gilMahasiswa;
    public GameObject go_gilDosen;
    public GameObject go_question;
    public GameObject go_jwbBenar;
    public GameObject go_jwbSalah;
    public GameObject go_waktuHabis;
    public GameObject go_papan;
    public GameObject go_kartuPos;
    public GameObject go_kartuNeg;
    public GameObject go_win;
    public GameObject go_lose;
    public GameObject go_panelPapan;

    public Button btn_lemparDadu;
    public TextMeshProUGUI AngkaDaduTxt;
    public GameObject go_angkaDadu;
    public bool islemparDaduOnce;
    public int angkaDadu;
    bool answered;

    public TextMeshProUGUI TimerTxt;
    float currentDuration = -1f;
    bool startTimer = false;
    bool isTimeout = false;

    public GameObject[] kotak;
    bool isKarakterMundur321;

    public bool isCorrect;
    //string isAnsweredCorrectly;

    public int curPosMhs;
    public int curPosDos;

    public GameObject go_mhs;
    public GameObject go_dosen;

    public string[] kartuPositif = new string[3];
    public string[] kartuNegatif = new string[4];

    public string[] kartuPositifPerintah = new string[3];
    public string[] kartuNegatifPerintah = new string[4];

    public TextMeshProUGUI kalPosTxt;
    public TextMeshProUGUI kalPerintahPosTxt;
    public TextMeshProUGUI kalNegTxt;
    public TextMeshProUGUI kalPerintahNegTxt;

    int majuOrMundur;

    public Image DiceImg;
    public Sprite[] diceSidesSprites;
    public Image DosenImg;
    public Sprite[] DosenSprites;
    public Image MahasiswaImg;
    public Sprite[] MahasiswaSprites;


    // Start is called before the first frame update
    void Start()
    {
        go_versus.SetActive(false);
        go_gilMahasiswa.SetActive(false);
        go_gilDosen.SetActive(false);
        go_question.SetActive(false);
        go_jwbBenar.SetActive(false);
        go_jwbSalah.SetActive(false);
        go_waktuHabis.SetActive(false);
        go_papan.SetActive(false);
        go_kartuPos.SetActive(false);
        go_kartuNeg.SetActive(false);
        go_win.SetActive(false);
        go_lose.SetActive(false);
       

        QnA.Add(new QuestionAndAnswers() 
        { 
            Question = "Sifat-sifat yang dimiliki oleh seorang technopreneur, kecuali … ", 
            Answers = new string[3] { "logic", "smart", "creativity" }, 
            CorrectAnswers = 2
        });
        QnA.Add(new QuestionAndAnswers()
        {
            Question = "Seorang yang berpikir “Outside the box” adalah …  ",
            Answers = new string[3] { "Entrepreneur", "Sociopreneur", "Technopreneur" },
            CorrectAnswers = 3
        });
        QnA.Add(new QuestionAndAnswers()
        {
            Question = "Sebuah badan usaha yang memberikan pendanaan pada sebuah start-up adalah …",
            Answers = new string[3] { "Angel Investor", "Bootstrap", "Venture Capital" },
            CorrectAnswers = 3
        });
        QnA.Add(new QuestionAndAnswers()
        {
            Question = "Langkah “Select the target market” termasuk dalam ?",
            Answers = new string[3] { "Market Segmentation", "Market Targeting", "Market Positioning" },
            CorrectAnswers = 2
        });
        QnA.Add(new QuestionAndAnswers()
        {
            Question = "Design Thinking merupakan kombinasi dari …",
            Answers = new string[3] { "Analytical Thinking & Intuitive Thinking", "Analytical Thinking & Creative Thinking", "Creative Thinking & Intuitive Thinking" },
            CorrectAnswers = 1
        });

        kartuNegatif[0] = "Kamu tidak memperhatikan dosen pada saat kelas";
        kartuNegatif[1] = "Waduh...Kamu ketiduran jadi tidak masuk kelas";
        kartuNegatif[2] = "Waduh...Kamu sibuk bermain game jadi lupa ngerjain tugas";
        kartuNegatif[3] = "Astaga...Kamu lupa belajar padahal hari ini ada tes";

        kartuNegatifPerintah[0] = "KAMU TIDAK MENDAPAT GILIRAN LEMPAR DADU";
        kartuNegatifPerintah[1] = "MUNDUR 1 KOTAK";
        kartuNegatifPerintah[2] = "MUNDUR 2 KOTAK";
        kartuNegatifPerintah[3] = "MUNDUR 3 KOTAK";


        kartuPositif[0] = "Kamu selalu hadir di kelas dan tidak pernah membolos";
        kartuPositif[1] = "Wah...Kamu mendapat nilai 100 pada UTS dan UAS";
        kartuPositif[2] = "Wow...Semester ini kamu rajin belajar jadi dapat IPS diatas 3.5";

        kartuPositifPerintah[0] = "MAJU 1 KOTAK";
        kartuPositifPerintah[1] = "MAJU 2 KOTAK";
        kartuPositifPerintah[2] = "MAJU 3 KOTAK";

        curPosDos = 0;
        curPosMhs = 0;

        islemparDaduOnce = false;
        //diceSides = Resources.LoadAll<Sprite>("Resource/Dice/");

        btn_lemparDadu.onClick.AddListener(delegate { lemparDadu(); });

        go_dosen.transform.SetParent(kotak[curPosDos].transform, true);
        Vector2 StartPosDosen = go_dosen.transform.position;
        float x;
        float y;
        Vector2 EndPosDosen;
        float time = 0;
        float duration = 0.1f;

        go_mhs.transform.SetParent(kotak[curPosMhs].transform, true);
        Vector2 StartPosMhs = go_mhs.transform.position;
        Vector2 EndPosMhs;

        while (time < duration)
        {
            x = -55;
            y = 30;
            EndPosDosen = new Vector2(x, y);
            go_dosen.transform.localPosition = Vector2.Lerp(StartPosDosen, EndPosDosen, time / duration);

            x = 60;
            y = 15;
            EndPosMhs = new Vector2(x, y);
            go_mhs.transform.localPosition = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);

            time += Time.deltaTime;
        }

        go_versus.SetActive(true);
        this.Wait(2f, () =>
        {
            go_versus.SetActive(false);
            go_papan.SetActive(true);

        });

        this.Wait(2f, () =>
        {
            playgame();
        });
        
    }

    void playgame()
    {
        this.Wait(2f, () =>
        {
            go_gilMahasiswa.SetActive(true);
            this.Wait(2f, () =>
            {
                //go_papan.SetActive(false);
                go_gilMahasiswa.SetActive(false);
                go_question.SetActive(true);
                generateQuestion();
                answered = false;
                StartTimer();
                
            });

        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTimer) return;
        if (answered) return;
        currentDuration -= Time.deltaTime;
        if (currentDuration <= 0)
        {
            isTimeout = true;
            TimeOut();
        }
        TimerTxt.text = Convert.ToInt32(currentDuration).ToString();
    }

    void StartTimer()
    {
        answered = false;
        currentDuration = 15f;
        startTimer = true;
        isTimeout = false;
    }

    public void correctAnswer()
    {
        //Debug.Log("masuk");
        answered = true;
        startTimer = false;
        go_question.SetActive(false);
        go_jwbBenar.SetActive(true);
    }

    public void wrongAnswer()
    {
        answered = true;
        startTimer = false;
        go_question.SetActive(false);
        go_jwbSalah.SetActive(true);
        this.Wait(1f, () =>
        {
            go_jwbSalah.SetActive(false);
            go_gilDosen.SetActive(true);
            this.Wait(2f, () =>
            {
                go_gilDosen.SetActive(false);
                go_papan.SetActive(true);
                StartCoroutine(MoveDosen());
            });
        });
    }

    public void TimeOut()
    { 
        startTimer = false;
        if (isTimeout)
        {
            go_question.SetActive(false);
            go_waktuHabis.SetActive(true);
            this.Wait(2f, () =>
            {
                go_waktuHabis.SetActive(false);
                go_gilDosen.SetActive(true);
                this.Wait(2f, () =>
                {
                    go_gilDosen.SetActive(false);
                    go_papan.SetActive(true);
                    StartCoroutine(MoveDosen());
                });
            });
        }
        isTimeout = false;
    }

    void lemparDadu()
    {
        if(islemparDaduOnce == false)
        {
            islemparDaduOnce = true;
            //angkaDadu = UnityEngine.Random.Range(1, 4);
            //angkaDadu = 4;
            //AngkaDaduTxt.text = angkaDadu.ToString();
            //go_angkaDadu.SetActive(true);
            StartCoroutine(RollTheDice());
            this.Wait(3f, () =>
            {
                go_jwbBenar.SetActive(false);
                DiceImg.sprite = diceSidesSprites[0];
                go_papan.SetActive(true);
                islemparDaduOnce = false;
                //go_angkaDadu.SetActive(false);
                StartCoroutine(MoveMahasiswa(angkaDadu));
                
            });
        }
    }

    IEnumerator RollTheDice()
    {
        int randomDiceSide = 0;
        int finalSide = 0;
        for (int i = 0; i <= 30; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(0, 3);

            DiceImg.sprite = diceSidesSprites[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }

        finalSide = randomDiceSide + 1;
        angkaDadu = finalSide;
    }

    void checkPositifOrNegatif()
    {
        int x;
        //int n  = 0;
        if (curPosMhs == 2)
        {
            x = UnityEngine.Random.Range(0, 2);
            if (x == 0)
            {
                kalPosTxt.text = kartuPositif[0];
                kalPerintahPosTxt.text = kartuPositifPerintah[0];
                majuOrMundur = 1;
            }
            else
            {
                kalPosTxt.text = kartuPositif[2];
                kalPerintahPosTxt.text = kartuPositifPerintah[2];
                majuOrMundur = 3;
            }
        }
        else if (curPosMhs == 4)
        {
            x = UnityEngine.Random.Range(0, 3);
            if (x == 0)
            {
                kalNegTxt.text = kartuNegatif[0];
                kalPerintahNegTxt.text = kartuNegatifPerintah[0];
                majuOrMundur = 0;
            }
            else if (x == 1)
            {
                kalNegTxt.text = kartuNegatif[1];
                kalPerintahNegTxt.text = kartuNegatifPerintah[1];
                majuOrMundur = -1;
            }
            else
            {
                kalNegTxt.text = kartuNegatif[3];
                kalPerintahNegTxt.text = kartuNegatifPerintah[3];
                majuOrMundur = -3;
            }
            //majuOrMundur = -3;
        }
        else if (curPosMhs == 7)
        {
            x = UnityEngine.Random.Range(0, 2);
            if (x == 0)
            {
                kalPosTxt.text = kartuPositif[0];
                kalPerintahPosTxt.text = kartuPositifPerintah[0];
                majuOrMundur = 1;
            }
            else
            {
                kalPosTxt.text = kartuPositif[2];
                kalPerintahPosTxt.text = kartuPositifPerintah[2];
                majuOrMundur = 3;
            }
        }
        else if (curPosMhs == 9)
        {
            x = UnityEngine.Random.Range(0, 3);
            if (x == 0)
            {
                kalNegTxt.text = kartuNegatif[0];
                kalPerintahNegTxt.text = kartuNegatifPerintah[0];
                majuOrMundur = 0;
            }
            else if (x == 1)
            {
                kalNegTxt.text = kartuNegatif[1];
                kalPerintahNegTxt.text = kartuNegatifPerintah[1];
                majuOrMundur = -1;
            }
            else
            {
                kalNegTxt.text = kartuNegatif[3];
                kalPerintahNegTxt.text = kartuNegatifPerintah[3];
                majuOrMundur = -3;
            }
        }
        else
        {
            majuOrMundur = 0;
            this.Wait(2f, () =>
            {
                go_gilDosen.SetActive(true);
                this.Wait(2f, () =>
                {
                    go_gilDosen.SetActive(false);
                    StartCoroutine(MoveDosen());
                });
            });
           
        }
        this.Wait(1f, () =>
        {
            if (curPosMhs == 2 | curPosMhs == 7)
            {
                go_kartuPos.SetActive(true);
            }
            else if (curPosMhs == 4 | curPosMhs == 9)
            {
                go_kartuNeg.SetActive(true);
            }
        });       
        
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

        //Vector3 v3;
        //StartPosDosen = go_dosen.transform.position;
        //x = kotak[curPosMhs].transform.position.x - 30;
        //y = kotak[curPosMhs].transform.position.y + 10;
        //EndPosDosen = new Vector2(x, y);
        //if (curPosDos == 4 | curPosDos == 8)
        //{
        //    v3 = new Vector3(0, 180, 0);
        //    go_dosen.transform.Rotate(v3);
        //}
        //while (time < duration)
        //{
        //    go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, time / duration);
        //    time += Time.deltaTime;
        //    yield return null;
        //}

        // CODE BARU PAKAI CHILD
        kotak[curPosDos - 1].transform.SetParent(kotak[curPosDos].transform, true);
        StartPosDosen = kotak[curPosDos - 1].transform.localPosition;
        go_dosen.transform.SetParent(kotak[curPosDos].transform, false);
        if (curPosDos == 4)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (curPosDos == 8)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
        StartPosDosen = kotak[curPosDos - 1].transform.localPosition;
        //kotak[curPosDos - 1].transform.SetAsFirstSibling();
        while (time < duration)
        {
            if (curPosDos >= 4 && curPosDos <= 7)
                x = 55;
            else
                x = -55;
            y = 30;
            EndPosDosen = new Vector2(x, y);
            go_dosen.transform.localPosition = Vector2.Lerp(StartPosDosen, EndPosDosen, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        if(curPosDos == curPosMhs)
            DosenImg.sprite = DosenSprites[1];
        else
            DosenImg.sprite = DosenSprites[0];

        kotak[curPosDos - 1].transform.SetParent(go_panelPapan.transform);
        kotak[curPosDos - 1].transform.SetSiblingIndex(11 - curPosDos);
        if (curPosDos == 11)
        {
            go_papan.SetActive(false);
            go_lose.SetActive(true);
        }
        else
        {
            playgame();
        }
        
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

            //Vector3 v3;
            //if (curPosMhs == 4 && isKarakterMundur321)
            //{
            //    //Debug.Log("masuk if 1");
            //    v3 = new Vector3(0, 180, 0);
            //    go_mhs.transform.Rotate(v3);
            //}
            //else if (curPosMhs == 4 | curPosMhs == 8 | isKarakterMundur321)
            //{
            //    //Debug.Log("masuk if 2");
            //    v3 = new Vector3(0, 180, 0);
            //    go_mhs.transform.Rotate(v3);
            //    isKarakterMundur321 = false;
            //}
            //while (time < duration)
            //{
            //    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);
            //    time += Time.deltaTime;
            //    yield return null;
            //}


            kotak[curPosMhs - 1].transform.SetParent(kotak[curPosMhs].transform, true);
            StartPosMhs = kotak[curPosMhs - 1].transform.localPosition;
            go_mhs.transform.SetParent(kotak[curPosMhs].transform, false);
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
                    x = -60;
                else
                    x = 60;
                y = 15;
                EndPosMhs = new Vector2(x, y);
                go_mhs.transform.localPosition = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            kotak[curPosMhs - 1].transform.SetParent(go_panelPapan.transform);
            kotak[curPosMhs - 1].transform.SetSiblingIndex(11 - curPosMhs);

            if (curPosDos == curPosMhs)
                DosenImg.sprite = DosenSprites[1];
            else
                DosenImg.sprite = DosenSprites[0];

            if (curPosMhs == 11)
            {
                break;
            }
        }
        if (curPosMhs == 11)
        {
            go_papan.SetActive(false);
            go_win.SetActive(true);
        }
        else
        {
            checkPositifOrNegatif();
        }
            
    }

    IEnumerator MoveMahasiswa2()
    {
        int n = majuOrMundur;
        Vector3 v3;
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

                //if (curPosMhs == 4 | curPosMhs == 8)
                //{
                //    go_mhs.transform.RotateAround(go_mhs.transform.position, go_mhs.transform.up, 180f);
                //}
                //while (time < duration)
                //{
                //    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);
                //    time += Time.deltaTime;
                //    yield return null;
                //}

                kotak[curPosMhs - 1].transform.SetParent(kotak[curPosMhs].transform, true);
                StartPosMhs = kotak[curPosMhs - 1].transform.localPosition;
                go_mhs.transform.SetParent(kotak[curPosMhs].transform, false);
                go_mhs.transform.SetParent(kotak[curPosMhs].transform, false);
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
                        x = -60;
                    else
                        x = 60; ;
                    y = 15;
                    EndPosMhs = new Vector2(x, y);
                    go_mhs.transform.localPosition = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }

                kotak[curPosMhs - 1].transform.SetParent(go_panelPapan.transform);
                kotak[curPosMhs - 1].transform.SetSiblingIndex(11 - curPosMhs);

                if (curPosDos == curPosMhs)
                    DosenImg.sprite = DosenSprites[1];
                else
                    DosenImg.sprite = DosenSprites[0];
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

                //if (curPosMhs == 7 | curPosMhs == 8)
                //{
                //    v3 = new Vector3(0, 180, 0);
                //    go_mhs.transform.Rotate(v3);
                //    isKarakterMundur321 = true;
                //}
                //else if (curPosMhs == 3 | curPosMhs == 7)
                //{
                //    isKarakterMundur321 = true;
                //}
                //while (time < duration)
                //{
                //    go_mhs.transform.position = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);
                //    time += Time.deltaTime;
                //    yield return null;
                //}

                kotak[curPosMhs + 1].transform.SetParent(kotak[curPosMhs].transform, true);
                StartPosMhs = kotak[curPosMhs + 1].transform.localPosition;
                go_mhs.transform.SetParent(kotak[curPosMhs].transform, false);
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
                        x = -60;
                    else
                        x = 60;
                    y = 15;
                    EndPosMhs = new Vector2(x, y);
                    go_mhs.transform.localPosition = Vector2.Lerp(StartPosMhs, EndPosMhs, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
                kotak[curPosMhs + 1].transform.SetParent(go_panelPapan.transform);
                kotak[curPosMhs + 1].transform.SetSiblingIndex(11 - curPosMhs);

                if (curPosDos == curPosMhs)
                    DosenImg.sprite = DosenSprites[1];
                else
                    DosenImg.sprite = DosenSprites[0];
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
            go_gilDosen.SetActive(true);
            this.Wait(2f, () =>
            {
                go_gilDosen.SetActive(false);
                StartCoroutine(MoveDosen());
            });
        });
        
    }

    public void onButtonOk()
    {
        go_kartuPos.SetActive(false);
        go_kartuNeg.SetActive(false);
        StartCoroutine(MoveMahasiswa2());
    }

    void setAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];
            
            if(QnA[currentQuestion].CorrectAnswers == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        currentQuestion = UnityEngine.Random.Range(0, QnA.Count);

        QuestionTxt.text = QnA[currentQuestion].Question;
        setAnswers();

        QnA.RemoveAt(currentQuestion);
    }

    bool isTimer(float t)
    {
        float timeRemaining = t;
        bool timerIsRunning = true;
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                return true;
            }
        }
        return false;
    }
}