using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vocabulary;
using System.Xml.Linq;
using System.Security.Policy;
using System.Xml;
using System.Threading;

namespace Vocabulary
{
    public class Word
    {
        public string word { get; set; }
        public string translate { get; set; }
        public string translates { get; set; } = "";
        public Word() { }
        public Word(string word, string translate)
        {
            this.word = word;
            this.translate = translate;
        }
        public void AddTr(string temp) { translates += (temp + " "); }
    }

    public interface IVocabulary
    {
        void Start();
        void Menu();
        void Add_Start();
        void Add();
        void Change();
        void Search();
        void Delete();
        void Export();

    }
    public class Words : IVocabulary
    {
        private string type;
        public void Start()
        {
            Console.WriteLine("1. Создать файл\n2. Открыть сохраненный файл");
            int choose = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            try
            {
                if (choose == 1)
                {
                    Console.WriteLine("1. Рус-англ\n2. Англ-рус");
                    int choose2 = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    if (choose2 == 1) 
                    {
                        type = "../../Rus.xml";
                        FileStream stream = new FileStream(type, FileMode.Create); stream.Close();
                        this.Add_Start();
                    }
                    else if (choose2 == 2)
                    {
                        type = "../../Eng.xml";
                        FileStream stream = new FileStream(type, FileMode.Create); stream.Close();
                        this.Add_Start();
                    }
                    else
                        throw new Exception("Введен неверное значение");
                }
                else if (choose == 2)
                {
                    Console.WriteLine("1. Рус-англ\n2. Англ-рус");
                    choose = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    if (choose == 1) { type = "../../Rus.xml"; this.Menu(); }
                    else if (choose == 2) { type = "../../Eng.xml"; this.Menu(); }
                    else
                        throw new Exception("Введен неверное значение");
                }
                else
                    throw new Exception("Введен неверное значение");

            }
            catch (Exception e) { Console.WriteLine(e.Message); this.Start(); }
        }
        public void Menu()
        {
            Console.Clear();
            Console.WriteLine("1. Добавить слово\n2. Удалить слово/перевод\n3. Поиск\n4. Экспорт\n5. Изменить слов/перевод\n0. Выход");
            int choose = Convert.ToInt32(Console.ReadLine());
            switch (choose)
            {
                case 1: this.Add(); break;
                case 2: this.Delete(); break;
                case 3: this.Search(); break;
                case 4: this.Export(); break;
                case 5: this.Change(); break;
                case 0: break;
                default: this.Menu(); break;
            }
        }
        public void Add()
        {
            Console.Clear();
            Console.Write("Введите слово: ");
            string temp_1 = Console.ReadLine();
            Console.Clear();
            Console.Write("Введите перевод: ");
            string temp_2 = Console.ReadLine();
            Console.Clear();
            Word word = new Word(temp_1, temp_2);
            Console.WriteLine("1. Добавить ещё перевод\n2. Пропустить");
            int choose = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            while (choose == 1)
            {
                Console.Write("Введите перевод: ");
                temp_2 = Console.ReadLine();
                word.AddTr(temp_2);
                Console.Clear();
                Console.WriteLine("1. Добавить ещё перевод\n2. Пропустить");
                choose = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
            XDocument x = XDocument.Load(type);
            x.Root.Add(new XElement("Word",
                new XAttribute("word", word.word),
                new XAttribute("translate", word.translate),
                new XElement("translates", word.translates)));
            x.Save(type);
            this.Menu();
        }
        public void Add_Start()
        {
            Console.Clear();
            Console.Write("Введите слово: ");
            string temp_1 = Console.ReadLine();
            Console.Clear();
            Console.Write("Введите перевод: ");
            string temp_2 = Console.ReadLine();
            Console.Clear();
            Word word = new Word(temp_1, temp_2);
            Console.WriteLine("1. Добавить ещё перевод\n2. Пропустить");
            int choose = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            while (choose == 1)
            {
                Console.Write("Введите перевод: ");
                temp_2 = Console.ReadLine();
                word.AddTr(temp_2);
                Console.Clear();
                Console.WriteLine("1. Добавить ещё перевод\n2. Пропустить");
                choose = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
            XDocument doc = new XDocument(new XElement("Words",
            new XElement("Word",
                new XAttribute("word", word.word),
                new XAttribute("translate", word.translate),
                new XElement("translates", word.translates))));
            doc.Save(type);
            this.Menu();
        }
        public void Change()
        {
            Console.Clear();
            Console.WriteLine("1. Заменить слово\n2. Заменить перевод");
            int choose = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.Write("Введите слово: ");
            string search = Console.ReadLine();
            Console.Clear();
            Console.Write("Введите новое слово: ");
            string change = Console.ReadLine();
            Console.Clear();
            if (choose == 1)
            {
                XDocument x = XDocument.Load(type);
                XElement e = x.Root;
                var tom = x.Element("Words")?
                    .Elements("Word")
                    .FirstOrDefault(p => p.Attribute("word")?.Value == search);
                if (tom != null)
                {
                    var word = tom.Attribute("word");
                    if (word != null) word.Value = change;
                    x.Save(type);
                    Console.WriteLine("Слово было успешно заменено");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                XDocument x = XDocument.Load(type);
                XElement e = x.Root;
                var tom = x.Element("Words")?
                    .Elements("Word")
                    .FirstOrDefault(p => p.Attribute("translate")?.Value == search);
                if (tom != null)
                {
                    var translate = tom.Attribute("translate");
                    if (translate != null) translate.Value = change;
                    x.Save(type);
                    Console.WriteLine("Слово было успешно заменено");
                    Thread.Sleep(2000);
                }
            }
            this.Menu();
        }
        public void Search()
        {
            Console.Clear();
            Console.Write("Введите слово: ");
            string search = Console.ReadLine();
            Console.Clear();
            XDocument x = XDocument.Load(type);
            XElement e = x.Root;
            var tom = x.Element("Words")?
                .Elements("Word")
                .FirstOrDefault(p => p.Attribute("word")?.Value == search);
            if (tom != null)
            {
                var translate = tom.Attribute("translate");
                var translates = tom.Element("translates");
                Console.WriteLine("Перевод: " + translate.Value + " " + translates.Value);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Слово небыло найдено");
                Console.ReadLine();
            }
            this.Menu();
        }
        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("1. Удалить слово\n2. Удалить перевод");
            int choose = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.Write("Введите слово: ");
            string search = Console.ReadLine();
            Console.Clear();
            if (choose == 1)
            {
                XDocument x = XDocument.Load(type);
                XElement e = x.Root;
                if (e != null)
                {
                    var tom = x.Element("Words")?
                    .Elements("Word")
                    .FirstOrDefault(p => p.Attribute("word")?.Value == search);
                    if (tom != null)
                    {
                        tom.Remove();
                        x.Save(type);
                        Console.WriteLine("Слово было успешно удалено");
                        Thread.Sleep(2000);
                    }
                }
            }
            else
            {
                XDocument x = XDocument.Load(type);
                XElement e = x.Root;
                if (e != null)
                {
                    var tom = x.Element("Words")?
                    .Elements("Word")
                    .FirstOrDefault(p => p.Attribute("word")?.Value == search);
                    if (tom != null)
                    {
                        var transl = tom.Element("translates").Value;
                        if (transl == "" || transl == null)
                            Console.Write("Невозможно удалить, так как нет других переводов для слова");
                        else
                        {
                            string[] temp = tom.Element("translates").Value.Split(' ');
                            List<string> list = new List<string>(temp);
                            var translate = tom.Attribute("translate");
                            if (translate != null) translate.Value = list[0];
                            list.RemoveAt(0);
                            string temp_2 = "";
                            if(list.Count !=0)
                            {
                                for (int i = 0; i < list.Count; i++)
                                    temp_2 += list[i] + " ";
                                temp_2 = temp_2.Remove(temp_2.Length - 1);
                                tom.Element("translates").Value = temp_2;
                            }
                            Console.WriteLine("Перевод было успешно заменено");
                            Thread.Sleep(2000);
                        }
                        
                    }
                }
                x.Save(type);
            }
            this.Menu();
        }
        public void Export()
        {
            Console.Clear();
            Console.Write("Введите слово: ");
            string temp = Console.ReadLine();
            Console.Clear();
            XDocument x = XDocument.Load(type);
            XElement e = x.Root;
            var search = x.Element("Words")?
                .Elements("Word")?
                .FirstOrDefault(p => p.Attribute("word")?.Value == temp);
            string word = search?.Attribute("word")?.Value;
            string translate = search?.Attribute("translate")?.Value;
            string translates = search?.Element("translates")?.Value;
            FileStream stream = new FileStream("../../result.xml", FileMode.Create); stream.Close();
            XDocument doc = new XDocument(new XElement("Words",
           new XElement("Word",
               new XAttribute("word", word),
               new XAttribute("translate", translate),
               new XElement("translates", translates))));
            doc.Save("../../result.xml");
            Console.WriteLine("Слово было успешно экспортировано");
            Thread.Sleep(2000);
            this.Menu();
        }
    }
}
namespace NET_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Words words = new Words();
            words.Start();
        }
    }
}
