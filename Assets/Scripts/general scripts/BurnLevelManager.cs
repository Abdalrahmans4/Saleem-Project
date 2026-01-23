using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using System.Collections;
using Unity.Cinemachine; // If using Cinemachine, otherwise use standard Camera

public class BurnLevelManager : MonoBehaviour
{
    [Header("--- 1. REFERENCES ---")]
    public GameObject saleemPlayer;         // Your FPS Player Controller object
    public Camera mainPlayerCam;            // The camera usually used for walking
    public Camera treatmentCam;             // The static camera for the mini-game
    public GameObject spinningTriggerObj;   // The shiny object you find in the air

    [Header("--- 2. CUTSCENE & VISUALS ---")]
    public PlayableDirector cutsceneDirector;
    public Renderer armRenderer;            // The mesh of the injured arm
    public Texture burnedTexture;           // Initial burn texture
    public Texture bandagedTexture;         // Final bandaged texture

    [Header("--- 3. TREATMENT AUDIO ---")]
    public AudioSource audioSource;         // Attach an AudioSource to this Manager
    public AudioClip sfxCorrectPing;        // "Ding" sound
    public AudioClip sfxWrongBong;          // "Bong" sound
    public AudioClip sfxNurseHurry;         // "Hurry up" dramatic music (optional)

    [Header("--- 4. UI PANELS ---")]
    public GameObject treatmentUIPanel;     // The UI with instructions/tools
    public Image bloodOverlay;              // A red UI Image that flashes on mistake
    public TextMeshProUGUI instructionsText; // "Step 1: Clean..."
    public TextMeshProUGUI timerText;       // "Time: 15"
    public GameObject winScreen;            // 3 Stars Win Screen
    public GameObject loseScreen;           // Game Over Screen
    public GameObject[] starsImages;        // Array of 3 Star Images (UI)

    [Header("--- 5. NURSE MISSION ---")]
    public GameObject nurseNPC;             // The nurse object
    public Transform finalDestination;      // The empty object near the injured kid
    public float missionTime = 15f;         // 15 seconds

    // INTERNAL STATE
    private int currentStep = 0; // 0=Clean, 1=Medicate, 2=Bandage
    private int mistakes = 0;
    private bool isTreating = false;
    private bool isRunningForNurse = false;
    private float timerCount;
    private int clicksOnWound = 0; // For the "Rubbing" check
    private NurseController nurseScript;

    void Start()
    {
        // Setup Start State
        mainPlayerCam.gameObject.SetActive(true);
        treatmentCam.gameObject.SetActive(false);
        treatmentUIPanel.SetActive(false);
        bloodOverlay.gameObject.SetActive(false);
        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);

        // Reset Arm
        if (armRenderer) armRenderer.material.mainTexture = burnedTexture;

        // Spinning Object Logic (Simple Rotation)
        StartCoroutine(SpinTriggerRoutine());

