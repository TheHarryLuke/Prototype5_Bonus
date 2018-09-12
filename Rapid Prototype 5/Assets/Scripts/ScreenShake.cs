using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public Animator camAnim;

    public void CamShake()
    {
        camAnim.SetTrigger("shake");
    }

    public IEnumerator Shake(float _duration, float _magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while(elapsed < _duration)
        {
            //If time elapsed is less than time to shake, keep shaking
            float x = Random.Range(-1, 1f) * _magnitude;
            float y = Random.Range(-1, 1f) * _magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null; //Wait til next frame is drawn before calling while loop again
        }
        transform.localPosition = originalPosition;
    }
}
