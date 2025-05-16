using System.Threading;
using System.Threading.Tasks;
using Sandbox;

namespace Glide.Player
{
	public sealed class PlayerMovement : Component
	{
		[Property, Group( "Movement Forces" )] public float WalkSpeed { get; set; } = 200f;

		[Property, Group( "Movement Forces" )] public float RunSpeed { get; set; } = 400f;
		[Property, Group( "Movement Forces" )] public float JumpForce { get; set; } = 300f;

		[Property, Group( "Cooldowns" ), Description( "Jump cooldown in ms" )]
		public int JumpCooldown { get; set; } = 350;

		private bool _isGrounded = false;
		private Vector3 _inputDir;
		private float _peed;
		private bool _jumpAllowed = true;

		protected override void OnStart()
		{
			// foreach(string s in Input.ActionNames)
			// {
			// 	Log.Info(s);
			// }
		}

		protected override void OnUpdate()
		{
			HandleInputs();
			HandleMovement();
		}

		private void HandleMovement()
		{
			if ( _isGrounded && _jumpAllowed && Input.Down( "Jump" ) )
			{
				_jumpAllowed = false;
				new Thread( () =>
				{
					Thread.Sleep( JumpCooldown );
				} );
			}
		}

		private void HandleInputs()
		{
			_isGrounded = Scene.Trace.Ray( new Ray( LocalPosition, Vector3.Down ), 13f ).Run().Hit;
			_inputDir = Input.AnalogMove;
			_peed = Input.Down( "Run" ) ? RunSpeed : WalkSpeed;
		}
	}
}
