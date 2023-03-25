using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletsMagazine : MonoBehaviour
{
    // TODO add fast reload, auto reload, ammo remaining bullet options
    // TODO add remove X type bullet
    [SerializeField]
    private int _maxSizeMagazine;

    private int _ergonomics;
    private int _weight;
    private int _price;
    private int _countAvailableAttachments;

    [SerializeField]
    private List<GameObject> _bulletsInMagazine;

    private void Start()
    {
        _bulletsInMagazine = new();
    }

    // get list of bullets from inventory*
    public void AddBulletsToMagazine(List<GameObject> bulletsToAdd)
    {
        int availableSpace = _maxSizeMagazine - _bulletsInMagazine.Count;

        if (availableSpace == 0)
        {
            return;
        }
        else if (availableSpace < 0)
        {
            print("STRONG FAIL");
        }

        _bulletsInMagazine.AddRange(bulletsToAdd.GetRange(0, availableSpace));
        bulletsToAdd.RemoveRange(0, availableSpace);
    }

    public List<GameObject> RemoveAllBulletsFromMagazine()
    {
        // "return" bullets from magaz and "null" magaz
        var tempMag = new List<GameObject>(_bulletsInMagazine);
        _bulletsInMagazine = new List<GameObject>();
        return tempMag;
    }

    // remove X count bullets
    public List<GameObject> RemoveXBulletsFromMagazine(int countToRemove)
    {
        if (_bulletsInMagazine.Count == 0)
        {
            return new List<GameObject>();
        }

        countToRemove = Math.Min(countToRemove, _bulletsInMagazine.Count);

        var removedBullets = _bulletsInMagazine.GetRange(_bulletsInMagazine.Count - countToRemove, countToRemove);

        _bulletsInMagazine.RemoveRange(_bulletsInMagazine.Count - countToRemove, countToRemove);

        return removedBullets;
    }

    public List<GameObject> ReturnMagazine()
    {
        return _bulletsInMagazine;
    }



}