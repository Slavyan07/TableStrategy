using System;
using System.Runtime.Serialization;

namespace TableStrategy
{
	[DataContract]
	/// <summary> Живое существо </summary>
	public class GameEntity
	{
		[DataMember]
		private int _currentHealth;
		[DataMember]
		private int _maxHealth;

		[DataMember]
		public bool Alive { get; private set; }
		public int CurrentHealth => _currentHealth;
		public int MaxHealth => _maxHealth;

		/// <summary> Событие при смерти </summary>
		public Action? OnDeath { get; set; }

		public GameEntity(int health)
		{
			_maxHealth = health;
			_currentHealth = health;

			Alive = true;
		}

		public virtual void Damage(int damage) 
		{
			if (!Alive)
				return;

			_currentHealth -= damage;

			if (_currentHealth <= 0)
				Death();
		}

		public virtual void Treatment(int value)
		{
			if (!Alive)
				return;

			_currentHealth += value;
		}

		public virtual void Death()
		{
			if (!Alive)
				return;

			_currentHealth = 0;
			Alive = false;
			OnDeath?.Invoke();
		}
	}
}
