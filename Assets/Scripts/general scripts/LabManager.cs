using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.Cinemachine; // Unity 6 Namespace

public class LabManager : MonoBehaviour
{
    [Header("1. Cutscene Setup")]
    public PlayableDirector timeline;
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

    // --- LOGIC FIX HERE ---
    private int currentQuestionIndex = 0;

    // 0 = A, 1 = B, 2 = C
    // Your Order: Q1 is A (0), Q2 is C (2), Q3 is B (1)
    private int[] correctAnswers = { 0, 2, 1 };

    void Start()
    {
        // 1. FORCE CAMERAS: Ensure Player Cam is active, Cutscene is off
        mainCam.Priority = 10;
        cutsceneCam.Priority = 0;

        // 2. Hide Quiz Elements
        feedbackCube.enabled = false;
        foreach (var row in answerRows) row.SetActive(false);

        // 3. Show Start Button
        startButton.SetActive(true);
    }

    // --- PHASE 1: START CUTSCENE ---
    public void StartGameSequence()
    {
        startButton.SetActive(false);
        StartCoroutine(PlayCutsceneRoutine());
    }

    IEnumerator PlayCutsceneRoutine()
    {
        // Switch to Cutscene Cam
        cutsceneCam.Priority = 20;

        // Play Timeline
        timeline.Play();

        // Wait for Timeline duration
        yield return new WaitForSeconds((float)timeline.duration);

        // Switch back to Player Cam
        cutsceneCam.Priority = 0;
        mainCam.Priority = 10;

        // START THE QUIZ
        LoadQuestion(0);
    }

    // --- PHASE 2: QUIZ LOGIC ---

    void LoadQuestion(int index)
    {
        currentQuestionIndex = index;

        // Change Projector Image
        if (index < questionImages.Length)
        {
            projectorScreen.material.mainTexture = questionImages[index];
        }

        // Activate ONLY the current row
        for (int i = 0; i < answerRows.Length; i++)
        {
            if (i == index)
                answerRows[i].SetActive(true);
            else
                answerRows[i].SetActive(false);
        }

        // Hide feedback while thinking
        feedbackCube.enabled = false;
    }

    // This checks if the answer is Right or Wrong
    public void SubmitAnswer(int answerIndex)
    {
        // Get the correct answer for THIS question
        int correctAnswer = correctAnswers[currentQuestionIndex];

        // Compare
        bool isCorrect = (answerIndex == correctAnswer);

        // Show Feedback (Green Check / Red X)
        feedbackCube.enabled = true;
        feedbackCube.material.mainTexture = isCorrect ? correctTexture : wrongTexture;

        // Wait 3 seconds, then move on
        StartCoroutine(NextQuestionDelay());
    }

    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(3f);

        // Disable the old row
        answerRows[currentQuestionIndex].SetActive(false);

        // Increment Question Index
        int nextQ = currentQuestionIndex + 1;

        if (nextQ < questionImages.Length)
        {
            LoadQuestion(nextQ);
        }
        else
        {
            Debug.Log("Game Over - You finished all questions!");
            // Optional: Turn off projector or show "Well Done" image
        }
    }
}