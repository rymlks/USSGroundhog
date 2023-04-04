using System;
using System.Collections.Generic;

namespace EasyState.Models.Resolvers
{
    public class EvaluatorResolver<T> : IResolver<T> where T : DataTypeBase
    {
        private readonly List<IEvaluatorConnection<T>> _evaluators;
        private readonly int _evaluatorCount;
        public EvaluatorResolver(List<IEvaluatorConnection<T>> connections)
        {
            _evaluators = connections ?? throw new ArgumentNullException(nameof(connections));
            _evaluatorCount = _evaluators.Count;
        }

        public State<T> Resolve(T data)
        {
            float highScore = float.MinValue;
            State<T> selectedState = null;
            for (int i = 0; i < _evaluatorCount; i++)
            {
                float score = _evaluators[i].Evaluator.BaseGetScore(data);
                if (score > highScore)
                {
                    highScore = score;
                    selectedState = _evaluators[i].DestState;
                }
            }
            if(selectedState is null)
            {
                throw new NullReferenceException("This should have never happened, an evaluator dest state must have been null.");
            }
            return selectedState;
        }
    }
}
