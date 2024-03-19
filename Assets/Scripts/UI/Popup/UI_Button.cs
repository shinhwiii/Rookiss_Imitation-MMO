using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum Images
    {
        ItemIcon,
    }

    enum GameObjects
    {
        TestObject,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject gameObject = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(gameObject, (PointerEventData data) => { gameObject.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    private int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;

        GetText((int)Texts.ScoreText).text = $"Á¡¼ö : {_score}Á¡";
    }
}
