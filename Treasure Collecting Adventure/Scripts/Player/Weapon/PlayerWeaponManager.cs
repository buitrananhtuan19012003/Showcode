using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWeapon
{
    public WeaponDataSO WeaponData;
    public int Index;

    public SaveWeapon(WeaponDataSO weaponData, int index)
    {
        this.WeaponData = weaponData;
        this.Index = index;
    }
}

public class PlayerWeaponManager : PlayerWeaponAbstract
{
    public static PlayerWeaponManager Instance;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private int maxEquippedWeapon = 3;
    [SerializeField] private int maxBackpackWeapon = 3;
    [SerializeField] private NullAwareList<Weapon> equippedWeapons = new NullAwareList<Weapon>();
    [SerializeField] private NullAwareList<Weapon> backpackWeapons = new NullAwareList<Weapon>();
    [SerializeField] private Transform[] weaponSheathSlots = new Transform[3];

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float dropForce = 3f;
    [SerializeField] private GameObject pickupObjectPrefab;

    private List<SaveWeapon> saveEquippedWeapon = new List<SaveWeapon>();
    private List<SaveWeapon> saveBackpackWeapon = new List<SaveWeapon>();

    private int curWeaponIndex = -1;
    private bool isReadySwap = true;

    private bool isHolstering;
    public bool originalHolster = false;
    public bool IsHolstering { get => this.isHolstering; }
    [SerializeField] private float swapCooldown = 0.2f;

