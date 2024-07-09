using UnityEngine;
using UnityEngine.UI;

public class UI_CooldownImage : UI_Base
{
    private Cooldown _cooldownRef;
    private Image _cooldownImage;

    protected override void Init()
    {
        _cooldownImage = GetComponent<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_cooldownRef.RemainingTime > 0f)
        {
            _cooldownImage.fillAmount = _cooldownRef.RemainingTime / _cooldownRef.MaxTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetCooldown(Cooldown cooldown)
    {
        if (cooldown == null)
        {
            return;
        }

        if (_cooldownRef != null)
        {
            Clear();
        }

        _cooldownRef = cooldown;
        cooldown.CooldownStarted += Show;
        if (_cooldownRef.RemainingTime > 0f)
        {
            gameObject.SetActive(true);
        }
    }

    public void Clear()
    {
        if (_cooldownRef == null)
        {
            return;
        }

        _cooldownRef.CooldownStarted -= Show;
        _cooldownRef = null;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
