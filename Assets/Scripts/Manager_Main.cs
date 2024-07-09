using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

using Random = UnityEngine.Random;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Manager_Main : MonoBehaviour
{
    Member member;
    [SerializeField] GameObject memberPrefab;
    [SerializeField] GameObject memberParent;

    public List<GameObject> memberPrefabList = new List<GameObject>();
    public List<Member> memberList = new List<Member>();
    public List<string> memberSelected = new List<string>();
    public List<bool> memberIsTaken = new List<bool>();

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI nameList;
    [SerializeField] Button delegateSecretSanta;


    //--------------------


    private void Start()
    {
        member = FindObjectOfType<Member>();
        nameList.text = "";
    }
    private void Update()
    {
        //Display list f selected players
        nameList.text = "";

        for (int i = 0; i < memberSelected.Count; i++)
        {
            nameList.text += memberSelected[i] + "\n";
        }
    }


    //--------------------


    public void DelegateButton_IsPressed()
    {
        //Instantiate memberPrefab
        for (int i = 0; i < memberSelected.Count; i++)
        {
            //Instantiate Members
            memberPrefabList.Add(Instantiate(memberPrefab, Vector3.zero, Quaternion.identity) as GameObject);
            memberPrefabList[i].transform.parent = memberParent.transform;
        }

        //Update memberList to be the same size as memberPrefabList
        for (int i = 0; i < memberPrefabList.Count; i++)
        {
            memberList.Add(memberPrefabList[i].GetComponent<Member>());
            memberIsTaken.Add(false);
        }

        //Include names to all elements in memberList
        for (int i = 0; i < memberList.Count; i++)
        {
            memberList[i].name = memberSelected[i];
        }

        //Delegate names to memberList.playerToGift that is not the same name as memberList.name

        for (int i = 0; i < memberSelected.Count - 1;)
        {
            int randomNum = (int)Random.Range(0, memberPrefabList.Count);

            if (memberList[i].name != memberSelected[randomNum])
            {
                if (!memberIsTaken[randomNum])
                {
                    memberList[i].personToGift = memberSelected[randomNum];
                    memberIsTaken[randomNum] = true;
                    i++;
                }
            }
        }

        for (int i = 0; i < memberSelected.Count; i++)
        {
            if (memberIsTaken[i] == false)
            {
                memberList[memberList.Count - 1].personToGift = memberSelected[i];
                memberIsTaken[i] = true;
            }
        }

        //Make Documents named after memberList.name
        #region
        //Create the folder
        Directory.CreateDirectory(Application.streamingAssetsPath + "/SecretSantaDelegates");

        //Don't run if the log is empty
        if (memberList.Count <= 0) { return; }

        //Create the file
        for (int i = 0; i < memberList.Count; i++)
        {
            string txtDocumentName = Application.streamingAssetsPath + "/SecretSanta_" + memberList[i].name + ".txt";

            //Check if the text exixst. If it doesen't, create a new one
            if (!File.Exists(txtDocumentName))
            {
                //Add a heading inside file
                File.WriteAllText(txtDocumentName, memberList[i].name + ", you are the \"Secret Santa\" for " + memberList[i].personToGift);
            }
            File.WriteAllText(txtDocumentName, memberList[i].name + ", you are the \"Secret Santa\" for " + memberList[i].personToGift);
        }
        #endregion

        //Print to double check that there are none players with the same name and giftingName
        for (int i = 0; i < memberList.Count; i++)
        {
            if (memberList[i].name == memberList[i].personToGift)
            {
                print("Error: " + i + " | " + memberList[i].name + " - " + memberList[i].personToGift);
            }
        }

        DeleteLists();
    }

    void DeleteLists()
    {
        //Delete the list of players
        for (int i = 0; i < memberPrefabList.Count; i++)
        {
            Destroy(memberPrefabList[i]);
        }

        memberSelected.Clear();
        memberList.Clear();
        memberIsTaken.Clear();
        memberPrefabList.Clear();
    }

    public void AutoButton_IsPressed()
    {
        memberSelected.Add("Adrian");
        memberSelected.Add("Sarah");
        memberSelected.Add("Ronny");
        memberSelected.Add("Pernille");
        memberSelected.Add("Sigurd");
        memberSelected.Add("Ingvild");
        memberSelected.Add("Gunnar");
        memberSelected.Add("Elisabeth");
        memberSelected.Add("Simon");
        memberSelected.Add("Solveig");
    }

    public void InputField_OnEndEdit()
    {
        //When enter pressed//
        AddOrRemovePlayer();

        inputField.text = "";
    }
    public void InputField_OnDeselect()
    {
        //When clicked outside the InputField
        inputField.text = "";
    }

    void AddOrRemovePlayer()
    {
        memberSelected.Add(inputField.text);
    }

    //--------------------


    public void ExitButton_isPressed()
    {
        Application.Quit();
    }
}

