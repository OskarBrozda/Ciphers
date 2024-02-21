using System.Text;
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
            return View();
        }

        [HttpPost]
        public ActionResult EncryptDecrypt(string text, string action, char[] v)
        {
            if (!UpdateMatrix(v))
            {
                ViewBag.PolibiusMatrix = matrix;
                return View("Index");
            }
            
            if (string.IsNullOrEmpty(text))
            {
                ViewBag.DangerText = "Wprowadź tekst!";
                ViewBag.PolibiusMatrix = matrix;
                return View("Index");
            }
            
            if (action == "encrypt")
                ViewBag.result = PolibiusCipher.Encrypt(text, matrix);
            else if (action == "decrypt")
                ViewBag.result = PolibiusCipher.Decrypt(text, matrix);
            
            ViewBag.PolibiusMatrix = matrix;
            return View("Index");
        }

        private bool UpdateMatrix(char[] v)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int index = i * 7 + j;
                    matrix[i, j] = v[index];
                }
            }
            
            if (!IsCustomTableValidAndUnique(matrix)) return false;
            return true;
        }
        
        private bool IsCustomTableValidAndUnique(char[,] customCharTable)
        {
            string[] customTable = new string[35];
            int counter = 0;
            foreach (char c in customCharTable)
                customTable[counter++] = c.ToString();
         
         
            HashSet<string> uniqueLetters = new HashSet<string>();
    
            for (int i = 0; i < customTable.Length; i++)
            {
                if (!string.IsNullOrEmpty(customTable[i]) && char.TryParse(customTable[i], out char parsedChar) && char.IsLetter(parsedChar))
                {
                    if (!uniqueLetters.Add(customTable[i].ToUpper()))
                        customTable[i] = "";
                }
                else
                {
                    ViewBag.DangerText = "Nieprawidłowa tabela Polibiusza. Upewnij się, że każde pole jest wypełnione.";
                    return false;
                }
            }
    
            foreach (var letter in customTable)
            {
                if (String.IsNullOrEmpty(letter))
                {
                    ViewBag.DangerText =  "Nieprawidłowa tabela Polibiusza. Upewnij się, że każda litera jest unikalna.";
                    return false;
                }
            }
    
            return true;
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
                {
                    decryptedText.Append(matrix[row, col]);
                }
            }

            return decryptedText.ToString();
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