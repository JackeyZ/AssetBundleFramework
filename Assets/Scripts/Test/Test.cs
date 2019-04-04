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
        NameTable nt = GetComponent<NameTable>();
        if(nt != null)
        {
            nt.Find("Key1").GetComponent<Image>().LoadSprite("ui/images/playerview.u3dassetbundle", "btn_a");
            nt.Find("Key2").GetComponent<Image>().LoadSprite("ui/images/playerview.u3dassetbundle", "btn_b");
            nt.Find("Key3").GetComponent<Image>().LoadSprite("ui/images/playerview.u3dassetbundle", "btn_c");
            nt.Find("Key4").GetComponent<Image>().LoadSprite("ui/images/playerview.u3dassetbundle", "btn_d");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
