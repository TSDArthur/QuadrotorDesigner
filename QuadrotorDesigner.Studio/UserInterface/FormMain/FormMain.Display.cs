﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Win32;
using QuadrotorDesigner.Utils.IOStream;
using QuadrotorDesigner.Workspace.Properties;

namespace QuadrotorDesigner.Workspace.UserInterface
{
    public partial class FormMain : DarkForm
    {
        // list for dock windows
        private List<DarkDockContent> dockToolsList = new List<DarkDockContent>();

        // dock windows objects
        private DockTools.DockComponents dockToolComponents;
        private DockTools.DockProperties dockToolProperties;
        private DockTools.DockOutput dockToolOutput;

        // tree view manager
        private ComponentTreeView.TreeViewManager treeViewManager;

        private void DisplayInitializeDockPanel()
        {
            // Add the dock panel message filter to filter through for dock panel splitter
            Application.AddMessageFilter(dockPanelMain.DockResizeFilter);

            // Add the control scroll message filter to re-route all mousewheel events
            Application.AddMessageFilter(new ControlScrollFilter());

            // Add the dock content drag message filter to handle moving dock content around.
            Application.AddMessageFilter(dockPanelMain.DockContentDragFilter);

            dockToolComponents = new DockTools.DockComponents();
            dockToolProperties = new DockTools.DockProperties();
            dockToolOutput = new DockTools.DockOutput();

            dockToolsList.Add(dockToolComponents);
            dockToolsList.Add(dockToolProperties);
            dockToolsList.Add(dockToolOutput);

            foreach (var dockWindow in dockToolsList)
            {
                dockPanelMain.AddContent(dockWindow);
            }

            DisplaySetupMenu();
        }

        private void DisplaySetupBinding()
        {
            // component selector
            treeViewManager = new ComponentTreeView.TreeViewManager(dockToolComponents.treeComponents);
            treeViewManager.Refresh(DocumentManager.LocalModelDocuments);
            treeViewManager.ItemDoubleClicked += ActionSelectorDoubleClicked;
            treeViewManager.ItemRightClicked += ActionSelectorRightClicked;
            treeViewManager.ItemNodeSelected += ActionSelectorNodeChanged;
        }

        private void DisplayToggleDockWindow(DarkToolWindow dockWindow)
        {
            if (dockWindow.DockPanel == null)
            {
                dockPanelMain.AddContent(dockWindow);
            }
            else
            {
                dockPanelMain.RemoveContent(dockWindow);
            }
        }

        private void DisplaySetupMenu()
        {
            // check states
            menuItemComponentsExplorer.CheckState = dockPanelMain.ContainsContent(dockToolComponents) ? CheckState.Checked : CheckState.Unchecked;
            menuItemProperties.CheckState = dockPanelMain.ContainsContent(dockToolProperties) ? CheckState.Checked : CheckState.Unchecked;
            menuItemOutput.CheckState = dockPanelMain.ContainsContent(dockToolOutput) ? CheckState.Checked : CheckState.Unchecked;

            buttonComponentsExplorer.CheckState = menuItemComponentsExplorer.CheckState;
            buttonProperties.CheckState = menuItemProperties.CheckState;
            buttonOutput.CheckState = menuItemOutput.CheckState;
        }
    }
}
