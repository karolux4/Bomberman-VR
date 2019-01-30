using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    public List<int> Kills;
    public List<int> Wins;
    public List<int> Deaths;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        for(int i=0;i<4;i++)
        {
            Kills[i] = 0;
            Wins[i] = 0;
            Deaths[i] = 0;
        }
    }
}
