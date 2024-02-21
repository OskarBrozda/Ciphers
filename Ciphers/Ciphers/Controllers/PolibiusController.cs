using System;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace PolibiusCipher.Controllers
{
    public class PolibiusController : Controller
    {
        public static char[,] matrix =
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
        public ActionResult EncryptDecrypt(string text, string action,
            char v00, char v01, char v02, char v03, char v04, char v05, char v06,
            char v10, char v11, char v12, char v13, char v14, char v15, char v16,
            char v20, char v21, char v22, char v23, char v24, char v25, char v26,
            char v30, char v31, char v32, char v33, char v34, char v35, char v36,
            char v40, char v41, char v42, char v43, char v44, char v45, char v46)
        {
            if (!UpdateMatrix(v00, v01, v02, v03, v04, v05, v06,
                    v10, v11, v12, v13, v14, v15, v16,
                    v20, v21, v22, v23, v24, v25, v26,
                    v30, v31, v32, v33, v34, v35, v36,
                    v40, v41, v42, v43, v44, v45, v46))
            {
                ViewBag.PolibiusMatrix = matrix;
                return View("Index");
            }
            
            if (action == "encrypt")
            {
                ViewBag.result = PolibiusCipher.Encrypt(text);
            }
            else if (action == "decrypt")
            {
                ViewBag.result = PolibiusCipher.Decrypt(text);
            }
            
            ViewBag.PolibiusMatrix = matrix;
            return View("Index");
        }

        public bool UpdateMatrix(char v00, char v01, char v02, char v03, char v04, char v05, char v06,
            char v10, char v11, char v12, char v13, char v14, char v15, char v16,
            char v20, char v21, char v22, char v23, char v24, char v25, char v26,
            char v30, char v31, char v32, char v33, char v34, char v35, char v36,
            char v40, char v41, char v42, char v43, char v44, char v45, char v46)
        {
            char[,] result = new char[5, 7];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int index = i * 7 + j;
                    switch (index)
                    {
                        case 0:
                            result[i, j] = v00;
                            break;
                        case 1:
                            result[i, j] = v01;
                            break;
                        case 2:
                            result[i, j] = v02;
                            break;
                        case 3:
                            result[i, j] = v03;
                            break;
                        case 4:
                            result[i, j] = v04;
                            break;
                        case 5:
                            result[i, j] = v05;
                            break;
                        case 6:
                            result[i, j] = v06;
                            break;
                        case 7:
                            result[i, j] = v10;
                            break;
                        case 8:
                            result[i, j] = v11;
                            break;
                        case 9:
                            result[i, j] = v12;
                            break;
                        case 10:
                            result[i, j] = v13;
                            break;
                        case 11:
                            result[i, j] = v14;
                            break;
                        case 12:
                            result[i, j] = v15;
                            break;
                        case 13:
                            result[i, j] = v16;
                            break;
                        case 14:
                            result[i, j] = v20;
                            break;
                        case 15:
                            result[i, j] = v21;
                            break;
                        case 16:
                            result[i, j] = v22;
                            break;
                        case 17:
                            result[i, j] = v23;
                            break;
                        case 18:
                            result[i, j] = v24;
                            break;
                        case 19:
                            result[i, j] = v25;
                            break;
                        case 20:
                            result[i, j] = v26;
                            break;
                        case 21:
                            result[i, j] = v30;
                            break;
                        case 22:
                            result[i, j] = v31;
                            break;
                        case 23:
                            result[i, j] = v32;
                            break;
                        case 24:
                            result[i, j] = v33;
                            break;
                        case 25:
                            result[i, j] = v34;
                            break;
                        case 26:
                            result[i, j] = v35;
                            break;
                        case 27:
                            result[i, j] = v36;
                            break;
                        case 28:
                            result[i, j] = v40;
                            break;
                        case 29:
                            result[i, j] = v41;
                            break;
                        case 30:
                            result[i, j] = v42;
                            break;
                        case 31:
                            result[i, j] = v43;
                            break;
                        case 32:
                            result[i, j] = v44;
                            break;
                        case 33:
                            result[i, j] = v45;
                            break;
                        case 34:
                            result[i, j] = v46;
                            break;
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    matrix[i, j] = result[i, j];
                }
            }
            
            if (!IsCustomTableValidAndUnique(result)) return false;
            return true;
        }
        
        public bool IsCustomTableValidAndUnique(char[,] customCharTable)
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
        public static string Encrypt(string input)
        {
            input = input.ToUpper();
            StringBuilder encryptedText = new StringBuilder();

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
                        if (PolibiusController.matrix[i, j] == c)
                        {
                            encryptedText.Append((i + 1).ToString());
                            encryptedText.Append((j + 1).ToString());
                            break;
                        }
                    }
                }
            }

            return encryptedText.ToString();
        }

        public static string Decrypt(string input)
        {
            StringBuilder decryptedText = new StringBuilder();

            for (int i = 0; i < input.Length; i += 2)
            {
                int row = int.Parse(input[i].ToString()) - 1;
                int col = int.Parse(input[i + 1].ToString()) - 1;

                if (row >= 0 && row < 5 && col >= 0 && col < 7)
                {
                    decryptedText.Append(PolibiusController.matrix[row, col]);
                }
            }

            return decryptedText.ToString();
        }
    }
}