using System;

namespace EasyState.Data
{
    public static class EasyStateFileEvents
    {
        public static event Action<string> FileChanged;


        public static void OnFileChanged(string fileName)=> FileChanged?.Invoke(fileName);
    }
}
