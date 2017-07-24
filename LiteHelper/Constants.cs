using System;
using System.Collections.Generic;

namespace LiteHelper
{
    public static class Constants
    {

        public static string CrashlyticsKey = "d47c40ae7f8ae5ef15505b369e03162d2b40980e";

        public static string ShowMenuMessage = "ShowMenuMessage";

        public static string Key1 = "Key1";
        public static string Key2 = "Key2";
        public static string Key3 = "Key3";
        public static string Key4 = "Key4";
        public static string Key5 = "Key5";

        public static string City = "City";
        public static string Pin = "Pin";
        public static string Project = "Project";

        public static string Codes = "Codes";
        public static string VibroOff = "VibroOff";


        public static string DefaultCityCode = "kaliningrad";
        public static string DefaultCity = "Калининград";
        public static string DefaultProject = "Dozor Lite";
        public static string DefaultProjectCode = "lite";
        public static string Paid = "Paid";


        public static string CodeStatusWrong = "Код не принят.";
        public static string CodeStatusSending = "Отправляется...";
        public static string CodeStatusTimeOut = "Нет сети.";

        public static string NoNetwork = "Нет сети.";


        public static Dictionary<string, string> CityList = new Dictionary<string, string> {
            {"Agent24", "agent24"},
            {"Chara", "chara"},
            {"Fasten", "fasten"},
            {"Game", "game"},
            {"Griddynamics", "griddynamics"},
            {"I Love Msk", "ilovemsk"},
            {"Memory", "memory"},
            {"Munich", "munich"},
            {"Packman", "pacman"},
            {"Pattaya", "pattaya"},
            {"Puzzle", "puzzle"},
            {"Rossosh", "rossosh"},
            {"Walk", "walk"},
            {"Абакан", "abakan"},
            {"Антверпен", "antwerp"},
            {"Армавир", "armawir"},
            {"Архангельск", "arh"},
            {"Астрахань", "astrakhan"},
            {"Ахтубинск", "ahtubinsk"},
            {"Ачинск", "achinsk"},
            {"Балаково", "balakovo"},
            {"Белово", "belovo"},
            {"Великие Луки", "vluki"},
            {"Владивосток", "vl"},
            {"Владикавказ", "vladikavkaz"},
            {"Волгоград", "vlg"},
            {"Воронеж", "vrn"},
            {"Выкса", "viksa"},
            {"Гамбург", "hamburg"},
            {"Димитровград", "dimitrovgrad"},
            {"Димитров", "dmitrov"},
            {"Донецк", "donetsk"},
            {"Дубна", "dubna"},
            {"Екатеринбург", "e-burg"},
            {"Запорожье", "zp"},
            {"Зеленоград", "zelenograd"},
            {"Ижевск", "izhevsk"},
            {"Иркутск", "irkutsk"},
            {"Казань", "kazan"},
            {"Калининград", "kaliningrad"},
            {"Калуга", "kaluga"},
            {"Кемерово", "kemerovo"},
            {"Киров", "kirov"},
            {"Краснодар", "krasnodar"},
            {"Красноярск", "krsk"},
            {"Кременчуг", "kremenchug"},
            {"Кубок", "kubok"},
            {"Кулебаки", "kulebaki"},
            {"Лиски", "liski"},
            {"Люберцы", "lubercy"},
            {"Мега-Волгоград", "mega-vlg"},
            {"Мега-Иркутск", "mega-irkutsk"},
            {"Мега-Смоленск", "mega-smolensk"},
            {"Миргород и Лубны", "mirgorod"},
            {"Москва", "moscow"},
            {"Москва-beta", "msk"},
            {"Набережные Челны", "chelny"},
            {"Нерюнгри", "nerungri"},
            {"Нижний Новгород", "nnov"},
            {"Новокузнецк", "nvkz"},
            {"Новороссийск", "nvrsk"},
            {"Новосибирск", "novosib"},
            {"Норильск", "norilsk"},
            {"Обнинск", "obninsk"},
            {"Орел", "orel"},
            {"Оренбург", "orenburg"},
            {"Пермь", "perm"},
            {"Подгорное", "demo"},
            {"Полтава", "poltava"},
            {"Псков", "pskov"},
            {"Раменское", "ramenskoe"},
            {"Рига", "riga"},
            {"Ростов-на-Дону", "rostov"},
            {"Самара", "samara"},
            {"Санкт-Петербург", "spb"},
            {"Саратов", "saratov"},
            {"Севастополь", "sevastopol"},
            {"Симферополь", "simf"},
            {"Смоленск", "smolensk"},
            {"Сортавала", "sortavala"},
            {"Сосновый Бор", "sosnovybor"},
            {"Сочи", "sochi"},
            {"Ставрополь", "stavropol"},
            {"Старый Оскол", "oskol"},
            {"Стрежевой", "strezhevoy"},
            {"Таксовичкоф", "taxovichkof"},
            {"Тверь", "tver"},
            {"Тольятти", "tlt"},
            {"Томск", "tomsk"},
            {"Тула", "tula"},
            {"Ульяновск", "ul"},
            {"Феодосия", "feodosia"},
            {"Черняховск", "chernyahovsk"},
            {"Шатура и Рошаль", "shatura"},
            {"Щелково", "schelkovo"},
            {"Южноуральск", "uzhnouralsk"},
            {"Ялта", "yalta"},
        };

        public static Dictionary<string, string> ProjectsList = new Dictionary<string, string> {
            {"Dozor Lite", "lite"},
            {"Velo Dozor", "velo"}
        };

        public static string AppDisplayName = "Dozor Lite Helper";

        public static string SupportEmail = "cuckooshka.kgd@gmail.com";

        public static string DefaultPin = "1111111";

        public static string DefaultStatus = "Игра с данным PIN-кодом не найдена/неактивна.";

        internal static string GetHtmlUrl (string cityCode, string pin, string project)
        {
            return UrlBeginning (cityCode, project) + $"/go/?pin={pin}";
        }

        internal static string UrlBeginning (string cityCode, string project)
        {
            return $"http://{project}.dzzzr.ru/{cityCode}";
        }

        internal static string GetSendCodeUrl (string cityCode, string pin, string project)
        {
            return UrlBeginning (cityCode, project) + $"/go/?pin={pin}";
        }
    }
}
