using System.Linq;
using UnityEngine;
using System.Collections;

public class CheckVisibilityTool : MonoBehaviour {

	public float checkCD;

	private Renderer[] renderers;
	private bool visibility;

	public bool IsVisible
	{
		get
		{
			return visibility;
		}
	}

	// Use this for initialization
	void OnEnable() {
		visibility = true;
		renderers = GetComponentsInChildren<Renderer>();
		StartCoroutine(StartCheck());
	}

	void OnDisable() {
		StopAllCoroutines();
		visibility = false;
	}

	IEnumerator StartCheck() {
		while (true) {
			yield return new WaitForSeconds(checkCD);
			CheckIfVisible();
		}
	}


	void CheckIfVisible() {
		if (visibility) {
			for (int i = 0; i < renderers.Count(); i++) {
				if (renderers[i] && renderers[i].isVisible) {
					break;
				}
				visibility = false;
			}
		} else if (!visibility) {
			for (int i = 0; i < renderers.Count(); i++) {
				if (renderers[i] && renderers[i].isVisible) {
					visibility = true;
					break;
				}
			}
		}
	}

	/// <summary>
	/// 通过aabb检测是否可见的方式
	/// 5.4下使用Cube进行测试 效率过低
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="renderer"></param>
	/// <returns></returns>
	bool CheckIfVisibleByTestAABB(Camera camera, Renderer renderer) {
		var planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}

}
