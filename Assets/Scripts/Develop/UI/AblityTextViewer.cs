using SymphonyFrameWork.System;
using TMPro;
using UnityEngine;

public class AblityTextViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _text;
    [SerializeField] private AblityName _ablityName;
    private AblityRepository _ablityRepository;

    private void Start()
    {
        _ablityRepository = ServiceLocator.GetInstance<AblityRepository>();
        Refresh();
    }

    /// <summary>
    /// Repository から付与済みアビリティを取得して表示
    /// </summary>
    public void Refresh()
    {
        foreach (var label in _text)
        {
            label.text = "--------------";
            label.gameObject.SetActive(false);
        }

        if (_ablityRepository == null)
        {
            _ablityRepository = ServiceLocator.GetInstance<AblityRepository>();
            if (_ablityRepository == null)
            {
                Debug.LogWarning("AblityTextViewer: AblityRepository が取得できませんでした。");
                return;
            }
        }

        var grantedAbilities = _ablityRepository.GetGrantedAbilities();
        if (grantedAbilities == null || grantedAbilities.Count == 0)
        {
            return;
        }

        int index = 0;
        foreach (var ability in grantedAbilities)
        {
            if (ability == AblityType.None)
            {
                continue;
            }

            if (index >= _text.Length)
            {
                break;
            }

            _text[index].text = GetName(ability);
            _text[index].gameObject.SetActive(true);
            index++;
        }
    }

    private string GetName(AblityType ability)
    {
        if (_ablityName != null)
        {
            return _ablityName.GetName(ability);
        }

        return ability.ToString();
    }
}
