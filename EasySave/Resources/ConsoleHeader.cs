public static class ConsoleHeader
{
    public static void Display()
    {
        // Adds an empty line at the top for better spacing.
        Console.WriteLine(Environment.NewLine);

        // Draws a separator line before displaying the ASCII art.
        WriteSeparator();

        // ASCII art lines representing the header.
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

        // Iterates over each line of the ASCII art and writes it to the console, centered.
        foreach (var line in artLines)
        {
            WriteCenteredLine(line);
        }

        // Draws a separator line after displaying the ASCII art.
        WriteSeparator();

        // Adds empty lines at the bottom for better spacing.
        Console.WriteLine();
        Console.WriteLine();
    }

    // Centers the provided text within the console window.
    private static void WriteCenteredLine(string text)
    {
        var spaces = (Console.WindowWidth - text.Length) / 2;
        Console.WriteLine(new string(' ', spaces < 0 ? 0 : spaces) + text);
    }

    // Draws a separator line across the width of the console window.
    private static void WriteSeparator() =>
        Console.WriteLine(new string('=', Console.WindowWidth));
}
