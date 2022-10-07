using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasSpawner : MonoBehaviour
{
    public GameObject GO_HowToPlay;
    public GameObject GO_Versus;
    public GameObject GO_GilMahasiswa;
    public GameObject GO_KartuPositif;
    public GameObject GO_KartuNegatif;
    public GameObject GO_Quiz;
    public GameObject GO_JawabanBenar;
    public GameObject GO_JawabanSalah;
    public GameObject GO_WaktuHabis;
    public GameObject GO_gilDosen;
    public GameObject GO_Win;
    public GameObject GO_Lose;

    void Start()
    {
        GameInstance.onStart += onStart;
        GameInstance.onGiliranMahasiswa += onGiliranMahasiswa;
        GameInstance.onKartuPositif += OnKartuPositif;
        GameInstance.onKartuNegatif += OnKartuNegatif;
        GameInstance.onQuizStart += onQuizStart;
        GameInstance.onJawabanBenar += onJawabanBenar;
        GameInstance.onJawabanSalah += onJawabanSalah;
        GameInstance.onLemparDadu += onLemparDadu;
        GameInstance.onTimeout += onTimeout;
        GameInstance.onGiliranDosen += onGiliranDosen;
        GameInstance.onGameOver += onGameOver;
        GameInstance.onMahasiswaMoveOnKartu += onMahasiswaMoveOnKartu;
    }
    private void onStart()
    {
        GO_HowToPlay.SetActive(false);
        GO_Versus.SetActive(true);
        this.Wait(3f, () =>
        {
            GO_Versus.SetActive(false);
            this.Wait(2f, () =>
            {
                GameInstance.onGiliranMahasiswa?.Invoke();
            });
                
        });
    }
    private void onGiliranMahasiswa()
    {
        GO_GilMahasiswa.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_GilMahasiswa.SetActive(false);
            GameInstance.onQuizStart?.Invoke();
        });
    }
    private void OnKartuPositif(int x)
    {
        GO_KartuPositif.SetActive(true);
        GameObject.Find("Kartu Positif").GetComponent<KartuPositif>().OnKartuPositif(x);
    }
    private void OnKartuNegatif(int x)
    {
        GO_KartuNegatif.SetActive(true);
        GameObject.Find("Kartu Negatif").GetComponent<KartuNegatif>().onKartuNegatif(x);
    }
    private void onQuizStart()
    {
        GO_Quiz.SetActive(true);
    }
    private void onJawabanBenar()
    {
        GO_Quiz.SetActive(false);
        GO_JawabanBenar.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_JawabanSalah.SetActive(false);
        });
    }
    private void onJawabanSalah()
    {
        GO_Quiz.SetActive(false);
        GO_JawabanSalah.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_JawabanSalah.SetActive(false);
            GameInstance.onGiliranDosen?.Invoke();
        });
    }
    private void onTimeout()
    {
        GO_Quiz.SetActive(false);
        GO_WaktuHabis.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_WaktuHabis.SetActive(false);
            GameInstance.onGiliranDosen?.Invoke();
        });
    }
    private void onLemparDadu()
    {
        GO_JawabanBenar.SetActive(false);
    }
    public void onMahasiswaMoveOnKartu(int x)
    {
        GO_KartuNegatif.SetActive(false);
        GO_KartuPositif.SetActive(false);
    }
    private void onGiliranDosen()
    {
        GO_gilDosen.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_gilDosen.SetActive(false);
            GameInstance.onDosenMove?.Invoke();
        });
    }
    private void onGameOver(bool win)
    {
        if (win)
            GO_Win.SetActive(true);
        else
            GO_Lose.SetActive(true);
    }
    void Update()
    {

    }
}
