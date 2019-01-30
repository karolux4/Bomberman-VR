using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDestroy : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(!this.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            Destroy(this.gameObject);
        }
	}
}
