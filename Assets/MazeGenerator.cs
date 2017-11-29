public class MazeGenerator {
    private const int JUMP_SIZE = 2;
    private System.Random rng = new System.Random();
    private bool[,] maze;
    private int mazeSize;

    public bool[,] GenerateMaze(int _mazeSize) {
        mazeSize = _mazeSize * JUMP_SIZE - 1;
        maze = new bool[mazeSize, mazeSize];
        DepthFirstSearch(0, 0);
        return maze;
    }

    private void DepthFirstSearch(int _x, int _y) {
        // Sets current cell as visited.
        maze[_x, _y] = true;
        // Sets orderOfSearch to a random permutation of {0,1,2,3}.
        int[] orderOfSearch = new int[] { 0, 1, 2, 3 };
        for (int i = 0; i < 4; i++) {
            int r = rng.Next(i, 4);
            int temp = orderOfSearch[r];
            orderOfSearch[r] = orderOfSearch[i];
            orderOfSearch[i] = temp;
        }
        // Tries to visit cells to the North, East, South, and West in order of orderOfSearch.
        for (int i = 0; i < 4; i++) {
            // Recurses if cell is within array bounds amd has not already been visited.
            if ((orderOfSearch[0] == i) && (_y + JUMP_SIZE < mazeSize) && (!maze[_x, _y + JUMP_SIZE])) {
                // Sets cell between current cell and next cell as visited.
                maze[_x, _y + 1] = true;
                DepthFirstSearch(_x, _y + JUMP_SIZE);
            } else if ((orderOfSearch[1] == i) && (_x + JUMP_SIZE < mazeSize) && (!maze[_x + JUMP_SIZE, _y])) {
                maze[_x + 1, _y] = true;
                DepthFirstSearch(_x + JUMP_SIZE, _y);
            } else if ((orderOfSearch[2] == i) && (_y - JUMP_SIZE >= 0) && (!maze[_x, _y - JUMP_SIZE])) {
                maze[_x, _y - 1] = true;
                DepthFirstSearch(_x, _y - JUMP_SIZE);
            } else if ((orderOfSearch[3] == i) && (_x - JUMP_SIZE >= 0) && (!maze[_x - JUMP_SIZE, _y])) {
                maze[_x - 1, _y] = true;
                DepthFirstSearch(_x - JUMP_SIZE, _y);
            }
        }
    }
}
