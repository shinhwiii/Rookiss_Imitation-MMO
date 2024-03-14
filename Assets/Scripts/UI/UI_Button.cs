using TMPro;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private int _score = 0;

    public void OnButtonClicked()
    {
        Debug.Log("clicked");

        _text.text = $"Á¡¼ö : {++_score}Á¡";
    }
}
