using MelonLoader;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Upgrades;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using UnityEngine;
using Il2CppTMPro;

[assembly: MelonInfo(typeof(price_changer.Main), "price_changer", "1.0.0", "you")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace price_changer
{
    public class Main : MelonMod
    {
        private bool inputFixApplied;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("price_changer loaded");
        }

        public override void OnUpdate()
        {
            // Fix input field safely (only once when needed)
            if (!inputFixApplied)
            {
                var input = Object.FindObjectOfType<TMP_InputField>();
                if (input != null)
                {
                    input.characterValidation = TMP_InputField.CharacterValidation.None;
                    inputFixApplied = true;
                }
            }

            // Hotkey
            if (Input.GetKeyDown(KeyCode.F11))
            {
                ShowPricePopup();
            }
        }

        private void ShowPricePopup()
        {
            if (PopupScreen.instance == null)
                return;

            Il2CppSystem.Action<string> mod = new Il2CppSystem.Action<string>((string s) =>
            {
                float multi;

                if (!float.TryParse(s, out multi))
                    return;

                // Global game model
                foreach (TowerModel tower in Game.instance.model.towers)
                    tower.cost *= multi;

                foreach (UpgradeModel upgrade in Game.instance.model.upgrades)
                    upgrade.cost = (int)(upgrade.cost * multi);
            });

            PopupScreen.instance.ShowSetNamePopup(
                "Price Changer",
                "Multiply prices by",
                mod,
                "1.56"
            );
        }
    }
}
