// See https://aka.ms/new-console-template for more information

long n = 35231;

string s = n.ToString();
char[] c = s.ToCharArray();
Array.Reverse(c);
long[] result = new long[c.Length];
for (int i = 0; i < c.Length; i++)
{
    result[i] = long.Parse(c[i].ToString());
}


Console.ReadKey();