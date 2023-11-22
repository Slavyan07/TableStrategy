using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TableStrategy
{
	/// <summary> Игровая сторона </summary>
	public enum GameSide
	{ 
		LEFT,
		RIGHT,
	}
	/// <summary> Состояние юнита </summary>
	public enum UnitState
	{ 
		STAY,
		ATTACK,
		DAMAGE,
		DEATH,
	}
	[DataContract]
	/// <summary> Информация об игровом юните </summary>
	public struct GameUnitInfo
	{
		[DataMember]
		public string Name { get; private set; }
		[DataMember]
		public int Health { get; set; }
		[DataMember]
		public int Range { get; set; }
		[DataMember]
		public int Speed { get; set; }

		[DataMember]
		public int Defend { get; set; }
		[DataMember]
		public int Attack { get; set; }
		[DataMember]
		public int CloseAttack { get; set; }

		[DataMember]
		public int Price { get; set; }
		[DataMember]
		private string[] _sprites;

		public GameUnitInfo(string name, int price, int attack, int closeAttack, int health, int defend, int range, int speed,  string spritesFilePath)
		{ 
			Name = name;

			Attack = attack;
			CloseAttack = closeAttack;

			Health = health;
			Defend = defend;

			Range = range;
			Speed = speed;

			Price = price;

			_sprites = new string[4]
			{
				Environment.CurrentDirectory + $"\\Resources\\{spritesFilePath}\\Stay.png",
				Environment.CurrentDirectory + $"\\Resources\\{spritesFilePath}\\Attack.png",
				Environment.CurrentDirectory + $"\\Resources\\{spritesFilePath}\\Damage.png",
				Environment.CurrentDirectory + $"\\Resources\\{spritesFilePath}\\Death.png",
			};
		}

		public string GetSprite(UnitState state) => _sprites[(int)state];
	}

	[DataContract]
	/// <summary> Игровой юнит </summary>
	public class GameUnit : GameEntity
	{
		[DataMember]
		private GridTransform _transform;

		[DataMember]
		public bool CanMove { get; private set; }
		[DataMember]
		public bool CanAttack { get; private set; }

		[DataMember]
		public GameUnitInfo Info { get; private set; }
		[DataMember]
		public GameSide GameSide { get; private set; }
		[DataMember]
		public UnitState CurrentState { get; private set; }
		public GridTransform UnitTransform => _transform;

		[DataMember]
		public int DefendBonus { get; set; }

		public string HealthString => $"{CurrentHealth}/{Info.Health}";

		public GameUnit(GameUnitInfo unitInfo, GameSide side) : base(unitInfo.Health)
		{
			CurrentState = UnitState.STAY;
			Info = unitInfo;
			GameSide = side;
			UnitRestart();

			DefendBonus = 0;
		}

		public void NewTransform(GridPoint position)
		{ 
			_transform = new GridTransform(position);
		}
		public void Attack(GameEntity gameEntity)
		{
			if (!CanAttack)
				throw new Exception("Данный персонаж не может атаковать");

			CurrentState = UnitState.ATTACK;
			gameEntity.Damage(Info.Attack);
			CanAttack = false;
		}
		public override void Damage(int damage)
		{
			CurrentState = UnitState.DAMAGE;
			damage = damage - DefendBonus < 0 ? 0 : damage - DefendBonus;
			base.Damage(damage);
		}

		public override void Death()
		{
			base.Death();
			CurrentState = UnitState.DEATH;
		}

		public void UnitRestart()
		{
			CanMove = true;
			CanAttack = true;
		}
		public void NullState()
		{
			if (!Alive)
				return;

			CurrentState = UnitState.STAY;
		}
		public void Disable()
		{
			CanMove = false;
			CanAttack = false;
		}
		public void DisableMove() => CanMove = false;
		public void DisableAttack() => CanAttack = false;
		public string GetRenderImage() => Info.GetSprite(CurrentState);
		public string GetRenderImage(UnitState state) => Info.GetSprite(state);

		public void Move(GridPoint position)
		{
			if (!CanMove)
				throw new Exception("Данный персонаж не может передвигаться");

			List<GridPoint> canMove = _transform.CellAroundCircle(Info.Speed);
			if (!canMove.Any(c => c == position))
				throw new Exception("Персонаж не может туда сходить");

			_transform.SetPosition(position);
			CanMove = false;
		}
	}
}
