def main(arr):
	try:
		arr = set(arr)
		arr = sorted(arr)
		arr = arr[0:2]
		return str(arr)+":this code come from python"
	except Exception as err:
		return str(err)
        
        
def fun(arr):
	try:
		import platform
		return str(platform.architecture())+":this code come from python1"
	except Exception as err:
		return str(err)