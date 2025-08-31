using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using HarmonyLib;

#if ANDROID
namespace TheOtherRoles.Patches
{
    internal class AuthFix
    {
        [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.LoginWithCorrectPlatformImpl))]
        public static class AuthPatch
        {
            public static bool Prefix(EOSManager __instance, OnLoginCallback successCallbackIn)
            {
                LoginOptions loginOptions2 = new LoginOptions
                {
                    Credentials = new (new Credentials
                    {
                        Token = new Utf8String("DUMMY"),
                        Type = (ExternalCredentialType)15
                    })
                };
                __instance.PlatformInterface.GetConnectInterface().Login(ref loginOptions2, null, successCallbackIn);
                __instance.stopTimeOutCheck = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(StoreManager), nameof(StoreManager.InitiateStorePurchaseStar))]
        public static class DisableStarBuyPatch
        {
            public static bool Prefix()
            {
                PurchasePopUp purchasePopUp = DestroyableSingleton<StoreMenu>.Instance.plsWaitModal;
                purchasePopUp.waitingText.gameObject.SetActive(false);
                purchasePopUp.titleText.text = "NOT SUPPORTED";
                purchasePopUp.infoText.text = "Platform Purchases are not supported in Starlight.\nBuy in the vanilla client instead.";
                purchasePopUp.infoText.gameObject.SetActive(true);
                purchasePopUp.controllerFocusHolder.gameObject.SetActive(true);
                purchasePopUp.closeButton.gameObject.SetActive(true);
                return false;
            }
        }
    }
}
#endif
