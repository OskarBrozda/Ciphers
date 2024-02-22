using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Ciphers.Controllers;

public class VigenereController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult EncryptDecrypt(string text, string action, string keyWorld)
    {
        if (string.IsNullOrEmpty(text))
        {
            ViewBag.DangerText = "Wprowadź tekst!";
            ViewBag.KeyWorld = keyWorld;
            return View("Index");
        }
        
        if (string.IsNullOrEmpty(keyWorld))
        {
            ViewBag.DangerText = "Wprowadź słowo klucz!";
            ViewBag.KeyWorld = keyWorld;
            return View("Index");
        }
        
        if (action == "encrypt")
            ViewBag.result = VigenereCipher.Encrypt(text, keyWorld);
        else if (action == "decrypt")
            ViewBag.result = VigenereCipher.Decrypt(text, keyWorld);

        ViewBag.KeyWorld = keyWorld;
        return View("Index");
    }
}

public class VigenereCipher
{
    public static string Encrypt(string text, string keyWorld)
    {
        List<int> key = KeyWorldEncrypt(keyWorld);
        StringBuilder encryptedText = new StringBuilder();
        int index = 0;

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (index == keyWorld.Length) index %= keyWorld.Length;
                encryptedText.Append(CaesarCipher.Encrypt(c.ToString(), key[index]));
                index++;
            }
            else if (c == ' ')
                encryptedText.Append("");
            
        }
        return encryptedText.ToString();
    }

    public static string Decrypt(string text, string keyWorld)
    {
        List<int> key = KeyWorldEncrypt(keyWorld);
        StringBuilder decryptedText = new StringBuilder();
        int index = 0;
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (index == keyWorld.Length) index %= keyWorld.Length;
                decryptedText.Append(CaesarCipher.Decrypt(c.ToString(), key[index]));
                index++;
            }
            else if (c == ' ')
                decryptedText.Append("");
        }

        return decryptedText.ToString();
    }

    private static List<int> KeyWorldEncrypt(string keyWorld)
    {
        const string polishAlphabet = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";
        List<int> key = new();
        foreach (char c in keyWorld)
        {
            key.Add(polishAlphabet.IndexOf(c));
        }
        return key;
    }
}