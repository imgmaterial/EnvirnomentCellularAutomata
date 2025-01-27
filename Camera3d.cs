using Godot;
using System;

public partial class Camera3d : Camera3D
{
	[Export] public float MoveSpeed = 5.0f;
	[Export] public float MouseSensitivity = 5.0f;
    
	private float _rotationX = 0.0f;
	private float _rotationY = 0.0f;
	private Vector2 _mouseDelta = Vector2.Zero;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion && 
		    Input.IsMouseButtonPressed(MouseButton.Right))
		{
			_mouseDelta = mouseMotion.Relative;
		}
	}

	public override void _Process(double delta)
	{
		float deltaFloat = (float)delta;

		// Handle mouse rotation with delta
		if (Input.IsMouseButtonPressed(MouseButton.Right))
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
            
			// Apply rotation using accumulated mouse delta and delta time
			_rotationY -= _mouseDelta.X * MouseSensitivity * deltaFloat;
			_rotationX -= _mouseDelta.Y * MouseSensitivity * deltaFloat;
			_rotationX = Mathf.Clamp(_rotationX, -Mathf.Pi / 2, Mathf.Pi / 2);
            
			Rotation = new Vector3(_rotationX, _rotationY, 0);
            
			// Reset mouse delta
			_mouseDelta = Vector2.Zero;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		
		if (Input.IsActionPressed("move_up"))
			Position += Vector3.Up * MoveSpeed * (float)delta;
    
		if (Input.IsActionPressed("move_down"))
			Position += Vector3.Down * MoveSpeed * (float)delta;

		// WASD Movement with delta
		Vector3 inputDirection = Vector3.Zero;
		inputDirection.X = Input.GetAxis("move_left", "move_right");
		inputDirection.Z = Input.GetAxis("move_forward", "move_back");
        
		Vector3 direction = (GlobalTransform.Basis * inputDirection).Normalized();
		Position += direction * MoveSpeed * deltaFloat;
	}
}