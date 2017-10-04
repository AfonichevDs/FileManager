using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    static class AbbreviationDictionary
    {
        public static Dictionary<string, string> Abbreviations { get; set; }

        static AbbreviationDictionary()
        {
            Abbreviations = new Dictionary<string, string>();
            Abbreviations.Add("Пр.", "Приклад");
            Abbreviations.Add("Нар.", "Народний");
            Abbreviations.Add("Міжнар.", "Міжнародний");
            Abbreviations.Add("б.м", "Без місяця");
            Abbreviations.Add("ВНЗ", "Вищий навчальний заклад");
            Abbreviations.Add("НАН України", "Національна академія наук України");
            Abbreviations.Add("авт.", "Автор");
            Abbreviations.Add("адмін.", "Адміністрація");
            Abbreviations.Add("анот.", "Анотація");
            Abbreviations.Add("арк.", "Аркуш");
            Abbreviations.Add("вид.", "Видання");
            Abbreviations.Add("іл.", "Ілюстрація");
            Abbreviations.Add("каб.", "Кабінет");
            Abbreviations.Add("конф.", "Конференція");
            Abbreviations.Add("лаб.", "Лабораторія");
            Abbreviations.Add("мех.", "Механічний");
            Abbreviations.Add("обл.", "Область");
            Abbreviations.Add("прим.", "Примітка");
            Abbreviations.Add("проф.", "Професор");
            Abbreviations.Add("фр.", "Французька");
        }
    }
}
