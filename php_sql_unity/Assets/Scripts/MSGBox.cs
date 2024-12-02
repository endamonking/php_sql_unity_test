using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MSGBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMSGTab()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.closeTab(TAB.MSG);
        }
    }

    public void DisplayText(string text)
    {
        messageBox.text = text;
    }

}
