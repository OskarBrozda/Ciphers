using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace PolibiusCipher.Controllers
{
    public class PolibiusController : Controller
    {
        public static char[,] matrix = {
            {'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E'},
            {'Ę', 'F', 'G', 'H', 'I', 'J', 'K'},
            {'L', 'Ł', 'M', 'N', 'Ń', 'O', 'Ó'},
            {'P', 'Q', 'R', 'S', 'Ś', 'T', 'U'},
            {'V', 'W', 'X', 'Y', 'Z', 'Ź', 'Ż'}
        };

        public ActionResult Index()
        {
            ViewBag.PolibiusMatrix = matrix;
            return View();
        }

        [HttpPost]
        public ActionResult EncryptDecrypt(string text, string action)
        {
            string result = "";

            if (action == "Encrypt")
            {
                result = PolibiusCipher.Encrypt(text);
            }
            else if (action == "Decrypt")
            {
                result = PolibiusCipher.Decrypt(text);
            }

            ViewBag.Result = result;
            ViewBag.PolibiusMatrix = matrix;
            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateMatrix(char v00, char v01, char v02, char v03, char v04, char v05, char v06,
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
                        case 0: result[i, j] = v00; break;
                        case 1: result[i, j] = v01; break;
                        case 2: result[i, j] = v02; break;
                        case 3: result[i, j] = v03; break;
                        case 4: result[i, j] = v04; break;
                        case 5: result[i, j] = v05; break;
                        case 6: result[i, j] = v06; break;
                        case 7: result[i, j] = v10; break;
                        case 8: result[i, j] = v11; break;
                        case 9: result[i, j] = v12; break;
                        case 10: result[i, j] = v13; break;
                        case 11: result[i, j] = v14; break;
                        case 12: result[i, j] = v15; break;
                        case 13: result[i, j] = v16; break;
                        case 14: result[i, j] = v20; break;
                        case 15: result[i, j] = v21; break;
                        case 16: result[i, j] = v22; break;
                        case 17: result[i, j] = v23; break;
                        case 18: result[i, j] = v24; break;
                        case 19: result[i, j] = v25; break;
                        case 20: result[i, j] = v26; break;
                        case 21: result[i, j] = v30; break;
                        case 22: result[i, j] = v31; break;
                        case 23: result[i, j] = v32; break;
                        case 24: result[i, j] = v33; break;
                        case 25: result[i, j] = v34; break;
                        case 26: result[i, j] = v35; break;
                        case 27: result[i, j] = v36; break;
                        case 28: result[i, j] = v40; break;
                        case 29: result[i, j] = v41; break;
                        case 30: result[i, j] = v42; break;
                        case 31: result[i, j] = v43; break;
                        case 32: result[i, j] = v44; break;
                        case 33: result[i, j] = v45; break;
                        case 34: result[i, j] = v46; break;
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
            
            ViewBag.PolibiusMatrix = matrix; 
            return View("Index");
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