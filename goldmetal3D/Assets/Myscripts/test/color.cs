using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class color : MonoBehaviour
{
    Text ColorChange;
    // Start is called before the first frame update
    void Start()
    {
        ColorChange=GetComponent<Text>();
        StartCoroutine(change());
    }
    IEnumerator change()
    {
       while(true)
        {
            ColorChange.color = new Color(0, 0, 0);
            yield return new WaitForSeconds(1f);
            ColorChange.color = new Color(255, 255, 255);
            yield return new WaitForSeconds(1f);

        }

    }


    // Update is called once per frame
    void Update()
    {
      
    }
}
