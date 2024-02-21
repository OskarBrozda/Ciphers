using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Ciphers.Controllers;

public class TrithemiusController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult EncryptDecrypt(string text, string action)
    {
        if (string.IsNullOrEmpty(text))
        {
            ViewBag.DangerText = "Wprowadź tekst!";
            return View("Index");
        }
        
        if (action == "encrypt")
            ViewBag.result = TrithemiusCipher.Encrypt(text);
        else if (action == "decrypt")
            ViewBag.result = TrithemiusCipher.Decrypt(text);

        
        return View("Index");
    }
}

public class TrithemiusCipher
{
    public static string Encrypt(string text)
    {
        StringBuilder encryptedText = new StringBuilder();
        int index = 0;
        
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (index == 35) index %= 35;
                encryptedText.Append(CaesarCipher.Encrypt(c.ToString(), index));
                index++;
            }
            else if (c == ' ')
                encryptedText.Append("");
        }

        return encryptedText.ToString();
    }

    public static string Decrypt(string text)
    {
        StringBuilder decryptedText = new StringBuilder();
        int index = 0;
        
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (index == 35) index %= 35;
                decryptedText.Append(CaesarCipher.Decrypt(c.ToString(), index));
                index++;
            }
            else if (c == ' ')
                decryptedText.Append("");
        }
        
        return decryptedText.ToString();
    }
}