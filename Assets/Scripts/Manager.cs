using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Manager : MonoBehaviour
{
    #region Singleton
    public static Manager instance;
    private void Awake() { instance = this; }
    #endregion

    [Header("References")]
    public Camera mainCamera;
    public Canvas canvas;
    public SpriteRenderer ColoredBackground;
    public Rotator rotator;
    public Transform PinPlaceholder;
    public TMP_Text PinCountText;
    public GameObject InstructionText;

    [Space]
    [Header("Instance Prefabs")]
    public GameObject PinPrefab;
    public GameObject PinTextPrefab;

    [Space]
    [Header("Game Settings")]
    public int PinCount = 15;
    bool EndedGame = false;

    private void Start()
    {
        UpdateUI();
    }
    private void Update()
    {
        if (CheckThrowInput() &&
            !EndedGame &&                               // Are we still playing?
            PinCount > 0 &&                             // Is there any pins to throw?
            Time.time > 0.05f)                          // Are we out of splash screen?
        {
            if (InstructionText)
                Destroy(InstructionText);
            ThrowPins();
            UpdateUI();
        }
    }

    private bool CheckThrowInput()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
#elif UNITY_ANDROID
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }

    private void UpdateUI()
    {
        PinCountText.text = PinCount.ToString();
    }
    private void ThrowPins()
    {
        Pin pin = Instantiate(PinPrefab, PinPlaceholder.position, PinPlaceholder.rotation).GetComponent<Pin>();

        GameObject PinTextInstance = Instantiate(PinTextPrefab, pin.transform.position, Quaternion.identity, canvas.transform);
        pin.PinNumber = PinTextInstance.GetComponent<TMP_Text>();
        pin.SetNumber(PinCount);

        PinCount--;
    }
    public void EndGame(EndGameType type)
    {
        if (EndedGame)
            return;

        switch (type)
        {
            case EndGameType.WinWithCheck:
                if (PinCount != 0)
                    return;
                ColoredBackground.color = Color.green;
                break;
            case EndGameType.Win:
                ColoredBackground.color = Color.green;
                break;
            case EndGameType.Lose:
                ColoredBackground.color = Color.red;
                break;
        }
        StartCoroutine(LoadScene(2f));
        EndedGame = true;
        rotator.CanRotate = false;
        ColoredBackground.GetComponent<Animator>().SetTrigger("EndGame");
    }
    IEnumerator LoadScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum EndGameType
{
    Win,
    WinWithCheck,
    Lose
}