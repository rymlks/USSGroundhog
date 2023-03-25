using EasyState.Models;

namespace EasyState.Core
{
    internal class NullEventController : IEventController
    {
        public static NullEventController Default = new NullEventController();

        public void OnInitialize(EasyStateMachine stateMachine, DataTypeBase data){}

        public void OnPostUpdate(DataTypeBase data){}

        public void OnPreUpdate(DataTypeBase data){}

        public void OnStopped(EasyStateMachine stateMachine){}
    }
}
