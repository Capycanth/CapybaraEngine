using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara_1.Engine.Behavior
{
    public class BehaviorTree
    {
        private IBehaviorNode root;

        public BehaviorTree(IBehaviorNode root)
        {
            this.root = root;
        }

        public void Execute()
        {
            root.Execute()();
        }
    }
}
