using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    // Speed multiplier
    public float speedMultiplier;
    public List<Material> Skyboxes;
    int currentSkybox;

    // Update is called once per frame
    void Update()
    {
        //Sets the float value of "_Rotation", adjust it by Time.time and a multiplier.
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedMultiplier);
    }
    private void OnEnable()
    {
        currentSkybox = Random.Range(0, Skyboxes.Count);
        RenderSettings.skybox = Skyboxes[currentSkybox];
    }
    public void ChangeSkybox()
    {
        currentSkybox += 1;
        if(currentSkybox == Skyboxes.Count) { currentSkybox = 0; }
        RenderSettings.skybox = Skyboxes[currentSkybox];
    }
}
