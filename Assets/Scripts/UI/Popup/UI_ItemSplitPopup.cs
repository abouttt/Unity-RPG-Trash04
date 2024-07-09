using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class UI_ItemSplitPopup : UI_Popup
{
    enum GameObjects
    {
        ItemPrice,
    }

    enum Texts
    {
        GuideText,
        PriceText,
    }

    enum Buttons
    {
        UpButton,
        DownButton,
        YesButton,
        NoButton,
    }

    enum InputFields
    {
        InputField,
    }

    public int Count { get; private set; }

    private int _price;
    private int _minCount;
    private int _maxCount;
    private bool _isShowedPrice;

    private DOTweenAnimation _dotween;

    protected override void Init()
    {
        base.Init();

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));

        _dotween = PopupRT.GetComponent<DOTweenAnimation>();
    }

    private void Start()
    {
        Managers.UI.Register<UI_ItemSplitPopup>(this);

        Showed += () =>
        {
            PopupRT.localScale = new Vector3(0f, 1f, 1f);
            _dotween.DORestart();
        };

        var inputField = Get<TMP_InputField>((int)InputFields.InputField);
        inputField.onValueChanged.AddListener(value => OnValueChanged(value));
        inputField.onEndEdit.AddListener(value => OnEndEdit(value));
        inputField.onSubmit.AddListener(value => GetButton((int)Buttons.YesButton).onClick.Invoke());

        GetButton((int)Buttons.UpButton).onClick.AddListener(() => OnClickUpOrDownButton(1));
        GetButton((int)Buttons.DownButton).onClick.AddListener(() => OnClickUpOrDownButton(-1));
        GetButton((int)Buttons.NoButton).onClick.AddListener(Managers.UI.Close<UI_ItemSplitPopup>);
    }

    public void SetEvent(UnityAction callback, string text, int minCount, int maxCount, int price = 0, bool showPrice = false)
    {
        var yesButton = GetButton((int)Buttons.YesButton);
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(callback);
        yesButton.onClick.AddListener(Managers.UI.Close<UI_ItemSplitPopup>);

        Count = maxCount;
        _maxCount = maxCount;
        _minCount = minCount;
        _price = price;
        _isShowedPrice = showPrice;

        GetText((int)Texts.GuideText).text = text;
        var inputField = Get<TMP_InputField>((int)InputFields.InputField);
        inputField.text = Count.ToString();
        inputField.ActivateInputField();
        GetObject((int)GameObjects.ItemPrice).SetActive(_isShowedPrice);
        RefreshPriceText();
    }

    private void OnValueChanged(string value)
    {
        Count = string.IsNullOrEmpty(value) ? 0 : Mathf.Clamp(int.Parse(value), _minCount, _maxCount);
        Get<TMP_InputField>((int)InputFields.InputField).text = Count.ToString();
        RefreshPriceText();
    }

    private void OnEndEdit(string value)
    {
        Count = Mathf.Clamp(string.IsNullOrEmpty(value) ? _maxCount : int.Parse(value), _minCount, _maxCount);
        var inputField = Get<TMP_InputField>((int)InputFields.InputField);
        inputField.text = Count.ToString();
        inputField.caretPosition = inputField.text.Length;
    }

    private void OnClickUpOrDownButton(int count)
    {
        Count = Mathf.Clamp(Count + count, _minCount, _maxCount);
        Get<TMP_InputField>((int)InputFields.InputField).text = Count.ToString();
    }

    private void RefreshPriceText()
    {
        if (!_isShowedPrice)
        {
            return;
        }

        int totalPrice = _price * Count;
        GetText((int)Texts.PriceText).text = totalPrice.ToString();
        //GetText((int)Texts.PriceText).color = _price * Count <= Player.Status.Gold ? Color.white : Color.red;
    }
}
