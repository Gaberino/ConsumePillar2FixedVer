// GENERATED AUTOMATICALLY FROM 'Assets/InputStuff/PlayerActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""78c279a0-dc2d-4ce2-a79f-cf2e3741f7bc"",
            ""actions"": [
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""5be4b059-af4e-4319-8632-f36cbb77a429"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""d95229fd-20b3-4c2f-b117-316a91d483be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""f25c286f-3397-4fcd-bd5c-66076655d97c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""af0183d6-9cce-4968-9605-c4d5adb0d8bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Consume"",
                    ""type"": ""Button"",
                    ""id"": ""38219a79-c911-40b1-9875-1096e8546557"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Undo"",
                    ""type"": ""Button"",
                    ""id"": ""eb0e854f-d42c-4904-affe-6ee14deed946"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""bd5dcc3b-6d70-4dce-8bc7-9c509fa32ef3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4cd226f5-7819-4072-a937-3c6a42bb12d0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30fcde5f-27a7-46a8-9a80-41a3afe74557"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99399b42-dc6b-41a1-ba20-a1637d59fd3f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1b1aa51-cc3e-452c-b61a-3f6f45b4f566"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f7ceef0-3eb6-4be3-a660-8645bee97108"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d4a35ed-27fc-456c-9b1f-19f2461e3a29"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3f4dabb-41f1-40fb-a216-c787752bc49b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e869101-b02c-4a33-978b-b3f19591c484"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""778b43cd-7661-4591-8b42-693721bb9ec8"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Consume"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""791087f2-b764-457f-9cf7-d104ca4e9346"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1dfcbc9-81ef-431a-9873-e4d2420a8b3c"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_MoveUp = m_Default.FindAction("MoveUp", throwIfNotFound: true);
        m_Default_MoveDown = m_Default.FindAction("MoveDown", throwIfNotFound: true);
        m_Default_MoveLeft = m_Default.FindAction("MoveLeft", throwIfNotFound: true);
        m_Default_MoveRight = m_Default.FindAction("MoveRight", throwIfNotFound: true);
        m_Default_Consume = m_Default.FindAction("Consume", throwIfNotFound: true);
        m_Default_Undo = m_Default.FindAction("Undo", throwIfNotFound: true);
        m_Default_Reset = m_Default.FindAction("Reset", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_MoveUp;
    private readonly InputAction m_Default_MoveDown;
    private readonly InputAction m_Default_MoveLeft;
    private readonly InputAction m_Default_MoveRight;
    private readonly InputAction m_Default_Consume;
    private readonly InputAction m_Default_Undo;
    private readonly InputAction m_Default_Reset;
    public struct DefaultActions
    {
        private @PlayerActions m_Wrapper;
        public DefaultActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveUp => m_Wrapper.m_Default_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_Default_MoveDown;
        public InputAction @MoveLeft => m_Wrapper.m_Default_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_Default_MoveRight;
        public InputAction @Consume => m_Wrapper.m_Default_Consume;
        public InputAction @Undo => m_Wrapper.m_Default_Undo;
        public InputAction @Reset => m_Wrapper.m_Default_Reset;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @MoveUp.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveUp;
                @MoveDown.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveDown;
                @MoveLeft.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveLeft;
                @MoveRight.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMoveRight;
                @Consume.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnConsume;
                @Consume.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnConsume;
                @Consume.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnConsume;
                @Undo.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnUndo;
                @Undo.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnUndo;
                @Undo.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnUndo;
                @Reset.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnReset;
                @Reset.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnReset;
                @Reset.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnReset;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @Consume.started += instance.OnConsume;
                @Consume.performed += instance.OnConsume;
                @Consume.canceled += instance.OnConsume;
                @Undo.started += instance.OnUndo;
                @Undo.performed += instance.OnUndo;
                @Undo.canceled += instance.OnUndo;
                @Reset.started += instance.OnReset;
                @Reset.performed += instance.OnReset;
                @Reset.canceled += instance.OnReset;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IDefaultActions
    {
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnConsume(InputAction.CallbackContext context);
        void OnUndo(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
    }
}
