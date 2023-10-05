using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthProgressBar : MonoBehaviour
{
    private Slider backGroundSlider;
    private Slider firstGroundSlider;
    private float newHealth;
    private float lastHealth = 100;

    private void Awake()
    {
        backGroundSlider = transform.GetChild(1).GetComponent<Slider>();
        firstGroundSlider = transform.GetChild(2).GetComponent<Slider>();
        GameObject.Find("/Player").GetComponent<PlayerController>().OnPlayerGetDamage += CalculateProgressValue;
    }

    public void CalculateProgressValue(float currentHealth)
    {
        firstGroundSlider.value = currentHealth;
        newHealth = currentHealth;
        StartCoroutine(SmoothDamage());
    }

    IEnumerator SmoothDamage()
    {
        yield return new WaitForFixedUpdate();
        while(newHealth < lastHealth)
        {
            lastHealth -= 0.5f;
            backGroundSlider.value = lastHealth;
            yield return new WaitForFixedUpdate();
        }
    }
}
