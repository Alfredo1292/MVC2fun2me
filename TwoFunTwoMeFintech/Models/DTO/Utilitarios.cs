﻿namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Utilitarios
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}