using UnityEngine;
using UnityEngine.Playables; // For Timeline
using System.Collections;
using Unity.Cinemachine;    // REQUIRED for Unity 6

public class LabManager : MonoBehaviour
{
    [Header("1. Cutscene Setup")]
    public PlayableDirector timeline;

    // FIX: Changed 'CinemachineVirtualCamera' to 'CinemachineCamera'
    public CinemachineCamera mainCam;
    public CinemachineCamera cutsceneCam;

    public GameObject startButton;

    [Header("2. Projector Setup")]
    public MeshRenderer projectorScreen;
    public Texture[] questionImages;

    [Header("3. Feedback Setup")]
    public MeshRenderer feedbackCube;
    public Texture correctTexture;
    public Texture wrongTexture;

    [Header("4. Rows Setup")]
    public GameObject[] answerRows;

    // Internal State
    private int currentQuestionIndex = 0;
    private int[] correctAnswers = { 0, 1, 2 };

    void Start()
    {
        feedbackCube.enabled = false;
        foreach (var row in answerRows) row.SetActive(false);
        startButton.SetActive(true);
    }

    public void StartGameSequence()
    {
        startButton.SetActive(false);
        StartCoroutine(PlayCutsceneRoutine());
    }

    IEnumerator PlayCutsceneRoutine()
    {
        // FIX: In Unity 6, we use .Priority (same as before) but ensure references are correct
        cutsceneCam.Priority = 20;

        timeline.Play();

        yield return new WaitForSeconds((float)timeline.duration);

        cutsceneCam.Priority = 0;

        LoadQuestion(0);
    }

    void LoadQuestion(int index)
    {
        currentQuestionIndex = index;

        if (index < questionImages.Length)
        {
            projectorScreen.material.mainTexture = questionImages[index];
        }

        for (int i = 0; i < answerRows.Length; i++)
        {
            if (i == index)
                answerRows[i].SetActive(true);
            else
                answerRows[i].SetActive(false);
        }

        feedbackCube.enabled = false;
    }

    public void SubmitAnswer(int answerIndex)
    {
        bool isCorrect = (answerIndex == correctAnswers[currentQuestionIndex]);

        feedbackCube.enabled = true;
        feedbackCube.material.mainTexture = isCorrect ? correctTexture : wrongTexture;

        StartCoroutine(NextQuestionDelay());
    }

    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(3f);

        answerRows[currentQuestionIndex].SetActive(false);

        int nextQ = currentQuestionIndex + 1;
        if (nextQ < questionImages.Length)
        {
            LoadQuestion(nextQ);
        }
        else
        {
            Debug.Log("Quiz Finished!");
        }
    }
}