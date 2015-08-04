﻿// -------------------------------------------------------------------------------------------
// <copyright file="ToolView.cs" company="MapWindow OSS Team - www.mapwindow.org">
//  MapWindow OSS Team - 2015
// </copyright>
// -------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MW5.Plugins.Concrete;
using MW5.Plugins.Interfaces;
using MW5.Tools.Controls.Parameters;
using MW5.Tools.Helpers;
using MW5.Tools.Model.Parameters;
using MW5.Tools.Services;
using MW5.UI.Forms;

namespace MW5.Tools.Views
{
    /// <summary>
    /// The gis tool view.
    /// </summary>
    public partial class ToolView : GisToolViewBase, IToolView
    {
        private readonly IAppContext _context;
        private readonly ParameterControlFactory _controlFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolView"/> class.
        /// </summary>
        public ToolView(IAppContext context, ParameterControlFactory controlFactory)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");

            _context = context;
            _controlFactory = controlFactory;

            InitializeComponent();
        }

        /// <summary>
        /// Gets the ok button.
        /// </summary>
        public ButtonBase OkButton
        {
            get { return btnRun; }
        }

        public bool RunInBackground
        {
            get { return chkBackground.Checked; }
        }

        /// <summary>
        /// Generates controls for parameters.
        /// </summary>
        public void GenerateControls(IEnumerable<BaseParameter> parameters)
        {
            parameters = parameters.ToList();

            panelRequired.Generate(parameters.Where(p => p.Required), _controlFactory, false);

            if (parameters.All(p => p.Required))
            {
                tabOptional.TabVisible = false;
            }
            else
            {
                panelOptional.Generate(parameters.Where(p => !p.Required), _controlFactory, true);
            }
        }

        public void Initialize()
        {
            chkBackground.Checked = AppConfig.Instance.TaskRunInBackground;
            
            var tool = Model.Tool;
            tool.Progress = new EventProgress();

            Text = tool.Name;

            webBrowser1.DocumentText = tool.LoadManual();
        }

        public override void BeforeClose()
        {
            AppConfig.Instance.TaskRunInBackground = chkBackground.Checked;
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class GisToolViewBase : MapWindowView<ToolViewModel>
    {
    }
}