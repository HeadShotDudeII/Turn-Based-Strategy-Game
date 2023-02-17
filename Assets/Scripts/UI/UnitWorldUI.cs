using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitActionPointText;
    [SerializeField] private Image healthBarImage;

    HealthSystem healthSystem;
    Unit unit;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        healthSystem = GetComponentInParent<HealthSystem>();
    }

    private void Start()
    {
        UpdateActionPointText();
        UpdateHealthBar();
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamage += HealthSystem_OnDamage;
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointText();
    }

    private void UpdateActionPointText()
    {
        unitActionPointText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthPercentage();
    }
}
