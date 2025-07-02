using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace LTH
{
    public class ConfirmPopup : BasePopup
    {
        [Header("Message Text")]
        [SerializeField] private TMP_Text messageText;

        [Header("Confirm UI")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_Text confirmButtonText;

        [Header("Cancel UI")]
        [SerializeField] private Button cancelButton;
        [SerializeField] private TMP_Text cancelButtonText;

        private Action onConfirm;
        private Action onCancel;

        public override void Open()
        {
            base.Open();
        }

        public void Show(string message, string confirmText, Action confirm, string cancelText = null, Action cancel = null)
        {
            messageText.text = message;
            confirmButtonText.text = confirmText;
            onConfirm = confirm;

            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() =>
            {
                onConfirm?.Invoke();
                Close();
            });

            if (!string.IsNullOrEmpty(cancelText))
            {
                cancelButtonText.text = cancelText;
                onCancel = cancel;
                cancelButton.gameObject.SetActive(true);

                cancelButton.onClick.RemoveAllListeners();
                cancelButton.onClick.AddListener(() => {
                    onCancel?.Invoke();
                    Close();
                });
            }
            else
            {
                cancelButton.gameObject.SetActive(false);
            }
        }
    }
}

