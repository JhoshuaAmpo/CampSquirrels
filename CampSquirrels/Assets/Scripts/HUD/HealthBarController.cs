using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    RectTransform bar;
    PlayerHealth playerHealth;

    float maxWidth;

    private void Awake() {
        List<Image> images = new();
        GetComponentsInChildren<Image>(images);
        maxWidth = images[0].rectTransform.rect.width;
        bar = images[^1].rectTransform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnHealthChange += UpdateHealthBar;
    }

    private void UpdateHealthBar(float curHP, float maxHP) {
        float percent = 1 - (curHP / maxHP);
        float newWidth = maxWidth * percent;
        bar.offsetMax = new(-newWidth,bar.offsetMax.y);
    }

}
