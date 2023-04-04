namespace EasyState.Models
{
    public interface ICustomDataTypeFormatter
    {
        /// <summary>
        /// Convert an instance of  <see cref="DataTypeBase"/> to a string to be displayed in
        /// the debuger window data popups. Unity 2021.1 and newer supports rich text <a href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html">see here</a> for list of 
        /// supported tags.
        /// </summary>
        /// <returns>text that will be displayed in data popup inside the debugger during debugging</returns>
        string GetDataAsString();
    }
}
