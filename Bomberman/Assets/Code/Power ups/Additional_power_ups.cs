using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Additional_power_ups : MonoBehaviour {
    public int bomb_power;
    public int limit;
    public int bounce_limit;
    public float speed;
    public int lifes_count;
    public bool bomb_kick = false;
    public bool invincible = false;
    public float explosion_time;
    public Coroutine hit;
    public void Hit()
    {
        hit = StartCoroutine(Invincible());
    }
    IEnumerator Invincible()
    {
        this.gameObject.transform.Find("Bomberman").gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(1);
        this.gameObject.transform.Find("Bomberman").gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        invincible = false;
    }
}
