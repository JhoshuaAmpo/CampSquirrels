using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireLifeSpanTimerHUD : MonoBehaviour
{
    [SerializeField]
    private CampfireController campfireController;
    TextMeshProUGUI textMeshProUGUI;
    private void Awake() {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    private void Start() {
        textMeshProUGUI.text = string.Format($"{Mathf.Clamp(0,campfireController.Lifespan,100):0}");
    }

    private void Update() {
        textMeshProUGUI.text = string.Format($"{Mathf.Clamp(0,campfireController.Lifespan,100):0}");
    }
}
