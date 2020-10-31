using ImGuiNET;
using UnityEngine;

public class DearImGuiManager : MonoBehaviour
{
    private bool _wasActiveLastFrame = true;

    protected void Awake()
    {
    }

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
        bool hasAnOpenMenu = false;

        //handle main menu
        ImGui.Begin("Menu", ImGuiWindowFlags.MenuBar);
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                hasAnOpenMenu = true;
                if (ImGui.MenuItem("Open", "Ctrl+O"))
                {
                }

                if (ImGui.MenuItem("Save", "Ctrl+S"))
                {
                }

                if (ImGui.MenuItem("Close"))
                {
                }

                if (ImGui.MenuItem("Quit", "Ctrl+Q"))
                {
                    Application.Quit();
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Settings"))
            {
                hasAnOpenMenu = true;
                if (ImGui.MenuItem("Enable Blending", string.Empty, false))
                {
                }

                if (ImGui.MenuItem("Enable Heights", string.Empty, false))
                {
                }

                if (ImGui.BeginMenu("Display Units"))
                {
                    if (ImGui.MenuItem("Centimeters", string.Empty, true))
                    {
                    }

                    if (ImGui.MenuItem("Meters", string.Empty, false))
                    {
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Help"))
            {
                hasAnOpenMenu = true;
                if (ImGui.MenuItem("About"))
                {
                }
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();

            var normalColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            var selectedColor = new Vector4(0.054f, 0.054f, 0.054f, 1.0f);

            ImGui.PushStyleColor(ImGuiCol.Text, ToolManager.Instance.GetToolMode() == ToolMode.None ? selectedColor : normalColor);
            if (ImGui.Button(char.ToString('\uf245'), new Vector2(32, 32)))
            {
                ToolManager.Instance.SetToolMode(ToolMode.None);
            }
            ImGui.PopStyleColor(1);

            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Text, ToolManager.Instance.GetToolMode() == ToolMode.RaiseTerrain ? selectedColor : normalColor);
            if(ImGui.Button(char.ToString('\uf062'), new Vector2(32, 32)))
            {
                ToolManager.Instance.SetToolMode(ToolMode.RaiseTerrain);
            }
            ImGui.PopStyleColor(1);

            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Text, ToolManager.Instance.GetToolMode() == ToolMode.LowerTerrain ? selectedColor : normalColor);
            if (ImGui.Button(char.ToString('\uf063'), new Vector2(32, 32)))
            {
                ToolManager.Instance.SetToolMode(ToolMode.LowerTerrain);
            }
            ImGui.PopStyleColor(1);

            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Text, ToolManager.Instance.GetToolMode() == ToolMode.PaintTerrain ? selectedColor : normalColor);
            if(ImGui.Button(char.ToString('\uf5aa'), new Vector2(32, 32)))
            {
                ToolManager.Instance.SetToolMode(ToolMode.PaintTerrain);
            }
            ImGui.PopStyleColor(1);
        }

        ImGui.End();

        if (ToolManager.Instance != null)
        {
            bool isActive = !ImGui.IsAnyItemHovered() &&
                            !ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow) &&
                            !hasAnOpenMenu;

            //reset inputs when tools become active again...
            if (isActive && !_wasActiveLastFrame)
            {
                Input.ResetInputAxes();
            }

            ToolManager.Instance.UpdateTools(isActive);
            _wasActiveLastFrame = isActive;
        }
    }
}