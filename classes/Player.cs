namespace ToeTactTics_V2.classes
{
    public struct Player(string name)
    {
        public string username = name;
        public string symbol = "";
        public int wins = 0;
    }
}
