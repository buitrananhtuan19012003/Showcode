using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LupinrangerPatranger.CharacterSystem
{

    public interface ITriggerSelectSellCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when an item is selected for sell
        /// </summary>
        void OnSelectSellCharacter(Player player, GameObject players);
    }

    public interface ITriggerSoldCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when an item was sold
        /// </summary>
        void OnSoldCharacter(Player player, GameObject players);
    }

    public interface ITriggerFailedToSellCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when an item couldn't be sold
        /// </summary>
        void OnFailedToSellCharacter(Player player, GameObject players, Trigger.FailureCause failureCause);
    }

    public interface ITriggerSelectBuyCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when an item is selected for purchase
        /// </summary>
        void OnSelectBuyCharacter(Player player, GameObject players);
    }

    public interface ITriggerBoughtCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when an item was bought
        /// </summary>
        void OnBoughtCharacter(Player player, GameObject players);
    }

    public interface ITriggerFailedToBuyCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when a purchase failed(Often because no currency or if the inventory is full)
        /// </summary>
        void OnFailedToBuyCharacter(Player player, GameObject players, Trigger.FailureCause failureCause);
    }

    public interface ITriggerCraftStart : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when the user starts crafting
        /// </summary>
        void OnCraftStart(Player player, GameObject players);
    }

    public interface ITriggerFailedCraftStart : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when craft start failed, for example the player has no ingredients or if he is already crafting
        /// </summary>
        void OnFailedCraftStart(Player player, GameObject players, Trigger.FailureCause failureCause);
    }

    public interface ITriggerCraftCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when the user crafted an item
        /// </summary>
        void OnCraftCharacter(Player player, GameObject players);
    }

    public interface ITriggerFailedToCraftCharacter : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when the user failed to craft item
        /// </summary>
        void OnFailedToCraftCharacter(Player player, GameObject players, Trigger.FailureCause failureCause);
    }

    public interface ITriggerCraftStop : ITriggerEventHandler
    {
        /// <summary>
        /// Use this callback to detect when the user stops crafting
        /// </summary>
        void OnCraftStop(Player player, GameObject players);
    }
}