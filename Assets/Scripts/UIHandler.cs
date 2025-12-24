using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    private VisualElement m_healthbar;
    public static UIHandler instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument uidoc = GetComponent<UIDocument>();
        m_healthbar = uidoc.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);
    }

    public void SetHealthValue(float a)
    {
        m_healthbar.style.width = Length.Percent(100.0f * a);
    }
}
