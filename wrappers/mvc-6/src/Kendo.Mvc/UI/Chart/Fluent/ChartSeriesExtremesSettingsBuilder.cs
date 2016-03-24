using System;
using System.Collections.Generic;

namespace Kendo.Mvc.UI.Fluent
{
    /// <summary>
    /// Defines the fluent API for configuring ChartSeriesExtremesSettings
    /// </summary>
    public partial class ChartSeriesExtremesSettingsBuilder
        
    {
        public ChartSeriesExtremesSettingsBuilder(ChartSeriesExtremesSettings container)
        {
            Container = container;
        }

        protected ChartSeriesExtremesSettings Container
        {
            get;
            private set;
        }

        // Place custom settings here
    }
}