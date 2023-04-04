namespace EasyState.Runtime.Utility
{
    public static  class EasyStateExtensions
    {
        public static bool RequiresCustomRate(this StateMachineUpdateRate updateRate)
        {
            return  updateRate == StateMachineUpdateRate.CustomRefreshRate ||
                    updateRate == StateMachineUpdateRate.BackgroundWithEventSync ||
                    updateRate == StateMachineUpdateRate.BackgroundThread;

        }
    }
}
