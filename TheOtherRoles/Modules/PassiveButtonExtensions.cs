using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheOtherRoles.Modules
{
    public static class PassiveButtonExtensions
    {
        public static PassiveButton MakePassiveButton(this GameObject gameObject, Action onClick, bool rollover = true)
        {
            var button = gameObject.AddComponent<PassiveButton>();
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnMouseOut = new Button.ButtonClickedEvent();
            button.OnMouseOver = new Button.ButtonClickedEvent();

            button.OnClick.AddListener(onClick);

            if (rollover)
            {
                if (gameObject.GetComponent<SpriteRenderer>() == null)
                    throw new InvalidOperationException(
                        "Cannot add ButtonRolloverHandler to gameObject, as it doesn't have a SpriteRenderer"
                    );
                var rolloverHandler = gameObject.AddComponent<ButtonRolloverHandler>();
                rolloverHandler.Target = gameObject.GetComponent<SpriteRenderer>();
                rolloverHandler.OutColor = Color.white;
                rolloverHandler.OverColor = Color.green;
                rolloverHandler.HoverSound = null;
            }

            return button;
        }
    }
}
