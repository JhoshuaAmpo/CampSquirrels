using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageVisual : MonoBehaviour
{
    [SerializeField]
    private float duration = 0;
    List<GameObject> children;
    PlayerHealth playerHealth;
    bool isUp = false;
    private float prevHP = -1;
    private void Awake() {
        children = new();
        foreach (RectTransform child in transform){
            children.Add(child.gameObject);
        }
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnHealthChange += DisplayHUD;
    }

    private void DisplayHUD(float currentHP, float maxHP)
    {
        if (prevHP == -1) { prevHP = maxHP;}
        if (prevHP - currentHP <= 2) { return;}
        prevHP = currentHP;
        if(isUp) {return;}
        isUp = true;
        StartCoroutine(ProcessHUD());
    }

    private IEnumerator ProcessHUD(){
        children[UnityEngine.Random.Range(0,children.Count)].SetActive(true);
        yield return new WaitForSeconds(duration);
        children[UnityEngine.Random.Range(0,children.Count)].SetActive(false);
        isUp = false;
    }
}
