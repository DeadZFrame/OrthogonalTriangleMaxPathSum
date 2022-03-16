using System;
using System.IO;

namespace CanAltay
{
    static class MaxPathSum
    {
        const string input =  @"215
                                193 124
                                117 237 442
                                218 935 347 235
                                320 804 522 417 345
                                229 601 723 835 133 124
                                248 202 277 433 207 263 257
                                359 464 504 528 516 716 871 182
                                461 441 426 656 863 560 380 171 923
                                381 348 573 533 447 632 387 176 975 449
                                223 711 445 645 245 543 931 532 937 541 444
                                330 131 333 928 377 733 017 778 839 168 197 197
                                131 171 522 137 217 224 291 413 528 520 227 229 928
                                223 626 034 683 839 053 627 310 713 999 629 817 410 121
                                924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

        static void Main(String[] args)
        {
            string[] inputToArray = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //split values into an array and clear spaces

            int[,] matrix = ToMatrix(input, inputToArray);
            int maxSum = TraverseNode(matrix, inputToArray);

            Console.WriteLine("The result for input in Question2 is " + maxSum + ".");

            Console.WriteLine("Please enter the file path for Question1: ");

            //string filePath = @"C:\Users\Lenovo\Desktop\MaxSum.txt";

            string read = Console.ReadLine();
            string file = File.ReadAllText(read); //read from file

            string[] fileToArray = file.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //file input into String array

            int[,] matrixFile = ToMatrix(file, fileToArray);
            int maxSumFile = TraverseNode(matrixFile, fileToArray);

            Console.WriteLine("The result for input in file is " + maxSumFile + ".");
        }

        static int TraverseNode(this int[,] arrayToMatrix, string[] inputToArray) //traverse the matrix trough downwards and diagonally adjanted nodes
        {
            int prevValue = 0; //referance value for finding the matrix of previous row
            int prevMatrix = 0; //reference value for finding adjanted columns of next row

            int max = 0; //max value for one row
            int sum = 0; //to sum max values

            for (int row = 0; row < inputToArray.GetLength(0); row++) //traverse every row
            {
                for (int col = 0; col <= row; col++) //and every column
                {
                    if (row == 0) //first node
                    {
                        sum += arrayToMatrix[row, col];
                    }
                    else if (row == 1) //find max of only two values, because there is only two nodes on this row
                    {
                        if (col == 1)
                            max = Math.Max(arrayToMatrix[row, col - 1], arrayToMatrix[row, col]);
                        prevValue = max;
                    }
                    else if (row == 2) //prevents traverse of non-adjanted nodes for this row
                    {
                        if (col == 0)
                            prevMatrix = FindPrevMatrix(arrayToMatrix, prevValue, inputToArray);
                        if (col == 1)
                        {
                            max = Math.Max(arrayToMatrix[row, col - 1], arrayToMatrix[row, col]);
                            if (prevMatrix > 0)
                                max = Math.Max(arrayToMatrix[row, col + 1], max);
                            prevValue = max;
                        }
                    }
                    else if (row > 2) //traverse all adjanted nodes for the rest of the rows
                    {   
                        if (col == 0)
                            prevMatrix = FindPrevMatrix(arrayToMatrix, prevValue, inputToArray);
                        if (prevMatrix == 0)
                        {
                            max = Math.Max(arrayToMatrix[row, col +1], arrayToMatrix[row, col]);
                            prevValue = max;
                        }
                        else if (col != 0 && col == prevMatrix)
                        {
                            max = Math.Max(arrayToMatrix[row, col - 1], arrayToMatrix[row, col]);
                            max = Math.Max(arrayToMatrix[row, col + 1], max);
                            prevValue = max;
                        }
                    }
                }
                sum += max; //sum each max value
                //Console.WriteLine(max);
            }

            return sum;
        }

        static int FindPrevMatrix(this int[,] arrayToMatrix, int prevValue, string[] inputToArray) //finds previous column of matrix for adjanted traverse
        {
            int prevMatrix = 0;

            for(int row = 0; row < inputToArray.GetLength(0); row++)
            {
                for(int col = 0; col <= row; col++)
                {
                    if(arrayToMatrix[row, col] == prevValue)
                    {
                        prevMatrix = col;
                    }
                }
            }

            return prevMatrix;
        }

        static int[,] ToMatrix(this string input, string[] inputToArray)  //converts string array to matrix
        {
            int[,] arrayToMatrix = new int[inputToArray.Length, inputToArray.Length + 1];

            for (int row = 0; row < inputToArray.Length; row++)
            {
                string[] eachCharInRow = inputToArray[row].Trim().Split(' '); //assaigns values to rows

                for (int col = 0; col < eachCharInRow.Length; col++)
                {
                    int num;
                    int.TryParse(eachCharInRow[col], out num); //converts to int of string's colums
                    arrayToMatrix[row, col] = num;
                }
            }
            
            return EliminatePrimes(arrayToMatrix);
        }

        static bool IsPrime(int num) //checks if prime or not
        {
            if (num <= 1 || num % 2 == 0) return false;
            if (num == 2) return true;

            var boundary = (int)Math.Floor(Math.Sqrt(num)); //mathematical formula of prime number boundary

            for (int i = 3; i <= boundary; i += 2) //checks divisibility by numbers except itself
                if (num % i == 0)
                    return false;

            return true;
        }

        static int[,] EliminatePrimes(this int[,] arrayToMatrix) //sets all prime numbers to -1
        {
            int length = arrayToMatrix.GetLength(0);
            for (int row = 0; row < length; row++)
            {
                for (int col = 0; col < length; col++)
                {
                    if (arrayToMatrix[row, col] == 0)
                        continue;
                    else if (IsPrime(arrayToMatrix[row, col]))
                        arrayToMatrix[row, col] = -1;
                }
            }

            return arrayToMatrix;
        }
    }
}