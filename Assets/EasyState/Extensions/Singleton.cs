namespace EasyState.Extensions
{
    public class Singleton<T> where T : new()
    {
        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        lock (m_Lock)
                        {
                            m_Instance = new T();
                        }
                    }

                    return m_Instance;
                }
            }
        }

        private static readonly object m_Lock = new object();
        private static T m_Instance;
    }
}