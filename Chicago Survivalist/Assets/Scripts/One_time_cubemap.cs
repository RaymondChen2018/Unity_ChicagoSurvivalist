using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class One_time_cubemap : MonoBehaviour {
    int cubemapSize = 128;
    Camera cam;
    RenderTexture renderTexture;

	public void Render() {
        UpdateCubemap(63);
    }
    void UpdateCubemap(int faceMask)
    {
        if (!cam)
        {
            GameObject obj = new GameObject("CubemapCamera", typeof(Camera));
            obj.hideFlags = HideFlags.HideAndDontSave;
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            cam = obj.GetComponent<Camera>();
            cam.farClipPlane = 100; // don't render very far into cubemap
            cam.enabled = false;
        }

        if (!renderTexture)
        {
            renderTexture = new RenderTexture(cubemapSize, cubemapSize, 16);
            renderTexture.dimension = UnityEngine.Rendering.TextureDimension.Cube;
            renderTexture.hideFlags = HideFlags.HideAndDontSave;
            GetComponent<Renderer>().material.SetTexture("_Cube", renderTexture);
        }
        Vector3 renderPos = transform.position;
        renderPos.y = 10;
        cam.transform.position = transform.position;
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("UI")); 
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Trigger"));
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerTrigger"));
        cam.RenderToCubemap(renderTexture, faceMask);
        Destroy(this);
    }
}
