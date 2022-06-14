using FastGraphs;
using System.Diagnostics;


Stopwatch stopWatch = new Stopwatch();

double Rec(double[,]? mat, int[] degreeses)
{
    Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms Entering a recursion");
    int length = mat.GetLength(0);
    Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms Graph Simplification");
    FastGraph.Reduction(ref mat, ref degreeses, length);
    Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms Completion of simplification");
    //factorization
    if (mat.GetLength(0) < 5)
    {
        Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms Formula calculation");
        if (mat.GetLength(0) == 3)
        {
            double a= mat[0,1];
            double b= mat[0,2];
            double c= mat[1,2];
            return a * b * c + (1 - a) * b * c + a * (1 - b) * c + a * b * (1 - c);
        }
        if (mat.GetLength(0) == 4)
        {
            double a= mat[0, 3];
            double b= mat[0, 1];
            double c= mat[1, 2];
            double d= mat[2, 3];
            double e= mat[0, 2];
            double f= mat[1, 3];
            return f * ((a + b - a * b) * (c + d - c * d) * e + (1 - e) * (a + b - a * b) * (c + d - c * d) + e * ((a + b - a * b) * (1 - c - d + c * d) + (1 - a - b + a * b) * (c + d - c * d))) + (1 - f) * (e * (a + d - a * d) * (b + c - b * c) + (1 - e) * (a * b * c * d + a * b * c * (1 - d) + a * b * (1 - c) * d + a * (1 - b) * c * d + (1 - a) * b * c * d));

        }
        else { return 0; }
    }
    else
    {
        Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms Consideration of a graph with a contracted edge and a removed");
        int i = length - 1;
        int j = 0;
        double[,]? F1 = null;
        double[,]? F2 = null;
        int[]? d1 = null;
        int[]? d2 = null;
        while ((i > -1) && (F1 == null) && (F2 == null))
        {
            if (mat [i, j] > 0)
            {
                F1 = new double[mat.GetLength(0)-1, mat.GetLength(0)-1];
                F1 = FastGraph.EdgePull(mat, i, j);
                F2 = mat;
                F2[i, j] = 0;
                F2[j, i] = 0;
                d1 = FastGraph.DegreeCount(F1);
                d2 = FastGraph.DegreeCount(F2);
            }
            j++;
            if (j > length - 1)
            {
                i--;
                j = 0;
            }
        }

        if ((F1 == null) && (F2 == null) && (i < 0)) return 0;

        return (1-((1-Rec(F1, d1)) * (1-Rec(F2, d2)) ));
    }
}




//main





int length = 5;
double[,]? mat = FastGraph.GenerateRandomMatrix(length);
//FastGraph.SaveFile("1.txt", mat);
mat = FastGraph.ReadFromFileAsync("1.txt");
int[]? degreeses;
degreeses=FastGraph.DegreeCount(mat);
stopWatch.Start();
Console.WriteLine("Start of calculation");
Console.WriteLine("The probability that this graph is connected is " + Rec(mat, degreeses));
stopWatch.Stop();
Console.WriteLine("Total program execution time " + stopWatch.ElapsedMilliseconds+"ms");


