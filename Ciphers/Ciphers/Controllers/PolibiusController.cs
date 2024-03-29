using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace PolibiusCipher.Controllers;

public class PolibiusController : Controller
{
    private static char[,] matrix =
    {
        { 'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E' },
        { 'Ę', 'F', 'G', 'H', 'I', 'J', 'K' },
        { 'L', 'Ł', 'M', 'N', 'Ń', 'O', 'Ó' },
        { 'P', 'Q', 'R', 'S', 'Ś', 'T', 'U' },
        { 'V', 'W', 'X', 'Y', 'Z', 'Ź', 'Ż' }
    };

    public ActionResult Index()
    {
        ViewBag.PolibiusMatrix = matrix;
        ViewBag.Table = tableToString(matrix);
        return View();
    }

    [HttpPost]
    public ActionResult EncryptDecrypt(string text, string action, char[] v)
    {
        if (string.IsNullOrEmpty(text))
        {
            ViewBag.DangerText = "Wprowadź tekst!";
            ViewBag.PolibiusMatrix = matrix;
            ViewBag.Table = tableToString(matrix);
            return View("Index");
        }
        
        try
        {
            matrix = PolibiusCipher.UpdateMatrix(v);
        }
        catch (IndexOutOfRangeException)
        {
            ViewBag.DangerText = "Nieprawidłowa tabela Polibiusza. Upewnij się, że każde pole jest wypełnione.";
            ViewBag.PolibiusMatrix = matrix;
            ViewBag.Table = tableToString(matrix);
            return View("Index");
        }
        catch (Exception e)
        {
            matrix = PolibiusCipher.UpdateMatrix(v, "no");
            ViewBag.DangerText = e.Message;
            ViewBag.PolibiusMatrix = matrix;
            ViewBag.Table = tableToString(matrix);
            return View("Index");
        }

        if (action == "encrypt")
            ViewBag.result = PolibiusCipher.Encrypt(text, matrix);
        else if (action == "decrypt")
            ViewBag.result = PolibiusCipher.Decrypt(text, matrix);

        ViewBag.PolibiusMatrix = matrix;
        ViewBag.Table = tableToString(matrix);
        return View("Index");
    }

    [HttpPost]
    public ActionResult SetPolibiustable(string stringTable = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż")
    {
        char[] table = new char[35];
        stringTable = stringTable.Trim().ToUpper();
        
        try
        {
            if (stringTable.Length > 35) throw new IndexOutOfRangeException("Zbyt duża ilość znaków do wczytania!");
            
            for (int i = 0; i < stringTable.Length; i++)
                table[i] = stringTable[i];
            
            matrix = PolibiusCipher.UpdateMatrix(table);
        }
        catch (IndexOutOfRangeException e)
        {
            if(e.Message == "Zbyt duża ilość znaków do wczytania!") 
                ViewBag.DangerText = e.Message;
            else
                ViewBag.DangerText = "Nieprawidłowa tabela Polibiusza. Upewnij się, że każde pole jest wypełnione.";
            ViewBag.PolibiusMatrix = matrix;
            ViewBag.Table = stringTable;
            return View("Index");
        }
        catch (Exception e)
        {
            matrix = PolibiusCipher.UpdateMatrix(table, "no");
            ViewBag.DangerText = e.Message;
            ViewBag.PolibiusMatrix = matrix;
            ViewBag.Table = stringTable;
            return View("Index");
        }

        ViewBag.PolibiusMatrix = matrix;
        ViewBag.Table = stringTable;
        return View("Index");
    }

    private string tableToString(char[,] c)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                sb.Append(c[i,j]);
            }
        }

        return sb.ToString();
    }
}

public class PolibiusCipher
{
    public static string Encrypt(string input, char[,] matrix)
    {
        input = input.ToUpper();
        StringBuilder encryptedText = new StringBuilder();
        int index = 1;

        foreach (char c in input)
        {
            if (c == ' ')
            {
                encryptedText.Append("");
                continue;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (matrix[i, j] == c)
                    {
                        if (IsPrime(index++))
                        {
                            encryptedText.Append((j + 1).ToString());
                            encryptedText.Append((i + 1).ToString());
                            break;
                        }

                        encryptedText.Append((i + 1).ToString());
                        encryptedText.Append((j + 1).ToString());
                        break;
                    }
                }
            }
        }

        return encryptedText.ToString();
    }

    public static string Decrypt(string input, char[,] matrix)
    {
        StringBuilder decryptedText = new StringBuilder();
        int index = 1;

        for (int i = 0; i < input.Length; i += 2)
        {
            int row, col;

            if (IsPrime(index++))
            {
                col = int.Parse(input[i].ToString()) - 1;
                row = int.Parse(input[i + 1].ToString()) - 1;
            }
            else
            {
                row = int.Parse(input[i].ToString()) - 1;
                col = int.Parse(input[i + 1].ToString()) - 1;
            }

            if (row >= 0 && row < 5 && col >= 0 && col < 7)
                decryptedText.Append(matrix[row, col]);
        }

        return decryptedText.ToString();
    }
    
    public static char[,] UpdateMatrix(char[] v, string checkForUnique = "yes")
    {
        char[,] matrix = new char[5,7];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                int index = i * 7 + j;
                matrix[i, j] = v[index];
            }
        }
        
        if(checkForUnique=="yes") 
            IsCustomTableUnique(matrix);
        
        return matrix;
    }

    private static void IsCustomTableUnique(char[,] customCharTable)
    {
        string[] customTable = new string[35];
        int counter = 0;
        foreach (char c in customCharTable)
            customTable[counter++] = c.ToString();
        
        
        HashSet<string> uniqueLetters = new HashSet<string>();
        
        for (int i = 0; i < customTable.Length; i++)
        {
            if (!string.IsNullOrEmpty(customTable[i]) && char.TryParse(customTable[i], out char parsedChar) &&
                char.IsLetter(parsedChar))
            {
                if (!uniqueLetters.Add(customTable[i].ToUpper()))
                    customTable[i] = "";
            }
            else
                throw new Exception("Nieprawidłowa tabela Polibiusza. Upewnij się, że każde pole jest wypełnione literą.");
        }

        foreach (var letter in customTable)
        {
            if (String.IsNullOrEmpty(letter))
                throw new Exception("Nieprawidłowa tabela Polibiusza. Upewnij się, że każda litera jest unikalna.");
        }
    }

    private static bool IsPrime(int number)
    {
        if (number <= 1)
            return false;

        if (number == 2)
            return true;

        if (number % 2 == 0)
            return false;

        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }
}