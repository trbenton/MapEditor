using System.Collections.Generic;
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
    public static ToolManager Instance { get; private set; }

    public GameObject RaiseObject;
    public GameObject LowerObject;
    public GameObject PaintObject;
    public Color PaintColor;

    private readonly List<ToolMode> _modes = new List<ToolMode>
    {
        ToolMode.RaiseTerrain,
        ToolMode.LowerTerrain,
        ToolMode.PaintTerrain
    };

    private ToolMode _mode = ToolMode.RaiseTerrain;

    protected void Awake()
    {
        Instance = this;
    }

    public void UpdateTools(bool isActive)
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            _mode = _modes.IndexOf(_mode) == _modes.Count - 1
                ? _modes[0]
                : _modes[_modes.IndexOf(_mode) + 1];
        }

        RaiseObject.SetActive(false);
        LowerObject.SetActive(false);
        PaintObject.SetActive(false);

        if (!isActive)
        {
            return;
        }

        switch (_mode)
        {
            case ToolMode.RaiseTerrain:
                HandleRaiseMode();
                break;
            case ToolMode.LowerTerrain:
                HandleLowerMode();
                break;
            case ToolMode.PaintTerrain:
                HandlePaintMode();
                break;
        }
    }

    private void HandleRaiseMode()
    {
        var tile = GetSelectedTile(out var hitPosition);
        if (tile == null)
        {
            return;
        }

        var pos = tile.GetClosestVertex(hitPosition);
        RaiseObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        var currentPos = RaiseObject.transform.position;
        RaiseObject.transform.position = new Vector3(currentPos.x, currentPos.y + 0.15f, currentPos.z);
        RaiseObject.SetActive(true);

        if (Input.GetMouseButtonUp(0))
        {
            tile.Raise(pos);
        }
    }

    private void HandleLowerMode()
    {
        var tile = GetSelectedTile(out var hitPosition);
        if (tile == null)
        {
            return;
        }

        var pos = tile.GetClosestVertex(hitPosition);
        LowerObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        var currentPos = LowerObject.transform.position;
        LowerObject.transform.position = new Vector3(currentPos.x, currentPos.y + 0.15f, currentPos.z);
        LowerObject.SetActive(true);

        if (Input.GetMouseButtonUp(0))
        {
            tile.Lower(pos);
        }
    }

    private void HandlePaintMode()
    {
        var tile = GetSelectedTile(out _);
        if (tile == null)
        {
            return;
        }

        var pos = tile.transform.position;
        PaintObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        var currentPos = PaintObject.transform.position;
        PaintObject.transform.position = new Vector3(currentPos.x, currentPos.y + 0.15f, currentPos.z);
        PaintObject.SetActive(true);

        if (Input.GetMouseButton(0))
        {
            tile.SetColor(PaintColor);
        }
    }

    private Tile GetSelectedTile(out Vector3 hitPosition)
    {
        const int layerMask = (1 << (int)EditorLayers.MapTile);

        hitPosition = Vector3.zero;
        var ray = EditorCamera.MainCamera != null ? EditorCamera.MainCamera.ScreenPointToRay(Input.mousePosition) : new Ray();
        bool res = Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask);
        if (!res)
        {
            return null;
        }

        hitPosition = hit.point;
        var raycastedObject = hit.transform.gameObject;
        return raycastedObject.GetComponent<Tile>();
    }
}
