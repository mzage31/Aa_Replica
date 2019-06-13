using UnityEngine;

public class Pin : MonoBehaviour
{
    public static string Str_Rotator = "Rotator";
    public static string Str_Pin = "Pin";
    public TMPro.TMP_Text PinNumber;
    public Vector2 MovementVelocity;

    Rigidbody2D rb2D;

    bool CanMove = true;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PinNumber.transform.rotation = Quaternion.identity;
        PinNumber.transform.position = transform.position;
    }
    void FixedUpdate()
    {
        if (CanMove)
            rb2D.MovePosition(rb2D.position + MovementVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Str_Rotator))
        {
            CanMove = false;
            transform.SetParent(other.transform);
            Manager.instance.EndGame(EndGameType.WinWithCheck);
        }
        else if (other.CompareTag(Str_Pin))
        {
            Manager.instance.EndGame(EndGameType.Lose);
        }
    }
    public void SetNumber(int number)
    {
        if (PinNumber)
            PinNumber.text = number.ToString();
    }
}