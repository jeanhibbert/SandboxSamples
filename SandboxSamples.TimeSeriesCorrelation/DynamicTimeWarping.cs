namespace SandboxSamples.TimeSeriesCorrelation;
/* This application defines two time series as arrays of double values, 
 * then calculates the DTW distance using the DTW method. The DTW distance is calculated using dynamic programming, 
 * which fills a two-dimensional dp array with the minimum cumulative cost of transforming one time series into another. 
 * The final result is stored in dp[m, n], where m and n are the lengths of the two time series. */

internal class DynamicTimeWarping
{
    public static void Start()
    {
        double[] series1 = new double[] { 1, 2, 3, 4, 5 };
        double[] series2 = new double[] { 5, 4, 3, 2, 1 };

        // Calculate DTW distance
        double dtwDistance = DTW(series1, series2);

        // Print result
        Console.WriteLine("DTW distance between two time series: " + dtwDistance);
    }

    static double DTW(double[] series1, double[] series2)
    {
        int m = series1.Length;
        int n = series2.Length;
        double[,] dp = new double[m + 1, n + 1];

        // Initialize dp array
        for (int i = 0; i <= m; i++)
        {
            dp[i, 0] = double.MaxValue;
        }
        for (int j = 0; j <= n; j++)
        {
            dp[0, j] = double.MaxValue;
        }
        dp[0, 0] = 0;

        // Fill dp array
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                double cost = Math.Abs(series1[i - 1] - series2[j - 1]);
                dp[i, j] = cost + Math.Min(Math.Min(dp[i - 1, j], dp[i, j - 1]), dp[i - 1, j - 1]);
            }
        }

        return dp[m, n];
    }
}
