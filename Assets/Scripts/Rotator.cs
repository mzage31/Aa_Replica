using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float StartSpeed = -10;
    float RealSpeed = 0;
    [HideInInspector] public bool CanRotate = true;
    void Start()
    {
        RealSpeed = StartSpeed;
        StartCoroutine(SpeedChanger());
    }
    void Update()
    {
        if (CanRotate)
            transform.Rotate(new Vector3(0, 0, RealSpeed * 10f * Time.deltaTime));
    }
    IEnumerator SpeedChanger()
    {
        yield return new WaitForSeconds(Random.Range(4f, 10f));
        while (true)
        {
            RealSpeed = Mathf.Sign(RealSpeed) * StartSpeed * Random.Range(0.8f, 1.5f);
            yield return new WaitForSeconds(Random.Range(4f, 10f));
        }
    }
}
