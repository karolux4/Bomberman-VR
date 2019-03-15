using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Load : MonoBehaviour {

    public GameObject Player;
    public Sprite Heart;
    private int existing_hearts;
    public GameObject UI;
    public GameObject Damage;
    public GameObject Win_Menu;
    public int ActiveAICount;
    public AudioSource Background;
    public AudioSource Win;
    private bool started = false;
    // Use this for initialization
    void Start () {
        started = false;
        Time.timeScale = 1;
        existing_hearts = Player.GetComponent<Additional_power_ups>().lifes_count;
        for (int i = 0; i <existing_hearts;i++)
        {
            //float aspect_ratio = (float)Screen.width / (float)1920;
            GameObject obj = new GameObject("Heart"+(i+1));
            obj.AddComponent<Image>();
            obj.GetComponent<Image>().sprite = Heart;
            obj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(0.15f, 0.15f);
            obj.transform.SetParent(this.gameObject.GetComponent<Canvas>().gameObject.transform);
            float startx;
            if (existing_hearts%2==0)
            {
                startx = -(existing_hearts/2)*0.15f;
            }
            else
            {
                startx = -(int)(existing_hearts / 2) * 0.15f - 0.075f;
            }
            float x = startx + i*0.15f;
            float y = 0.4f;
            obj.GetComponent<RectTransform>().localPosition = new Vector3(x, y, -0.05f);
            obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        }
	}
    private void Update()
    {
        if(existing_hearts!=Player.GetComponent<Additional_power_ups>().lifes_count)
        {
            if(existing_hearts>Player.GetComponent<Additional_power_ups>().lifes_count)
            {
                Damage.SetActive(true);
                StartCoroutine(DamageWait());
            }
            for(int i=0;i<existing_hearts;i++)
            {
                Destroy(GameObject.Find("Heart"+(i+1)));
            }
            for(int i=0;i < Player.GetComponent<Additional_power_ups>().lifes_count;i++)
            {
                GameObject obj = new GameObject("Heart" + (i + 1));
                obj.AddComponent<Image>();
                obj.GetComponent<Image>().sprite = Heart;
                obj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(0.15f, 0.15f);
                obj.transform.SetParent(this.gameObject.GetComponent<Canvas>().gameObject.transform);
                float startx;
                if (Player.GetComponent<Additional_power_ups>().lifes_count % 2 == 0)
                {
                    startx = -(Player.GetComponent<Additional_power_ups>().lifes_count / 2) * 0.15f;
                }
                else
                {
                    startx = -(int)(Player.GetComponent<Additional_power_ups>().lifes_count / 2) * 0.15f - 0.075f;
                }
                float x = startx + i * 0.15f;
                float y = 0.4f;
                obj.GetComponent<RectTransform>().localPosition = new Vector3(x, y, -0.05f);
                obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            }
            existing_hearts = Player.GetComponent<Additional_power_ups>().lifes_count;
        }

        if ((ActiveAICount <= 0)&&(!started))
        {
            Player.GetComponent<Movement_physics>().end = true;
            started = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<Movement_physics>().end = true;
            UI.SetActive(false);
            Win_Menu.SetActive(true);
            Background.Stop();
            GameObject.Find("Stats").GetComponent<Stats>().Wins[0]++;
            Win.Play();
        }
    }
    private IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(1);
        Damage.SetActive(false);
    }
}
