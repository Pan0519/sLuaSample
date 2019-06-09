using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeType : MonoBehaviour
{

    public Button ClickBtn;

    public Text ResultText;

    [SerializeField]
    GameObject parentGo;

    // Start is called before the first frame update
    void Start()
    {
        ClickBtn.onClick.AddListener(Click);
    }

    void Click()
    {
        var getType = Knife.Instance.Bind<Text>(parentGo);

        setResult($"Get Type is null?{getType == null}");
    }

    void setResult(object obj)
    {
        ResultText.text = $"Show Result {obj}";
    }
}
