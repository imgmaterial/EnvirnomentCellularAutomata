using Godot;
using System;
using System.Threading.Tasks;

public partial class Terrain : Node3D
{
	// public when Che {get;set;}ode enters the scene tree for the first time.
	private PackedScene cellScene;
	private Node3D[,] terrainGrid;
	private Godot.Timer timer;
	private int weatherOffset = 0;
	[Export] public Noise TerrainNoise { get; set; }
	[Export] public Noise BiomNoise { get; set; }
	
	[Export] public Vector2I TerrainSize = new Vector2I(64, 64);
	[Export] public StandardMaterial3D Desert { get; set; }
	[Export] public StandardMaterial3D Continental { get; set; }
	[Export] public StandardMaterial3D Polar { get; set; }
	public override void _Ready()
	{
		terrainGrid = new Node3D[TerrainSize.Y, TerrainSize.X];
		timer = this.GetParent().GetNode<Godot.Timer>("TickTimer");
		timer.Timeout += OnTickTimerTimeout;
		GenerateTerrainGeometry();
		GenerateBioms();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void GenerateTerrainGeometry()
	{
		float x = 0;
		float y = 0;
		Vector3 position = Vector3.Zero;
		Random random = new Random();
		cellScene = GD.Load<PackedScene>("res://cell.tscn");
		for (int i = 0; i < TerrainSize.Y; i++)
		{
			for (int j = 0; j < TerrainSize.X; j++)
			{
				Node3D instance = cellScene.Instantiate<Node3D>();
				terrainGrid[i, j] = instance;
				AddChild(instance);
				instance.GlobalPosition = position;
				position.X += 2*(float)Math.Sqrt(3)/2;
				position.Y = 10*(TerrainNoise.GetNoise2D(x,y)+1);
				x += 1;
				
			}
			x = 10;
			y += 1;
			position.X = i%2==0?0.866f:0;
			position.Z += 1.5f;
		}
	}

	private void GenerateBioms()
	{
		for (int i = 0; i < TerrainSize.Y; i++)
		{
			for (int j = 0; j < TerrainSize.X; j++)
			{
				Cell cell = terrainGrid[i, j] as Cell;
				float biomValue = BiomNoise.GetNoise2D(i, j+weatherOffset);
				if (biomValue < 0.3)
				{
					cell.SetBiome(BiomeEnum.Desert,this);
				}
				else if(biomValue < 0.7)
				{
					cell.SetBiome(BiomeEnum.Continental,this);
				}
				else if (biomValue < 1)
				{
					cell.SetBiome(BiomeEnum.Polar,this);
				}
			}
		}
	}
	

	public void OnTickTimerTimeout()
	{
		weatherOffset++;
		GenerateBioms();
		GD.Print("Tick!");
	}


}
