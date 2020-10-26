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
        //handle main menu
        ImGui.Begin("Menu", ImGuiWindowFlags.MenuBar);
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
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
                if (ImGui.MenuItem("About"))
                {
                }
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }
        ImGui.End();

        if (ToolManager.Instance != null)
        {
            //TODO: check if any popups are open here as well...
            ToolManager.Instance.UpdateTools(!ImGui.IsAnyItemHovered() && 
                                             !ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow));
        }
    }
}