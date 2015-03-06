using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    //Setting of timming variables
    public float dayLength = 1000;
    private float daysPassed;
    public float nightLength = 500;
    private float nightsPassed;
    private float nextPhase = 0;
    private float currentPhase = 0;
    private float relativePhase = 0;
    public float currentTime = 0;
    public float timeSpeed = 1;
    public bool isDay = true;
    private bool previousIsDay = true;

    //Setting of gradient variables
    public Gradient daySpectrum;
    public Gradient nightSpectrum;
    public Color currentLightColor;

	// Use this for initialization
	void Start () {
	
        if (isDay == true)
        {
            nextPhase = dayLength;
            currentPhase = dayLength;
        }
        else
        {
            nextPhase = nightLength;
            currentPhase = nightLength;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //Sets the current time
        currentTime = Time.time * timeSpeed;

        /*
        if (isDay != previousIsDay)
        {
            if (isDay == true)
            {
                relativePhase = ((relativePhase / currentPhase) * dayLength);
                currentPhase = dayLength;
            }
            else if (isDay == false)
            {
                relativePhase = ((relativePhase / currentPhase) * nightLength);
                currentPhase = nightLength;
            }
            previousIsDay = isDay;
        }*/
        
        //Checks for day/night to end
        if (isDay == true && currentTime > nextPhase)
        {
            nextPhase += nightLength;
            isDay = false;
            previousIsDay = isDay;
            daysPassed += 1;
            currentPhase = nightLength;
        }
        else if (isDay == false && currentTime > nextPhase)
        {
            nextPhase += dayLength;
            isDay = true;
            previousIsDay = isDay;
            nightsPassed += 1;
            currentPhase = dayLength;
        }

        //Calculate the relative phase
        relativePhase = currentTime - ((daysPassed * dayLength) + (nightsPassed * nightLength));

        //Adjusts current light color to be the appropriate colour
        if (isDay == true) currentLightColor = daySpectrum.Evaluate(relativePhase / currentPhase);
        else if (isDay == false) currentLightColor = nightSpectrum.Evaluate(relativePhase / currentPhase);
        
        //Sets color of the sun light
        gameObject.GetComponent<Light>().color = currentLightColor;

        //Set rotation of the sun
        transform.localEulerAngles = new Vector3((relativePhase / currentPhase) * 180, 0, 0);
	}
}
