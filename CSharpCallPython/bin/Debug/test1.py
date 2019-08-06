def mainF(arr):
	try:
		arr = set(arr)
		arr = sorted(arr)
		arr = arr[0:3]
		return str(arr)+":this code come from python"
	except Exception as err:
		return str(err)
        
        
def fun(arr):
	try:
		import sys
		return "this code come from python"
	except Exception as err:
		return str(err)