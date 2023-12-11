using Godot;
using System;
using System.Collections.Generic;

public partial class cs : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public void runall()
	{
		loop_benchmark();
		dict_benchmark();
		// dict_benchmark2();
		dict_benchmark3();
		tostring_benchmark();
		dictstring_benchmark();
		dictstring_benchmark2();
		newnode_benchmark();
		func_call_benchmark();
		func_call_from_node_benchmark();
		func_call_from_cs_node_benchmark();
		func_call_from_cs_node_csstyle_benchmark();
		emit_signal_benchmark();
		node3d_position_benchmark();
		emit_signal_cs_benchmark();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	ulong loop_overhead = 0;

	void logv(string tip, double time, int count)
	{
		GetParent().Call("logv", "c#", tip, time, count);
	}

	public void loop_benchmark()
	{
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		for (var i = 0; i < 10000000; i++)
		{
			sumv += i;
		}

		var t2 = Godot.Time.GetTicksUsec();
		loop_overhead = t2 - t1;
		logv("loop", (t2 - t1) / 1000, 10000000);
	}

	public void dict_benchmark()
	{
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		var d = new Godot.Collections.Dictionary();
		for (var i = 0; i < 10000000 / 2; i++)
		{
			sumv += i;
			d[i] = i;
		}
		sum2 = 0;
		for (var i = 0; i < 10000000 / 2; i++)
		{
			sumv += i;
			sum2 = d[i].AsInt32();
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("dict(int, godot)", (t2 - t1 - loop_overhead) / 1000, 10000000);
		d.Dispose();
	}

	public void dict_benchmark2()
	{
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		var d = new Godot.Collections.Dictionary<int, int>();
		for (var i = 0; i < 10000000 / 2; i++)
		{
			sumv += i;
			d[i] = i;
		}
		sum2 = 0;
		for (var i = 0; i < 10000000 / 2; i++)
		{
			sumv += i;
			sum2 = d[i];
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("dict(int, godot with T)", (t2 - t1 - loop_overhead) / 1000, 10000000);
	}

	public void dict_benchmark3()
	{
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		var d = new Dictionary<int, int>();
		for (var i = 0; i < 10000000 / 2; i++)
		{
			sumv += i;
			d[i] = i;
		}
		sum2 = 0;
		for (var i = 0; i < 10000000 / 2; i++)
		{
			sumv += i;
			sum2 = d[i];
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("dict(int, c#)", (t2 - t1 - loop_overhead) / 1000, 10000000);
	}

	ulong tostring_overhead = 0;
	public string sumv = "";
	public void tostring_benchmark()
	{
		var t1 = Godot.Time.GetTicksUsec();
		sumv = "";
		var sumv2 = 0;
		for (var i = 0; i < 10000000; i++)
		{
			sumv = i.ToString();
			sumv2 += i;
		}

		var t2 = Godot.Time.GetTicksUsec();
		tostring_overhead = t2 - t1 - loop_overhead;
		logv("tostring", tostring_overhead / 1000, 10000000);
	}
	public int sum2 = 0;
	public void dictstring_benchmark()
	{
		var r = 0.01;
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		var d = new Godot.Collections.Dictionary();
		for (var i = 0; i < 10000000 / 2 * r; i++)
		{
			sumv += i;
			d[i.ToString()] = i;
		}
		sum2 = 0;
		for (var i = 0; i < 10000000 / 2 * r; i++)
		{
			sumv += i;
			sum2 = d[i.ToString()].AsInt32();
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("dict(string, godot)", (t2 - t1 - loop_overhead * r - tostring_overhead * r) / 1000, (int)(10000000 * r));
		d.Dispose();
	}

	public void dictstring_benchmark2()
	{
		var r = 0.01;
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		var d = new Dictionary<string, int>();
		for (var i = 0; i < 10000000 / 2 * r; i++)
		{
			sumv += i;
			d[i.ToString()] = i;
		}
		sum2 = 0;
		for (var i = 0; i < 10000000 / 2 * r; i++)
		{
			sumv += i;
			sum2 = d[i.ToString()];
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("dict(string, c#)", (t2 - t1 - loop_overhead * r - tostring_overhead * r) / 1000, (int)(10000000 * r));
	}

	public void newnode_benchmark()
	{
		var r = 10000.0 / 10000000;
		var t1 = Godot.Time.GetTicksUsec();
		sum2 = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sum2 += i;
			AddChild(new Godot.Node());
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("new and add node", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}

	int _inner_func(int i)
	{
		return 1;
	}
	public void func_call_benchmark()
	{
		var r = 1.0;
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		sum2 = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sumv += i;
			sum2 = _inner_func(i);
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("func call", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}

	public void func_call_from_node_benchmark()
	{
		var r = 1.0;
		var n = GetParent();
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		sum2 = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sumv += i;
			sum2 = n.Call("call_from_node", i).AsInt32();
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("func call (from parent node)", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}

	public void func_call_from_cs_node_benchmark()
	{
		var r = 1.0;
		var n = GetParent().GetNode("cs2");
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		sum2 = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sumv += i;
			sum2 = n.Call("call_from_node", i).AsInt32();
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("func call (to cs node call by string)", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}

	public void func_call_from_cs_node_csstyle_benchmark()
	{
		var r = 1.0;
		var n = GetParent().GetNode("cs2") as another_cs;
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		sum2 = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sumv += i;
			sum2 = n.call_from_node(i);
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("func call (to cs node)", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}

	public void emit_signal_benchmark()
	{
		var r = 1.0;
		var n = GetParent();
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sumv += i;
			n.EmitSignal("a_signal_1", i);
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("emit signal (to gd node)", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}

	public void emit_signal_cs_benchmark()
	{
		var t = 10000000;
		var r = t / 10000000.0;
		var n = GetParent().GetNode("cs2");
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		for (var i = 0; i < t; i++)
		{
			sumv += i;
			n.EmitSignal("ASignal", i);
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("emit signal (to cs node)", (t2 - t1 - loop_overhead * r) / 1000, t);
	}

	public int large_obj_banchmark(Godot.Collections.Dictionary a)
	{
		int sumv = 0;
		for (int i = 0; i < 1000; i++)
		{
			var t1 = a[i].AsGodotDictionary();
			for (int j = 0; j < 100; j++)
			{
				var t2 = t1[j].AsGodotDictionary();
				for (int k = 0; k < 10; k++)
				{
					sumv += t2[k].AsInt32();
				}
			}
		}
		return sumv;
	}

	public string large_string_banchmark(string v)
	{
		return v;
	}

	public void node3d_position_benchmark()
	{
		var r = 1.0;
		var n = GetParent().GetNode("Node3D") as Godot.Node3D;
		var t1 = Godot.Time.GetTicksUsec();
		var sumv = 0;
		for (var i = 0; i < 10000000 * r; i++)
		{
			sumv += i;
			n.Position = new Vector3(i, i, i);
		}

		var t2 = Godot.Time.GetTicksUsec();
		logv("node3d position", (t2 - t1 - loop_overhead * r) / 1000, (int)(10000000 * r));
	}
}
