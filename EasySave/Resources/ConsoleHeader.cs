public static class ConsoleHeader
{
    public static void Display()
    {
        // Add one or two spaces at the top
        Console.WriteLine();
        Console.WriteLine();

        // Separator before the ASCII art
        WriteSeparator();

        string[] artLines = new string[]
        {
            @"$$$$$$$$\                                     $$$$$$\                                ",
            @"$$  _____|                                   $$  __$$\                               ",
            @"$$ |      $$$$$$\   $$$$$$$\ $$\   $$\       $$ /  \__| $$$$$$\ $$\    $$\  $$$$$$\  ",
            @"$$$$$\    \____$$\ $$  _____|$$ |  $$ |      \$$$$$$\   \____$$\\$$\  $$  |$$  __$$\ ",
            @"$$  __|   $$$$$$$ |\$$$$$$\  $$ |  $$ |       \____$$\  $$$$$$$ |\$$\$$  / $$$$$$$$ |",
            @"$$ |     $$  __$$ | \____$$\ $$ |  $$ |      $$\   $$ |$$  __$$ | \$$$  /  $$   ____|",
            @"$$$$$$$$\\$$$$$$$ |$$$$$$$  |\$$$$$$$ |      \$$$$$$  |\$$$$$$$ |  \$  /   \$$$$$$$ |",
            @"\________|\_______|\_______/  \____$$ |       \______/  \_______|   \_/     \_______|",
            @"                             $$\   $$ |                                              ",
            @"                             \$$$$$$  |                                              ",
            @"                              \______/                                               "
        };

        foreach (var line in artLines)
        {
            WriteCenteredLine(line);
        }

        // Separator after the ASCII art
        WriteSeparator();

        // Add one or two spaces at the bottom
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void WriteCenteredLine(string text)
    {
        int consoleWidth = Console.WindowWidth;
        int stringWidth = text.Length;
        int spaces = (consoleWidth - stringWidth) / 2;
        string paddedString = new string(' ', spaces) + text;
        Console.WriteLine(paddedString);
    }

    private static void WriteSeparator()
    {
        // Create a separator line of dashes that spans the width of the console
        string separator = new string('=', Console.WindowWidth);
        Console.WriteLine(separator);
    }
}
