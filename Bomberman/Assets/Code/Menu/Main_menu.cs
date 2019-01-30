using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Main_menu : MonoBehaviour {
    public AudioSource click;
    public AudioMixer mixer;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void P2_Multiplayer()
    {
        SceneManager.LoadScene(2);
    }
    public void P4_Multiplayer()
    {
        SceneManager.LoadScene(3);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Button_click_sound()
    {
        click.Play();
    }
    public void MasterVolume(float slider_value)
    {
        mixer.SetFloat("masterVol",Mathf.Log10(slider_value)*20);
    }
    public void SFXVolume(float slider_value)
    {
        mixer.SetFloat("sfxVol", Mathf.Log10(slider_value) * 20);
    }
    public void MusicVolume(float slider_value)
    {
        mixer.SetFloat("musicVol", Mathf.Log10(slider_value) * 20);
    }
    public void Sensitivity(float slider_value)
    {
        GameObject.Find("Sensitivity").GetComponent<Sensitivity_Value>().Sensitivity = slider_value * 4+1;
    }
}
