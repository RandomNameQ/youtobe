using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMagazineControl : MonoBehaviour
{
    private BulletsMagazine _bulletsMagazine;

    [SerializeField]
    private List<GameObject> allBullets = new();

    private void Awake()
    {
        _bulletsMagazine = FindObjectOfType<BulletsMagazine>();
    }

    private void Start()
    {
        FindBullets();
        FullFillMagazine();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            FullFillMagazine();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            RemoveAllBulletsFromMagazine();
        }
    }

    private void FindBullets()
    {
        var bullets = GameObject.Find("BulletPool").transform.Find("Bullets");

        for (int i = 0; i < bullets.childCount; i++)
        {
            allBullets.Add(bullets.GetChild(i).gameObject);
        }
    }
    private void FullFillMagazine()
    {
     
       _bulletsMagazine.AddBulletsToMagazine(allBullets);
    }

    private void RemoveAllBulletsFromMagazine()
    {
        _bulletsMagazine.RemoveXBulletsFromMagazine(10);
    }
    

    

   
}
