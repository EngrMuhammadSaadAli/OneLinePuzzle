//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Purchasing;
//using System;
////using UnityEngine.Purchasing.Security;
//using UnityEngine.SceneManagement;

//[Serializable]
//public class StoreIds
//{
//    public string Name;
//    public string Id;
//    public int TotalHint;//Hints get on in app completed....
//}

//public class IAPManager : MonoBehaviour, IStoreListener
//{

//    public static IAPManager Instance;

//    [Header("Product")]
//    public List<StoreIds> StoreProductIds = new List<StoreIds>();

//    public StoreIds GetStoreIds(string id)
//    {
//        StoreIds storeId = null;

//        for (int i = 0; i < StoreProductIds.Count; i++)
//        {
//            if (id == StoreProductIds[i].Id)
//            {
//                storeId = StoreProductIds[i];
//                break;
//            }
//        }
//        return storeId;
//    }

//    internal static string removeAdsProductId = "remove_ads";
//    internal static string CompletePackProductId = "complete_pack";

//    //private static IStoreController m_StoreController;
//    //// The Unity Purchasing system.
//    //private static IExtensionProvider m_StoreExtensionProvider;
//    //// The store-specific Purchasing subsystems.

//    string currentIAPProductId;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//    }


//    void Start()
//    {
//        // If we haven't set up the Unity Purchasing reference
//        //if (m_StoreController == null)
//        //{
//        //    // Begin to configure our connection to Purchasing
//        //    InitializePurchasing();
//        //}
//    }

//    public void InitializePurchasing()
//    {
//        // If we have already connected to Purchasing ...
//        if (IsInitialized())
//        {
//            // ... we are done here.
//            return;
//        }

//        // Create a builder, first passing in a suite of Unity provided stores.
//        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


//        for (int i = 0; i < StoreProductIds.Count; i++)
//        {
//            builder.AddProduct(StoreProductIds[i].Id, ProductType.Consumable);
//        }

//#if UNITY_ANDROID || UNITY_IOS
//        builder.AddProduct(removeAdsProductId, ProductType.NonConsumable);
//        builder.AddProduct(CompletePackProductId, ProductType.NonConsumable);
//#endif

//        UnityPurchasing.Initialize(this, builder);
//    }


//    private bool IsInitialized()
//    {
//        return m_StoreController != null && m_StoreExtensionProvider != null;
//    }


//    public void BuyProductItem(string productId)
//    {
//        if (Application.internetReachability == NetworkReachability.NotReachable)
//        {
//            return;
//        }

//        currentIAPProductId = productId;

//        BuyProductID(productId);
//    }


//    public void RemoveAds()
//    {
//        if (Application.internetReachability == NetworkReachability.NotReachable)
//        {
//            return;
//        }

//        currentIAPProductId = removeAdsProductId;

//        BuyProductID(removeAdsProductId);
//    }

//    void BuyProductID(string productId)
//    {
//        if (IsInitialized())
//        {
//            Product product = m_StoreController.products.WithID(productId);
//            if (product != null && product.availableToPurchase)
//            {
//                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//                m_StoreController.InitiatePurchase(product);
//            }
//            else
//            {
//                //Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//            }
//        }
//        else
//        {
//            //Debug.Log("BuyProductID FAIL. Not initialized.");
//        }
//    }



//    public void RestorePurchases()
//    {
//        //Debug.Log("Restore!##$");
//        // If Purchasing has not yet been set up ...
//        if (!IsInitialized())
//        {

//            //Debug.Log("RestorePurchases FAIL. Not initialized.");
//            return;
//        }

//        // If we are running on an Apple device ... 
//        if (Application.platform == RuntimePlatform.IPhonePlayer ||
//            Application.platform == RuntimePlatform.OSXPlayer)
//        {
//            // ... begin restoring purchases
//            //Debug.Log("RestorePurchases started ...");

//            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

//            apple.RestoreTransactions((result) =>
//            {
//                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//            });
//        }
//        // Otherwise ...
//        else
//        {
//            // We are not running on an Apple device. No work is necessary to restore purchases.
//            //Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//        }
//    }


//    //
//    // --- IStoreListener
//    //

//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {

