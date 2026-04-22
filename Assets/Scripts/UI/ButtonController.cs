using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    void Start()
    {
        // 1. Получаем компонент или добавляем его, если его нет
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null) trigger = gameObject.AddComponent<EventTrigger>();

        // 2. Настраиваем наведение (PointerEnter)
        // AddEvent(trigger, EventTriggerType.PointerEnter, () => { OnHover(); });

        // 3. Настраиваем нажатие (PointerClick)
        AddEvent(trigger, EventTriggerType.PointerClick, () => { OnClick(); });
    }

    // Метод-помощник, чтобы не писать много кода для каждого события
    void AddEvent(EventTrigger trigger, EventTriggerType type, System.Action action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((eventData) => { action(); });
        trigger.triggers.Add(entry);
    }

    // void OnHover() => AudioController.Instance.PlaySound(AudioController.Instance.buttonClick);
    void OnClick() => AudioController.Instance.PlaySound(AudioController.Instance.buttonClick);
}