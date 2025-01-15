using CardsService.Model;
using CardsService.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsService.Tests.Tools
{
    public class FakePolicy1 : IActionPolicy
    {
        public string ActionName => "FAKE_ACTION1";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }

    public class FakePolicy2 : IActionPolicy
    {
        public string ActionName => "FAKE_ACTION2";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }

    public class FakePolicy3 : IActionPolicy
    {
        public string ActionName => "FAKE_ACTION3";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }
}
