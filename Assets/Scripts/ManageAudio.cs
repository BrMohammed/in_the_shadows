using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAudio : MonoBehaviour
{
	// Start is called before the first frame update
	public static ManageAudio instance;
	void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    }
    public void M_Sound()
	{
		FindObjectOfType<AudioManager>().MuteSound("click");
	}
	public void M_Music()
	{
		FindObjectOfType<AudioManager>().MuteSound("Theme");
	}

}