    public NullAwareList<Weapon> EquippedWeapons { get => this.equippedWeapons; }
    public NullAwareList<Weapon> BackpackWeapons { get => this.backpackWeapons; }
    public Transform[] WeaponSheathSlots { get => this.weaponSheathSlots; set => this.weaponSheathSlots = value; }
    public int CurWeaponIndex { get => this.curWeaponIndex; }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.playerWeapon == null)
            playerWeapon = transform.GetComponentInParent<PlayerWeapon>();
    }

    protected override void Awake()
    {
        base.Awake();
        PlayerWeaponManager.Instance = this;

        this.equippedWeapons.GenerateList(this.maxEquippedWeapon);
        this.backpackWeapons.GenerateList(this.maxBackpackWeapon);
        isHolstering = false;
    }


    public void HandleUpdateWeaponManager()
    {
        this.SetOffset();
        this.SetHolster(false);
        this.SetDefaultCurrentindex();
        this.HosterAnimation();
       // this.SetCanSwitchWeapon();
    }

    public void SaveWeapon()
    {
        for (int i = 0; i < this.equippedWeapons.GetList().Count; i++)
        {
            if (this.equippedWeapons.GetList()[i] != null)
            {
                SaveWeapon saveEquipped = new SaveWeapon(this.equippedWeapons.GetList()[i].WeaponData, i);
                this.saveEquippedWeapon.Add(saveEquipped);
            }
        }

        for (int i = 0; i < this.backpackWeapons.GetList().Count; i++)
        {
            if (this.backpackWeapons.GetList()[i] != null)
            {
                SaveWeapon saveBackpack = new SaveWeapon(this.backpackWeapons.GetList()[i].WeaponData, i);
                this.saveBackpackWeapon.Add(saveBackpack);
            }
        }
    }

    public void LoadWeapon()
    {
        //if (this.playerWeapon.PlayerCtrl.Character == null) return;

        foreach (SaveWeapon saveEquipped in this.saveEquippedWeapon)
        {
            Weapon weapon = Instantiate(saveEquipped.WeaponData.Model).GetComponent<Weapon>();
            this.AddWeaponToEquipped(weapon, saveEquipped.Index);
            if (saveEquipped.Index == this.curWeaponIndex)
            {
                this.SetActiveWeapon(this.curWeaponIndex, false);
            }
        }

        foreach (SaveWeapon saveBackpack in this.saveBackpackWeapon)
        {
            Weapon weapon = Instantiate(saveBackpack.WeaponData.Model).GetComponent<Weapon>();
            this.AddWeaponToBackpack(weapon, saveBackpack.Index);
        }

        this.UpdateUI();
    }

    public bool AddWeapon(Weapon weapon)
    {
        if (this.equippedWeapons.GetList().Count < this.maxEquippedWeapon || (this.equippedWeapons.IsContainsNull() && this.equippedWeapons.GetList().Count <= this.maxEquippedWeapon))
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_WEA_PICKUP_METALITEMPICKUP);
            }
            this.AddWeaponToEquipped(this.GetNewWeapon(weapon));
            this.UpdateUI();
            return true;
        }
        if (this.backpackWeapons.GetList().Count < this.maxBackpackWeapon || (this.backpackWeapons.IsContainsNull() && this.backpackWeapons.GetList().Count <= this.maxBackpackWeapon))
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_WEA_PICKUP_METALITEMPICKUP);
            }
            this.AddWeaponToBackpack(this.GetNewWeapon(weapon));
            this.UpdateUI();
            return true;
        }
        Debug.Log("Can't hold any more weapon");
        return false;
    }

    private Weapon GetNewWeapon(Weapon weapon)
    {
        Weapon w = Instantiate(weapon);
        return w;
    }

    private void AddWeaponToEquipped(Weapon weapon)
    {
        this.AddWeaponToEquipped(weapon, -1);
    }

    private void AddWeaponToEquipped(Weapon weapon, int index)
    {
        WeaponRaycast raycastWeapon = weapon.GetComponent<WeaponRaycast>();
        if (raycastWeapon != null)
        {
            raycastWeapon.recoil.playerFPSCam = playerWeapon.PlayerCtrl.PlayerCamera.FPSCamera;
            raycastWeapon.recoil.playerTPSCam = playerWeapon.PlayerCtrl.PlayerCamera.TPSCamera;
            raycastWeapon.recoil.rigController = playerWeapon.PlayerCtrl.RigAnimator;
        }

        if (index == -1)
            this.equippedWeapons.Add(weapon);
        else
            this.equippedWeapons.Add(weapon, index);

        DealDamageBox dealDamageBox = weapon.GetComponent<DealDamageBox>();
        if (dealDamageBox != null && weapon.WeaponData.WeaponType == WeaponType.Melee)
        {
            dealDamageBox.Damage = this.CalculateMeleeDamage(weapon);
            this.playerWeapon.PlayerCtrl.Character.DealDamageCtrl.ListDealDamageMelee.Add(dealDamageBox);
        }

        this.SetSheathForWeapon(weapon);
        if (this.curWeaponIndex == -1)
        {
            this.SetActiveWeapon(this.equippedWeapons.GetList().IndexOf(weapon), false);
            //SetAnimationEquip(weapon);
        }
    }

    private void AddWeaponToBackpack(Weapon weapon)
    {
        this.AddWeaponToBackpack(weapon, -1);
    }

    private void AddWeaponToBackpack(Weapon weapon, int index)
    {
        WeaponRaycast raycastWeapon = weapon.GetComponent<WeaponRaycast>();
        if (raycastWeapon != null)
        {
            raycastWeapon.recoil.playerFPSCam = playerWeapon.PlayerCtrl.PlayerCamera.FPSCamera;
            raycastWeapon.recoil.playerTPSCam = playerWeapon.PlayerCtrl.PlayerCamera.TPSCamera;
            raycastWeapon.recoil.rigController = playerWeapon.PlayerCtrl.RigAnimator;
        }

        if (index == -1)
            this.backpackWeapons.Add(weapon);
        else
            this.backpackWeapons.Add(weapon, index);

        DealDamageBox dealDamageBox = weapon.GetComponent<DealDamageBox>();
        if (dealDamageBox != null && weapon.WeaponData.WeaponType == WeaponType.Melee)
        {
            dealDamageBox.Damage = this.CalculateMeleeDamage(weapon);
            this.playerWeapon.PlayerCtrl.Character.DealDamageCtrl.ListDealDamageMelee.Add(dealDamageBox);
        }

        this.SetSheathForWeapon(weapon);
    }

    private int CalculateMeleeDamage(Weapon weapon)
    {
        int powerChr = this.playerWeapon.PlayerCtrl.Character.CharacterData.Power;
        int meleeDamage = (int)weapon.WeaponData.MeleeDamage;
        return (int)(meleeDamage * (1 + (float)(powerChr / (powerChr + 12))));
    }

    private void SetSheathForWeapon(Weapon weapon)
    {
        weapon.transform.SetParent(this.weaponSheathSlots[(int)weapon.WeaponSlot[0] - 1]); //TODO: Change to RigLayer

        if (PlayerCtrl.Instance.Character.CharacterData.BodyType == BodyType.BigSize)
        {
            weapon.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        weapon.gameObject.SetActive(false);
    }

    public void RemoveWeaponFromEquipped(int weaponIndex, bool isDrop)
    {
        Weapon weapon = this.equippedWeapons.GetList()[weaponIndex];
        this.RemoveWeaponFromEquipped(weapon, isDrop);
    }

    public void RemoveWeaponFromEquipped(Weapon weapon, bool isDrop)
    {
        this.equippedWeapons.Remove(weapon);
        if (isDrop)
        {
            this.DropWeapon(weapon);
        }
        else
        {
            Destroy(weapon.gameObject);
        }

        this.UpdateUI();
    }

    public void RemoveWeaponFromBackpack(int weaponIndex, bool isDrop)
    {
        Weapon weapon = this.backpackWeapons.GetList()[weaponIndex];
        this.RemoveWeaponFromBackpack(weapon, isDrop);
    }

    public void RemoveWeaponFromBackpack(Weapon weapon, bool isDrop)
    {
        this.backpackWeapons.Remove(weapon);
        if (isDrop)
        {
            this.DropWeapon(weapon);
        }
        else
        {
            Destroy(weapon.gameObject);
        }

        this.UpdateUI();
    }

    private void DropWeapon(Weapon weapon)
    {
        Transform playerTransform = this.playerWeapon.PlayerCtrl.PlayerTransform;
        Vector3 dropPosition = playerTransform.position + new Vector3(0, 1.35f, 0.67f);
        Quaternion dropRoation = playerTransform.rotation;

        GameObject pickupObject = Instantiate(this.pickupObjectPrefab, dropPosition, dropRoation, this.spawnPoint);
        PickupWeapons pickupWeapon = pickupObject.GetComponent<PickupWeapons>();
        pickupWeapon.SetWeapon(weapon.gameObject);

        Rigidbody weaponRigidbody = pickupObject.transform.GetComponent<Rigidbody>();
        if (weaponRigidbody != null)
        {
            float randomDropForce = Random.Range(this.dropForce - 1f, this.dropForce + 1f);
            Vector3 randomDropDirection = new Vector3(Random.Range(-0.5f, 0.6f), 0, 0);

            weaponRigidbody.AddForce((playerTransform.forward + randomDropDirection) * randomDropForce, ForceMode.Impulse);
        }
    }

    public void SwitchWeapon(WeaponList weaponListA, int weaponIndexA, WeaponList weaponListB, int weaponIndexB)
    {
        Weapon weaponA, weaponB;
        bool isTheCurrentWeaponA = false;
        bool isTheCurrentWeaponB = false;
        if (weaponListA == WeaponList.EquippedWeapons)
        {
            weaponA = this.equippedWeapons.GetList()[weaponIndexA];

            if (this.curWeaponIndex == weaponIndexA && weaponA != null)
            {
                weaponA.gameObject.SetActive(false);
                isTheCurrentWeaponA = true;
            }
        }
        else
        {
            weaponA = this.backpackWeapons.GetList()[weaponIndexA];
        }

        if (weaponListB == WeaponList.EquippedWeapons)
        {
            weaponB = this.equippedWeapons.GetList()[weaponIndexB];
            if (this.curWeaponIndex == weaponIndexB && weaponB != null)
            {
                weaponB.gameObject.SetActive(false);
                isTheCurrentWeaponB = true;
            }
        }
        else
        {
            weaponB = this.backpackWeapons.GetList()[weaponIndexB];
        }

        //bool foundWeaponA = this.equippedWeapons.GetList().Contains(weaponA) || this.backpackWeapons.GetList().Contains(weaponA);
        //bool foundWeaponB = this.equippedWeapons.GetList().Contains(weaponB) || this.backpackWeapons.GetList().Contains(weaponB);
        //if (!foundWeaponA || !foundWeaponB) return;

        List<Weapon> equippedList = this.equippedWeapons.GetList();
        List<Weapon> backpackList = this.backpackWeapons.GetList();

        //Case: Equipped -> Equipped
        if (weaponListA == WeaponList.EquippedWeapons && weaponListB == WeaponList.EquippedWeapons)
        {
            Weapon temp = weaponA;
            equippedList[weaponIndexA] = weaponB;
            equippedList[weaponIndexB] = temp;
        }

        //Case: Equipped -> Backpack
        else if (weaponListA == WeaponList.EquippedWeapons && weaponListB == WeaponList.BackpackWeapons)
        {
            equippedList[weaponIndexA] = weaponB;
            backpackList[weaponIndexB] = weaponA;
        }

        //Case: Backpack -> Equipped
        else if (weaponListA == WeaponList.BackpackWeapons && weaponListB == WeaponList.EquippedWeapons)
        {
            backpackList[weaponIndexA] = weaponB;
            equippedList[weaponIndexB] = weaponA;
        }

        //Case: Backpack -> Backpack
        else if (weaponListA == WeaponList.BackpackWeapons && weaponListB == WeaponList.BackpackWeapons)
        {
            Weapon temp = weaponA;
            backpackList[weaponIndexA] = weaponB;
            backpackList[weaponIndexB] = temp;
        }

        if (isTheCurrentWeaponA && weaponB != null)
            weaponB.gameObject.SetActive(true);
        if (isTheCurrentWeaponB && weaponA != null)
            weaponA.gameObject.SetActive(true);
    }

    public void SetActiveWeapon(int weaponIndex, bool isAlpha)
    {
        if (!SetCanSwitchWeapon(true)) return;
        if (!this.isReadySwap) return;

        List<Weapon> listEquippedWeapon = this.equippedWeapons.GetList();
        int weaponCount = listEquippedWeapon.Count;
        if (weaponCount == 0) return;

        if (isAlpha)
        {
            if (weaponIndex < 0 || weaponIndex >= weaponCount || listEquippedWeapon[weaponIndex] == null)
            {
                Debug.Log("Don't have that weapon index");
                return;
            }
        }
        else
        {
            weaponIndex = (weaponIndex % weaponCount + weaponCount) % weaponCount;
            //if (weaponIndex >= weaponCount)
            //    weaponIndex = 0;
            //else if (weaponIndex < 0)
            //    weaponIndex = weaponCount - 1;
        }
        if ((weaponIndex == this.curWeaponIndex && listEquippedWeapon[weaponIndex].gameObject?.activeSelf == true)
            && !isHolstering || listEquippedWeapon[weaponIndex] == null) return;

        if (this.curWeaponIndex > -1 && listEquippedWeapon[curWeaponIndex] != null)
        {
            listEquippedWeapon[this.curWeaponIndex].gameObject.SetActive(false);
        }


        listEquippedWeapon[weaponIndex].gameObject?.SetActive(true);

        isHolstering = false;
        if (PlayerCtrl.Instance.Character.IsSpecialSkill)
        {
            SetAnimationHolsterSpecial(listEquippedWeapon[weaponIndex]);
        }
        else
        {
            SetAnimationEquip(listEquippedWeapon[weaponIndex]);
        }



        this.curWeaponIndex = weaponIndex;

        this.UpdateUI();
        StartCoroutine(this.StartCooldown());
    }
    private IEnumerator StartCooldown()
    {
        this.isReadySwap = false;
        yield return new WaitForSeconds(this.swapCooldown);
        this.isReadySwap = true;
    }

    public void UpdateUI()
    {
        if (UIManager.HasInstance == false) return;

        //UI_InventoryPanel inventoryPanel = UIManager.Instance.InGamePanel.PauseMenu.InventoryPanel;
        //inventoryPanel.ResetSlot();
        //List<UI_DraggableItem> equippedList = inventoryPanel.DraggablesEquippedList;
        //List<UI_DraggableItem> backpackList = inventoryPanel.DraggablesBackpackList;

        //for (int i = 0; i < this.equippedWeapons.GetList().Count; i++)
        //{
        //    if (this.equippedWeapons.GetList()[i] == null)
        //    {
        //        equippedList[i].SetActiveSlot(false);
        //        continue;
        //    }
        //    equippedList[i].SetActiveSlot(true);
        //    equippedList[i].SetWeaponData(this.equippedWeapons.GetList()[i].WeaponData);
        //    equippedList[i].SetModel();
        //}
        //for (int i = 0; i < this.backpackWeapons.GetList().Count; i++)
        //{
        //    if (this.backpackWeapons.GetList()[i] == null)
        //    {
        //        backpackList[i].SetActiveSlot(false);
        //        continue;
        //    }
        //    backpackList[i].SetActiveSlot(true);
        //    backpackList[i].SetWeaponData(this.backpackWeapons.GetList()[i].WeaponData);
        //    backpackList[i].SetModel();
        //}

        if (this.curWeaponIndex == -1) return;
        //inventoryPanel.EquippedList.SetEquipSlot(this.curWeaponIndex);
    }

    public int GetCurrentWeaponIndex()
    {
        return this.curWeaponIndex;
    }
    public Weapon GetActiveWeapon()
    {
        if (this.curWeaponIndex < 0) return null;
        return this.equippedWeapons.GetList()[this.curWeaponIndex];
    }

    public WeaponRaycast GetActiveRaycastWeapon()
    {
        Weapon weapon = GetActiveWeapon();
        if (weapon == null) return null;
        WeaponRaycast activeRaycastWeapon = weapon.GetComponent<WeaponRaycast>();
        return activeRaycastWeapon;
    }

    public void HosterAnimation()
    {
        List<Weapon> listEquippedWeapon = this.equippedWeapons.GetList();
        if (curWeaponIndex > -1 && listEquippedWeapon[curWeaponIndex] == null || curWeaponIndex == -1)
        {
            playerWeapon.PlayerCtrl.RigAnimator.SetBool("holster_weapon", true);
            return;
        }
        playerWeapon.PlayerCtrl.RigAnimator.SetBool("holster_weapon", isHolstering);
    }

    public void SetHolster(bool button)
    {
        if (!SetCanSwitchWeapon(false)) return;
        List<Weapon> listEquippedWeapon = this.equippedWeapons.GetList();

        if (button && curWeaponIndex > -1 && listEquippedWeapon[curWeaponIndex] != null)
        {
            isHolstering = !isHolstering;
        }
        if (!button)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isHolstering ||
                PlayerCtrl.Instance.Character.IsSpecialSkill && Input.GetKeyDown(KeyCode.C) && !isHolstering)
            {
                originalHolster = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && isHolstering ||
                 PlayerCtrl.Instance.Character.IsSpecialSkill && Input.GetKeyDown(KeyCode.C) && isHolstering)

            {
                originalHolster = true;
            }

            if (playerWeapon.PlayerCtrl.PlayerLocomotion.IsSprinting == true && curWeaponIndex > -1 ||
                playerWeapon.PlayerCtrl.Character.IsSpecialSkill && curWeaponIndex > -1)
            {
                isHolstering = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && curWeaponIndex > -1 || PlayerCtrl.Instance.Character.TimerCD_SpecialSkill < 0.05 && PlayerCtrl.Instance.Character.IsCoolingDownSpecicalSkill)
            {
                isHolstering = originalHolster;
            }
            if (PlayerCtrl.Instance.Character.CharacterData.CharacterName == "Sera" || PlayerCtrl.Instance.Character.CharacterData.CharacterName == "Darlene")
            {
                if (PlayerCtrl.Instance.Character.TimerCD_SpecialSkill > 1 && PlayerCtrl.Instance.Character.TimerCD_SpecialSkill < 1.2 && PlayerCtrl.Instance.Character.IsCoolingDownSpecicalSkill)
                {
                    isHolstering = originalHolster;
                }
            }

        }
    }
    public void SetDefaultCurrentindex()
    {
        List<Weapon> listEquippedWeapon = this.equippedWeapons.GetList();
        if (curWeaponIndex > -1)
        {
            if (listEquippedWeapon[curWeaponIndex] == null)
            {
                curWeaponIndex = -1;
            }
        }

    }
    public void SetAnimationEquip(Weapon weapon)
    {
        if (PlayerCtrl.Instance.Character.IsSpecialSkill) return;
        if (weapon.WeaponData.WeaponType == WeaponType.Melee && weapon.WeaponData.WeaponType != WeaponType.None)
        {
            playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("equip_melee");
        }
        if (weapon.WeaponData.WeaponType == WeaponType.Pistol && weapon.WeaponData.WeaponType != WeaponType.None)
        {
            playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("equip_pistol");
        }
        else if (weapon.WeaponData.WeaponType != WeaponType.Pistol && weapon.WeaponData.WeaponType != WeaponType.None
            && weapon.WeaponData.WeaponType != WeaponType.Melee)
        {
            playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("equip");
        }
    }

    public void SetAnimationHolsterSpecial(Weapon weapon) // for character form when current index = -1; (when IsSpecialSkill)
    {
        if (PlayerCtrl.Instance.Character.IsSpecialSkill)
        {
            if (weapon.WeaponData.WeaponType == WeaponType.Melee && weapon.WeaponData.WeaponType != WeaponType.None)
            {
                playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("holster_melee");
            }
            if (weapon.WeaponData.WeaponType == WeaponType.Pistol && weapon.WeaponData.WeaponType != WeaponType.None)
            {
                playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("holster_pistol");
            }
            else if (weapon.WeaponData.WeaponType != WeaponType.Pistol && weapon.WeaponData.WeaponType != WeaponType.None
                        && weapon.WeaponData.WeaponType != WeaponType.Melee)
            {
                playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("holster");
            }

        }
    }

    public void SetAnimationHolsterIsnotSpecial(Weapon weapon) // Only Xerath when devolution
    {
        if (!PlayerCtrl.Instance.Character.IsSpecialSkill)
        {
            if (weapon.WeaponData.WeaponType == WeaponType.Melee && weapon.WeaponData.WeaponType != WeaponType.None)
            {
                playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("holster_melee");
            }
            if (weapon.WeaponData.WeaponType == WeaponType.Pistol && weapon.WeaponData.WeaponType != WeaponType.None)
            {
                playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("holster_pistol");
            }
            else if (weapon.WeaponData.WeaponType != WeaponType.Pistol && weapon.WeaponData.WeaponType != WeaponType.None
                        && weapon.WeaponData.WeaponType != WeaponType.Melee)
            {
                playerWeapon.PlayerCtrl.RigAnimator.SetTrigger("holster");
            }

        }
    }

    public void SetOffset()
    {
        Weapon weapon = GetActiveWeapon();
        if (weapon != null)
        {
            if (isHolstering)
            {
                if (PlayerCtrl.Instance.Character.CharacterData.BodyType == BodyType.BigSize)
                {
                    if (weapon.gameObject.transform.localPosition == weapon.WeaponData.BigsizePosHolster) return;
                    weapon.gameObject.transform.localPosition = weapon.WeaponData.BigsizePosHolster;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(weapon.WeaponData.BigsizeRosHolster);
                }
                else if (PlayerCtrl.Instance.Character.CharacterData.BodyType == BodyType.Short)
                {
                    if (weapon.gameObject.transform.localPosition == weapon.WeaponData.ShortPosHolster) return;
                    weapon.gameObject.transform.localPosition = weapon.WeaponData.ShortPosHolster;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(weapon.WeaponData.ShortRosHolster);
                }
                else
                {
                    if (weapon.gameObject.transform.localPosition == weapon.WeaponData.NormalPosHolster) return;
                    weapon.gameObject.transform.localPosition = weapon.WeaponData.NormalPosHolster;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(weapon.WeaponData.NormalRosHolster);
                }
            }
            if (!isHolstering)
            {
                if (PlayerCtrl.Instance.Character.CharacterData.BodyType == BodyType.BigSize)
                {
                    if (weapon.gameObject.transform.localPosition == weapon.WeaponData.BigsizePosEquip) return;
                    weapon.gameObject.transform.localPosition = weapon.WeaponData.BigsizePosEquip;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(weapon.WeaponData.BigsizeRosEquip);
                }
                else if (PlayerCtrl.Instance.Character.CharacterData.BodyType == BodyType.Short)
                {
                    if (weapon.gameObject.transform.localPosition == weapon.WeaponData.ShortPosEquip) return;
                    weapon.gameObject.transform.localPosition = weapon.WeaponData.ShortPosEquip;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(weapon.WeaponData.ShortRosEquip);
                }
                else
                {
                    if (weapon.gameObject.transform.localPosition == weapon.WeaponData.NormalPosEquip) return;
                    weapon.gameObject.transform.localPosition = weapon.WeaponData.NormalPosEquip;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(weapon.WeaponData.NormalRosEquip);
                }
            }
        }
    }

    public bool SetCanSwitchWeapon(bool forEquip)
    {
        if (forEquip)
        {
            if (playerWeapon.PlayerCtrl.PlayerAim.IsAim == true || playerWeapon.PlayerWeaponReload.IsReload == true || playerWeapon.PlayerWeaponAttack.IsAttack == true || PlayerCtrl.Instance.Character.IsSpecialSkill)
            {
                return false;
            }
            return true;
        }
        else
        {
            if (playerWeapon.PlayerCtrl.PlayerAim.IsAim == true || playerWeapon.PlayerWeaponReload.IsReload == true || playerWeapon.PlayerWeaponAttack.IsAttack == true)
            {
                return false;
            }
            return true;
        }
    }
}
