using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Tooltip("Mapping of sub menu panels/canvases to the buttons that open them.")] [SerializeField]
        private List<MenuButtonTuple> menusAndButtons;

        [Tooltip("The menu for confirmation before exiting. Opens when ESC is pressed.")] [SerializeField]
        private RectTransform confirmExitMenu;

        [Tooltip("Image that is shown when any child menu is open to capture backdrop clicks.")] [SerializeField]
        private RectTransform backDropImage;

        [Header("Audio")]
        [Tooltip(
            "Music to play after start. Requires our AudioController to be available.")]
        [SerializeField]
        private string music;

        [Tooltip(
            "Sound to play on clicking any button. Requires our AudioController to be available.")]
        [SerializeField]
        private string clickSoundName = "click";

        [Tooltip(
            "Sets the default \"click\" sound to be played on pressing any buttons. Requires our AudioController to be available.")]
        [SerializeField]
        private bool addClickToAllButtons = true;

        private List<ButtonFuncTuple> _registeredCallbacks;

        private void Start()
        {
            if (music != null)
            {
                AudioController.Instance.PlayMusic(music); // Remove these lines if you do not have our AudioController
            }

            SetupSubMenus();
            HideSubMenus();
        }

        private void OnEnable()
        {
            RegisterButtons();
        }

        private void OnDisable()
        {
            UnregisterButtons();
        }

        void Update()
        {
            if (!Keyboard.current.escapeKey.wasPressedThisFrame)
                return;

            var anyWasOpen = IsAnySubMenuOpen();
            HideSubMenus();

            if (!anyWasOpen && confirmExitMenu)
            {
                ShowSubMenu(confirmExitMenu);
            }

            AudioController.Instance.PlayUISound(clickSoundName);
        }

        #region Public Functions

        public void ShowSubMenu(RectTransform uiElement)
        {
            HideSubMenus();
            EnableUiElement(uiElement);
            SetBackDropActive(true);
        }

        public void HideSubMenus()
        {
            foreach (var tuple in menusAndButtons)
            {
                DisableUiElement(tuple.menu);
            }

            SetBackDropActive(false);
        }

        public void EnableUiElement(RectTransform uiElement)
        {
            if (uiElement)
                uiElement.gameObject.SetActive(true);
        }

        public void DisableUiElement(RectTransform uiElement)
        {
            if (uiElement)
                uiElement.gameObject.SetActive(false);
        }

        public void ToggleUiElementActive(RectTransform uiElement)
        {
            uiElement.gameObject.SetActive(!uiElement.gameObject.activeSelf);
        }

        #endregion

        #region Private Functions

        private void SetupSubMenus()
        {
            foreach (var tuple in menusAndButtons)
            {
                var canvas = tuple.menu ? tuple.menu.GetComponent<Canvas>() : null;
                if (!canvas)
                    continue;

                // Add menu to foreground, just to be sure
                var wasActive = canvas.gameObject.activeSelf;
                canvas.gameObject.SetActive(true);
                canvas.overrideSorting = true;
                canvas.sortingOrder = 5;
                canvas.gameObject.SetActive(wasActive);
            }
        }

        private void RegisterButtons()
        {
            // Cleanup
            UnregisterButtons();

            _registeredCallbacks = new List<ButtonFuncTuple>();
            foreach (var tuple in menusAndButtons)
            {
                if (!tuple.button || !tuple.menu)
                {
                    Debug.LogWarning($"Menu/Button mapping incomplete for ({tuple.menu} , {tuple.button})");
                    continue;
                }

                var func = new UnityAction(delegate
                {
                    ShowSubMenu(tuple.menu);
                    if (addClickToAllButtons)
                    {
                        AudioController.Instance.PlaySound(clickSoundName);
                    }
                });

                tuple.button.onClick.AddListener(func);

                var reg = new ButtonFuncTuple
                {
                    button = tuple.button, func = func
                };
                _registeredCallbacks.Add(reg);
            }
        }

        private void UnregisterButtons()
        {
            if (_registeredCallbacks == null)
                return;

            foreach (var reg in _registeredCallbacks)
            {
                reg.button.onClick.RemoveListener(reg.func);
            }
        }

        private bool IsAnySubMenuOpen()
        {
            return menusAndButtons.Any(tuple => tuple.menu && tuple.menu.gameObject.activeInHierarchy);
        }

        private void SetBackDropActive(bool activeState)
        {
            if (!backDropImage)
                return;

            if (activeState)
            {
                EnableUiElement(backDropImage);
            }
            else
            {
                DisableUiElement(backDropImage);
            }
        }

        #endregion

        // For backwards compatibility, use traditional structs instead of Tuples
        [Serializable]
        private struct MenuButtonTuple
        {
            public RectTransform menu;
            public Button button;
        }

        private struct ButtonFuncTuple
        {
            public Button button;
            public UnityAction func;
        }
    }
}