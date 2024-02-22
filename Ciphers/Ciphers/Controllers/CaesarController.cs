using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Ciphers.Controllers;

public class CaesarController : Controller
{
    private static int number = 1;
    
    public ActionResult Index()
    {
        ViewBag.Number = number;
        return View();
    }

    [HttpPost]
    public ActionResult EncryptDecrypt(string text, string action, int number)
    { 
        if (string.IsNullOrEmpty(text))
        {
            ViewBag.DangerText = "Wprowadź tekst!";
            ViewBag.Number = number;
            return View("Index");
        }
        
        if (number > 35 || number < 0)
        {
            ViewBag.DangerText = "Wprowadź poprawną wartość klucza!";
            ViewBag.Number = number;
            return View("Index");
        }
            
        if (action == "encrypt")
            ViewBag.result = CaesarCipher.Encrypt(text, number);
        else if (action == "decrypt")
            ViewBag.result = CaesarCipher.Decrypt(text, number);
            
        ViewBag.Number = number;
        return View("Index");
    }
}

public class CaesarCipher
{
    private const string polishAlphabet = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";

    public static string Encrypt(string text, int number)
    {
        StringBuilder encryptedText = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                int index = (polishAlphabet.IndexOf(char.ToLower(c)) + number) % polishAlphabet.Length;
                char encryptedChar = polishAlphabet[index];
                encryptedText.Append(encryptedChar);
            }
            else if (c == ' ')
                encryptedText.Append("");
        }

        return encryptedText.ToString().ToUpper();
    }

    public static string Decrypt(string text, int number)
    {
        StringBuilder decryptedText = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                int index = (polishAlphabet.IndexOf(char.ToLower(c)) - number + polishAlphabet.Length) %
                            polishAlphabet.Length;
                char decryptedChar = polishAlphabet[index];
                decryptedText.Append(decryptedChar);
            }
            else if (c == ' ')
                decryptedText.Append("");
        }

        return decryptedText.ToString().ToUpper();
    }
}