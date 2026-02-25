using System.Text;
using System.Collections.Generic;

public static class PlayfairCipher
{
    static char[,] table = new char[5, 5];

    public static string Encrypt(string plaintext, string key)
    {
        GenerateTable(key);
        plaintext = PrepareText(plaintext);

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < plaintext.Length; i += 2)
        {
            EncryptPair(plaintext[i], plaintext[i + 1], result);
        }

        return result.ToString();
    }

    static void GenerateTable(string key)
    {
        HashSet<char> used = new HashSet<char>();
        key = key.ToUpper().Replace("J", "I");

        int x = 0, y = 0;

        foreach (char c in key)
        {
            if (!used.Contains(c) && char.IsLetter(c))
            {
                table[x, y++] = c;
                used.Add(c);
                if (y == 5) { y = 0; x++; }
            }
        }

        for (char c = 'A'; c <= 'Z'; c++)
        {
            if (c == 'J' || used.Contains(c)) continue;
            table[x, y++] = c;
            if (y == 5) { y = 0; x++; }
        }
    }

    static void EncryptPair(char a, char b, StringBuilder sb)
    {
        (int ax, int ay) = Find(a);
        (int bx, int by) = Find(b);

        if (ax == bx)
        {
            sb.Append(table[ax, (ay + 1) % 5]);
            sb.Append(table[bx, (by + 1) % 5]);
        }
        else if (ay == by)
        {
            sb.Append(table[(ax + 1) % 5, ay]);
            sb.Append(table[(bx + 1) % 5, by]);
        }
        else
        {
            sb.Append(table[ax, by]);
            sb.Append(table[bx, ay]);
        }
    }

    static (int, int) Find(char c)
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                if (table[i, j] == c)
                    return (i, j);
        return (-1, -1);
    }

    static string PrepareText(string text)
    {
        text = text.ToUpper().Replace("J", "I");
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            if (!char.IsLetter(text[i])) continue;
            sb.Append(text[i]);

            if (i + 1 < text.Length && text[i] == text[i + 1])
                sb.Append('X');
        }

        if (sb.Length % 2 != 0)
            sb.Append('X');

        return sb.ToString();
    }
    public static char[,] GeneratePlayfairTable(string key)
    {
        key = key.ToUpper().Replace("J", "I");

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        string finalKey = "";

        foreach (char c in key)
        {
            if (!finalKey.Contains(c.ToString()))
                finalKey += c;
        }

        foreach (char c in alphabet)
        {
            if (!finalKey.Contains(c.ToString()))
                finalKey += c;
        }

        char[,] table = new char[5, 5];

        int index = 0;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                table[row, col] = finalKey[index];
                index++;
            }
        }

        return table;
    }

}
