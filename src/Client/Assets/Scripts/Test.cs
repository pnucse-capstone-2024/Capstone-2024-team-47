using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(TestCoroutine));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TestCoroutine()
    {
        Debug.Log("TestCoroutine Start");
        yield return new WaitForSeconds(1f);

        PopupQueue.Instance.Push("Hello, Popup!", () => Debug.Log("Hello, Callback!"));
    }
}
