public class TestOptions
{
    public string opt_key1 { set; get; }

    public SubTestOptions opt_key2 { set; get; }
}

public class SubTestOptions
{
    public string k1 { set; get; }
    public string k2 { set; get; }
}