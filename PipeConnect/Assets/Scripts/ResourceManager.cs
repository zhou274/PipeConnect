using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class ResourceManager : Singleton<ResourceManager>
{
  
    public static event Action<int> CoinsChanged; 
#if IN_APP
    public static event Action<string> ProductPurchased;
    public static event Action<bool> ProductRestored;
#endif

   

    public static bool EnableAds
    {
        get => PrefManager.GetBool(nameof(EnableAds), true);
        set => PrefManager.SetBool(nameof(EnableAds), value);
    }

    public static string NoAdsProductId => GameSettings.Default.InAppSetting.removeAdsId;

    public static int Coins
    {
        get => PrefManager.GetInt(nameof(Coins));
        set
        {
            PrefManager.SetInt(nameof(Coins),value);
            CoinsChanged?.Invoke(value);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
#if IN_APP
        Purchaser = new Purchaser(new List<string>(), new[] { NoAdsProductId });
        Purchaser.RestorePurchased += PurchaserOnRestorePurchased;
#endif
    }

#if IN_APP




    public static bool AbleToRestore => EnableAds;

    public Purchaser Purchaser { get; private set; }

  
    private void PurchaserOnRestorePurchased(bool success)
    {
        if (EnableAds && Purchaser.ItemAlreadyPurchased(NoAdsProductId))
        {
            EnableAds = false;
            ProductPurchased?.Invoke(NoAdsProductId);
        }
        ProductRestored?.Invoke(success);
    }


    public static void RestorePurchase()
    {
        Instance.Purchaser.Restore();
    }

    public static void PurchaseNoAds(Action<bool> completed = null)
    {
        if (!EnableAds)
        {
            return;
        }

        Instance.Purchaser.BuyProduct(NoAdsProductId, success =>
        {
            if (success)
            {
                EnableAds = false;
            }
            completed?.Invoke(success);
            if (success)
                ProductPurchased?.Invoke(NoAdsProductId);
        });
    }
#endif
}


public partial class ResourceManager
{

    public static IEnumerable<LevelGroup> LevelGroups => global::LevelGroups.Default;

    public static LevelGroup GetLevelGroup(string groupID)
    {
        return LevelGroups.First(group => @group.id == groupID);
    }


    public static Level GetLevel(string groupId, int level)
    {
        return GetLevelGroup(groupId).levels.FirstOrDefault(l => l.no == level);
    }

    public static bool IsLevelLocked(string groupId, int no)
    {
        var completedLevel = GetCompletedLevel(groupId);

        return no > completedLevel + 1;
    }

    public static int GetCompletedLevel(string groupID)
    {
       return PrefManager.GetInt($"{groupID}_Level_Complete");
    }

    public static void CompleteLevel(string groupId, int lvl)
    {
        if (GetCompletedLevel(groupId)>=lvl)
        {
            return;
        }

        PrefManager.SetInt($"{groupId}_Level_Complete",lvl);
    }

    public static bool HasLevel(string groupID, int lvl) => GetLevelGroup(groupID).Count() >= lvl;

  
}

public enum GameMode
{
    Easy,Normal,Hard
}