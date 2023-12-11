extends Node

# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	

func runall():
	loop_benchmark()
	dict_benchmark()
	tostring_benchmark()
	dictstring_benchmark()
	newnode_benchmark()
	func_call_benchmark()
	func_call_from_node_benchmark()
	func_call_from_cs_node_benchmark()
	emit_signal_benchmark()
	emit_signal_cs_benchmark()
	node3d_position_benchmark()

var loop_overhead = 0
func loop_benchmark():
	var t1 = Time.get_ticks_usec()
	var sumv = 0
	for i in range(10000000):
		sumv += i
	var t2 = Time.get_ticks_usec()
	loop_overhead = t2 - t1
	get_parent().logv("gds","loop",(t2-t1)/1000,10000000)

func dict_benchmark():
	var t1 = Time.get_ticks_usec()
	var sumv = 0
	var dict = {}
	for i in range(10000000/2):
		sumv += i
		dict[i] = i
	var sum2 = 0
	for i in range(10000000/2):
		sumv += i
		sum2 = dict[i]
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"dict(int)", (t2-t1-loop_overhead)/1000, 10000000)

var tostring_overhead
func tostring_benchmark():
	var t1 = Time.get_ticks_usec()
	var sumv = ""
	var sum = 0
	for i in range(10000000):
		sum += i
		sumv = str(i)
	var t2 = Time.get_ticks_usec()
	tostring_overhead = t2 - t1 -loop_overhead
	get_parent().logv("gds" ,"tostring", (t2-t1-loop_overhead)/1000, 10000000)

func dictstring_benchmark():
	var r = 0.01
	var t1 = Time.get_ticks_usec()
	var sumv = 0
	var dict = {}
	for i in range(10000000/2*r):
		sumv += i
		dict[str(i)] = i
	var sum2 = 0
	for i in range(10000000/2*r):
		sumv += i
		sum2 = dict[str(i)]
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"dict(string)", (t2-t1-loop_overhead*r-tostring_overhead*r)/1000, int(10000000*r))

func newnode_benchmark():
	var r = 10000.0/10000000
	var t1 = Time.get_ticks_usec()
	var sum = 0
	for i in range(10000000*r):
		sum += i
		add_child(Node.new())
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"new and add node", (t2-t1-loop_overhead*r)/1000, int(10000000*r))

func _inner_func(v):
	return 1

func func_call_benchmark():
	var t1 = Time.get_ticks_usec()
	var sum = 0
	var sum2 = 0
	for i in range(10000000):
		sum += i
		sum2 = _inner_func(i)
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"func call", (t2-t1-loop_overhead)/1000, 10000000)

func func_call_from_node_benchmark():
	var n = get_parent()
	var t1 = Time.get_ticks_usec()
	var sum = 0
	var sum2 = 0
	for i in range(10000000):
		sum += i
		sum2 = n.call_from_node(i)
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"func call (from parent node)", (t2-t1-loop_overhead)/1000, 10000000)

func func_call_from_cs_node_benchmark():
	var n = get_parent().get_node("cs2")
	var t1 = Time.get_ticks_usec()
	var sum = 0
	var sum2 = 0
	for i in range(10000000):
		sum += i
		sum2 = n.call_from_node(i)
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"func call (to cs node)", (t2-t1-loop_overhead)/1000, 10000000)

func emit_signal_benchmark():
	var n = get_parent()
	var t1 = Time.get_ticks_usec()
	var sum = 0
	for i in range(10000000):
		sum += i
		n.a_signal_1.emit(i)
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"emit signal (to gd node)", (t2-t1-loop_overhead)/1000, 10000000)

func emit_signal_cs_benchmark():
	var t = 10000000
	var r = t / 10000000.0
	var n = get_parent().get_node("cs2")
	assert(n.has_signal("ASignal"), "CS node dont have signal")
	var t1 = Time.get_ticks_usec()
	var sum = 0
	for i in range(t):
		sum += i
		n.emit_signal("ASignal", i)
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"emit signal (to cs node)", (t2-t1-loop_overhead*r)/1000, 8)

func large_obj_banchmark(a):
	var sumv = 0
	for i in range(1000):
		var t1 = a[i]
		for j in range(100):
			var t2 = t1[j]
			for k in range(10):
				sumv += t2[k]
	return sumv

func large_string_banchmark(a):
	return a

func node3d_position_benchmark():
	var n = get_parent().get_node("Node3D")
	var t1 = Time.get_ticks_usec()
	var sum = 0
	for i in range(10000000):
		sum += i
		n.position = Vector3(i, i, i)
	var t2 = Time.get_ticks_usec()
	get_parent().logv("gds" ,"node3d position", (t2-t1-loop_overhead)/1000, 10000000)
