using CatHack.modules;
using WindowsInput;
using WindowsInput.Native;

namespace Decidueye
{
    public class Gliding
    {
        private static readonly InputSimulator inputSimulator = new InputSimulator();
        private static Point noTarget = new Point(0, 0);
        private static Point closestPosition = new Point(0, 0);
        private static Point enemyPos = new Point(0, 0);
        private static double distance = 1000;

        public static void autoShift()
        {
            var lowestDistance = 1000.0;
            foreach (Point x in ChampPosition.GetAllEnemyPositions())
            {
                Point enemyPos = ChampPosition.GetEnemyPosition(); // get the enemy position
                Point mousePos = System.Windows.Forms.Cursor.Position; // get the current mouse position
                double deltaX = enemyPos.X - mousePos.X; // calculate the X difference
                double deltaY = enemyPos.Y - mousePos.Y; // calculate the Y difference
                double distance = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)); // calculate the distance using the distance formula 
                if (distance < lowestDistance) lowestDistance = distance;
            }
            if (enemyPos == noTarget) { }
            else if (lowestDistance > 265)
            {
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                Variables.alreadyDown = false;
            }
            else if (lowestDistance <= 215 && !Variables.alreadyDown)
            {
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                Variables.alreadyDown = true;
            }
        }
    }
    //new idea to make a square around cursor being 
}
