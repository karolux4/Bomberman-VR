    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer_Game_Load : MonoBehaviour {

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public Sprite Heart;
    public int existing_hearts;
    public GameObject UI;
    public GameObject Win_Menu;
    public List<GameObject> Players;
    public List<int> Hearts;
    public int ActiveAI_Count;
    public int ActivePlayers_Count;
    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        Players = new List<GameObject>();
        Players.Add(Player1);
        Players.Add(Player2);
        Hearts = new List<int>();
        Hearts.Add(existing_hearts);
        Hearts.Add(existing_hearts);
        if (Player3 != null)
        {
            Players.Add(Player3);
            Players.Add(Player4);
            Hearts.Add(existing_hearts);
            Hearts.Add(existing_hearts);
        }
        for (int i = 0; i < Players.Count; i++)
        {
            Hearts[i] = Players[i].GetComponent<Additional_power_ups>().lifes_count;
            for (int j = 0; j < Hearts[i]; j++)
            {
                float aspect_ratio = (float)Screen.width / (float)1920;
                GameObject obj = new GameObject(i+"Heart" + (j + 1));
                obj.AddComponent<Image>();
                obj.GetComponent<Image>().sprite = Heart;
                obj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(200/Players.Count * aspect_ratio, 200 / Players.Count * aspect_ratio);
                obj.transform.SetParent(this.gameObject.transform.Find("Canvas_P"+(i+1)));
                float x = -910 + j * 200 / Players.Count;
                x+=960 *((int)(i+1)/(int)3);
                float y = -(((float)1920 / (float)Screen.width) * Screen.height) / 2 + 50;
                y+= 540 * (i % 2);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            }
        }
    }
    private void Update()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Hearts[i] < Players[i].GetComponent<Additional_power_ups>().lifes_count)
            {
                for (int j = Hearts[i]; j < Players[i].GetComponent<Additional_power_ups>().lifes_count; j++)
                {
                    float aspect_ratio = (float)Screen.width / (float)1920;
                    GameObject obj = new GameObject(i+"Heart" + (j + 1));
                    obj.AddComponent<Image>();
                    obj.GetComponent<Image>().sprite = Heart;
                    obj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(200 / Players.Count * aspect_ratio, 200 / Players.Count * aspect_ratio);
                    obj.transform.SetParent(this.gameObject.transform.Find("Canvas_P"+(i+1)));
                    float x = -910 + j * 200/Players.Count + 960 * ((int)i / (int)3);
                    float y = -(((float)1920 / (float)Screen.width) * Screen.height) / 2 + 50;
                    y+=540 * (i % 2);
                    obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                }
                Hearts[i] = Players[i].GetComponent<Additional_power_ups>().lifes_count;
            }
            else if (Hearts[i] > Players[i].GetComponent<Additional_power_ups>().lifes_count)
            {
                RectTransform rec = this.gameObject.transform.Find("Canvas_P" + (i + 1)).GetComponentInChildren<Image>(true).rectTransform;
                rec.sizeDelta = new Vector2(3840/Players.Count, (((float)1920 / (float)Screen.width) * Screen.height/2));
                this.gameObject.transform.Find("Canvas_P" + (i + 1)).GetComponentInChildren<Image>(true).gameObject.SetActive(true);
                for (int j = Hearts[i]; j > Players[i].GetComponent<Additional_power_ups>().lifes_count; j--)
                {
                    if (j > 0)
                    {
                        Destroy(GameObject.Find(i+"Heart" + j));
                    }
                }
                Hearts[i] = Players[i].GetComponent<Additional_power_ups>().lifes_count;
                StartCoroutine(DamageWait(this.gameObject.transform.Find("Canvas_P"+(i+1)).GetComponentInChildren<Image>(true).gameObject));
            }
        }
    }
    private IEnumerator DamageWait(GameObject Damage)
    {
        yield return new WaitForSeconds(1);
        Damage.SetActive(false);
    }
}