//        //Debug.Log("OnInitialized: PASS");
//        // Overall Purchasing system, configured with products for this application.
//        m_StoreController = controller;
//        // Store specific subsystem, for accessing device-specific store features.
//        m_StoreExtensionProvider = extensions;
//    }


//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//    }



//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//    {
//        bool validPurchase = true;
//#if !UNITY_EDITOR
//#if UNITY_ANDROID || UNITY_IOS

//        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

//        try
//        {

//            var result = validator.Validate(args.purchasedProduct.receipt);

//        }
//        catch (IAPSecurityException)
//        {
//            validPurchase = false;
//        }
//#endif
//#endif
//        if (validPurchase)
//        {
//            if (String.Equals(args.purchasedProduct.definition.id, currentIAPProductId, StringComparison.Ordinal))
//            {
//                StoreIds storeId = GetStoreIds(args.purchasedProduct.definition.id);

//                if (storeId != null)
//                {
//                    if (storeId.Id == args.purchasedProduct.definition.id)
//                    {
//                        OnConsumableBuySuccess(storeId.TotalHint);
//                    }
//                }
//                else
//                {
//                    if (args.purchasedProduct.definition.id == removeAdsProductId)
//                    {
//                        OnNoAdsCompleted();
//                    }
//                    else if (args.purchasedProduct.definition.id == CompletePackProductId)
//                    {
//                        OnCompletePack();
//                    }
//                }
//            }
//            else
//            {
//                //Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//            }
//        }
//        return PurchaseProcessingResult.Complete;
//    }


//    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//    {
//        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
//        // this reason with the user to guide their troubleshooting actions.
//        //Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//    }


//    public string GetPrice(string productId)
//    {
//        return m_StoreController.products.WithID(productId).metadata.localizedPriceString;
//    }

//    public bool IsAlreadyPurchased(string productId)
//    {
//        return m_StoreController.products.WithID(productId).hasReceipt;
//    }

//    private void OnConsumableBuySuccess(int hints)
//    {
//        GameData.getInstance().tipRemain += hints;
//        if (MainMenuPanel.Instance)
//        {
//            MainMenuPanel.Instance.IAPPanel.ShowHintNumber();
//            MainMenuPanel.Instance.giftPanel.ShowGiftText("CONGRATULATIONS\n" + "YOUR NUMBER OF HINTS INCREASED TO " + GameData.getInstance().tipRemain + ".");
//            MainMenuPanel.Instance.giftPanel.gameObject.SetActive(true);
//        }
//        else if (SceneManager.GetActiveScene().name == "Game")
//        {
//            GameData.getInstance().level.OnHintPurchase();
//        }
//        FirebaseEventHandler.PurchaseEvent("Purchase " + hints.ToString());
//    }

//    private void OnNoAdsCompleted()
//    {
//        GameData.getInstance().IsAdsAvailable = false;
//        MainMenuPanel.Instance.IAPPanel.ShowHintNumber();
//        MainMenuPanel.Instance.giftPanel.ShowGiftText("CONGRATULATIONS\n" + "NO ADS SHOWN \n FROM NOW!");
//        MainMenuPanel.Instance.giftPanel.gameObject.SetActive(true);
//        MainMenuPanel.Instance.IAPPanel.checkForPurchased();
//        GameData.getInstance().RemoveAds = true;
//        FirebaseEventHandler.PurchaseEvent("Purchase Remove ADs");
//    }

//    void OnCompletePack()
//    {
//        OnNoAdsCompleted();
//        LevelDataHandler.Instance.levelData.UnlockAllLevelsOfType(LevelType.Basic);
//        MainMenuPanel.Instance.IAPPanel.ShowHintNumber();
//        MainMenuPanel.Instance.giftPanel.ShowGiftText("CONGRATULATIONS\n" + "No ADS SHOWN AND ALL BASIC LEVEL UNLOCKED!");
//        MainMenuPanel.Instance.giftPanel.gameObject.SetActive(true);
//        MainMenuPanel.Instance.IAPPanel.checkForPurchased();
//        GameData.getInstance().UnlockCompletePack = true;
//        FirebaseEventHandler.PurchaseEvent("Purchase Complete Pack");
//    }
// }
