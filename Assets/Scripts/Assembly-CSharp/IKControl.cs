using UnityEngine;

public class IKControl : MonoBehaviour
{
	private Animator animator;

	public bool ikActive = false;

	public Transform lookObj = null;

	private void Start()
	{
		animator = GetComponent<Animator>();
		lookObj = GetComponent<Enemy>().player.transform;
	}

	private void OnAnimatorIK()
	{
		if (ikActive && lookObj != null)
		{
			animator.SetLookAtWeight(1f, 1f, 1f, 1f, 1f);
			animator.SetLookAtPosition(lookObj.position);
		}
	}
}
