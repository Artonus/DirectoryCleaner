namespace DirectoryCleaner
{
    public class Parser
    {
        public string GetArgumentValue(string[] args, string arg)
        {
            for (int i = 0; i < args.Length; i+=2)
            {
                if (args[i].Trim() == arg)
                    return args[i + 1].Trim();
            }
            return string.Empty;
        }
        public bool ArgumentExists(string[] args, string arg)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i].Trim() == arg)
                    return true;
            }
            return false;
        }
    }
}