import sys

def add(a,b):
    return a+b

if __name__ == "__main__":
    print(sys.path)
    print(sys.argv[1])
    print(sys.argv[2])
    print(add(len(sys.argv[1]),len(sys.argv[2])))
    print("hello python")