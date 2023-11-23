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
        // Explicitly create the delegate
        //var query = names.Where(new Func<string, bool>(NameLongerThanFour));

        // Compiler creates delegate
        //var query = names.Where(NameLongerThanFour);

        // Using lambda expression
        IOrderedEnumerable<string> query = names
            .Where(name => name.Length > 4)
            .OrderBy(name => name.Length)
            .ThenBy(name => name);
        foreach (string item in query)
        {
            WriteLine(item);
        }



    }

    static bool NameLongerThanFour(string name)
    {
        return name.Length > 4;
    }

    static void FilteringByType()
    {
        SectionTitle("Filtering by type");
        List<Exception> exceptions = new()
        {
            new ArgumentException(), new SystemException(),
            new IndexOutOfRangeException(), new InvalidOperationException(),
            new NullReferenceException(), new InvalidCastException(),
            new OverflowException(), new DivideByZeroException(),
            new ApplicationException()
        };

        IEnumerable<ArithmeticException> arithemeticExceptionQuery =
            exceptions.OfType<ArithmeticException>();
        foreach (ArithmeticException exception in arithemeticExceptionQuery)
        {
            WriteLine(exception);
        }
    }

    static void Output(IEnumerable<string> cohort, string description = "")
    {
        if (!string.IsNullOrEmpty(description))
        {
            WriteLine(description);
        }
        Write(" ");
        WriteLine(string.Join(", ", cohort.ToArray()));
        WriteLine();
    }

    static void WorkingWithSets()
    {
        string[] cohort1 =
            { "Rachel", "Gareth", "Jonathan", "George" };
        string[] cohort2 =
            { "Jack", "Stephen", "Daniel", "Jack", "Jared" };
        string[] cohort3 =
            { "Declan", "Jack", "Jack", "Jasmine", "Conor" };
        SectionTitle("The cohorts");
        Output(cohort1, "Cohort 1");
        Output(cohort2, "Cohort 2");
        Output(cohort3, "Cohort 3");
        SectionTitle("Set operations");
        Output(cohort2.Distinct(), "cohort2.Distinct()");
        Output(cohort2.DistinctBy(name => name.Substring(0, 2)),
            "Cohort2.DistinctBy(name => name.Substring(0, 2)):");
        Output(cohort2.Union(cohort3), "Output(cohort2.Union(cohort3))");
        Output(cohort2.Concat(cohort3), "Output(cohort2.Concat(cohort3)");
        Output(cohort2.Intersect(cohort3), "Output(cohort2.Intersect(cohort3)");
        Output(cohort2.Except(cohort3), "cohort2.Except(cohort3)");
        Output(cohort1.Zip(cohort2, (c1, c2) => $"{c1} matched with {c2}"),
          "cohort1.Zip(cohort2)");
    }


}