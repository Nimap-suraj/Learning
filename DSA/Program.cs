internal class Program
{
    private static void Main(string[] args)
    {
        int n = 5;
        int cnt = 1;
        List<int> arr = Enumerable.Repeat(0, n).ToList();
        for (int i = 1; i <= n; i++)
        {
          
            
                if (i % cnt == 0)
                {
                    for(int j = 1; j <= n; j++)
                        {
                        arr[j-1] = 1;
                        }    
                }
                cnt++;
            
        }
        foreach (var num in arr)
        {
            Console.Write(num + " ");
        }
    }
}