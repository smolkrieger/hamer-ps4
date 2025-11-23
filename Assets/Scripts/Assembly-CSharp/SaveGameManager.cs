using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
	private GameControll gameControll;

	private string path;

	public List<ObjectSaveID> objectsToSave = new List<ObjectSaveID>();

	private void Awake()
	{
		path = Application.persistentDataPath + "/save.xml";
		gameControll = GetComponent<GameControll>();
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("LoadGame") && PlayerPrefs.GetInt("LoadGame") == 1 && File.Exists(path))
		{
			Load();
			Debug.Log("GameLoaded!");
		}
	}

	public void Save()
	{
		XElement xElement = new XElement("gen");
		foreach (ObjectSaveID item in objectsToSave)
		{
			xElement.Add(item.GetElement());
		}
		xElement.Add(new XElement("lifes", gameControll.lifeCount));
		XDocument xDocument = new XDocument(xElement);
		File.WriteAllText(path, xDocument.ToString());
		Debug.Log(path);
		gameControll.MainMenuExit(1);
	}

	public void Load()
	{
		XElement xElement = null;
		if (File.Exists(path))
		{
			xElement = XDocument.Parse(File.ReadAllText(path)).Element("gen");
			LoadSceneData(xElement);
		}
		else
		{
			Debug.Log("Save file not found!");
		}
	}

	private void LoadSceneData(XElement root)
	{
		int num = int.Parse(root.Element("lifes").Value);
		Debug.Log(num);
		gameControll.lifeCount = num;
		for (int i = 0; i < objectsToSave.Count; i++)
		{
			XElement xElement = CheckObjectID(i, root);
			if (xElement != null)
			{
				if (objectsToSave[i].objectType == ObjectType.item)
				{
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					zero.x = float.Parse(xElement.Attribute("x").Value);
					zero.y = float.Parse(xElement.Attribute("y").Value);
					zero.z = float.Parse(xElement.Attribute("z").Value);
					zero2.x = float.Parse(xElement.Attribute("rx").Value);
					zero2.y = float.Parse(xElement.Attribute("ry").Value);
					zero2.z = float.Parse(xElement.Attribute("rz").Value);
					objectsToSave[i].transform.position = zero;
					objectsToSave[i].transform.eulerAngles = zero2;
				}
				if (objectsToSave[i].objectType == ObjectType.door)
				{
					bool locked = bool.Parse(xElement.Attribute("locked").Value);
					int state = int.Parse(xElement.Attribute("state").Value);
					int locksCount = int.Parse(xElement.Attribute("locks").Value);
					objectsToSave[i].GetComponent<Door>().locked = locked;
					objectsToSave[i].GetComponent<Door>().state = state;
					objectsToSave[i].GetComponent<Door>().locksCount = locksCount;
					objectsToSave[i].GetComponent<Door>().LoadState();
				}
				if (objectsToSave[i].objectType == ObjectType.switchable)
				{
				}
				if (objectsToSave[i].objectType == ObjectType.puzzle)
				{
					Vector3 zero3 = Vector3.zero;
					zero3.x = float.Parse(xElement.Attribute("x").Value);
					zero3.y = float.Parse(xElement.Attribute("y").Value);
					zero3.z = float.Parse(xElement.Attribute("z").Value);
					int iD = int.Parse(xElement.Attribute("puzzleID").Value);
					objectsToSave[i].transform.position = zero3;
					objectsToSave[i].GetComponent<PuzzleBlock>().ID = iD;
				}
				if (objectsToSave[i].objectType == ObjectType.locks)
				{
					bool isOpen = bool.Parse(xElement.Attribute("lockState").Value);
					Vector3 zero4 = Vector3.zero;
					Vector3 zero5 = Vector3.zero;
					zero4.x = float.Parse(xElement.Attribute("x").Value);
					zero4.y = float.Parse(xElement.Attribute("y").Value);
					zero4.z = float.Parse(xElement.Attribute("z").Value);
					zero5.x = float.Parse(xElement.Attribute("rx").Value);
					zero5.y = float.Parse(xElement.Attribute("ry").Value);
					zero5.z = float.Parse(xElement.Attribute("rz").Value);
					objectsToSave[i].transform.position = zero4;
					objectsToSave[i].transform.eulerAngles = zero5;
					objectsToSave[i].GetComponent<Lock>().isOpen = isOpen;
					objectsToSave[i].GetComponent<Lock>().LoadState();
				}
				if (objectsToSave[i].objectType == ObjectType.triggerEvent)
				{
					bool activated = bool.Parse(xElement.Attribute("state").Value);
					objectsToSave[i].GetComponent<TriggerEvents>().activated = activated;
					objectsToSave[i].GetComponent<TriggerEvents>().LoadState();
				}
			}
			else
			{
				Object.Destroy(objectsToSave[i].gameObject);
			}
		}
	}

	private XElement CheckObjectID(int i, XElement root)
	{
		foreach (XElement item in root.Elements("instance"))
		{
			int num = int.Parse(item.Attribute("id").Value);
			if (num == objectsToSave[i].objectID)
			{
				return item;
			}
		}
		return null;
	}
}
