namespace Dialogue
{
    [System.Serializable]
    public class MonologueStatements
    {
        public MonologueStatements(string[] statements)
        {
            this.statements = statements;
        }

        public string[] statements;
    }
}