using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameControllet : MonoBehaviour
{
    public Interactable[] Interactables;
    public InputActions InputActions;

    public float TimeScale;
    public float DayStart = 8f;
    public float DayEnd = 17f;
    float CurrentTime;

    bool Interacting;

    Interactable SelectedInteractable;
    VisualElement InteractionContainer;
    TextElement InteractableName;
    TextElement TimerLabel;

    int currentInteractable = -1;

    void Awake()
    {
        UIDocument document = GetComponent<UIDocument>();
        InteractableName = document.rootVisualElement.Query<TextElement>("interactable-name").First();
        TimerLabel = document.rootVisualElement.Query<TextElement>("timer").First();
        InteractionContainer = document.rootVisualElement.Query<VisualElement>("interaction-container").First();
        InteractionContainer.AddToClassList("hide");

        InputActions = new InputActions();
        InputActions.Gameplay.Select.performed += SelectAction;
        InputActions.Gameplay.Interact.performed += InteractAction;
        InputActions.Gameplay.Give.performed += GiveAction;
    }

    void Start()
    {
        CurrentTime = DayStart * 60;
        foreach (var interactable in Interactables)
        {
            interactable.UpdateTime(DayStart, true);
        }
    }

    void Update()
    {
        CurrentTime += TimeScale * Time.deltaTime;
        if (CurrentTime > DayEnd * 60)
        {
            Debug.Log("It's over");
        }

        foreach (var interactable in Interactables)
        {
            interactable.UpdateTime(CurrentTime / 60);
            var random = Random.value / (TimeScale * Time.deltaTime);
            var chance = interactable.GetActivationChance();
            if (random < chance)
            {
                interactable.Activate();
            }
        }

        int hour = (int)CurrentTime / 60;
        int minute = (int)CurrentTime % 60;

        TimerLabel.text = $"<mspace=18px>{hour:D2}h{minute:D2}";
    }

    void SelectAction(InputAction.CallbackContext context)
    {
        if (Interacting)
        {
            return;
        }

        if (SelectedInteractable != null)
        {
            SelectedInteractable.Deselect();
        }

        var index = currentInteractable + (int)context.ReadValue<float>();
        if (index >= Interactables.Length)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = Interactables.Length - 1;
        }

        Select(Interactables[index]);
        currentInteractable = index;
    }

    void InteractAction(InputAction.CallbackContext context)
    {
        if (SelectedInteractable != null)
        {
            Interacting = !Interacting;

            if (Interacting)
            {
                SelectedInteractable.Interact();
            }
            else
            {
                SelectedInteractable.Release();
                Deselect();
            }
        }
    }

    void GiveAction(InputAction.CallbackContext context)
    {
        if (SelectedInteractable != null && !Interacting)
        {
            SelectedInteractable.Give();
            Deselect();
        }
    }

    void Select(Interactable interactable)
    {
        SelectedInteractable = interactable;
        SelectedInteractable.Select();

        InteractionContainer.RemoveFromClassList("hide");
        InteractableName.text = SelectedInteractable.Name;
    }

    void Deselect()
    {
        SelectedInteractable = null;
        InteractionContainer.AddToClassList("hide");
    }

    void OnEnable()
    {
        InputActions.Enable();
    }

    void OnDisable()
    {
        InputActions.Disable();
    }
}
