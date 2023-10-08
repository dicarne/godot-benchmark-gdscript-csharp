using Godot;
using System;

public partial class another_cs : Node
{
    public override void _EnterTree()
    {
        base._EnterTree();
		ASignal += handle_int;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	[Signal]
	public delegate void ASignalEventHandler(int a);

	int b = 0;
	void handle_int(int a) {
		b = a;
	}

	public int call_from_node(int i) {
		return i;
	}
}
