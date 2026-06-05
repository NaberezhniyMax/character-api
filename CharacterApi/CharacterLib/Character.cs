using System;

namespace CharacterLib
{
    /// <summary>
    /// Клас персонажа гри. Зберігає рівень, здоров'я, енергію та досвід.
    /// </summary>
    public class Character
    {
        public const int MaxHealth = 100;
        public const int MaxEnergy = 50;
        public const int ExperiencePerLevel = 100;

        public int Level { get; private set; }
        public int Health { get; private set; }
        public int Energy { get; private set; }
        public int Experience { get; private set; }

        public Character(int level, int health, int energy, int experience)
        {
            if (level < 1)
                throw new ArgumentException("Рівень повинен бути не менше 1.", nameof(level));
            if (health < 0 || health > MaxHealth)
                throw new ArgumentException($"Здоров'я має бути в діапазоні [0, {MaxHealth}].", nameof(health));
            if (energy < 0 || energy > MaxEnergy)
                throw new ArgumentException($"Енергія має бути в діапазоні [0, {MaxEnergy}].", nameof(energy));
            if (experience < 0)
                throw new ArgumentException("Досвід не може бути від'ємним.", nameof(experience));

            Level = level;
            Health = health;
            Energy = energy;
            Experience = experience;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException("Пошкодження не може бути від'ємним.", nameof(damage));
            if (Health == 0)
                throw new InvalidOperationException("Персонаж вже мертвий.");
            Health = Math.Max(0, Health - damage);
        }

        public bool Upgrade()
        {
            if (Experience < ExperiencePerLevel)
                return false;
            Experience -= ExperiencePerLevel;
            Level++;
            Health = MaxHealth;
            Energy = MaxEnergy;
            return true;
        }

        public void UseEnergy(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Кількість енергії не може бути від'ємною.", nameof(amount));
            if (amount > Energy)
                throw new InvalidOperationException("Недостатньо енергії.");
            Energy -= amount;
        }

        public void GainExperience(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Кількість досвіду не може бути від'ємною.", nameof(amount));
            Experience += amount;
        }

        public string GenerateProfile()
        {
            string status = Health > 0 ? "Живий" : "Мертвий";
            return $"Рівень: {Level} | ХП: {Health}/{MaxHealth} | Енергія: {Energy}/{MaxEnergy} | Досвід: {Experience} | Статус: {status}";
        }

        public bool IsAlive => Health > 0;
    }
}
