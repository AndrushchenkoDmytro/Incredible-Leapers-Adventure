using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    [SerializeField] Image hpIndicator;
    private float lastHp = 100;
    private float currentHP = 100;
    IEnumerator current;
    bool isEnd = true;

    private void Awake()
    {
        GetComponentInParent<Boss>().OnBossGetDamage += UpdateHpIndicatorValue;
        current = SmoothValue();
    }
    public void UpdateHpIndicatorValue(float currentHP)
    {
        this.currentHP = currentHP;
        if(isEnd == true)
        {
            current = SmoothValue();
        }
        StartCoroutine(current);
        isEnd = false;

    }

    IEnumerator SmoothValue()
    {
        while (lastHp != currentHP)
        {
            lastHp = Mathf.MoveTowards(lastHp, currentHP, Time.fixedDeltaTime * 10);
            hpIndicator.fillAmount = lastHp * 0.01f;
            yield return new WaitForFixedUpdate();
        }
        if(lastHp <= 0)
        {
            gameObject.SetActive(false);
        }
        isEnd = true;
        yield break;
    }
}
