using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameControllet : MonoBehaviour
{
    public Interactable[] Interactables;
    public InputActions InputActions;

    Interactable SelectedInteractable;
    VisualElement UIContainer;
    TextElement InteractableName;

    int currentInteractable = -1;

    void Awake()
    {
        UIDocument document = GetComponent<UIDocument>();
        InteractableName = document.rootVisualElement.Query<TextElement>("interactable-name").First();
        UIContainer = document.rootVisualElement.Query<VisualElement>("ui-container").First();
        UIContainer.AddToClassList("hide");

        InputActions = new InputActions();
        InputActions.Gameplay.Select.performed += SelectAction;
        InputActions.Gameplay.Interact.performed += InteractAction;
        InputActions.Gameplay.Give.performed += GiveAction;
    }

    void SelectAction(InputAction.CallbackContext context)
    {
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

        SelectedInteractable = Interactables[index];
        SelectedInteractable.Select();

        UIContainer.RemoveFromClassList("hide");
        InteractableName.text = SelectedInteractable.Name;

        currentInteractable = index;
    }

    void InteractAction(InputAction.CallbackContext context)
    {
        UIContainer.AddToClassList("hide");
        if (SelectedInteractable != null) {
            SelectedInteractable.Deselect();
        }
    }

    void GiveAction(InputAction.CallbackContext context)
    {
        UIContainer.AddToClassList("hide");
        if (SelectedInteractable != null) {
            SelectedInteractable.Deselect();
        }
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
