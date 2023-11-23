partial class Program
{
    private static void DeferredExecution(string[] names)
    {
        SectionTitle("Deferred Execution");
        // Question: Which names end with an M
        // (Using LINQ extension method)
        var query1 = names.Where(name => name.EndsWith("m"));
        // Question: Which names end with an M
        // (Using LINQ query comprehension syntax
        var query2 = from name in names where name.EndsWith("m") select name;

        // Answer returned as an array of strings containing Pam and Jim.
        string[] result1 = query1.ToArray();
        // Answer returned as a list of strings containing Pam and Jim.
        List<string> result2 = query2.ToList();

        foreach (string name in query1)
        {
            WriteLine(name);
            //james[2] = "Jimmy";
        }
    }

    private static void FilteringUsingWhere(string[] names)
    {
        SectionTitle("Filtering entities using where");
        var query = names.Where(new Func<string, bool>(NameLongerThanFour));
        foreach (string item in  query)
        {
            WriteLine(item);
        }
    }

    static bool NameLongerThanFour(string name)
    {
        return name.Length > 4;
    }
}