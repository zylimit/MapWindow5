﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MW5.Plugins.Mvp;
using MW5.Plugins.Services;

namespace MW5.Plugins
{
    public static class CompositionRoot
    {
        public static void Compose(IApplicationContainer container)
        {
            container.RegisterSingleton<IBroadcasterService, PluginBroadcaster>()
                .RegisterSingleton<IPluginManager, PluginManager>();
        }
    }
}
