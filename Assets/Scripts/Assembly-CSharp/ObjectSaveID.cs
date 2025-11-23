using System.Xml.Linq;
using UnityEngine;

public class ObjectSaveID : MonoBehaviour
{
	[Tooltip("id of object for save manager")]
	public int objectID;

	[Tooltip("Type of object (item,door etc)")]
	public ObjectType objectType = ObjectType.door;

	private SaveGameManager saveManager;

	private void Awake()
	{
		saveManager = Object.FindObjectOfType<SaveGameManager>();
		saveManager.objectsToSave.Add(this);
	}

	private void OnDestroy()
	{
		saveManager.objectsToSave.Remove(this);
	}

	public XElement GetElement()
	{
		if (objectType == ObjectType.item)
		{
			XAttribute xAttribute = new XAttribute("id", objectID);
			XAttribute xAttribute2 = new XAttribute("x", base.transform.position.x);
			XAttribute xAttribute3 = new XAttribute("y", base.transform.position.y);
			XAttribute xAttribute4 = new XAttribute("z", base.transform.position.z);
			XAttribute xAttribute5 = new XAttribute("rx", base.transform.eulerAngles.x);
			XAttribute xAttribute6 = new XAttribute("ry", base.transform.eulerAngles.y);
			XAttribute xAttribute7 = new XAttribute("rz", base.transform.eulerAngles.z);
			return new XElement("instance", xAttribute, xAttribute2, xAttribute3, xAttribute4, xAttribute5, xAttribute6, xAttribute7);
		}
		if (objectType == ObjectType.door)
		{
			XAttribute xAttribute8 = new XAttribute("id", objectID);
			XAttribute xAttribute9 = new XAttribute("locked", GetComponent<Door>().locked);
			XAttribute xAttribute10 = new XAttribute("state", GetComponent<Door>().state);
			XAttribute xAttribute11 = new XAttribute("locks", GetComponent<Door>().locksCount);
			return new XElement("instance", xAttribute8, xAttribute9, xAttribute10, xAttribute11);
		}
		if (objectType == ObjectType.switchable)
		{
			XAttribute content = new XAttribute("id", objectID);
			return new XElement("instance", content);
		}
		if (objectType == ObjectType.puzzle)
		{
			XAttribute xAttribute12 = new XAttribute("id", objectID);
			XAttribute xAttribute13 = new XAttribute("puzzleID", GetComponent<PuzzleBlock>().ID);
			XAttribute xAttribute14 = new XAttribute("x", base.transform.position.x);
			XAttribute xAttribute15 = new XAttribute("y", base.transform.position.y);
			XAttribute xAttribute16 = new XAttribute("z", base.transform.position.z);
			return new XElement("instance", xAttribute12, xAttribute14, xAttribute15, xAttribute16, xAttribute13);
		}
		if (objectType == ObjectType.locks)
		{
			XAttribute xAttribute17 = new XAttribute("id", objectID);
			XAttribute xAttribute18 = new XAttribute("lockState", GetComponent<Lock>().isOpen);
			XAttribute xAttribute19 = new XAttribute("x", base.transform.position.x);
			XAttribute xAttribute20 = new XAttribute("y", base.transform.position.y);
			XAttribute xAttribute21 = new XAttribute("z", base.transform.position.z);
			XAttribute xAttribute22 = new XAttribute("rx", base.transform.eulerAngles.x);
			XAttribute xAttribute23 = new XAttribute("ry", base.transform.eulerAngles.y);
			XAttribute xAttribute24 = new XAttribute("rz", base.transform.eulerAngles.z);
			return new XElement("instance", xAttribute17, xAttribute19, xAttribute20, xAttribute21, xAttribute22, xAttribute23, xAttribute24, xAttribute18);
		}
		if (objectType == ObjectType.triggerEvent)
		{
			XAttribute xAttribute25 = new XAttribute("id", objectID);
			XAttribute xAttribute26 = new XAttribute("state", GetComponent<TriggerEvents>().activated);
			return new XElement("instance", xAttribute25, xAttribute26);
		}
		return null;
	}
}
