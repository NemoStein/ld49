using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameControllet : MonoBehaviour
{
    public Interactable[] Interactables;
    public InputActions InputActions;

    Interactable SelectedInteractable;
    VisualElement InteractionContainer;
    TextElement InteractableName;

    int currentInteractable = -1;

    void Awake()
    {
        UIDocument document = GetComponent<UIDocument>();
        InteractableName = document.rootVisualElement.Query<TextElement>("interactable-name").First();
        InteractionContainer = document.rootVisualElement.Query<VisualElement>("interaction-container").First();
        InteractionContainer.AddToClassList("hide");

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

        InteractionContainer.RemoveFromClassList("hide");
        InteractableName.text = SelectedInteractable.Name;

        currentInteractable = index;
    }

    void InteractAction(InputAction.CallbackContext context)
    {
        InteractionContainer.AddToClassList("hide");
        if (SelectedInteractable != null)
        {
            SelectedInteractable.Interact();
        }
    }

    void GiveAction(InputAction.CallbackContext context)
    {
        InteractionContainer.AddToClassList("hide");
        if (SelectedInteractable != null)
        {
            SelectedInteractable.Give();
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
