namespace EasyState.Models.Decorators
{
    public class ConditionalAction : IAction 
    {
        public  bool ExpectedResult;
        public bool ExecutedThisCycle { get; private set; }
        private readonly ICondition _conditional;
        private readonly IAction _decoratedAction;

        public ConditionalAction(ICondition conditional, IAction decoratedAction, bool expectedResult = true)
        {
            _conditional = conditional;
            _decoratedAction = decoratedAction;
            ExpectedResult = expectedResult;
        }

        public void BaseAct(DataTypeBase data)
        {
            ExecutedThisCycle = false;
            if(_conditional.BaseEvaluate(data) == ExpectedResult)
            {
                _decoratedAction.BaseAct(data);
                ExecutedThisCycle = true;
            }
        }

        public string GetName() => _decoratedAction.GetName();
    }
}
