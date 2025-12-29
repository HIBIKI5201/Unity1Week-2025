using SymphonyFrameWork.System;
using TMPro;
using UnityEngine;

public class AblityTextViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _text;
    [SerializeField] private AblityName _ablityName;
    private AblityManager _ablityManager;

    private void Start()
    {
        _ablityManager = ServiceLocator.GetInstance<AblityManager>();
        Refresh();
    }

    /// <summary>
    /// パッシブアビリティ表示を更新
    /// </summary>
    public void Refresh()
    {
        foreach (var t in _text)
        {
            t.text = "--------------";
            t.gameObject.SetActive(false);
        }
        //アクティブアビリティを取得
        var active = _ablityManager.GetActive();
        //パッシブアビリティを取得
        var passives = _ablityManager.GetPassives();
        //上から順に表示
        int index = 0;
        // アクティブアビリティ表示
        if (index < _text.Length)
        {
            _text[index].text = $"{GetName(active)}";
            _text[index].gameObject.SetActive(true);
            index++;
        }

        // パッシブアビリティ表示
        foreach (var p in passives)
        {
            if (index >= _text.Length)
                break;

            _text[index].text = $"{GetName(p)}";
            _text[index].gameObject.SetActive(true);
            index++;
        }
    }

    /// <summary>
    /// Ability → 表示名変換
    /// </summary>
    private string GetName(object ability)
    {
        if (ability == null) return "";

        if (ability is IAbilityTypeHolder holder)
            return _ablityName.GetName(holder.AbilityType);

        return ability.GetType().Name;
    }
}
