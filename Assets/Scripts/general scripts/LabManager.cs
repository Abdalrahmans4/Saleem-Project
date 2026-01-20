using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.Cinemachine;
using TMPro;

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

    [Header("3. End Game Setup")]
    public Texture successTexture;        // The "Happy/Certificate" Image
    public Texture failTexture;           // The "Sad/Try Again" Image

    // NEW: Drag your 3 mistake objects here (Element 0 = 1 mistake, Element 1 = 2 mistakes, etc.)
    public GameObject[] mistakeObjects;

    [Header("4. Feedback Setup")]
    public GameObject feedbackCube;
    public Texture correctTexture;
    public Texture wrongTexture;

    [Header("5. Rows Setup")]
    public GameObject[] answerRows;

    private int currentQuestionIndex = 0;
    private int mistakeCount = 0;
    private int[] correctAnswers = { 0, 2, 1 }; // 0=A, 1=B, 2=C

    void Start()
    {
        mainCam.Priority = 10;
        cutsceneCam.Priority = 0;

        if (feedbackCube != null) feedbackCube.SetActive(false);

        // Hide all mistake objects at start
        foreach (var obj in mistakeObjects)
        {
            if (obj != null) obj.SetActive(false);
        }

        foreach (var row in answerRows) row.SetActive(false);
        startButton.SetActive(true);
    }

    public void StartGameSequence()
    {
        // RESET GAME
        mistakeCount = 0;
        foreach (var obj in mistakeObjects)
        {
            if (obj != null) obj.SetActive(false);
        }

        startButton.SetActive(false);
        StartCoroutine(PlayCutsceneRoutine());
    }

    IEnumerator PlayCutsceneRoutine()
    {
        cutsceneCam.Priority = 20;
        timeline.Play();
        yield return new WaitForSeconds((float)timeline.duration);
        cutsceneCam.Priority = 0;
        mainCam.Priority = 10;

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
            answerRows[i].SetActive(i == index);
        }

        feedbackCube.SetActive(false);
    }

    public void SubmitAnswer(int answerIndex)
    {
        int correctAnswer = correctAnswers[currentQuestionIndex];
        bool isCorrect = (answerIndex == correctAnswer);

        if (!isCorrect)
        {
            mistakeCount++;
        }

        feedbackCube.SetActive(true);
        feedbackCube.GetComponent<Renderer>().material.mainTexture = isCorrect ? correctTexture : wrongTexture;

        StartCoroutine(NextQuestionDelay());
    }

    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(3f);

        answerRows[currentQuestionIndex].SetActive(false);
        feedbackCube.SetActive(false);

        int nextQ = currentQuestionIndex + 1;

        if (nextQ < questionImages.Length)
        {
            LoadQuestion(nextQ);
        }
        else
        {
            // === GAME FINISHED ===
            Debug.Log($"Quiz Finished! Total Mistakes: {mistakeCount}");

            if (mistakeCount == 0)
            {
                // PERFECT SCORE
                if (successTexture != null)
                    projectorScreen.material.mainTexture = successTexture;
            }
            else
            {
                // MISTAKES WERE MADE
                if (failTexture != null)
                    projectorScreen.material.mainTexture = failTexture;

                // Activate the specific object for the number of mistakes
                // Mistake 1 -> Index 0
                // Mistake 2 -> Index 1
                // Mistake 3 -> Index 2
                int indexToActivate = mistakeCount - 1;

                if (indexToActivate >= 0 && indexToActivate < mistakeObjects.Length)
                {
                    if (mistakeObjects[indexToActivate] != null)
                        mistakeObjects[indexToActivate].SetActive(true);
                }
            }

            // Wait 5 Seconds then Reset
            yield return new WaitForSeconds(5f);

            // Hide everything and reset
            foreach (var obj in mistakeObjects)
            {
                if (obj != null) obj.SetActive(false);
            }
            startButton.SetActive(true);
            mainCam.Priority = 10;
        }
    }
}