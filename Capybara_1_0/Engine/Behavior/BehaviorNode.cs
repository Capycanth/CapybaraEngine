using System;
using System.Collections.Generic;

namespace Capybara_1.Engine.Behavior
{
    public enum ConditionReturnEnum
    {
        False = 0,
        True = 1,
    }

    public interface IBehaviorNode
    {
        Action Execute();
    }

    public class ConditionNode : IBehaviorNode
    {
        private Func<ConditionReturnEnum> _condition;
        private Dictionary<ConditionReturnEnum, IBehaviorNode> children;

        public ConditionNode(Func<ConditionReturnEnum> condition, Dictionary<ConditionReturnEnum, IBehaviorNode> children)
        {
            _condition = condition;
            this.children = children;
        }

        public Action Execute()
        {
            return children[_condition()].Execute();
        }
    }

    public class ActionNode : IBehaviorNode
    {
        private Action action;

        public ActionNode(Action action)
        {
            this.action = action;
        }

        public Action Execute()
        {
            return action;
        }
    }
}
