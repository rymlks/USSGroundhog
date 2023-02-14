using EasyState.Models;
using EasyState.Models.Decorators;
using System;

namespace EasyState.Builders
{
    public class ActionBuilder: Builder<IAction>
    {
        private IAction _action;

        public ActionBuilder(IAction action)
        {
            _action = action;
        }

        public ConditionalActionBuilder WithConditional(ICondition condition)
        {
            var conditionalAction = new ConditionalAction(condition, _action);
            var newBuilder = new ConditionalActionBuilder(conditionalAction);
            return newBuilder;
        }
        public ActionBuilder WithConditional(ICondition condition, bool expectedResult)
        {
            _action = new ConditionalAction(condition, _action, expectedResult);
            return this;
        }
        public override IAction Build()
        {
            if (_action == null)
            {
                throw new InvalidOperationException("Action is null and can not be built");
            }

            return _action;

        }
    }

    public class ConditionalActionBuilder : ActionBuilder
    {
        private readonly ConditionalAction _conditionalAction;
        public ConditionalActionBuilder(ConditionalAction action) : base(action)
        {
            _conditionalAction = action;
        }
        public ConditionalActionBuilder WithExpectedResult(bool result)
        {
            _conditionalAction.ExpectedResult = result;
            return this;
        }

    }
}
