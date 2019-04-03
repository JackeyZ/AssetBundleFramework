using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public string bundleName;
    public string assetName;
	// Use this for initialization
	void Start ()
    {
        Image image = GetComponent<Image>();
        image.LoadSprite(bundleName, assetName);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
