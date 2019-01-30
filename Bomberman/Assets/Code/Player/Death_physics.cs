using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death_physics : MonoBehaviour {
    public GameObject UI;
    public GameObject Win_Menu;
    public GameObject Death_Menu;
    public AudioSource Death;
    public AudioSource Background;
    public AudioSource Win;
    public List<GameObject> AI;
	// Update is called once per frame
	void Update () {
        if(SceneManager.GetActiveScene().buildIndex==1)
        {
            SinglePlayer();
        }
        else
        {
            Multiplayer();
        }
	}
    private void SinglePlayer()
    {
        if ((gameObject.tag == "Player" && gameObject.GetComponent<Additional_power_ups>().lifes_count <= 0) && (!Death_Menu.activeInHierarchy)&&UI.GetComponent<Game_Load>().ActiveAICount!=0)
        {
            this.gameObject.GetComponent<Movement_physics>().end = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            UI.SetActive(false);
            Death_Menu.SetActive(true);
            Background.Stop();
            Death.Play();
            int index = Index(this.gameObject.name);
            GameObject.Find("Stats").GetComponent<Stats>().Deaths[index]++;
            for (int i=0;i<AI.Count;i++)
            {
                if(AI[i].activeInHierarchy)
                {
                    index = Index(AI[i].gameObject.name);
                    GameObject.Find("Stats").GetComponent<Stats>().Wins[index]++;
                }
            }
            GameObject Camera = this.gameObject.transform.GetChild(0).gameObject;
            Camera.transform.parent = null;
            Camera.transform.localPosition = new Vector3(0.5f, 20f, 2.5f);
            Camera.transform.eulerAngles = new Vector3(90, 0, 0);
            this.gameObject.SetActive(false);
        }
        else if (gameObject.tag == "AI" && gameObject.GetComponent<Additional_power_ups>().lifes_count <= 0)
        {
            int index = Index(this.gameObject.name);
            GameObject.Find("Stats").GetComponent<Stats>().Deaths[index]++;
            UI.GetComponent<Game_Load>().ActiveAICount--;
            this.gameObject.SetActive(false);
        }
    }
    private bool started = false;
    private void Multiplayer()
    {
        if(gameObject.tag=="Player"&& UI.GetComponent<Multiplayer_Game_Load>().ActivePlayers_Count==1&&UI.GetComponent<Multiplayer_Game_Load>().ActiveAI_Count==0&&!started)
        {
            this.gameObject.GetComponent<Movement_physics>().end = true;
            started = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Win_Menu.transform.Find("Canvas_P" + this.gameObject.name[this.gameObject.name.Length - 1]).gameObject.SetActive(true);
            Background.Stop();
            Win.Play();
            int index = Index(this.gameObject.name);
            GameObject.Find("Stats").GetComponent<Stats>().Wins[index]++;
        }
        else if ((gameObject.tag == "Player" && gameObject.GetComponent<Additional_power_ups>().lifes_count <= 0))
        {
            Death_Menu.transform.Find("Canvas_P" + this.gameObject.name[this.gameObject.name.Length - 1]).gameObject.SetActive(true);
            UI.transform.Find("Canvas_P" + this.gameObject.name[this.gameObject.name.Length - 1]).gameObject.SetActive(false);
            UI.GetComponent<Multiplayer_Game_Load>().ActivePlayers_Count--;
            GameObject Camera = this.gameObject.transform.GetChild(0).gameObject;
            Camera.transform.parent = null;
            Camera.transform.localPosition = new Vector3(0.5f, 20f, 2.5f);
            Camera.transform.eulerAngles = new Vector3(90, 0, 0);
            this.gameObject.SetActive(false);
            int index = Index(this.gameObject.name);
            GameObject.Find("Stats").GetComponent<Stats>().Deaths[index]++;
            if (UI.GetComponent<Multiplayer_Game_Load>().ActivePlayers_Count==0)
            {
                for (int i = 0; i < AI.Count; i++)
                {
                    if (AI[i].activeInHierarchy)
                    {
                        index = Index(AI[i].gameObject.name);
                        GameObject.Find("Stats").GetComponent<Stats>().Wins[index]++;
                    }
                }
                this.gameObject.GetComponent<Movement_physics>().end = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Death_Menu.transform.Find("Canvas_P" + this.gameObject.name[this.gameObject.name.Length - 1]).gameObject.transform.Find("Restart_Button").gameObject.SetActive(true);
                Death_Menu.transform.Find("Canvas_P" + this.gameObject.name[this.gameObject.name.Length - 1]).gameObject.transform.Find("Quit_Button").gameObject.SetActive(true);
                Background.Stop();
                Death.Play();
            }
        }
        else if (gameObject.tag == "AI" && gameObject.GetComponent<Additional_power_ups>().lifes_count <= 0)
        {
            int index = Index(this.gameObject.name);
            GameObject.Find("Stats").GetComponent<Stats>().Deaths[index]++;
            UI.GetComponent<Multiplayer_Game_Load>().ActiveAI_Count--;
            this.gameObject.SetActive(false);
        }
    }
    private int Index(string name)
    {
        int index = 0;
        if (name[name.Length - 1] == '4' || name[name.Length - 1] == '3' || name[name.Length - 1] == '2' || name[name.Length-1]== '1')
        {
            index = int.Parse(name[name.Length - 1].ToString())-1;
        }
        else
        {
            index = 0;
        }
        return index;
    }
}
