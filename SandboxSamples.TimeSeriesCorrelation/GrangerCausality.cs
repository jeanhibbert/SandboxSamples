using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SandboxSamples.TimeSeriesCorrelation;

/* Granger causality test is a statistical test used to determine if one time series is useful in forecasting another time series. */

internal class GrangerCausality
{
    public static void Start()
    {
        double[] series1 = new double[] { 1, 2, 3, 4, 5 };
        double[] series2 = new double[] { 5, 4, 3, 2, 1 };

        // Perform Granger causality test
        bool isGrangerCausality = GrangerCausalityTest(series1, series2);

        // Print result
        Console.WriteLine("Is series1 Granger Causal to series2? " + isGrangerCausality);
    }

    // Granger causality test implementation
    static bool GrangerCausalityTest(double[] series1, double[] series2)
    {
        int n = series1.Length;
        int p = 2; // number of lags

        Matrix<double> X = DenseMatrix.OfArray(new double[n - p, 2 * p]);
        MathNet.Numerics.LinearAlgebra.Vector<double> y = DenseVector.OfArray(new double[n - p]);

        // Create input matrix X and output vector y
        for (int i = 0; i < n - p; i++)
        {
            for (int j = 0; j < p; j++)
            {
                X[i, j] = series1[i + j];
                X[i, j + p] = series2[i + j];
            }
            y[i] = series2[i + p];
        }

        // Perform regression
        MathNet.Numerics.LinearAlgebra.Vector<double> beta1 = X.Solve(y);
        MathNet.Numerics.LinearAlgebra.Vector<double> beta2 = DenseVector.OfArray(new double[p]);

        Matrix<double> X2 = DenseMatrix.OfArray(new double[n - p, p]);
        for (int i = 0; i < n - p; i++)
        {
            for (int j = 0; j < p; j++)
            {
                X2[i, j] = series2[i + j];
            }
        }

        // Perform regression again
        beta2 = X2.Solve(y);

        // Compare R-squared values
        double rSquared1 = 1 - (y - X * beta1).L2Norm() * (y - X * beta1).L2Norm() / (y - y.Average()).L2Norm() / (y - y.Average()).L2Norm();
        double rSquared2 = 1 - (y - X2 * beta2).L2Norm() * (y - X2 * beta2).L2Norm() / (y - y.Average()).L2Norm() / (y - y.Average()).L2Norm();

        return rSquared1 > rSquared2;
    }
}

