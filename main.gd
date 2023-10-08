extends Control


# Called when the node enters the scene tree for the first time.
func _init():
	a_signal_1.connect(signal_from_node)

func _ready():
	large_object_benchmark()
	await get_tree().process_frame
	$gds.runall()
	await get_tree().process_frame
	$cs.runall()
	await get_tree().process_frame
	finally()
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func call_from_node(v):
	return v

var b = 0
func signal_from_node(v):
	b = v

signal a_signal_1

var sheet = ""

func logv(type, tip, time, count):
	print("%s %s: %f ms (%d)" % [type, tip, time, int(count)])
	sheet += "\"%s\",\"%s\",%f,%d\n" % [type, tip, time, int(count)]

func finally():
	print("all over")
	print(sheet)
	var f = FileAccess.open("user://benchmark.csv", FileAccess.WRITE)
	f.store_string(sheet)

func large_object_benchmark():
	var a = {}
	for i in range(1000):
		var t1 = {}
		a[i] = t1
		for j in range(100):
			var t2 = {}
			t1[j] = t2
			for k in range(10):
				t2[k] = k
	var strv = JSON.stringify(a)
	var n1 = $gds
	var n2 = $cs
	var t1 = Time.get_ticks_usec()
	n1.large_obj_banchmark(a)
	var t2 = Time.get_ticks_usec()
	logv("gds", "large object foreach", (t2-t1)/1000, 1)
	
	t1 = Time.get_ticks_usec()
	n1.large_string_banchmark(strv)
	t2 = Time.get_ticks_usec()
	logv("gds", "large string passby", (t2-t1)/1000, 1)
	
	t1 = Time.get_ticks_usec()
	n2.large_obj_banchmark(a)
	t2 = Time.get_ticks_usec()
	logv("c#", "large object foreach", (t2-t1)/1000, 1)
	
	t1 = Time.get_ticks_usec()
	n2.large_string_banchmark(strv)
	t2 = Time.get_ticks_usec()
	logv("c#", "large string passby", (t2-t1)/1000, 1)
