using System.Threading;

int[,] boardData = new int[,] {
    {0,0,0},
    {0,0,0},
    {0,0,0}
};

string getColorByItemLocation(int i, int j)
{
    int value = boardData[i, j];

    if (
        (i == 0 && j == 0 && value == 1) || (i == 0 && j == 1 && value == 2) || (i == 0 && j == 2 && value == 3) ||
        (i == 1 && j == 0 && value == 4) || (i == 1 && j == 1 && value == 5) || (i == 1 && j == 2 && value == 6) ||
        (i == 2 && j == 0 && value == 7) || (i == 2 && j == 1 && value == 8)
    ) return "\x1b[1m" + "\x1b[92m";

    return "\x1b[22m" + "\x1b[91m";
}
bool checkWins()
{
    if (
        (boardData[0, 0] == 1) && (boardData[0, 1] == 2) && (boardData[0, 2] == 3) &&
        (boardData[1, 0] == 4) && (boardData[1, 1] == 5) && (boardData[1, 2] == 6) &&
        (boardData[2, 0] == 7) && (boardData[2, 1] == 8)
    ) return true;

    return false;
}
bool shiftItem(ConsoleKey direction)
{
    int[] pureItemIndex = [-1, -1];

    for (int i = 0; i < boardData.GetLength(0); i++)
        for (int j = 0; j < boardData.GetLength(1); j++)
            if (boardData[i, j] == 0)
                pureItemIndex = [i, j];

    int index1 = 0;
    int index2 = 0;

    if (direction == ConsoleKey.UpArrow) index1++;
    if (direction == ConsoleKey.DownArrow) index1--;
    if (direction == ConsoleKey.LeftArrow) index2++;
    if (direction == ConsoleKey.RightArrow) index2--;

    int? value = null;

    try
    {
        value = (int?)boardData.GetValue(pureItemIndex[0] + index1, pureItemIndex[1] + index2);
    }
    catch (Exception)
    { }

    if (value == null)
    {
        return false;
    }

    boardData[pureItemIndex[0], pureItemIndex[1]] = (int)value;
    boardData[pureItemIndex[0] + index1, pureItemIndex[1] + index2] = 0;

    return true;
}
void randomBoard()
{
    List<int> data = [0, 1, 2, 3, 4, 5, 6, 7, 8];

    for (int i = 0; i < boardData.GetLength(0); i++)
    {
        for (int j = 0; j < boardData.GetLength(1); j++)
        {
            Random random = new();
            int randomIndex = random.Next(data.ToArray().Length);

            boardData[i, j] = data[randomIndex];
            data.RemoveAt(randomIndex);
        }
    }
}
string renderBoard()
{
    string result = "";
    int left = (Console.WindowWidth - 13) / 2;
    // int left = 3;
    string space = "";

    for (int i = 0; i < (Console.WindowHeight - 7) / 2; i++) result += "\n";
    for (int i = 0; i < left; i++) space += " ";

    for (int i = 0; i < boardData.GetLength(0); i++)
    {

        if (i == 0) result += space + "┌───┬───┬───┐" + "\n";

        for (int j = 0; j < boardData.GetLength(1); j++)
        {
            if (j == 0) result += space + ("│ ");

            if (boardData[i, j] == 0)
                result += "  │ ";
            else
                result += getColorByItemLocation(i, j) + boardData[i, j] + "\x1b[39m" + " │ ";


            if (j == boardData.GetLength(1) - 1) result += "\n";
        }

        if (i < boardData.GetLength(0) - 1) result += space + "├───┼───┼───┤" + "\n";
        else result += space + "└───┴───┴───┘";
    }
    for (int i = 0; i < (Console.WindowHeight - 10) / 2; i++) result += "\n";

    return result;
}

randomBoard();

ConsoleKey lastKey = ConsoleKey.None;

int i = 0;

while (true)
{
    // TODO: `makeMessage`
    Console.Clear();
    bool itemShifted = shiftItem(lastKey);
    Console.WriteLine(renderBoard());

    string direction = "✗";

    if (lastKey == ConsoleKey.UpArrow) direction = "↑";
    else if (lastKey == ConsoleKey.DownArrow) direction = "↓";
    else if (lastKey == ConsoleKey.LeftArrow) direction = "←";
    else if (lastKey == ConsoleKey.RightArrow) direction = "→";

    if (itemShifted)
        Console.WriteLine($"✔ Shifted to {direction} successfully");
    else
        Console.WriteLine($"✗ Can't Shift to {direction} !!");

    if (checkWins()) break;

    lastKey = Console.ReadKey().Key;
}

Console.Clear();
Console.WriteLine("You Win!");
