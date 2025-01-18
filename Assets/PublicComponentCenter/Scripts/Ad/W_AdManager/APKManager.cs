using System.Collections;
using System.Collections.Generic;
using PublicComponentCenter;
using UnityEngine;

public class APKManager : MonoBehaviour
{
    public void Privacy()
    {
        GameEntry.Ad.PrivacyPolicy();
    }

    public void Service()
    {
        GameEntry.Ad.ContactCustomerService();
    }

    public void More()
    {
        GameEntry.Ad.MoreWonderful();
    }

    public void CleanPackage()
    {
        GameEntry.Ad.CleanPackageData();
    }
}
