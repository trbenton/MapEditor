using UnityEngine;

public enum ToolMode
{
    None = 0,
    RaiseTerrain = 1,
    LowerTerrain = 2,
    PaintTerrain = 3
}

public class ToolManager : MonoBehaviour
{
    public GameObject RaiseObject;
    public GameObject LowerObject;
    public GameObject PaintObject;

    private ToolMode _mode = ToolMode.RaiseTerrain;

    protected void Awake()
    {

    }

    protected void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            _mode += 1;
        }

        RaiseObject.SetActive(false);
        LowerObject.SetActive(false);
        PaintObject.SetActive(false);

        switch (_mode)
        {
            case ToolMode.RaiseTerrain:
                HandleRaiseMode();
                break;
            case ToolMode.LowerTerrain:
                break;
            case ToolMode.PaintTerrain:
                break;
        }
    }

    private void HandleRaiseMode()
    {
        const int layerMask = (1 << (int)EditorLayers.MapTile);

        var ray = EditorCamera.MainCamera != null ? EditorCamera.MainCamera.ScreenPointToRay(Input.mousePosition) : new Ray();
        bool res = Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask);
        if (!res)
        {
            return;
        }

        var raycastedObject = hit.transform.gameObject;
        var tile = raycastedObject.GetComponent<Tile>();
        if (tile == null)
        {
            return;
        }

        var pos = tile.GetClosestVertex(hit.point);
        RaiseObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        RaiseObject.transform.position = new Vector3(RaiseObject.transform.position.x, RaiseObject.transform.position.y + 0.15f, RaiseObject.transform.position.z);
        RaiseObject.SetActive(true);
    }
}
