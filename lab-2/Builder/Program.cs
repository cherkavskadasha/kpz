using System;
using System.Collections.Generic;

namespace Builder
{
    public class Character
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public string BodyType { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string Clothing { get; set; }
        public List<string> Inventory { get; set; } = new List<string>();

        public void ShowInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Height: {Height} cm");
            Console.WriteLine($"Body Type: {BodyType}");
            Console.WriteLine($"Hair Color: {HairColor}");
            Console.WriteLine($"Eye Color: {EyeColor}");
            Console.WriteLine($"Clothing: {Clothing}");
            Console.WriteLine("Inventory: " + (Inventory.Count > 0 ? string.Join(", ", Inventory) : "None"));
            Console.WriteLine();
        }
    }

    public interface ICharacterBuilder
    {
        ICharacterBuilder SetName(string name);
        ICharacterBuilder SetHeight(int height);
        ICharacterBuilder SetBodyType(string bodyType);
        ICharacterBuilder SetHairColor(string hairColor);
        ICharacterBuilder SetEyeColor(string eyeColor);
        ICharacterBuilder SetClothing(string clothing);
        ICharacterBuilder AddToInventory(string item);
        Character Build();
    }

    public class HeroBuilder : ICharacterBuilder
    {
        private Character _character = new Character();

        public ICharacterBuilder SetName(string name) { _character.Name = name; return this; }
        public ICharacterBuilder SetHeight(int height) { _character.Height = height; return this; }
        public ICharacterBuilder SetBodyType(string bodyType) { _character.BodyType = bodyType; return this; }
        public ICharacterBuilder SetHairColor(string hairColor) { _character.HairColor = hairColor; return this; }
        public ICharacterBuilder SetEyeColor(string eyeColor) { _character.EyeColor = eyeColor; return this; }
        public ICharacterBuilder SetClothing(string clothing) { _character.Clothing = clothing; return this; }
        public ICharacterBuilder AddToInventory(string item) { _character.Inventory.Add(item); return this; }
        public Character Build() => _character;
    }

    public class EnemyBuilder : ICharacterBuilder
    {
        private Character _character = new Character();

        public ICharacterBuilder SetName(string name) { _character.Name = name; return this; }
        public ICharacterBuilder SetHeight(int height) { _character.Height = height; return this; }
        public ICharacterBuilder SetBodyType(string bodyType) { _character.BodyType = bodyType; return this; }
        public ICharacterBuilder SetHairColor(string hairColor) { _character.HairColor = hairColor; return this; }
        public ICharacterBuilder SetEyeColor(string eyeColor) { _character.EyeColor = eyeColor; return this; }
        public ICharacterBuilder SetClothing(string clothing) { _character.Clothing = clothing; return this; }
        public ICharacterBuilder AddToInventory(string item) { _character.Inventory.Add(item); return this; }
        public Character Build() => _character;
    }

    public class CharacterDirector
    {
        public Character CreateHero(HeroBuilder builder)
        {
            return builder
                .SetName("Artemis")
                .SetHeight(180)
                .SetBodyType("Athletic")
                .SetHairColor("Blonde")
                .SetEyeColor("Green")
                .SetClothing("Golden Armor")
                .AddToInventory("Sword")
                .AddToInventory("Healing Potion")
                .Build();
        }

        public Character CreateEnemy(EnemyBuilder builder)
        {
            return builder
                .SetName("Darklord")
                .SetHeight(200)
                .SetBodyType("Muscular")
                .SetHairColor("Black")
                .SetEyeColor("Red")
                .SetClothing("Dark Cloak")
                .AddToInventory("Cursed Staff")
                .Build();
        }
    }

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Лабораторна робота 2, Завдання 5\nВиконала Черкавська Д.В., група ВТ-23-2\n");

            var director = new CharacterDirector();

            var heroBuilder = new HeroBuilder();
            var hero = director.CreateHero(heroBuilder);
            Console.WriteLine("Hero created:");
            hero.ShowInfo();

            var enemyBuilder = new EnemyBuilder();
            var enemy = director.CreateEnemy(enemyBuilder);
            Console.WriteLine("Enemy created:");
            enemy.ShowInfo();
        }
    }
}
