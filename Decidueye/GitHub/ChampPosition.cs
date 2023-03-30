using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace CatHack.modules
{
    class ChampPosition
    {
        private static readonly Color RGB_ENEMY_LEVEL_NUMBER_COLOR = Color.FromArgb(0xFD, 0xE9, 0xE9); // 0xFF, 0xEB, 0xEB
        private static Point[] Searched;
        private static Rectangle FOV;

        public static Point GetEnemyPosition()
        {
            // Define a rectangle representing the area on the screen where the enemy might be
            Rectangle enemyDetectionArea = new Rectangle(200, 0, 1400, 900);

            // Search for pixels of a specific color within the enemy detection area
            Point[] detectedPixels = PixelSearch.Search(enemyDetectionArea, RGB_ENEMY_LEVEL_NUMBER_COLOR, 1);

            // Create a point object to store the calculated position of the enemy
            Point enemyPosition = new Point();

            // If any pixels matching the color were found
            if (detectedPixels.Length != 0)
            {
                // Sort the detected pixels by their Y coordinate
                Point[] orderedPixelsByY = detectedPixels.OrderBy(pixel => pixel.Y).ToArray<Point>();

                // Create a list of tuples containing the detected pixels and their distances from the top-left corner of the enemy detection area
                List<Tuple<SharpDX.Vector2, double>> pixelDistancesFromCorner = new List<Tuple<SharpDX.Vector2, double>>();
                Point[] pixelsToCheck = orderedPixelsByY;

                // Loop through the detected pixels
                for (int i = 0; i < pixelsToCheck.Length; i++)
                {
                    Point currentPixel = pixelsToCheck[i];

                    // Create a 2D vector representing the current pixel
                    SharpDX.Vector2 currentPixelVector = new SharpDX.Vector2((float)currentPixel.X, (float)currentPixel.Y);

                    // Check if the current pixel is within 25 pixels of any previously added vectors
                    if ((from tuple in pixelDistancesFromCorner where (tuple.Item1 - currentPixelVector).Length() < 25f || Math.Abs(tuple.Item1.X - currentPixelVector.X) < 25f select tuple).Count<Tuple<SharpDX.Vector2, double>>() < 1)
                    {
                        // Add the current vector to the list along with its distance from the top-left corner of the enemy detection area
                        pixelDistancesFromCorner.Add(new Tuple<SharpDX.Vector2, double>(currentPixelVector, (double)(currentPixelVector - new SharpDX.Vector2((float)enemyDetectionArea.X, (float)enemyDetectionArea.Y)).Length()));

                        // If the list contains more than two vectors, break out of the loop
                        if (pixelDistancesFromCorner.Count > 2)
                        {
                            break;
                        }
                    }
                }

                // Sort the list of vectors by their distance from the top-left corner of the enemy detection area, and select the closest one
                Tuple<SharpDX.Vector2, double> closestPixelTuple = (from tuple in pixelDistancesFromCorner orderby tuple.Item2 select tuple).ElementAt(0);

                // Create a point object based on the selected vector, and offset it by a fixed amount
                Point closestPixelPoint = new Point((int)closestPixelTuple.Item1.X, (int)closestPixelTuple.Item1.Y);
                enemyPosition.X = closestPixelPoint.X + 53;
                enemyPosition.Y = closestPixelPoint.Y + 100;
            }

            // Return the calculated position of the enemy
            return enemyPosition;
        }

        public static List<Point> GetAllEnemyPositions()
        {
            // Define a rectangle representing the area on the screen where the enemies might be
            Rectangle enemyDetectionArea = new Rectangle(200, 0, 1400, 900);

            // Create a list to store the positions of all enemies found
            List<Point> enemyPositions = new List<Point>();

            // Loop until no more enemy pixels are detected
            while (true)
            {
                // Search for pixels of a specific color within the enemy detection area
                Point[] detectedPixels = PixelSearch.Search(enemyDetectionArea, RGB_ENEMY_LEVEL_NUMBER_COLOR, 1);

                // If no more enemy pixels are detected, exit the loop
                if (detectedPixels.Length == 0)
                {
                    break;
                }

                // Sort the detected pixels by their Y coordinate
                Point[] orderedPixelsByY = detectedPixels.OrderBy(pixel => pixel.Y).ToArray<Point>();

                // Create a list of tuples containing the detected pixels and their distances from the top-left corner of the enemy detection area
                List<Tuple<SharpDX.Vector2, double>> pixelDistancesFromCorner = new List<Tuple<SharpDX.Vector2, double>>();
                Point[] pixelsToCheck = orderedPixelsByY;

                // Loop through the detected pixels
                for (int i = 0; i < pixelsToCheck.Length; i++)
                {
                    Point currentPixel = pixelsToCheck[i];

                    // Create a 2D vector representing the current pixel
                    SharpDX.Vector2 currentPixelVector = new SharpDX.Vector2((float)currentPixel.X, (float)currentPixel.Y);

                    // Check if the current pixel is within 25 pixels of any previously added vectors
                    if ((from tuple in pixelDistancesFromCorner where (tuple.Item1 - currentPixelVector).Length() < 25f || Math.Abs(tuple.Item1.X - currentPixelVector.X) < 25f select tuple).Count<Tuple<SharpDX.Vector2, double>>() < 1)
                    {
                        // Add the current vector to the list along with its distance from the top-left corner of the enemy detection area
                        pixelDistancesFromCorner.Add(new Tuple<SharpDX.Vector2, double>(currentPixelVector, (double)(currentPixelVector - new SharpDX.Vector2((float)enemyDetectionArea.X, (float)enemyDetectionArea.Y)).Length()));

                        // If the list contains more than two vectors, break out of the loop
                        if (pixelDistancesFromCorner.Count > 2)
                        {
                            break;
                        }
                    }
                }

                // Sort the list of vectors by their distance from the top-left corner of the enemy detection area, and select the closest one
                Tuple<SharpDX.Vector2, double> closestPixelTuple = (from tuple in pixelDistancesFromCorner orderby tuple.Item2 select tuple).ElementAt(0);

                // Create a point object based on the selected vector, and offset it by a fixed amount
                Point closestPixelPoint = new Point((int)closestPixelTuple.Item1.X, (int)closestPixelTuple.Item1.Y);
                Point enemyPosition = new Point(closestPixelPoint.X + 53, closestPixelPoint.Y + 100);

                // Add the enemy position to the list of enemy positions
                enemyPositions.Add(enemyPosition);

                // Set the enemy detection area to exclude the previously detected enemy pixel
                enemyDetectionArea = new Rectangle(enemyDetectionArea.X, enemyDetectionArea.Y, enemyDetectionArea.Width, enemyDetectionArea.Height - (enemyPosition.Y - enemyDetectionArea.Y));
            }
            // Return the list of all enemy positions
            return enemyPositions;
        }
    }
}