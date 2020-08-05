namespace calculator
{
    class RecordObj
    {
        public RecordObj(string inf, string pref, string post, string deci, string bin)
        {
            Infix = inf;
            Prefix = pref;
            Postfix = post;
            Deci = deci;
            Bin = bin;
        }
        public string Infix { get; set; }
        public string Prefix { get; set; }
        public string Postfix { get; set; }
        public string Deci { get; set; }
        public string Bin { get; set; }
    }
}
