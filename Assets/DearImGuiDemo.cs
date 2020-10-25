using UnityEngine;
using ImGuiNET;

public class DearImGuiDemo : MonoBehaviour
{
    protected void OnEnable()
    {
        ImGuiUn.Layout += OnLayout;
    }

    protected void OnDisable()
    {
        ImGuiUn.Layout -= OnLayout;
    }

    protected void OnLayout()
    {
        ImGui.ShowDemoWindow();
        if (ToolManager.Instance != null)
        {
            ToolManager.Instance.UpdateTools(!ImGui.IsAnyItemHovered() && !ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow));
        }
    }
}