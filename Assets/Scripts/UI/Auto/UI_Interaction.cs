using UnityEngine;

public class UI_Interaction : UI_Auto
{
    enum Images
    {
        LoadingTimeImage,
        BG,
        Frame,
    }

    enum Texts
    {
        KeyText,
        InteractionText,
        NameText,
    }

    private Interactor _interactor;
    private UI_FollowWorldObject _followTarget;

    protected override void Init()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _interactor = Player.Interactor;
        _followTarget = GetComponent<UI_FollowWorldObject>();
    }

    protected override void Start()
    {
        base.Start();

        GetText((int)Texts.KeyText).text = Managers.Input.GetBindingPath("Interact");
        _interactor.TargetChanged += target => SetTarget(target);
    }

    private void LateUpdate()
    {
        if (_interactor.Target == null)
        {
            return;
        }

        if (_interactor.Target.IsInteracted)
        {
            Body.SetActive(false);
        }
        else if (!Body.activeSelf)
        {
            Body.SetActive(true);
        }

        if (GetImage((int)Images.LoadingTimeImage).isActiveAndEnabled)
        {
            GetImage((int)Images.LoadingTimeImage).fillAmount = _interactor.ProgressedLoadingTime / _interactor.Target.LoadingTime;
        }
    }

    private void SetTarget(Interactable target)
    {
        if (target != null)
        {
            _followTarget.SetTargetAndOffset(target.transform, target.UIOffset);

            bool canInteract = target.CanInteract;
            GetImage((int)Images.BG).gameObject.SetActive(canInteract);
            GetText((int)Texts.KeyText).gameObject.SetActive(canInteract);

            var interactionText = GetText((int)Texts.InteractionText);
            interactionText.text = target.InteractionMessage;
            interactionText.gameObject.SetActive(canInteract);

            var name = GetText((int)Texts.NameText);
            name.text = target.InteractionName;
            name.gameObject.SetActive(!string.IsNullOrEmpty(target.InteractionName));

            bool hasLoadingtime = target.LoadingTime > 0f;
            GetImage((int)Images.LoadingTimeImage).gameObject.SetActive(hasLoadingtime);
            GetImage((int)Images.Frame).gameObject.SetActive(hasLoadingtime);
        }

        Body.SetActive(target != null);
    }
}
