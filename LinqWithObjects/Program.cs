// a strubg array is a sequence that implements IEnumerable<string>
string[] names = new[] { "Michael", "Pam", "Jim", "Dwight",
  "Angela", "Kevin", "Toby", "Creed" };
SectionTitle("Deferred execution");
//Question: Which names end with an M
// (Written using a LINQ extension method)
var query1 = names.Where(name => name.EndsWith("m"));
//Question: Which names end with an M
// (Written using LINQ query comprehension syntax)
var query2 = from name in names where name.EndsWith("m") select name;

// answer returned as an array of strings
string[] result1 = query1.ToArray();
// answer returned as a list of strings
List<string> result2 = query2.ToList();
// answer returned by enumerating results
foreach (string name in query1)
{
    WriteLine(name);
    names[2] = "Jimmy";
    // On the second iteration Jimmy does not end with an m
}