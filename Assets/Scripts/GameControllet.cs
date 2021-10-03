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

    GameState CurrentGameState;

    bool Interacting;
    int currentInteractable = -1;
    Interactable SelectedInteractable;

    VisualElement MenuState;
    VisualElement GameplayState;
    VisualElement EndgameState;
    VisualElement InteractionContainer;
    TextElement InteractableName;
    TextElement TimerLabel;

    void Awake()
    {
        var document = GetComponent<UIDocument>();
        var root = document.rootVisualElement;

        MenuState = root.Query<VisualElement>("menu-state").First();
        GameplayState = root.Query<VisualElement>("gameplay-state").First();
        EndgameState = root.Query<VisualElement>("endgame-state").First();

        InteractableName = GameplayState.Query<TextElement>("interactable-name").First();
        TimerLabel = GameplayState.Query<TextElement>("timer").First();
        InteractionContainer = GameplayState.Query<VisualElement>("interaction-container").First();
        InteractionContainer.AddToClassList("hide");

        InputActions = new InputActions();
        InputActions.Gameplay.Select.performed += SelectAction;
        InputActions.Gameplay.Interact.performed += InteractAction;
        InputActions.Gameplay.Give.performed += GiveAction;
    }

    void Start()
    {
        ChangeGameState(GameState.Menu);

        CurrentTime = DayStart * 60;
        foreach (var interactable in Interactables)
        {
            interactable.UpdateTime(DayStart, true);
        }
    }

    void Update()
    {
        if (CurrentGameState == GameState.Gameplay)
        {
            CurrentTime += TimeScale * Time.deltaTime;
            if (CurrentTime > DayEnd * 60)
            {
                ChangeGameState(GameState.Endgame);
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
        if (CurrentGameState == GameState.Menu)
        {
            ChangeGameState(GameState.Gameplay);
            return;
        }

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

    void ChangeGameState(GameState state)
    {
        CurrentGameState = state;
        VisualElement stateUI;

        switch (state)
        {
            case GameState.Menu:
                stateUI = MenuState;
                break;
            case GameState.Gameplay:
                stateUI = GameplayState;
                break;
            case GameState.Endgame:
                stateUI = EndgameState;
                break;
            default:
                throw new System.Exception("How??");
        }

        MenuState.RemoveFromClassList("show");
        GameplayState.RemoveFromClassList("show");
        EndgameState.RemoveFromClassList("show");

        stateUI.AddToClassList("show");
    }
}

enum GameState
{
    Menu,
    Gameplay,
    Endgame,
}
