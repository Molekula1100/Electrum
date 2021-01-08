using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private const float SPEED = 1f;
    private bool inCol;
    private float playerOrigScale, duration = 5f;

    public static float speedScale = 1f, prevUseSpeedPotionTime, playerHealth, playerMaxHealth = 5;
    public static bool needChangeSpeedScale;
    public static string uniqName;

    private JoystickFirst joystickFirst;
    private Animator anim;
    private Transform hand;
    private Collider2D otherCollider;

    [SerializeField] protected Healthbar healthbar;

    public WeaponTypes weaponType;
    public Coin coinInv;
    public Coin coinShop;
    public GameObject PickUpButton, shopButton, equipedweapon;
    public Shop shop;

    public static Vector2 playerVector;
    public static InventoryManager inventoryManager;
 
    //private HeroClasses heroClass;
    void Awake()
    {
        uniqName = gameObject.name;
    }

    private void Start()
    {
        needChangeSpeedScale = false;
        playerHealth = 5;
        healthbar.Set(playerHealth);
        playerOrigScale = gameObject.transform.localScale.x;
        hand = gameObject.transform.GetChild(0);
        anim = GetComponent<Animator>();
        inventoryManager = GameObject.Find("GameController").GetComponent<InventoryManager>();
        joystickFirst = GameObject.Find("Joystick1").GetComponent<JoystickFirst>();
        PickUpButton.SetActive(false);
        shopButton.SetActive(false);
        
        switch (MenuManager.selectedHeroClass) //from menu hero selection this function selects player sprite
        {        
            case HeroClasses.Knight:
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("KnightController");
                break;

            case HeroClasses.Thief:
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("ThiefController");
                break;

            case HeroClasses.Wizard:
                    anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("WizardController");
                break;
            default:
                break;
               
        }
    }
    
    private void Update()
    {        
        if(Input.GetKeyDown(KeyCode.E))
        {
            OpenCloseInventory();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwapWeapon();
        }
        if(inCol)
        {
            OnCollisionCheck(otherCollider);
            if(otherCollider != null && (otherCollider.gameObject.tag == "Item" || otherCollider.gameObject.name.StartsWith("Weapon")))
            {
                PickUpButton.SetActive(true);
            }
            else if(otherCollider != null && (otherCollider.gameObject.tag == "shop"))
            {
                shopButton.SetActive(true);
            }
        }
        else
        {
            PickUpButton.SetActive(false);
            shopButton.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(needChangeSpeedScale && Time.time > prevUseSpeedPotionTime + duration)
        {
            speedScale = 1f;
            needChangeSpeedScale = false;
        }
        if (!JoystickFirst.isFree)
        {
            float v = joystickFirst.Vertical;
            float h = joystickFirst.Horizontal;
            //print(SPEED);
            playerVector = (new Vector2(h, v) * Time.deltaTime) * SPEED * speedScale;
            transform.Translate(playerVector);
            if(h<0)
            {
                gameObject.transform.localScale = new Vector3(-playerOrigScale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
            else if(h>0)
            {
                gameObject.transform.localScale = new Vector3(playerOrigScale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }
        else
        {
            playerVector = Vector2.zero;
            anim.SetBool("isRunning", false);
        }
        if(playerVector != Vector2.zero)
        {
            anim.SetBool("isRunning", true);
        }
        if(playerHealth <= 0)
        {
            RoomGenerator.SpawnedObj.Clear();
            GetComponent<AudioSource>().Play();
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        otherCollider = col;
        inCol = true;

        if(col.gameObject.CompareTag("Portal")
        && GameObject.Find("GameController").GetComponent<GameManager>().allEnemiesKilled)
        {
            Invoke("Teleportation", 0.5f);
        }

        if(col.gameObject.CompareTag("enemyBullet"))
        {
            playerHealth -= col.gameObject.transform.parent.GetComponent<EnemyBehaviour>().Damage;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        inCol = false;
    }

    private void EquipWeapon(Collider2D col)
    {
        if(!Inventory.CheckWeapon(1))
        {
            Inventory.Equip(col.gameObject.GetComponent<Weapon>(), 1);
            SetWeapon(col.gameObject);
        }
        else if(!Inventory.CheckWeapon(2))
        {
            Inventory.Equip(col.gameObject.GetComponent<Weapon>(), 2);
            SetWeapon(col.gameObject);
        }
        else
        {
            Inventory.Equip(col.gameObject.GetComponent<Weapon>(), equipedweapon.GetComponent<Weapon>().number);
            equipedweapon.SetActive(true);
            Player.DropItem(equipedweapon);  
            equipedweapon = null;
            SetWeapon(col.gameObject); 
        }
        for(int i = 0; i < Inventory.weapons.Length; i++)
        {
            if(Inventory.weapons[i] != null)
            {
                inventoryManager.weaponSlots[i].item = Inventory.weapons[i];
                inventoryManager.weaponSlots[i].Respawn();
            }
        }
    }

    public void Attack()
    {      
        switch (weaponType)
        {
            case WeaponTypes.Wand:
                equipedweapon.GetComponent<Wand>().Strike();
                break;
            case WeaponTypes.Staff:
                equipedweapon.GetComponent<Staff>().Strike();
                break;
            case WeaponTypes.Sword:
                equipedweapon.GetComponent<Sword>().Strike();
                break;
        }
    }

    public void SetWeapon(GameObject col)
    {
        if(equipedweapon != null) 
        {
            equipedweapon.SetActive(false);
            equipedweapon.GetComponent<Weapon>().isEquiped = false;
        }
        equipedweapon = col;
        col.transform.SetParent(gameObject.transform);
        col.transform.position = new Vector3(hand.position.x, hand.position.y, -1f);
        col.GetComponent<Weapon>().isEquiped = true;
        equipedweapon.SetActive(true);
        weaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), col.tag); 
    }

    public static void DropItem(GameObject item)
    {
        if(Inventory.items.Contains(item.GetComponent<Item>()))
        {
           // int index = Inventory.items.FindIndex(a => a == item.GetComponent<Item>());
            //Inventory.items[index] = null;
            Inventory.items.Remove(item.GetComponent<Item>());
        }
        item.transform.parent = null;
        item.SetActive(true);
        if(item.GetComponent<Weapon>() != null) 
        {
            item.GetComponent<Weapon>().isEquiped = false;
        }
        else if(item.GetComponent<Item>() == Inventory.weapons[0])
        {
            Inventory.weapons[0] = null;
        }
        else if(item.GetComponent<Item>() == Inventory.weapons[1])
        {
            Inventory.weapons[1] = null;
        }
        inventoryManager.UpdateInventory();
    }

    public static void UseItem(GameObject item)
    {
        item.GetComponent<UsableItem>().Effect();
        Inventory.items.Remove(item.GetComponent<Item>());
        inventoryManager.UpdateInventory();
        Destroy(item.gameObject);
    }

    public void BuyItem(Item item)
    {
        if(item.price <= Inventory.coins)
        {
            GameObject g = Instantiate(item.gameObject, transform.position, Quaternion.identity);
            g.transform.parent = transform;
            g.SetActive(false);
            Inventory.AddToInventory(g.GetComponent<Item>());
            Shop.items.Remove(item);
            shop.Respawn();
            Inventory.coins -= item.price;
            coinShop.UpdateText();
        }
    }

    public void SwapWeapon()
    {
        if(equipedweapon.GetComponent<Weapon>().number == 1)
        {
            SetWeapon(Inventory.weapons[1].gameObject);
        }
        else
        {
            SetWeapon(Inventory.weapons[0].gameObject);
        }
    }

    private void OnCollisionCheck(Collider2D col)
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (col.gameObject.name.StartsWith("Weapon"))
            {  
                EquipWeapon(col);
            }
            else if(col.gameObject.tag == "Item")
            {
                Inventory.AddToInventory(col.gameObject.GetComponent<Item>());
                col.gameObject.transform.position = transform.position;
                col.gameObject.transform.parent = transform;
                col.gameObject.SetActive(false);
            }
        }
        if(col != null && col.gameObject.tag == "shop" && Input.GetKeyDown(KeyCode.K))
        {
            Shop.ShopPanel.SetActive(!Shop.ShopPanel.activeSelf);
        }
    }

    public void CloseShop()
    {
        Shop.ShopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        if(otherCollider.gameObject.tag == "shop" && inCol)
        {
            Shop.ShopPanel.SetActive(true);
            coinShop.UpdateText();
            coinInv.UpdateText();
        }
    }

    public void OpenCloseInventory()
    {
        InventoryManager.inventoryPanel.SetActive(!InventoryManager.inventoryPanel.activeSelf);
        coinShop.UpdateText();
        coinInv.UpdateText();
    }

    public void PickUp()
    {
        if(inCol)
        {
            if (otherCollider.gameObject.name.StartsWith("Weapon"))
            {  
                EquipWeapon(otherCollider);
            }
            else if(otherCollider.gameObject.tag == "Item")
            {
                Inventory.AddToInventory(otherCollider.gameObject.GetComponent<Item>());
                otherCollider.gameObject.transform.position = transform.position;
                otherCollider.gameObject.transform.parent = transform;
                otherCollider.gameObject.SetActive(false);
            }
        }
    }

    void Teleportation()
    {
        RoomGenerator.SpawnedObj.Clear();
        transform.position = Vector2.zero;
        GameManager.readyToSpawn = true;
    }
}
