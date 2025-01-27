using Godot;
using System;

public partial class Cell : Node3D
{
	public BiomeEnum Biome { get; set; }
	public int HydrationLevel { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetBiome(BiomeEnum biome,Terrain terrain)
	{
		if (this.Biome == biome)
		{
			return;
		}
		switch (biome)
		{
			case BiomeEnum.Desert:
				this.GetChild<MeshInstance3D>(0).MaterialOverride = terrain.Desert;
				break;
			case BiomeEnum.Continental:
				this.GetChild<MeshInstance3D>(0).MaterialOverride = terrain.Continental;
				break;
			case BiomeEnum.Polar:
				this.GetChild<MeshInstance3D>(0).MaterialOverride = terrain.Polar;
				break;
		}
	}
}
