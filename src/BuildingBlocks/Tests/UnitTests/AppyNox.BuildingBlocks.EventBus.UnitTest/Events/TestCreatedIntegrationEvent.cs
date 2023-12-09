using AppyNox.EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.Events
{
    public class TestCreatedIntegrationEvent : IntegrationEvent
    {
        #region Public Constructors

        public TestCreatedIntegrationEvent(int _id) : base()
        {
            TestDataId = _id;
        }

        #endregion

        #region Properties

        public int TestDataId { get; set; }

        #endregion
    }
}