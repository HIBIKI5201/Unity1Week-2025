using SymphonyFrameWork.System;
using TMPro;
using UnityEngine;

public class HaveAblityText : MonoBehaviour
{
    private TextMeshProUGUI[] _ablity;
    private AblityRepository _ablityRepository;
    private void Start()
    {
        _ablityRepository = ServiceLocator.GetInstance<AblityRepository>();
    }
}
