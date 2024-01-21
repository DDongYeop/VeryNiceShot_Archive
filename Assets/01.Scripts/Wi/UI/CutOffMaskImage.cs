using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutOffMaskImage : Image
{
	public override Material materialForRendering
	{
		get
		{
			Material material = new Material(base.materialForRendering);
			material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
			return material;
		}
	}

	public override void SetAllDirty()
	{
		base.SetAllDirty();
	}
}
