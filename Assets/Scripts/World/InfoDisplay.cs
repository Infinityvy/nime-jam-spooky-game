using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    public static InfoDisplay instance;

    [SerializeField]
    private TMP_Text text;

    private void Awake()
    {
        instance = this;
    }

    public void updateText()
    {
        Session session = Session.instance;
        StorageZone storageZone = StorageZone.instance;

        text.text = "Money: " + session.money.ToString() + "\nStored/Quota\n" + storageZone.getStoredValue() + "/" + session.quota.ToString() + "\nDue in: " + session.dueIn.ToString() + " Layers";
    }
}
