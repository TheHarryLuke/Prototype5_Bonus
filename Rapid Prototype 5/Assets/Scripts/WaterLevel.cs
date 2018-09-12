using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterLevel : MonoBehaviour {

    public static float m_WaterLevel = 0.0f;

    public float m_WaterLevelRiseSpeed = 0.1f;
    public int oxygenDecreaseAmount = 5;

    public float flashSpeed = 5f;
    public float flashCooldown = 1f;
    public float flashCooldownSpeed = 1f;

    private GameObject m_Player;

    public Slider oxygenSlider;
    public Color OxygenDefaultColour;
    public Color flashColour;
    public Image Border;
    public Image BorderAroundSlider;

    // Use this for initialization
    void Start ()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_WaterLevel += (m_WaterLevelRiseSpeed * Time.deltaTime);
        gameObject.transform.Translate(Vector3.up * m_WaterLevelRiseSpeed * Time.deltaTime);

        if(m_WaterLevel > (m_Player.transform.position.y + 1.5f))
        {
            //Water above player head height
            //GameManager.GameFinishedSetEnd(false);
            oxygenSlider.value -= oxygenDecreaseAmount * Time.deltaTime;
            oxygenSlider.value = Mathf.Clamp(oxygenSlider.value, 0, 100);
            if (!oxygenSlider.gameObject.activeSelf)
            {
                oxygenSlider.gameObject.SetActive(true);
            }
        }
        else
        {
            //Increase oxygen amount and hide it if it's greater than 99
            oxygenSlider.value += oxygenDecreaseAmount * Time.deltaTime * 4.0f;
            oxygenSlider.value = Mathf.Clamp(oxygenSlider.value, 0, 100);
            if (oxygenSlider.value >= 99)
            {
                oxygenSlider.gameObject.SetActive(false);
            }
        }

        if (oxygenSlider.value < 50f && Time.time > flashCooldown)
        {
            BorderAroundSlider.color = flashColour;
            flashCooldown = Time.time + flashCooldownSpeed;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            BorderAroundSlider.color = Color.Lerp(BorderAroundSlider.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        if (0 >= oxygenSlider.value)
        {
            GameManager.GameFinishedSetEnd(false);
        }

        if (oxygenSlider.value < 30f)
        {
            Border.color = Color.Lerp(Border.color, OxygenDefaultColour, Time.deltaTime);
        }
        else
        {
            Border.color = Color.Lerp(Border.color, Color.clear, Time.deltaTime);
        }
    }
}
