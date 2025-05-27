using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator
{
    // 1. Інтерфейс героя
    interface IHero
    {
        string GetDescription();
        int GetPower();
    }

    // 2. Герої
    class Warrior : IHero
    {
        public string GetDescription() => "Warrior";
        public int GetPower() => 10;
    }

    class Mage : IHero
    {
        public string GetDescription() => "Mage";
        public int GetPower() => 8;
    }

    class Paladin : IHero
    {
        public string GetDescription() => "Paladin";
        public int GetPower() => 12;
    }

    // 3. Базовий декоратор
    abstract class HeroDecorator : IHero
    {
        protected IHero _hero;

        public HeroDecorator(IHero hero)
        {
            _hero = hero;
        }

        public virtual string GetDescription() => _hero.GetDescription();

        public virtual int GetPower() => _hero.GetPower();
    }

    // 4. Конкретні декоратори - інвентар

    class Armor : HeroDecorator
    {
        public Armor(IHero hero) : base(hero) { }

        public override string GetDescription() => _hero.GetDescription() + ", wearing Armor";

        public override int GetPower() => _hero.GetPower() + 5; // броня дає +5 до сили
    }

    class Weapon : HeroDecorator
    {
        public Weapon(IHero hero) : base(hero) { }

        public override string GetDescription() => _hero.GetDescription() + ", equipped with Weapon";

        public override int GetPower() => _hero.GetPower() + 7; // зброя дає +7 до сили
    }

    class Artifact : HeroDecorator
    {
        public Artifact(IHero hero) : base(hero) { }

        public override string GetDescription() => _hero.GetDescription() + ", carrying Artifact";

        public override int GetPower() => _hero.GetPower() + 10; // артефакт дає +10 до сили
    }

    // 5. Демонстрація
    class Program
    {
        static void Main()
        {
            // Герой — воїн без інвентаря
            IHero warrior = new Warrior();
            Console.WriteLine($"{warrior.GetDescription()} has power {warrior.GetPower()}");

            // Воїн із бронею
            IHero armoredWarrior = new Armor(warrior);
            Console.WriteLine($"{armoredWarrior.GetDescription()} has power {armoredWarrior.GetPower()}");

            // Воїн із бронею та зброєю
            IHero armedArmoredWarrior = new Weapon(armoredWarrior);
            Console.WriteLine($"{armedArmoredWarrior.GetDescription()} has power {armedArmoredWarrior.GetPower()}");

            // Воїн із бронею, зброєю та артефактом
            IHero fullyEquippedWarrior = new Artifact(armedArmoredWarrior);
            Console.WriteLine($"{fullyEquippedWarrior.GetDescription()} has power {fullyEquippedWarrior.GetPower()}");

            // Аналогічно з магом
            IHero mage = new Mage();
            IHero mageWithArtifact = new Artifact(mage);
            Console.WriteLine($"{mageWithArtifact.GetDescription()} has power {mageWithArtifact.GetPower()}");

            // Паладін із двома артефактами
            IHero paladin = new Paladin();
            IHero paladinWithArtifacts = new Artifact(new Artifact(paladin));
            Console.WriteLine($"{paladinWithArtifacts.GetDescription()} has power {paladinWithArtifacts.GetPower()}");
        }
    }
}