        nurseScript = nurseNPC.GetComponent<NurseController>();
    }

    void Update()
    {
        // === TRIGGER START ===
        // We check this in Update, or you can use OnTriggerEnter on the Player script.
        // For simplicity, let's assume the player presses E when near the trigger.
        if (!isTreating && !isRunningForNurse && Input.GetKeyDown(KeyCode.E))
        {
            // Raycast or distance check to spinningTriggerObj would go here
            // For now, let's assume this function is called by the Trigger script below
        }

        // === TIMER LOGIC (NURSE PHASE) ===
        if (isRunningForNurse)
        {
            timerCount -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timerCount).ToString();

            if (timerCount <= 0)
            {
                TriggerGameOver();
            }
        }
    }

    // Called by the Trigger Object
    public void StartMiniGameSequence()
    {
        spinningTriggerObj.SetActive(false); // Hide the trigger
        StartCoroutine(CutsceneRoutine());
    }

    IEnumerator SpinTriggerRoutine()
    {
        while (spinningTriggerObj.activeSelf)
        {
            spinningTriggerObj.transform.Rotate(0, 50 * Time.deltaTime, 0);
            yield return null;
        }
    }

    IEnumerator CutsceneRoutine()
    {
        // 1. Disable Player Control
        // saleemPlayer.GetComponent<PlayerController>().enabled = false; // Disable your movement script
        Cursor.lockState = CursorLockMode.None; // Unlock mouse 
        Cursor.visible = false;

        // 2. Play Cutscene
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            yield return new WaitForSeconds((float)cutsceneDirector.duration);
        }

        // 3. Switch to Treatment Mode
        StartTreatment();
    }

    void StartTreatment()
    {
        isTreating = true;
        mainPlayerCam.gameObject.SetActive(false);
        treatmentCam.gameObject.SetActive(true);
        treatmentUIPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        instructionsText.text = "Step 1: Clean and Cool (OxyWater)";
        currentStep = 0;
        mistakes = 0;
        clicksOnWound = 0;
    }

    // === MINI-GAME LOGIC ===

    public void OnToolUsed(string toolName)
    {
        if (!isTreating) return;

        bool stepSuccess = false;

        // CHECK LOGIC
        if (currentStep == 0 && toolName == "OxyWater") stepSuccess = true;
        else if (currentStep == 1 && toolName == "BurnCream") stepSuccess = true;
        else if (currentStep == 2 && toolName == "BandAidRoll") stepSuccess = true;

        // TRAP CHECK
        if (toolName == "Alcohol")
        {
            RegisterMistake("ALCOHOL IS BAD!");
            return;
        }

        if (stepSuccess)
        {
            audioSource.PlayOneShot(sfxCorrectPing);
            currentStep++;

            if (currentStep == 1) instructionsText.text = "Step 2: Medicate (BurnCream)";
            if (currentStep == 2) instructionsText.text = "Step 3: Protect (Bandage)";
            if (currentStep == 3) FinishTreatment();
        }
        else
        {
            RegisterMistake("Wrong Tool!");
        }
    }

    // Called when clicking the WOUND directly
    public void OnWoundClicked()
    {
        if (!isTreating) return;

        clicksOnWound++;
        if (clicksOnWound > 3)
        {
            RegisterMistake("Don't Rub the Wound!");
            clicksOnWound = 0; // Reset so they don't get spammed instantly
        }
    }

    void RegisterMistake(string reason)
    {
        mistakes++;
        Debug.Log("Mistake: " + reason);

        // Feedback
        audioSource.PlayOneShot(sfxWrongBong);
        StartCoroutine(ScreenShakeAndRedFlash());

        if (mistakes >= 3)
        {
            TriggerGameOver();
        }
    }

    IEnumerator ScreenShakeAndRedFlash()
    {
        // Flash Red
        bloodOverlay.gameObject.SetActive(true);

        // Shake Camera
        Vector3 originalPos = treatmentCam.transform.position;
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);
            treatmentCam.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        treatmentCam.transform.position = originalPos;
        bloodOverlay.gameObject.SetActive(false);
    }

    void FinishTreatment()
    {
        isTreating = false;

        // Change Texture
        armRenderer.material.mainTexture = bandagedTexture;
        audioSource.PlayOneShot(sfxCorrectPing);

        // Hide Treatment UI
        treatmentUIPanel.SetActive(false);

        // Start Phase 2: RUN!
        StartNurseRunPhase();
    }

    // === NURSE RUN LOGIC ===

    void StartNurseRunPhase()
    {
        isRunningForNurse = true;
        timerCount = missionTime;

        // Switch Camera Back
        treatmentCam.gameObject.SetActive(false);
        mainPlayerCam.gameObject.SetActive(true);

        // Enable Movement
        // saleemPlayer.GetComponent<PlayerController>().enabled = true; 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        instructionsText.gameObject.SetActive(true);
        instructionsText.text = "FIND THE NURSE! (15s)";

        if (sfxNurseHurry) audioSource.PlayOneShot(sfxNurseHurry);
    }

    // Called when Player presses E on Nurse
    public void NurseFound()
    {
        if (!isRunningForNurse) return;

        instructionsText.text = "Bring her to the patient!";
        nurseScript.StartFollowing(saleemPlayer.transform);
    }

    // Called when Nurse reaches the empty object trigger
    public void MissionComplete()
    {
        isRunningForNurse = false;
        instructionsText.text = "";

        // Calculate Stars
        int starsEarned = 3;
        if (mistakes == 1) starsEarned = 2;
        if (mistakes == 2) starsEarned = 1;

        ShowWinScreen(starsEarned);
    }

    void ShowWinScreen(int stars)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        winScreen.SetActive(true);

        // Show correct star count
        for (int i = 0; i < starsImages.Length; i++)
        {
            starsImages[i].SetActive(i < stars);
        }
    }

    void TriggerGameOver()
    {
        isTreating = false;
        isRunningForNurse = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        loseScreen.SetActive(true);
    }
}