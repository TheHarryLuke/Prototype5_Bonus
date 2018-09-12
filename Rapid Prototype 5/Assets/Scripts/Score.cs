using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    
    public Slider oxygenSlider;
    public Image oxygenImage;
    public int oxygenDecreaseAmount = 5;

    public Image healthFlash;
    public float flashSpeed = 5f;
    public Color flashColour;
    public float flashCooldown = 1f;
    public float flashCooldownSpeed = 1f;
    public Image Border;
    public Color OxygenDefaultColour;

    // Update is called once per frame
    void Update ()
    {
        if(transform.position.y < -15.0f)
        {
            oxygenSlider.value -= oxygenDecreaseAmount * Time.deltaTime;
            oxygenSlider.value = Mathf.Clamp(oxygenSlider.value, 0, 100);
            if (!oxygenSlider.gameObject.activeSelf)
            {
                oxygenImage.gameObject.SetActive(true);
                oxygenSlider.gameObject.SetActive(true);
            }
        }
        else
        {
            oxygenSlider.value += oxygenDecreaseAmount * Time.deltaTime * 4.0f;
            oxygenSlider.value = Mathf.Clamp(oxygenSlider.value, 0, 100);
            if (oxygenSlider.value >= 99)
            {
                oxygenImage.gameObject.SetActive(false);
                oxygenSlider.gameObject.SetActive(false);
            }
        }
        
        if (oxygenSlider.value < 50 && Time.time > flashCooldown)
        {
            healthFlash.color = flashColour;
            flashCooldown = Time.time + flashCooldownSpeed;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            healthFlash.color = Color.Lerp(healthFlash.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        if (0 >= oxygenSlider.value)
        {
            GameManager.GameFinishedSetEnd(false);
        }

        if(oxygenSlider.value < 99)
        {
            Border.color = Color.Lerp(Border.color, OxygenDefaultColour, Time.deltaTime);
        }
        else
        {
            Border.color = Color.Lerp(Border.color, Color.clear, Time.deltaTime);
        }
    }
}
